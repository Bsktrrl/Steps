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
    RaycastHit hit;

    [Header("Game Paused")]
    public bool pauseGame;
    public bool npcInteraction;

    [Header("mainMenu_Name")]
    [SerializeField] string mainMenu_Name;

    #endregion


    //--------------------


    private void Update()
    {
        RespawnPlayerIfToLowInMapHeight();
    }

    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += LoadPlayerStats;
        Movement.Action_StepTaken += StepsOnFallableBlock;
        Movement.Action_StepTaken += BlockLookingAt;
        Movement.Action_BodyRotated += BlockLookingAt;

        Block_Moveable.Action_ObjectHasBeenPushed += BlockLookingAt;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= LoadPlayerStats;
        Movement.Action_StepTaken -= StepsOnFallableBlock;
        Movement.Action_StepTaken -= BlockLookingAt;
        Movement.Action_BodyRotated -= BlockLookingAt;

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


    void BlockLookingAt()
    {
        Vector3 origin = playerBody.transform.position;
        Vector3 direction = playerBody.transform.forward;

        if (Physics.Raycast(origin + (Vector3.up * 0.25f), direction, out hit, 1.1f))
        {
            if (hit.collider.TryGetComponent<BlockInfo>(out _))
            {
                if (block_LookingAt_Horizontal != hit.collider.gameObject)
                {
                    block_LookingAt_Horizontal = hit.collider.gameObject;
                    Action_UpdatedLookingAtHorizontal?.Invoke();
                }
            }
            else
            {
                block_LookingAt_Horizontal = null;
                Action_UpdatedLookingAtHorizontal?.Invoke();
            }
        }
        else
        {
            block_LookingAt_Horizontal = null;
            Action_UpdatedLookingAtHorizontal?.Invoke();
        }
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


    //--------------------


    public void QuitLevel()
    {
        if (!string.IsNullOrEmpty(mainMenu_Name))
        {
            StartCoroutine(LoadSceneCoroutine(mainMenu_Name));
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            Debug.Log($"Loading progress: {operation.progress * 100}%");
            yield return null;
        }
    }
}

[Serializable]
public class DetectedBlockInfo
{
    public GameObject block;
    public Vector3 blockPosition;

    public BlockType blockType;
}
