using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSwitchable : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetTrigger("Switch");
        }
    }
}
