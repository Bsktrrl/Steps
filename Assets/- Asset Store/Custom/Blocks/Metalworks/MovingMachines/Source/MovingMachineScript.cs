using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMachineScript : MonoBehaviour
{
    Animator anim;


    //--------------------


    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        StopMovement();
    }


    //--------------------


    public void StartMovement()
    {
        anim.SetBool("Moving", true);
        print("10. StartMovement");
    }
    public void StopMovement()
    {
        anim.SetBool("Moving", false);
        print("20. StopMovement");
    }
}
