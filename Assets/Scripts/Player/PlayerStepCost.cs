using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStepCost : Singleton<PlayerStepCost>
{
    public static event Action updateStepCounter;

    private void Start()
    {
        PlayerMovement.playerStopped += DecreaseStepCounter;
        PlayerDetector.finishedRaycast += UpdatePlayerStepCountDisplay;
    }

    void DecreaseStepCounter()
    {
        if(PlayerDetectorController.Instance.platform_Center)
        {
            if (PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>())
            {
                PlayerStats.Instance.stats.steps_Current -= PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>().stepsCost;

                //if (PlayerStats.Instance.stats.steps_Current < 0)
                //{
                //    RefillSteps();
                //}

                updateStepCounter?.Invoke();
            }
        }
    }

    public void RefillSteps()
    {
        transform.position = PlayerStats.Instance.startPos;

        StartCoroutine(WaitAfterReset(0.5f));
    }

    IEnumerator WaitAfterReset(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PlayerStats.Instance.stats.steps_Current = PlayerStats.Instance.stats.steps_Max;
        updateStepCounter?.Invoke();

        yield return new WaitForSeconds(waitTime);

        gameObject.GetComponent<PlayerMovement>().movementStates = MovementStates.Still;
    }


    void UpdatePlayerStepCountDisplay()
    {
        PlayerDetectorController.Instance.player_StepCounterDisplayForward.SetActive(false);
        PlayerDetectorController.Instance.player_StepCounterDisplayBackward.SetActive(false);
        PlayerDetectorController.Instance.player_StepCounterDisplayRight.SetActive(false);
        PlayerDetectorController.Instance.player_StepCounterDisplayLeft.SetActive(false);

        if (PlayerMovement.Instance.movementStates == MovementStates.Moving) { return; }

        if (PlayerDetectorController.Instance.platform_Vertical_Forward && Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Forward))
        {
            if (PlayerDetectorController.Instance.platform_Horizontal_Forward)
            {
                if (PlayerDetectorController.Instance.platform_Horizontal_Forward.GetComponent<Platform>())
                {
                    if (PlayerDetectorController.Instance.platform_Horizontal_Forward.GetComponent<Platform>().platformType != PlatformType.Wall)
                    {
                        //print("-------------------- 1.Forward");
                        RotateStepCountDisplay(PlayerDetectorController.Instance.player_StepCounterDisplayForward, PlayerDetectorController.Instance.platform_Vertical_Forward);
                    }
                }
            }
            else
            {
                //print("-------------------- 2.Forward");
                RotateStepCountDisplay(PlayerDetectorController.Instance.player_StepCounterDisplayForward, PlayerDetectorController.Instance.platform_Vertical_Forward);
            }
        }

        if (PlayerDetectorController.Instance.platform_Vertical_Backward && Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Backward))
        {
            if (PlayerDetectorController.Instance.platform_Horizontal_Backward)
            {
                if (PlayerDetectorController.Instance.platform_Horizontal_Backward.GetComponent<Platform>())
                {
                    if (PlayerDetectorController.Instance.platform_Horizontal_Backward.GetComponent<Platform>().platformType != PlatformType.Wall)
                    {
                        //print("-------------------- 1.Backard");
                        RotateStepCountDisplay(PlayerDetectorController.Instance.player_StepCounterDisplayBackward, PlayerDetectorController.Instance.platform_Vertical_Backward);
                    }
                }
            }
            else
            {
                //print("-------------------- 2.Backard");
                RotateStepCountDisplay(PlayerDetectorController.Instance.player_StepCounterDisplayBackward, PlayerDetectorController.Instance.platform_Vertical_Backward);
            }
        }

        if (PlayerDetectorController.Instance.platform_Vertical_Right && Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Right))
        {
            if (PlayerDetectorController.Instance.platform_Horizontal_Right)
            {
                if (PlayerDetectorController.Instance.platform_Horizontal_Right.GetComponent<Platform>())
                {
                    if (PlayerDetectorController.Instance.platform_Horizontal_Right.GetComponent<Platform>().platformType != PlatformType.Wall)
                    {
                        //print("-------------------- 1.Right");
                        RotateStepCountDisplay(PlayerDetectorController.Instance.player_StepCounterDisplayRight, PlayerDetectorController.Instance.platform_Vertical_Right);
                    }
                }
            }
            else
            {
                //print("-------------------- 2.Right");
                RotateStepCountDisplay(PlayerDetectorController.Instance.player_StepCounterDisplayRight, PlayerDetectorController.Instance.platform_Vertical_Right);
            }
        }

        if (PlayerDetectorController.Instance.platform_Vertical_Left && Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Left))
        {
            if (PlayerDetectorController.Instance.platform_Horizontal_Left)
            {
                if (PlayerDetectorController.Instance.platform_Horizontal_Left.GetComponent<Platform>())
                {
                    if (PlayerDetectorController.Instance.platform_Horizontal_Left.GetComponent<Platform>().platformType != PlatformType.Wall)
                    {
                        //print("-------------------- 1.Left");
                        RotateStepCountDisplay(PlayerDetectorController.Instance.player_StepCounterDisplayLeft, PlayerDetectorController.Instance.platform_Vertical_Left);
                    }
                }
            }
            else
            {
                //print("-------------------- 2.Left");
                RotateStepCountDisplay(PlayerDetectorController.Instance.player_StepCounterDisplayLeft, PlayerDetectorController.Instance.platform_Vertical_Left);
            }
        }
    }

    void RotateStepCountDisplay(GameObject stepCounterDisplayLeft, GameObject platform_Vertical_Direction)
    {
        //Change StepCost Text
        stepCounterDisplayLeft.GetComponent<TextMeshProUGUI>().text = platform_Vertical_Direction.GetComponent<Platform>().stepsCost.ToString();

        //Change StepCost Gameobject Rotation
        
        if (platform_Vertical_Direction.transform.rotation.x == -26.5f)
        {
            //print("2. localRotation.x == -26.5f | Name: " + platform_Vertical_Direction.name);
            stepCounterDisplayLeft.GetComponent<RectTransform>().localRotation = Quaternion.Euler(-26.5f, 0, 0);
        }
        else if (platform_Vertical_Direction.transform.rotation.x == 26.5f)
        {
            //print("3. localRotation.x == 26.5f | Name: " + platform_Vertical_Direction.name);
            stepCounterDisplayLeft.GetComponent<RectTransform>().localRotation = Quaternion.Euler(26.5f, 0, 0);
        }

        else if(platform_Vertical_Direction.transform.rotation.x == 0)
        {
            //print("1. localRotation.x == 0 | Name: " + platform_Vertical_Direction.name);
            //stepCounterDisplayLeft.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            stepCounterDisplayLeft.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(stepCounterDisplayLeft.GetComponent<RectTransform>().localPosition.x, stepCounterDisplayLeft.GetComponent<RectTransform>().localPosition.y, 0), Quaternion.Euler(0, 0, 0));
        }
        else if(platform_Vertical_Direction.transform.rotation.x < 0)
        {
            //print("4. localRotation.x < 0 | Name: " + platform_Vertical_Direction.name);
            //stepCounterDisplayLeft.GetComponent<RectTransform>().localRotation = Quaternion.Euler(-26.5f, 0, 0);
            stepCounterDisplayLeft.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(stepCounterDisplayLeft.GetComponent<RectTransform>().localPosition.x, stepCounterDisplayLeft.GetComponent<RectTransform>().localPosition.y, -0.2f), Quaternion.Euler(-26.5f, 0, 0));
        }
        else if(platform_Vertical_Direction.transform.rotation.x > 0)
        {
            //print("5. localRotation.x >= 0 | Name: " + platform_Vertical_Direction.name);
            //stepCounterDisplayLeft.GetComponent<RectTransform>().localRotation = Quaternion.Euler(26.5f, 0, 0);
            stepCounterDisplayLeft.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(stepCounterDisplayLeft.GetComponent<RectTransform>().localPosition.x, stepCounterDisplayLeft.GetComponent<RectTransform>().localPosition.y, 0.2f), Quaternion.Euler(26.5f, 0, 0));
        }

        stepCounterDisplayLeft.SetActive(true);
    }
}
