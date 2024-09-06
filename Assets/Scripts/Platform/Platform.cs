using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Platform : MonoBehaviour
{
    [Header("Platform Definitions")]
    public PlatformType platformType;
    public Requirement requirement;

    [Header("Platform Settings")]
    public float speed;
    public int stepsCost;

    [Header("Platform Components")]
    public Image image_Darkener;
    public TextMeshProUGUI stepCost_Text;


    //--------------------


    private void Start()
    {
        PlayerDetector.finishedRaycast += UpdatePlatformShadow;
        PlayerDetector.newPlatformDetected += UpdatePlatformStepCount;
        //PlayerMovement.playerStopped += UpdatePlatformStepCount;

        stepCost_Text.text = stepsCost.ToString();
        stepCost_Text.fontSize = 0.3f;

        UpdatePlatformShadow();
    }


    //--------------------


    void UpdatePlatformShadow()
    {
        GameObject standingOnPrevious_Check = PlayerDetectorController.Instance.platform_Center;

        //Get the previous Platform standing on
        if (standingOnPrevious_Check
            && standingOnPrevious_Check != PlayerDetectorController.Instance.platform_Center
            && PlayerDetectorController.Instance.platform_Center_Previous != standingOnPrevious_Check)
        {
            PlayerDetectorController.Instance.platform_Center_Previous = standingOnPrevious_Check;
        }

        //Change Platform Standing Shadow
        if (PlayerDetectorController.Instance.platform_Center_Previous != PlayerDetectorController.Instance.platform_Center)
        {
            if (PlayerDetectorController.Instance.platform_Center_Previous)
            {
                if (PlayerDetectorController.Instance.platform_Center_Previous.GetComponent<Platform>())
                {
                    if (PlayerDetectorController.Instance.platform_Center_Previous.GetComponent<Platform>().image_Darkener.gameObject.activeInHierarchy)
                    {
                        PlayerDetectorController.Instance.platform_Center_Previous.GetComponent<Platform>().image_Darkener.gameObject.SetActive(false);
                    }
                }
            }

            if (PlayerDetectorController.Instance.platform_Center)
            {
                if (PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>())
                {
                    if (!PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>().image_Darkener.gameObject.activeInHierarchy)
                    {
                        PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>().image_Darkener.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    void UpdatePlatformStepCount()
    {
        stepCost_Text.gameObject.SetActive(false);

        if (PlayerDetectorController.Instance.platform_Vertical_Forward == gameObject && Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Forward))
        {
            if (PlayerDetectorController.Instance.platform_Horizontal_Forward)
            {
                if (PlayerDetectorController.Instance.platform_Horizontal_Forward.GetComponent<Platform>())
                {
                    if (PlayerDetectorController.Instance.platform_Horizontal_Forward.GetComponent<Platform>().platformType != PlatformType.Wall)
                    {
                        stepCost_Text.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                stepCost_Text.gameObject.SetActive(true);
            }
        }
        else if (PlayerDetectorController.Instance.platform_Vertical_Backward == gameObject && Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Backward))
        {
            if (PlayerDetectorController.Instance.platform_Horizontal_Backward)
            {
                if (PlayerDetectorController.Instance.platform_Horizontal_Backward.GetComponent<Platform>())
                {
                    if (PlayerDetectorController.Instance.platform_Horizontal_Backward.GetComponent<Platform>().platformType != PlatformType.Wall)
                    {
                        stepCost_Text.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                stepCost_Text.gameObject.SetActive(true);
            }
        }
        else if (PlayerDetectorController.Instance.platform_Vertical_Right == gameObject && Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Right))
        {
            if (PlayerDetectorController.Instance.platform_Horizontal_Right)
            {
                if (PlayerDetectorController.Instance.platform_Horizontal_Right.GetComponent<Platform>())
                {
                    if (PlayerDetectorController.Instance.platform_Horizontal_Right.GetComponent<Platform>().platformType != PlatformType.Wall)
                    {
                        stepCost_Text.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                stepCost_Text.gameObject.SetActive(true);
            }
        }
        else if (PlayerDetectorController.Instance.platform_Vertical_Left == gameObject && Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Left))
        {
            if (PlayerDetectorController.Instance.platform_Horizontal_Left)
            {
                if (PlayerDetectorController.Instance.platform_Horizontal_Left.GetComponent<Platform>())
                {
                    if (PlayerDetectorController.Instance.platform_Horizontal_Left.GetComponent<Platform>().platformType != PlatformType.Wall)
                    {
                        stepCost_Text.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                stepCost_Text.gameObject.SetActive(true);
            }
        }

        //if (Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Forward))
        //{
            
        //}
        //if (Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Backward))
        //{
            
        //}
        //if (Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Right))
        //{
            
        //}
        //if (Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Left))
        //{
            
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

    Wall,

    SavePoint,
    RefillSteps,

    Teleporter,
}