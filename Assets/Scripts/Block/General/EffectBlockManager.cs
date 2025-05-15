using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBlockManager : Singleton<EffectBlockManager>
{
    [Header("EffectBlock Types")]
    public GameObject effectBlock_SpawnPoint_Prefab;
    public GameObject effectBlock_RefillSteps_Prefab;
    public GameObject effectBlock_Pusher_Prefab;
    public GameObject effectBlock_Teleporter_Prefab;
    public GameObject effectBlock_Moveable_Prefab;
    public GameObject effectBlock_MushroomCircle_Prefab;
}
