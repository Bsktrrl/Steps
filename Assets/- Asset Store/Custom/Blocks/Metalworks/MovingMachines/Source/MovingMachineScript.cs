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

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        anim.SetBool("Moving", true);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        anim.SetBool("Moving", false);
    //    }
    //}


    //--------------------


    public void StartMovement()
    {
        anim.SetBool("Moving", true);
    }
    public void StopMovement()
    {
        anim.SetBool("Moving", false);
    }
}
