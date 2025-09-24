using Unity.Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactable_NPC : MonoBehaviour
{
    [Header("Character")]
    public NPCs characterName;

    [Header("Stats from Excel")]
    [SerializeField] TextAsset dialogueSheet;
    public int levelNumber;

    int startRow = 2;
    int columns = 61; //Size + 1
    int totalLanguageElements = 6; //Count for an extra empty cell between languages (5 + 1)

    [Header("Dialogue Info")]
    public DialogueInfo dialogueInfo = new DialogueInfo();

    int segmentIndex = 0;

    [HideInInspector] public bool canInteract;
    [HideInInspector] public bool isInteracting;

    [Header("To be saved in database")]
    [HideInInspector] public bool hasTalked;
    public int lastSegment;

    [Header("Animations")]
    [SerializeField] Animator anim;
    bool blink;
    int animationCount;
    Quaternion originalRotation;
    private Quaternion npcBodyOriginalRotation;

    [Header("Camera")]
    public CinemachineCamera NPCVirtualCamera;
    public GameObject NPCBody;

    [Header("DialogueSetup")]
    List<DialogueStat> this_TempDataInfo_StartingStat_List = new List<DialogueStat>();
    List<int> tempIndexList = new List<int>();


    //--------------------


    private void Start()
    {
        BuildDialogue();

        dialogueInfo.npcName = characterName;

        originalRotation = transform.rotation;
        npcBodyOriginalRotation = NPCBody.transform.rotation;
    }
    private void Update()
    {
        if (!blink)
        {
            StartCoroutine(RandomBlink());
        }

        TalkAnimation();
    }


    //--------------------


    private void OnEnable()
    {
        Player_KeyInputs.Action_dialogueButton_isPressed += StartNewDialogueSegment;
        Player_KeyInputs.Action_dialogueNextButton_isPressed += StartNewDialogueSegment;
        Player_KeyInputs.Action_InteractButton_isPressed += CanInteract;

        OptionButton.Action_OptionButtonIsPressed += StartNewDialogueSegment_OptionButton;

        DataManager.Action_dataHasLoaded += SetupNPC;
    }
    private void OnDisable()
    {
        Player_KeyInputs.Action_dialogueButton_isPressed -= StartNewDialogueSegment;
        Player_KeyInputs.Action_dialogueNextButton_isPressed -= StartNewDialogueSegment;
        Player_KeyInputs.Action_InteractButton_isPressed -= CanInteract;

        OptionButton.Action_OptionButtonIsPressed -= StartNewDialogueSegment_OptionButton;

        DataManager.Action_dataHasLoaded -= SetupNPC;
    }


    //--------------------


    void CanInteract()
    {
        if (canInteract && !isInteracting)
        {
            canInteract = false;
            StartNPCDialogue();
        }
    }
    void StartNPCDialogue()
    {
        StartCoroutine(StartNPCDialogueCoroutine());
    }
    IEnumerator StartNPCDialogueCoroutine()
    {
        PlayerManager.Instance.npcInteraction = true;
        ButtonMessages.Instance.HideButtonMessage();

        yield return StartCoroutine(TurnNPCTowardsPlayer());

        yield return new WaitForSeconds(0.05f);

        yield return StartCoroutine(CameraController.Instance.StartVirtualCameraBlend_In(CameraController.Instance.CM_Other));

        DialogueManager.Instance.npcObject = this;
        DialogueManager.Instance.activeNPC = characterName;
        DialogueManager.Instance.segmentTotal = dialogueInfo.dialogueSegments.Count - 1;

        if (hasTalked)
        {
            DialogueManager.Instance.currentSegement = lastSegment;
            segmentIndex = lastSegment;
        }
        else
        {
            int tempIndex = SetCorrectSegmentFromTheStart(characterName);

            DialogueManager.Instance.currentSegement = tempIndex;
            segmentIndex = tempIndex;
        }
        
        SetupDialogueDisplay(segmentIndex, dialogueInfo.npcName);
    }

    int SetCorrectSegmentFromTheStart(NPCs npc)
    {
        if (dialogueInfo == null || dialogueInfo.dialogueSegments == null || dialogueInfo.dialogueSegments.Count == 0)
            return 0;

        // Player stats (null-safe, only keep valid entries)
        var playerStatsRaw = GetPlayerStatsForNPC(npc) ?? new List<DialogueStat>();
        var playerSet = ToSet(playerStatsRaw); // unique (character,value) pairs with value > 0

        // All first-segments (index + segment), null-safe
        var firstSegments = dialogueInfo.dialogueSegments
            .Select((seg, idx) => new { seg, idx })
            .Where(x => x.seg != null && x.seg.firstSegment)
            .ToList();

        if (firstSegments.Count == 0)
            return 0;

        // -------- Bucket 1: Exact set match (requirements == playerSet) --------
        var exactMatches = firstSegments
            .Where(x =>
            {
                var reqSet = ToSet(x.seg.statRequired);
                return reqSet.Count > 0 && SetEquals(reqSet, playerSet);
            })
            .ToList();

        if (exactMatches.Count > 0)
            return exactMatches[UnityEngine.Random.Range(0, exactMatches.Count)].idx;

        // -------- Buckets 2..N: Descend by requirement count; require full subset match --------
        int maxReqCount = firstSegments
            .Select(x => ToSet(x.seg.statRequired).Count) // safe: ToSet handles null
            .DefaultIfEmpty(0)
            .Max();

        for (int k = maxReqCount; k > 0; k--)
        {
            var bucket = firstSegments
                .Where(x =>
                {
                    var reqSet = ToSet(x.seg.statRequired);
                    return reqSet.Count == k && IsSubset(reqSet, playerSet);
                })
                .ToList();

            if (bucket.Count > 0)
                return bucket[UnityEngine.Random.Range(0, bucket.Count)].idx;
        }

        // -------- Final bucket: No-requirement firstSegments --------
        var noReq = firstSegments
            .Where(x => ToSet(x.seg.statRequired).Count == 0)
            .ToList();

        if (noReq.Count > 0)
            return noReq[UnityEngine.Random.Range(0, noReq.Count)].idx;

        // Fallback: first firstSegment
        return firstSegments[0].idx;

        // ----- local helpers -----
        // Build a set of unique keys "character:value" for valid stats (value > 0).
        HashSet<string> ToSet(List<DialogueStat> list)
        {
            if (list == null) return new HashSet<string>();
            return new HashSet<string>(
                list.Where(s => s != null && s.value > 0)
                    .Select(s => $"{(int)s.character}:{s.value}")
            );
        }

        bool IsSubset(HashSet<string> a, HashSet<string> b) => a.All(b.Contains);
        bool SetEquals(HashSet<string> a, HashSet<string> b) => a.Count == b.Count && a.All(b.Contains);
    }

    // Helper method to get correct player's stats list
    List<DialogueStat> GetPlayerStatsForNPC(NPCs npc)
    {
        var store = DataManager.Instance.charatersData_Store;
        return npc switch
        {
            NPCs.Floriel => store?.floriel_Data?.dialogueStatList,
            NPCs.Granith => store?.granith_Data?.dialogueStatList,
            NPCs.Archie => store?.archie_Data?.dialogueStatList,
            NPCs.Aisa => store?.aisa_Data?.dialogueStatList,
            NPCs.Mossy => store?.mossy_Data?.dialogueStatList,
            NPCs.Larry => store?.larry_Data?.dialogueStatList,
            NPCs.Stepellier => store?.stepellier_Data?.dialogueStatList,
            _ => null
        };
    }



    //--------------------


    void SetupNPC()
    {
        switch (characterName)
        {
            case NPCs.None:
                break;

            case NPCs.Floriel:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.floriel_Data != null && DataManager.Instance.charatersData_Store.floriel_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.floriel_Data != null && DataManager.Instance.charatersData_Store.floriel_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.floriel_Data != null && DataManager.Instance.charatersData_Store.floriel_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.floriel_Data != null && DataManager.Instance.charatersData_Store.floriel_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.floriel_Data != null && DataManager.Instance.charatersData_Store.floriel_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.floriel_Data != null && DataManager.Instance.charatersData_Store.floriel_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Granith:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.granith_Data != null && DataManager.Instance.charatersData_Store.granith_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.granith_Data != null && DataManager.Instance.charatersData_Store.granith_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.granith_Data != null && DataManager.Instance.charatersData_Store.granith_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.granith_Data != null && DataManager.Instance.charatersData_Store.granith_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.granith_Data != null && DataManager.Instance.charatersData_Store.granith_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.granith_Data != null && DataManager.Instance.charatersData_Store.granith_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Archie:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.archie_Data != null && DataManager.Instance.charatersData_Store.archie_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.archie_Data != null && DataManager.Instance.charatersData_Store.archie_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.archie_Data != null && DataManager.Instance.charatersData_Store.archie_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.archie_Data != null && DataManager.Instance.charatersData_Store.archie_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.archie_Data != null && DataManager.Instance.charatersData_Store.archie_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.archie_Data != null && DataManager.Instance.charatersData_Store.archie_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Aisa:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.aisa_Data != null && DataManager.Instance.charatersData_Store.aisa_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.aisa_Data != null && DataManager.Instance.charatersData_Store.aisa_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.aisa_Data != null && DataManager.Instance.charatersData_Store.aisa_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.aisa_Data != null && DataManager.Instance.charatersData_Store.aisa_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.aisa_Data != null && DataManager.Instance.charatersData_Store.aisa_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.aisa_Data != null && DataManager.Instance.charatersData_Store.aisa_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Mossy:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.mossy_Data != null && DataManager.Instance.charatersData_Store.mossy_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.mossy_Data != null && DataManager.Instance.charatersData_Store.mossy_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.mossy_Data != null && DataManager.Instance.charatersData_Store.mossy_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.mossy_Data != null && DataManager.Instance.charatersData_Store.mossy_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.mossy_Data != null && DataManager.Instance.charatersData_Store.mossy_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.mossy_Data != null && DataManager.Instance.charatersData_Store.mossy_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Larry:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.larry_Data != null && DataManager.Instance.charatersData_Store.larry_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.larry_Data != null && DataManager.Instance.charatersData_Store.larry_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.larry_Data != null && DataManager.Instance.charatersData_Store.larry_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.larry_Data != null && DataManager.Instance.charatersData_Store.larry_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.larry_Data != null && DataManager.Instance.charatersData_Store.larry_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.larry_Data != null && DataManager.Instance.charatersData_Store.larry_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Stepellier:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.stepellier_Data != null && DataManager.Instance.charatersData_Store.stepellier_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.stepellier_Data != null && DataManager.Instance.charatersData_Store.stepellier_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.stepellier_Data != null && DataManager.Instance.charatersData_Store.stepellier_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.stepellier_Data != null && DataManager.Instance.charatersData_Store.stepellier_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.stepellier_Data != null && DataManager.Instance.charatersData_Store.stepellier_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store != null && DataManager.Instance.charatersData_Store.stepellier_Data != null && DataManager.Instance.charatersData_Store.stepellier_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;

            default:
                break;
        }
    }
    void ShowNPC()
    {
        gameObject.SetActive(true);
    }
    void HideNPC()
    {
        gameObject.SetActive(false);
    }

    IEnumerator TurnNPCTowardsPlayer()
    {
        Transform target = PlayerManager.Instance.playerBody.transform;

        float rotationSpeed = 8f;
        float angleThreshold = 1f;

        transform.rotation = originalRotation;
        NPCBody.transform.rotation = npcBodyOriginalRotation;

        while (true)
        {
            // Get direction to player
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0f; // Ignore vertical difference if needed

            if (direction == Vector3.zero)
                break;

            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Measure angle difference
            float angle = Quaternion.Angle(transform.rotation, targetRotation);

            if (angle <= angleThreshold)
            {
                // Snap to final rotation and exit
                transform.rotation = targetRotation;
                break;
            }

            // Smoothly rotate toward player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator TurnNPCAwayFromPlayer()
    {
        yield return new WaitForSeconds(0.1f);

        float rotationSpeed = 8f;
        float angleThreshold = 1f;

        while (true)
        {
            float angle = Quaternion.Angle(NPCBody.transform.rotation, npcBodyOriginalRotation);

            if (angle <= angleThreshold)
            {
                NPCBody.transform.rotation = npcBodyOriginalRotation;
                break;
            }

            NPCBody.transform.rotation = Quaternion.Slerp(NPCBody.transform.rotation, npcBodyOriginalRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }


    //--------------------


    void BuildDialogue()
    {
        ReadExcelSheet();
    }
    public void ReadExcelSheet()
    {
        //Separate Excel Sheet into a string[] by its ";"
        string[] excelData = dialogueSheet.text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);

        // Calculate the size of the Excel table
        int excelTableSize = (excelData.Length / columns - 1) - 0;

        // Initialize the list
        dialogueInfo.dialogueSegments = new List<DialogueSegment>();

        // Populate the list with default DataObject instances
        for (int i = 0; i < excelTableSize; i++)
        {
            DialogueSegment dialogueSegment = new DialogueSegment();

            for (int j = 0; j < DialogueManager.Instance.languageAmount; j++)
            {
                dialogueSegment.languageDialogueList.Add(null);
                dialogueSegment.languageOptionList.Add(new LanguageOptions());
            }

            dialogueInfo.dialogueSegments.Add(dialogueSegment);
        }

        //Fill the new element with data
        for (int i = 0; i < excelTableSize; i++)
        {
            #region Description

            //Segment Description
            if (excelData[columns * (i + startRow - 1) + 2] != "")
                dialogueInfo.dialogueSegments[i].segmentDescription = excelData[columns * (i + startRow - 1) + 2].Trim();
            else
                dialogueInfo.dialogueSegments[i].segmentDescription = "";

            #endregion

            #region Is first and last segment

            //First Segment
            if (excelData[columns * (i + startRow - 1) + 4] != "")
                dialogueInfo.dialogueSegments[i].firstSegment = true;
            else
                dialogueInfo.dialogueSegments[i].firstSegment = false;

            //Last Segment
            if (excelData[columns * (i + startRow - 1) + 5] != "")
                dialogueInfo.dialogueSegments[i].lastSegment = true;
            else
                dialogueInfo.dialogueSegments[i].lastSegment = false;

            #endregion

            #region Animations

            //Player Animation number
            if (excelData[columns * (i + startRow - 1) + 7] != "")
                dialogueInfo.dialogueSegments[i].animation_Player = AnimationDataSplicer(excelData[columns * (i + startRow - 1) + 7].Trim());
            else
                dialogueInfo.dialogueSegments[i].animation_Player = null;

            //NPC Animation number
            if (excelData[columns * (i + startRow - 1) + 8] != "")
                dialogueInfo.dialogueSegments[i].animation_NPC = AnimationDataSplicer(excelData[columns * (i + startRow - 1) + 8].Trim());
            else
                dialogueInfo.dialogueSegments[i].animation_NPC = null;

            //Cutscene
            if (excelData[columns * (i + startRow - 1) + 9] != "")
                dialogueInfo.dialogueSegments[i].cutscene = ParseIntSafe(excelData, columns * (i + startRow - 1) + 9);
            else
                dialogueInfo.dialogueSegments[i].cutscene = -1;

            #endregion

            #region Stats

            //Stats
            if (excelData[columns * (i + startRow - 1) + 11] != "")
                dialogueInfo.dialogueSegments[i].statRequired = StatsDataSplicer(excelData[columns * (i + startRow - 1) + 11].Trim());
            else
                dialogueInfo.dialogueSegments[i].statRequired = null;

            if (excelData[columns * (i + startRow - 1) + 12] != "")
                dialogueInfo.dialogueSegments[i].statToGet = StatsDataSplicer(excelData[columns * (i + startRow - 1) + 12].Trim());
            else
                dialogueInfo.dialogueSegments[i].statToGet = null;

            #endregion

            #region Option Parameters

            #region Setup the first Option language

            #region Option 1

            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 16] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 16);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_Linked = -1;

            //Option 1 - AlternativeLink
            if (excelData[columns * (i + startRow - 1) + 17] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_AlternativeLinked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 17);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_AlternativeLinked = -1;

            //Option 1 - EndingValue
            if (excelData[columns * (i + startRow - 1) + 18] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_StoryValue = ParseIntSafe(excelData, columns * (i + startRow - 1) + 18);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_StoryValue = 0;

            #endregion

            #region Option 2

            //Option 2 - Link
            if (excelData[columns * (i + startRow - 1) + 20] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 20);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_Linked = -1;

            //Option 2 - AlternativeLink
            if (excelData[columns * (i + startRow - 1) + 21] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_AlternativeLinked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 21);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_AlternativeLinked = -1;

            //Option 2 - EndingValue
            if (excelData[columns * (i + startRow - 1) + 22] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_StoryValue = ParseIntSafe(excelData, columns * (i + startRow - 1) + 22);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_StoryValue = 0;

            #endregion

            #region Option 3

            //Option 3 - Link
            if (excelData[columns * (i + startRow - 1) + 24] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 24);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_Linked = -1;

            //Option 3 - AlternativeLink
            if (excelData[columns * (i + startRow - 1) + 25] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_AlternativeLinked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 25);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_AlternativeLinked = -1;

            //Option 3 - EndingValue
            if (excelData[columns * (i + startRow - 1) + 26] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_StoryValue = ParseIntSafe(excelData, columns * (i + startRow - 1) + 26);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_StoryValue = 0;

            #endregion

            #region Option 4

            //Option 4 - Link
            if (excelData[columns * (i + startRow - 1) + 28] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 28);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_Linked = -1;

            //Option 4 - AlternativeLink
            if (excelData[columns * (i + startRow - 1) + 29] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_AlternativeLinked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 29);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_AlternativeLinked = -1;

            //Option 4 - EndingValue
            if (excelData[columns * (i + startRow - 1) + 30] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_StoryValue = ParseIntSafe(excelData, columns * (i + startRow - 1) + 30);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_StoryValue = 0;

            #endregion

            #endregion

            #region Insert Option Parameters to the rest of the languages

            for (int j = 1; j < DialogueManager.Instance.languageAmount; j++)
            {
                dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Linked = dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_Linked;
                dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_AlternativeLinked = dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_AlternativeLinked;
                dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_StoryValue = dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_StoryValue;

                dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Linked = dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_Linked;
                dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_AlternativeLinked = dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_AlternativeLinked;
                dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_StoryValue = dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_StoryValue;

                dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Linked = dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_Linked;
                dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_AlternativeLinked = dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_AlternativeLinked;
                dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_StoryValue = dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_StoryValue;

                dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Linked = dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_Linked;
                dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_AlternativeLinked = dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_AlternativeLinked;
                dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_StoryValue = dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_StoryValue;
            }

            #endregion

            #endregion

            #region Languages

            #region Norwegian

            //Setup the Norwegian language

            //Message
            if (excelData[columns * (i + startRow - 1) + 14] != "")
                dialogueInfo.dialogueSegments[i].languageDialogueList[0] = excelData[columns * (i + startRow - 1) + 14].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageDialogueList[0] = "";

            //Option 1
            if (excelData[columns * (i + startRow - 1) + 15] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_Text = excelData[columns * (i + startRow - 1) + 15].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_Text = "";

            //Option 2
            if (excelData[columns * (i + startRow - 1) + 19] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_Text = excelData[columns * (i + startRow - 1) + 19].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_Text = "";

            //Option 3
            if (excelData[columns * (i + startRow - 1) + 23] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_Text = excelData[columns * (i + startRow - 1) + 23].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_Text = "";

            //Option 4
            if (excelData[columns * (i + startRow - 1) + 27] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_Text = excelData[columns * (i + startRow - 1) + 27].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_Text = "";

            #endregion

            #region Other

            //Setup all languages at once (exept Norwegian)
            for (int j = 0; j < DialogueManager.Instance.languageAmount - 1; j++)
            {
                //Message
                if (excelData[columns * (i + startRow - 1) + 32 + (totalLanguageElements * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageDialogueList[j + 1] = excelData[columns * (i + startRow - 1) + 32 + (totalLanguageElements * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageDialogueList[j + 1] = "";

                //Option 1
                if (excelData[columns * (i + startRow - 1) + 33 + (totalLanguageElements * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j + 1].option1_Text = excelData[columns * (i + startRow - 1) + 33 + (totalLanguageElements * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j + 1].option1_Text = "";
                
                //Option 2
                if (excelData[columns * (i + startRow - 1) + 34 + (totalLanguageElements * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j + 1].option2_Text = excelData[columns * (i + startRow - 1) + 34 + (totalLanguageElements * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j + 1].option2_Text = "";

                //Option 3
                if (excelData[columns * (i + startRow - 1) + 35 + (totalLanguageElements * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j + 1].option3_Text = excelData[columns * (i + startRow - 1) + 35 + (totalLanguageElements * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j + 1].option3_Text = "";

                //Option 4
                if (excelData[columns * (i + startRow - 1) + 36 + (totalLanguageElements * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j + 1].option4_Text = excelData[columns * (i + startRow - 1) + 36 + (totalLanguageElements * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j + 1].option4_Text = "";
            }

            #endregion

            #endregion

            CleanTheTextDialogue(i);
        }

        //Remove elements that doesn't have a name
        dialogueInfo.dialogueSegments = dialogueInfo.dialogueSegments.Where(obj => obj != null && !string.IsNullOrEmpty(obj.segmentDescription)).ToList();
    }
    List<int> AnimationDataSplicer(string text)
    {
        List<int> animationSplizer = new List<int>();

        if (string.IsNullOrWhiteSpace(text))
            return animationSplizer;

        string[] parts = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in parts)
        {
            if (int.TryParse(part, out int number))
            {
                animationSplizer.Add(number);
            }
            else
            {
                Debug.LogWarning($"Invalid number in animation data: '{part}'");
            }
        }

        return animationSplizer;
    }
    List<DialogueStat> StatsDataSplicer(string text)
    {
        List<DialogueStat> statsSplizerList = new List<DialogueStat>();

        if (string.IsNullOrWhiteSpace(text))
            return statsSplizerList;

        // Step 1: Split by commas to get each "NPCName number" pair
        string[] entries = text.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (string entry in entries)
        {
            // Step 2: Split by space
            string[] parts = entry.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                Debug.LogWarning($"Invalid entry format: '{entry}'");
                continue;
            }

            // Step 3: Try parse the enum (NPCName)
            if (!Enum.TryParse(parts[0], out NPCs npcName))
            {
                Debug.LogWarning($"'{parts[0]}' is not a valid NPC enum value.");
                continue;
            }

            // Step 4: Try parse the int
            if (!int.TryParse(parts[1], out int number))
            {
                Debug.LogWarning($"'{parts[1]}' is not a valid number.");
                continue;
            }

            // Step 5: Add to list
            statsSplizerList.Add(new DialogueStat
            {
                character = npcName,
                value = number
            });
        }

        return statsSplizerList;
    }

    int ParseIntSafe(string[] data, int index)
    {
        //if (index < 0 || index >= data.Length) return -1;
        //string cleaned = new string(data[index].Where(char.IsDigit).ToArray());

        //if (int.TryParse(cleaned, out int result)) return result;

        //return -1;

        if (index >= 0 && index < data.Length && int.TryParse(data[index], out int result))
            return result;
        return -1;
    }
    int ParseIntSafe(string value)
    {
        if (int.TryParse(value, out int result))
            return result;
        return -1; // or any default
    }
    int ParseEndValue(string[] data, int index)
    {
        int tempValue = ParseIntSafe(data, index);

        if (tempValue == 1)
            return -1;
        else if (tempValue == 2)
            return 1;

        return 0;
    }

    void CleanTheTextDialogue(int i)
    {
        dialogueInfo.dialogueSegments[i].segmentDescription = CleanQuotes(dialogueInfo.dialogueSegments[i].segmentDescription);

        for (int j = 0; j < DialogueManager.Instance.languageAmount; j++)
        {
            dialogueInfo.dialogueSegments[i].languageDialogueList[j] = CleanQuotes(dialogueInfo.dialogueSegments[i].languageDialogueList[j]);
            dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Text = CleanQuotes(dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Text);
            dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Text = CleanQuotes(dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Text);
            dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Text = CleanQuotes(dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Text);
            dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Text = CleanQuotes(dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Text);
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


    //--------------------


    void StartNewDialogueSegment()
    {
        if (!isInteracting) return;
        if (TypewriterEffect.Instance.isTyping) return;

        //If current segment is a "lastSegment", end the dialogue
        if (dialogueInfo.dialogueSegments[segmentIndex].lastSegment)
        {
            lastSegment = segmentIndex;
            StartCoroutine(TurnNPCAwayFromPlayer());
            StartCoroutine(DialogueManager.Instance.EndDialogue());
        }

        //If the next segment doesn't have any options, run the next segment in the list
        else if (dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option1_Text == "")
        {
            if (!TypewriterEffect.Instance.isTyping)
            {
                segmentIndex++;

                SetupDialogueDisplay(segmentIndex, dialogueInfo.npcName);
            }
        }
    }
    public void StartNewDialogueSegment_OptionButton()
    {
        if (!isInteracting || TypewriterEffect.Instance.isTyping) return;


        //-----


        LanguageOptions languageOptions = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0];
        int segment = -1;

        if (DialogueManager.Instance.selectedButton == 1)
        {
            //segment = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option1_Linked - 1;
            segment = ChooseSegment(languageOptions.option1_Linked - 1, languageOptions.option1_AlternativeLinked - 1);
            UpdateEndingValue(characterName, dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option1_StoryValue);
        }
        else if (DialogueManager.Instance.selectedButton == 2)
        {
            //segment = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option2_Linked - 1;
            segment = ChooseSegment(languageOptions.option2_Linked - 1, languageOptions.option2_AlternativeLinked - 1);
            UpdateEndingValue(characterName, dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option2_StoryValue);
        }
        else if (DialogueManager.Instance.selectedButton == 3)
        {
            //segment = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option3_Linked - 1;
            segment = ChooseSegment(languageOptions.option3_Linked - 1, languageOptions.option3_AlternativeLinked - 1);
            UpdateEndingValue(characterName, dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option3_StoryValue);
        }
        else if (DialogueManager.Instance.selectedButton == 4)
        {
            //segment = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option4_Linked - 1;
            segment = ChooseSegment(languageOptions.option4_Linked - 1, languageOptions.option4_AlternativeLinked - 1);
            UpdateEndingValue(characterName, dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option4_StoryValue);
        }

        segmentIndex = segment;
        DialogueManager.Instance.currentSegement = segmentIndex;

        //print("DialogueSegment: Button: " + (DialogueManager.Instance.selectedButton - 1) + " | Index: " + segmentIndex + " | Segment: " + segment);

        SetupDialogueDisplay(segment, dialogueInfo.npcName);
    }
    int ChooseSegment(int linked, int alternativeLinked)
    {
        switch (characterName)
        {
            case NPCs.None:
                return -1;

            case NPCs.Floriel:
                return CharacterStats(DataManager.Instance.charatersData_Store.floriel_Data, linked, alternativeLinked);
            case NPCs.Granith:
                return CharacterStats(DataManager.Instance.charatersData_Store.granith_Data, linked, alternativeLinked);
            case NPCs.Archie:
                return CharacterStats(DataManager.Instance.charatersData_Store.archie_Data, linked, alternativeLinked);
            case NPCs.Aisa:
                return CharacterStats(DataManager.Instance.charatersData_Store.aisa_Data, linked, alternativeLinked);
            case NPCs.Mossy:
                return CharacterStats(DataManager.Instance.charatersData_Store.mossy_Data, linked, alternativeLinked);
            case NPCs.Larry:
                return CharacterStats(DataManager.Instance.charatersData_Store.larry_Data, linked, alternativeLinked);
            case NPCs.Stepellier:
                return CharacterStats(DataManager.Instance.charatersData_Store.stepellier_Data, linked, alternativeLinked);

            default:
                return -1;
        }
    }

    int CharacterStats(NPCData npcData, int linked, int alternativeLinked)
    {
        bool linkedValid = linked >= 0 && linked < dialogueInfo.dialogueSegments.Count;
        bool altValid = alternativeLinked >= 0 && alternativeLinked < dialogueInfo.dialogueSegments.Count;

        // --- CASE: Both are empty/invalid ---
        if (!linkedValid && !altValid)
        {
            segmentIndex += 1;
            return segmentIndex; // Move to next segment
        }

        // --- CASE: Linked has no requirements ---
        if (linkedValid)
        {
            var requiredStats = dialogueInfo.dialogueSegments[linked].statRequired;

            if (requiredStats == null || requiredStats.Count == 0)
                return linked;

            if (npcData.dialogueStatList != null && npcData.dialogueStatList.Count > 0)
            {
                bool allRequirementsMet = true;

                for (int i = 0; i < requiredStats.Count; i++)
                {
                    bool foundMatch = false;
                    for (int j = 0; j < npcData.dialogueStatList.Count; j++)
                    {
                        if (npcData.dialogueStatList[j].character == requiredStats[i].character &&
                            npcData.dialogueStatList[j].value == requiredStats[i].value)
                        {
                            foundMatch = true;
                            break;
                        }
                    }
                    if (!foundMatch)
                    {
                        allRequirementsMet = false;
                        break;
                    }
                }

                if (allRequirementsMet)
                    return linked;
            }
        }

        // --- CASE: Linked failed ---
        if (!altValid)
            return linkedValid ? linked : segmentIndex + 1; // Fall back to linked even if requirements not met, or skip if linked invalid

        return alternativeLinked; // Use alternative if valid
    }

    void UpdateEndingValue(NPCs npc, int tempEndingValue)
    {
        switch (npc)
        {
            case NPCs.None:
                break;

            case NPCs.Floriel:
                NPCManager.Instance.charatersData.floriel_Data.endingValue += tempEndingValue;
                break;
            case NPCs.Granith:
                NPCManager.Instance.charatersData.granith_Data.endingValue += tempEndingValue;
                break;
            case NPCs.Archie:
                NPCManager.Instance.charatersData.archie_Data.endingValue += tempEndingValue;
                break;
            case NPCs.Aisa:
                NPCManager.Instance.charatersData.aisa_Data.endingValue += tempEndingValue;
                break;
            case NPCs.Mossy:
                NPCManager.Instance.charatersData.mossy_Data.endingValue += tempEndingValue;
                break;
            case NPCs.Larry:
                NPCManager.Instance.charatersData.larry_Data.endingValue += tempEndingValue;
                break;

            case NPCs.Stepellier:
                NPCManager.Instance.charatersData.stepellier_Data.endingValue += tempEndingValue;
                break;

            default:
                break;
        }

        NPCManager.Instance.SaveData();
    }


    //--------------------


    void SetupDialogueDisplay(int index, NPCs npc)
    {
        NPCManager.Instance.UpdateStatsGathered(index, dialogueInfo);

        //Text
        if (dialogueInfo.dialogueSegments.Count > index)
        {
            switch (SettingsManager.Instance.settingsData.currentLanguage)
            {
                case Languages.Norwegian:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(npc, dialogueInfo.dialogueSegments[index].languageDialogueList[0], dialogueInfo.dialogueSegments[index].languageOptionList[0]);
                    break;
                case Languages.English:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(npc, dialogueInfo.dialogueSegments[index].languageDialogueList[1], dialogueInfo.dialogueSegments[index].languageOptionList[1]);
                    break;
                case Languages.German:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(npc, dialogueInfo.dialogueSegments[index].languageDialogueList[2], dialogueInfo.dialogueSegments[index].languageOptionList[2]);
                    break;
                case Languages.Japanese:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(npc, dialogueInfo.dialogueSegments[index].languageDialogueList[3], dialogueInfo.dialogueSegments[index].languageOptionList[3]);
                    break;
                case Languages.Chinese:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(npc, dialogueInfo.dialogueSegments[index].languageDialogueList[4], dialogueInfo.dialogueSegments[index].languageOptionList[4]);
                    break;

                default:
                    break;
            }
        }
        else
        {
            StartCoroutine(TurnNPCAwayFromPlayer());
            StartCoroutine(DialogueManager.Instance.EndDialogue());
            return;
        }

        //Animation
        if (dialogueInfo.dialogueSegments[index] != null && dialogueInfo.dialogueSegments[index].animation_NPC != null && dialogueInfo.dialogueSegments[index].animation_NPC.Count > 0)
        {
            animationCount = 0;
            StartCoroutine(RunAnimations(index));
        } 
    }
    void SetupDialogue(DialogueSegment dialogueSegment)
    {
        dialogueInfo.dialogueSegments.Add(dialogueSegment);
    }
    DialogueSegment SetupDialogueSegment(List<string> languageDialogueList, List<LanguageOptions> languageOptionList)
    {
        DialogueSegment segment = new DialogueSegment();

        segment.languageDialogueList = languageDialogueList;
        segment.languageOptionList = languageOptionList;

        return segment;
    }

    List<string> SetupLanguageTextList(string language_1, string language_2, string language_3, string language_4, string language_5)
    {
        List<string> languageList = new List<string>();

        languageList.Add(language_1);
        languageList.Add(language_2);
        languageList.Add(language_3);
        languageList.Add(language_4);
        languageList.Add(language_5);

        return languageList;
    }
    List<LanguageOptions> SetupOptionLanguages(LanguageOptions language1, LanguageOptions language2, LanguageOptions language3, LanguageOptions language4, LanguageOptions language5)
    {
        List<LanguageOptions> languageOptions = new List<LanguageOptions>();

        languageOptions.Add(language1);
        languageOptions.Add(language2);
        languageOptions.Add(language3);
        languageOptions.Add(language4);
        languageOptions.Add(language5);

        return languageOptions;
    }
    LanguageOptions SetupOption(string _option1_Text, int _segment1_Linked, string _option2_Text, int _segment2_Linked, string _option3_Text, int _segment3_Linked, string _option4_Text, int _segment4_Linked)
    {
        LanguageOptions option = new LanguageOptions();
        option.option1_Text = _option1_Text;
        option.option1_Linked = _segment1_Linked;
        option.option2_Text = _option2_Text;
        option.option2_Linked = _segment2_Linked;
        option.option3_Text = _option3_Text;
        option.option3_Linked = _segment3_Linked;
        option.option4_Text = _option4_Text;
        option.option4_Linked = _segment4_Linked;

        return option;
    }


    //--------------------


    IEnumerator RunAnimations(int index)
    {
        int animationNumber = dialogueInfo.dialogueSegments[index].animation_NPC[animationCount];

        PerformAnimation(animationNumber);

        // Wait until the animator enters the state
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(AnimationManager.Instance.animationList[animationNumber]))
        {
            yield return null;
        }

        // Wait until the animation finishes
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        // Start the next coroutine
        if (animationCount < dialogueInfo.dialogueSegments[index].animation_NPC.Count - 1)
        {
            animationCount++;
            StartCoroutine(RunAnimations(index));
        }
    }
    void PerformAnimation(int animNumber)
    {
        anim.SetTrigger(AnimationManager.Instance.animationList[animNumber]);
        anim.SetTrigger(AnimationManager.Instance.blink);
    }
    IEnumerator RandomBlink()
    {
        blink = true;

        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 10f));

        anim.SetTrigger(AnimationManager.Instance.blink);

        blink = false;
    }
    void TalkAnimation()
    {
        if (TypewriterEffect.Instance.isTyping)
            anim.SetBool("Talking", true);
        else
            anim.SetBool("Talking", false);
    }
}