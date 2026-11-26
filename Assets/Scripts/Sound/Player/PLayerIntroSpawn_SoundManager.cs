using UnityEngine;

public class PLayerIntroSpawn_SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource player_IntroSpawn_Start;


    //--------------------


    private void OnEnable()
    {
        PlayerSpawnScript.Action_HubSpawnStarted += PlayIntroSpawnSound;
    }
    private void OnDisable()
    {
        PlayerSpawnScript.Action_HubSpawnStarted -= PlayIntroSpawnSound;
    }


    //--------------------


    private void PlayIntroSpawnSound()
    {
        if (player_IntroSpawn_Start)
            player_IntroSpawn_Start.Play();
    }
}
