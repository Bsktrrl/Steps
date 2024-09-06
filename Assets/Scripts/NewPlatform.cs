using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewPlatform : MonoBehaviour
{
    public PlatformType platformType;
    public float speed;
    public int stepsCost;

    public Image image_Darkener;
    public TextMeshProUGUI stepCost_Text;
    private void Start()
    {
        NewDetector.finishedRaycast += UpdatePlatformShadow;
        NewDetector.newPlatformDetected += UpdatePlatformStepCount;

        stepCost_Text.text = stepsCost.ToString();
        stepCost_Text.fontSize = 0.3f;

        UpdatePlatformShadow();
    }

    void UpdatePlatformShadow()
    {
        GameObject standingOnPrevious_Check = PlayerController.Instance.Platform_Center;

        //Get the previous Platform standing on
        if (standingOnPrevious_Check
            && standingOnPrevious_Check != PlayerController.Instance.Platform_Center
            && PlayerController.Instance.Platform_Center_Previous != standingOnPrevious_Check)
        {
            PlayerController.Instance.Platform_Center_Previous = standingOnPrevious_Check;
        }

        //Change Platform Standing Shadow
        if (PlayerController.Instance.Platform_Center_Previous != PlayerController.Instance.Platform_Center)
        {
            if (PlayerController.Instance.Platform_Center_Previous)
            {
                if (PlayerController.Instance.Platform_Center_Previous.GetComponent<NewPlatform>())
                {
                    if (PlayerController.Instance.Platform_Center_Previous.GetComponent<NewPlatform>().image_Darkener.gameObject.activeInHierarchy)
                    {
                        PlayerController.Instance.Platform_Center_Previous.GetComponent<NewPlatform>().image_Darkener.gameObject.SetActive(false);
                    }
                }
            }

            if (PlayerController.Instance.Platform_Center)
            {
                if (PlayerController.Instance.Platform_Center.GetComponent<NewPlatform>())
                {
                    if (!PlayerController.Instance.Platform_Center.GetComponent<NewPlatform>().image_Darkener.gameObject.activeInHierarchy)
                    {
                        PlayerController.Instance.Platform_Center.GetComponent<NewPlatform>().image_Darkener.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    void UpdatePlatformStepCount()
    {
        stepCost_Text.gameObject.SetActive(false);

        if (PlayerController.Instance.platform_Vertical_Forward == gameObject)
        {
            if (PlayerController.Instance.platform_Horizontal_Forward)
            {
                if (PlayerController.Instance.platform_Horizontal_Forward.GetComponent<NewPlatform>().platformType != PlatformType.Wall)
                {
                    stepCost_Text.gameObject.SetActive(true);
                }
            }
            else
            {
                stepCost_Text.gameObject.SetActive(true);
            }
        }
        else if (PlayerController.Instance.platform_Vertical_Backward == gameObject)
        {
            if (PlayerController.Instance.platform_Horizontal_Backward)
            {
                if (PlayerController.Instance.platform_Horizontal_Backward.GetComponent<NewPlatform>().platformType != PlatformType.Wall)
                {
                    stepCost_Text.gameObject.SetActive(true);
                }
            }
            else
            {
                stepCost_Text.gameObject.SetActive(true);
            }
        }
        else if (PlayerController.Instance.platform_Vertical_Right == gameObject)
        {
            if (PlayerController.Instance.platform_Horizontal_Right)
            {
                if (PlayerController.Instance.platform_Horizontal_Right.GetComponent<NewPlatform>().platformType != PlatformType.Wall)
                {
                    stepCost_Text.gameObject.SetActive(true);
                }
            }
            else
            {
                stepCost_Text.gameObject.SetActive(true);
            }
        }
        else if (PlayerController.Instance.platform_Vertical_Left == gameObject)
        {
            if (PlayerController.Instance.platform_Horizontal_Left)
            {
                if (PlayerController.Instance.platform_Horizontal_Left.GetComponent<NewPlatform>().platformType != PlatformType.Wall)
                {
                    stepCost_Text.gameObject.SetActive(true);
                }
            }
            else
            {
                stepCost_Text.gameObject.SetActive(true);
            }
        }


        //if ((PlayerController.Instance.platform_Vertical_Forward == gameObject && PlayerController.Instance.platform_Horizontal_Forward.GetComponent<NewPlatform>().platformType != PlatformType.Wall)
        //    || PlayerController.Instance.platform_Vertical_Backward == gameObject && PlayerController.Instance.platform_Horizontal_Backward.GetComponent<NewPlatform>().platformType != PlatformType.Wall
        //    || PlayerController.Instance.platform_Vertical_Right == gameObject && PlayerController.Instance.platform_Horizontal_Right.GetComponent<NewPlatform>().platformType != PlatformType.Wall
        //    || PlayerController.Instance.platform_Vertical_Left == gameObject && PlayerController.Instance.platform_Horizontal_Left.GetComponent<NewPlatform>().platformType != PlatformType.Wall)
        //{
        //    stepCost_Text.gameObject.SetActive(true);
        //}
    }
}

public enum PlatformType
{
    None,

    Wood,
    Grass,
    Sand,
    Ice,
    Hill,
    Mountain,
    Water,
    Lava,

    Wall
}