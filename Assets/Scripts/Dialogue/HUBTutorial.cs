using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.Rendering.HableCurve;

public class HUBTutorial : Singleton<HUBTutorial>
{
    [Header("Document")]
    [SerializeField] TextAsset stepellierTutorialDocument_Keyboard;
    [SerializeField] TextAsset stepellierTutorialDocument_PlayStation;
    [SerializeField] TextAsset stepellierTutorialDocument_xBox;

    [Header("Data from Excel")]
    public TutorialData tutorialData = new TutorialData();
    int startRow = 2;
    int columns = 11; //Size + 1
    int currentLanguageAmount = 3;

    [Header("Stepellier Object")]
    [SerializeField] GameObject stepellier_Object;
    [SerializeField] List<GameObject> stepellier_MeshObjectList;
    [SerializeField] Animator stepellier_Animator;

    [Header("Dialogue Display")]
    [SerializeField] GameObject dialogueDisplayCanvas_Parent;
    [SerializeField] TextMeshProUGUI dialogueDisplay_Text;
    [SerializeField] TextMeshProUGUI dialogueDisplay_Name;

    [Header("Current Segment showing")]
    [SerializeField] int currentSegmentShowing = -1;
    [SerializeField] TutorialParts currentTutorialPart;

    float playerRotY_Temp;


    //--------------------


    private void Start()
    {
        PlayerManager.Instance.PauseGame();

        HideStepellier();
        HideArrow();

        BuildTutorialTextDatabase();
    }


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetupTutorialSaveStates;
        DataManager.Action_dataHasLoaded += SetupDialogueRefferences;

        Player_KeyInputs.Action_dialogueNextButton_isPressed += StartNewSegment;

        TypewriterEffect.Action_Typewriting_Finished += ShowArrow;

        DataManager.Action_dataHasLoaded += SetupTutorial_Movement;
        Movement.Action_RespawnPlayerLate += SetupTutorial_Respawn;
        DataManager.Action_dataHasLoaded += UnPauseGame;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetupTutorialSaveStates;
        DataManager.Action_dataHasLoaded -= SetupDialogueRefferences;

        Player_KeyInputs.Action_dialogueNextButton_isPressed -= StartNewSegment;

        TypewriterEffect.Action_Typewriting_Finished -= ShowArrow;

