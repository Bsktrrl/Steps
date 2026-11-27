using UnityEngine;

public class SessionStatsGathered : Singleton<SessionStatsGathered>, ISceneSingleton
{
    private PlayerDataManager collection;
    private Session_Stats sessionStats;
    private MainMenuManager mainMenu;
    private PauseMenuManager pauseMenu;

    private void Awake()
    {
        if (!RegisterSceneInstance())
            enabled = false;
    }

    private void OnDestroy() => UnregisterSceneInstance();

    internal void BeginTracking(PlayerDataManager owner, Session_Stats stats, MainMenuManager menu, PauseMenuManager pause)
    {
        collection = owner;
        sessionStats = stats;
        mainMenu = menu;
        pauseMenu = pause;
    }

    private void Update()
    {
        if (!collection || !collection.IsRecording)
            return;

        float seconds = Time.unscaledDeltaTime;
        sessionStats.totalTimeUsed += seconds;

        if (pauseMenu && pauseMenu.isActive)
        {
            sessionStats.totalTimeUsed_Menus += seconds;
            sessionStats.totalTimeUsed_InPauseMenu += seconds;
            switch (pauseMenu.menuState)
            {
                case MenuState.Pause_Menu_Skins: sessionStats.totalTimeUsed_WardrobeMenu += seconds; break;
                case MenuState.Pause_Menu_Options: sessionStats.totalTimeUsed_OptionsMenu += seconds; break;
                case MenuState.Pause_Menu_Info: sessionStats.totalTimeUsed_InfoMenu += seconds; break;
            }
        }
        else if (mainMenu)
        {
            sessionStats.totalTimeUsed_Menus += seconds;
            if (mainMenu.skinsMenu_Parent && mainMenu.skinsMenu_Parent.activeInHierarchy)
                sessionStats.totalTimeUsed_WardrobeMenu += seconds;
            else if (mainMenu.optionsMenu_Parent && mainMenu.optionsMenu_Parent.activeInHierarchy)
                sessionStats.totalTimeUsed_OptionsMenu += seconds;
            else if (mainMenu.infoMenu_Parent && mainMenu.infoMenu_Parent.activeInHierarchy)
                sessionStats.totalTimeUsed_InfoMenu += seconds;
            else if (mainMenu.overworldMenu_Parent && mainMenu.overworldMenu_Parent.activeInHierarchy)
                sessionStats.totalTimeUsed_OverworldMenu += seconds;
            else
                sessionStats.totalTimeUsed_MainMenu += seconds;
        }
    }
}
