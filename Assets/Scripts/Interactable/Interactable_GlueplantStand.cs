
using System;
using System.Collections;
using UnityEngine;

public class Interactable_GlueplantStand : MonoBehaviour
{
    public static event Action Action_Glueplant_isPlaced;

    [Header("Glueplant Type")]
    [SerializeField] GlueplantType glueplantType;
    [SerializeField] int standNumber;

    [Header("Glueplant Assets")]
    [SerializeField] GameObject glueplant_Rivergreen;
    [SerializeField] GameObject glueplant_Sandlands;
    [SerializeField] GameObject glueplant_Frostfield;
    [SerializeField] GameObject glueplant_Firevein;
    [SerializeField] GameObject glueplant_Witchmire;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += LoadGame;
        Player_KeyInputs.Action_InteractButton_isPressed += RunInterraction;

        Movement.Action_StepTaken_Late += DisplayButtonMessageDelayed;
        Movement.Action_BodyRotated += DisplayButtonMessageDelayed;
        Movement.Action_RespawnPlayerLate += DisplayButtonMessageDelayed;
        Action_Glueplant_isPlaced += DisplayButtonMessageDelayed;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= LoadGame;
        Player_KeyInputs.Action_InteractButton_isPressed -= RunInterraction;

        Movement.Action_StepTaken_Late -= DisplayButtonMessageDelayed;
        Movement.Action_BodyRotated -= DisplayButtonMessageDelayed;
        Movement.Action_RespawnPlayerLate -= DisplayButtonMessageDelayed;
        Action_Glueplant_isPlaced -= DisplayButtonMessageDelayed;
    }


    //--------------------



    void LoadGame()
    {
        GameObject glueplant = GetGlueplant();
        if (glueplant)
            glueplant.SetActive(GetStandStats()?.IsTaken(standNumber) == true);
        DisplayButtonMessageDelayed();
    }

    StandStats GetStandStats()
    {
        var map = MapManager.Instance;
        return map ? DataManager.Instance.glueplantStandStats_Store?.GetForLevel(map.levelName) : null;
    }

    GameObject GetGlueplant()
    {
        return glueplantType switch
        {
            GlueplantType.Rivergreen => glueplant_Rivergreen,
            GlueplantType.Sandlands => glueplant_Sandlands,
            GlueplantType.Frostfield => glueplant_Frostfield,
            GlueplantType.Firevein => glueplant_Firevein,
            GlueplantType.Witchmire => glueplant_Witchmire,
            _ => null
        };
    }

    bool CanPlaceGlueplant()
    {
        return StandStats.IsValidStandNumber(standNumber) && GetGlueplant() &&
               GetStandStats()?.IsTaken(standNumber) == false;
    }

    void RunInterraction()
    {
        if (PlayerManager.Instance.block_LookingAt_Horizontal != gameObject || !CanPlaceGlueplant())
            return;

        if (!GlueplantStandManager.Instance.TryPlaceGlueplant(standNumber))
            return;

        GetGlueplant().SetActive(true);
        Action_Glueplant_isPlaced?.Invoke();
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
        if (!MapManager.Instance)
            return;

        GameObject lookedAtObject = PlayerManager.Instance.block_LookingAt_Horizontal;

        if (lookedAtObject == null)
        {
            ButtonMessageManager.Instance.HideButtonMessage(
                ButtonMessageManager.Instance.buttonMessages.buttonMessage_PlaceGlueplant
            );
            return;
        }

        BlockInfo blockInfo = lookedAtObject.GetComponent<BlockInfo>();

        if (blockInfo == null || blockInfo.blockElement != BlockElement.GlueplantStand)
        {
            ButtonMessageManager.Instance.HideButtonMessage(
                ButtonMessageManager.Instance.buttonMessages.buttonMessage_PlaceGlueplant
            );
            return;
        }

        // Important:
        // The player is looking at a GlueplantStand, but not THIS GlueplantStand.
        // Let the correct GlueplantStand decide whether to show/hide the message.
        if (lookedAtObject != gameObject)
        {
            return;
        }

        if (!CanPlaceGlueplant())
        {
            ButtonMessageManager.Instance.HideButtonMessage(
                ButtonMessageManager.Instance.buttonMessages.buttonMessage_PlaceGlueplant
            );
        }
        else
        {
            ButtonMessageManager.Instance.SetButtonMessage(
                ButtonMessageManager.Instance.buttonMessages.buttonMessage_PlaceGlueplant
            );

            ButtonMessageManager.Instance.ShowButtonMessage(
                ButtonMessageManager.Instance.buttonMessages.buttonMessage_PlaceGlueplant
            );
        }
    }
}


[Serializable]
public class GlueplantStandStats
{
    public StandStats standStats_Rivergreen = new StandStats();
    public StandStats standStats_Sandlands = new StandStats();
    public StandStats standStats_Frostfield = new StandStats();
    public StandStats standStats_Firevein = new StandStats();
    public StandStats standStats_Witchmire = new StandStats();

    internal const int MetalworksLevelCount = (int)LevelNames.Metalworks_Lv5 - (int)LevelNames.Metalworks_Lv1 + 1;

    internal static bool IsMetalworks(LevelNames level) =>
        level >= LevelNames.Metalworks_Lv1 && level <= LevelNames.Metalworks_Lv5;

    internal StandStats GetForLevel(string sceneName)
    {
        return Enum.TryParse(sceneName?.Replace("_Lv_", "_Lv"), out LevelNames level)
            ? GetForLevel(level)
            : null;
    }

    internal StandStats GetForLevel(LevelNames level)
    {
        return level switch
        {
            LevelNames.Metalworks_Lv1 => standStats_Rivergreen ??= new StandStats(),
            LevelNames.Metalworks_Lv2 => standStats_Sandlands ??= new StandStats(),
            LevelNames.Metalworks_Lv3 => standStats_Frostfield ??= new StandStats(),
            LevelNames.Metalworks_Lv4 => standStats_Firevein ??= new StandStats(),
            LevelNames.Metalworks_Lv5 => standStats_Witchmire ??= new StandStats(),
            _ => null
        };
    }
}

[Serializable]
public class StandStats
{
    public bool stand_1_isTaken;
    public bool stand_2_isTaken;
    public bool stand_3_isTaken;
    public bool stand_4_isTaken;

    internal const int StandsPerLevel = 4;

    internal int CountTaken => (stand_1_isTaken ? 1 : 0) + (stand_2_isTaken ? 1 : 0) +
                               (stand_3_isTaken ? 1 : 0) + (stand_4_isTaken ? 1 : 0);

    internal static bool IsValidStandNumber(int number) => number >= 1 && number <= StandsPerLevel;

    internal bool IsTaken(int number)
    {
        return number switch
        {
            1 => stand_1_isTaken,
            2 => stand_2_isTaken,
            3 => stand_3_isTaken,
            4 => stand_4_isTaken,
            _ => false
        };
    }

    internal bool TryPlace(int number)
    {
        if (!IsValidStandNumber(number) || IsTaken(number))
            return false;

        switch (number)
        {
            case 1: stand_1_isTaken = true; break;
            case 2: stand_2_isTaken = true; break;
            case 3: stand_3_isTaken = true; break;
            case 4: stand_4_isTaken = true; break;
        }

        return true;
    }
}

public enum GlueplantType
{
    HUB,
    Rivergreen,
    Sandlands,
    Frostfield,
    Firevein,
    Witchmire,
    Metalworks
}
