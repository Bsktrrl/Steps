using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Teleporter : MonoBehaviour
{
    


    //--------------------


    private void Start()
    {
        Player_Movement.Action_StepTaken += TeleportPlayer;
    }


    //--------------------


    void TeleportPlayer()
    {
        if (MainManager.Instance.block_StandingOn.block.GetComponent<Block_Teleport>())
        {
            Vector3 newPos = MainManager.Instance.block_StandingOn.block.GetComponent<Block_Teleport>().newLandingSpot.transform.position;

            gameObject.transform.position = new Vector3(newPos.x, newPos.y + 0.85f, newPos.z);
        }
    }
}
