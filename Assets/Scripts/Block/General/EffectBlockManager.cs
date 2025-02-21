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
    //public Color color_Grass;
    //public Color color_Wood;
    //public Color color_Sand;
    //public Color color_SandBlock;


    [Header("Teleporters")]
    public int teleporterCounter;
}
