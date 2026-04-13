using UnityEngine;

public class Block_Elevator_StepOn : MonoBehaviour
{
    public bool isStandingOnBlock;

    private MovingMachineScript movingMachineScript;
    private Block_Elevator blockElevator;


    //--------------------


    private void Awake()
    {
        movingMachineScript = GetComponent<MovingMachineScript>();
        blockElevator = GetComponent<Block_Elevator>();
    }

    private void Update()
    {
        isStandingOnBlock = Movement.Instance != null && Movement.Instance.blockStandingOn == gameObject;

        if (movingMachineScript == null)
            return;

        bool shouldAnimate = isStandingOnBlock;

        if (blockElevator != null && blockElevator.IsSnappingToGrid)
            shouldAnimate = true;

        if (shouldAnimate)
            movingMachineScript.StartMovement();
        else
            movingMachineScript.StopMovement();
    }
}