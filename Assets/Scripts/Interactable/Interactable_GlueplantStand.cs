
using System;
using System.Collections;
using System.Collections.Generic;
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
        StandStats stats = GetStandStats(glueplantType);

        if (stats == null)
            return;

        if (IsStandTaken(stats, standNumber))
            PlaceGlueplant();
    }

    StandStats GetStandStats(GlueplantType type)
    {
        GlueplantStandStats store = DataManager.Instance.glueplantStandStats_Store;

        switch (type)
        {
            case GlueplantType.Rivergreen:
                return store.standStats_Rivergreen;

            case GlueplantType.Sandlands:
                return store.standStats_Sandlands;

            case GlueplantType.Frostfield:
                return store.standStats_Frostfield;

            case GlueplantType.Firevein:
                return store.standStats_Firevein;

            case GlueplantType.Witchmire:
                return store.standStats_Witchmire;

            default:
                return null;
        }
    }
    bool IsStandTaken(StandStats stats, int standNumber)
    {
        switch (standNumber)
        {
            case 1:
                return stats.stand_1_isTaken;

            case 2:
                return stats.stand_2_isTaken;

            case 3:
                return stats.stand_3_isTaken;

            case 4:
                return stats.stand_4_isTaken;

            default:
                return false;
        }
    }


    //--------------------


    void RunInterraction()
    {
        if (PlayerManager.Instance.block_LookingAt_Horizontal == gameObject)
        {
            GlueplantStandManager.Instance.playerPlacingGlueplant = true;
            PlaceGlueplant();
        }
        //Check conditions for being able to interract

        //Place Glueplant if conditions is met
    }
    void PlaceGlueplant()
    {
        switch (glueplantType)
        {
            case GlueplantType.Rivergreen:
                glueplant_Rivergreen.SetActive(true);
                break;
            case GlueplantType.Sandlands:
                glueplant_Sandlands.SetActive(true);
                break;
            case GlueplantType.Frostfield:
                glueplant_Frostfield.SetActive(true);
                break;
            case GlueplantType.Firevein:
                glueplant_Firevein.SetActive(true);
                break;
            case GlueplantType.Witchmire:
                glueplant_Witchmire.SetActive(true);
                break;

            default:
                break;
        }

        UpdateSaveState();

        Action_Glueplant_isPlaced?.Invoke();
    }
    void UpdateSaveState()
    {
        switch (glueplantType)
        {
            case GlueplantType.Rivergreen:
                switch (standNumber)
                {
                    case 1:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Rivergreen.stand_1_isTaken = true;
                        break;
                    case 2:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Rivergreen.stand_2_isTaken = true;
                        break;
                    case 3:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Rivergreen.stand_3_isTaken = true;
                        break;
                    case 4:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Rivergreen.stand_4_isTaken = true;
                        break;
                    default:
                        break;
                }
                break;
            case GlueplantType.Sandlands:
                switch (standNumber)
                {
                    case 1:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Sandlands.stand_1_isTaken = true;
                        break;
                    case 2:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Sandlands.stand_2_isTaken = true;
                        break;
                    case 3:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Sandlands.stand_3_isTaken = true;
                        break;
                    case 4:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Sandlands.stand_4_isTaken = true;
                        break;
                    default:
                        break;
                }
                break;
            case GlueplantType.Frostfield:
                switch (standNumber)
                {
                    case 1:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Frostfield.stand_1_isTaken = true;
                        break;
                    case 2:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Frostfield.stand_2_isTaken = true;
                        break;
                    case 3:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Frostfield.stand_3_isTaken = true;
                        break;
                    case 4:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Frostfield.stand_4_isTaken = true;
                        break;
                    default:
                        break;
                }
                break;
            case GlueplantType.Firevein:
                switch (standNumber)
                {
                    case 1:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Firevein.stand_1_isTaken = true;
                        break;
                    case 2:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Firevein.stand_2_isTaken = true;
                        break;
                    case 3:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Firevein.stand_3_isTaken = true;
                        break;
                    case 4:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Firevein.stand_4_isTaken = true;
                        break;
                    default:
                        break;
                }
                break;
            case GlueplantType.Witchmire:
                switch (standNumber)
                {
                    case 1:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Witchmire.stand_1_isTaken = true;
                        break;
                    case 2:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Witchmire.stand_2_isTaken = true;
                        break;
                    case 3:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Witchmire.stand_3_isTaken = true;
                        break;
                    case 4:
                        DataManager.Instance.glueplantStandStats_Store.standStats_Witchmire.stand_4_isTaken = true;
                        break;
                    default:
                        break;
                }
                break;

            default:
                break;
        }

        DataPersistanceManager.instance.SaveGame();

        StartCoroutine(CheckPlacedGlueplant_Delay());
    }
    IEnumerator CheckPlacedGlueplant_Delay()
    {
        yield return new WaitForEndOfFrame();

        GlueplantStandManager.Instance.playerPlacingGlueplant = false;
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

        StandStats stats = GetStandStats(glueplantType);

        if (stats == null)
        {
            ButtonMessageManager.Instance.HideButtonMessage(
                ButtonMessageManager.Instance.buttonMessages.buttonMessage_PlaceGlueplant
            );

            Debug.LogWarning($"No StandStats found for {glueplantType} on {gameObject.name}");
            return;
        }

        if (IsStandTaken(stats, standNumber))
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
    public StandStats standStats_Rivergreen;
    public StandStats standStats_Sandlands;
    public StandStats standStats_Frostfield;
    public StandStats standStats_Firevein;
    public StandStats standStats_Witchmire;
}

[Serializable]
public class StandStats
{
    public bool stand_1_isTaken;
    public bool stand_2_isTaken;
    public bool stand_3_isTaken;
    public bool stand_4_isTaken;
}

public enum GlueplantType
{
    Tutorial,
    Rivergreen,
    Sandlands,
    Frostfield,
    Firevein,
    Witchmire,
    Metalworks
}