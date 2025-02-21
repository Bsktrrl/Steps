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
    public GameObject effectBlock_Moveable_Canvas;
}
