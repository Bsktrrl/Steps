using System;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine;

public class AnalyticsInitializer : MonoBehaviour
{
    private IAnalyticsService collectingService;
    private bool hasStarted;
    private int enableVersion;

    private void Start()
    {
        // The level's PlayerDataManager is created with the player during Awake.
        hasStarted = true;
        StartAnalyticsIfAllowed();
    }

    private void OnEnable()
    {
        if (hasStarted)
            StartAnalyticsIfAllowed();
    }

    private bool AllowsCollection()
    {
        PlayerDataManager collection = PlayerDataManager.ExistingInstance;
        return isActiveAndEnabled && collection && collection.isActiveAndEnabled &&
            collection.gameObject.scene == gameObject.scene && collection.UsesUnityAnalytics;
    }

    private async void StartAnalyticsIfAllowed()
    {
        if (!AllowsCollection())
            return;

        int version = enableVersion;
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
                await UnityServices.InitializeAsync();

            // Initialization may finish after the scene or component was disabled.
            if (!this || version != enableVersion || !AllowsCollection())
                return;

            IAnalyticsService service = AnalyticsService.Instance;
            service.StartDataCollection();
            collectingService = service;
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"Unity Analytics could not start. {exception.Message}");
        }
    }

    private void OnDisable()
    {
        enableVersion++;
        if (collectingService == null)
            return;

        try
        {
            collectingService.StopDataCollection();
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"Unity Analytics could not stop. {exception.Message}");
        }
        finally
        {
            collectingService = null;
        }
    }
}
