using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Teleport : MonoBehaviour
{
    public GameObject newLandingSpot;


    //--------------------


    private void Start()
    {
        Player_Movement.Action_StepTaken += TeleportPlayer;
    }


    //--------------------


    void TeleportPlayer()
    {
        if (MainManager.Instance.block_StandingOn_Current.block == gameObject && newLandingSpot)
        {
            StartCoroutine(TeleportWait(0.01f));
        }
    }

    IEnumerator TeleportWait(float waitTime)
    {
        MainManager.Instance.isTeleporting = true;
        MainManager.Instance.pauseGame = true;
        Player_Movement.Instance.movementStates = MovementStates.Moving;

        yield return new WaitForSeconds(waitTime);

        Vector3 newPos = gameObject.GetComponent<Block_Teleport>().newLandingSpot.transform.position;

        MainManager.Instance.player.transform.position = new Vector3(newPos.x, newPos.y + MainManager.Instance.player.GetComponent<Player_Movement>().heightOverBlock, newPos.z);

        yield return new WaitForSeconds(waitTime);

        Player_Movement.Instance.movementStates = MovementStates.Still;
        MainManager.Instance.isTeleporting = false;
        MainManager.Instance.pauseGame = false;
    }
}
