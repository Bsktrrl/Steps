using UnityEngine;

public class MapStatsGathered : Singleton<MapStatsGathered>, ISceneSingleton
{
    private PlayerDataManager collection;
    private MapManager map;
    private PauseMenuManager pauseMenu;
    private FreeCam freeCam;
    private DataManager data;
    private FeedbackLevelStats levelStats;
    private Session_Stats sessionStats;
    private bool attemptEnded;

    private bool CanRecord => isActiveAndEnabled && collection && collection.IsRecording &&
        levelStats != null && !attemptEnded;

    private void Awake()
    {
        if (!RegisterSceneInstance())
            enabled = false;
    }

    private void OnDestroy() => UnregisterSceneInstance();

    internal void BeginTracking(PlayerDataManager owner, MapManager levelMap, PauseMenuManager pause,
        FeedbackLevelStats level, Session_Stats session)
    {
        collection = owner;
        map = levelMap;
        pauseMenu = pause;
        GameObject player = MapManager.SpawnedPlayer;
        freeCam = player ? player.GetComponentInChildren<FreeCam>(true) : null;
        data = DataManager.ExistingInstance;
        levelStats = level;
        sessionStats = session;
        attemptEnded = false;
    }

    internal void EndTracking()
    {
        attemptEnded = true;
    }

    private void Update()
    {
        if (!CanRecord || !map || map.IntroInputLocked || map.IsCompletingLevel ||
            (pauseMenu && pauseMenu.isActive) || DataPersistanceManager.GameplayInputBlocked)
            return;

        float seconds = Time.unscaledDeltaTime;
        levelStats.timeUsed += seconds;
        if (freeCam && freeCam._isActive)
            levelStats.freeCamTimer += seconds;

        sessionStats.AddSkinTime(data?.skinsInfo_Store?.activeSkinType ?? SkinType.Default, seconds);
    }

    private void OnEnable()
    {
        Movement.Action_StepTaken += RecordStep;
        Movement.Action_isSwiftSwim += RecordSwiftSwim;
        Movement.Action_isDashing += RecordDash;
        Movement.Action_isJumping += RecordJump;
        Movement.Action_isAscending += RecordAscend;
        Movement.Action_isDescending += RecordDescend;
        NPCManager.Action_DialogueIsFinished += RecordDialogue;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= RecordStep;
        Movement.Action_isSwiftSwim -= RecordSwiftSwim;
        Movement.Action_isDashing -= RecordDash;
        Movement.Action_isJumping -= RecordJump;
        Movement.Action_isAscending -= RecordAscend;
        Movement.Action_isDescending -= RecordDescend;
        NPCManager.Action_DialogueIsFinished -= RecordDialogue;
    }

    private void RecordStep() { if (CanRecord) levelStats.stepsTaken++; }
    private void RecordSwiftSwim() { if (CanRecord) levelStats.ability_SwiftSwim++; }
    private void RecordDash() { if (CanRecord) levelStats.ability_Dash++; }
    private void RecordJump() { if (CanRecord) levelStats.ability_Jump++; }
    private void RecordAscend() { if (CanRecord) levelStats.ability_Ascend++; }
    private void RecordDescend() { if (CanRecord) levelStats.ability_Descend++; }
    private void RecordDialogue() { if (CanRecord) levelStats.talkTo_NPC++; }
    internal void RecordRespawn() { if (CanRecord) levelStats.respawnTaken++; }
    internal void RecordCameraRotation() { if (CanRecord) levelStats.cameraRotationTaken++; }
    internal void RecordGrapplingHook() { if (CanRecord) levelStats.ability_GrapplingHook++; }
    internal void RecordCeilingStep() { if (CanRecord) levelStats.ability_CeilingGrab++; }
    internal void RecordSwimStep() { if (CanRecord) levelStats.ability_Swim++; }

    internal void RecordPickup(Items item, int index, string detail = "")
    {
        if (!CanRecord || index <= 0)
            return;

        levelStats.pickups.Add(new FeedbackPickup
        {
            item = item == Items.None ? "Ability" : item.ToString(),
            index = index,
            detail = detail,
            attemptSeconds = levelStats.timeUsed,
            seconds = levelStats.previousLevelSeconds + levelStats.timeUsed
        });
    }
}
