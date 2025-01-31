using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager_v2 : Singleton<PlayerManager_v2>
{
    [Header("Player Object")]
    public GameObject playerObject;
    public GameObject playerBodyObject;

    [Header("Game Paused")]
    public bool pauseGame;
}
