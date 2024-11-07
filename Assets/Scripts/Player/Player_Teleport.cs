using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Teleport : MonoBehaviour
{
    public bool isTeleporting;


    //--------------------


    private void Start()
    {
        Player_Movement.Action_StepTaken += TeleportPlayer;
    }


    //--------------------


    void TeleportPlayer()
    {
        if (MainManager.Instance.block_StandingOn.blockAbility == BlockAbility.Teleporter)
        {
            if (MainManager.Instance.block_StandingOn.block.GetComponent<Block_Teleport>())
            {
                if (MainManager.Instance.block_StandingOn.block.GetComponent<Block_Teleport>().newLandingSpot)
                {
                    StartCoroutine(TeleportWait(0.01f));
                }
            }
        }
    }

    IEnumerator TeleportWait(float waitTime)
    {
        isTeleporting = true;

        yield return new WaitForSeconds(waitTime);

        Vector3 newPos = MainManager.Instance.block_StandingOn.block.GetComponent<Block_Teleport>().newLandingSpot.transform.position;

        gameObject.transform.position = new Vector3(newPos.x, newPos.y + gameObject.GetComponent<Player_Movement>().heightOverBlock, newPos.z);

        isTeleporting = false;
    }
}