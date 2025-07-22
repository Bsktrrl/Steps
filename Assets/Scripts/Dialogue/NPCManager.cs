using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    public CharatersData charatersData;


    //--------------------


    public void LoadData()
    {
        charatersData = new CharatersData();
        charatersData = DataManager.Instance.charatersData_Store;
    }
    public void SaveData()
    {
        DataManager.Instance.charatersData_Store = charatersData;

        DataPersistanceManager.instance.SaveGame();
    }


    //--------------------


    public void SetDialogueFinished(NPCs npc, int dialogueNumber)
    {
        switch (npc)
        {
            case NPCs.None:
                break;

            case NPCs.Floriel:
                switch (dialogueNumber)
                {
                    case 1:
                        charatersData.floriel_Data.level_1_DialogueFinished = true;
                        break;
                    case 2:
                        charatersData.floriel_Data.level_1_DialogueFinished = true;
                        break;
                    case 3:
                        charatersData.floriel_Data.level_1_DialogueFinished = true;
                        break;
                    case 4:
                        charatersData.floriel_Data.level_1_DialogueFinished = true;
                        break;
                    case 5:
                        charatersData.floriel_Data.level_1_DialogueFinished = true;
                        break;
                    case 6:
                        charatersData.floriel_Data.level_1_DialogueFinished = true;
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Granith:
                switch (dialogueNumber)
                {
                    case 1:
                        charatersData.granith_Data.level_1_DialogueFinished = true;
                        break;
                    case 2:
                        charatersData.granith_Data.level_1_DialogueFinished = true;
                        break;
                    case 3:
                        charatersData.granith_Data.level_1_DialogueFinished = true;
                        break;
                    case 4:
                        charatersData.granith_Data.level_1_DialogueFinished = true;
                        break;
                    case 5:
                        charatersData.granith_Data.level_1_DialogueFinished = true;
                        break;
                    case 6:
                        charatersData.granith_Data.level_1_DialogueFinished = true;
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Archie:
                switch (dialogueNumber)
                {
                    case 1:
                        charatersData.archie_Data.level_1_DialogueFinished = true;
                        break;
                    case 2:
                        charatersData.archie_Data.level_1_DialogueFinished = true;
                        break;
                    case 3:
                        charatersData.archie_Data.level_1_DialogueFinished = true;
                        break;
                    case 4:
                        charatersData.archie_Data.level_1_DialogueFinished = true;
                        break;
                    case 5:
                        charatersData.archie_Data.level_1_DialogueFinished = true;
                        break;
                    case 6:
                        charatersData.archie_Data.level_1_DialogueFinished = true;
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Aisa:
                switch (dialogueNumber)
                {
                    case 1:
                        charatersData.aisa_Data.level_1_DialogueFinished = true;
                        break;
                    case 2:
                        charatersData.aisa_Data.level_1_DialogueFinished = true;
                        break;
                    case 3:
                        charatersData.aisa_Data.level_1_DialogueFinished = true;
                        break;
                    case 4:
                        charatersData.aisa_Data.level_1_DialogueFinished = true;
                        break;
                    case 5:
                        charatersData.aisa_Data.level_1_DialogueFinished = true;
                        break;
                    case 6:
                        charatersData.aisa_Data.level_1_DialogueFinished = true;
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Mossy:
                switch (dialogueNumber)
                {
                    case 1:
                        charatersData.mossy_Data.level_1_DialogueFinished = true;
                        break;
                    case 2:
                        charatersData.mossy_Data.level_1_DialogueFinished = true;
                        break;
                    case 3:
                        charatersData.mossy_Data.level_1_DialogueFinished = true;
                        break;
                    case 4:
                        charatersData.mossy_Data.level_1_DialogueFinished = true;
                        break;
                    case 5:
                        charatersData.mossy_Data.level_1_DialogueFinished = true;
                        break;
                    case 6:
                        charatersData.mossy_Data.level_1_DialogueFinished = true;
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Larry:
                switch (dialogueNumber)
                {
                    case 1:
                        charatersData.larry_Data.level_1_DialogueFinished = true;
                        break;
                    case 2:
                        charatersData.larry_Data.level_1_DialogueFinished = true;
                        break;
                    case 3:
                        charatersData.larry_Data.level_1_DialogueFinished = true;
                        break;
                    case 4:
                        charatersData.larry_Data.level_1_DialogueFinished = true;
                        break;
                    case 5:
                        charatersData.larry_Data.level_1_DialogueFinished = true;
                        break;
                    case 6:
                        charatersData.larry_Data.level_1_DialogueFinished = true;
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Stepellier:
                switch (dialogueNumber)
                {
                    case 1:
                        charatersData.stepellier_Data.level_1_DialogueFinished = true;
                        break;
                    case 2:
                        charatersData.stepellier_Data.level_1_DialogueFinished = true;
                        break;
                    case 3:
                        charatersData.stepellier_Data.level_1_DialogueFinished = true;
                        break;
                    case 4:
                        charatersData.stepellier_Data.level_1_DialogueFinished = true;
                        break;
                    case 5:
                        charatersData.stepellier_Data.level_1_DialogueFinished = true;
                        break;
                    case 6:
                        charatersData.stepellier_Data.level_1_DialogueFinished = true;
                        break;

                    default:
                        break;
                }
                break;

            default:
                break;
        }

        SaveData();
    }
}

[Serializable]
public class CharatersData
{
    [Header("Characters")]
    public NPCData floriel_Data;
    public NPCData granith_Data;
    public NPCData archie_Data;
    public NPCData aisa_Data;
    public NPCData mossy_Data;
    public NPCData larry_Data;

    public NPCData stepellier_Data;
}

[Serializable]
public class NPCData
{
    [Header("Level dialogue completions")]
    public bool level_1_DialogueFinished;
    public bool level_2_DialogueFinished;
    public bool level_3_DialogueFinished;
    public bool level_4_DialogueFinished;
    public bool level_5_DialogueFinished;
    public bool level_6_DialogueFinished;

    [Header("Ending Value")]
    public int endingValue;

    [Header("Stats Aquired")]
    public List<DialogueStat> dialogueStatList = new List<DialogueStat>();
}
