using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMessageManager : Singleton<ButtonMessageManager>
{

    [Header("Button Messages")]
    [SerializeField] private GameObject buttonMessage_Parent;
    public ButtonMessageList buttonMessages;

    private PlayerManager playerManager;
    private AncientTablet activeTablet;
    private Interactable_NPC talkTarget;
    private CameraController talkCameraController;
    private CinemachineCamera talkCamera;
    private InputType talkPromptInput;
    private double lastTalkCheck = double.NegativeInfinity;

    private bool InteractionPromptsSuppressed =>
        activeTablet || (playerManager && playerManager.npcInteraction);

    //--------------------


    private void OnEnable()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        DataManager.Action_dataHasLoaded += UpdateButtonMessages;
        SettingsManager.Action_SetNewLanguage += UpdateButtonMessages;
        PauseMenuManager.Action_openPauseMenu += HidePromptsForPause;
        PauseMenuManager.Action_closePauseMenu += RestorePromptsAfterPause;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= UpdateButtonMessages;
        SettingsManager.Action_SetNewLanguage -= UpdateButtonMessages;
        PauseMenuManager.Action_openPauseMenu -= HidePromptsForPause;
        PauseMenuManager.Action_closePauseMenu -= RestorePromptsAfterPause;
        SetTalkTarget(null);
    }

    private void LateUpdate()
    {
        if (Time.fixedTimeAsDouble != lastTalkCheck &&
            (!ReferenceEquals(talkTarget, null) || Interactable_NPC_ColliderTrigger.HasOverlaps))
            RefreshTalkTarget();
    }

    internal void RefreshTalkTarget()
    {
        lastTalkCheck = Time.fixedTimeAsDouble;

        if (playerManager && playerManager.npcInteraction)
        {
            if (talkTarget)
                talkTarget.canInteract = false;

            // The dialogue's blend-out still needs the camera chosen at its start.
            HideTalkPrompt();
            return;
        }

        if (!isActiveAndEnabled || !playerManager || !playerManager.player || !playerManager.playerBody ||
            !Interactable_NPC_ColliderTrigger.HasOverlaps || playerManager.pauseGame || activeTablet ||
            Player_KeyInputs.Instance.freeCam_isPressed)
        {
            SetTalkTarget(null);
            return;
        }

        var movement = Movement.Instance;
        var ceilingGrab = Player_CeilingGrab.Instance;
        SetTalkTarget(movement.isRespawning || ceilingGrab.isCeilingGrabbing || ceilingGrab.isCeilingRotation
            ? null
            : Interactable_NPC_ColliderTrigger.FindTalkTarget(playerManager, movement, talkTarget));
    }

    private void SetTalkTarget(Interactable_NPC target)
    {
        bool targetChanged = !ReferenceEquals(talkTarget, target);
        if (targetChanged)
        {
            if (talkTarget)
                talkTarget.canInteract = false;
            if (talkCameraController && talkCameraController.CM_Other == talkCamera)
                talkCameraController.CM_Other = null;

            talkTarget = target;
            talkCameraController = target ? CameraController.Instance : null;
            talkCamera = target ? target.NPCVirtualCamera : null;
            if (talkCameraController)
                talkCameraController.CM_Other = talkCamera;
        }

        if (!target)
        {
            HideTalkPrompt();
            return;
        }

        target.canInteract = true;
        var prompt = buttonMessages?.buttonMessage_Talk;
        if (prompt?.buttonMessage_Parent)
        {
            InputType input = ControllerState.Instance.activeController;
            if (targetChanged || input != talkPromptInput || !prompt.buttonMessage_Parent.activeSelf)
            {
                talkPromptInput = input;
                SetButtonMessage(prompt);
                ShowButtonMessage(prompt);
            }
        }
    }

    private void HideTalkPrompt()
    {
        var prompt = buttonMessages?.buttonMessage_Talk;
        if (prompt?.buttonMessage_Parent && prompt.buttonMessage_Parent.activeSelf)
            HideButtonMessage(prompt);
    }


    //--------------------


    void UpdateButtonMessages()
    {
        buttonMessages.buttonMessage_Talk.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_Talk_Keyboard;
        buttonMessages.buttonMessage_PlaceGlueplant.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_PlaceGlueplant_Keyboard;
        buttonMessages.buttonMessage_Push.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_Push_Keyboard;
        buttonMessages.buttonMessage_Read.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_Read_Keyboard;

        SetAllButtonMessages();
    }


    //--------------------


    void SetAllButtonMessages()
    {
        SetButtonMessage(buttonMessages.buttonMessage_Talk);
        SetButtonMessage(buttonMessages.buttonMessage_PlaceGlueplant);
        SetButtonMessage(buttonMessages.buttonMessage_Push);
        SetButtonMessage(buttonMessages.buttonMessage_Read);
    }

    public void SetButtonMessage(ButtonMessage _buttonMessage)
    {
        _buttonMessage.buttonMessage_Text.text = SetButtonMessageText(_buttonMessage);
        _buttonMessage.buttonMessage_Image.sprite = SetButtonMessageSprite(_buttonMessage);
    }
    string SetButtonMessageText(ButtonMessage _buttonMessage)
    {
        return _buttonMessage.button_MessageText;
    }
    Sprite SetButtonMessageSprite(ButtonMessage _buttonMessage)
    {
        switch (ControllerState.Instance.activeController)
        {
            case InputType.Keyboard:
                return _buttonMessage.button_MessageSprite_Keyboard;
            case InputType.Xbox:
                return _buttonMessage.button_MessageSprite_PlayStation;
            case InputType.PlayStation:
                return _buttonMessage.button_MessageSprite_xBox;

            default:
                return null;
        }
    }


    //--------------------


    public void ShowButtonMessage(ButtonMessage _buttonMessage)
    {
        if (InteractionPromptsSuppressed &&
            (_buttonMessage == buttonMessages.buttonMessage_Read || _buttonMessage == buttonMessages.buttonMessage_Talk))
            return;

        _buttonMessage.buttonMessage_Parent.SetActive(true);
    }
    public void HideButtonMessage(ButtonMessage _buttonMessage)
    {
        if (_buttonMessage?.buttonMessage_Parent)
            _buttonMessage.buttonMessage_Parent.SetActive(false);
    }

    internal void HideInteractionPrompts()
    {
        HideButtonMessage(buttonMessages.buttonMessage_Read);
        HideButtonMessage(buttonMessages.buttonMessage_Talk);
    }

    private void HidePromptsForPause()
    {
        if (buttonMessage_Parent)
            buttonMessage_Parent.SetActive(false);
    }

    private void RestorePromptsAfterPause()
    {
        if (!buttonMessage_Parent)
            return;

        if (playerManager)
            RefreshReadPrompt(playerManager.block_LookingAt_Horizontal);
        buttonMessage_Parent.SetActive(true);
    }

    internal void BeginTabletInteraction(AncientTablet tablet)
    {
        activeTablet = tablet;
        HideInteractionPrompts();
    }

    internal void EndTabletInteraction(AncientTablet tablet)
    {
        if (activeTablet == tablet)
            activeTablet = null;
    }

    internal void RefreshReadPrompt(GameObject lookedAtObject)
    {
        ButtonMessage readPrompt = buttonMessages.buttonMessage_Read;
        if (InteractionPromptsSuppressed ||
            !lookedAtObject || !lookedAtObject.TryGetComponent(out BlockInfo blockInfo) ||
            blockInfo.blockElement != BlockElement.Tablet)
        {
            HideButtonMessage(readPrompt);
            return;
        }

        SetButtonMessage(readPrompt);
        ShowButtonMessage(readPrompt);
    }
}


[Serializable]
public class ButtonMessageList
{
    public ButtonMessage buttonMessage_Talk;
    public ButtonMessage buttonMessage_PlaceGlueplant;
    public ButtonMessage buttonMessage_Push;
    public ButtonMessage buttonMessage_Read;
}
[Serializable]
public class ButtonMessage
{
    [Header("Parents")]
    public GameObject buttonMessage_Parent;
    public TextMeshProUGUI buttonMessage_Text;
    public Image buttonMessage_Image;

    [Header("Message Text List in languages")]
    [HideInInspector] public string button_MessageText;

    [Header("Button Sprite")]
    public Sprite button_MessageSprite_Keyboard;
    public Sprite button_MessageSprite_PlayStation;
    public Sprite button_MessageSprite_xBox;
}
