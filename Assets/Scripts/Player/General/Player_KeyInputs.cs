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

    public static event Action Action_RespawnHold;
    public static event Action Action_RespawnCanceled;

    public static event Action Action_FreeCamIsActive;
    public static event Action Action_FreeCamIsPassive;

    public static event Action Action_PauseMenuIsPressed;

    [Header("Input System")]
    public PlayerControls playerControls;
    InputAction[] movementPressActions;
    MapManager mapManager;
    MapStatsGathered mapStatsGathered;

    [Header("KeyPresses")]
    public bool forward_isPressed = false;
    public bool back_isPressed = false;
    public bool left_isPressed = false;
    public bool right_isPressed = false;

    public bool forward_isHold = false;
    public bool back_isHold = false;
    public bool left_isHold = false;
    public bool right_isHold = false;

    public bool up_isPressed = false;
    public bool down_isPressed = false;

    public bool grapplingHook_isPressed = false;

    public bool cameraX_isPressed = false;
    public bool cameraY_isPressed = false;

    public bool respawn_isPressed = false;

    public bool freeCam_isPressed = false;

    PauseMenuManager pauseMenuManager;

    public bool tutorialMovementBlocker;

    internal bool IsMovementInputHeld
    {
        get
        {
            if (movementPressActions == null)
                return forward_isPressed || back_isPressed || left_isPressed || right_isPressed ||
                    up_isPressed || down_isPressed || grapplingHook_isPressed;

            foreach (InputAction action in movementPressActions)
                if (action?.IsPressed() == true)
                    return true;

            return false;
        }
    }

    private bool GameplayInputLocked => MapManager.Instance == null ||
        MapManager.Instance.GameplayInputLocked;

    private bool DialogueCameraAllowed => !DataPersistanceManager.GameplayInputBlocked &&
        !AncientTablet.ActiveTablet && MapManager.Instance != null &&
        !MapManager.Instance.IntroInputLocked &&
        MapManager.Instance.levelName == LevelNames.HUB &&
        HUBTutorial.Instance != null && HUBTutorial.Instance.DialogueCameraAllowed;

    public void ClearCutsceneInput()
    {
        ClearMovementInput();
    }

    private void ClearMovementInput()
    {
        if (Movement.Instance != null)
            Movement.Instance.CancelCutsceneGrapplingTarget();

        forward_isPressed = back_isPressed = left_isPressed = right_isPressed = false;
        forward_isHold = back_isHold = left_isHold = right_isHold = false;
        up_isPressed = down_isPressed = grapplingHook_isPressed = false;

        if (respawn_isPressed)
        {
            respawn_isPressed = false;
            Action_RespawnCanceled?.Invoke();
        }
    }



    //--------------------


    private void Start()
    {
        pauseMenuManager = FindAnyObjectByType<PauseMenuManager>();
        mapStatsGathered = FindAnyObjectByType<MapStatsGathered>();

        playerControls = new PlayerControls();
        mapManager = FindObjectOfType<MapManager>();

        // Use the live actions because gameplay can clear its movement flags while a button is held.
        if (TryGetComponent(out PlayerInput playerInput) &&
            playerInput.actions?.FindActionMap("PlayerMovement") is InputActionMap movement)
        {
            movementPressActions = new[]
            {
                movement.FindAction("Forward_Down"),
                movement.FindAction("Backward_Down"),
                movement.FindAction("Left_Down"),
                movement.FindAction("Right_Down"),
                movement.FindAction("AbilityUp_Down"),
                movement.FindAction("AbilityDown_Down"),
                movement.FindAction("AbilityLeft"),
                movement.FindAction("AbilityRight_DownPress")
            };
        }

        if (PlayerManager.Instance.playerBody.transform.GetComponentInChildren<Animator>())
        {
            Player_Animations.Instance.playerAnimator = PlayerManager.Instance.playerBody.GetComponentInChildren<Animator>();
        }
    }
    private void Update()
    {
        if (freeCam_isPressed)
        {
            mapStatsGathered.levelStats.freeCamTimer += Time.deltaTime;
        }
    }
    private void OnEnable()
    {
        tutorialMovementBlocker = false;
    }


    //--------------------


    void OnForward_Down()
    {
        if (GameplayInputLocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        Run_Movement_Tutorial(ref forward_isPressed);
        Run_DemoMessage_Tutorial_End();

        if (Tutorial.Instance.tutorial_isRunning && !tutorialMovementBlocker)
        {
            StartCoroutine(TutorialMovement_Delay());
        }

        if (!ButtonChecks_Movement()) return;
        if (PlayerManager.Instance.pauseMovement) return;

        forward_isPressed = true;
        Action_WalkButton_isPressed?.Invoke();
    }
    void OnForward_Hold()
    {
        if (!forward_isPressed || !ButtonChecks_Movement()) return;
        if (PlayerManager.Instance.pauseMovement) return;

        if (!Tutorial.Instance.tutorial_isRunning && Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement != BlockElement.Ice)
            forward_isHold = true;
    }
    void OnForward_Up()
    {
        forward_isPressed = false;
        forward_isHold = false;

        if (!freeCam_isPressed)
            Action_WalkButton_isReleased?.Invoke();
    }

    void OnBackward_Down()
    {
        if (GameplayInputLocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        Run_Movement_Tutorial(ref back_isPressed);
        Run_DemoMessage_Tutorial_End();

        if (Tutorial.Instance.tutorial_isRunning && !tutorialMovementBlocker)
        {
            StartCoroutine(TutorialMovement_Delay());
        }

        if (!ButtonChecks_Movement()) { return; }
        if (PlayerManager.Instance.pauseMovement) return;

        back_isPressed = true;
        Action_WalkButton_isPressed?.Invoke();
    }
    void OnBackward_Hold()
    {
        if (!back_isPressed || !ButtonChecks_Movement()) return;
        if (PlayerManager.Instance.pauseMovement) return;

        if (!Tutorial.Instance.tutorial_isRunning && Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement != BlockElement.Ice)
            back_isHold = true;
    }
    void OnBackward_Up()
    {
        back_isPressed = false;
        back_isHold = false;

        if (!freeCam_isPressed)
            Action_WalkButton_isReleased?.Invoke();
    }

    void OnLeft_Down()
    {
        if (GameplayInputLocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        Run_Movement_Tutorial(ref left_isPressed);
        Run_DemoMessage_Tutorial_End();

        if (Tutorial.Instance.tutorial_isRunning && !tutorialMovementBlocker)
        {
            StartCoroutine(TutorialMovement_Delay());
        }

        if (!ButtonChecks_Movement()) { return; }
        if (PlayerManager.Instance.pauseMovement) return;

        left_isPressed = true;
        Action_WalkButton_isPressed?.Invoke();
    }
    void OnLeft_Hold()
    {
        if (!left_isPressed || !ButtonChecks_Movement()) return;
        if (PlayerManager.Instance.pauseMovement) return;

        if (!Tutorial.Instance.tutorial_isRunning && Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement != BlockElement.Ice)
            left_isHold = true;
    }
    void OnLeft_Up()
    {
        left_isPressed = false;
        left_isHold = false;

        if (!freeCam_isPressed)
            Action_WalkButton_isReleased?.Invoke();
    }

    void OnRight_Down()
    {
        if (GameplayInputLocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        Run_Movement_Tutorial(ref right_isPressed);
        Run_DemoMessage_Tutorial_End();

        if (Tutorial.Instance.tutorial_isRunning && !tutorialMovementBlocker)
        {
            StartCoroutine(TutorialMovement_Delay());
        }

        if (!ButtonChecks_Movement()) { return; }
        if (PlayerManager.Instance.pauseMovement) return;

        right_isPressed = true;
        Action_WalkButton_isPressed?.Invoke();
    }
    void OnRight_Hold()
    {
        if (!right_isPressed || !ButtonChecks_Movement()) return;
        if (PlayerManager.Instance.pauseMovement) return;

        if (!Tutorial.Instance.tutorial_isRunning && Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement != BlockElement.Ice)
            right_isHold = true;
    }
    void OnRight_Up()
    {
        right_isPressed = false;
        right_isHold = false;

        if (!freeCam_isPressed)
            Action_WalkButton_isReleased?.Invoke();
    }


    //--------------------


    void OnAbilityUp_Down()
    {
        if (GameplayInputLocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!ButtonChecks_Movement() || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }
        if (PlayerManager.Instance.pauseMovement) return;

        if (Movement.Instance.isGrapplingHooking) return;
        if (Movement.Instance.isJumping) return;
        if (Movement.Instance.isDashing) return;
        if (Movement.Instance.isSwiftSwim) return;
        if (Movement.Instance.isAscending) return;
        if (Movement.Instance.isDescending) return;
        if (grapplingHook_isPressed) return;

        //print("1. OnAbilityUp_Down");

        up_isPressed = true;

        //if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement != BlockElement.Water)
        Action_Ascend_isPressed?.Invoke();
    }
    void OnAbilityUp_Up()
    {
        up_isPressed = false;
    }

    void OnAbilityDown_Down()
    {
        if (GameplayInputLocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!ButtonChecks_Movement() || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }
        if (PlayerManager.Instance.pauseMovement) return;

        if (Movement.Instance.isGrapplingHooking) return;
        if (Movement.Instance.isJumping) return;
        if (Movement.Instance.isDashing) return;
        if (Movement.Instance.isSwiftSwim) return;
        if (Movement.Instance.isAscending) return;
        if (Movement.Instance.isDescending) return;
        if (grapplingHook_isPressed) return;

        //print("2. OnAbilityDown_Down");

        down_isPressed = true;

        //if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement != BlockElement.Water)
        Action_Descend_isPressed?.Invoke();
    }
    void OnAbilityDown_Up()
    {
        down_isPressed = false;
    }

    void OnAbilityLeft()
    {
        if (GameplayInputLocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!ButtonChecks_Other()) { return; }
        if (Movement.Instance.movementStates == MovementStates.Falling) { return; }

        if (PlayerManager.Instance.pauseMovement) return;

        if (Movement.Instance.isGrapplingHooking) return;
        if (Movement.Instance.isJumping) return;
        if (Movement.Instance.isDashing) return;
        if (Movement.Instance.isSwiftSwim) return;
        if (Movement.Instance.isAscending) return;
        if (Movement.Instance.isDescending) return;
        if (grapplingHook_isPressed) return;

        print("3. OnAbilityLeft");

        Player_CeilingGrab.Instance.CeilingGrab();

        Action_CeilingGrab_isPressed?.Invoke();
    }

    void OnAbilityRight_DownPress()
    {
        if (grapplingHook_isPressed) return;
        if (GameplayInputLocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!ButtonChecks_Other() || Player_CeilingGrab.Instance.isCeilingGrabbing) return;
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook && !PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook) return;

        if (PlayerManager.Instance.pauseMovement) return;

        if (Movement.Instance.isJumping) return;
        if (Movement.Instance.isDashing) return;
        if (Movement.Instance.isSwiftSwim) return;
        if (Movement.Instance.isAscending) return;
        if (Movement.Instance.isDescending) return;

        print("4. OnAbilityRight_DownPress");

        grapplingHook_isPressed = true;

        Movement.Instance.Action_isGrapplingHooking_Invoke();

        Player_Animations.Instance.Trigger_GrapplingHookAnimation();
    }
    void OnAbilityRight_RelesePress()
    {
        if (!grapplingHook_isPressed)
            return;

        Movement movement = Movement.Instance;
        if (GameplayInputLocked || !Run_StayingPopupDisplayExit() || freeCam_isPressed ||
            PlayerManager.Instance.pauseMovement || !ButtonChecks_Other_MinusGrapplingHook())
        {
            movement.CancelGrapplingCharge();
            return;
        }

        movement.UpdateGrapplingHookMovement_Release();
    }


    //--------------------


    void OnDialogueSkip_Pressed()
    {
        if (DataPersistanceManager.GameplayInputBlocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!PlayerManager.Instance.npcInteraction) { return; }

        Action_dialogueButton_isPressed?.Invoke();
    }
    void OnDialogueNext_Pressed()
    {
        if (DataPersistanceManager.GameplayInputBlocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!PlayerManager.Instance.npcInteraction) { return; }

        Action_dialogueNextButton_isPressed?.Invoke();
    }
    void OnInteractButton_Pressed()
    {
        if (DataPersistanceManager.GameplayInputBlocked) return;

        AncientTablet openTablet = AncientTablet.ActiveTablet;
        if (openTablet)
        {
            openTablet.CloseIfReady();
            return;
        }

        if (GameplayInputLocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!ButtonChecks_Movement()) { return; }
        if (Movement.Instance.GetMovementState() is MovementStates.Moving or MovementStates.Falling) { return; }

        GameObject lookedAt = PlayerManager.Instance.block_LookingAt_Horizontal;
        if (lookedAt && lookedAt.TryGetComponent(out AncientTablet tablet))
        {
            tablet.TryOpen(this);
            return;
        }

        ButtonMessageManager.Instance.RefreshTalkTarget();
        Action_InteractButton_isPressed?.Invoke();
    }


    //--------------------


    void OnCameraRotateX()
    {
        if (GameplayInputLocked && !DialogueCameraAllowed) return;
        if (!Player_Animations.Instance.IsPickupAnimationPending && !Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!DialogueCameraAllowed)
        {
            Run_CameraRotation_Tutorial(true);
            Run_DemoMessage_Tutorial_End();
        }

        if (!ButtonChecks_CameraRotation()) { return; }
        if (Movement.Instance.isUpdatingDarkenBlocks) { return; }

        MapStatsGathered.Instance.levelStats.cameraRotationTaken++;
        CameraController.Instance.RotateCameraX();
    }
    void OnCameraRotateY()
    {
        if (GameplayInputLocked && !DialogueCameraAllowed) return;
        if (!Player_Animations.Instance.IsPickupAnimationPending && !Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!DialogueCameraAllowed)
        {
            Run_CameraRotation_Tutorial(false);
            Run_DemoMessage_Tutorial_End();
        }

        if (!ButtonChecks_CameraRotation()) { return; }
        if (Movement.Instance.isUpdatingDarkenBlocks) { return; }

        MapStatsGathered.Instance.levelStats.cameraRotationTaken++;
        CameraController.Instance.RotateCameraY();
    }
    bool ButtonChecks_CameraRotation()
    {
        bool dialogueCameraAllowed = DialogueCameraAllowed;
        bool pickupAnimationPending = Player_Animations.Instance.IsPickupAnimationPending;
        bool climbingLadder = Movement.Instance.IsClimbingLadder;
        if (GameplayInputLocked && !dialogueCameraAllowed) return false;
        if (!Run_IntroSequance()) return false;
        if (Movement.Instance.isRespawning || (pauseMenuManager && pauseMenuManager.isActive)) return false;

        if (CameraController.Instance.IsMovementBlockingRotation) return false;
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (Player_CeilingGrab.Instance.isCeilingRotation) { return false; }

        // IMPORTANT:
        // Stepellier dialogue allows normal camera rotation, not gameplay input.
        if (PlayerManager.Instance.pauseGame && !CameraController.Instance.isRotating &&
            !dialogueCameraAllowed && !pickupAnimationPending && !climbingLadder) return false;

        if (PlayerManager.Instance.npcInteraction && !dialogueCameraAllowed) { return false; }

        // IMPORTANT:
        // Do NOT block when camera is rotating, because that is exactly when we want buffering/reversing.
        // if (CameraController.Instance.isRotating) { return false; }

        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Tutorial.Instance.tutorial_isRunning && !dialogueCameraAllowed) { return false; }
        if (PopUpManager.Instance.ability_Active && !pickupAnimationPending) return false;
        if (PopUpManager.Instance.oneTimer_Active) { return false; }
        if (PopUpManager.Instance.ability_CanBeClosed && !pickupAnimationPending) return false;
        if (MapManager.Instance.introSequence) { return false; }

        if (Movement.Instance.isGrapplingHooking) { return false; }

        // Prevent camera rotation while actual pause menu is open.
        if (pauseMenuManager && pauseMenuManager.pauseMenu_MainMenu_Parent.activeInHierarchy) { return false; }

        Movement.Instance.CancelGrapplingCharge();

        return true;
    }


    //--------------------


    void OnRespawn_In()
    {
        if (GameplayInputLocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (freeCam_isPressed) return;

        Run_Respawn_Tutorial();

        if (!ButtonChecks_Other()) { return; }

        respawn_isPressed = true;
        Action_RespawnHold?.Invoke();
    }
    void OnRespawn_Out()
    {
        if (AncientTablet.ActiveTablet || freeCam_isPressed) return;

        respawn_isPressed = false;
        Action_RespawnCanceled?.Invoke();
    }


    //--------------------


    void OnFreeCam()
    {
        if (GameplayInputLocked) return;
        if (!Run_StayingPopupDisplayExit()) return;
        if (Tutorial.Instance.state_Movement || Tutorial.Instance.state_CameraRotation || Tutorial.Instance.state_Respawn) return;
        if (PopUpManager.Instance.ability_Active || PopUpManager.Instance.oneTimer_Active || PopUpManager.Instance.ability_CanBeClosed) return;

        if (Movement.Instance.GetMovementState() == MovementStates.Moving) return;
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) return;

        if (Player_CeilingGrab.Instance.isCeilingRotation) return;
        if (PlayerManager.Instance.npcInteraction) return;
        if (CameraController.Instance.isRotating) return;
        if (Player_Interact.Instance.isInteracting) return;
        if (PopUpManager.Instance.ability_Active) return;
        if (PopUpManager.Instance.oneTimer_Active) return;
        if (PopUpManager.Instance.ability_CanBeClosed) return;
        if (MapManager.Instance.introSequence) return;

        if (Movement.Instance.isGrapplingHooking) return;

        if (pauseMenuManager && pauseMenuManager.isActive) return;


        if (freeCam_isPressed)
        {
            if (Tutorial.Instance.tutorial_isRunning && !Tutorial.Instance.state_FreeCam_2) return;

            PlayerManager.Instance.UnpauseGame();
            Run_FreeCam_2_Tutorial_End();

            freeCam_isPressed = false;

            Action_FreeCamIsPassive?.Invoke();
        }
        else
        {
            if (Tutorial.Instance.tutorial_isRunning && !Tutorial.Instance.state_FreeCam_1) return;

            freeCam_isPressed = true;
            PlayerManager.Instance.PauseGame();
            ClearMovementInput();

            Run_FreeCam_1_Tutorial_End();

            Action_FreeCamIsActive?.Invoke();
        }
    }

    void OnHintButton()
    {
        print("1. OnHintButton");

        if (!mapManager || mapManager.GameplayInputLocked || (pauseMenuManager && pauseMenuManager.isActive))
            return;

        print("2. OnHintButton");
        if (mapManager.TryGetComponent(out HintManager hints))
        {
            print("3. OnHintButton");
            hints.PerformHint();
        }
    }

    //--------------------


    void OnQuit()
    {
        if (GameplayInputLocked) return;
        if (!Run_StayingPopupDisplayExit()) return;

        Key_Quit();
    }

    bool ButtonChecks_Movement()
    {
        if (GameplayInputLocked) return false;
        if (Movement.Instance != null && (Movement.Instance.isRespawning || Movement.Instance.isGrapplingHooking)) return false;
        if (!Run_IntroSequance()) return false;

        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (PlayerManager.Instance.pauseGame) { return false; }
        if (PlayerManager.Instance.npcInteraction) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Player_CeilingGrab.Instance.isCeilingRotation) { return false; }
        if (Tutorial.Instance.tutorial_isRunning) { return false; }
        if (PopUpManager.Instance.ability_Active) { return false; }
        if (PopUpManager.Instance.oneTimer_Active) { return false; }
        if (PopUpManager.Instance.ability_CanBeClosed) { return false; }
        if (MapManager.Instance.introSequence) { return false; }

        Movement.Instance.CancelGrapplingCharge();

        return true;
    }
    bool ButtonChecks_Other(bool allowFreeCamPause = false)
    {
        if (GameplayInputLocked) return false;
        if (Movement.Instance != null && Movement.Instance.isRespawning) return false;
        if (!Run_IntroSequance()) return false;

        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (Player_CeilingGrab.Instance.isCeilingRotation) { return false; }
        if (PlayerManager.Instance.pauseGame && !allowFreeCamPause) { return false; }
        if (PlayerManager.Instance.npcInteraction) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Tutorial.Instance.tutorial_isRunning && !allowFreeCamPause) { return false; }
        if (PopUpManager.Instance.ability_Active) { return false; }
        if (PopUpManager.Instance.oneTimer_Active) { return false; }
        if (PopUpManager.Instance.ability_CanBeClosed) { return false; }
        if (MapManager.Instance.introSequence) { return false; }

        if (Movement.Instance.isGrapplingHooking) { return false; }

        Movement.Instance.CancelGrapplingCharge();

        return true;
    }
    bool ButtonChecks_Other_MinusGrapplingHook()
    {
        if (GameplayInputLocked) return false;
        if (Movement.Instance != null && Movement.Instance.isRespawning) return false;
        if (!Run_IntroSequance()) return false;

        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (Player_CeilingGrab.Instance.isCeilingRotation) { return false; }
        if (PlayerManager.Instance.pauseGame) { return false; }
        if (PlayerManager.Instance.npcInteraction) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Tutorial.Instance.tutorial_isRunning) { return false; }
        if (PopUpManager.Instance.ability_Active) { return false; }
        if (PopUpManager.Instance.oneTimer_Active) { return false; }
        if (PopUpManager.Instance.ability_CanBeClosed) { return false; }
        if (MapManager.Instance.introSequence) { return false; }

        return true;
    }
    bool ButtonChecks_Abilities()
    {
        if (GameplayInputLocked) return false;
        if (Movement.Instance != null && Movement.Instance.isRespawning) return false;
        if (!Run_IntroSequance()) return false;

        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (Movement.Instance.isJumping) { return false; }
        if (Movement.Instance.isGrapplingHooking) { return false; }
        if (Movement.Instance.isDashing) { return false; }
        if (Movement.Instance.isSwiftSwim) { return false; }
        if (Movement.Instance.isAscending) { return false; }
        if (Movement.Instance.isDescending) { return false; }
        if (MapManager.Instance.introSequence) { return false; }

        return true;
    }


    //--------------------


    public void Key_Quit()
    {
        if (pauseMenuManager && pauseMenuManager.isActive) return;
        if (!ButtonChecks_Other(allowFreeCamPause: freeCam_isPressed)) return;

        RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement = PauseMenuManager.Instance.pauseMenu_StartButton;
        PlayerManager.Instance.pauseGame = true;
        PauseMenuManager.Instance.OpenPauseMenu();

        Action_PauseMenuIsPressed?.Invoke();
    }


    //--------------------


    //Into Sequence
    bool Run_IntroSequance()
    {
        if (MapManager.Instance.introSequence_Finished)
            return true;
        else
            return false;
    }



    //--------------------


    //Tutorial
    void Run_Movement_Tutorial(ref bool button_isPressed)
    {
        if (Tutorial.Instance.state_Movement)
        {
            button_isPressed = true;
            Action_WalkButton_isPressed?.Invoke();

            Tutorial.Instance.Tutorial_Movement(false);
        }
    }
    void Run_CameraRotation_Tutorial(bool cameraX)
    {
        if (Tutorial.Instance.state_CameraRotation)
        {
            MapStatsGathered.Instance.levelStats.cameraRotationTaken++;

            if (cameraX)
                CameraController.Instance.RotateCameraX();
            else
                CameraController.Instance.RotateCameraY();

            Tutorial.Instance.Tutorial_CameraRotation(false);
        }
    }
    void Run_Respawn_Tutorial()
    {
        if (Tutorial.Instance.state_Respawn)
        {
            respawn_isPressed = true;
            Action_RespawnHold?.Invoke();
        }
    }

    void Run_FreeCam_1_Tutorial_End()
    {
        if (Tutorial.Instance.state_FreeCam_1 && freeCam_isPressed)
        {
            Tutorial.Instance.Tutorial_FreeCam_1(false);
        }
    }
    void Run_FreeCam_2_Tutorial_End()
    {
        if (Tutorial.Instance.state_FreeCam_2 && freeCam_isPressed)
        {
            Tutorial.Instance.Tutorial_FreeCam_2(false);
        }
    }

    void Run_DemoMessage_Tutorial_End()
    {
        if (Tutorial.Instance.state_DemoMessage)
        {
            Tutorial.Instance.Tutorial_DemoMessage(false);
        }
    }


    //--------------------


    IEnumerator TutorialMovement_Delay()
    {
        yield return null;

        tutorialMovementBlocker = true;
    }


    //Ability
    bool Run_StayingPopupDisplayExit()
    {
        if (PopUpManager.Instance.ability_CanBeClosed)
        {
            PopUpManager.Instance.HideAbilityPopup();

            return false;
        }

        if (PopUpManager.Instance.oneTimer_CanBeClosed)
        {
            PopUpManager.Instance.HideOneTimerPopup();

            return false;
        }

        return true;
    }
}