        DataManager.Action_dataHasLoaded -= SetupTutorial_Movement;
        Movement.Action_RespawnPlayerLate -= SetupTutorial_Respawn;
        DataManager.Action_dataHasLoaded -= UnPauseGame;
    }

    void SetupTutorialSaveStates()
    {
        //Build SaveStates of TutorialSegements, if there are none yet
        if (DataManager.Instance.oneTimeRunData_Store.tutorialSegmenets.Count <= 0)
        {
            int enumSize = Enum.GetValues(typeof(TutorialParts)).Length;

            for (int i = 1; i < enumSize; i++)
            {
                TutorialDataParts tutorialParts = new TutorialDataParts();
                tutorialParts.tutorialParts = (TutorialParts)i;
                tutorialParts.isGoneThrough = false;

                DataManager.Instance.oneTimeRunData_Store.tutorialSegmenets.Add(tutorialParts);
            }
        }

        DataPersistanceManager.instance.SaveGame();
    }
    void SetupDialogueRefferences()
    {
        dialogueDisplayCanvas_Parent = DialogueManager.Instance.dialogueCanvas;
        dialogueDisplay_Name = DialogueManager.Instance.nameText;
    }


    //--------------------


    #region Tutorial Setup

    public void StartTutorial(TutorialParts tutorialPart)
    {
        PlayerManager.Instance.PauseGame();

        GetCorrectSegment((int)tutorialPart, tutorialPart);

        StartCoroutine(StartTutorial_Delay());
    }
    public void EndTutorial()
    {
        StartCoroutine(EndTutorial_Delay());
    }

    IEnumerator StartTutorial_Delay()
    {
        yield return StartCoroutine(Stepellier_Enter());

        yield return ShowDialogueDisplay();
    }
    IEnumerator EndTutorial_Delay()
    {
        yield return CloseDialogueDisplay();

        yield return StartCoroutine(Stepellier_Exit());
    }

    void GetCorrectSegment(int index, TutorialParts tutorialPart)
    {
        int tempSegment = -1;

        for (int i = 0; i < tutorialData.tutorialDataSegment.Count; i++)
        {
            if (tutorialData.tutorialDataSegment[i].segmentNumber == index)
            {
                tempSegment = i;
                break;
            }
        }

        currentSegmentShowing = tempSegment;
        currentTutorialPart = tutorialPart;
    }


    //-----


    void SetupTutorial_Movement()
    {
        StartCoroutine(SetupTutorial_Movement_Delay(1.4f));
    }
    IEnumerator SetupTutorial_Movement_Delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        StartTutorial(TutorialParts.Movement);
    }
    void SetupTutorial_Respawn()
    {
        if (Movement.Instance.isRespawningFirstTime)
        {
            //Set new SpawnPos for Stepellier


            StartCoroutine(SetupTutorial_Respawn_Delay(0.4f));
        }
    }
    IEnumerator SetupTutorial_Respawn_Delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        StartTutorial(TutorialParts.Respawn);
    }

    void UnPauseGame()
    {
        PlayerManager.Instance.UnpauseGame();
    }

    #endregion


    //--------------------


    #region StepellierSpawn

    IEnumerator Stepellier_Enter()
    {
        //Spawn in Stepellier
        yield return StartCoroutine(SpawnStepellier_Delay(0.2f));

        //Set Player reaction (and rotation) to Stepellier
        yield return StartCoroutine(PlayerReactToStepellierSpawning());
    }
    IEnumerator Stepellier_Exit()
    {
        //Despawn Stepellier
        yield return StartCoroutine(DespawnStepellier_Delay(0.2f));

        yield return PlayerRotateBackToOriginalRotation();
    }


    //-----


    IEnumerator SpawnStepellier_Delay(float waitTime)
    {
        stepellier_Animator.SetTrigger(AnimationManager.Instance.effect_Teleport);

        yield return new WaitForSeconds(waitTime);

        stepellier_Object.transform.SetPositionAndRotation(tutorialData.tutorialDataSegment[currentSegmentShowing].stepellier_spawnPos, RotateStepellier());

        ShowStepellier();
        SFX_Respawn.Instance.PlayRespawnSound();

    }
    IEnumerator DespawnStepellier_Delay(float waitTime)
    {
        stepellier_Animator.SetTrigger(AnimationManager.Instance.effect_Teleport);

        yield return new WaitForSeconds(waitTime / 2);

        SFX_Respawn.Instance.PlayRespawnSound();

        yield return new WaitForSeconds(waitTime / 2);

        HideStepellier();

        stepellier_Object.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        yield return new WaitForSeconds(waitTime);

        Movement.Instance.UpdateAvailableMovementBlocks();

        yield return new WaitForSeconds(waitTime);

        //SHOW THIS AFTER TESTING TO PREVENT THE PLAYER OF GOING TROUGH THE SAME TUTORIAL AGAIN
        //for (int i = 0; i < DataManager.Instance.oneTimeRunData_Store.tutorialSegmenets.Count; i++)
        //{
        //    if (DataManager.Instance.oneTimeRunData_Store.tutorialSegmenets[i].tutorialParts == currentTutorialPart)
        //    {
        //        DataManager.Instance.oneTimeRunData_Store.tutorialSegmenets[i].isGoneThrough = true;
        //        DataPersistanceManager.instance.SaveGame();

        //        break;
        //    }
        //}

        PlayerManager.Instance.UnpauseGame();
    }
    
    IEnumerator PlayerReactToStepellierSpawning()
    {
        yield return new WaitForSeconds(0.1f);

        playerRotY_Temp = PlayerManager.Instance.playerBody.transform.position.y;
        Movement.Instance.RotatePlayerBody(RotatePlayer());

        yield return new WaitForSeconds(0.15f);

        yield return PlayerJump();

        yield return new WaitForSeconds(0.15f);
    }
    IEnumerator PlayerRotateBackToOriginalRotation()
    {
        Movement.Instance.RotatePlayerBody(playerRotY_Temp);

        yield return new WaitForSeconds(0.1f);
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

    Quaternion RotateStepellier()
    {
        MoveDirection moveDir = tutorialData.tutorialDataSegment[currentSegmentShowing].stepellier_spawnRot;

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
    float RotatePlayer()
    {
        MoveDirection moveDir = tutorialData.tutorialDataSegment[currentSegmentShowing].stepellier_spawnRot;

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

    IEnumerator ShowDialogueDisplay()
    {
        yield return new WaitForSeconds(0f);

        SetupStepellierNameText_toDisplay();

        if (!dialogueDisplayCanvas_Parent.activeInHierarchy)
        {
            dialogueDisplayCanvas_Parent.SetActive(true);
        }

        yield return new WaitForSeconds(0.4f);

        PlayerManager.Instance.npcInteraction = true;
        SelectSegment();
    }
    IEnumerator CloseDialogueDisplay()
    {
        yield return new WaitForSeconds(0f);

        if (DialogueManager.Instance.closingMenuAnimatorList.Count > 0)
        {
            for (int i = 0; i < DialogueManager.Instance.closingMenuAnimatorList.Count; i++)
            {
                DialogueManager.Instance.closingMenuAnimatorList[i].SetTrigger("Close");
            }
        }

        yield return new WaitForSeconds(DialogueManager.Instance.closingMenuDelay);

        dialogueDisplayCanvas_Parent.SetActive(false);
    }


    void StartNewSegment()
    {
        if (TypewriterEffect.Instance.isTyping)
        {
            TypewriterEffect.Instance.SkipTypewriter();
        }
        else if (tutorialData.tutorialDataSegment[currentSegmentShowing].isEnding)
        {
            PlayerManager.Instance.npcInteraction = false;

            SetupDialogueText_toDisplay("");
            EndTutorial();
        }
        else
        {
            currentSegmentShowing++;
            SelectSegment();
        }
    }
    void SelectSegment()
    {
        HideArrow();

        SetupDialogueText_toDisplay(tutorialData.tutorialDataSegment[currentSegmentShowing].languageDialogueList[(int)DataManager.Instance.settingData_StoreList.currentLanguage]);
    }

    void SetupStepellierNameText_toDisplay()
    {
        dialogueDisplay_Name.text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Antagonist;
    }
    void SetupDialogueText_toDisplay(string _text)
    {
        TypewriterEffect.Instance.ShowText(_text);
    }


    void ShowArrow()
    {
        DialogueManager.Instance.activeNPC = NPCs.Stepellier;
        DialogueManager.Instance.ShowArrow();
    }
    void HideArrow()
    {
        DialogueManager.Instance.HideArrow();
    }

    #endregion


    //--------------------


    #region Excel Setup

    void BuildTutorialTextDatabase()
    {
        ReadExcelSheet();
    }
    public void ReadExcelSheet()
    {
        TextAsset activeTutorialDocument = null;

        if (ControllerState.Instance.activeController == InputType.Keyboard)
            activeTutorialDocument = stepellierTutorialDocument_Keyboard;
        else if (ControllerState.Instance.activeController == InputType.PlayStation)
            activeTutorialDocument = stepellierTutorialDocument_PlayStation;
        else if (ControllerState.Instance.activeController == InputType.Xbox)
            activeTutorialDocument = stepellierTutorialDocument_xBox;

        if (activeTutorialDocument == null) return;

        // Separate Excel Sheet into a string[] by its ";" and line breaks
        string[] excelData = activeTutorialDocument.text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);

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

            if (excelData[columns * (i + startRow - 1) + 0] != "")
                tutorialData.tutorialDataSegment[i].segmentDescription = excelData[columns * (i + startRow - 1) + 0].Trim();
            else
                tutorialData.tutorialDataSegment[i].segmentDescription = "";

            #endregion

            #region Segment

            if (excelData[columns * (i + startRow - 1) + 1] != "")
                tutorialData.tutorialDataSegment[i].segmentNumber = ParseIntSafe(excelData, columns * (i + startRow - 1) + 1);
            else
                tutorialData.tutorialDataSegment[i].segmentNumber = 0;

            #endregion

            #region Is Ending

            if (excelData[columns * (i + startRow - 1) + 2] != "")
                tutorialData.tutorialDataSegment[i].isEnding = true;
            else
                tutorialData.tutorialDataSegment[i].isEnding = false;

            #endregion

            #region Position and Rotation

            //Position X
            if (excelData[columns * (i + startRow - 1) + 3] != "")
                tutorialData.tutorialDataSegment[i].stepellier_spawnPos.x = ParseIntSafe(excelData, columns * (i + startRow - 1) + 3);
            else
                tutorialData.tutorialDataSegment[i].stepellier_spawnPos.x = 0;

            //Position Y
            if (excelData[columns * (i + startRow - 1) + 4] != "")
                tutorialData.tutorialDataSegment[i].stepellier_spawnPos.y = ParseIntSafe(excelData, columns * (i + startRow - 1) + 4);
            else
                tutorialData.tutorialDataSegment[i].stepellier_spawnPos.y = 0;

            //Position Z
            if (excelData[columns * (i + startRow - 1) + 5] != "")
                tutorialData.tutorialDataSegment[i].stepellier_spawnPos.z = ParseIntSafe(excelData, columns * (i + startRow - 1) + 5);
            else
                tutorialData.tutorialDataSegment[i].stepellier_spawnPos.z = 0;

            //Rotation
            if (excelData[columns * (i + startRow - 1) + 6] != "")
                tutorialData.tutorialDataSegment[i].stepellier_spawnRot = SetRotationValue(excelData[columns * (i + startRow - 1) + 6].Trim());
            else
                tutorialData.tutorialDataSegment[i].stepellier_spawnRot = MoveDirection.None;

            #endregion

            #region Talk Animation

            if (excelData[columns * (i + startRow - 1) + 7] != "")
                tutorialData.tutorialDataSegment[i].talkAnimation = ParseIntSafe(excelData, columns * (i + startRow - 1) + 7);
            else
                tutorialData.tutorialDataSegment[i].talkAnimation = 0;

            #endregion

            #region Languages

            for (int j = 0; j < currentLanguageAmount; j++)
            {
                if (excelData[columns * (i + startRow - 1) + 8 + j] != "")
                    tutorialData.tutorialDataSegment[i].languageDialogueList[j] = excelData[columns * (i + startRow - 1) + 8 + j].Trim();
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

    [Header("Segment")]
    public int segmentNumber;

    [Header("Ending")]
    public bool isEnding;
}

[Serializable]
public class TutorialDataParts
{
    public TutorialParts tutorialParts = TutorialParts.None;
    public bool isGoneThrough;
}