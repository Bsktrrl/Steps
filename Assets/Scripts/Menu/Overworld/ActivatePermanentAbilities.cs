using System.Collections.Generic;
using UnityEngine;

public class ActivatePermanentAbilities : Singleton<ActivatePermanentAbilities>
{
    [Header("Rivergreen")]
    [SerializeField] LoadLevel rivergreenStats;
    [SerializeField] List<Abilities> abilityToGetInRivergreenList = new List<Abilities>();

    [Header("Sandlands")]
    [SerializeField] LoadLevel sandlandsStats;
    [SerializeField] List<Abilities> abilityToGetInSandlandsList = new List<Abilities>();

    [Header("Frostfield")]
    [SerializeField] LoadLevel frostfieldStats;
    [SerializeField] List<Abilities> abilityToGetInFrostfieldList = new List<Abilities>();

    [Header("Firevein")]
    [SerializeField] LoadLevel fireveinStats;
    [SerializeField] List<Abilities> abilityToGetInFireveinList = new List<Abilities>();

    [Header("Witchmire")]
    [SerializeField] LoadLevel witchmireStats;
    [SerializeField] List<Abilities> abilityToGetInWitchmireList = new List<Abilities>();

    [Header("Metalworks")]
    [SerializeField] LoadLevel metalworksStats;
    [SerializeField] List<Abilities> abilityToGetInMetalworksList = new List<Abilities>();


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += CheckIfAbilityShouldUpdate;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= CheckIfAbilityShouldUpdate;
    }


    //--------------------



    void CheckIfAbilityShouldUpdate()
    {
        DataManager data = DataManager.Instance;
        int language = (int)data.settingData_StoreList.currentLanguage;
        var languages = data.game_TextDatabase_Store?.gameText_LanguageList;
        var text = languages != null && language >= 0 && language < languages.Count ? languages[language] : null;

        ActivateAbilities(rivergreenStats, abilityToGetInRivergreenList,
            text?.finishedRegion_Message_Water1, text?.name_Region_Water, text?.finishedRegion_Message_Water2);
        ActivateAbilities(sandlandsStats, abilityToGetInSandlandsList,
            text?.finishedRegion_Message_Sand1, text?.name_Region_Sand, text?.finishedRegion_Message_Sand2);
        ActivateAbilities(frostfieldStats, abilityToGetInFrostfieldList,
            text?.finishedRegion_Message_Ice1, text?.name_Region_Ice, text?.finishedRegion_Message_Ice2);
        ActivateAbilities(fireveinStats, abilityToGetInFireveinList,
            text?.finishedRegion_Message_Lava1, text?.name_Region_Lava, text?.finishedRegion_Message_Lava2);
        ActivateAbilities(witchmireStats, abilityToGetInWitchmireList,
            text?.finishedRegion_Message_Swamp1, text?.name_Region_Swamp, text?.finishedRegion_Message_Swamp2);
        ActivateAbilities(metalworksStats, abilityToGetInMetalworksList,
            text?.finishedRegion_Message_Metal1, text?.name_Region_Metal, text?.finishedRegion_Message_Metal2);
    }

    void ActivateAbilities(LoadLevel finale, List<Abilities> rewards, string messageStart, string regionName, string messageEnd)
    {
        if (!finale || SaveLoad_MapInfo.Instance.GetMapInfo(finale.levelToPlay)?.isCompleted != true)
            return;

        AbilitiesGot permanent = DataManager.Instance.playerStats_Store.abilitiesGot_Permanent;
        bool unlocked = false;
        foreach (Abilities ability in rewards)
        {
            if (ability == Abilities.None || permanent.HasAbility(ability))
                continue;

            SaveAbilityData(ability);
            unlocked = true;
        }

        if (!unlocked)
            return;

        DataPersistanceManager.instance.SaveGame();
        if (!string.IsNullOrEmpty(regionName))
            PopUpManager_MainMenu.Instance.ShowPermanentAbilityPopup($"{messageStart} <color=#B593D5>{regionName}</color> {messageEnd}");
    }

    void SaveAbilityData(Abilities ability)
    {
        switch (ability)
        {
            case Abilities.None:
                break;

            case Abilities.Snorkel:
                DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.Snorkel = true;
                PlayerStats.Instance.stats.abilitiesGot_Permanent.Snorkel = true;
                break;
            case Abilities.Flippers:
                DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.Flippers = true;
                PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers = true;
                break;
            case Abilities.OxygenTank:
                DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.OxygenTank = true;
                PlayerStats.Instance.stats.abilitiesGot_Permanent.OxygenTank = true;
                break;
            case Abilities.DrillHelmet:
                DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.DrillHelmet = true;
                PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillHelmet = true;
                break;
            case Abilities.DrillBoots:
                DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.DrillBoots = true;
                PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillBoots = true;
                break;
            case Abilities.HandDrill:
                DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.HandDrill = true;
                PlayerStats.Instance.stats.abilitiesGot_Permanent.HandDrill = true;
                break;
            case Abilities.SpringShoes:
                DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.SpringShoes = true;
                PlayerStats.Instance.stats.abilitiesGot_Permanent.SpringShoes = true;
                break;
            case Abilities.ClimingGloves:
                DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.ClimingGloves = true;
                PlayerStats.Instance.stats.abilitiesGot_Permanent.ClimingGloves = true;
                break;
            case Abilities.GrapplingHook:
                DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.GrapplingHook = true;
                PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook = true;
                break;

            default:
                break;
        }
    }
}
