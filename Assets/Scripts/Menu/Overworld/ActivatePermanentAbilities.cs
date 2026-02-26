using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        //print("0. CheckIfAbilityShouldUpdate");
        //Rivergreen
        for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count; i++)
        {
            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapName == rivergreenStats.levelToPlay && DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].isCompleted && (!PlayerStats.Instance.stats.abilitiesGot_Permanent.Snorkel && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers && !PlayerStats.Instance.stats.abilitiesGot_Permanent.OxygenTank))
            {
                string tempText =
                    DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Water1
                    + " <color=#B593D5>"
                    + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water
                    + "</color> "
                    + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Water2;

                PopUpManager_MainMenu.Instance.ShowPermanentAbilityPopup(tempText);

                ActivateAbilities(rivergreenStats, abilityToGetInRivergreenList);
                SaveAbilityData(Abilities.Snorkel);
                SaveAbilityData(Abilities.Flippers);
                SaveAbilityData(Abilities.OxygenTank);
            }

            //Sandlands
            if (sandlandsStats.isCompleted && (!PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillHelmet && !PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillBoots))
            {
                string tempText =
                    DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Sand1
                    + " <color=#B593D5>"
                    + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand
                    + "</color> "
                    + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Sand2;

                PopUpManager_MainMenu.Instance.ShowPermanentAbilityPopup(tempText);

                ActivateAbilities(sandlandsStats, abilityToGetInSandlandsList);
                SaveAbilityData(Abilities.DrillHelmet);
                SaveAbilityData(Abilities.DrillBoots);
            }

            //Frostfield
            if (frostfieldStats.isCompleted && !PlayerStats.Instance.stats.abilitiesGot_Permanent.SpringShoes)
            {
                string tempText =
                    DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Ice1
                    + " <color=#B593D5>"
                    + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice
                    + "</color> "
                    + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Ice2;

                PopUpManager_MainMenu.Instance.ShowPermanentAbilityPopup(tempText);

                ActivateAbilities(frostfieldStats, abilityToGetInFrostfieldList);
                SaveAbilityData(Abilities.SpringShoes);
            }

            //Firevein
            if (fireveinStats.isCompleted && !PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook)
            {
                string tempText =
                    DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Lava1
                    + " <color=#B593D5>"
                    + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava
                    + "</color> "
                    + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Lava2;

                PopUpManager_MainMenu.Instance.ShowPermanentAbilityPopup(tempText);

                ActivateAbilities(fireveinStats, abilityToGetInFireveinList);
                SaveAbilityData(Abilities.GrapplingHook);
            }

            //Witchmire
            if (witchmireStats.isCompleted && !PlayerStats.Instance.stats.abilitiesGot_Permanent.HandDrill)
            {
                string tempText =
                    DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Swamp1
                    + " <color=#B593D5>"
                    + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp
                    + "</color> "
                    + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Swamp2;

                PopUpManager_MainMenu.Instance.ShowPermanentAbilityPopup(tempText);

                ActivateAbilities(witchmireStats, abilityToGetInWitchmireList);
                SaveAbilityData(Abilities.HandDrill);
            }

            //Metalwork
            if (metalworksStats.isCompleted && !PlayerStats.Instance.stats.abilitiesGot_Permanent.ClimingGloves)
            {
                string tempText =
                    DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Metal1
                    + " <color=#B593D5>"
                    + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal
                    + "</color> "
                    + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Metal2;

                PopUpManager_MainMenu.Instance.ShowPermanentAbilityPopup(tempText);

                ActivateAbilities(metalworksStats, abilityToGetInMetalworksList);
                SaveAbilityData(Abilities.ClimingGloves);
            }
        }
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
        
        DataPersistanceManager.instance.SaveGame();
    }


    //--------------------


    void ActivateAbilities(LoadLevel levelStats, List<Abilities> abilityList)
    {
        if (levelStats.isCompleted)
        {
            for (int i = 0; i < abilityList.Count; i++)
            {
                SaveAbilityData(abilityList[i]);
            }
        }
    }
}
