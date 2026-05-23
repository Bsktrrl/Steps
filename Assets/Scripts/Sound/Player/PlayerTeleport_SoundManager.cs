using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport_SoundManager : MonoBehaviour
{
    private void OnEnable()
    {
        Block_Teleport.Action_StartTeleport += PlayTeleportSound;
    }
    private void OnDisable()
    {
        Block_Teleport.Action_StartTeleport -= PlayTeleportSound;
    }


    //--------------------


    void PlayTeleportSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        GetComponent<AudioSource>().PlayOneShot(audioSource.clip);
    }
}
