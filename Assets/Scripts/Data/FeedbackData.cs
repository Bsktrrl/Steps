using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

internal enum FeedbackOutcome
{
    Completed,
    Exited,
    Reset,
    Quit,
    Interrupted,
    Legacy,
}

[Serializable]
public class FeedbackReport
{
    public string id;
    public string kind;
    public string sessionId;
    public string level;
    public string scene;
    public string outcome;
    public string recordedUtc;
    // Preserve acknowledgements written by the earlier feedback uploader.
    public bool submittedToExistingForm;
    public FeedbackLevelStats levelStats;
    public Session_Stats sessionStats;

    internal static FeedbackReport Create(string kind, string sessionId)
    {
        return new FeedbackReport
        {
            id = Guid.NewGuid().ToString("N"),
            kind = kind,
            sessionId = sessionId
        };
    }

    internal void Finish(FeedbackOutcome result)
    {
        outcome = result.ToString();
        recordedUtc = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
    }
}

[Serializable]
public class FeedbackLevelStats
{
    public float previousLevelSeconds;
    public float timeUsed;
    public int stepsTaken;
    public float goalReachedTimer;
    public float quitTimer;
    public int respawnTaken;
    public int cameraRotationTaken;
    public float freeCamTimer;
    public int ability_Swim;
    public int ability_SwiftSwim;
    public int ability_Ascend;
    public int ability_Descend;
    public int ability_Dash;
    public int ability_GrapplingHook;
    public int ability_Jump;
    public int ability_CeilingGrab;
    public int talkTo_NPC;
    public List<FeedbackPickup> pickups = new List<FeedbackPickup>();
}

[Serializable]
public class FeedbackPickup
{
    public string item;
    public int index;
    public string detail;
    public float seconds;
    public float attemptSeconds;
}

[Serializable]
public class FeedbackLevelTime
{
    public string key;
    public float seconds;
}

[Serializable]
public class PlayerStatsData
{
    public Session_Stats sessionStats = new Session_Stats();
    [HideInInspector] public string activeSessionId;
    [HideInInspector] public FeedbackReport activeLevelReport;
    [HideInInspector] public List<FeedbackReport> pendingFeedbackReports = new List<FeedbackReport>();
    [HideInInspector] public List<FeedbackLevelTime> levelTimes = new List<FeedbackLevelTime>();

    // These fields keep the existing save format readable.
    [HideInInspector] public List<Session_Stats> pendingSessionReports = new List<Session_Stats>();
    [HideInInspector] public Level_Stats rivergreenLv1_Stats;
    [HideInInspector] public Level_Stats rivergreenLv2_Stats;
    [HideInInspector] public Level_Stats rivergreenLv3_Stats;
    [HideInInspector] public Level_Stats rivergreenLv4_Stats;
    [HideInInspector] public Level_Stats rivergreenLv5_Stats;
    [HideInInspector] public Level_Stats sandlandsLv1_Stats;
    [HideInInspector] public Level_Stats sandlandsLv2_Stats;
    [HideInInspector] public Level_Stats sandlandsLv3_Stats;
    [HideInInspector] public Level_Stats sandlandsLv4_Stats;
    [HideInInspector] public Level_Stats sandlandsLv5_Stats;
    [HideInInspector] public Level_Stats frostfieldLv1_Stats;
    [HideInInspector] public Level_Stats frostfieldLv2_Stats;
    [HideInInspector] public Level_Stats frostfieldLv3_Stats;
    [HideInInspector] public Level_Stats frostfieldLv4_Stats;
    [HideInInspector] public Level_Stats frostfieldLv5_Stats;
    [HideInInspector] public Level_Stats fireveinLv1_Stats;
    [HideInInspector] public Level_Stats fireveinLv2_Stats;
    [HideInInspector] public Level_Stats fireveinLv3_Stats;
    [HideInInspector] public Level_Stats fireveinLv4_Stats;
    [HideInInspector] public Level_Stats fireveinLv5_Stats;
    [HideInInspector] public Level_Stats witchmireLv1_Stats;
    [HideInInspector] public Level_Stats witchmireLv2_Stats;
    [HideInInspector] public Level_Stats witchmireLv3_Stats;
    [HideInInspector] public Level_Stats witchmireLv4_Stats;
    [HideInInspector] public Level_Stats witchmireLv5_Stats;
    [HideInInspector] public Level_Stats metalworksLv1_Stats;
    [HideInInspector] public Level_Stats metalworksLv2_Stats;
    [HideInInspector] public Level_Stats metalworksLv3_Stats;
    [HideInInspector] public Level_Stats metalworksLv4_Stats;
    [HideInInspector] public Level_Stats metalworksLv5_Stats;

