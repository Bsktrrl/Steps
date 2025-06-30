using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimations : MonoBehaviour
{
    Animator anim;

    bool blink;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Play different idle animations with number keys (Also plays a blink animation as a transition)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetTrigger("Angry");
            anim.SetTrigger("Blink");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("Annoyed");
            anim.SetTrigger("Blink");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetTrigger("Confident");
            anim.SetTrigger("Blink");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            anim.SetTrigger("Confused");
            anim.SetTrigger("Blink");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            anim.SetTrigger("Despair");
            anim.SetTrigger("Blink");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            anim.SetTrigger("Disgusted");
            anim.SetTrigger("Blink");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            anim.SetTrigger("Embarrassed");
            anim.SetTrigger("Blink");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            anim.SetTrigger("Friendly");
            anim.SetTrigger("Blink");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            anim.SetTrigger("Happy");
            anim.SetTrigger("Blink");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            anim.SetTrigger("Laughing");
            anim.SetTrigger("Blink");
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            anim.SetTrigger("Sad");
            anim.SetTrigger("Blink");
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            anim.SetTrigger("Sceptical");
            anim.SetTrigger("Blink");
        }

        //Toggle talking animation by pressing T
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (anim.GetBool("Talking"))
            {
                anim.SetBool("Talking", false);
            }
            else
            {
                anim.SetBool("Talking", true);
            }
        }

        //Start Blink coroutine if it's not active
        if (!blink)
        {
            StartCoroutine("Blink");
        }
    }

    //Blink every 0-10 seconds
    IEnumerator Blink()
    {
        blink = true;

        yield return new WaitForSeconds(Random.Range(0f, 10f));

        anim.SetTrigger("Blink");
        blink = false;
    }
}
