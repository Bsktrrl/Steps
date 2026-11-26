using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : Singleton<PlayerManager>
{
    #region Variables

    public static event Action Action_UpdatedLookingAtHorizontal;

    [Header("Player Object")]
    public GameObject player;
    public GameObject playerBody;

    [Header("Data")]
    public GameObject dataManagerObject;

    [Header("Player Block Moving Towards")]
    public DetectedBlockInfo block_MovingTowards;

    [Header("Player Block Looking At")]
    //public Vector3 lookingDirection;
    public GameObject block_LookingAt_Horizontal;
    public GameObject block_LookingAt_Vertical;

    [Header("Game Paused")]
    public bool pauseGame;
    public bool pauseMovement;
    public bool npcInteraction;

    [Header("mainMenu_Name")]
    [SerializeField] string mainMenu_Name;

    #endregion


    //--------------------


    private void Start()
    {
        if (!DataManager.Instance.oneTimeRunData_Store.tutorial_PlayerSpawned)
        {
            PauseGame();
        }
    }

    private void Update()
    {
        RespawnPlayerIfToLowInMapHeight();
    }

    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += LoadPlayerStats;
        DataManager.Action_dataHasLoaded += SetNewPlayerPosition;
        Movement.Action_StepTaken += StepsOnFallableBlock;
        Movement.Action_StepTaken += BlockLookingAt;
        Movement.Action_HasLandedFromFalling += BlockLookingAt;
        Movement.Action_BodyRotated += BlockLookingAt;
        Movement.Action_RespawnPlayerEarly += ClearInteractionTargetForRespawn;
        Movement.Action_RespawnPlayerLate += BlockLookingAt;

        Block_Moveable.Action_ObjectHasBeenPushed += BlockLookingAt;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= LoadPlayerStats;
        DataManager.Action_dataHasLoaded -= SetNewPlayerPosition;
        Movement.Action_StepTaken -= StepsOnFallableBlock;
        Movement.Action_StepTaken -= BlockLookingAt;
        Movement.Action_HasLandedFromFalling -= BlockLookingAt;
        Movement.Action_BodyRotated -= BlockLookingAt;
        Movement.Action_RespawnPlayerEarly -= ClearInteractionTargetForRespawn;
        Movement.Action_RespawnPlayerLate -= BlockLookingAt;

        Block_Moveable.Action_ObjectHasBeenPushed -= BlockLookingAt;
    }


    //--------------------


    void LoadPlayerStats()
    {
        SaveLoad_PlayerStats.Instance.LoadData();
    }
    public void SavePlayerStats()
    {
        SaveLoad_PlayerStats.Instance.SaveData();
    }


    //--------------------


    void SetNewPlayerPosition()
    {
        if (player)
            StartCoroutine(SetNewPlayerPosition_Delay(0.1f));
    }
    IEnumerator SetNewPlayerPosition_Delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        MapManager map = MapManager.Instance;
        if (!map || map.gameObject.scene != gameObject.scene || map.levelName != LevelNames.HUB)
            yield break;

        HUD_Map hud = HUD_Map.Instance;
        var spawn = DataManager.Instance.oneTimeRunData_Store.glueplantHUB_PickedUp
            ? hud.playerStartPosition_Later
            : hud.playerStartPosition_Beginning;

        map.playerStartRot = spawn.rot;
        CameraController.Instance.SetRespawnCameraRotation();
        Movement.Instance.RotatePlayerBody(180);

        transform.position = spawn.pos;
        map.playerStartPos = spawn.pos;
    }


    //--------------------


    void StepsOnFallableBlock()
    {
        if (Movement.Instance.blockStandingOn && !Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            if (Movement.Instance.blockStandingOn.GetComponent<Block_Falling>())
            {
                Movement.Instance.blockStandingOn.GetComponent<Block_Falling>().StepsOnFallableBlock();
            }
        }
    }


    public bool PreventButtonsOfTrigger()
    {
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }

        if (pauseGame) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Player_GraplingHook.Instance.isGrapplingHooking) { return false; }
        //if (Player_Dash.Instance.isDashing) { return false; }
        //if (Player_Ascend.Instance.isAscending) { return false; }
        //if (Player_Descend.Instance.isDescending) { return false; }
        //if (Player_Jumping.Instance.isJumping) { return false; }
        //if (Player_SwiftSwim.Instance.isSwiftSwimming_Down) { return false; }
        //if (Player_SwiftSwim.Instance.isSwiftSwimming_Up) { return false; }
        if (!Player_CeilingGrab.Instance.isCeilingRotation) { return false; }

        return true;
    }


    //--------------------


    void BlockLookingAt() => RefreshHorizontalLookTarget();

    internal GameObject RefreshHorizontalLookTarget()
    {
        if (playerBody &&
            Physics.Raycast(playerBody.transform.position + Vector3.up * 0.25f,
                playerBody.transform.forward, out RaycastHit hit, 1.1f) &&
            hit.collider.TryGetComponent<BlockInfo>(out _))
        {
            GameObject target = hit.collider.gameObject;
            if (block_LookingAt_Horizontal != target)
            {
                block_LookingAt_Horizontal = target;
                Action_UpdatedLookingAtHorizontal?.Invoke();
            }

            return target;
        }

        ClearHorizontalLookTarget();
        return null;
    }

    void ClearHorizontalLookTarget()
    {
        block_LookingAt_Horizontal = null;
        Action_UpdatedLookingAtHorizontal?.Invoke();
    }

    void ClearInteractionTargetForRespawn()
    {
        ClearHorizontalLookTarget();

        ButtonMessageManager prompts = GetComponentInChildren<ButtonMessageManager>();
        if (!prompts)
            return;

        prompts.HideButtonMessage(prompts.buttonMessages?.buttonMessage_Read);
        prompts.HideButtonMessage(prompts.buttonMessages?.buttonMessage_PlaceGlueplant);
    }


    //--------------------


    void RespawnPlayerIfToLowInMapHeight()
    {
        if (transform.position.y <= -5)
        {
            Movement.Instance.RespawnPlayer();
        }
    }


    //--------------------


    public void PauseGame()
    {
        pauseGame = true;
    }
    public void UnpauseGame()
    {
        pauseGame = false;
    }

    public void PauseMovement()
    {
        pauseMovement = true;
    }
    public void UnpauseMovement()
    {
        pauseMovement = false;
    }


    //--------------------


    private static string exitedLevelScene;
    private static bool selectNextLevelOnReturn;

    public static string ConsumeExitedLevelScene(out bool selectNextLevel)
    {
        string sceneName = exitedLevelScene;
        selectNextLevel = selectNextLevelOnReturn;
        exitedLevelScene = null;
        selectNextLevelOnReturn = false;
        return sceneName;
    }

    public void QuitLevel()
    {
        if (!string.IsNullOrEmpty(mainMenu_Name))
            Player_Statistics_Manager.Active?.FinishLevelAttempt();

        ReturnToLevelMenu(false);
    }

    internal bool TryBeginLevelExit()
    {
        return !string.IsNullOrEmpty(mainMenu_Name) && Application.CanStreamedLevelBeLoaded(mainMenu_Name) &&
            TryBeginSceneChange();
    }

    public void CompleteLevelAndLeave()
    {
        if (!TryBeginSceneChange())
            return;

        Player_Statistics_Manager.Active?.RecordLevelCompletion();
        Player_Statistics_Manager.Active?.FinishLevelAttempt();
        MapStatsGathered.Instance.CompleteLevel();

        switch (MapManager.Instance.levelName)
        {
            case LevelNames.Rivergreen_Lv5:
            case LevelNames.Sandlands_Lv5:
            case LevelNames.Frostfield_Lv5:
            case LevelNames.Firevein_Lv5:
            case LevelNames.Witchmire_Lv5:
            case LevelNames.Metalworks_Lv5:
                StartCoroutine(LoadSceneCoroutine(nameof(LevelNames.HUB)));
                break;

            default:
                ReturnToLevelMenu(true);
                break;
        }
    }

    private void ReturnToLevelMenu(bool selectNextLevel)
    {
        if (!string.IsNullOrEmpty(mainMenu_Name))
        {
            exitedLevelScene = SceneManager.GetActiveScene().name;
            selectNextLevelOnReturn = selectNextLevel;
            StartCoroutine(LoadSceneCoroutine(mainMenu_Name));
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        DataPersistanceManager persistence = DataPersistanceManager.instance;
        if (persistence)
            yield return persistence.SaveBeforeSceneChange();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            Debug.Log($"Loading progress: {operation.progress * 100}%");
            yield return null;
        }
    }

    private static bool TryBeginSceneChange()
    {
        DataPersistanceManager persistence = DataPersistanceManager.instance;
        return !persistence || persistence.TryBeginSceneChange();
    }
}

[Serializable]
public class DetectedBlockInfo
{
    public GameObject block;
    public Vector3 blockPosition;

    public BlockType blockType;
}