    internal void CarrySessionIntoNewGame(PlayerStatsData replacement)
    {
        replacement.activeSessionId = activeSessionId;
        replacement.sessionStats = sessionStats;
        replacement.pendingFeedbackReports = pendingFeedbackReports;
        replacement.pendingSessionReports = pendingSessionReports;
    }

    internal FeedbackLevelTime GetLevelTime(string level, string scene)
    {
        string key = level == nameof(LevelNames.None) ? "scene:" + scene : level;
        levelTimes ??= new List<FeedbackLevelTime>();
        foreach (FeedbackLevelTime time in levelTimes)
        {
            if (time?.key == key)
                return time;
        }

        // Recover the elapsed pickup clock from earlier saves once per level.
        Level_Stats legacy = level switch
        {
            nameof(LevelNames.Rivergreen_Lv1) => rivergreenLv1_Stats,
            nameof(LevelNames.Rivergreen_Lv2) => rivergreenLv2_Stats,
            nameof(LevelNames.Rivergreen_Lv3) => rivergreenLv3_Stats,
            nameof(LevelNames.Rivergreen_Lv4) => rivergreenLv4_Stats,
            nameof(LevelNames.Rivergreen_Lv5) => rivergreenLv5_Stats,
            nameof(LevelNames.Sandlands_Lv1) => sandlandsLv1_Stats,
            nameof(LevelNames.Sandlands_Lv2) => sandlandsLv2_Stats,
            nameof(LevelNames.Sandlands_Lv3) => sandlandsLv3_Stats,
            nameof(LevelNames.Sandlands_Lv4) => sandlandsLv4_Stats,
            nameof(LevelNames.Sandlands_Lv5) => sandlandsLv5_Stats,
            nameof(LevelNames.Frostfield_Lv1) => frostfieldLv1_Stats,
            nameof(LevelNames.Frostfield_Lv2) => frostfieldLv2_Stats,
            nameof(LevelNames.Frostfield_Lv3) => frostfieldLv3_Stats,
            nameof(LevelNames.Frostfield_Lv4) => frostfieldLv4_Stats,
            nameof(LevelNames.Frostfield_Lv5) => frostfieldLv5_Stats,
            nameof(LevelNames.Firevein_Lv1) => fireveinLv1_Stats,
            nameof(LevelNames.Firevein_Lv2) => fireveinLv2_Stats,
            nameof(LevelNames.Firevein_Lv3) => fireveinLv3_Stats,
            nameof(LevelNames.Firevein_Lv4) => fireveinLv4_Stats,
            nameof(LevelNames.Firevein_Lv5) => fireveinLv5_Stats,
            nameof(LevelNames.Witchmire_Lv1) => witchmireLv1_Stats,
            nameof(LevelNames.Witchmire_Lv2) => witchmireLv2_Stats,
            nameof(LevelNames.Witchmire_Lv3) => witchmireLv3_Stats,
            nameof(LevelNames.Witchmire_Lv4) => witchmireLv4_Stats,
            nameof(LevelNames.Witchmire_Lv5) => witchmireLv5_Stats,
            nameof(LevelNames.Metalworks_Lv1) => metalworksLv1_Stats,
            nameof(LevelNames.Metalworks_Lv2) => metalworksLv2_Stats,
            nameof(LevelNames.Metalworks_Lv3) => metalworksLv3_Stats,
            nameof(LevelNames.Metalworks_Lv4) => metalworksLv4_Stats,
            nameof(LevelNames.Metalworks_Lv5) => metalworksLv5_Stats,
            _ => null
        };
        var tracking = new FeedbackLevelTime { key = key, seconds = legacy?.PreviousPickupTime() ?? 0f };
        levelTimes.Add(tracking);
        return tracking;
    }

