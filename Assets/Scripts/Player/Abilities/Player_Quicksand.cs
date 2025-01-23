using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Quicksand : Singleton<Player_Quicksand>
{
    [Header("General")]
    public bool isInQuicksand;

    [Header("Quicksand")]
    public int quicksandCounter;
}
