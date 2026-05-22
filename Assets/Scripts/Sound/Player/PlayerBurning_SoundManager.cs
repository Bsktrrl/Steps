using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBurning_SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource player_Fire_Start;
    [SerializeField] AudioSource player_Fire_Ongoing;
    [SerializeField] AudioSource player_Fire_End;


    //--------------------


    private void OnEnable()
    {
        Player_Burning.Action_PlayerStartedBurning += Fire_Start;
        Player_Burning.Action_PlayerEndBurning += Fire_End;
    }
    private void OnDisable()
    {
        Player_Burning.Action_PlayerStartedBurning -= Fire_Start;
        Player_Burning.Action_PlayerEndBurning -= Fire_End;
    }


    //--------------------


    void Fire_Start()
    {
        player_Fire_Start.PlayOneShot(player_Fire_Start.clip);
        player_Fire_Ongoing.Stop();

        StartCoroutine(StartFireOngoing_Delay(0.2f));
    }
    void Fire_Ongoing()
    {
        player_Fire_Ongoing.Play();
    }
    void Fire_End()
    {
        player_Fire_End.PlayOneShot(player_Fire_End.clip);
        player_Fire_Ongoing.Stop();
    }

    IEnumerator StartFireOngoing_Delay(float waitTimer)
    {
        yield return new WaitForSeconds(waitTimer);

        Fire_Ongoing();
    }
}
