using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionStatsGathered : Singleton<SessionStatsGathered>
{
    public Session_Stats sessionStats = new Session_Stats();

    float realTime = 0;

    MainMenuManager mainMenuManager;
    PlayerManager playerManager;
    PauseMenuManager pauseMenuManager;
    SkinsManager skinsManager;
    SkinWardrobeManager skinWardrobeManager;
    SettingsManager optionsManager;


    //--------------------


    private void Start()
    {
        mainMenuManager = FindObjectOfType<MainMenuManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        pauseMenuManager = FindObjectOfType<PauseMenuManager>();
        skinsManager = FindObjectOfType<SkinsManager>();
        skinWardrobeManager = FindObjectOfType<SkinWardrobeManager>();
        optionsManager = FindObjectOfType<SettingsManager>();
    }
    private void Update()
    {
        UpdateAllTimers_Session();
    }
    void UpdateAllTimers_Session()
    {
        realTime = Time.deltaTime;

        //Total Time
        sessionStats.totalTimeUsed += realTime;

        //MainMenuManager
        if (mainMenuManager)
        {
            sessionStats.totalTimeUsed_Menus += realTime;

            if (mainMenuManager.mainMenu_Parent.activeInHierarchy)
                sessionStats.totalTimeUsed_MainMenu += realTime;
            if (mainMenuManager.overworldMenu_Parent.activeInHierarchy)
                sessionStats.totalTimeUsed_OverworldMenu += realTime;
            if (mainMenuManager.skinsMenu_Parent.activeInHierarchy)
                sessionStats.totalTimeUsed_WardrobeMenu += realTime;
            if (mainMenuManager.optionsMenu_Parent.activeInHierarchy)
                sessionStats.totalTimeUsed_OptionsMenu += realTime;
        }

        //PauseMenuManager
        if (pauseMenuManager && pauseMenuManager.isActive)
        {
            sessionStats.totalTimeUsed_Menus += realTime;

            if (pauseMenuManager.pauseMenu_Skins_Parent.activeInHierarchy)
            {
                sessionStats.totalTimeUsed_WardrobeMenu += realTime;
            }
            else if (pauseMenuManager.pauseMenu_Options_Parent.activeInHierarchy)
            {
                sessionStats.totalTimeUsed_OptionsMenu += realTime;
            }
            else
            {
                sessionStats.totalTimeUsed_InPauseMenu += realTime;
            }
        }

        //WardrobeSkin Wearing
        if (playerManager && skinsManager && pauseMenuManager && !pauseMenuManager.isActive)
        {
            switch (skinsManager.skinInfo.activeSkinType)
            {
                case SkinType.None:
                    break;

                case SkinType.Default:
                    sessionStats.totalTimeEquippedInLevels_Default += realTime;
                    break;

                case SkinType.Rivergreen_Lv1:
                    sessionStats.totalTimeEquippedInLevels_RivergreenLv1 += realTime;
                    break;
                case SkinType.Rivergreen_Lv2:
                    sessionStats.totalTimeEquippedInLevels_RivergreenLv2 += realTime;
                    break;
                case SkinType.Rivergreen_Lv3:
                    sessionStats.totalTimeEquippedInLevels_RivergreenLv3 += realTime;
                    break;
                case SkinType.Rivergreen_Lv4:
                    sessionStats.totalTimeEquippedInLevels_RivergreenLv4 += realTime;
                    break;
                case SkinType.Rivergreen_Lv5:
                    sessionStats.totalTimeEquippedInLevels_RivergreenLv5 += realTime;
                    break;

                case SkinType.Sandlands_Lv1:
                    sessionStats.totalTimeEquippedInLevels_SandlandsLv1 += realTime;
                    break;
                case SkinType.Sandlands_Lv2:
                    sessionStats.totalTimeEquippedInLevels_SandlandsLv2 += realTime;
                    break;
                case SkinType.Sandlands_Lv3:
                    sessionStats.totalTimeEquippedInLevels_SandlandsLv3 += realTime;
                    break;
                case SkinType.Sandlands_Lv4:
                    sessionStats.totalTimeEquippedInLevels_SandlandsLv4 += realTime;
                    break;
                case SkinType.Sandlands_Lv5:
                    sessionStats.totalTimeEquippedInLevels_SandlandsLv5 += realTime;
                    break;

                case SkinType.Frostfield_Lv1:
                    sessionStats.totalTimeEquippedInLevels_FrostfieldLv1 += realTime;
                    break;
                case SkinType.Frostfield_Lv2:
                    sessionStats.totalTimeEquippedInLevels_FrostfieldLv2 += realTime;
                    break;
                case SkinType.Frostfield_Lv3:
                    sessionStats.totalTimeEquippedInLevels_FrostfieldLv3 += realTime;
                    break;
                case SkinType.Frostfield_Lv4:
                    sessionStats.totalTimeEquippedInLevels_FrostfieldLv4 += realTime;
                    break;
                case SkinType.Frostfield_Lv5:
                    sessionStats.totalTimeEquippedInLevels_FrostfieldLv5 += realTime;
                    break;

                case SkinType.Firevein_Lv1:
                    sessionStats.totalTimeEquippedInLevels_FireveinLv1 += realTime;
                    break;
                case SkinType.Firevein_Lv2:
                    sessionStats.totalTimeEquippedInLevels_FireveinLv2 += realTime;
                    break;
                case SkinType.Firevein_Lv3:
                    sessionStats.totalTimeEquippedInLevels_FireveinLv3 += realTime;
                    break;
                case SkinType.Firevein_Lv4:
                    sessionStats.totalTimeEquippedInLevels_FireveinLv4 += realTime;
                    break;
                case SkinType.Firevein_Lv5:
                    sessionStats.totalTimeEquippedInLevels_FireveinLv5 += realTime;
                    break;

                case SkinType.Witchmire_Lv1:
                    sessionStats.totalTimeEquippedInLevels_WitchmireLv1 += realTime;
                    break;
                case SkinType.Witchmire_Lv2:
                    sessionStats.totalTimeEquippedInLevels_WitchmireLv2 += realTime;
                    break;
                case SkinType.Witchmire_Lv3:
                    sessionStats.totalTimeEquippedInLevels_WitchmireLv3 += realTime;
                    break;
                case SkinType.Witchmire_Lv4:
                    sessionStats.totalTimeEquippedInLevels_WitchmireLv4 += realTime;
                    break;
                case SkinType.Witchmire_Lv5:
                    sessionStats.totalTimeEquippedInLevels_WitchmireLv5 += realTime;
                    break;

                case SkinType.Metalworks_Lv1:
                    sessionStats.totalTimeEquippedInLevels_MetalworksLv1 += realTime;
                    break;
                case SkinType.Metalworks_Lv2:
                    sessionStats.totalTimeEquippedInLevels_MetalworksLv2 += realTime;
                    break;
                case SkinType.Metalworks_Lv3:
                    sessionStats.totalTimeEquippedInLevels_MetalworksLv3 += realTime;
                    break;
                case SkinType.Metalworks_Lv4:
                    sessionStats.totalTimeEquippedInLevels_MetalworksLv4 += realTime;
                    break;
                case SkinType.Metalworks_Lv5:
                    sessionStats.totalTimeEquippedInLevels_MetalworksLv5 += realTime;
                    break;

                default:
                    break;
            }
        }
    }


    //--------------------


    public void ResetSessionStats()
    {
        //General
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed = 0;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_Menus = 0;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_MainMenu = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_OverworldMenu = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_WardrobeMenu = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_OptionsMenu = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_InPauseMenu = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_InLevels = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_InFreeCam = 0;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalLevelsVisited = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalLevelExited = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalLevelsCleared = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalStepsTaken = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalRespawnTaken = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalCameraRotationTaken = 0;

        //WardrobeSkin Wearing
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default = 0;

        DataPersistanceManager.instance.SaveGame();
    }
    public void SaveSessionStats()
    {
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed += sessionStats.totalTimeUsed;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_Menus += sessionStats.totalTimeUsed_Menus;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_MainMenu += sessionStats.totalTimeUsed_MainMenu;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_OverworldMenu += sessionStats.totalTimeUsed_OverworldMenu;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_WardrobeMenu += sessionStats.totalTimeUsed_WardrobeMenu;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_OptionsMenu += sessionStats.totalTimeUsed_OptionsMenu;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_InPauseMenu += sessionStats.totalTimeUsed_InPauseMenu;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_InLevels += sessionStats.totalTimeUsed_InLevels;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeUsed_InFreeCam += sessionStats.totalTimeUsed_InFreeCam;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_Default += sessionStats.totalTimeEquippedInLevels_Default;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_RivergreenLv1 += sessionStats.totalTimeEquippedInLevels_RivergreenLv1;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_RivergreenLv2 += sessionStats.totalTimeEquippedInLevels_RivergreenLv2;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_RivergreenLv3 += sessionStats.totalTimeEquippedInLevels_RivergreenLv3;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_RivergreenLv4 += sessionStats.totalTimeEquippedInLevels_RivergreenLv4;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_RivergreenLv5 += sessionStats.totalTimeEquippedInLevels_RivergreenLv5;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_SandlandsLv1 += sessionStats.totalTimeEquippedInLevels_SandlandsLv1;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_SandlandsLv2 += sessionStats.totalTimeEquippedInLevels_SandlandsLv2;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_SandlandsLv3 += sessionStats.totalTimeEquippedInLevels_SandlandsLv3;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_SandlandsLv4 += sessionStats.totalTimeEquippedInLevels_SandlandsLv4;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_SandlandsLv5 += sessionStats.totalTimeEquippedInLevels_SandlandsLv5;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FrostfieldLv1 += sessionStats.totalTimeEquippedInLevels_FrostfieldLv1;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FrostfieldLv2 += sessionStats.totalTimeEquippedInLevels_FrostfieldLv2;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FrostfieldLv3 += sessionStats.totalTimeEquippedInLevels_FrostfieldLv3;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FrostfieldLv4 += sessionStats.totalTimeEquippedInLevels_FrostfieldLv4;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FrostfieldLv5 += sessionStats.totalTimeEquippedInLevels_FrostfieldLv5;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FireveinLv1 += sessionStats.totalTimeEquippedInLevels_FireveinLv1;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FireveinLv2 += sessionStats.totalTimeEquippedInLevels_FireveinLv2;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FireveinLv3 += sessionStats.totalTimeEquippedInLevels_FireveinLv3;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FireveinLv4 += sessionStats.totalTimeEquippedInLevels_FireveinLv4;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_FireveinLv5 += sessionStats.totalTimeEquippedInLevels_FireveinLv5;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_WitchmireLv1 += sessionStats.totalTimeEquippedInLevels_WitchmireLv1;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_WitchmireLv2 += sessionStats.totalTimeEquippedInLevels_WitchmireLv2;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_WitchmireLv3 += sessionStats.totalTimeEquippedInLevels_WitchmireLv3;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_WitchmireLv4 += sessionStats.totalTimeEquippedInLevels_WitchmireLv4;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_WitchmireLv5 += sessionStats.totalTimeEquippedInLevels_WitchmireLv5;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_MetalworksLv1 += sessionStats.totalTimeEquippedInLevels_MetalworksLv1;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_MetalworksLv2 += sessionStats.totalTimeEquippedInLevels_MetalworksLv2;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_MetalworksLv3 += sessionStats.totalTimeEquippedInLevels_MetalworksLv3;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_MetalworksLv4 += sessionStats.totalTimeEquippedInLevels_MetalworksLv4;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalTimeEquippedInLevels_MetalworksLv5 += sessionStats.totalTimeEquippedInLevels_MetalworksLv5;

        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalLevelsVisited += sessionStats.totalLevelsVisited;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalLevelExited += sessionStats.totalLevelExited;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalLevelsCleared += sessionStats.totalLevelsCleared;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalStepsTaken += sessionStats.totalStepsTaken;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalRespawnTaken += sessionStats.totalRespawnTaken;
        DataManager.Instance.PlayerStatsData_Store.sessionStats.totalCameraRotationTaken += sessionStats.totalCameraRotationTaken;

        DataPersistanceManager.instance.SaveGame();
    }
}