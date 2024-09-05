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
        stepCost_Text.text = stepsCost.ToString();
    }


    //--------------------



}
