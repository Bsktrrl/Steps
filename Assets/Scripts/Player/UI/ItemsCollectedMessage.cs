using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsCollectedMessage : MonoBehaviour
{
    [Header("Essence Message")]
    [SerializeField] GameObject essenceGetMessage_Parent;
    [SerializeField] TextMeshProUGUI essenceGetMessage_Text;

    [Header("StepUp Message")]
    [SerializeField] GameObject stepUpGetMessage_Parent;
    [SerializeField] TextMeshProUGUI stepUpGetMessage_Text;

    [Header("Skin Message")]
    [SerializeField] GameObject skinGetMessage_Parent;
    [SerializeField] Image skinGetMessage_Image;
    [SerializeField] TextMeshProUGUI skinGetMessage_Text;

    [Header("Display Effect Settings")]
    [SerializeField] float fadeInTime = 0.5f;
    [SerializeField] float visibleTime = 1.5f;
    [SerializeField] float fadeOutTime = 0.5f;

    readonly Dictionary<GameObject, Coroutine> _runningEffects = new();

    bool readyEssanceMessage;
    bool readyStepUpMessage;
    bool readySkinMessage;



    //-------------------------


    private void OnEnable()
    {
        Interactable_Pickup.Action_EssencePickupGot += ReadyEssanceMessage;
        Interactable_Pickup.Action_StepsUpPickupGot += ReadyStepUpMessage;
        Interactable_Pickup.Action_SkinPickupGot += ReadySkinMessage;

        Movement.Action_PickupAnimation_Complete += ShowMessage;
    }
    private void OnDisable()
    {
        Interactable_Pickup.Action_EssencePickupGot -= ReadyEssanceMessage;
        Interactable_Pickup.Action_StepsUpPickupGot -= ReadyStepUpMessage;
        Interactable_Pickup.Action_SkinPickupGot -= ReadySkinMessage;

        Movement.Action_PickupAnimation_Complete -= ShowMessage;
    }


    //-------------------------


    void ReadyEssanceMessage()
    {
        readyEssanceMessage = true;
    }
    void ReadyStepUpMessage()
    {
        readyStepUpMessage = true;
    }
    void ReadySkinMessage()
    {
        readySkinMessage = true;
    }

    void ShowMessage()
    {
        if (readyEssanceMessage)
            ShowEssenceMessage();
        else if (readyStepUpMessage)
            ShowStepUpMessage();
        else if (readySkinMessage)
            ShowSkinMessage();

        readyEssanceMessage = false;
        readyStepUpMessage = false;
        readySkinMessage = false;
    }

    void ShowEssenceMessage()
    {
        int essenceCount = 0;
        for (int i = 0; i < MapManager.Instance.mapInfo_ToSave.essenceList.Count; i++)
        {
            if (MapManager.Instance.mapInfo_ToSave.essenceList[i].isTaken)
            {
                essenceCount++;
            }
        }

        essenceGetMessage_Text.SetText("{0} / {1}", essenceCount, 10);
        DisplayEffect(essenceGetMessage_Parent);
    }
    void ShowStepUpMessage()
    {
        int stepUpCount = 0;
        for (int i = 0; i < MapManager.Instance.mapInfo_ToSave.maxStepList.Count; i++)
        {
            if (MapManager.Instance.mapInfo_ToSave.maxStepList[i].isTaken)
            {
                stepUpCount++;
            }
        }

        stepUpGetMessage_Text.SetText("{0} / {1}", stepUpCount, 3);
        DisplayEffect(stepUpGetMessage_Parent);
    }
    void ShowSkinMessage()
    {
        int skinCount = MapManager.Instance.mapInfo_ToSave.levelSkin.isTaken ? 1 : 0;

        skinGetMessage_Image.sprite = PauseMenuManager.Instance.SelectSpriteForLevel(MapManager.Instance.mapInfo_ToSave.skintype);
        skinGetMessage_Text.SetText("{0} / {1}", skinCount, 1);
        DisplayEffect(skinGetMessage_Parent);
    }


    //-------------------------


    void DisplayEffect(GameObject parent)
    {
        var cg = parent.GetComponent<CanvasGroup>() ?? parent.AddComponent<CanvasGroup>();
        StartCoroutine(Fade(cg, fadeInTime, visibleTime, fadeOutTime));
    }

    IEnumerator Fade(CanvasGroup cg, float tIn, float hold, float tOut)
    {
        for (float t = 0; t < tIn; t += Time.unscaledDeltaTime) { cg.alpha = tIn <= 0 ? 1 : t / tIn; yield return null; }
        cg.alpha = 1f;

        if (hold > 0f) yield return new WaitForSecondsRealtime(hold);

        for (float t = 0; t < tOut; t += Time.unscaledDeltaTime) { cg.alpha = tOut <= 0 ? 0 : 1f - t / tOut; yield return null; }
        cg.alpha = 0f;
    }
}
