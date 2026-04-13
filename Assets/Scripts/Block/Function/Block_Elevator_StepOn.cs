using UnityEngine;

public class Block_Elevator_StepOn : MonoBehaviour
{
    public bool isStandingOnBlock;


    //--------------------


    private void Update()
    {
        isStandingOnBlock = Movement.Instance != null && Movement.Instance.blockStandingOn == gameObject;

        if (gameObject.GetComponent<MovingMachineScript>())
        {
            if (isStandingOnBlock)
            {
                gameObject.GetComponent<MovingMachineScript>().StartMovement();
            }
            else
            {
                gameObject.GetComponent<MovingMachineScript>().StopMovement();
            }
        }
    }
}