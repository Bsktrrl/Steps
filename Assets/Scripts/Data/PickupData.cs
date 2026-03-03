using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupData : MonoBehaviour
{
    [Header("If an Essence, which in the level is it?")]
    public int essenceNo;

    [Header("If a Footprint, which in the level is it?")]
    public int footprintNo;

    [Header("If a Skin, which in the level is it?")]
    public int skinNo;

    [Header("If an Ability, which in the level is it?")]
    public int abilityNo;
}