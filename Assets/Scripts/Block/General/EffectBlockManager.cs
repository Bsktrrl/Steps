using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBlockManager : Singleton<EffectBlockManager>
{
    [Header("EffectBlock Types")]
    public GameObject effectBlock_SpawnPoint_Canvas;
    public GameObject effectBlock_RefillSteps_Canvas;
    public GameObject effectBlock_Pusher_Canvas;
    public GameObject effectBlock_Teleporter_Canvas;

    [Header("Colors")]
    public Color color_Grass;
    public Color color_Wood;
    public Color color_Sand;
    public Color color_SandBlock;


    [Header("Teleporters")]
    public int teleporterCounter;
    public Color teleporter_Color_1;
    public Color teleporter_Color_2;
    public Color teleporter_Color_3;
    public Color teleporter_Color_4;
    public Color teleporter_Color_5;
    public Color teleporter_Color_6;
    public Color teleporter_Color_7;
    public Color teleporter_Color_8;
    public Color teleporter_Color_9;
    public Color teleporter_Color_10;
}
