using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    public static event Action playerReset;

    private void Start()
    {
        MainManager.noMoreSteps += ResetPlayerPos;
    }


    //--------------------


    void ResetPlayerPos()
    {
        transform.position = MainManager.Instance.startPos;
        StartCoroutine(DespawnTime(0.5f));
    }

    IEnumerator DespawnTime(float waitTime)
    {
        playerReset?.Invoke();

        gameObject.GetComponent<PlayerMovement>().moveToPos = MainManager.Instance.startPos;
        gameObject.GetComponent<PlayerMovement>().movementState = MovementState.Still;
        gameObject.GetComponent<PlayerMovement>().movementDirection = MovementDirection.None;

        MainManager.Instance.gamePaused = true;

        yield return new WaitForSeconds(waitTime);

        MainManager.Instance.gamePaused = false;
    }

}
