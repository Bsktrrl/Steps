using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AnalyticsInitializer : MonoBehaviour
{
    async void Awake()
    {
        await UnityServices.InitializeAsync();
        Debug.Log("Unity Services Initialized");

        if (AnalyticsService.Instance != null)
        {
            AnalyticsService.Instance.StartDataCollection();
        }
    }
}
