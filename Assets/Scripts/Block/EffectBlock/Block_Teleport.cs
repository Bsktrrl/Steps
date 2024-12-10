using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Teleport : MonoBehaviour
{
    public static event Action Action_StartTeleport;
    public static event Action Action_EndTeleport;

    public GameObject newLandingSpot;


    //--------------------


    private void OnEnable()
    {
        Player_Movement.Action_StepTaken += TeleportPlayer;
        Action_StartTeleport += StartTeleport_Action;
        Action_EndTeleport += EndTeleport_Action;
    }

    private void OnDisable()
    {
        Player_Movement.Action_StepTaken -= TeleportPlayer;
        Action_StartTeleport -= StartTeleport_Action;
        Action_EndTeleport -= EndTeleport_Action;
    }


    //--------------------


    void TeleportPlayer()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block == gameObject && newLandingSpot)
        {
            StartCoroutine(TeleportWait(0.005f));
        }
    }

    IEnumerator TeleportWait(float waitTime)
    {
        int stepTemp = PlayerStats.Instance.stats.steps_Current;

        PlayerManager.Instance.isTransportingPlayer = true;
        PlayerManager.Instance.pauseGame = true;
        Player_Movement.Instance.movementStates = MovementStates.Moving;

        Action_StartTeleport?.Invoke();

        yield return new WaitForSeconds(waitTime);

        Vector3 newPos = gameObject.GetComponent<Block_Teleport>().newLandingSpot.transform.position;
        PlayerManager.Instance.player.transform.position = new Vector3(newPos.x, newPos.y + PlayerManager.Instance.player.GetComponent<Player_Movement>().heightOverBlock, newPos.z);

        yield return new WaitForSeconds(waitTime);

        Player_Movement.Instance.movementStates = MovementStates.Still;
        PlayerManager.Instance.isTransportingPlayer = false;
        PlayerManager.Instance.pauseGame = false;

        PlayerStats.Instance.stats.steps_Current = stepTemp - gameObject.GetComponent<Block_Teleport>().newLandingSpot.GetComponent<BlockInfo>().movementCost;

        Player_BlockDetector.Instance.Update_BlockStandingOn();
        Player_BlockDetector.Instance.RaycastSetup();
        Player_Movement.Instance.Action_StepTakenInvoke();

        if (!PlayerStats.Instance.stats.abilitiesGot_Permanent.IceSpikes && !PlayerStats.Instance.stats.abilitiesGot_Temporary.IceSpikes)
        {
            yield return new WaitForSeconds(waitTime * 20);

            Player_Movement.Instance.IceGlide();
        }
        
        Action_EndTeleport?.Invoke();
    }

    void StartTeleport_Action()
    {
        Player_Movement.Action_StepTaken -= TeleportPlayer;
    }
    void EndTeleport_Action()
    {
        Player_Movement.Action_StepTaken += TeleportPlayer;
    }
}
