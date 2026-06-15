using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class AncientTablet : MonoBehaviour
{
    [Header("Tablet Text Entry")]
    [SerializeField] int textEntryNumber;

    [Header("Tablet Camera")]
    [SerializeField] CinemachineCamera tabletVirtualCamera;

    bool tabletInterractTransitioning;
    bool tabletIsShowing;


    //--------------------


    private void OnEnable()
    {
        Player_KeyInputs.Action_InteractButton_isPressed += Interraction;

        Movement.Action_StepTaken_Late += DisplayButtonMessageDelayed;
        Movement.Action_BodyRotated += DisplayButtonMessageDelayed;
        Movement.Action_RespawnPlayerLate += DisplayButtonMessageDelayed;
    }

    private void OnDisable()
    {
        Player_KeyInputs.Action_InteractButton_isPressed -= Interraction;

        Movement.Action_StepTaken_Late -= DisplayButtonMessageDelayed;
        Movement.Action_BodyRotated -= DisplayButtonMessageDelayed;
        Movement.Action_RespawnPlayerLate -= DisplayButtonMessageDelayed;
    }


    //--------------------


    void Interraction()
    {
        if (tabletInterractTransitioning) return;

        if (tabletIsShowing)
            EndInterraction();
        else
            RunInterraction();
    }


    void RunInterraction()
    {
        if (PlayerManager.Instance.block_LookingAt_Horizontal == gameObject)
        {
            print("Tablet - RunInterraction");

            tabletInterractTransitioning = true;

            StartCoroutine(RunInterraction_Delay());
        }
    }
    IEnumerator RunInterraction_Delay()
    {
        ButtonMessageManager.Instance.HideButtonMessage(ButtonMessageManager.Instance.buttonMessages.buttonMessage_Read);


        RenderHiderOnContact.Instance.FreeCamOn();
        HoleShaderOnOffScript.Instance.HoleShader_Off();

        //Change camera
        CameraController.Instance.CM_Other = tabletVirtualCamera;
        yield return StartCoroutine(CameraController.Instance.StartVirtualCameraBlend_In(CameraController.Instance.CM_Other));

        if (textEntryNumber < TabletManager.Instance.tabletData.tabletDataSegment.Count)
            TabletManager.Instance.tabletUI_Text.text = TabletManager.Instance.tabletData.tabletDataSegment[textEntryNumber].languageDialogueList[(int)DataManager.Instance.settingData_StoreList.currentLanguage];
        else
            TabletManager.Instance.tabletUI_Text.text = TabletManager.Instance.tabletData.tabletDataSegment[0].languageDialogueList[(int)DataManager.Instance.settingData_StoreList.currentLanguage];

        TabletManager.Instance.ShowTabletUI(TabletManager.Instance.tabletUI_Parent);

        yield return new WaitForSeconds(TabletManager.Instance.fadeDuration);
        tabletIsShowing = true;
        tabletInterractTransitioning = false;
    }

    void EndInterraction()
    {
        print("Tablet - EndInterraction");

        tabletInterractTransitioning = true;

        StartCoroutine(EndInterraction_Delay());
    }
    IEnumerator EndInterraction_Delay()
    {
        TabletManager.Instance.HideTabletUI(TabletManager.Instance.tabletUI_Parent);
        yield return new WaitForSeconds(TabletManager.Instance.fadeDuration);

        CameraController.Instance.CM_Other = null;
        yield return StartCoroutine(CameraController.Instance.StartVirtualCameraBlend_Out(CameraController.Instance.CM_Other));

        RenderHiderOnContact.Instance.FreeCamOff();
        HoleShaderOnOffScript.Instance.HoleShader_On();

        ButtonMessageManager.Instance.SetButtonMessage(ButtonMessageManager.Instance.buttonMessages.buttonMessage_Read);
        ButtonMessageManager.Instance.ShowButtonMessage(ButtonMessageManager.Instance.buttonMessages.buttonMessage_Read);

        tabletIsShowing = false;
        tabletInterractTransitioning = false;
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
        GameObject lookedAtObject = PlayerManager.Instance.block_LookingAt_Horizontal;

        if (lookedAtObject == null)
        {
            ButtonMessageManager.Instance.HideButtonMessage(
                ButtonMessageManager.Instance.buttonMessages.buttonMessage_Read
            );
            return;
        }

        BlockInfo blockInfo = lookedAtObject.GetComponent<BlockInfo>();

        if (blockInfo == null || blockInfo.blockElement != BlockElement.Tablet)
        {
            ButtonMessageManager.Instance.HideButtonMessage(
                ButtonMessageManager.Instance.buttonMessages.buttonMessage_Read
            );
            return;
        }

        //If not blocked, show message
        ButtonMessageManager.Instance.SetButtonMessage(
                ButtonMessageManager.Instance.buttonMessages.buttonMessage_Read
            );

        ButtonMessageManager.Instance.ShowButtonMessage(
            ButtonMessageManager.Instance.buttonMessages.buttonMessage_Read
        );
    }
}
