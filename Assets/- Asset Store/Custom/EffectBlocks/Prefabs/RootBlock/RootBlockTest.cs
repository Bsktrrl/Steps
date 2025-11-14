using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBlockTest : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        //Press '1' to play the activate animation of the Root Block and the Root Line
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetTrigger("Activate");
        }

        //Press '2' to play the deactivate animation of the Root Line
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("Deactivate");
        }
    }
}
