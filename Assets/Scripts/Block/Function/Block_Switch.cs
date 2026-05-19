using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Block_Switch : MonoBehaviour
{
    [Header("SwitchInfo")]
    [SerializeField] int switch_no;
    [SerializeField] RegionName region;

    [SerializeField] bool isPressed;

    [Header("TargetInfo")]
    [SerializeField] List<GameObject> targetObjectList;

    [Header("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;

    [SerializeField] List<SkinnedMeshRenderer> lodRenderers = new List<SkinnedMeshRenderer>();

    bool gameIsLoaded;


    //--------------------


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //foreach (var smr in GetComponentsInChildren<SkinnedMeshRenderer>())
        //{
        //    if (smr.name.Contains("LOD"))
        //    {
        //        lodRenderers.Add(smr);
        //    }
        //}
    }


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += LoadGame;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= LoadGame;
    }


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) //6 = Player
        {
            if (!isPressed)
                ActivateSwitch();
            else
                DeactivateSwitch();
        }
    }


    //--------------------


    void SaveGame()
    {
        switch (region)
        {
            case RegionName.None:
                break;

            case RegionName.Rivergreen:
                switch (switch_no)
                {
                    case 1:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_1 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_1;
                        break;
                    case 2:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_2 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_2;
                        break;
                    case 3:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_3 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_3;
                        break;
                    case 4:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_4 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_4;
                        break;
                    case 5:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_5 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_5;
                        break;
                    case 6:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_6 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_6;
                        break;
                    case 7:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_7 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_7;
                        break;
                    case 8:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_8 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_8;
                        break;
                    case 9:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_9 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_9;
                        break;
                    case 10:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_10 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_10;
                        break;
                    case 11:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_11 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_11;
                        break;
                    case 12:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_12 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_12;
                        break;
                    case 13:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_13 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_13;
                        break;
                    case 14:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_14 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_14;
                        break;
                    case 15:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_15 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_15;
                        break;
                    case 16:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_16 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_16;
                        break;
                    case 17:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_17 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_17;
                        break;
                    case 18:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_18 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_18;
                        break;
                    case 19:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_19 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_19;
                        break;
                    case 20:
                        DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_20 = !DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_20;
                        break;

                    default:
                        break;
                }
                break;
            case RegionName.Sandlands:
                switch (switch_no)
                {
                    case 1:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_1 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_1;
                        break;
                    case 2:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_2 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_2;
                        break;
                    case 3:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_3 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_3;
                        break;
                    case 4:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_4 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_4;
                        break;
                    case 5:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_5 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_5;
                        break;
                    case 6:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_6 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_6;
                        break;
                    case 7:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_7 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_7;
                        break;
                    case 8:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_8 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_8;
                        break;
                    case 9:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_9 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_9;
                        break;
                    case 10:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_10 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_10;
                        break;
                    case 11:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_11 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_11;
                        break;
                    case 12:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_12 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_12;
                        break;
                    case 13:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_13 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_13;
                        break;
                    case 14:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_14 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_14;
                        break;
                    case 15:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_15 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_15;
                        break;
                    case 16:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_16 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_16;
                        break;
                    case 17:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_17 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_17;
                        break;
                    case 18:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_18 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_18;
                        break;
                    case 19:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_19 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_19;
                        break;
                    case 20:
                        DataManager.Instance.switchesData_Store.switches_Sandlands.switch_20 = !DataManager.Instance.switchesData_Store.switches_Sandlands.switch_20;
                        break;

                    default:
                        break;
                }
                break;
            case RegionName.Frostfields:
                switch (switch_no)
                {
                    case 1:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_1 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_1;
                        break;
                    case 2:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_2 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_2;
                        break;
                    case 3:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_3 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_3;
                        break;
                    case 4:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_4 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_4;
                        break;
                    case 5:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_5 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_5;
                        break;
                    case 6:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_6 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_6;
                        break;
                    case 7:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_7 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_7;
                        break;
                    case 8:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_8 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_8;
                        break;
                    case 9:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_9 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_9;
                        break;
                    case 10:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_10 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_10;
                        break;
                    case 11:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_11 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_11;
                        break;
                    case 12:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_12 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_12;
                        break;
                    case 13:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_13 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_13;
                        break;
                    case 14:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_14 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_14;
                        break;
                    case 15:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_15 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_15;
                        break;
                    case 16:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_16 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_16;
                        break;
                    case 17:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_17 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_17;
                        break;
                    case 18:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_18 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_18;
                        break;
                    case 19:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_19 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_19;
                        break;
                    case 20:
                        DataManager.Instance.switchesData_Store.switches_Frostfield.switch_20 = !DataManager.Instance.switchesData_Store.switches_Frostfield.switch_20;
                        break;

                    default:
                        break;
                }
                break;
            case RegionName.Firevein:
                switch (switch_no)
                {
                    case 1:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_1 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_1;
                        break;
                    case 2:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_2 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_2;
                        break;
                    case 3:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_3 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_3;
                        break;
                    case 4:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_4 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_4;
                        break;
                    case 5:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_5 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_5;
                        break;
                    case 6:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_6 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_6;
                        break;
                    case 7:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_7 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_7;
                        break;
                    case 8:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_8 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_8;
                        break;
                    case 9:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_9 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_9;
                        break;
                    case 10:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_10 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_10;
                        break;
                    case 11:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_11 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_11;
                        break;
                    case 12:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_12 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_12;
                        break;
                    case 13:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_13 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_13;
                        break;
                    case 14:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_14 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_14;
                        break;
                    case 15:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_15 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_15;
                        break;
                    case 16:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_16 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_16;
                        break;
                    case 17:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_17 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_17;
                        break;
                    case 18:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_18 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_18;
                        break;
                    case 19:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_19 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_19;
                        break;
                    case 20:
                        DataManager.Instance.switchesData_Store.switches_Firevein.switch_20 = !DataManager.Instance.switchesData_Store.switches_Firevein.switch_20;
                        break;

                    default:
                        break;
                }
                break;
            case RegionName.Witchmire:
                switch (switch_no)
                {
                    case 1:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_1 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_1;
                        break;
                    case 2:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_2 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_2;
                        break;
                    case 3:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_3 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_3;
                        break;
                    case 4:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_4 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_4;
                        break;
                    case 5:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_5 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_5;
                        break;
                    case 6:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_6 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_6;
                        break;
                    case 7:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_7 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_7;
                        break;
                    case 8:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_8 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_8;
                        break;
                    case 9:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_9 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_9;
                        break;
                    case 10:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_10 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_10;
                        break;
                    case 11:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_11 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_11;
                        break;
                    case 12:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_12 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_12;
                        break;
                    case 13:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_13 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_13;
                        break;
                    case 14:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_14 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_14;
                        break;
                    case 15:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_15 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_15;
                        break;
                    case 16:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_16 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_16;
                        break;
                    case 17:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_17 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_17;
                        break;
                    case 18:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_18 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_18;
                        break;
                    case 19:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_19 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_19;
                        break;
                    case 20:
                        DataManager.Instance.switchesData_Store.switches_Witchmire.switch_20 = !DataManager.Instance.switchesData_Store.switches_Witchmire.switch_20;
                        break;

                    default:
                        break;
                }
                break;

            case RegionName.Metalworks:

                break;

            default:
                break;
        }

        DataPersistanceManager.instance.SaveGame();
    }

    void LoadGame()
    {
        switch (region)
        {
            case RegionName.None:
                break;

            case RegionName.Rivergreen:
                switch (switch_no)
                {
                    case 1:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_1)
                            ActivateSwitch();
                        break;
                    case 2:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_2)
                            ActivateSwitch();
                        break;
                    case 3:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_3)
                            ActivateSwitch();
                        break;
                    case 4:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_4)
                            ActivateSwitch();
                        break;
                    case 5:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_5)
                            ActivateSwitch();
                        break;
                    case 6:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_6)
                            ActivateSwitch();
                        break;
                    case 7:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_7)
                            ActivateSwitch();
                        break;
                    case 8:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_8)
                            ActivateSwitch();
                        break;
                    case 9:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_9)
                            ActivateSwitch();
                        break;
                    case 10:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_10)
                            ActivateSwitch();
                        break;
                    case 11:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_11)
                            ActivateSwitch();
                        break;
                    case 12:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_12)
                            ActivateSwitch();
                        break;
                    case 13:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_13)
                            ActivateSwitch();
                        break;
                    case 14:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_14)
                            ActivateSwitch();
                        break;
                    case 15:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_15)
                            ActivateSwitch();
                        break;
                    case 16:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_16)
                            ActivateSwitch();
                        break;
                    case 17:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_17)
                            ActivateSwitch();
                        break;
                    case 18:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_18)
                            ActivateSwitch();
                        break;
                    case 19:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_19)
                            ActivateSwitch();
                        break;
                    case 20:
                        if (DataManager.Instance.switchesData_Store.switches_Rivergreen.switch_20)
                            ActivateSwitch();
                        break;

                    default:
                        break;
                }
                break;
            case RegionName.Sandlands:
                switch (switch_no)
                {
                    case 1:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_1)
                            ActivateSwitch();
                        break;
                    case 2:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_2)
                            ActivateSwitch();
                        break;
                    case 3:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_3)
                            ActivateSwitch();
                        break;
                    case 4:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_4)
                            ActivateSwitch();
                        break;
                    case 5:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_5)
                            ActivateSwitch();
                        break;
                    case 6:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_6)
                            ActivateSwitch();
                        break;
                    case 7:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_7)
                            ActivateSwitch();
                        break;
                    case 8:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_8)
                            ActivateSwitch();
                        break;
                    case 9:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_9)
                            ActivateSwitch();
                        break;
                    case 10:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_10)
                            ActivateSwitch();
                        break;
                    case 11:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_11)
                            ActivateSwitch();
                        break;
                    case 12:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_12)
                            ActivateSwitch();
                        break;
                    case 13:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_13)
                            ActivateSwitch();
                        break;
                    case 14:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_14)
                            ActivateSwitch();
                        break;
                    case 15:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_15)
                            ActivateSwitch();
                        break;
                    case 16:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_16)
                            ActivateSwitch();
                        break;
                    case 17:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_17)
                            ActivateSwitch();
                        break;
                    case 18:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_18)
                            ActivateSwitch();
                        break;
                    case 19:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_19)
                            ActivateSwitch();
                        break;
                    case 20:
                        if (DataManager.Instance.switchesData_Store.switches_Sandlands.switch_20)
                            ActivateSwitch();
                        break;

                    default:
                        break;
                }
                break;
            case RegionName.Frostfields:
                switch (switch_no)
                {
                    case 1:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_1)
                            ActivateSwitch();
                        break;
                    case 2:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_2)
                            ActivateSwitch();
                        break;
                    case 3:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_3)
                            ActivateSwitch();
                        break;
                    case 4:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_4)
                            ActivateSwitch();
                        break;
                    case 5:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_5)
                            ActivateSwitch();
                        break;
                    case 6:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_6)
                            ActivateSwitch();
                        break;
                    case 7:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_7)
                            ActivateSwitch();
                        break;
                    case 8:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_8)
                            ActivateSwitch();
                        break;
                    case 9:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_9)
                            ActivateSwitch();
                        break;
                    case 10:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_10)
                            ActivateSwitch();
                        break;
                    case 11:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_11)
                            ActivateSwitch();
                        break;
                    case 12:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_12)
                            ActivateSwitch();
                        break;
                    case 13:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_13)
                            ActivateSwitch();
                        break;
                    case 14:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_14)
                            ActivateSwitch();
                        break;
                    case 15:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_15)
                            ActivateSwitch();
                        break;
                    case 16:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_16)
                            ActivateSwitch();
                        break;
                    case 17:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_17)
                            ActivateSwitch();
                        break;
                    case 18:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_18)
                            ActivateSwitch();
                        break;
                    case 19:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_19)
                            ActivateSwitch();
                        break;
                    case 20:
                        if (DataManager.Instance.switchesData_Store.switches_Frostfield.switch_20)
                            ActivateSwitch();
                        break;

                    default:
                        break;
                }
                break;
            case RegionName.Firevein:
                switch (switch_no)
                {
                    case 1:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_1)
                            ActivateSwitch();
                        break;
                    case 2:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_2)
                            ActivateSwitch();
                        break;
                    case 3:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_3)
                            ActivateSwitch();
                        break;
                    case 4:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_4)
                            ActivateSwitch();
                        break;
                    case 5:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_5)
                            ActivateSwitch();
                        break;
                    case 6:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_6)
                            ActivateSwitch();
                        break;
                    case 7:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_7)
                            ActivateSwitch();
                        break;
                    case 8:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_8)
                            ActivateSwitch();
                        break;
                    case 9:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_9)
                            ActivateSwitch();
                        break;
                    case 10:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_10)
                            ActivateSwitch();
                        break;
                    case 11:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_11)
                            ActivateSwitch();
                        break;
                    case 12:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_12)
                            ActivateSwitch();
                        break;
                    case 13:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_13)
                            ActivateSwitch();
                        break;
                    case 14:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_14)
                            ActivateSwitch();
                        break;
                    case 15:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_15)
                            ActivateSwitch();
                        break;
                    case 16:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_16)
                            ActivateSwitch();
                        break;
                    case 17:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_17)
                            ActivateSwitch();
                        break;
                    case 18:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_18)
                            ActivateSwitch();
                        break;
                    case 19:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_19)
                            ActivateSwitch();
                        break;
                    case 20:
                        if (DataManager.Instance.switchesData_Store.switches_Firevein.switch_20)
                            ActivateSwitch();
                        break;

                    default:
                        break;
                }
                break;
            case RegionName.Witchmire:
                switch (switch_no)
                {
                    case 1:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_1)
                            ActivateSwitch();
                        break;
                    case 2:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_2)
                            ActivateSwitch();
                        break;
                    case 3:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_3)
                            ActivateSwitch();
                        break;
                    case 4:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_4)
                            ActivateSwitch();
                        break;
                    case 5:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_5)
                            ActivateSwitch();
                        break;
                    case 6:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_6)
                            ActivateSwitch();
                        break;
                    case 7:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_7)
                            ActivateSwitch();
                        break;
                    case 8:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_8)
                            ActivateSwitch();
                        break;
                    case 9:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_9)
                            ActivateSwitch();
                        break;
                    case 10:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_10)
                            ActivateSwitch();
                        break;
                    case 11:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_11)
                            ActivateSwitch();
                        break;
                    case 12:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_12)
                            ActivateSwitch();
                        break;
                    case 13:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_13)
                            ActivateSwitch();
                        break;
                    case 14:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_14)
                            ActivateSwitch();
                        break;
                    case 15:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_15)
                            ActivateSwitch();
                        break;
                    case 16:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_16)
                            ActivateSwitch();
                        break;
                    case 17:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_17)
                            ActivateSwitch();
                        break;
                    case 18:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_18)
                            ActivateSwitch();
                        break;
                    case 19:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_19)
                            ActivateSwitch();
                        break;
                    case 20:
                        if (DataManager.Instance.switchesData_Store.switches_Witchmire.switch_20)
                            ActivateSwitch();
                        break;

                    default:
                        break;
                }
                break;

            case RegionName.Metalworks:
                break;

            default:
                break;
        }

        gameIsLoaded = true;
    }


    //--------------------


    void ActivateSwitch()
    {
        if (gameIsLoaded)
            audioSource.PlayOneShot(clip);

        ButtonAnimation_Off();

        foreach (GameObject targetObject in targetObjectList)
        {
            if (targetObject.TryGetComponent(out Block_SwitchTarget target))
            {
                target.MoveTarget(gameIsLoaded);
            }
        }

        isPressed = true;

        if (gameIsLoaded)
            SaveGame();
    }

    void DeactivateSwitch()
    {
        if (gameIsLoaded)
            audioSource.PlayOneShot(clip);

        ButtonAnimation_On();

        foreach (GameObject targetObject in targetObjectList)
        {
            if (targetObject.TryGetComponent(out Block_SwitchTarget target))
            {
                target.MoveTargetBack(gameIsLoaded);
            }
        }

        isPressed = false;

        if (gameIsLoaded)
            SaveGame();
    }


    //--------------------


    void ButtonAnimation_On()
    {
        foreach (var lod in lodRenderers)
        {
            lod.SetBlendShapeWeight(0, 100);
        }
    }
    void ButtonAnimation_Off()
    {
        foreach (var lod in lodRenderers)
        {
            lod.SetBlendShapeWeight(0, 0);
        }
    }
}
