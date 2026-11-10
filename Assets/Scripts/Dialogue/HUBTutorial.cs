using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using TMPro;
using UnityEngine;

public class HUBTutorial : Singleton<HUBTutorial>
{
    [Header("Document")]
    [SerializeField] TextAsset stepellierTutorialDocument_Keyboard;
    [SerializeField] TextAsset stepellierTutorialDocument_PlayStation;
    [SerializeField] TextAsset stepellierTutorialDocument_xBox;

    [Header("Data from Excel")]
    public TutorialData tutorialData = new TutorialData();
    const int columns = 13;
    const int currentLanguageAmount = 3;

    // Guards against events arriving before Start()/data loading is complete.
    bool tutorialDatabaseBuilt;
    bool tutorialSaveStatesReady;
    bool playerSpawned;
    bool tutorialStartupQueued;
    bool tutorialSequenceActive;
    bool dialogueIsVisible;
    bool selectingSegment;

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
        TryStartMovementTutorial();
        // If save data arrived during OnEnable(), Start() must not re-pause
        // normal gameplay when no tutorial was launched.
        if (tutorialSaveStatesReady && !tutorialStartupQueued && !tutorialSequenceActive)
            PlayerManager.Instance.UnpauseGame();
    }


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetupTutorialSaveStates;
        DataManager.Action_dataHasLoaded += SetupDialogueRefferences;

        Player_KeyInputs.Action_dialogueNextButton_isPressed += StartNewSegment;

        TypewriterEffect.Action_Typewriting_Finished += ShowArrow;

        PlayerSpawnScript.Action_PlayerHasSpawned += SetupTutorial_Movement;
        //Movement.Action_RespawnPlayerLate += SetupTutorial_Respawn;
        DataManager.Action_dataHasLoaded += UnPauseGame;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetupTutorialSaveStates;
        DataManager.Action_dataHasLoaded -= SetupDialogueRefferences;

        Player_KeyInputs.Action_dialogueNextButton_isPressed -= StartNewSegment;

        TypewriterEffect.Action_Typewriting_Finished -= ShowArrow;

        PlayerSpawnScript.Action_PlayerHasSpawned -= SetupTutorial_Movement;
        //Movement.Action_RespawnPlayerLate -= SetupTutorial_Respawn;
        DataManager.Action_dataHasLoaded -= UnPauseGame;
    }

    void SetupTutorialSaveStates()
    {
        if (DataManager.Instance == null)
        {
            Debug.LogError("[HUBTutorial] Cannot initialize tutorial save states: save data is unavailable.", this);
            return;
        }

        var states = DataManager.Instance.oneTimeRunData_Store.tutorialSegmenets;
        if (states == null)
        {
            Debug.LogError("[HUBTutorial] tutorialSegmenets is null in the loaded save data.", this);
            return;
        }

        bool addedStates = false;
        foreach (TutorialParts part in Enum.GetValues(typeof(TutorialParts)))
        {
            if (part == TutorialParts.None) continue;
            if (states.Any(state => state != null && state.tutorialParts == part)) continue;

            states.Add(new TutorialDataParts
            {
                tutorialParts = part,
                isGoneThrough = false
            });
            addedStates = true;
        }

        tutorialSaveStatesReady = true;
        if (addedStates && DataPersistanceManager.instance != null)
            DataPersistanceManager.instance.SaveGame();

        TryStartMovementTutorial();
    }
    void SetupDialogueRefferences()
    {
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("[HUBTutorial] DialogueManager is not available.", this);
            return;
        }

        dialogueDisplayCanvas_Parent = DialogueManager.Instance.dialogueCanvas;
        dialogueDisplay_Name = DialogueManager.Instance.nameText;
    }


    //--------------------


    #region Tutorial Setup

    public void StartTutorial(TutorialParts tutorialPart)
    {
        if (tutorialSequenceActive) return;
        if (!tutorialDatabaseBuilt)
        {
            Debug.LogError("[HUBTutorial] Tutorial data has not been loaded yet.", this);
            return;
        }
        if (!GetCorrectSegment((int)tutorialPart, tutorialPart)) return;

        tutorialSequenceActive = true;
        dialogueIsVisible = false;
        selectingSegment = false;
        PlayerManager.Instance.PauseGame();
        StartCoroutine(StartTutorial_Delay());
    }

    public void EndTutorial()
    {
        if (!tutorialSequenceActive || !dialogueIsVisible) return;
        dialogueIsVisible = false;
        StartCoroutine(EndTutorial_Delay());
    }

    IEnumerator StartTutorial_Delay()
    {
        // Camera must finish rotating before the tutorial continues.
        yield return StartCoroutine(RotateCameraAndWait());

        // Only now spawn Stepellier.
        yield return StartCoroutine(Stepellier_Enter());

        yield return ShowDialogueDisplay();
    }
    IEnumerator EndTutorial_Delay()
    {
        yield return CloseDialogueDisplay();

        yield return StartCoroutine(Stepellier_Exit());
    }

    bool GetCorrectSegment(int index, TutorialParts tutorialPart)
    {
        int tempSegment = tutorialData.tutorialDataSegment.FindIndex(segment =>
            segment != null && segment.segmentNumber == index);

        if (tempSegment < 0)
        {
            Debug.LogError($"[HUBTutorial] No starting dialogue for {tutorialPart} (Segment={index}). Check the CSV and TutorialParts enum.", this);
            return false;
        }

        currentSegmentShowing = tempSegment;
        currentTutorialPart = tutorialPart;
        return true;
    }

    bool IsCurrentSegmentValid()
    {
        return tutorialData != null && tutorialData.tutorialDataSegment != null &&
               currentSegmentShowing >= 0 &&
               currentSegmentShowing < tutorialData.tutorialDataSegment.Count;
    }

    // Rotate a single step at a time: multiple RotateCameraX/Y calls in
    // the same frame can be discarded while the camera is already rotating.
    IEnumerator RotateCameraAndWait()
    {
        if (!IsCurrentSegmentValid()) yield break;
        if (CameraController.Instance == null || Movement.Instance == null)
        {
            Debug.LogError("[HUBTutorial] CameraController or Movement is missing.", this);
            yield break;
        }

        while (Movement.Instance.GetMovementState() == MovementStates.Moving)
            yield return null;

        while (CameraController.Instance.isRotating)
            yield return null;

        MoveDirection requested = tutorialData.tutorialDataSegment[currentSegmentShowing].camera_Rotation;
        CameraRotationState target;
        switch (requested)
        {
            case MoveDirection.Forward: target = CameraRotationState.Forward; break;
            case MoveDirection.Backward: target = CameraRotationState.Backward; break;
            case MoveDirection.Left: target = CameraRotationState.Left; break;
            case MoveDirection.Right: target = CameraRotationState.Right; break;
            default: yield break; // Blank rotation: do not rotate the camera.
        }

        // +1: Forward -> Left -> Backward -> Right -> Forward.
        CameraRotationState[] order =
        {
            CameraRotationState.Forward,
            CameraRotationState.Left,
            CameraRotationState.Backward,
            CameraRotationState.Right
        };

        int from = Array.IndexOf(order, CameraController.Instance.cameraRotationState);
        int to = Array.IndexOf(order, target);
        if (from < 0 || to < 0)
        {
            Debug.LogWarning("[HUBTutorial] Camera rotation state not present in the known rotation order.", this);
            yield break;
        }

        int positiveSteps = (to - from + order.Length) % order.Length;
        int negativeSteps = (from - to + order.Length) % order.Length;
        bool positive = positiveSteps <= negativeSteps;
        int steps = positive ? positiveSteps : negativeSteps;

        for (int i = 0; i < steps; i++)
        {
            bool normal = DataManager.Instance.settingData_StoreList.currentRevertedCameraMotion
                          == RevertedCameraMotion.Normal;
            if (positive == normal)
                CameraController.Instance.RotateCameraX();
            else
                CameraController.Instance.RotateCameraY();

            // Allow the animation to enter the isRotating state before checking it.
            yield return null;
            while (CameraController.Instance.isRotating)
                yield return null;
        }
    }

    //-----


    void SetupTutorial_Movement()
    {
        playerSpawned = true;
        TryStartMovementTutorial();
    }

    void TryStartMovementTutorial()
    {
        if (!playerSpawned || !tutorialSaveStatesReady || !tutorialDatabaseBuilt ||
            tutorialStartupQueued || tutorialSequenceActive)
            return;

        var states = DataManager.Instance.oneTimeRunData_Store.tutorialSegmenets;
        TutorialDataParts startState = states.FirstOrDefault(state =>
            state != null && state.tutorialParts == TutorialParts.Start);

        if (startState == null)
        {
            Debug.LogError("[HUBTutorial] TutorialParts.Start is missing from the save states.", this);
            return;
        }
        if (startState.isGoneThrough)
        {
            playerSpawned = false;
            return;
        }

        playerSpawned = false;
        tutorialStartupQueued = true;
        PlayerManager.Instance.PauseGame();
        StartCoroutine(SetupTutorial_Movement_Delay(0f));
    }

    IEnumerator SetupTutorial_Movement_Delay(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        tutorialStartupQueued = false;
        StartTutorial(TutorialParts.Start);
    }
    //void SetupTutorial_Respawn()
    //{
    //    //1 is Respawn Tutorial
    //    if (DataManager.Instance.oneTimeRunData_Store.tutorialSegmenets[1].isGoneThrough) return;

    //    if (Movement.Instance.isRespawningFirstTime)
    //    {
    //        PlayerManager.Instance.PauseGame();

    //        //Set new SpawnPos for Stepellier based on which chekpoint the player respawns to


    //        StartCoroutine(SetupTutorial_Respawn_Delay(0.5f));
    //    }
    //}
    //IEnumerator SetupTutorial_Respawn_Delay(float waitTime)
    //{
    //    yield return new WaitForSecondsRealtime(waitTime);

    //    StartTutorial(TutorialParts.Respawn);
    //}

    void UnPauseGame()
    {
        // The save-loaded event can fire after a tutorial has already been queued.
        if (!tutorialStartupQueued && !tutorialSequenceActive)
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
        yield return StartCoroutine(DespawnStepellier_Delay(0.2f));
        tutorialSequenceActive = false;
        selectingSegment = false;
        currentSegmentShowing = -1;
        currentTutorialPart = TutorialParts.None;
    }


    //-----


    IEnumerator SpawnStepellier_Delay(float waitTime)
    {
        stepellier_Animator.SetTrigger(AnimationManager.Instance.effect_Teleport);

        yield return new WaitForSecondsRealtime(waitTime);

        stepellier_Object.transform.SetPositionAndRotation(tutorialData.tutorialDataSegment[currentSegmentShowing].stepellier_spawnPos, RotateStepellier());

        ShowStepellier();
        SFX_Respawn.Instance.PlayRespawnSound();

    }
    IEnumerator DespawnStepellier_Delay(float waitTime)
    {
        stepellier_Animator.SetTrigger(AnimationManager.Instance.effect_Teleport);

        yield return new WaitForSecondsRealtime(waitTime / 2);

        SFX_Respawn.Instance.PlayRespawnSound();

        yield return new WaitForSecondsRealtime(waitTime / 2);

        HideStepellier();

        stepellier_Object.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        yield return new WaitForSecondsRealtime(waitTime);

        Movement.Instance.UpdateAvailableMovementBlocks();

        yield return new WaitForSecondsRealtime(waitTime);

        //Save the tutorial State
        var states = DataManager.Instance.oneTimeRunData_Store.tutorialSegmenets;
        TutorialDataParts completed = states.FirstOrDefault(state =>
            state != null && state.tutorialParts == currentTutorialPart);
        if (completed != null)
        {
            completed.isGoneThrough = true;
            if (DataPersistanceManager.instance != null)
                DataPersistanceManager.instance.SaveGame();
        }

        PlayerManager.Instance.UnpauseGame();

        Movement.Instance.SetDarkenBlocks();
    }

    IEnumerator PlayerReactToStepellierSpawning()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        playerRotY_Temp = PlayerManager.Instance.playerBody.transform.eulerAngles.y;
        if (tutorialData.tutorialDataSegment[currentSegmentShowing].player_Rot != MoveDirection.None)
            Movement.Instance.RotatePlayerBody(RotatePlayer());

        yield return new WaitForSecondsRealtime(0.15f);

        yield return PlayerJump();

        yield return new WaitForSecondsRealtime(0.15f);
    }
    IEnumerator PlayerRotateBackToOriginalRotation()
    {
        Movement.Instance.RotatePlayerBody(playerRotY_Temp);

        yield return new WaitForSecondsRealtime(0.25f);
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
            timer += Time.unscaledDeltaTime;
            float t = timer / jumpUpTime;

            playerTransform.position = Vector3.Lerp(startPos, jumpPos, t);

            yield return null;
        }

        timer = 0f;

        // Slightly slower fall back down
        while (timer < jumpDownTime)
        {
            timer += Time.unscaledDeltaTime;
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
        MoveDirection moveDir = tutorialData.tutorialDataSegment[currentSegmentShowing].player_Rot;

        switch (moveDir)
        {
            case MoveDirection.Forward:
                return 0;

            case MoveDirection.Backward:
                return 180;

            case MoveDirection.Right:
                return -90;

            case MoveDirection.Left:
                return 90;

            case MoveDirection.None:
                return 0;

            default:
                return 0;
        }
    }

    void ShowStepellier()
    {
        if (stepellier_MeshObjectList == null) return;
        foreach (GameObject mesh in stepellier_MeshObjectList)
            if (mesh != null) mesh.SetActive(true);
    }
    void HideStepellier()
    {
        if (stepellier_MeshObjectList == null) return;
        foreach (GameObject mesh in stepellier_MeshObjectList)
            if (mesh != null) mesh.SetActive(false);
    }

    #endregion


    //--------------------


    #region Dialogue

    IEnumerator ShowDialogueDisplay()
    {
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("[HUBTutorial] DialogueManager is unavailable while opening the tutorial.", this);
            PlayerManager.Instance.UnpauseGame();
            tutorialSequenceActive = false;
            yield break;
        }
        SetupDialogueRefferences();
        if (dialogueDisplayCanvas_Parent == null || dialogueDisplay_Name == null ||
            TypewriterEffect.Instance == null)
        {
            Debug.LogError("[HUBTutorial] Dialogue canvas, name text, or TypewriterEffect is missing.", this);
            PlayerManager.Instance.UnpauseGame();
            tutorialSequenceActive = false;
            yield break;
        }

        SetupStepellierNameText_toDisplay();
        if (!dialogueDisplayCanvas_Parent.activeInHierarchy)
            dialogueDisplayCanvas_Parent.SetActive(true);

        yield return new WaitForSecondsRealtime(0.4f);
        PlayerManager.Instance.npcInteraction = true;
        dialogueIsVisible = true;
        yield return StartCoroutine(SelectSegment());
    }
    IEnumerator CloseDialogueDisplay()
    {
        yield return new WaitForSecondsRealtime(0f);

        if (DialogueManager.Instance.closingMenuAnimatorList.Count > 0)
        {
            for (int i = 0; i < DialogueManager.Instance.closingMenuAnimatorList.Count; i++)
            {
                DialogueManager.Instance.closingMenuAnimatorList[i].SetTrigger("Close");
            }
        }

        yield return new WaitForSecondsRealtime(DialogueManager.Instance.closingMenuDelay);

        dialogueDisplayCanvas_Parent.SetActive(false);
    }


    void StartNewSegment()
    {
        // The dialogue-next event is global. Ignore presses outside this tutorial.
        if (!tutorialSequenceActive || !dialogueIsVisible || selectingSegment ||
            !IsCurrentSegmentValid() || TypewriterEffect.Instance == null)
            return;

        if (TypewriterEffect.Instance.isTyping)
        {
            TypewriterEffect.Instance.SkipTypewriter();
            return;
        }

        if (tutorialData.tutorialDataSegment[currentSegmentShowing].isEnding)
        {
            PlayerManager.Instance.npcInteraction = false;
            SetupDialogueText_toDisplay("");
            EndTutorial();
            return;
        }

        if (currentSegmentShowing + 1 >= tutorialData.tutorialDataSegment.Count)
        {
            Debug.LogError("[HUBTutorial] Reached the last CSV dialogue row without an Ending marker.", this);
            PlayerManager.Instance.npcInteraction = false;
            EndTutorial();
            return;
        }

        currentSegmentShowing++;
        StartCoroutine(SelectSegment());
    }
    IEnumerator SelectSegment()
    {
        selectingSegment = true;
        if (!IsCurrentSegmentValid())
        {
            Debug.LogError("[HUBTutorial] Invalid current dialogue segment index.", this);
            selectingSegment = false;
            yield break;
        }
        HideArrow();
        yield return StartCoroutine(RotateCameraAndWait());

        int languageIndex = (int)DataManager.Instance.settingData_StoreList.currentLanguage;
        var languages = tutorialData.tutorialDataSegment[currentSegmentShowing].languageDialogueList;
        if (languageIndex < 0 || languageIndex >= languages.Count)
        {
            Debug.LogWarning($"[HUBTutorial] Language index {languageIndex} is unavailable, using language 0.", this);
            languageIndex = 0;
        }
        SetupDialogueText_toDisplay(languages[languageIndex]);
        selectingSegment = false;
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
        // TypewriterEffect is shared with other NPCs.
        if (!tutorialSequenceActive || !dialogueIsVisible || DialogueManager.Instance == null) return;
        DialogueManager.Instance.activeNPC = NPCs.Stepellier;
        DialogueManager.Instance.ShowArrow();
    }
    void HideArrow()
    {
        if (DialogueManager.Instance != null)
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
        tutorialDatabaseBuilt = false;
        TextAsset document = null;
        if (ControllerState.Instance == null)
        {
            Debug.LogError("[HUBTutorial] ControllerState is missing, cannot select the dialogue CSV.", this);
            return;
        }

        switch (ControllerState.Instance.activeController)
        {
            case InputType.Keyboard: document = stepellierTutorialDocument_Keyboard; break;
            case InputType.PlayStation: document = stepellierTutorialDocument_PlayStation; break;
            case InputType.Xbox: document = stepellierTutorialDocument_xBox; break;
        }

        if (document == null)
        {
            Debug.LogError($"[HUBTutorial] Dialogue CSV is not assigned for controller {ControllerState.Instance.activeController}.", this);
            return;
        }

        List<List<string>> rows = ParseSemicolonCsv(document.text);
        if (rows == null || rows.Count < 2)
        {
            Debug.LogError($"[HUBTutorial] Dialogue CSV '{document.name}' has no usable data rows.", this);
            return;
        }

        var segments = new List<TutorialDataSegment>();
        // Row 0 contains the column headers. All 13 fields must be present.
        for (int row = 1; row < rows.Count; row++)
        {
            List<string> fields = rows[row];
            if (fields.Count != columns)
            {
                Debug.LogError($"[HUBTutorial] '{document.name}', CSV row {row + 1}: " +
                    $"expected {columns} columns, found {fields.Count}. Row skipped.", this);
                continue;
            }

            string description = fields[0].Trim();
            if (string.IsNullOrEmpty(description)) continue;

            var segment = new TutorialDataSegment
            {
                segmentDescription = description,
                segmentNumber = ParseIntField(fields[1], 0, document.name, row + 1, "Segment"),
                isEnding = !string.IsNullOrWhiteSpace(fields[2]),
                stepellier_spawnPos = new Vector3(
                    ParseFloatField(fields[3], document.name, row + 1, "Pos X"),
                    ParseFloatField(fields[4], document.name, row + 1, "Pos Y"),
                    ParseFloatField(fields[5], document.name, row + 1, "Pos Z")),
                stepellier_spawnRot = SetRotationValue(fields[6].Trim()),
                player_Rot = SetRotationValue(fields[7].Trim()),
                camera_Rotation = SetRotationValue(fields[8].Trim()),
                talkAnimation = ParseIntField(fields[9], 0, document.name, row + 1, "TalkAnimation")
            };
            for (int language = 0; language < currentLanguageAmount; language++)
                segment.languageDialogueList.Add(fields[10 + language].Trim());

            segments.Add(segment);
        }

        if (segments.Count == 0)
        {
            Debug.LogError($"[HUBTutorial] No valid dialogue segments in '{document.name}'.", this);
            return;
        }
        if (!segments[segments.Count - 1].isEnding)
            Debug.LogWarning($"[HUBTutorial] The final dialogue segment in '{document.name}' has no Ending marker.", this);

        tutorialData.tutorialDataSegment = segments;
        tutorialDatabaseBuilt = true;
        Debug.Log($"[HUBTutorial] Loaded {segments.Count} dialogue segments from '{document.name}'.", this);
        TryStartMovementTutorial();
    }

    // CSV reader that handles ; separators, escaped quotes, and line breaks
    // INSIDE quoted dialogue fields. String.Split(';', '\n') cannot do this.
    static List<List<string>> ParseSemicolonCsv(string csv)
    {
        var rows = new List<List<string>>();
        var row = new List<string>();
        var field = new StringBuilder();
        bool insideQuotes = false;

        for (int i = 0; i < csv.Length; i++)
        {
            char c = csv[i];
            if (c == '"')
            {
                if (insideQuotes && i + 1 < csv.Length && csv[i + 1] == '"')
                {
                    field.Append('"');
                    i++;
                }
                else if (insideQuotes)
                {
                    insideQuotes = false;
                }
                else if (field.Length == 0)
                {
                    insideQuotes = true;
                }
                else
                {
                    field.Append(c);
                }
            }
            else if (c == ';' && !insideQuotes)
            {
                row.Add(field.ToString());
                field.Length = 0;
            }
            else if ((c == '\r' || c == '\n') && !insideQuotes)
            {
                row.Add(field.ToString());
                field.Length = 0;
                if (row.Count > 1 || !string.IsNullOrWhiteSpace(row[0]))
                    rows.Add(row);
                row = new List<string>();
                if (c == '\r' && i + 1 < csv.Length && csv[i + 1] == '\n') i++;
            }
            else
            {
                field.Append(c);
            }
        }

        if (insideQuotes)
        {
            Debug.LogError("[HUBTutorial] CSV contains an unclosed quoted field.");
            return null;
        }

        if (field.Length > 0 || row.Count > 0)
        {
            row.Add(field.ToString());
            if (row.Count > 1 || !string.IsNullOrWhiteSpace(row[0])) rows.Add(row);
        }
        return rows;
    }

    static int ParseIntField(string input, int fallback, string file, int row, string column)
    {
        if (string.IsNullOrWhiteSpace(input)) return fallback;
        if (int.TryParse(input.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
            return value;
        Debug.LogWarning($"[HUBTutorial] '{file}', row {row}, {column}: invalid integer '{input}'.");
        return fallback;
    }

    static float ParseFloatField(string input, string file, int row, string column)
    {
        if (string.IsNullOrWhiteSpace(input)) return 0f;
        if (float.TryParse(input.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
            return value;
        Debug.LogWarning($"[HUBTutorial] '{file}', row {row}, {column}: invalid position '{input}'.");
        return 0f;
    }

    MoveDirection SetRotationValue(string rotText)
    {
        switch (rotText)
        {
            case "Forward": return MoveDirection.Forward;
            case "Back": return MoveDirection.Backward;
            case "Backward": return MoveDirection.Backward;
            case "Left": return MoveDirection.Left;
            case "Right": return MoveDirection.Right;
            default: return MoveDirection.None;
        }
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
    public MoveDirection player_Rot;

    public MoveDirection camera_Rotation;

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