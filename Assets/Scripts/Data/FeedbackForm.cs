using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FeedbackForm : Singleton<FeedbackForm>, ISceneSingleton
{
    internal const double QuitWaitSeconds = 5d;
    private const int UploadTimeoutSeconds = 12;
    private const double RetryDelaySeconds = 20d;

    private PlayerDataManager collection;
    private DataPersistanceManager persistence;
    private List<FeedbackReport> reports;
    private int savedCount;
    private int uploadIndex;
    private double nextUploadAt;
    private UnityWebRequest activeRequest;

    internal bool HasPendingUploads => FindNextUpload() >= 0;

    private void Awake()
    {
        if (!RegisterSceneInstance())
            enabled = false;
    }

    private void OnDestroy() => UnregisterSceneInstance();

    internal void Initialize(PlayerDataManager owner, DataPersistanceManager saveManager, List<FeedbackReport> savedReports)
    {
        collection = owner;
        persistence = saveManager;
        reports = savedReports;
        savedCount = reports.Count;
    }

    internal void MarkReportsSaved(List<FeedbackReport> savedReports)
    {
        if (reports != null && ReferenceEquals(reports, savedReports))
            savedCount = reports.Count;
    }

    internal void RetryUploadsNow()
    {
        nextUploadAt = 0d;
    }

    private void OnDisable()
    {
        activeRequest?.Abort();
        StopAllCoroutines();
        DisposeRequest();
    }

    private void Update()
    {
        if (activeRequest != null || Time.realtimeSinceStartupAsDouble < nextUploadAt ||
            (persistence && persistence.IsChangingScene))
            return;

        int next = FindNextUpload();
        if (next >= 0)
            StartCoroutine(Upload(next));
        else
            nextUploadAt = Time.realtimeSinceStartupAsDouble + 1d;
    }

    private int FindNextUpload()
    {
        if (!isActiveAndEnabled || !collection || !collection.CollectsFeedback ||
            reports == null || savedCount == 0)
            return -1;

        int count = Math.Min(savedCount, reports.Count);
        for (int offset = 0; offset < count; offset++)
        {
            int index = (uploadIndex + offset) % count;
            FeedbackReport report = reports[index];
            if (!string.IsNullOrEmpty(report?.id) && ExistingFeedbackForm.Supports(report))
                return index;
        }

        return -1;
    }

    private IEnumerator Upload(int index)
    {
        List<FeedbackReport> queue = reports;
        FeedbackReport report = queue[index];

        try
        {
            UnityWebRequestAsyncOperation operation = StartRequest(report);
            if (operation == null)
            {
                ScheduleRetry(index, "request could not start");
                yield break;
            }

            yield return operation;
            if (!ReferenceEquals(queue, reports))
                yield break;

            string body = activeRequest.downloadHandler?.text;
            bool acknowledged = activeRequest.result == UnityWebRequest.Result.Success &&
                ExistingFeedbackForm.ResponseWasRecorded(body);
            if (!acknowledged)
            {
                ScheduleRetry(index, $"HTTP {activeRequest.responseCode} without confirmation");
                yield break;
            }

            queue.RemoveAt(index);
            savedCount--;
            uploadIndex = index;

            if (persistence)
                persistence.SaveGame();
        }
        finally
        {
            DisposeRequest();
        }
    }

    private UnityWebRequestAsyncOperation StartRequest(FeedbackReport report)
    {
        try
        {
            activeRequest = UnityWebRequest.Post(ExistingFeedbackForm.Url, ExistingFeedbackForm.Build(report));
            activeRequest.timeout = UploadTimeoutSeconds;
            return activeRequest.SendWebRequest();
        }
        catch (Exception)
        {
            return null;
        }
    }

    private void DisposeRequest()
    {
        activeRequest?.Dispose();
        activeRequest = null;
    }

    private void ScheduleRetry(int index, string reason)
    {
        uploadIndex = index + 1;
        nextUploadAt = Time.realtimeSinceStartupAsDouble + RetryDelaySeconds;
        Debug.LogWarning($"Playtest feedback is queued for retry ({reason}).", this);
    }
}
