using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class AncientTablet : MonoBehaviour
{
    [Header("Tablet Text Entry")]
    [SerializeField] int textEntryNumber;

    [Header("Tablet Camera")]
    [SerializeField] CinemachineCamera tabletVirtualCamera;

    internal static AncientTablet ActiveTablet { get; private set; }

    bool tabletIsShowing;
    PlayerManager player;
    TabletManager tabletManager;
    CameraController cameraController;
    PrioritySettings playerCameraPriority;
    PrioritySettings tabletCameraPriority;
    float previousBlendTime;
    ButtonMessageManager interactionPrompts;
    RenderHiderOnContact renderHider;
    HoleShaderOnOffScript holeShader;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_StepTaken_Late += DisplayButtonMessageDelayed;
        Movement.Action_BodyRotated += DisplayButtonMessageDelayed;
        Movement.Action_RespawnPlayerLate += DisplayButtonMessageDelayed;
        Movement.Action_RespawnPlayerEarly += InterruptInteraction;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken_Late -= DisplayButtonMessageDelayed;
        Movement.Action_BodyRotated -= DisplayButtonMessageDelayed;
        Movement.Action_RespawnPlayerLate -= DisplayButtonMessageDelayed;
        Movement.Action_RespawnPlayerEarly -= InterruptInteraction;

        StopAllCoroutines();
        if (ActiveTablet == this)
            ReleaseInteraction(interrupted: true);
    }


    //--------------------


    internal void TryOpen(Player_KeyInputs inputs)
    {
        if (!isActiveAndEnabled || ActiveTablet || !tabletVirtualCamera || !tabletVirtualCamera.isActiveAndEnabled)
            return;

        Movement movement = Movement.Instance;
        if (movement.GetMovementState() != MovementStates.Still || movement.IsClimbingLadder ||
            Player_Animations.Instance.IsPickupAnimationPending || Player_Interact.Instance.isInteracting)
            return;

        player = PlayerManager.Instance;
        var freeCam = player.GetComponentInChildren<FreeCam>();
        var respawn = player.GetComponentInChildren<SFX_Respawn>();
        if (player.pauseMovement || player.npcInteraction || (freeCam && (freeCam._isActive || freeCam.IsReturningToPlayer)) ||
            (respawn && respawn.isRespawning))
            return;

        cameraController = CameraController.Instance;
        tabletManager = FindAnyObjectByType<TabletManager>();
        if (!cameraController.CM_Player || !cameraController.CM_Brain || cameraController.isCeilingRotating ||
            !tabletManager || !tabletManager.tabletUI_Parent || !tabletManager.tabletUI_Text ||
            !tabletManager.TryGetReadText(textEntryNumber, out string text))
            return;

        playerCameraPriority = cameraController.CM_Player.Priority;
        tabletCameraPriority = tabletVirtualCamera.Priority;
        previousBlendTime = cameraController.CM_Brain.DefaultBlend.Time;
        interactionPrompts = FindAnyObjectByType<ButtonMessageManager>();
        renderHider = FindAnyObjectByType<RenderHiderOnContact>();
        holeShader = FindAnyObjectByType<HoleShaderOnOffScript>();

        ActiveTablet = this;
        player.PauseMovement();
        inputs.ClearCutsceneInput();
        if (interactionPrompts)
            interactionPrompts.BeginTabletInteraction(this);

        StartCoroutine(OpenTablet(text));
    }

    IEnumerator OpenTablet(string text)
    {
        if (renderHider)
            renderHider.FreeCamOn();
        if (holeShader)
            holeShader.HoleShader_Off();

        tabletManager.HideHUDUI(tabletManager.hud_Parent);
        yield return cameraController.StartVirtualCameraBlend_In(tabletVirtualCamera);

        if (!tabletManager || !tabletManager.tabletUI_Text || !tabletManager.tabletUI_Parent)
        {
            ReleaseInteraction(interrupted: true);
            yield break;
        }

        tabletManager.tabletUI_Text.text = text;
        tabletManager.ShowTabletUI(tabletManager.tabletUI_Parent);
        Player_Statistics_Manager.Active?.RecordTabletRead(textEntryNumber, transform.position);

        yield return new WaitForSeconds(tabletManager.fadeDuration);
        tabletIsShowing = true;
    }

    internal void CloseIfReady()
    {
        if (ActiveTablet != this || !tabletIsShowing)
            return;

        tabletIsShowing = false;
        StartCoroutine(CloseTablet());
    }

    IEnumerator CloseTablet()
    {
        if (!tabletManager)
        {
            ReleaseInteraction(interrupted: true);
            yield break;
        }

        tabletManager.HideTabletUI(tabletManager.tabletUI_Parent);
        yield return new WaitForSeconds(tabletManager.fadeDuration);
        yield return cameraController.StartVirtualCameraBlend_Out(tabletVirtualCamera);

        if (tabletManager)
            tabletManager.ShowHUDUI(tabletManager.hud_Parent);
        ReleaseInteraction(interrupted: false);
        DisplayButtonMessage();
    }

    void InterruptInteraction()
    {
        if (ActiveTablet != this)
            return;

        StopAllCoroutines();
        ReleaseInteraction(interrupted: true);
    }

    void ReleaseInteraction(bool interrupted)
    {
        if (interrupted && tabletManager)
            tabletManager.RestoreGameplayUI();

        if (tabletVirtualCamera)
            tabletVirtualCamera.Priority = tabletCameraPriority;
        if (cameraController)
        {
            if (cameraController.CM_Player)
            {
                cameraController.CM_Player.Priority = playerCameraPriority;
                cameraController.CM_Player.Prioritize();
            }
            if (cameraController.CM_Brain)
                cameraController.CM_Brain.DefaultBlend.Time = previousBlendTime;
        }

        tabletIsShowing = false;
        ActiveTablet = null;
        if (renderHider)
            renderHider.FreeCamOff();
        if (player)
            player.UnpauseMovement();
        if (interactionPrompts)
            interactionPrompts.EndTabletInteraction(this);
    }


    //--------------------


    void DisplayButtonMessageDelayed()
    {
        StartCoroutine(DisplayButtonMessageAfterFrame());
    }

    IEnumerator DisplayButtonMessageAfterFrame()
    {
        yield return null;
        DisplayButtonMessage();
    }
    void DisplayButtonMessage()
    {
        ButtonMessageManager.Instance.RefreshReadPrompt(PlayerManager.Instance.block_LookingAt_Horizontal);
    }
}
