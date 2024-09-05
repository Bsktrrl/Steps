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
        stepCost_Text.text = stepsCost.ToString();

        stepCost_Text.fontSize = 0.3f;
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
