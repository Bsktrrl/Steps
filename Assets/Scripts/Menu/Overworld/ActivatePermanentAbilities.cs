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


    public void UpdatePermanentAbilities()
    {
        ActivateAbilities(rivergreenStats, abilityToGetInRivergreenList);
        ActivateAbilities(sandlandsStats, abilityToGetInSandlandsList);
        ActivateAbilities(frostfieldStats, abilityToGetInFrostfieldList);
        ActivateAbilities(fireveinStats, abilityToGetInFireveinList);
        ActivateAbilities(witchmireStats, abilityToGetInWitchmireList);
        ActivateAbilities(metalworksStats, abilityToGetInMetalworksList);
    }

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
