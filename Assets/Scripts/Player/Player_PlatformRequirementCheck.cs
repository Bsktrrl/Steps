using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_PlatformRequirementCheck : Singleton<Player_PlatformRequirementCheck>
{
    public bool CheckPlatformRequirement(MovementDirection direction)
    {
        switch (direction)
        {
            case MovementDirection.None:
                break;

            case MovementDirection.Forward:
                return CalculateRequirement(PlayerDetectorController.Instance.platform_Vertical_Forward);
            case MovementDirection.Backward:
                return CalculateRequirement(PlayerDetectorController.Instance.platform_Vertical_Backward);
            case MovementDirection.Left:
                return CalculateRequirement(PlayerDetectorController.Instance.platform_Vertical_Left);
            case MovementDirection.Right:
                return CalculateRequirement(PlayerDetectorController.Instance.platform_Vertical_Right);

            default:
                break;
        }

        return false;
    }

    bool CalculateRequirement(GameObject platform)
    {
        if (platform)
        {
            #region SwimSuit
            if (platform.GetComponent<Platform>().requirement == Requirement.SwimSuit
                && PlayerStats.Instance.keyItems.SwimSuit)
            {
                return true;
            }
            else if (platform.GetComponent<Platform>().requirement == Requirement.SwimSuit
                && !PlayerStats.Instance.keyItems.SwimSuit)
            {
                return false;
            }
            #endregion

            #region HikerGear
            if (platform.GetComponent<Platform>().requirement == Requirement.HikerGear
                && PlayerStats.Instance.keyItems.HikerGear)
            {
                return true;
            }
            else if (platform.GetComponent<Platform>().requirement == Requirement.HikerGear
                && !PlayerStats.Instance.keyItems.HikerGear)
            {
                return false;
            }
            #endregion

            #region LavaSuit
            if (platform.GetComponent<Platform>().requirement == Requirement.LavaSuit
                && PlayerStats.Instance.keyItems.LavaSuit)
            {
                return true;
            }
            else if (platform.GetComponent<Platform>().requirement == Requirement.LavaSuit
                && !PlayerStats.Instance.keyItems.LavaSuit)
            {
                return false;
            }
            #endregion

            if (platform.GetComponent<Platform>().requirement == Requirement.None)
            {
                return true;
            }
        }

        return false;
    }
}

public enum Requirement
{
    None,

    SwimSuit,
    HikerGear,
    LavaSuit,
}