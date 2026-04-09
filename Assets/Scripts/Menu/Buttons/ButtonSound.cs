using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public static event Action Action_FirstButtonMuted_On;
    public static event Action Action_FirstButtonMuted_Off;

    public static event Action Action_Start_Off;

    public static event Action Action_Button_MenuTransition_Forward;
    public static event Action Action_Button_MenuTransition_Back;
    public static event Action Action_Button_Navigate;
    public static event Action Action_Button_PressSound;
    public static event Action Action_Button_BackSound;
    public static event Action Action_Button_Cannot;
    public static event Action Action_Button_Buy;
    public static event Action Action_Button_Equip_On;
    public static event Action Action_Button_Equip_Off;


    //-----


    [Header("Button States")]
    public bool isHighlighted;
    [SerializeField] bool isFirstMuted;
    [SerializeField] bool isBackButtonAndInSameMenu;

    bool isStart;

    [Header("Pressing Button")]
    public ButtonSoundStates buttonPress_Sound;

    [Header("Go Back Button")]
    public ButtonSoundStates buttonBack_Sound;


    //--------------------


    private void Start()
    {
        if (!SoundManager.Instance.pauseMenu)
            isStart = true;
    }


    //--------------------


    private void OnEnable()
    {
        Button_ToPress.Action_ButtonIsPressed += TryButtonPressSound;
        CancelPauseMenuByButtonPress.Action_BackButton_IsPressed += TryBackButtonSound;
        RegionButton.Action_ButtonIsPressed += TryButtonPressSound;
        SkinWardrobeButton.Action_ButtonIsPressed += TryButtonPressSound;

        isFirstMuted = true;
        Action_FirstButtonMuted_On += TurnOnMuted;
        Action_FirstButtonMuted_Off += TurnOffMuted;

        Action_Start_Off += TurnOffStart;

        StartCoroutine(TurnOffMuted_Delay());
    }
    private void OnDisable()
    {
        Button_ToPress.Action_ButtonIsPressed -= TryButtonPressSound;
        CancelPauseMenuByButtonPress.Action_BackButton_IsPressed -= TryBackButtonSound;
        RegionButton.Action_ButtonIsPressed -= TryButtonPressSound;
        SkinWardrobeButton.Action_ButtonIsPressed -= TryButtonPressSound;

        isHighlighted = false;
        Action_FirstButtonMuted_On -= TurnOnMuted;
        Action_FirstButtonMuted_Off -= TurnOffMuted;

        Action_Start_Off -= TurnOffStart;
    }

    IEnumerator TurnOffMuted_Delay()
    {
        yield return new WaitForEndOfFrame();

        Action_FirstButtonMuted_Off?.Invoke();
    }


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        TryHighlight();
    }

    public void OnSelect(BaseEventData eventData)
    {
        TryHighlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHighlighted = false;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isHighlighted = false;
    }


    //--------------------


    private void TryHighlight()
    {
        if (isHighlighted) return;

        isHighlighted = true;

        if (isFirstMuted)
        { 
            Action_FirstButtonMuted_Off?.Invoke();
            return; 
        }

        Action_Button_Navigate?.Invoke();
    }

    public void TryButtonPressSound()
    {
        if (isStart)
        {
            isHighlighted = true;
            Action_Start_Off?.Invoke();
        }

        if (!isHighlighted) return;

        switch (buttonPress_Sound)
        {
            case ButtonSoundStates.None:
                break;

            case ButtonSoundStates.MenuTransition_Forward:
                Action_Button_MenuTransition_Forward?.Invoke();
                break;
            case ButtonSoundStates.MenuTransition_Back:
                Action_Button_MenuTransition_Back?.Invoke();
                break;
            case ButtonSoundStates.ButtonNavigate:
                Action_Button_Navigate?.Invoke();
                break;
            case ButtonSoundStates.ButtonPress:
                Action_Button_PressSound?.Invoke();
                break;
            case ButtonSoundStates.ButtonBack:
                Action_Button_BackSound?.Invoke();
                break;
            case ButtonSoundStates.ButtonCannot:
                Action_Button_Cannot?.Invoke();
                break;
            case ButtonSoundStates.ButtonBuy:
                Action_Button_Buy?.Invoke();
                break;
            case ButtonSoundStates.ButtonEquip_On:
                Action_Button_Equip_On?.Invoke();
                break;
            case ButtonSoundStates.ButtonEquip_Off:
                Action_Button_Equip_Off?.Invoke();
                break;

            default:
                break;
        }

        if (isBackButtonAndInSameMenu)
            Action_FirstButtonMuted_On?.Invoke();
    }
    void TryBackButtonSound()
    {
        if (!isHighlighted) return;

        switch (buttonBack_Sound)
        {
            case ButtonSoundStates.None:
                break;

            case ButtonSoundStates.MenuTransition_Forward:
                Action_Button_MenuTransition_Forward?.Invoke();
                break;
            case ButtonSoundStates.MenuTransition_Back:
                Action_Button_MenuTransition_Back?.Invoke();
                break;
            case ButtonSoundStates.ButtonNavigate:
                Action_Button_Navigate?.Invoke();
                break;
            case ButtonSoundStates.ButtonPress:
                Action_Button_PressSound?.Invoke();
                break;
            case ButtonSoundStates.ButtonBack:
                Action_Button_BackSound?.Invoke();
                break;
            case ButtonSoundStates.ButtonCannot:
                Action_Button_Cannot?.Invoke();
                break;
            case ButtonSoundStates.ButtonBuy:
                Action_Button_Buy?.Invoke();
                break;
            case ButtonSoundStates.ButtonEquip_On:
                Action_Button_Equip_On?.Invoke();
                break;
            case ButtonSoundStates.ButtonEquip_Off:
                Action_Button_Equip_Off?.Invoke();
                break;

            default:
                break;
        }

        if (isBackButtonAndInSameMenu)
            Action_FirstButtonMuted_On?.Invoke();
    }

    void TurnOnMuted()
    {
        isFirstMuted = true;
    }
    void TurnOffMuted()
    {
        isFirstMuted = false;
    }
    void TurnOffStart()
    {
        isStart = false;
    }
}

public enum ButtonSoundStates
{
    None,

    MenuTransition_Forward,
    MenuTransition_Back,
    ButtonNavigate,
    ButtonPress,
    ButtonBack,
    ButtonCannot,
    ButtonBuy,
    ButtonEquip_On,
    ButtonEquip_Off,
}
