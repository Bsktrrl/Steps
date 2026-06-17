using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Map : Singleton<HUD_Map>
{
    public PlayerSpawnParameters playerStartPosition_Beginning;
    public PlayerSpawnParameters playerStartPosition_Later;
}

[Serializable]
public class PlayerSpawnParameters
{
    public Vector3 pos;
    public MovementDirection rot;
}
