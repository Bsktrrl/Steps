using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BodyHeight : Singleton<Player_BodyHeight>
{
    [Header("Local PlayerBody height level")]
    [HideInInspector] public float height_Normal = -0.2f;
    [HideInInspector] public float height_CeilingGrab = 0f;

    float height_Stair = -0.2f;
    float height_Water = -0.8f;
    float height_SwampWater = -0.6f;
    float height_Mud = -1f;
    float height_Lava = -0.9f;

    float height_QuickSand_1 = -0.7f;
    float height_QuickSand_2 = -0.9f;
    float height_QuickSand_3 = -1.1f;
    float height_QuickSand_4 = -1.3f;
    float height_QuickSand_5 = -1.5f;


    //--------------------


    private void Update()
    {
        SetPlayerBodyHeight();
    }


    //--------------------


    public float SetPlayerBodyHeight()
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return 0; }
        if (Movement.Instance.performGrapplingHooking) { return height_Normal; }

        if (Movement.Instance.blockStandingOn)
        {
            if (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && !Movement.Instance.isDashing && !Movement.Instance.isJumping && !Movement.Instance.isGrapplingHooking)
            {
                //Stair
                if (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair || Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                {
                    PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Stair), ReturnRotation());
                    return height_Stair;
                }

                //Water
                else if (Movement.Instance.blockStandingOn.GetComponent<Block_Water>())
                {
                    PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Water), ReturnRotation());
                    return height_Water;
                }

                //Swamp Water
                else if (Movement.Instance.blockStandingOn.GetComponent<Block_SwampWater>())
                {
                    PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_SwampWater), ReturnRotation());
                    return height_SwampWater;
                }

                //Mud
                else if (Movement.Instance.blockStandingOn.GetComponent<Block_Mud>())
                {
                    PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Mud), ReturnRotation());
                    return height_Mud;
                }

                //Quicksand
                else if (Movement.Instance.blockStandingOn.GetComponent<Block_Quicksand>())
                {
                    if (Player_Quicksand.Instance.quicksandCounter == 0)
                    {
                        PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Normal), ReturnRotation());
                        return height_Normal;
                    }
                    else if (Player_Quicksand.Instance.quicksandCounter == 1)
                    {
                        PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_QuickSand_1), ReturnRotation());
                        return height_QuickSand_1;
                    }
                    else if (Player_Quicksand.Instance.quicksandCounter == 2)
                    {
                        PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_QuickSand_2), ReturnRotation());
                        return height_QuickSand_2;
                    }
                    else if (Player_Quicksand.Instance.quicksandCounter == 3)
                    {
                        PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_QuickSand_3), ReturnRotation());
                        return height_QuickSand_3;
                    }
                    else if (Player_Quicksand.Instance.quicksandCounter == 4)
                    {
                        PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_QuickSand_4), ReturnRotation());
                        return height_QuickSand_4;
                    }
                    else if (Player_Quicksand.Instance.quicksandCounter == 5)
                    {
                        PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_QuickSand_5), ReturnRotation());
                        return height_QuickSand_5;
                    }
                    else
                    {
                        PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Normal), ReturnRotation());
                        return height_Normal;
                    }
                }

                //Lava
                else if (Movement.Instance.blockStandingOn.GetComponent<Block_Lava>())
                {
                    PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Lava), ReturnRotation());
                    return height_Lava;
                }

                //Other
                else
                {
                    if (Player_CeilingGrab.Instance.isCeilingGrabbing)
                        PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_CeilingGrab), ReturnRotation());
                    else
                        PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Normal), ReturnRotation());
                    return height_Normal;
                }
            }
            
            //Other
            else
            {
                PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Normal), ReturnRotation());
                return height_Normal;
            }

        }

        //SwiftSwim
        //else if (Movement.Instance.isSwiftSwimming_Up || Movement.Instance.isSwiftSwimming_Down)
        //{
        //    PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Water), ReturnRotation());
        //    return height_Water;
        //}

        //Other
        else
        {
            PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Normal), ReturnRotation());
            return height_Normal;
        }
    }

    Vector3 ReturnPosition(float value_Y)
    {
        if (CameraController.Instance.cameraState == CameraState.GameplayCam)
            return new Vector3(PlayerManager.Instance.playerBody.transform.localPosition.x, value_Y, PlayerManager.Instance.playerBody.transform.localPosition.z);
        else if (CameraController.Instance.cameraState == CameraState.CeilingCam)
            return new Vector3(PlayerManager.Instance.playerBody.transform.localPosition.x, -value_Y, PlayerManager.Instance.playerBody.transform.localPosition.z);
        else
            return new Vector3(PlayerManager.Instance.playerBody.transform.localPosition.x, value_Y, PlayerManager.Instance.playerBody.transform.localPosition.z);
    }

    Quaternion ReturnRotation()
    {
        return Quaternion.Euler(PlayerManager.Instance.playerBody.transform.localRotation.eulerAngles.x, PlayerManager.Instance.playerBody.transform.localRotation.eulerAngles.y, Player_CeilingGrab.Instance.playerCeilingRotationValue);
    }
}
