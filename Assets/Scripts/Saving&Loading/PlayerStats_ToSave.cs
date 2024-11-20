using System;
using UnityEngine;

[Serializable]
public class Stats
{
    [Header("Steps")]
    public int steps_Max = 5;
    public int steps_Current = 5;

    [Header("Items")]
    public ItemsGot itemsGot;

    [Header("Abilities")]
    public AbilitiesGot abilitiesGot;

    public void ResetStats()
    {
        steps_Max = 10;
        steps_Current = 10;

        itemsGot.coin = 0;
        itemsGot.collectable = 0;

        abilitiesGot.FenceSneak = false;
        abilitiesGot.SwimSuit = false;
        abilitiesGot.SwiftSwim = false;
        abilitiesGot.Flippers = false;
        abilitiesGot.LavaSuit = false;
        abilitiesGot.LavaSwiftSwim = false;
        abilitiesGot.HikerGear = false;
        abilitiesGot.IceSpikes = false;
        abilitiesGot.GrapplingHook = false;
        abilitiesGot.Hammer = false;
        abilitiesGot.ClimbingGear = false;
        abilitiesGot.Dash = false;
        abilitiesGot.Ascend = false;
        abilitiesGot.Descend = false;
        abilitiesGot.ControlStick = false;
    }
}


//--------------------


[Serializable]
public class ItemsGot
{
    public int coin = 0;
    public int collectable = 0;
}

[Serializable]
public class AbilitiesGot
{
    public bool FenceSneak;
    public bool SwimSuit;
    public bool SwiftSwim;
    public bool Flippers;
    public bool LavaSuit;
    public bool LavaSwiftSwim;

    public bool HikerGear; //Moving up Slopes

    public bool IceSpikes;
    public bool GrapplingHook;
    public bool Hammer;
    public bool ClimbingGear;
    public bool Dash;
    public bool Ascend;
    public bool Descend;
    public bool ControlStick;
}
