using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEngine;


public class NPCDialogueSheets : Singleton<NPCDialogueSheets>
{
    [Header("Document")]
    [SerializeField] List<TextAsset> NPC_List;

    [Header("Stats from Excel")]
    [SerializeField] int startRow;
    [SerializeField] int columns;
    [SerializeField] List<string> languageNames;

    [Header("DialogueObjectList")]
    public List<DialogueObjectList> newDataObjectList = new List<DialogueObjectList>();


    //--------------------


    private void Start()
    {
        for (int i = 0; i < NPC_List.Count; i++)
        {
            DialogueObjectList dataObjectList = new DialogueObjectList();

            newDataObjectList.Add(dataObjectList);

            ReadExcelSheet(newDataObjectList.Count - 1);
        }
    }


    //--------------------


    public void ReadExcelSheet(int index)
    {
        //Separate Excel Sheet into a string[] by its ";"
        string[] excelData = NPC_List[index].text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);

        // Calculate the size of the Excel table
        int excelTableSize = (excelData.Length / columns - 1) - 0;

        // Initialize the list
        newDataObjectList[index].dialogueList = new List<DialogueObject>();

        // Populate the list with default DataObject instances
        for (int i = 0; i < excelTableSize; i++)
        {
            newDataObjectList[index].listName = NPC_List[index].name;
            newDataObjectList[index].dialogueList.Add(new DialogueObject());
        }

        print("DataList: " + newDataObjectList[index].dialogueList.Count + " | " + excelTableSize);

        //Fill the new element with data
        for (int i = 0; i < excelTableSize; i++)
        {
            int result;
            newDataObjectList[index].dialogueList[i] = new DialogueObject();

            //Description
            if (excelData[columns * (i + startRow - 1) + 1] != "")
                newDataObjectList[index].dialogueList[i].description = excelData[columns * (i + startRow - 1) + 1].Trim();
            else
                newDataObjectList[index].dialogueList[i].description = "";

            for (int j = 0; j < languageNames.Count; j++)
            {
                newDataObjectList[index].dialogueList[i].language.Add("");

                if (excelData[columns * (i + startRow - 1) + 2 + j] != "")
                    newDataObjectList[index].dialogueList[i].language[j] = excelData[columns * (i + startRow - 1) + 2 + j].Trim();
                else
                    newDataObjectList[index].dialogueList[i].language[j] = "";
            }
        }

        //Remove elements that doesn't have a name
        newDataObjectList[index].dialogueList = newDataObjectList[index].dialogueList.Where(obj => obj != null && !string.IsNullOrEmpty(obj.description)).ToList();
    }
}

[Serializable]
public class DialogueObjectList
{
    [HideInInspector] public string listName;

    public List<DialogueObject> dialogueList = new List<DialogueObject>();
}

[Serializable]
public class DialogueObject
{
    [Header("Language versions")]
    [HideInInspector] public string description;

    public List<string> language = new List<string>();
}
