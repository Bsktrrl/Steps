using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Platform : MonoBehaviour
{
    public PlatformTypes platformType;
    public float speed;
    public int stepsCost;

    public Image image_Darkener;
    public TextMeshProUGUI stepCost_Text;


    //--------------------


    private void Start()
    {
        MainManager.flippersEquipped += Flippers_ChangeStepCost;
        MainManager.hikerGearEquipped += HikerGear_ChangeStepCost;
        PlayerMovement.finishMovement += UpdatePlatformVisibility;

        stepCost_Text.text = stepsCost.ToString();

        stepCost_Text.fontSize = 0.3f;

        UpdatePlatformVisibility();
    }


    //--------------------


    void UpdatePlatformVisibility()
    {
        if (MainManager.Instance.playerStats.platformObject_StandingOn_Current)
        {
            print("1. UpdatePlatformVisibility");

            if (transform.position.y >= MainManager.Instance.playerStats.platformObject_StandingOn_Current.transform.position.y - 0.5f
                && transform.position.y <= MainManager.Instance.playerStats.platformObject_StandingOn_Current.transform.position.y + 0.5f)
            {
                GetComponent<MeshRenderer>().enabled = true;
                GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }


    //--------------------


    void Flippers_ChangeStepCost()
    {
        if (platformType == PlatformTypes.Water)
        {
            stepsCost -= 1;
            speed += 2;
            stepCost_Text.text = stepsCost.ToString();
        }
    }
    void HikerGear_ChangeStepCost()
    {
        if (platformType == PlatformTypes.Hill || platformType == PlatformTypes.Mountain)
        {
            stepsCost -= 1;
            speed += 2;
            stepCost_Text.text = stepsCost.ToString();
        }
    }
}
