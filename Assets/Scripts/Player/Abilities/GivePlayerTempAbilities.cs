using System.Collections.Generic;
using UnityEngine;

public class GivePlayerTempAbilities : MonoBehaviour
{
    [SerializeField] List<Abilities> abilitiesToGive_List;

    internal void ApplyTo(AbilitiesGot levelAbilities)
    {
        if (abilitiesToGive_List == null)
            return;

        foreach (Abilities ability in abilitiesToGive_List)
        {
            switch (ability)
            {
                case Abilities.Snorkel:
                    levelAbilities.Snorkel = true;
                    break;
                case Abilities.OxygenTank:
                    levelAbilities.OxygenTank = true;
                    break;
                case Abilities.Flippers:
                    levelAbilities.Flippers = true;
                    break;
                case Abilities.DrillHelmet:
                    levelAbilities.DrillHelmet = true;
                    break;
                case Abilities.DrillBoots:
                    levelAbilities.DrillBoots = true;
                    break;
                case Abilities.HandDrill:
                    levelAbilities.HandDrill = true;
                    break;
                case Abilities.SpringShoes:
                    levelAbilities.SpringShoes = true;
                    break;
                case Abilities.ClimingGloves:
                    levelAbilities.ClimingGloves = true;
                    break;
                case Abilities.GrapplingHook:
                    levelAbilities.GrapplingHook = true;
                    break;
            }
        }
    }
}
