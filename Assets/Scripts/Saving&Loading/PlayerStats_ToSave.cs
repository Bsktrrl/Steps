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

        itemsGot.coin = 0;
        itemsGot.collectable = 0;

        abilitiesGot_Permanent.SwimSuit = false;
        abilitiesGot_Permanent.SwiftSwim = false;
        abilitiesGot_Permanent.Flippers = false;
        abilitiesGot_Permanent.Jumping = false;
        abilitiesGot_Permanent.GrapplingHook = false;
        abilitiesGot_Permanent.CeilingGrab = false;
        abilitiesGot_Permanent.Dash = false;
        abilitiesGot_Permanent.Ascend = false;
        abilitiesGot_Permanent.Descend = false;

        abilitiesGot_Temporary.SwimSuit = false;
        abilitiesGot_Temporary.SwiftSwim = false;
        abilitiesGot_Temporary.Flippers = false;
        abilitiesGot_Temporary.Jumping = false;
        abilitiesGot_Temporary.GrapplingHook = false;
        abilitiesGot_Temporary.CeilingGrab = false;
        abilitiesGot_Temporary.Dash = false;
        abilitiesGot_Temporary.Ascend = false;
        abilitiesGot_Temporary.Descend = false;
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
    public bool SwimSuit;
    public bool SwiftSwim;
    public bool Flippers;

    public bool Ascend;
    public bool Descend;

    public bool Dash;

    public bool Jumping;

    public bool GrapplingHook;

    public bool CeilingGrab;
}
