using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>, ISceneSingleton
{
    [SerializeField] private FeedbackCollectionSettings feedbackSettings;

    private static string runSessionId;
    private PlayerStatsData data;
    private FeedbackForm feedback;
    private bool sessionEnded;

    internal bool CollectsFeedback => isActiveAndEnabled && feedbackSettings && feedbackSettings.collectData;
    internal bool UsesUnityAnalytics => CollectsFeedback && feedbackSettings.enableUnityAnalytics;
    internal bool IsRecording => CollectsFeedback && data != null && !sessionEnded;
    internal FeedbackForm Feedback => feedback;

    private void Awake()
    {
        if (!RegisterSceneInstance())
            enabled = false;
    }

    private void OnDestroy() => UnregisterSceneInstance();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRunId()
    {
        runSessionId = null;
    }

    internal void BeginCollection(PlayerStatsData savedData, MapManager map, DataPersistanceManager saveManager)
    {
        if (!CollectsFeedback || savedData == null || ReferenceEquals(data, savedData))
            return;

        data = savedData;
        sessionEnded = false;
        runSessionId ??= Guid.NewGuid().ToString("N");
        data.pendingFeedbackReports ??= new List<FeedbackReport>();
        data.sessionStats ??= new Session_Stats();

        feedback = FeedbackForm.ExistingInstance;
        if (feedback && feedback.gameObject.scene == gameObject.scene)
            feedback.Initialize(this, saveManager, data.pendingFeedbackReports);
        else
            feedback = null;

        bool needsSave = RecoverReports();
        if (!string.IsNullOrEmpty(data.activeLevelReport?.id))
        {
            data.FinishLevel(FeedbackOutcome.Interrupted);
            needsSave = true;
        }

        if (data.activeSessionId != runSessionId)
        {
            data.sessionStats = new Session_Stats { session_No = Mathf.Max(1, data.sessionStats.session_No) };
            data.activeSessionId = runSessionId;
            needsSave = true;
        }

        MainMenuManager menu = MainMenuManager.Instance;
        PauseMenuManager pause = map ? FindAnyObjectByType<PauseMenuManager>() : null;
        SessionStatsGathered.ExistingInstance?.BeginTracking(this, data.sessionStats, menu, pause);

        MapStatsGathered tracker = MapStatsGathered.ExistingInstance;
        if (map && tracker && tracker.gameObject.scene == gameObject.scene)
        {
            FeedbackReport report = FeedbackReport.Create("level", runSessionId);
            report.level = map.levelName.ToString();
            report.scene = map.gameObject.scene.name;
            report.levelStats = new FeedbackLevelStats
            {
                previousLevelSeconds = data.GetLevelTime(report.level, report.scene).seconds
            };
            data.activeLevelReport = report;
            tracker.BeginTracking(this, map, pause, report.levelStats, data.sessionStats);
            needsSave = true;
        }

        if (needsSave)
            saveManager.SaveGame();
    }

    internal void FinishLevel(FeedbackOutcome outcome)
    {
        if (IsRecording)
        {
            MapStatsGathered.ExistingInstance?.EndTracking();
            data.FinishLevel(outcome);
        }
    }

    internal void FinishSession(FeedbackOutcome levelOutcome)
    {
        if (!IsRecording)
            return;

        FinishLevel(levelOutcome);
        QueueSession(FeedbackOutcome.Quit);
        sessionEnded = true;
    }

    private bool RecoverReports()
    {
        bool changed = false;
        if (data.pendingSessionReports is { Count: > 0 } legacyReports)
        {
            foreach (Session_Stats legacy in legacyReports)
            {
                if (legacy?.session_No > 0)
                    AddSessionReport(legacy, Guid.NewGuid().ToString("N"), FeedbackOutcome.Legacy);
            }

            legacyReports.Clear();
            changed = true;
        }

        if (string.IsNullOrEmpty(data.activeSessionId))
        {
            if (data.sessionStats.totalTimeUsed > 0f || data.sessionStats.totalLevelsVisited > 0)
            {
                data.activeSessionId = Guid.NewGuid().ToString("N");
                QueueSession(FeedbackOutcome.Legacy);
                changed = true;
            }
        }
        else if (data.activeSessionId != runSessionId)
        {
            data.FinishLevel(FeedbackOutcome.Interrupted);
            QueueSession(FeedbackOutcome.Interrupted);
            changed = true;
        }

        return changed;
    }

    private void QueueSession(FeedbackOutcome outcome)
    {
        Session_Stats session = data.sessionStats;
        AddSessionReport(session, data.activeSessionId, outcome);
        data.sessionStats = new Session_Stats { session_No = Mathf.Max(1, session.session_No) + 1 };
        data.activeSessionId = null;
    }

    private void AddSessionReport(Session_Stats session, string sessionId, FeedbackOutcome outcome)
    {
        session.session_No = Mathf.Max(1, session.session_No);
        FeedbackReport report = FeedbackReport.Create("session", sessionId);
        report.sessionStats = session;
        report.Finish(outcome);
        data.pendingFeedbackReports.Add(report);
    }
}