    internal void FinishLevel(FeedbackOutcome outcome)
    {
        FeedbackReport report = activeLevelReport;
        if (string.IsNullOrEmpty(report?.id) || report.levelStats == null)
        {
            activeLevelReport = null;
            return;
        }

        FeedbackLevelStats level = report.levelStats;
        level.pickups ??= new List<FeedbackPickup>();
        if (outcome == FeedbackOutcome.Completed)
            level.goalReachedTimer = level.timeUsed;
        else
            level.quitTimer = level.timeUsed;

        GetLevelTime(report.level, report.scene).seconds = level.previousLevelSeconds + level.timeUsed;
        (sessionStats ??= new Session_Stats()).AddLevel(level, outcome);
        report.Finish(outcome);
        if (ExistingFeedbackForm.Supports(report))
            (pendingFeedbackReports ??= new List<FeedbackReport>()).Add(report);
        activeLevelReport = null;
    }
}

[Serializable]
public class Session_Stats
{
    [Header("General")]
    public int session_No;
    public float totalTimeUsed;

    [Header("Timers")]
    public float totalTimeUsed_Menus;
    public float totalTimeUsed_MainMenu;
    public float totalTimeUsed_OverworldMenu;
    public float totalTimeUsed_WardrobeMenu;
    public float totalTimeUsed_OptionsMenu;
    public float totalTimeUsed_InPauseMenu;
    public float totalTimeUsed_InLevels;
    public float totalTimeUsed_InFreeCam;

    [Header("Counters")]
    public int totalLevelsVisited;
    public int totalLevelExited;
    public int totalLevelsCleared;
    public int totalStepsTaken;
    public int totalRespawnTaken;
    public int totalCameraRotationTaken;

    [Header("Wardrobe skin wearing timer")]
    public float totalTimeEquippedInLevels_Default;
    public float totalTimeEquippedInLevels_RivergreenLv1;
    public float totalTimeEquippedInLevels_RivergreenLv2;
    public float totalTimeEquippedInLevels_RivergreenLv3;
    public float totalTimeEquippedInLevels_RivergreenLv4;
    public float totalTimeEquippedInLevels_RivergreenLv5;
    public float totalTimeEquippedInLevels_SandlandsLv1;
    public float totalTimeEquippedInLevels_SandlandsLv2;
    public float totalTimeEquippedInLevels_SandlandsLv3;
    public float totalTimeEquippedInLevels_SandlandsLv4;
    public float totalTimeEquippedInLevels_SandlandsLv5;
    public float totalTimeEquippedInLevels_FrostfieldLv1;
    public float totalTimeEquippedInLevels_FrostfieldLv2;
    public float totalTimeEquippedInLevels_FrostfieldLv3;
    public float totalTimeEquippedInLevels_FrostfieldLv4;
    public float totalTimeEquippedInLevels_FrostfieldLv5;
    public float totalTimeEquippedInLevels_FireveinLv1;
    public float totalTimeEquippedInLevels_FireveinLv2;
    public float totalTimeEquippedInLevels_FireveinLv3;
    public float totalTimeEquippedInLevels_FireveinLv4;
    public float totalTimeEquippedInLevels_FireveinLv5;
    public float totalTimeEquippedInLevels_WitchmireLv1;
    public float totalTimeEquippedInLevels_WitchmireLv2;
    public float totalTimeEquippedInLevels_WitchmireLv3;
    public float totalTimeEquippedInLevels_WitchmireLv4;
    public float totalTimeEquippedInLevels_WitchmireLv5;
    public float totalTimeEquippedInLevels_MetalworksLv1;
    public float totalTimeEquippedInLevels_MetalworksLv2;
    public float totalTimeEquippedInLevels_MetalworksLv3;
    public float totalTimeEquippedInLevels_MetalworksLv4;
    public float totalTimeEquippedInLevels_MetalworksLv5;

    public float totalTimeUsed_InfoMenu;
    public int totalLevelsReset;
    public int totalLevelsInterrupted;
    public float totalTimeEquippedInLevels_Quartz;
    public float totalTimeEquippedInLevels_SourceBlock;

    [Header("Level totals")]
    public int totalSwimSteps;
    public int totalSwiftSwimUses;
    public int totalAscendUses;
    public int totalDescendUses;
    public int totalDashUses;
    public int totalGrapplingHookUses;
    public int totalJumpUses;
    public int totalCeilingGrabSteps;
    public int totalNPCDialogues;
    public int totalEssencePickups;
    public int totalFootprintPickups;
    public int totalSkinPickups;
    public int totalAbilityPickups;

