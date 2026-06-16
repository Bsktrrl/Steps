using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBTutorial_Trigger : MonoBehaviour
{
    [SerializeField] TutorialParts tutorialSegementIndex;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < DataManager.Instance.oneTimeRunData_Store.tutorialSegmenets.Count; i++)
        {
            if (DataManager.Instance.oneTimeRunData_Store.tutorialSegmenets[i].tutorialParts == tutorialSegementIndex && DataManager.Instance.oneTimeRunData_Store.tutorialSegmenets[i].isGoneThrough)
            {
                return;
            }
        }

        if (other.CompareTag("Player") && !Player_CeilingGrab.Instance.isCeilingGrabbing && !Player_CeilingGrab.Instance.isCeilingRotation)
        {
            HUBTutorial.Instance.StartTutorial(tutorialSegementIndex);
        }
    }
}

public enum TutorialParts
{
    None,

    Movement,
    Respawn,
    CameraRotation,
    Footprint,
    Skin,
    Essence,
    FreeCam,
    Glueplant,
}
