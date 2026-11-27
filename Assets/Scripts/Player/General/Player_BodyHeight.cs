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
        Movement movement = Movement.Instance;
        if (movement.performGrapplingHooking)
        {
            return height_Normal;
        }

        // These moves apply the height as part of their own motion.
        if (movement.IsClimbingLadder || movement.GetMovementState() == MovementStates.Falling)
            return PlayerManager.Instance.playerBody.transform.localPosition.y;

        return ApplyPlayerBodyHeight(GetHeightOnBlock(movement.blockStandingOn, movement));
    }


    //--------------------


    internal float GetHeightOnBlock(GameObject block, Movement movement)
    {
        float offset = block && block.TryGetComponent(out Block_HeightOffset heightOffset)
            ? heightOffset.height_Offset : 0f;
        return GetBaseHeightOnBlock(block, movement) + offset;
    }

    private float GetBaseHeightOnBlock(GameObject block, Movement movement)
    {
        if (!block || !block.TryGetComponent(out BlockInfo info) ||
            movement.isDashing || movement.isJumping || movement.isGrapplingHooking)
            return height_Normal;

        if (info.blockType == BlockType.Stair || info.blockType == BlockType.Slope)
            return height_Stair;
        if (block.GetComponent<Block_Water>())
            return height_Water;
        if (block.GetComponent<Block_SwampWater>())
            return height_SwampWater;
        if (block.GetComponent<Block_Mud>())
            return height_Mud;
        if (block.TryGetComponent(out Block_Quicksand quicksand))
        {
            return Player_Quicksand.Instance.quicksandCounter switch
            {
                0 when block == movement.blockStandingOn && quicksand.isActiveAndEnabled => height_QuickSand_1,
                1 => height_QuickSand_1,
                2 => height_QuickSand_2,
                3 => height_QuickSand_3,
                4 => height_QuickSand_4,
                >= 5 => height_QuickSand_5,
                _ => height_Normal
            };
        }

        if (block.GetComponent<Block_Lava>())
            return height_Lava;

        return info.blockElement == BlockElement.Pipe ? height_pipe : height_Normal;
    }

    internal float GetLocalYForHeight(float height)
    {
        return CameraController.Instance.cameraState == CameraState.CeilingCam ? -height : height;
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


    internal float ApplyPlayerBodyHeight(float height, float progress = 1f)
    {
        Transform body = PlayerManager.Instance.playerBody.transform;
        Vector3 position = body.localPosition;
        float sign = GetLocalYForHeight(1f);
        if (progress < 1f)
            height = Mathf.Lerp(position.y * sign, height, progress);

        position.y = height * sign;
        body.SetLocalPositionAndRotation(position, ReturnRotation());
        HoleShaderOnOffScript.Instance.SetHullShaderPosY(height);
        HoleShaderOnOffScript.Instance.PlayerBody_offset = height;
        return height;
    }
}