    internal void AddLevel(FeedbackLevelStats level, FeedbackOutcome outcome)
    {
        totalLevelsVisited++;
        switch (outcome)
        {
            case FeedbackOutcome.Completed: totalLevelsCleared++; break;
            case FeedbackOutcome.Exited:
            case FeedbackOutcome.Quit: totalLevelExited++; break;
            case FeedbackOutcome.Reset: totalLevelsReset++; break;
            case FeedbackOutcome.Interrupted: totalLevelsInterrupted++; break;
        }

        totalTimeUsed_InLevels += level.timeUsed;
        totalTimeUsed_InFreeCam += level.freeCamTimer;
        totalStepsTaken += level.stepsTaken;
        totalRespawnTaken += level.respawnTaken;
        totalCameraRotationTaken += level.cameraRotationTaken;
        totalSwimSteps += level.ability_Swim;
        totalSwiftSwimUses += level.ability_SwiftSwim;
        totalAscendUses += level.ability_Ascend;
        totalDescendUses += level.ability_Descend;
        totalDashUses += level.ability_Dash;
        totalGrapplingHookUses += level.ability_GrapplingHook;
        totalJumpUses += level.ability_Jump;
        totalCeilingGrabSteps += level.ability_CeilingGrab;
        totalNPCDialogues += level.talkTo_NPC;

        foreach (FeedbackPickup pickup in level.pickups)
        {
            switch (pickup.item)
            {
                case "Essence": totalEssencePickups++; break;
                case "Footprint": totalFootprintPickups++; break;
                case "Skin": totalSkinPickups++; break;
                case "Ability": totalAbilityPickups++; break;
            }
        }
    }

    internal void AddSkinTime(SkinType skin, float seconds)
    {
        switch (skin)
        {
            case SkinType.Rivergreen_Lv1: totalTimeEquippedInLevels_RivergreenLv1 += seconds; break;
            case SkinType.Rivergreen_Lv2: totalTimeEquippedInLevels_RivergreenLv2 += seconds; break;
            case SkinType.Rivergreen_Lv3: totalTimeEquippedInLevels_RivergreenLv3 += seconds; break;
            case SkinType.Rivergreen_Lv4: totalTimeEquippedInLevels_RivergreenLv4 += seconds; break;
            case SkinType.Rivergreen_Lv5: totalTimeEquippedInLevels_RivergreenLv5 += seconds; break;
            case SkinType.Sandlands_Lv1: totalTimeEquippedInLevels_SandlandsLv1 += seconds; break;
            case SkinType.Sandlands_Lv2: totalTimeEquippedInLevels_SandlandsLv2 += seconds; break;
            case SkinType.Sandlands_Lv3: totalTimeEquippedInLevels_SandlandsLv3 += seconds; break;
            case SkinType.Sandlands_Lv4: totalTimeEquippedInLevels_SandlandsLv4 += seconds; break;
            case SkinType.Sandlands_Lv5: totalTimeEquippedInLevels_SandlandsLv5 += seconds; break;
            case SkinType.Frostfield_Lv1: totalTimeEquippedInLevels_FrostfieldLv1 += seconds; break;
            case SkinType.Frostfield_Lv2: totalTimeEquippedInLevels_FrostfieldLv2 += seconds; break;
            case SkinType.Frostfield_Lv3: totalTimeEquippedInLevels_FrostfieldLv3 += seconds; break;
            case SkinType.Frostfield_Lv4: totalTimeEquippedInLevels_FrostfieldLv4 += seconds; break;
            case SkinType.Frostfield_Lv5: totalTimeEquippedInLevels_FrostfieldLv5 += seconds; break;
            case SkinType.Firevein_Lv1: totalTimeEquippedInLevels_FireveinLv1 += seconds; break;
            case SkinType.Firevein_Lv2: totalTimeEquippedInLevels_FireveinLv2 += seconds; break;
            case SkinType.Firevein_Lv3: totalTimeEquippedInLevels_FireveinLv3 += seconds; break;
            case SkinType.Firevein_Lv4: totalTimeEquippedInLevels_FireveinLv4 += seconds; break;
            case SkinType.Firevein_Lv5: totalTimeEquippedInLevels_FireveinLv5 += seconds; break;
            case SkinType.Witchmire_Lv1: totalTimeEquippedInLevels_WitchmireLv1 += seconds; break;
            case SkinType.Witchmire_Lv2: totalTimeEquippedInLevels_WitchmireLv2 += seconds; break;
            case SkinType.Witchmire_Lv3: totalTimeEquippedInLevels_WitchmireLv3 += seconds; break;
            case SkinType.Witchmire_Lv4: totalTimeEquippedInLevels_WitchmireLv4 += seconds; break;
            case SkinType.Witchmire_Lv5: totalTimeEquippedInLevels_WitchmireLv5 += seconds; break;
            case SkinType.Metalworks_Lv1: totalTimeEquippedInLevels_MetalworksLv1 += seconds; break;
            case SkinType.Metalworks_Lv2: totalTimeEquippedInLevels_MetalworksLv2 += seconds; break;
            case SkinType.Metalworks_Lv3: totalTimeEquippedInLevels_MetalworksLv3 += seconds; break;
            case SkinType.Metalworks_Lv4: totalTimeEquippedInLevels_MetalworksLv4 += seconds; break;
            case SkinType.Metalworks_Lv5: totalTimeEquippedInLevels_MetalworksLv5 += seconds; break;
            case SkinType.Quartz: totalTimeEquippedInLevels_Quartz += seconds; break;
            case SkinType.SourceBlock: totalTimeEquippedInLevels_SourceBlock += seconds; break;
            default: totalTimeEquippedInLevels_Default += seconds; break;
        }
    }
}

