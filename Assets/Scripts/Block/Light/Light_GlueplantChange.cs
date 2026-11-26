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
        var map = MapManager.Instance;
        if (!map || glueplantStand_number == null || glueplantStand_number.Count == 0)
            return false;

        StandStats stats = DataManager.Instance.glueplantStandStats_Store?.GetForLevel(map.levelName);
        return stats != null && glueplantStand_number.TrueForAll(stats.IsTaken);
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
