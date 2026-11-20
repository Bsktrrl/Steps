using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Player_Statistics_Manager : MonoBehaviour
{
    private const int GameProgressTotal = 31 * 15 + 32 + 30; //31 levels with 15 progress tings in it, 32 skins to buy, 30 NPC interractions

    public static Player_Statistics_Manager Active { get; private set; }
    public PlayerStatistics Statistics => data ? data.playerStatistics_Store : null;

    private static bool sessionCounted;
    private static double sessionStartedAt;
    private static double lastPlayTimeUpdate;

    private DataManager data;
    private MapManager map;
    private PlayerManager player;
    private Movement movement;
    private FreeCam freeCam;
    private PauseMenuManager pauseMenu;
    private readonly int[] collectiblePointsByLevel = new int[(int)LevelNames.HUB + 1];
    private readonly Dictionary<string, LevelNames> levelNamesByMapName = new Dictionary<string, LevelNames>();

    private int pendingSteps;
    private int pendingStepsReplenished;
    private int pendingRespawns;
    private float pendingStandingTime;

    // A save drains the pending counters. These totals belong to the whole attempt.
    private int attemptSteps;
    private float attemptTime;
    private float attemptFreeCamTime;
    private bool attemptCompleted;
    private bool attemptClosed;

    private bool IsTrackingLevel => data && map && !attemptClosed;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetSessionTracking()
    {
        Active = null;
        sessionCounted = false;
        sessionStartedAt = Time.realtimeSinceStartupAsDouble;
        lastPlayTimeUpdate = sessionStartedAt;
    }

    private void OnEnable()
    {
        Active = this;
        Movement.Action_StepTaken += RecordStep;
        Movement.Action_isJumping += RecordJump;
        Movement.Action_isDashing += RecordHandDrill;
        Movement.Action_isAscending += RecordDrillHelmet;
        Movement.Action_isDescending += RecordDrillBoots;
        Movement.Action_isSwiftSwim += RecordAbility;
        Player_CeilingGrab.Action_isCeilingGrabbing += RecordAbility;
        Interactable_Pickup.Action_EssencePickupGot += RecordEssence;
        SkinWardrobeButton.Action_BuySkin += RecordSkinBought;
    }

    private void OnDisable()
    {
        if (Active == this)
            Active = null;

        Movement.Action_StepTaken -= RecordStep;
        Movement.Action_isJumping -= RecordJump;
        Movement.Action_isDashing -= RecordHandDrill;
        Movement.Action_isAscending -= RecordDrillHelmet;
        Movement.Action_isDescending -= RecordDrillBoots;
        Movement.Action_isSwiftSwim -= RecordAbility;
        Player_CeilingGrab.Action_isCeilingGrabbing -= RecordAbility;
        Interactable_Pickup.Action_EssencePickupGot -= RecordEssence;
        SkinWardrobeButton.Action_BuySkin -= RecordSkinBought;
    }

    internal void BeginTracking(DataManager loadedData)
    {
        data = loadedData;
        ImportExistingProgress();

        if (!sessionCounted)
        {
            AddSessions();
            sessionCounted = true;
        }

        // Reloading save data in the same scene does not start a second attempt.
        if (map)
            return;

        map = FindAnyObjectByType<MapManager>();
        if (!map || map.gameObject.scene != gameObject.scene)
        {
            map = null;
            return;
        }

        GameObject spawnedPlayer = MapManager.SpawnedPlayer;
        if (spawnedPlayer)
        {
            player = spawnedPlayer.GetComponent<PlayerManager>();
            movement = spawnedPlayer.GetComponentInChildren<Movement>();
            freeCam = spawnedPlayer.GetComponentInChildren<FreeCam>();
        }

        pauseMenu = FindAnyObjectByType<PauseMenuManager>();

        string levelKey = map.levelName == LevelNames.None ? map.gameObject.scene.name : map.levelName.ToString();
        if (!AddUnique(Statistics.visitedLevels, levelKey))
            AddLevelsReplayed(1);
    }

    internal void ResetForNewGame()
    {
        if (data)
            Statistics.initialized = true;

        sessionStartedAt = Time.realtimeSinceStartupAsDouble;
        lastPlayTimeUpdate = sessionStartedAt;
        pendingSteps = 0;
        pendingStepsReplenished = 0;
        pendingRespawns = 0;
        pendingStandingTime = 0f;
        attemptSteps = 0;
        attemptTime = 0f;
        attemptFreeCamTime = 0f;
        attemptCompleted = false;
        attemptClosed = false;
    }

    private void LateUpdate()
    {
        if (!IsTrackingLevel || attemptCompleted || !player || !movement ||
            !data.dataHasLoaded_Check || map.IntroInputLocked || !movement.isRespawningFirstTime ||
            (pauseMenu && pauseMenu.isActive))
            return;

        float elapsed = Time.deltaTime;
        attemptTime += elapsed;

        if (freeCam && freeCam._isActive)
            attemptFreeCamTime += elapsed;
        else if (!player.pauseGame && !player.pauseMovement && !player.npcInteraction &&
                 !movement.isRespawning && !movement.isMoving && !movement.IsClimbingLadder &&
                 movement.GetMovementState() == MovementStates.Still)
            pendingStandingTime += elapsed;
    }

    // Call this when the Statistics menu opens, before populating its labels.
    public void SaveStatistics()
    {
        if (!data)
            return;

        if (DataPersistanceManager.instance)
            DataPersistanceManager.instance.SaveGame();
        else
            FlushStatistics();
    }

    internal void FlushStatistics()
    {
        if (!data)
            return;

        double now = Time.realtimeSinceStartupAsDouble;
        AddPlayTime((float)(now - lastPlayTimeUpdate));
        lastPlayTimeUpdate = now;
        AddLongestSession();

        AddStepsTaken(pendingSteps);
        AddStepsReplenished(pendingStepsReplenished);
        AddRespawns(pendingRespawns);
        AddTimeStandingStill(pendingStandingTime);

        pendingSteps = 0;
        pendingStepsReplenished = 0;
        pendingRespawns = 0;
        pendingStandingTime = 0f;
        CheckProgress();
    }

    internal void FinishLevelAttempt()
    {
        if (!IsTrackingLevel)
            return;

        AddAverageStepsPerLevel(attemptSteps);
        attemptClosed = true;
    }

    internal void SaveLevelExit()
    {
        FinishLevelAttempt();
        SaveStatistics();
    }

    internal void RecordLevelCompletion()
    {
        if (!IsTrackingLevel || attemptCompleted)
            return;

        attemptCompleted = true;
        LevelNames level = map.levelName;
        if (level != LevelNames.None && AddUnique(Statistics.completedLevels, level))
        {
            AddLevelsComplete(1);
            if (IsRegionFinale(level))
                AddRegionsComplete(1);
        }

        string levelName = map.mapInfo_ToSave?.mapNameDisplay?.mapNameDisplay_english;
        if (string.IsNullOrWhiteSpace(levelName))
            levelName = map.gameObject.scene.name;

        AddFastestLevelCOmpletion(levelName, attemptTime);
        AddSlowestLevelCOmpletion(levelName, attemptTime);
        AddAverageLevelCompletionTime(attemptTime);
        AddTimeInFreeCam(attemptFreeCamTime);
    }

    private void RecordStep()
    {
        if (!IsTrackingLevel || !movement || !movement.isRespawningFirstTime)
            return;

        pendingSteps++;
        attemptSteps++;
    }

    internal void RecordStepRefill(int restored)
    {
        if (IsTrackingLevel && movement && movement.isRespawningFirstTime && restored > 0)
            pendingStepsReplenished += restored;
    }

    internal void RecordRespawn(bool stepsExhausted)
    {
        if (!IsTrackingLevel)
            return;

        pendingRespawns++;
        if (stepsExhausted)
            AddStepsEmptied(1);
    }

    internal void RecordTravel(int spaces, bool inWater, bool ceilingClimbing)
    {
        if (!IsTrackingLevel || spaces <= 0)
            return;

        if (inWater)
            AddAbilitiesPerformed_SwimDistance(spaces);
        if (ceilingClimbing)
            AddAbilitiesPerformed_CeilingClimb(spaces);
    }

    internal void RecordHint()
    {
        if (IsTrackingLevel)
            AddHintsUsed(1);
    }

    private void RecordAbility()
    {
        if (IsTrackingLevel)
            AddAbilitiesPerformed(1);
    }

    private void RecordJump()
    {
        if (IsTrackingLevel)
            AddAbilitiesPerformed_Jump(1);
    }

    internal void RecordGrapplingHook()
    {
        if (IsTrackingLevel)
            AddAbilitiesPerformed_GrapplingHook(1);
    }

    private void RecordHandDrill()
    {
        if (IsTrackingLevel)
            AddAbilitiesPerformed_HandDrill(1);
    }

    private void RecordDrillHelmet()
    {
        if (IsTrackingLevel)
            AddAbilitiesPerformed_DrillHelmet(1);
    }

    private void RecordDrillBoots()
    {
        if (IsTrackingLevel)
            AddAbilitiesPerformed_DrillBoots(1);
    }

    private void RecordEssence()
    {
        if (IsTrackingLevel)
            AddPickupEssence(1);
    }

    internal void RecordSkinFound()
    {
        if (IsTrackingLevel)
            AddPickupSkin(1);
    }

    private void RecordSkinBought()
    {
        if (data)
            AddSkinBought(1);
    }

    internal void RecordTabletRead(int textEntry, Vector3 position)
    {
        if (!IsTrackingLevel)
            return;

        string key = map.gameObject.scene.name + "/" + textEntry + "/" + position.ToString("F3", CultureInfo.InvariantCulture);
        if (AddUnique(Statistics.tabletsRead, key))
            AddTabletRed(1);
    }

    internal void RecordDialogueStarted(NPCs npc, int levelNumber)
    {
        if (IsTrackingLevel && npc != NPCs.None && AddUnique(Statistics.dialoguesStarted, DialogueKey(npc, levelNumber)))
            AddDialogueInterracted(1);
    }

    public void CheckProgress()
    {
        if (!data)
            return;

        Array.Clear(collectiblePointsByLevel, 0, collectiblePointsByLevel.Length);
        var savedMaps = data.mapInfo_StoreList?.map_SaveInfo_List;
        if (savedMaps != null)
        {
            foreach (Map_SaveInfo savedMap in savedMaps)
            {
                if (savedMap == null || !TryGetLevel(savedMap.mapName, out LevelNames level))
                    continue;

                int index = (int)level;
                collectiblePointsByLevel[index] = Mathf.Max(collectiblePointsByLevel[index], GetCollectiblePoints(savedMap));
            }
        }

        int progressPoints = Statistics.completedLevels.Count + Mathf.Min(32, Statistics.pickup_SkinsBought);
        foreach (int points in collectiblePointsByLevel)
            progressPoints += points;

        foreach (var _ in GetFinishedDialogues(data.charatersData_Store, includeStepellier: false))
            progressPoints++;

        Statistics.progress = 100f * Mathf.Min(progressPoints, GameProgressTotal) / GameProgressTotal;
    }

    private static int GetCollectiblePoints(Map_SaveInfo savedMap)
    {
        int essencePoints = 0;
        if (savedMap.essenceList != null)
        {
            foreach (EssenceInfo essence in savedMap.essenceList)
            {
                if (essence?.isTaken != true)
                    continue;

                essencePoints++;
                if (essencePoints == 10)
                    break;
            }
        }

        int footprintPoints = 0;
        if (savedMap.maxStepList != null)
        {
            foreach (MaxStepInfo footprint in savedMap.maxStepList)
            {
                if (footprint?.isTaken != true)
                    continue;

                footprintPoints++;
                if (footprintPoints == 3)
                    break;
            }
        }

        return essencePoints + footprintPoints + (savedMap.levelSkin?.isTaken == true ? 1 : 0);
    }

    private void ImportExistingProgress()
    {
        PlayerStatistics statistics = Statistics;
        statistics.visitedLevels ??= new List<string>();
        statistics.completedLevels ??= new List<LevelNames>();
        statistics.tabletsRead ??= new List<string>();
        statistics.dialoguesStarted ??= new List<string>();

        if (statistics.initialized)
            return;

        // Recover counts that older saves already record elsewhere.
        var savedMaps = data.mapInfo_StoreList?.map_SaveInfo_List;
        if (savedMaps != null)
        {
            foreach (Map_SaveInfo savedMap in savedMaps)
            {
                if (savedMap == null || string.IsNullOrEmpty(savedMap.mapName))
                    continue;

                if (!TryGetLevel(savedMap.mapName, out LevelNames level))
                {
                    AddUnique(statistics.visitedLevels, savedMap.mapName);
                    continue;
                }

                if (!AddUnique(statistics.visitedLevels, level.ToString()))
                    continue;

                if (savedMap.isCompleted && AddUnique(statistics.completedLevels, level))
                {
                    AddLevelsComplete(1);
                    if (IsRegionFinale(level))
                        AddRegionsComplete(1);
                }

                AddPickupEssence(savedMap.essenceList?.Count(essence => essence != null && essence.isTaken) ?? 0);
                if (savedMap.levelSkin?.isTaken == true)
                    AddPickupSkin(1);
            }
        }

        SkinsWardrobeInfo wardrobe = data.skinsInfo_Store?.skinWardrobeInfo;
        if (wardrobe != null)
        {
            foreach (FieldInfo field in typeof(SkinsWardrobeInfo).GetFields())
            {
                if (field.FieldType != typeof(WardrobeSkinState) || field.Name == nameof(SkinsWardrobeInfo.skin_Default))
                    continue;

                var state = (WardrobeSkinState)field.GetValue(wardrobe);
                if (state == WardrobeSkinState.Bought || state == WardrobeSkinState.Selected)
                    AddSkinBought(1);
            }
        }

        foreach (var (npc, levelNumber) in GetFinishedDialogues(data.charatersData_Store, includeStepellier: true))
        {
            if (AddUnique(statistics.dialoguesStarted, DialogueKey(npc, levelNumber)))
                AddDialogueInterracted(1);
        }

        statistics.initialized = true;
    }

    private static IEnumerable<(NPCs npc, int levelNumber)> GetFinishedDialogues(CharatersData characters, bool includeStepellier)
    {
        if (characters == null)
            yield break;

        (NPCs npc, NPCData dialogue)[] characterData =
        {
            (NPCs.Floriel, characters.floriel_Data),
            (NPCs.Granith, characters.granith_Data),
            (NPCs.Archie, characters.archie_Data),
            (NPCs.Aisa, characters.aisa_Data),
            (NPCs.Mossy, characters.mossy_Data),
            (NPCs.Larry, characters.larry_Data),
            (NPCs.Stepellier, includeStepellier ? characters.stepellier_Data : null),
        };

        foreach (var (npc, dialogue) in characterData)
        {
            if (dialogue == null)
                continue;

            if (dialogue.level_1_DialogueFinished) yield return (npc, 1);
            if (dialogue.level_2_DialogueFinished) yield return (npc, 2);
            if (dialogue.level_3_DialogueFinished) yield return (npc, 3);
            if (dialogue.level_4_DialogueFinished) yield return (npc, 4);
            if (dialogue.level_5_DialogueFinished) yield return (npc, 5);
        }
    }

    private static string DialogueKey(NPCs npc, int levelNumber) => npc + "/" + levelNumber;

    private bool TryGetLevel(string mapName, out LevelNames level)
    {
        if (mapName != null && levelNamesByMapName.TryGetValue(mapName, out level))
            return true;

        if (!Enum.TryParse(mapName?.Replace("_Lv_", "_Lv"), out level) ||
            level < LevelNames.Rivergreen_Lv1 || level > LevelNames.HUB)
            return false;

        levelNamesByMapName.Add(mapName, level);
        return true;
    }

    private static bool IsRegionFinale(LevelNames level) =>
        level == LevelNames.Rivergreen_Lv5 || level == LevelNames.Sandlands_Lv5 ||
        level == LevelNames.Frostfield_Lv5 || level == LevelNames.Firevein_Lv5 ||
        level == LevelNames.Witchmire_Lv5 || level == LevelNames.Metalworks_Lv5;

    private static bool AddUnique<T>(List<T> entries, T value)
    {
        if (entries.Contains(value))
            return false;

        entries.Add(value);
        return true;
    }

    public void AddPlayTime(float time) => Statistics.playTime += time;
    public void AddSessions() => Statistics.sessions++;
    public void AddLongestSession()
    {
        float elapsed = (float)(Time.realtimeSinceStartupAsDouble - sessionStartedAt);
        Statistics.longestSession = Mathf.Max(Statistics.longestSession, elapsed);
    }

    public void AddStepsTaken(int steps) => Statistics.stepsTaken += steps;
    public void AddStepsReplenished(int steps) => Statistics.stepsReplenished += steps;
    public void AddStepsEmptied(int steps) => Statistics.stepsEmptied += steps;
    public void AddRegionsComplete(int regions) => Statistics.regionsCompleted += regions;
    public void AddLevelsComplete(int levels) => Statistics.levelsCompleted += levels;
    public void AddLevelsReplayed(int levels) => Statistics.levelsReplayed += levels;

    public void AddFastestLevelCOmpletion(string name, float time)
    {
        if (Statistics.levelCompletionSamples == 0 || time < Statistics.fastestLevelCompletion_Time)
        {
            Statistics.fastestLevelCompletion_Name = name;
            Statistics.fastestLevelCompletion_Time = time;
        }
    }

    public void AddSlowestLevelCOmpletion(string name, float time)
    {
        if (Statistics.levelCompletionSamples == 0 || time > Statistics.slowestLevelCompletion_Time)
        {
            Statistics.slowestLevelCompletion_Name = name;
            Statistics.slowestLevelCompletion_Time = time;
        }
    }

    public void AddAverageLevelCompletionTime(float time)
    {
        Statistics.levelCompletionTimeTotal += time;
        Statistics.levelCompletionSamples++;
        Statistics.averageLevelCompletionTime = (float)(Statistics.levelCompletionTimeTotal / Statistics.levelCompletionSamples);
    }

    public void AddRespawns(int respawns) => Statistics.respawnsTaken += respawns;
    public void AddTimeStandingStill(float time) => Statistics.timeStandingStill += time;
    public void AddTimeInFreeCam(float time) => Statistics.TimeInFreeCam += time;
    public void AddHintsUsed(int hints) => Statistics.hintsUsed += hints;

    public void AddAverageStepsPerLevel(int steps)
    {
        Statistics.levelStepsTotal += steps;
        Statistics.levelStepsSamples++;
        Statistics.averageStepsPerLevel = (float)Statistics.levelStepsTotal / Statistics.levelStepsSamples;
    }

    public void AddAbilitiesPerformed(int abilities) => Statistics.abilitiesPerformed += abilities;

    public void AddAbilitiesPerformed_Jump(int uses)
    {
        Statistics.ability_Jump_Performed += uses;
        AddAbilitiesPerformed(uses);
    }

    public void AddAbilitiesPerformed_GrapplingHook(int uses)
    {
        Statistics.ability_GrapplingHook_Performed += uses;
        AddAbilitiesPerformed(uses);
    }

    public void AddAbilitiesPerformed_HandDrill(int uses)
    {
        Statistics.ability_HandDrill_Performed += uses;
        AddAbilitiesPerformed(uses);
    }

    public void AddAbilitiesPerformed_DrillHelmet(int uses)
    {
        Statistics.ability_DrillHelmet_Performed += uses;
        AddAbilitiesPerformed(uses);
    }

    public void AddAbilitiesPerformed_DrillBoots(int uses)
    {
        Statistics.ability_DrillBoots_Performed += uses;
        AddAbilitiesPerformed(uses);
    }

    public void AddAbilitiesPerformed_SwimDistance(int spaces) => Statistics.ability_SwimDistance_Performed += spaces;
    public void AddAbilitiesPerformed_CeilingClimb(int spaces) => Statistics.ability_CeilingClimbDisatnace_Performed += spaces;
    public void AddPickupEssence(int essence) => Statistics.pickup_EssencePickedUp += essence;
    public void AddPickupSkin(int skin) => Statistics.pickup_SkinsFound += skin;
    public void AddSkinBought(int skins) => Statistics.pickup_SkinsBought += skins;
    public void AddTabletRed(int tablets) => Statistics.pickup_TabletsRed += tablets;
    public void AddDialogueInterracted(int dialogues) => Statistics.pickup_DialoguesInterracted += dialogues;
}

