using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBTutorial_Trigger : MonoBehaviour
{
    [SerializeField] int tutorialSegementIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (DataManager.Instance.oneTimeRunData_Store.tutorialSegmenet[tutorialSegementIndex] == true) return;

        if (other.CompareTag("Player") && !Player_CeilingGrab.Instance.isCeilingGrabbing && !Player_CeilingGrab.Instance.isCeilingRotation)
        {
            //Perform Tutorial Segment

            HUBTutorial.Instance.StartTutorial(tutorialSegementIndex);

            print("1000. PLayer has entered the tutorial trigger");
        }
        else
        {
            
        }
    }
}
