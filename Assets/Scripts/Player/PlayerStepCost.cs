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
                        PlayerDetectorController.Instance.player_StepCounterDisplayForward.GetComponent<TextMeshProUGUI>().text = PlayerDetectorController.Instance.platform_Vertical_Forward.GetComponent<Platform>().stepsCost.ToString();
                        PlayerDetectorController.Instance.player_StepCounterDisplayForward.SetActive(true);
                    }
                }
            }
            else
            {
                PlayerDetectorController.Instance.player_StepCounterDisplayForward.GetComponent<TextMeshProUGUI>().text = PlayerDetectorController.Instance.platform_Vertical_Forward.GetComponent<Platform>().stepsCost.ToString();
                PlayerDetectorController.Instance.player_StepCounterDisplayForward.SetActive(true);
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
                        PlayerDetectorController.Instance.player_StepCounterDisplayBackward.GetComponent<TextMeshProUGUI>().text = PlayerDetectorController.Instance.platform_Vertical_Backward.GetComponent<Platform>().stepsCost.ToString();
                        PlayerDetectorController.Instance.player_StepCounterDisplayBackward.SetActive(true);
                    }
                }
            }
            else
            {
                PlayerDetectorController.Instance.player_StepCounterDisplayBackward.GetComponent<TextMeshProUGUI>().text = PlayerDetectorController.Instance.platform_Vertical_Backward.GetComponent<Platform>().stepsCost.ToString();
                PlayerDetectorController.Instance.player_StepCounterDisplayBackward.SetActive(true);
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
                        PlayerDetectorController.Instance.player_StepCounterDisplayRight.GetComponent<TextMeshProUGUI>().text = PlayerDetectorController.Instance.platform_Vertical_Right.GetComponent<Platform>().stepsCost.ToString();
                        PlayerDetectorController.Instance.player_StepCounterDisplayRight.SetActive(true);
                    }
                }
            }
            else
            {
                PlayerDetectorController.Instance.player_StepCounterDisplayRight.GetComponent<TextMeshProUGUI>().text = PlayerDetectorController.Instance.platform_Vertical_Right.GetComponent<Platform>().stepsCost.ToString();
                PlayerDetectorController.Instance.player_StepCounterDisplayRight.SetActive(true);
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
                        PlayerDetectorController.Instance.player_StepCounterDisplayLeft.GetComponent<TextMeshProUGUI>().text = PlayerDetectorController.Instance.platform_Vertical_Left.GetComponent<Platform>().stepsCost.ToString();
                        PlayerDetectorController.Instance.player_StepCounterDisplayLeft.SetActive(true);
                    }
                }
            }
            else
            {
                PlayerDetectorController.Instance.player_StepCounterDisplayLeft.GetComponent<TextMeshProUGUI>().text = PlayerDetectorController.Instance.platform_Vertical_Left.GetComponent<Platform>().stepsCost.ToString();
                PlayerDetectorController.Instance.player_StepCounterDisplayLeft.SetActive(true);
            }
        }
    }
}
