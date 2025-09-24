using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public SoundObjectType soundObjectType;
}

public enum SoundObjectType
{
    None,

    Water,
    Waterfall,

    Swampwater,
    Swampwaterfall,

    Mud,
    Mudfall,

    Lava,
    Lavafall,

    Quicksand,
    Quicksandfall,

    IceCrack,
}
