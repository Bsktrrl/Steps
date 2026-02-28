using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStatsGathered : Singleton<MapStatsGathered>
{
    public Level_Stats levelStats = new Level_Stats();
    public Session_Stats sessionStats = new Session_Stats();

    float realTime = 0;


    //--------------------


    private void Update()
    {
        UpdateAllTimers();
    }
    void UpdateAllTimers()
    {
        realTime = Time.deltaTime;

        levelStats.timeUsed += Time.deltaTime;

        if (!levelStats.essence_1_TotalTimer_Check)
            levelStats.essence_1_TotalTimer += realTime;
        if (!levelStats.essence_2_TotalTimer_Check)
            levelStats.essence_2_TotalTimer += realTime;
        if (!levelStats.essence_3_TotalTimer_Check)
            levelStats.essence_3_TotalTimer += realTime;
        if (!levelStats.essence_4_TotalTimer_Check)
            levelStats.essence_4_TotalTimer += realTime;
        if (!levelStats.essence_5_TotalTimer_Check)
            levelStats.essence_5_TotalTimer += realTime;
        if (!levelStats.essence_6_TotalTimer_Check)
            levelStats.essence_6_TotalTimer += realTime;
        if (!levelStats.essence_7_TotalTimer_Check)
            levelStats.essence_7_TotalTimer += realTime;
        if (!levelStats.essence_8_TotalTimer_Check)
            levelStats.essence_8_TotalTimer += realTime;
        if (!levelStats.essence_9_TotalTimer_Check)
            levelStats.essence_9_TotalTimer += realTime;
        if (!levelStats.essence_10_TotalTimer_Check)
            levelStats.essence_10_TotalTimer += realTime;

        if (!levelStats.footprint_1_TotalTimer_Check)
            levelStats.footprint_1_TotalTimer += realTime;
        if (!levelStats.footprint_2_TotalTimer_Check)
            levelStats.footprint_2_TotalTimer += realTime;
        if (!levelStats.footprint_3_TotalTimer_Check)
            levelStats.footprint_3_TotalTimer += realTime;

        if (!levelStats.skin_TotalTimer_Check)
            levelStats.skin_TotalTimer += realTime;

        if (!levelStats.ability_1_TotalTimer_Check)
            levelStats.ability_1_TotalTimer += realTime;
        if (!levelStats.ability_2_TotalTimer_Check)
            levelStats.ability_2_TotalTimer += realTime;
        if (!levelStats.ability_3_TotalTimer_Check)
            levelStats.ability_3_TotalTimer += realTime;
    }


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += UpdateLevelStats;

        Movement.Action_RespawnPlayer += UpdateRespawnCount;

        Movement.Action_StepTaken += UpdateStepCount;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= UpdateLevelStats;

        Movement.Action_RespawnPlayer -= UpdateRespawnCount;

        Movement.Action_StepTaken -= UpdateStepCount;
    }


    //--------------------


    void UpdateLevelStats()
    {
        levelStats.timeUsed = 0;
        levelStats.stepsTaken = 0;
        levelStats.respawnTaken = 0;
        levelStats.goalReachedTimer = 0;
        levelStats.quitTimer = 0;
        levelStats.respawnTaken = -1;
        levelStats.cameraRotationTaken = 0;
        levelStats.freeCamTimer = 0;

        levelStats.ability_Swim = 0;
        levelStats.ability_SwiftSwim = 0;
        levelStats.ability_Ascend = 0;
        levelStats.ability_Descend = 0;
        levelStats.ability_Dash = 0;
        levelStats.ability_GrapplingHook = 0;
        levelStats.ability_Jump = 0;
        levelStats.ability_CeilingGrab = 0;

        Level_Stats levelStats_DataManger = GetLevelData(MapManager.Instance.levelName);

        if (levelStats_DataManger.essence_1_TotalTimer_Check)
        {
            levelStats.essence_1_TotalTimer = 0;
            levelStats.essence_1_TotalTimer_Check = true;
        }
        else
        {
            levelStats.essence_1_TotalTimer = levelStats_DataManger.essence_1_TotalTimer;
            levelStats.essence_1_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.essence_2_TotalTimer_Check)
        {
            levelStats.essence_2_TotalTimer = 0;
            levelStats.essence_2_TotalTimer_Check = true;
        }
        else
        {
            levelStats.essence_2_TotalTimer = levelStats_DataManger.essence_2_TotalTimer;
            levelStats.essence_2_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.essence_3_TotalTimer_Check)
        {
            levelStats.essence_3_TotalTimer = 0;
            levelStats.essence_3_TotalTimer_Check = true;
        }
        else
        {
            levelStats.essence_3_TotalTimer = levelStats_DataManger.essence_3_TotalTimer;
            levelStats.essence_3_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.essence_4_TotalTimer_Check)
        {
            levelStats.essence_4_TotalTimer = 0;
            levelStats.essence_4_TotalTimer_Check = true;
        }
        else
        {
            levelStats.essence_4_TotalTimer = levelStats_DataManger.essence_4_TotalTimer;
            levelStats.essence_4_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.essence_5_TotalTimer_Check)
        {
            levelStats.essence_5_TotalTimer = 0;
            levelStats.essence_5_TotalTimer_Check = true;
        }
        else
        {
            levelStats.essence_5_TotalTimer = levelStats_DataManger.essence_5_TotalTimer;
            levelStats.essence_5_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.essence_6_TotalTimer_Check)
        {
            levelStats.essence_6_TotalTimer = 0;
            levelStats.essence_6_TotalTimer_Check = true;
        }
        else
        {
            levelStats.essence_6_TotalTimer = levelStats_DataManger.essence_6_TotalTimer;
            levelStats.essence_6_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.essence_7_TotalTimer_Check)
        {
            levelStats.essence_7_TotalTimer = 0;
            levelStats.essence_7_TotalTimer_Check = true;
        }
        else
        {
            levelStats.essence_7_TotalTimer = levelStats_DataManger.essence_7_TotalTimer;
            levelStats.essence_7_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.essence_8_TotalTimer_Check)
        {
            levelStats.essence_8_TotalTimer = 0;
            levelStats.essence_8_TotalTimer_Check = true;
        }
        else
        {
            levelStats.essence_8_TotalTimer = levelStats_DataManger.essence_8_TotalTimer;
            levelStats.essence_8_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.essence_9_TotalTimer_Check)
        {
            levelStats.essence_9_TotalTimer = 0;
            levelStats.essence_9_TotalTimer_Check = true;
        }
        else
        {
            levelStats.essence_9_TotalTimer = levelStats_DataManger.essence_9_TotalTimer;
            levelStats.essence_9_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.essence_10_TotalTimer_Check)
        {
            levelStats.essence_10_TotalTimer = 0;
            levelStats.essence_10_TotalTimer_Check = true;
        }
        else
        {
            levelStats.essence_10_TotalTimer = levelStats_DataManger.essence_10_TotalTimer;
            levelStats.essence_10_TotalTimer_Check = false;
        }


        if (levelStats_DataManger.footprint_1_TotalTimer_Check)
        {
            levelStats.footprint_1_TotalTimer = 0;
            levelStats.footprint_1_TotalTimer_Check = true;
        }
        else
        {
            levelStats.footprint_1_TotalTimer = levelStats_DataManger.footprint_1_TotalTimer;
            levelStats.footprint_1_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.footprint_2_TotalTimer_Check)
        {
            levelStats.footprint_2_TotalTimer = 0;
            levelStats.footprint_2_TotalTimer_Check = true;
        }
        else
        {
            levelStats.footprint_2_TotalTimer = levelStats_DataManger.footprint_2_TotalTimer;
            levelStats.footprint_2_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.footprint_3_TotalTimer_Check)
        {
            levelStats.footprint_3_TotalTimer = 0;
            levelStats.footprint_3_TotalTimer_Check = true;
        }
        else
        {
            levelStats.footprint_3_TotalTimer = levelStats_DataManger.footprint_3_TotalTimer;
            levelStats.footprint_3_TotalTimer_Check = false;
        }


        if (levelStats_DataManger.skin_TotalTimer_Check)
        {
            levelStats.skin_TotalTimer = 0;
            levelStats.skin_TotalTimer_Check = true;
        }
        else
        {
            levelStats.skin_TotalTimer = levelStats_DataManger.skin_TotalTimer;
            levelStats.skin_TotalTimer_Check = false;
        }


        if (levelStats_DataManger.ability_1_TotalTimer_Check)
        {
            levelStats.ability_1_TotalTimer = 0;
            levelStats.ability_1_TotalTimer_Check = true;
        }
        else
        {
            levelStats.ability_1_TotalTimer = levelStats_DataManger.ability_1_TotalTimer;
            levelStats.ability_1_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.ability_2_TotalTimer_Check)
        {
            levelStats.ability_2_TotalTimer = 0;
            levelStats.ability_2_TotalTimer_Check = true;
        }
        else
        {
            levelStats.ability_2_TotalTimer = levelStats_DataManger.ability_2_TotalTimer;
            levelStats.ability_2_TotalTimer_Check = false;
        }

        if (levelStats_DataManger.ability_3_TotalTimer_Check)
        {
            levelStats.ability_3_TotalTimer = 0;
            levelStats.ability_3_TotalTimer_Check = true;
        }
        else
        {
            levelStats.ability_3_TotalTimer = levelStats_DataManger.ability_3_TotalTimer;
            levelStats.ability_3_TotalTimer_Check = false;
        }

        sessionStats.totalTimeEquippedInLevels_Default = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default;
        sessionStats.totalTimeEquippedInLevels_RivergreenLv1 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_RivergreenLv1;
        sessionStats.totalTimeEquippedInLevels_RivergreenLv2 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_RivergreenLv2;
        sessionStats.totalTimeEquippedInLevels_RivergreenLv3 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_RivergreenLv3;
        sessionStats.totalTimeEquippedInLevels_RivergreenLv4 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_RivergreenLv4;
        sessionStats.totalTimeEquippedInLevels_RivergreenLv5 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_RivergreenLv5;
        sessionStats.totalTimeEquippedInLevels_SandlandsLv1 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_SandlandsLv1;
        sessionStats.totalTimeEquippedInLevels_SandlandsLv2 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_SandlandsLv2;
        sessionStats.totalTimeEquippedInLevels_SandlandsLv3 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_SandlandsLv3;
        sessionStats.totalTimeEquippedInLevels_SandlandsLv4 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_SandlandsLv4;
        sessionStats.totalTimeEquippedInLevels_SandlandsLv5 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_SandlandsLv5;
        sessionStats.totalTimeEquippedInLevels_FrostfieldLv1 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FrostfieldLv1;
        sessionStats.totalTimeEquippedInLevels_FrostfieldLv2 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FrostfieldLv2;
        sessionStats.totalTimeEquippedInLevels_FrostfieldLv3 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FrostfieldLv3;
        sessionStats.totalTimeEquippedInLevels_FrostfieldLv4 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FrostfieldLv4;
        sessionStats.totalTimeEquippedInLevels_FrostfieldLv5 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FrostfieldLv5;
        sessionStats.totalTimeEquippedInLevels_FireveinLv1 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FireveinLv1;
        sessionStats.totalTimeEquippedInLevels_FireveinLv2 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FireveinLv2;
        sessionStats.totalTimeEquippedInLevels_FireveinLv3 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FireveinLv3;
        sessionStats.totalTimeEquippedInLevels_FireveinLv4 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FireveinLv4;
        sessionStats.totalTimeEquippedInLevels_FireveinLv5 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FireveinLv5;
        sessionStats.totalTimeEquippedInLevels_WitchmireLv1 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_WitchmireLv1;
        sessionStats.totalTimeEquippedInLevels_WitchmireLv2 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_WitchmireLv2;
        sessionStats.totalTimeEquippedInLevels_WitchmireLv3 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_WitchmireLv3;
        sessionStats.totalTimeEquippedInLevels_WitchmireLv4 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_WitchmireLv4;
        sessionStats.totalTimeEquippedInLevels_WitchmireLv5 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_WitchmireLv5;
        sessionStats.totalTimeEquippedInLevels_MetalworksLv1 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_MetalworksLv1;
        sessionStats.totalTimeEquippedInLevels_MetalworksLv2 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_MetalworksLv2;
        sessionStats.totalTimeEquippedInLevels_MetalworksLv3 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_MetalworksLv3;
        sessionStats.totalTimeEquippedInLevels_MetalworksLv4 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_MetalworksLv4;
        sessionStats.totalTimeEquippedInLevels_MetalworksLv5 = DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_MetalworksLv5;
    }

    Level_Stats GetLevelData(LevelNames levelName)
    {
        switch (levelName)
        {
            case LevelNames.None:
                return null;

            case LevelNames.Rivergreen_Lv1:
                return DataManager.Instance.PlayerStatsData_Store.rivergreenLv1_Stats;
            case LevelNames.Rivergreen_Lv2:
                return DataManager.Instance.PlayerStatsData_Store.rivergreenLv2_Stats;
            case LevelNames.Rivergreen_Lv3:
                return DataManager.Instance.PlayerStatsData_Store.rivergreenLv3_Stats;
            case LevelNames.Rivergreen_Lv4:
                return DataManager.Instance.PlayerStatsData_Store.rivergreenLv4_Stats;
            case LevelNames.Rivergreen_Lv5:
                return DataManager.Instance.PlayerStatsData_Store.rivergreenLv5_Stats;

            case LevelNames.Sandlands_Lv1:
                return DataManager.Instance.PlayerStatsData_Store.sandlandsLv1_Stats;
            case LevelNames.Sandlands_Lv2:
                return DataManager.Instance.PlayerStatsData_Store.sandlandsLv2_Stats;
            case LevelNames.Sandlands_Lv3:
                return DataManager.Instance.PlayerStatsData_Store.sandlandsLv3_Stats;
            case LevelNames.Sandlands_Lv4:
                return DataManager.Instance.PlayerStatsData_Store.sandlandsLv4_Stats;
            case LevelNames.Sandlands_Lv5:
                return DataManager.Instance.PlayerStatsData_Store.sandlandsLv5_Stats;

            case LevelNames.Frostfield_Lv1:
                return DataManager.Instance.PlayerStatsData_Store.frostfieldLv1_Stats;
            case LevelNames.Frostfield_Lv2:
                return DataManager.Instance.PlayerStatsData_Store.frostfieldLv2_Stats;
            case LevelNames.Frostfield_Lv3:
                return DataManager.Instance.PlayerStatsData_Store.frostfieldLv3_Stats;
            case LevelNames.Frostfield_Lv4:
                return DataManager.Instance.PlayerStatsData_Store.frostfieldLv4_Stats;
            case LevelNames.Frostfield_Lv5:
                return DataManager.Instance.PlayerStatsData_Store.frostfieldLv5_Stats;

            case LevelNames.Firevein_Lv1:
                return DataManager.Instance.PlayerStatsData_Store.fireveinLv1_Stats;
            case LevelNames.Firevein_Lv2:
                return DataManager.Instance.PlayerStatsData_Store.fireveinLv2_Stats;
            case LevelNames.Firevein_Lv3:
                return DataManager.Instance.PlayerStatsData_Store.fireveinLv3_Stats;
            case LevelNames.Firevein_Lv4:
                return DataManager.Instance.PlayerStatsData_Store.fireveinLv4_Stats;
            case LevelNames.Firevein_Lv5:
                return DataManager.Instance.PlayerStatsData_Store.fireveinLv5_Stats;

            case LevelNames.Witchmire_Lv1:
                return DataManager.Instance.PlayerStatsData_Store.witchmireLv1_Stats;
            case LevelNames.Witchmire_Lv2:
                return DataManager.Instance.PlayerStatsData_Store.witchmireLv2_Stats;
            case LevelNames.Witchmire_Lv3:
                return DataManager.Instance.PlayerStatsData_Store.witchmireLv3_Stats;
            case LevelNames.Witchmire_Lv4:
                return DataManager.Instance.PlayerStatsData_Store.witchmireLv4_Stats;
            case LevelNames.Witchmire_Lv5:
                return DataManager.Instance.PlayerStatsData_Store.witchmireLv5_Stats;

            case LevelNames.Metalworks_Lv1:
                return DataManager.Instance.PlayerStatsData_Store.metalworksLv1_Stats;
            case LevelNames.Metalworks_Lv2:
                return DataManager.Instance.PlayerStatsData_Store.metalworksLv2_Stats;
            case LevelNames.Metalworks_Lv3:
                return DataManager.Instance.PlayerStatsData_Store.metalworksLv3_Stats;
            case LevelNames.Metalworks_Lv4:
                return DataManager.Instance.PlayerStatsData_Store.metalworksLv4_Stats;
            case LevelNames.Metalworks_Lv5:
                return DataManager.Instance.PlayerStatsData_Store.metalworksLv5_Stats;

            default:
                return null;
        }
    }


    //--------------------


    void UpdateStepCount()
    {
        levelStats.stepsTaken++;
    }
    void UpdateRespawnCount()
    {
        levelStats.respawnTaken++;
    }


    //--------------------


    public void UpdateEssencePickedUp_Stats(int index)
    {
        if (index == 1)
            levelStats.essence_1_TotalTimer_Check = true;
        if (index == 2)
            levelStats.essence_2_TotalTimer_Check = true;
        if (index == 3)
            levelStats.essence_3_TotalTimer_Check = true;
        if (index == 4)
            levelStats.essence_4_TotalTimer_Check = true;
        if (index == 5)
            levelStats.essence_5_TotalTimer_Check = true;
        if (index == 6)
            levelStats.essence_6_TotalTimer_Check = true;
        if (index == 7)
            levelStats.essence_7_TotalTimer_Check = true;
        if (index == 8)
            levelStats.essence_8_TotalTimer_Check = true;
        if (index == 9)
            levelStats.essence_9_TotalTimer_Check = true;
        if (index == 10)
            levelStats.essence_10_TotalTimer_Check = true;
    }
    public void UpdateFootprintPickedUp_Stats(int index)
    {
        if (index == 1)
            levelStats.footprint_1_TotalTimer_Check = true;
        if (index == 2)
            levelStats.footprint_2_TotalTimer_Check = true;
        if (index == 3)
            levelStats.footprint_3_TotalTimer_Check = true;
    }
    public void UpdateSkinPickedUp_Stats(int index)
    {
        if (index == 1)
            levelStats.skin_TotalTimer_Check = true;
    }
    public void UpdateAbilityPickedUp_Stats(int index)
    {
        if (index == 1)
            levelStats.ability_1_TotalTimer_Check = true;
        if (index == 2)
            levelStats.ability_2_TotalTimer_Check = true;
        if (index == 3)
            levelStats.ability_3_TotalTimer_Check = true;
    }


    //--------------------


    public void ExitLevel()
    {
        levelStats.quitTimer = levelStats.timeUsed;

        SetupEndLevel();
    }
    public void CompleteLevel()
    {
        levelStats.goalReachedTimer = levelStats.timeUsed;

        SetupEndLevel();
    }
    void SetupEndLevel()
    {
        switch (MapManager.Instance.levelName)
        {
            case LevelNames.None:
                break;

            case LevelNames.Rivergreen_Lv1:
                DataManager.Instance.PlayerStatsData_Store.rivergreenLv1_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Rivergreen_Lv1();
                break;
            case LevelNames.Rivergreen_Lv2:
                DataManager.Instance.PlayerStatsData_Store.rivergreenLv2_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Rivergreen_Lv2();
                break;
            case LevelNames.Rivergreen_Lv3:
                DataManager.Instance.PlayerStatsData_Store.rivergreenLv3_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Rivergreen_Lv3();
                break;
            case LevelNames.Rivergreen_Lv4:
                DataManager.Instance.PlayerStatsData_Store.rivergreenLv4_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Rivergreen_Lv4();
                break;
            case LevelNames.Rivergreen_Lv5:
                DataManager.Instance.PlayerStatsData_Store.rivergreenLv5_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Rivergreen_Lv5();
                break;

            case LevelNames.Sandlands_Lv1:
                DataManager.Instance.PlayerStatsData_Store.sandlandsLv1_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Sandlands_Lv1();
                break;
            case LevelNames.Sandlands_Lv2:
                DataManager.Instance.PlayerStatsData_Store.sandlandsLv2_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Sandlands_Lv2();
                break;
            case LevelNames.Sandlands_Lv3:
                DataManager.Instance.PlayerStatsData_Store.sandlandsLv3_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Sandlands_Lv3();
                break;
            case LevelNames.Sandlands_Lv4:
                DataManager.Instance.PlayerStatsData_Store.sandlandsLv4_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Sandlands_Lv4();
                break;
            case LevelNames.Sandlands_Lv5:
                DataManager.Instance.PlayerStatsData_Store.sandlandsLv5_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Sandlands_Lv5();
                break;

            case LevelNames.Frostfield_Lv1:
                DataManager.Instance.PlayerStatsData_Store.frostfieldLv1_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Frostfield_Lv1();
                break;
            case LevelNames.Frostfield_Lv2:
                DataManager.Instance.PlayerStatsData_Store.frostfieldLv2_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Frostfield_Lv2();
                break;
            case LevelNames.Frostfield_Lv3:
                DataManager.Instance.PlayerStatsData_Store.frostfieldLv3_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Frostfield_Lv3();
                break;
            case LevelNames.Frostfield_Lv4:
                DataManager.Instance.PlayerStatsData_Store.frostfieldLv4_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Frostfield_Lv4();
                break;
            case LevelNames.Frostfield_Lv5:
                DataManager.Instance.PlayerStatsData_Store.frostfieldLv5_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Frostfield_Lv5();
                break;

            case LevelNames.Firevein_Lv1:
                DataManager.Instance.PlayerStatsData_Store.fireveinLv1_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Firevein_Lv1();
                break;
            case LevelNames.Firevein_Lv2:
                DataManager.Instance.PlayerStatsData_Store.fireveinLv2_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Firevein_Lv2();
                break;
            case LevelNames.Firevein_Lv3:
                DataManager.Instance.PlayerStatsData_Store.fireveinLv3_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Firevein_Lv3();
                break;
            case LevelNames.Firevein_Lv4:
                DataManager.Instance.PlayerStatsData_Store.fireveinLv4_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Firevein_Lv4();
                break;
            case LevelNames.Firevein_Lv5:
                DataManager.Instance.PlayerStatsData_Store.fireveinLv5_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Firevein_Lv5();
                break;

            case LevelNames.Witchmire_Lv1:
                DataManager.Instance.PlayerStatsData_Store.witchmireLv1_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Witchmire_Lv1();
                break;
            case LevelNames.Witchmire_Lv2:
                DataManager.Instance.PlayerStatsData_Store.witchmireLv2_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Witchmire_Lv2();
                break;
            case LevelNames.Witchmire_Lv3:
                DataManager.Instance.PlayerStatsData_Store.witchmireLv3_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Witchmire_Lv3();
                break;
            case LevelNames.Witchmire_Lv4:
                DataManager.Instance.PlayerStatsData_Store.witchmireLv4_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Witchmire_Lv4();
                break;
            case LevelNames.Witchmire_Lv5:
                DataManager.Instance.PlayerStatsData_Store.witchmireLv5_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Witchmire_Lv5();
                break;

            case LevelNames.Metalworks_Lv1:
                DataManager.Instance.PlayerStatsData_Store.metalworksLv1_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Metalworks_Lv1();
                break;
            case LevelNames.Metalworks_Lv2:
                DataManager.Instance.PlayerStatsData_Store.metalworksLv2_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Metalworks_Lv2();
                break;
            case LevelNames.Metalworks_Lv3:
                DataManager.Instance.PlayerStatsData_Store.metalworksLv3_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Metalworks_Lv3();
                break;
            case LevelNames.Metalworks_Lv4:
                DataManager.Instance.PlayerStatsData_Store.metalworksLv4_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Metalworks_Lv4();
                break;
            case LevelNames.Metalworks_Lv5:
                DataManager.Instance.PlayerStatsData_Store.metalworksLv5_Stats = levelStats;
                DataPersistanceManager.instance.SaveGame();
                FeedbackForm.Instance.SubmitFeedback_Metalworks_Lv5();
                break;

            default:
                break;
        }
    }

    //Update WardrobeSkin weared in-game
    //Update DataManager when level is exited
}
