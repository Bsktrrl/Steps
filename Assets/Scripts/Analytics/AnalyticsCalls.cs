using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AnalyticsCalls
{
    #region General

    public static void GetEssence(int pickupNumber)
    {
        if (AnalyticsService.Instance != null)
        {
            string levelName = SceneManager.GetActiveScene().name;

            var eventData = new Dictionary<string, object>
            {
                { "level_name", levelName },
                { "coin_id", pickupNumber }
            };

            AnalyticsService.Instance.CustomData("coin_pickup", eventData);

            Debug.Log($"Coin pickup event sent: Coin {pickupNumber} in {levelName}");
        }
        else
        {
            ErrorMessage();
        }
    }

    public static void GetAllEssenceInALevel()
    {
        if (AnalyticsService.Instance != null)
        {
            string levelName = SceneManager.GetActiveScene().name;

            var eventData = new Dictionary<string, object>
            {
                { "level_name", levelName },
                { "all_coins_collected", true }
            };

            AnalyticsService.Instance.CustomData("all_coins_collected", eventData);

            Debug.Log($"All coins collected in level: {levelName}");
        }
        else
        {
            ErrorMessage();
        }
    }

    #endregion


    //--------------------


    #region Overworld Menu

    public static void SelectLevel(string levelName, string regionName)
    {
        if (AnalyticsService.Instance != null)
        {
            var eventData = new Dictionary<string, object>
            {
                { "level_name", levelName },
                { "region_name", regionName }
            };

            AnalyticsService.Instance.CustomData("level_selected", eventData);

            Debug.Log($"Level selected: {levelName}");
        }
        else
        {
            ErrorMessage();
        }
    }

    public static void FirstLevelToSelect(string levelName)
    {

    }
    public static void CompleteLevel()
    {
        if (AnalyticsService.Instance != null)
        {
            string levelName = SceneManager.GetActiveScene().name;

            var eventData = new Dictionary<string, object>
            {
                { "level_name", levelName }
            };

            AnalyticsService.Instance.CustomData("level_completed", eventData);

            Debug.Log($"Level completed: {levelName}");
        }
        else
        {
            ErrorMessage();
        }
    }
    public static void CompleteRegion(string regionName)
    {

    }
    #endregion


    //--------------------


    #region Levels

    public static void OnLevel(float time, int stepCount, int respawnCount, int abilityCount, int cameraRotationCount, int swimCounter, int swiftSwimCounter, int jumpCounter, int dashCounter, int ascendCounter, int descendCounter, int grapplingHookCounter, int ceilingGrabCounter)
    {
        if (AnalyticsService.Instance != null)
        {
            string levelName = SceneManager.GetActiveScene().name;
            float stepsPerTime = time / stepCount;

            var eventData = new Dictionary<string, object>
            {
                { "level_name", SceneManager.GetActiveScene().name },

                { "time", time },
                { "steps", stepCount },
                { "seconds_per_step", stepsPerTime },
                { "respawns", respawnCount },
                { "abilities", abilityCount },
                { "camera_rotations", cameraRotationCount },

                { "swimming", swimCounter },
                { "swift_swim", swiftSwimCounter },
                { "jumping", jumpCounter },
                { "dashing", dashCounter },
                { "ascending", ascendCounter },
                { "descending", descendCounter },
                { "grappling_hooking", grapplingHookCounter },
                { "ceiling_grabbing", ceilingGrabCounter }
            };

            AnalyticsService.Instance.CustomData("onlevel", eventData);

            Debug.Log("Onlevel sent: " + JsonUtility.ToJson(new SerializableDictionaryWrapper(eventData)));

        }
        else
        {
            ErrorMessage();
        }
    }

    public static void OnLevelExit(float time, int stepCount, int respawnCount, int abilityCount, int cameraRotationCount, int swimCounter, int swiftSwimCounter, int jumpCounter, int dashCounter, int ascendCounter, int descendCounter, int grapplingHookCounter, int ceilingGrabCounter)
    {
        if (AnalyticsService.Instance != null)
        {
            string levelName = SceneManager.GetActiveScene().name;
            float stepsPerTime = time/ stepCount;

            var eventData = new Dictionary<string, object>
            {
                { "level_name", SceneManager.GetActiveScene().name },

                { "time", time },
                { "steps", stepCount },
                { "seconds_per_step", stepsPerTime },
                { "respawns", respawnCount },
                { "abilities", abilityCount },
                { "camera_rotations", cameraRotationCount },

                { "swimming", swimCounter },
                { "swift_swim", swiftSwimCounter },
                { "jumping", jumpCounter },
                { "dashing", dashCounter },
                { "ascending", ascendCounter },
                { "descending", descendCounter },
                { "grappling_hooking", grapplingHookCounter },
                { "ceiling_grabbing", ceilingGrabCounter }
            };

            AnalyticsService.Instance.CustomData("onlevel_exit", eventData);

            Debug.Log("Onlevel_Exit sent: " + JsonUtility.ToJson(new SerializableDictionaryWrapper(eventData)));

        }
        else
        {
            ErrorMessage();
        }
    }

    public static void OnLevelFinishing(float time, int stepCount, int respawnCount, int abilityCount, int cameraRotationCount, int swimCounter, int swiftSwimCounter, int jumpCounter, int dashCounter, int ascendCounter, int descendCounter, int grapplingHookCounter, int ceilingGrabCounter)
    {
        if (AnalyticsService.Instance != null)
        {
            string levelName = SceneManager.GetActiveScene().name;
            float stepsPerTime = time / stepCount;

            var eventData = new Dictionary<string, object>
            {
                { "level_name", SceneManager.GetActiveScene().name },

                { "time", time },
                { "steps", stepCount },
                { "seconds_per_step", stepsPerTime },
                { "respawns", respawnCount },
                { "abilities", abilityCount },
                { "camera_rotations", cameraRotationCount },

                { "swimming", swimCounter },
                { "swift_swim", swiftSwimCounter },
                { "jumping", jumpCounter },
                { "dashing", dashCounter },
                { "ascending", ascendCounter },
                { "descending", descendCounter },
                { "grappling_hooking", grapplingHookCounter },
                { "ceiling_grabbing", ceilingGrabCounter }
            };

            AnalyticsService.Instance.CustomData("onlevel_finish", eventData);

            Debug.Log("Onlevel_Finishing sent: " + JsonUtility.ToJson(new SerializableDictionaryWrapper(eventData)));
        }
        else
        {
            ErrorMessage();
        }
    }

    #endregion


    //--------------------


    static void ErrorMessage()
    {
        Debug.LogWarning("AnalyticsService.Instance is null. It has not been initialized");
    }
}

[System.Serializable]
public class SerializableDictionaryWrapper
{
    public List<string> keys = new List<string>();
    public List<string> values = new List<string>();

    public SerializableDictionaryWrapper(Dictionary<string, object> dict)
    {
        foreach (var kvp in dict)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value.ToString());
        }
    }
}