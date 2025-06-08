using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BodyHeight : Singleton<Player_BodyHeight>
{
    [Header("Local PlayerBody height level")]
    [HideInInspector] public float height_Normal = -0.2f;

    float height_Stair = -0.2f;
    float height_Water = -0.8f;
    float height_SwampWater = -0.6f;
    float height_Mud = -1f;
    float height_Lava = -0.9f;

    float height_QuickSand_1 = -0.6f;
    float height_QuickSand_2 = -0.8f;
    float height_QuickSand_3 = -1.0f;
    float height_QuickSand_4 = -1.2f;
    float height_QuickSand_5 = -1.4f;


    //--------------------


    private void Update()
    {
        SetPlayerBodyHeight();
    }


    //--------------------


    void SetPlayerBodyHeight()
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (Movement.Instance.blockStandingOn)
        {
            //Stair
            if (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair || Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Stair), ReturnRotation());

            //Water
            else if (Movement.Instance.blockStandingOn.GetComponent<Block_Water>())
                PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Water), ReturnRotation());

            //Swamp Water
            else if (Movement.Instance.blockStandingOn.GetComponent<Block_SwampWater>())
                PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_SwampWater), ReturnRotation());

            //Mud
            else if (Movement.Instance.blockStandingOn.GetComponent<Block_Mud>())
                PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Mud), ReturnRotation());

            //Quicksand
            else if (Movement.Instance.blockStandingOn.GetComponent<Block_Quicksand>())
            {
                if (Player_Quicksand.Instance.quicksandCounter == 1)
                    PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_QuickSand_1), ReturnRotation());
                else if (Player_Quicksand.Instance.quicksandCounter == 2)
                    PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_QuickSand_2), ReturnRotation());
                else if (Player_Quicksand.Instance.quicksandCounter == 3)
                    PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_QuickSand_3), ReturnRotation());
                else if (Player_Quicksand.Instance.quicksandCounter == 4)
                    PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_QuickSand_4), ReturnRotation());
                else if (Player_Quicksand.Instance.quicksandCounter == 5)
                    PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_QuickSand_5), ReturnRotation());
                else
                    PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Normal), ReturnRotation());
            }

            //Lava
            else if (Movement.Instance.blockStandingOn.GetComponent<Block_Lava>())
                PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Lava), ReturnRotation());

            //Other
            else
                PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Normal), ReturnRotation());
        }

        //SwiftSwim
        else if (Player_SwiftSwim.Instance.isSwiftSwimming_Up || Player_SwiftSwim.Instance.isSwiftSwimming_Down)
            PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Water), ReturnRotation());

        //Other
        else
            PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(ReturnPosition(height_Normal), ReturnRotation());
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