[Serializable]
public class PlayerStatistics
{
    public float progress;
    public float playTime;
    public int sessions;
    public float longestSession;

    public int stepsTaken;
    public int stepsReplenished;
    public int stepsEmptied;

    public int regionsCompleted;
    public int levelsCompleted;
    public int levelsReplayed;
    public string fastestLevelCompletion_Name;
    public float fastestLevelCompletion_Time;
    public string slowestLevelCompletion_Name;
    public float slowestLevelCompletion_Time;
    public int respawnsTaken;
    public float averageLevelCompletionTime;
    public float timeStandingStill;
    public float averageStepsPerLevel;
    public float TimeInFreeCam;
    public int hintsUsed;

    public int abilitiesPerformed;
    public int ability_Jump_Performed;
    public int ability_GrapplingHook_Performed;
    public int ability_HandDrill_Performed;
    public int ability_DrillHelmet_Performed;
    public int ability_DrillBoots_Performed;
    public int ability_SwimDistance_Performed;
    public int ability_CeilingClimbDisatnace_Performed;

    public int pickup_EssencePickedUp;
    public int pickup_SkinsFound;
    public int pickup_SkinsBought;
    public int pickup_TabletsRed;
    public int pickup_DialoguesInterracted;

    public bool initialized;
    public List<string> visitedLevels = new List<string>();
    public List<LevelNames> completedLevels = new List<LevelNames>();
    public List<string> tabletsRead = new List<string>();
    public List<string> dialoguesStarted = new List<string>();
    public double levelCompletionTimeTotal;
    public int levelCompletionSamples;
    public long levelStepsTotal;
    public int levelStepsSamples;
}