[Serializable]
public class Level_Stats
{
    [Header("General")]
    public float timeUsed;
    public int stepsTaken;
    public float goalReachedTimer;
    public float quitTimer;
    public int respawnTaken;
    public int cameraRotationTaken;
    public float freeCamTimer;

    [Header("Abilities used")]
    public int ability_Swim;
    public int ability_SwiftSwim;
    public int ability_Ascend;
    public int ability_Descend;
    public int ability_Dash;
    public int ability_GrapplingHook;
    public int ability_Jump;
    public int ability_CeilingGrab;

    [Header("Essence Picked Up")]
    public float essence_1_TotalTimer;
    public bool essence_1_TotalTimer_Check;
    public float essence_2_TotalTimer;
    public bool essence_2_TotalTimer_Check;
    public float essence_3_TotalTimer;
    public bool essence_3_TotalTimer_Check;
    public float essence_4_TotalTimer;
    public bool essence_4_TotalTimer_Check;
    public float essence_5_TotalTimer;
    public bool essence_5_TotalTimer_Check;
    public float essence_6_TotalTimer;
    public bool essence_6_TotalTimer_Check;
    public float essence_7_TotalTimer;
    public bool essence_7_TotalTimer_Check;
    public float essence_8_TotalTimer;
    public bool essence_8_TotalTimer_Check;
    public float essence_9_TotalTimer;
    public bool essence_9_TotalTimer_Check;
    public float essence_10_TotalTimer;
    public bool essence_10_TotalTimer_Check;

    [Header("Footprints Picked Up")]
    public float footprint_1_TotalTimer;
    public bool footprint_1_TotalTimer_Check;
    public float footprint_2_TotalTimer;
    public bool footprint_2_TotalTimer_Check;
    public float footprint_3_TotalTimer;
    public bool footprint_3_TotalTimer_Check;

    [Header("Skin Picked Up")]
    public float skin_TotalTimer;
    public bool skin_TotalTimer_Check;

    [Header("Abilities Picked Up")]
    public float ability_1_TotalTimer;
    public bool ability_1_TotalTimer_Check;
    public float ability_2_TotalTimer;
    public bool ability_2_TotalTimer_Check;
    public float ability_3_TotalTimer;
    public bool ability_3_TotalTimer_Check;

    [Header("Talk to NPC")]
    public float talkTo_NPC;

    internal float PreviousPickupTime()
    {
        return Mathf.Max(timeUsed, essence_1_TotalTimer, essence_2_TotalTimer, essence_3_TotalTimer,
            essence_4_TotalTimer, essence_5_TotalTimer, essence_6_TotalTimer, essence_7_TotalTimer, essence_8_TotalTimer,
            essence_9_TotalTimer, essence_10_TotalTimer, footprint_1_TotalTimer, footprint_2_TotalTimer, footprint_3_TotalTimer,
            skin_TotalTimer, ability_1_TotalTimer, ability_2_TotalTimer, ability_3_TotalTimer);
    }
}
