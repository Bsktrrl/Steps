using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Light_GlueplantChange : MonoBehaviour
{
    [Header("Glueplant Stats")]
    [SerializeField] GlueplantType glueplantType;
    [SerializeField] List<int> glueplantStand_number;

    float colorChangeDuration = 1.5f;
    bool colorHasChanged;

    private Coroutine changeColorCoroutine;


    //--------------------


    private void OnEnable()
    {
        Interactable_GlueplantStand.Action_Glueplant_isPlaced += ChangeLight;
        DataManager.Action_dataHasLoaded += ChangeLight;
    }
    private void OnDisable()
    {
        Interactable_GlueplantStand.Action_Glueplant_isPlaced -= ChangeLight;
        DataManager.Action_dataHasLoaded -= ChangeLight;
    }


    //--------------------


    void ChangeLight()
    {
        if (!colorHasChanged && CheckIfGlueplantPickedUp())
            ChangeLight_Purple();
    }


    //--------------------


    bool CheckIfGlueplantPickedUp()
    {
        if (glueplantStand_number == null || glueplantStand_number.Count == 0)
            return false;

        foreach (int standNumber in glueplantStand_number)
        {
            bool standIsTaken = false;

            switch (glueplantType)
            {
                case GlueplantType.Rivergreen:
                    switch (standNumber)
                    {
                        case 1:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Rivergreen.stand_1_isTaken;
                            break;
                        case 2:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Rivergreen.stand_2_isTaken;
                            break;
                        case 3:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Rivergreen.stand_3_isTaken;
                            break;
                        case 4:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Rivergreen.stand_4_isTaken;
                            break;
                    }
                    break;

                case GlueplantType.Sandlands:
                    switch (standNumber)
                    {
                        case 1:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Sandlands.stand_1_isTaken;
                            break;
                        case 2:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Sandlands.stand_2_isTaken;
                            break;
                        case 3:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Sandlands.stand_3_isTaken;
                            break;
                        case 4:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Sandlands.stand_4_isTaken;
                            break;
                    }
                    break;

                case GlueplantType.Frostfield:
                    switch (standNumber)
                    {
                        case 1:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Frostfield.stand_1_isTaken;
                            break;
                        case 2:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Frostfield.stand_2_isTaken;
                            break;
                        case 3:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Frostfield.stand_3_isTaken;
                            break;
                        case 4:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Frostfield.stand_4_isTaken;
                            break;
                    }
                    break;

                case GlueplantType.Firevein:
                    switch (standNumber)
                    {
                        case 1:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Firevein.stand_1_isTaken;
                            break;
                        case 2:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Firevein.stand_2_isTaken;
                            break;
                        case 3:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Firevein.stand_3_isTaken;
                            break;
                        case 4:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Firevein.stand_4_isTaken;
                            break;
                    }
                    break;

                case GlueplantType.Witchmire:
                    switch (standNumber)
                    {
                        case 1:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Witchmire.stand_1_isTaken;
                            break;
                        case 2:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Witchmire.stand_2_isTaken;
                            break;
                        case 3:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Witchmire.stand_3_isTaken;
                            break;
                        case 4:
                            standIsTaken = DataManager.Instance.glueplantStandStats_Store.standStats_Witchmire.stand_4_isTaken;
                            break;
                    }
                    break;
            }

            if (!standIsTaken)
                return false;
        }

        return true;
    }


    //--------------------


    string GetOriginalColor()
    {
        switch (glueplantType)
        {
            case GlueplantType.HUB:
                return "#000000";

            case GlueplantType.Rivergreen:
                return "#607800";
            case GlueplantType.Sandlands:
                return "#000000";
            case GlueplantType.Frostfield:
                return "#000000";
            case GlueplantType.Firevein:
                return "#000000";
            case GlueplantType.Witchmire:
                return "#000000";

            case GlueplantType.Metalworks:
                return "#FFFFFF";

            default:
                return "";
        }
    }
    string GetPurpleColor()
    {
        return "#41193F";
    }


    //--------------------


    void ChangeLight_Purple()
    {
        Light lightComponent = GetComponent<Light>();

        if (lightComponent)
        {
            string originalColor = GetOriginalColor();
            string newColor = GetPurpleColor();

            if (ColorUtility.TryParseHtmlString(originalColor, out Color startColor) &&
                ColorUtility.TryParseHtmlString(newColor, out Color endColor))
            {
                if (changeColorCoroutine != null)
                    StopCoroutine(changeColorCoroutine);

                changeColorCoroutine = StartCoroutine(ChangeLightColorOverTime(lightComponent, startColor, endColor, colorChangeDuration));
            }
        }

        colorHasChanged = true;
    }

    IEnumerator ChangeLightColorOverTime(Light lightComponent, Color startColor, Color endColor, float duration)
    {
        float timer = 0f;

        lightComponent.color = startColor;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            lightComponent.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }

        lightComponent.color = endColor;
    }
}