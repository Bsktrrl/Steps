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
                    sessionStats.totalTimeEquippedInLevels_Default += realTime;
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
                    sessionStats.totalTimeEquippedInLevels_Default += realTime;
                    break;
            }
        }
    }


    //--------------------


    internal void StoreSessionStats()
    {
        var saved = DataManager.Instance.playerStatsData_Store.sessionStats ??= new Session_Stats();

        saved.totalTimeUsed += sessionStats.totalTimeUsed;

        saved.totalTimeUsed_Menus += sessionStats.totalTimeUsed_Menus;

        saved.totalTimeUsed_MainMenu += sessionStats.totalTimeUsed_MainMenu;
        saved.totalTimeUsed_OverworldMenu += sessionStats.totalTimeUsed_OverworldMenu;
        saved.totalTimeUsed_WardrobeMenu += sessionStats.totalTimeUsed_WardrobeMenu;
        saved.totalTimeUsed_OptionsMenu += sessionStats.totalTimeUsed_OptionsMenu;
        saved.totalTimeUsed_InPauseMenu += sessionStats.totalTimeUsed_InPauseMenu;
        saved.totalTimeUsed_InLevels += sessionStats.totalTimeUsed_InLevels;
        saved.totalTimeUsed_InFreeCam += sessionStats.totalTimeUsed_InFreeCam;

        saved.totalTimeEquippedInLevels_Default += sessionStats.totalTimeEquippedInLevels_Default;

        saved.totalTimeEquippedInLevels_RivergreenLv1 += sessionStats.totalTimeEquippedInLevels_RivergreenLv1;
        saved.totalTimeEquippedInLevels_RivergreenLv2 += sessionStats.totalTimeEquippedInLevels_RivergreenLv2;
        saved.totalTimeEquippedInLevels_RivergreenLv3 += sessionStats.totalTimeEquippedInLevels_RivergreenLv3;
        saved.totalTimeEquippedInLevels_RivergreenLv4 += sessionStats.totalTimeEquippedInLevels_RivergreenLv4;
        saved.totalTimeEquippedInLevels_RivergreenLv5 += sessionStats.totalTimeEquippedInLevels_RivergreenLv5;

        saved.totalTimeEquippedInLevels_SandlandsLv1 += sessionStats.totalTimeEquippedInLevels_SandlandsLv1;
        saved.totalTimeEquippedInLevels_SandlandsLv2 += sessionStats.totalTimeEquippedInLevels_SandlandsLv2;
        saved.totalTimeEquippedInLevels_SandlandsLv3 += sessionStats.totalTimeEquippedInLevels_SandlandsLv3;
        saved.totalTimeEquippedInLevels_SandlandsLv4 += sessionStats.totalTimeEquippedInLevels_SandlandsLv4;
        saved.totalTimeEquippedInLevels_SandlandsLv5 += sessionStats.totalTimeEquippedInLevels_SandlandsLv5;

        saved.totalTimeEquippedInLevels_FrostfieldLv1 += sessionStats.totalTimeEquippedInLevels_FrostfieldLv1;
        saved.totalTimeEquippedInLevels_FrostfieldLv2 += sessionStats.totalTimeEquippedInLevels_FrostfieldLv2;
        saved.totalTimeEquippedInLevels_FrostfieldLv3 += sessionStats.totalTimeEquippedInLevels_FrostfieldLv3;
        saved.totalTimeEquippedInLevels_FrostfieldLv4 += sessionStats.totalTimeEquippedInLevels_FrostfieldLv4;
        saved.totalTimeEquippedInLevels_FrostfieldLv5 += sessionStats.totalTimeEquippedInLevels_FrostfieldLv5;

        saved.totalTimeEquippedInLevels_FireveinLv1 += sessionStats.totalTimeEquippedInLevels_FireveinLv1;
        saved.totalTimeEquippedInLevels_FireveinLv2 += sessionStats.totalTimeEquippedInLevels_FireveinLv2;
        saved.totalTimeEquippedInLevels_FireveinLv3 += sessionStats.totalTimeEquippedInLevels_FireveinLv3;
        saved.totalTimeEquippedInLevels_FireveinLv4 += sessionStats.totalTimeEquippedInLevels_FireveinLv4;
        saved.totalTimeEquippedInLevels_FireveinLv5 += sessionStats.totalTimeEquippedInLevels_FireveinLv5;

        saved.totalTimeEquippedInLevels_WitchmireLv1 += sessionStats.totalTimeEquippedInLevels_WitchmireLv1;
        saved.totalTimeEquippedInLevels_WitchmireLv2 += sessionStats.totalTimeEquippedInLevels_WitchmireLv2;
        saved.totalTimeEquippedInLevels_WitchmireLv3 += sessionStats.totalTimeEquippedInLevels_WitchmireLv3;
        saved.totalTimeEquippedInLevels_WitchmireLv4 += sessionStats.totalTimeEquippedInLevels_WitchmireLv4;
        saved.totalTimeEquippedInLevels_WitchmireLv5 += sessionStats.totalTimeEquippedInLevels_WitchmireLv5;

        saved.totalTimeEquippedInLevels_MetalworksLv1 += sessionStats.totalTimeEquippedInLevels_MetalworksLv1;
        saved.totalTimeEquippedInLevels_MetalworksLv2 += sessionStats.totalTimeEquippedInLevels_MetalworksLv2;
        saved.totalTimeEquippedInLevels_MetalworksLv3 += sessionStats.totalTimeEquippedInLevels_MetalworksLv3;
        saved.totalTimeEquippedInLevels_MetalworksLv4 += sessionStats.totalTimeEquippedInLevels_MetalworksLv4;
        saved.totalTimeEquippedInLevels_MetalworksLv5 += sessionStats.totalTimeEquippedInLevels_MetalworksLv5;

        saved.totalLevelsVisited += sessionStats.totalLevelsVisited;
        saved.totalLevelExited += sessionStats.totalLevelExited;
        saved.totalLevelsCleared += sessionStats.totalLevelsCleared;
        saved.totalStepsTaken += sessionStats.totalStepsTaken;
        saved.totalRespawnTaken += sessionStats.totalRespawnTaken;
        saved.totalCameraRotationTaken += sessionStats.totalCameraRotationTaken;
        sessionStats = new Session_Stats();
    }
}
