using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Windows;

public class HUBTutorial : Singleton<HUBTutorial>
{
    [Header("Document")]
    [SerializeField] TextAsset stepellierTutorialDocument;

    [Header("Data from Excel")]
    public TutorialData tutorialData = new TutorialData();
    int startRow = 2;
    int columns = 10; //Size + 1
    int currentLanguageAmount = 3;

    [Header("Stepellier Object")]
    [SerializeField] GameObject stepellier_Object;
    [SerializeField] List<GameObject> stepellier_MeshObjectList;
    [SerializeField] Animator stepellier_Animator;


    //--------------------


    private void Start()
    {
        HideStepellier();
        BuildTabletTextDatabase();
    }


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetupTutorialSaveStates;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetupTutorialSaveStates;
    }

    void SetupTutorialSaveStates()
    {
        //Build SaveStates of TutorialSegements, if there are none yet
        if (DataManager.Instance.oneTimeRunData_Store.tutorialSegmenet.Count <= 0)
        {
            for (int i = 0; i < tutorialData.tutorialDataSegment.Count; i++)
            {
                DataManager.Instance.oneTimeRunData_Store.tutorialSegmenet.Add(false);
            }
        }
    }


    //--------------------


    public void StartTutorial(int index)
    {
        Stepellier_Enter(index);
    }
    public void EndTutorial(int index)
    {
        Stepellier_Exit(index);
    }


    //--------------------


    #region StepellierSpawn

    void Stepellier_Enter(int index)
    {
        StartCoroutine(SpawnStepellier_Delay(0.2f, index));
    }
    void Stepellier_Exit(int index)
    {
        StartCoroutine(DespawnStepellier_Delay(0.2f, index));
    }

    IEnumerator SpawnStepellier_Delay(float waitTime, int index)
    {
        PlayerManager.Instance.PauseGame();

        stepellier_Animator.SetTrigger(AnimationManager.Instance.effect_Teleport);

        yield return new WaitForSeconds(waitTime);

        stepellier_Object.transform.SetPositionAndRotation(tutorialData.tutorialDataSegment[index].stepellier_spawnPos, RotateStepellier(index));

        ShowStepellier();
        SFX_Respawn.Instance.PlayRespawnSound();

        yield return StartCoroutine(PlayerReactToStepellierSpawning(index));

        EndTutorial(index);
    }
    IEnumerator DespawnStepellier_Delay(float waitTime, int index)
    {
        stepellier_Animator.SetTrigger(AnimationManager.Instance.effect_Teleport);

        yield return new WaitForSeconds(waitTime / 2);

        SFX_Respawn.Instance.PlayRespawnSound();

        yield return new WaitForSeconds(waitTime / 2);

        HideStepellier();

        stepellier_Object.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        yield return new WaitForSeconds(waitTime);

        Movement.Instance.UpdateAvailableMovementBlocks();

        //SHOW THIS AFTER TESTING
        //DataManager.Instance.oneTimeRunData_Store.tutorialSegmenet[index] = true;
        //DataPersistanceManager.instance.SaveGame();

        PlayerManager.Instance.UnpauseGame();
    }
    IEnumerator PlayerReactToStepellierSpawning(int index)
    {
        yield return new WaitForSeconds(0.1f);

        Movement.Instance.RotatePlayerBody(RotatePlayer(index));

        yield return new WaitForSeconds(0.15f);

        yield return PlayerJump();

        yield return new WaitForSeconds(0.15f);
    }

    IEnumerator PlayerJump()
    {
        Transform playerTransform = PlayerManager.Instance.playerBody.transform;

        Vector3 startPos = playerTransform.position;
        Vector3 jumpPos = startPos + Vector3.up * 0.3f;

        float jumpUpTime = 0.12f;
        float jumpDownTime = 0.16f;

        float timer = 0f;

        // Quick surprised jump upward
        while (timer < jumpUpTime)
        {
            timer += Time.deltaTime;
            float t = timer / jumpUpTime;

            playerTransform.position = Vector3.Lerp(startPos, jumpPos, t);

            yield return null;
        }

        timer = 0f;

        // Slightly slower fall back down
        while (timer < jumpDownTime)
        {
            timer += Time.deltaTime;
            float t = timer / jumpDownTime;

            playerTransform.position = Vector3.Lerp(jumpPos, startPos, t);

            yield return null;
        }

        playerTransform.position = startPos;
    }

    Quaternion RotateStepellier(int index)
    {
        MoveDirection moveDir = tutorialData.tutorialDataSegment[index].stepellier_spawnRot;

        switch (moveDir)
        {
            case MoveDirection.Forward:
                return Quaternion.Euler(0, 0, 0);

            case MoveDirection.Backward:
                return Quaternion.Euler(0, 180, 0);

            case MoveDirection.Right:
                return Quaternion.Euler(0, 90, 0);

            case MoveDirection.Left:
                return Quaternion.Euler(0, -90, 0);

            case MoveDirection.None:
                return Quaternion.Euler(0, 0, 0);

            default:
                return Quaternion.identity;
        }
    }
    float RotatePlayer(int index)
    {
        MoveDirection moveDir = tutorialData.tutorialDataSegment[index].stepellier_spawnRot;

        switch (moveDir)
        {
            case MoveDirection.Forward:
                return 0;

            case MoveDirection.Backward:
                return 180;

            case MoveDirection.Right:
                return 90;

            case MoveDirection.Left:
                return -90;

            case MoveDirection.None:
                return 0;

            default:
                return 0;
        }
    }

    void ShowStepellier()
    {
        for (int i = 0; i < stepellier_MeshObjectList.Count; i++)
        {
            stepellier_MeshObjectList[i].SetActive(true);
        }
    }
    void HideStepellier()
    {
        for (int i = 0; i < stepellier_MeshObjectList.Count; i++)
        {
            stepellier_MeshObjectList[i].SetActive(false);
        }
    }

    #endregion


    //--------------------


    #region Dialogue

    void SetupDialogueDisplay()
    {

    }

    #endregion


    //--------------------


    #region Excel Setup

    void BuildTabletTextDatabase()
    {
        ReadExcelSheet();
    }
    public void ReadExcelSheet()
    {
        if (stepellierTutorialDocument == null) return;

        //Separate Excel Sheet into a string[] by its ";"
        string[] excelData = stepellierTutorialDocument.text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);

        // Calculate the size of the Excel table
        int excelTableSize = (excelData.Length / columns - 1) - 0;

        // Initialize the list
        tutorialData.tutorialDataSegment = new List<TutorialDataSegment>();

        // Populate the list with default DataObject instances
        for (int i = 0; i < excelTableSize; i++)
        {
            TutorialDataSegment tutorialSegment = new TutorialDataSegment();

            for (int j = 0; j < currentLanguageAmount; j++)
            {
                tutorialSegment.languageDialogueList.Add(null);
            }

            tutorialData.tutorialDataSegment.Add(tutorialSegment);
        }

        //Fill the new element with data
        for (int i = 0; i < excelTableSize; i++)
        {
            #region Description

            //Segment Description
            if (excelData[columns * (i + startRow - 1) + 0] != "")
                tutorialData.tutorialDataSegment[i].segmentDescription = excelData[columns * (i + startRow - 1) + 0].Trim();
            else
                tutorialData.tutorialDataSegment[i].segmentDescription = "";

            #endregion

            #region Position and Rotation

            //Position X
            if (excelData[columns * (i + startRow - 1) + 1] != "")
                tutorialData.tutorialDataSegment[i].stepellier_spawnPos.x = ParseIntSafe(excelData, columns * (i + startRow - 1) + 1);
            else
                tutorialData.tutorialDataSegment[i].stepellier_spawnPos.x = 0;

            //Position Y
            if (excelData[columns * (i + startRow - 1) + 2] != "")
                tutorialData.tutorialDataSegment[i].stepellier_spawnPos.y = ParseIntSafe(excelData, columns * (i + startRow - 1) + 2);
            else
                tutorialData.tutorialDataSegment[i].stepellier_spawnPos.y = 0;

            //Position Z
            if (excelData[columns * (i + startRow - 1) + 3] != "")
                tutorialData.tutorialDataSegment[i].stepellier_spawnPos.z = ParseIntSafe(excelData, columns * (i + startRow - 1) + 3);
            else
                tutorialData.tutorialDataSegment[i].stepellier_spawnPos.z = 0;

            //Rotation
            if (excelData[columns * (i + startRow - 1) + 4] != "")
                tutorialData.tutorialDataSegment[i].stepellier_spawnRot = SetRotationValue(excelData[columns * (i + startRow - 1) + 4].Trim());
            else
                tutorialData.tutorialDataSegment[i].stepellier_spawnRot = MoveDirection.None;

            #endregion

            #region Talk Animation

            //Segment Description
            if (excelData[columns * (i + startRow - 1) + 5] != "")
                tutorialData.tutorialDataSegment[i].talkAnimation = ParseIntSafe(excelData, columns * (i + startRow - 1) + 5);
            else
                tutorialData.tutorialDataSegment[i].talkAnimation = 0;

            #endregion

            #region Languages

            for (int j = 0; j < currentLanguageAmount; j++)
            {
                if (excelData[columns * (i + startRow - 1) + 6 + j] != "")
                    tutorialData.tutorialDataSegment[i].languageDialogueList[j] = excelData[columns * (i + startRow - 1) + 6 + j].Trim();
                else
                    tutorialData.tutorialDataSegment[i].languageDialogueList[j] = "";
            }

            #endregion

            CleanTheTextDialogue(i);
        }

        //Remove elements that doesn't have a name
        tutorialData.tutorialDataSegment = tutorialData.tutorialDataSegment.Where(obj => obj != null && !string.IsNullOrEmpty(obj.segmentDescription)).ToList();
    }
    void CleanTheTextDialogue(int i)
    {
        tutorialData.tutorialDataSegment[i].segmentDescription = CleanQuotes(tutorialData.tutorialDataSegment[i].segmentDescription);

        for (int j = 0; j < tutorialData.tutorialDataSegment[i].languageDialogueList.Count; j++)
        {
            tutorialData.tutorialDataSegment[i].languageDialogueList[j] = CleanQuotes(tutorialData.tutorialDataSegment[i].languageDialogueList[j]);
        }
    }
    string CleanQuotes(string input)
    {
        // Remove enclosing quotes, and replace double double-quotes with a single one
        if (input == "")
        {
            return "";
        }
        else if (input == null)
        {
            return "";
        }
        else if (input.StartsWith("\"") && input.EndsWith("\""))
        {
            input = input.Substring(1, input.Length - 2);
        }

        return input.Replace("\"\"", "\"").Trim();
    }
    int ParseIntSafe(string[] data, int index)
    {
        if (index >= 0 && index < data.Length && int.TryParse(data[index], out int result))
            return result;
        return -1;
    }
    MoveDirection SetRotationValue(string rotText)
    {
        if (rotText == "Forward")
            return MoveDirection.Forward;
        else if (rotText == "Back")
            return MoveDirection.Backward;
        else if (rotText == "Left")
            return MoveDirection.Left;
        else if (rotText == "Right")
            return MoveDirection.Right;

        return MoveDirection.None;
    }
    
    #endregion
}


[Serializable]
public class TutorialData
{
    public List<TutorialDataSegment> tutorialDataSegment = new List<TutorialDataSegment>();
}
[Serializable]
public class TutorialDataSegment
{
    public string segmentDescription;

    [Header("Dialogue Languages")]
    public List<string> languageDialogueList = new List<string>();

    [Header("Stepellier Spawn Position")]
    public Vector3 stepellier_spawnPos;
    public MoveDirection stepellier_spawnRot;

    [Header("TalkAnimation")]
    public int talkAnimation;
}