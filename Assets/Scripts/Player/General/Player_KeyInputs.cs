using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_KeyInputs : Singleton<Player_KeyInputs>
{
    public static event Action Action_WalkButton_isPressed;
    public static event Action Action_WalkButton_isReleased;

    public static event Action Action_Ascend_isPressed;
    public static event Action Action_Descend_isPressed;
    public static event Action Action_CeilingGrab_isPressed;
    public static event Action Action_GrapplingHook_isPressed;

    public static event Action Action_dialogueButton_isPressed;
    public static event Action Action_dialogueNextButton_isPressed;
    public static event Action Action_InteractButton_isPressed;

    [Header("Input System")]
    public PlayerControls playerControls;
    MapManager mapManager;

    [Header("KeyPresses")]
    public bool forward_isPressed = false;
    public bool back_isPressed = false;
    public bool left_isPressed = false;
    public bool right_isPressed = false;

    public bool up_isPressed = false;
    public bool down_isPressed = false;

    public bool grapplingHook_isPressed = false;

    public bool cameraX_isPressed = false;
    public bool cameraY_isPressed = false;

    [Header("KeyPresses Respawn")]
    float holdDuration = 0.5f;
    [SerializeField] float holdtimer = 0;
    [SerializeField] bool useUnscaledTime = true; // ignore timescale (pause)
    Coroutine holdRoutine;

    [SerializeField] AudioSource playerAudioSource;
    [SerializeField] AudioClip respawnHoldSound;
    [SerializeField] AudioClip respawnCancelSound;
    [SerializeField] AudioClip respawnCompleteSound;
    [SerializeField] UnityEvent onHoldStarted;
    [SerializeField] UnityEvent onHoldCanceled;
    [SerializeField] UnityEvent onHoldCompleted;


    //--------------------


    private void Start()
    {
        playerControls = new PlayerControls();
        mapManager = FindObjectOfType<MapManager>();

        if (PlayerManager.Instance.playerBody.transform.GetComponentInChildren<Animator>())
        {
            Player_Animations.Instance.anim = PlayerManager.Instance.playerBody.GetComponentInChildren<Animator>();
        }
    }


    //--------------------


    void OnForward_Down()
    {
        if (!ButtonChecks_Movement()) { return; }

        forward_isPressed = true;
        Action_WalkButton_isPressed?.Invoke();
    }
    void OnForward_Up()
    {
        forward_isPressed = false;

        Action_WalkButton_isReleased?.Invoke();
    }
    void OnBackward_Down()
    {
        if (!ButtonChecks_Movement()) { return; }

        back_isPressed = true;
        Action_WalkButton_isPressed?.Invoke();
    }
    void OnBackward_Up()
    {
        back_isPressed = false;

        Action_WalkButton_isReleased?.Invoke();
    }
    void OnLeft_Down()
    {
        if (!ButtonChecks_Movement()) { return; }

        left_isPressed = true;
        Action_WalkButton_isPressed?.Invoke();
    }
    void OnLeft_Up()
    {
        left_isPressed = false;

        Action_WalkButton_isReleased?.Invoke();
    }
    void OnRight_Down()
    {
        if (!ButtonChecks_Movement()) { return; }

        right_isPressed = true;
        Action_WalkButton_isPressed?.Invoke();
    }
    void OnRight_Up()
    {
        right_isPressed = false;

        Action_WalkButton_isReleased?.Invoke();
    }

    void OnAbilityUp_Down()
    {
        if (!ButtonChecks_Movement() || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        up_isPressed = true;
        Action_Ascend_isPressed?.Invoke();
    }
    void OnAbilityUp_Up()
    {
        if (!ButtonChecks_Movement()) { return; }

        up_isPressed = false;
    }
    void OnAbilityDown_Down()
    {
        if (!ButtonChecks_Movement() || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }
        
        down_isPressed = true;
        Action_Descend_isPressed?.Invoke();

        //Player_Interact.Instance.InteractWithObject();
    }
    void OnAbilityDown_Up()
    {
        if (!ButtonChecks_Movement()) { return; }

        down_isPressed = false;

        //Player_Interact.Instance.InteractWithObject();
    }

    void OnDialogueSkip_Pressed()
    {
        if (!PlayerManager.Instance.npcInteraction) { return; }

        Action_dialogueButton_isPressed?.Invoke();
    }
    void OnDialogueNext_Pressed()
    {
        if (!PlayerManager.Instance.npcInteraction) { return; }

        Action_dialogueNextButton_isPressed?.Invoke();
    }
    void OnInteractButton_Pressed()
    {
        if (!ButtonChecks_Movement()) { return; }
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return; }

        Action_InteractButton_isPressed?.Invoke();
    }



    //--------------------


    void OnAbilityLeft()
    {
        if (!ButtonChecks_Other()) { return; }
        if (Movement.Instance.movementStates == MovementStates.Falling) { return; }

        Player_CeilingGrab.Instance.CeilingGrab();

        Action_CeilingGrab_isPressed?.Invoke();
    }

    void OnAbilityRight_DownPress()
    {
        if (!ButtonChecks_Other() || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        grapplingHook_isPressed = true;
    }
    void OnAbilityRight_RelesePress()
    {
        if (!ButtonChecks_Other()) { return; }

        grapplingHook_isPressed = false;
        Movement.Instance.UpdateGrapplingHookMovement_Release();

        Action_GrapplingHook_isPressed?.Invoke();
    }


    //--------------------


    void OnCameraRotateX()
    {
        if (!ButtonChecks_Other()) { return; }
        if (Movement.Instance.isUpdatingDarkenBlocks) { return; }

        MapManager.Instance.cameraRotated++;
        CameraController.Instance.RotateCameraX();
    }
    void OnCameraRotateY()
    {
        if (!ButtonChecks_Other()) { return; }
        if (Movement.Instance.isUpdatingDarkenBlocks) { return; }

        MapManager.Instance.cameraRotated++;
        CameraController.Instance.RotateCameraY();
    }


    //--------------------


    void OnRespawn_In()
    {
        if (!ButtonChecks_Other()) { return; }

        StartRespawnHold();
    }
    void OnRespawn_Out()
    {
        CancelRespawnHold();
    }


    //--------------------


    void OnQuit()
    {
        if (!ButtonChecks_Other()) { return; }

        Key_Quit();
    }

    bool ButtonChecks_Movement()
    {
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (PlayerManager.Instance.pauseGame) { return false; }
        if (PlayerManager.Instance.npcInteraction) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Player_CeilingGrab.Instance.isCeilingRotation) { return false; }

        return true;
    }
    bool ButtonChecks_Other()
    {
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (Player_CeilingGrab.Instance.isCeilingRotation) { return false; }
        if (PlayerManager.Instance.pauseGame) { return false; }
        if (PlayerManager.Instance.npcInteraction) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }

        if (Player_GraplingHook.Instance.isGrapplingHooking) { return false; }

        return true;
    }


    //--------------------


    void StartRespawnHold()
    {
        if (holdRoutine != null) StopCoroutine(holdRoutine);
        holdRoutine = StartCoroutine(HoldTimer());

        onHoldStarted?.Invoke();

        if (playerAudioSource != null)
        {
            playerAudioSource.loop = true;
            playerAudioSource.time = 0f;
            playerAudioSource.Play();
        }
    }

    void CancelRespawnHold()
    {
        if (holdRoutine != null)
        {
            StopCoroutine(holdRoutine);
            holdRoutine = null;
        }

        if (holdtimer < holdDuration)
        {
            if (playerAudioSource != null && playerAudioSource.isPlaying)
                playerAudioSource.Stop();

            onHoldCanceled?.Invoke();
        }

        holdtimer = 0;
    }
    private IEnumerator HoldTimer()
    {
        float t = 0f;
        while (t < holdDuration)
        {
            t += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            holdtimer += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            //onHoldProgress?.Invoke(Mathf.Clamp01(t / holdDuration));
            yield return null;
        }

        // finished
        holdRoutine = null;
        if (playerAudioSource != null && playerAudioSource.isPlaying) playerAudioSource.Stop();

        onHoldCompleted?.Invoke();
    }
    public void OnHoldStarted_Event()
    {
        playerAudioSource.clip = respawnHoldSound;
        playerAudioSource.loop = false;
        playerAudioSource.Play();
    }
    public void OnHoldCanceled_Event()
    {
        playerAudioSource.clip = respawnCancelSound;
        playerAudioSource.loop = false;
        playerAudioSource.Play();
    }
    public void OnHoldFinished_Event()
    {
        playerAudioSource.clip = respawnCompleteSound;
        playerAudioSource.loop = false;
        playerAudioSource.Play();

        Movement.Instance.RespawnPlayer();
    }


    //--------------------


    public void Key_Quit()
    {
        if (!ButtonChecks_Other()) { return; }

        RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement = PauseMenuManager.Instance.pauseMenu_StartButton;
        PauseMenuManager.Instance.OpenPauseMenu();
        PlayerManager.Instance.pauseGame = true;
    }
}
