using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block_Switch_Manager : Singleton<Block_Switch_Manager>
{
    [Header("Sound")]
    public AudioClip fence_Sound;
}

[Serializable]
public class SwitchesData
{
    public SwitchInfo switches_Rivergreen;
    public SwitchInfo switches_Sandlands;
    public SwitchInfo switches_Frostfield;
    public SwitchInfo switches_Firevein;
    public SwitchInfo switches_Witchmire;
}
[Serializable]
public class SwitchInfo
{
    public bool switch_1;
    public bool switch_2;
    public bool switch_3;
    public bool switch_4;
    public bool switch_5;
    public bool switch_6;
    public bool switch_7;
    public bool switch_8;
    public bool switch_9;
    public bool switch_10;
    public bool switch_11;
    public bool switch_12;
    public bool switch_13;
    public bool switch_14;
    public bool switch_15;
    public bool switch_16;
    public bool switch_17;
    public bool switch_18;
    public bool switch_19;
    public bool switch_20;
}

[Serializable]
public enum MoveDirection
{
    None,

    Forward,
    Backward,
    Right,
    Left,
    Up,
    Down,
}
