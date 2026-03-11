using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMachineScript : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetBool("Moving", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetBool("Moving", false);
        }
    }
}
