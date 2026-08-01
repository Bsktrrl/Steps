using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BodyHeight : Singleton<Player_BodyHeight>
{
    [Header("Local PlayerBody height level")]
    [HideInInspector] public float height_Normal = -0.15f; //Where the player is on the block under
    [HideInInspector] public float height_CeilingGrab = 0.26f;

    //INFO: -0.8 is where the player is right under water surface
    float height_Stair = 0.08f;
    float height_Water = -0.5f;
    float height_SwampWater = -0.4f;
    float height_Mud = -0.6f;
    float height_Lava = -0.7f;

    float height_QuickSand_1 = -0.3f;
    float height_QuickSand_2 = -0.42f;
    float height_QuickSand_3 = -0.54f;
    float height_QuickSand_4 = -0.66f;
    float height_QuickSand_5 = -0.78f;

    float height_pipe = -0.25f;


    //--------------------


    private void Start()
    {
        height_Normal = -0.15f;
    }

    private void Update()
    {
        SetPlayerBodyHeight();
    }


    //--------------------


    public float SetPlayerBodyHeight()
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            return height_CeilingGrab;
        }

        /*
         * Keep the existing grapple behaviour while the player is moving.
         * The Movement fix makes the player root finish at the correct
         * landing-block height. When grappling ends, this method resumes
         * and applies Block_HeightOffset normally.
         */
        if (Movement.Instance.performGrapplingHooking)
        {
            return height_Normal;
        }

        if (Movement.Instance.blockStandingOn)
        {
            if (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() &&
                !Movement.Instance.isDashing &&
                !Movement.Instance.isJumping &&
                !Movement.Instance.isGrapplingHooking)
            {
                //Stair
                if (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair ||
                    Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                {
                    return ApplyPlayerBodyHeight(height_Stair);
                }

                //Water
                else if (Movement.Instance.blockStandingOn.GetComponent<Block_Water>())
                {
                    return ApplyPlayerBodyHeight(height_Water);
                }

                //Swamp Water
                else if (Movement.Instance.blockStandingOn.GetComponent<Block_SwampWater>())
                {
                    return ApplyPlayerBodyHeight(height_SwampWater);
                }

                //Mud
                else if (Movement.Instance.blockStandingOn.GetComponent<Block_Mud>())
                {
                    return ApplyPlayerBodyHeight(height_Mud);
                }

                //Quicksand
                else if (Movement.Instance.blockStandingOn.GetComponent<Block_Quicksand>())
                {
                    if (Player_Quicksand.Instance.quicksandCounter == 0)
                    {
                        return ApplyPlayerBodyHeight(height_Normal);
                    }
                    else if (Player_Quicksand.Instance.quicksandCounter == 1)
                    {
                        return ApplyPlayerBodyHeight(height_QuickSand_1);
                    }
                    else if (Player_Quicksand.Instance.quicksandCounter == 2)
                    {
                        return ApplyPlayerBodyHeight(height_QuickSand_2);
                    }
                    else if (Player_Quicksand.Instance.quicksandCounter == 3)
                    {
                        return ApplyPlayerBodyHeight(height_QuickSand_3);
                    }
                    else if (Player_Quicksand.Instance.quicksandCounter == 4)
                    {
                        return ApplyPlayerBodyHeight(height_QuickSand_4);
                    }
                    else if (Player_Quicksand.Instance.quicksandCounter == 5)
                    {
                        return ApplyPlayerBodyHeight(height_QuickSand_5);
                    }
                    else
                    {
                        return ApplyPlayerBodyHeight(height_Normal);
                    }
                }

                //Lava
                else if (Movement.Instance.blockStandingOn.GetComponent<Block_Lava>())
                {
                    return ApplyPlayerBodyHeight(height_Lava);
                }

                //Pipe
                else if (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement ==
                         BlockElement.Pipe)
                {
                    return ApplyPlayerBodyHeight(height_pipe);
                }

                //Other
                else
                {
                    if (Player_CeilingGrab.Instance.isCeilingGrabbing)
                    {
                        return ApplyPlayerBodyHeight(
                            height_CeilingGrab,
                            includeBlockHeightOffset: false);
                    }

                    return ApplyPlayerBodyHeight(height_Normal);
                }
            }

            //Other
            else
            {
                return ApplyPlayerBodyHeight(height_Normal);
            }
        }

        //SwiftSwim
        //else if (Movement.Instance.isSwiftSwimming_Up ||
        //         Movement.Instance.isSwiftSwimming_Down)
        //{
        //    PlayerManager.Instance.playerBody.transform
        //        .SetLocalPositionAndRotation(
        //            ReturnPosition(height_Water),
        //            ReturnRotation());
        //
        //    return height_Water;
        //}

        //Other
        else
        {
            return ApplyPlayerBodyHeight(height_Normal);
        }
    }


    //--------------------


    Vector3 ReturnPosition(float value_Y)
    {
        if (CameraController.Instance.cameraState ==
            CameraState.GameplayCam)
        {
            return new Vector3(
                PlayerManager.Instance.playerBody.transform.localPosition.x,
                value_Y,
                PlayerManager.Instance.playerBody.transform.localPosition.z);
        }
        else if (CameraController.Instance.cameraState ==
                 CameraState.CeilingCam)
        {
            return new Vector3(
                PlayerManager.Instance.playerBody.transform.localPosition.x,
                -value_Y,
                PlayerManager.Instance.playerBody.transform.localPosition.z);
        }
        else
        {
            return new Vector3(
                PlayerManager.Instance.playerBody.transform.localPosition.x,
                value_Y,
                PlayerManager.Instance.playerBody.transform.localPosition.z);
        }
    }

    Quaternion ReturnRotation()
    {
        return Quaternion.Euler(
            PlayerManager.Instance.playerBody.transform
                .localRotation.eulerAngles.x,

            PlayerManager.Instance.playerBody.transform
                .localRotation.eulerAngles.y,

            Player_CeilingGrab.Instance
                .playerCeilingRotationValue);
    }


    //--------------------
    // Block Height Offset
    //--------------------


    private float GetBlockHeightOffset()
    {
        GameObject standingBlock =
            Movement.Instance.blockStandingOn;

        if (standingBlock == null)
        {
            return 0f;
        }

        if (standingBlock.TryGetComponent(
                out Block_HeightOffset heightOffsetComponent))
        {
            return heightOffsetComponent.height_Offset;
        }

        return 0f;
    }

    private float ApplyPlayerBodyHeight(
        float baseHeight,
        bool includeBlockHeightOffset = true)
    {
        float blockHeightOffset =
            includeBlockHeightOffset
                ? GetBlockHeightOffset()
                : 0f;

        /*
         * Calculate from the unchanged base value every frame.
         * Never add the offset back into height_Normal, height_Water,
         * or any of the other stored height values.
         */
        float finalHeight =
            baseHeight + blockHeightOffset;

        PlayerManager.Instance.playerBody.transform
            .SetLocalPositionAndRotation(
                ReturnPosition(finalHeight),
                ReturnRotation());

        HoleShaderOnOffScript.Instance
            .SetHullShaderPosY(finalHeight);

        HoleShaderOnOffScript.Instance.PlayerBody_offset =
            finalHeight;

        return finalHeight;
    }
}