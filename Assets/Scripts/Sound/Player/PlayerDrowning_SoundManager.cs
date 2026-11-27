using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrowning_SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource player_Downing_Start;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_isDrowning += Drowning_Start;
    }
    private void OnDisable()
    {
        Movement.Action_isDrowning -= Drowning_Start;
    }


    //--------------------


    void Drowning_Start()
    {
        if (isActiveAndEnabled && player_Downing_Start)
            StartCoroutine(StartDrowningSound(0.25f));
    }
    IEnumerator StartDrowningSound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (isActiveAndEnabled && player_Downing_Start)
            player_Downing_Start.Play();
    }
}
