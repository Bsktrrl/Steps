using System;
using UnityEngine;

[Serializable]
public class Stats
{
    [Header("Steps")]
    public int steps_Max = 7;
    public int steps_Current = 7;

    [Header("Items")]
    public ItemsGot itemsGot = new ItemsGot();

    [Header("Abilities")]
    public AbilitiesGot abilitiesGot_Permanent = new AbilitiesGot();
    public AbilitiesGot abilitiesGot_Temporary = new AbilitiesGot();

    public void ResetStats()
    {
        steps_Max = 7;
        steps_Current = 7;

        itemsGot.essence_Max = 0;
        itemsGot.essence_Current = 0;
        itemsGot.skin = 0;

        abilitiesGot_Permanent.Snorkel = false;
        abilitiesGot_Permanent.Flippers = false;
        abilitiesGot_Permanent.OxygenTank = false;
        abilitiesGot_Permanent.SpringShoes = false;
        abilitiesGot_Permanent.GrapplingHook = false;
        abilitiesGot_Permanent.ClimingGloves = false;
        abilitiesGot_Permanent.HandDrill = false;
        abilitiesGot_Permanent.DrillHelmet = false;
        abilitiesGot_Permanent.DrillBoots = false;

        abilitiesGot_Temporary.Snorkel = false;
        abilitiesGot_Temporary.Flippers = false;
        abilitiesGot_Temporary.OxygenTank = false;
        abilitiesGot_Temporary.SpringShoes = false;
        abilitiesGot_Temporary.GrapplingHook = false;
        abilitiesGot_Temporary.ClimingGloves = false;
        abilitiesGot_Temporary.HandDrill = false;
        abilitiesGot_Temporary.DrillHelmet = false;
        abilitiesGot_Temporary.DrillBoots = false;
    }
}


//--------------------


[Serializable]
public class ItemsGot
{
    public int essence_Max = 0;
    public int essence_Current = 0;
    public int skin = 0;
}

[Serializable]
public class AbilitiesGot
{
    public bool Snorkel;
    public bool Flippers;
    public bool OxygenTank;

    public bool DrillHelmet;
    public bool DrillBoots;

    public bool HandDrill;

    public bool SpringShoes;

    public bool GrapplingHook;

    public bool ClimingGloves;
}
