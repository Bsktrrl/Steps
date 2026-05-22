using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource player_Respawn_Hold;
    [SerializeField] AudioSource player_Respawn;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_RespawnPlayerByHolding += StartRespawnHold;

        Movement.Action_RespawnPlayer += PlayPlayerRespawn;
    }
    private void OnDisable()
    {
        
    }


    //--------------------


    void StartRespawnHold()
    {

    }
    void StopRespawnHold()
    {

    }
    void PlayPlayerRespawn()
    {

    }
}
