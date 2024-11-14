using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BodyHeight : MonoBehaviour
{
    [Header("Local PlayerBody height level")]
    [SerializeField] float height_Normal = 0;

    [SerializeField] float height_Stair = 0.5f;
    [SerializeField] float height_Water = -0.8f;
    [SerializeField] float height_Lava = -0.9f;


    //--------------------


    private void Update()
    {
        SetPlayerBodyHeight();
    }


    //--------------------


    void SetPlayerBodyHeight()
    {
        if (MainManager.Instance.block_StandingOn_Current.block)
        {
            if (MainManager.Instance.block_StandingOn_Current.blockType == BlockType.Stair)
            {
                //MainManager.Instance.playerBody.transform.SetLocalPositionAndRotation(new Vector3(MainManager.Instance.playerBody.transform.localPosition.x, stairHeight, MainManager.Instance.playerBody.transform.localPosition.z), MainManager.Instance.playerBody.transform.localRotation);
            }
            else if (MainManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Water>())
            {
                MainManager.Instance.playerBody.transform.SetLocalPositionAndRotation(new Vector3(MainManager.Instance.playerBody.transform.localPosition.x, height_Water, MainManager.Instance.playerBody.transform.localPosition.z), MainManager.Instance.playerBody.transform.localRotation);
            }
            else if (MainManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Lava>())
            {
                MainManager.Instance.playerBody.transform.SetLocalPositionAndRotation(new Vector3(MainManager.Instance.playerBody.transform.localPosition.x, height_Lava, MainManager.Instance.playerBody.transform.localPosition.z), MainManager.Instance.playerBody.transform.localRotation);
            }
            else
            {
                MainManager.Instance.playerBody.transform.SetLocalPositionAndRotation(new Vector3(MainManager.Instance.playerBody.transform.localPosition.x, height_Normal, MainManager.Instance.playerBody.transform.localPosition.z), MainManager.Instance.playerBody.transform.localRotation);
            }
        }
        else if (Player_SwiftSwim.Instance.isSwiftSwimming_Up || Player_SwiftSwim.Instance.isSwiftSwimming_Down)
        {
            MainManager.Instance.playerBody.transform.SetLocalPositionAndRotation(new Vector3(MainManager.Instance.playerBody.transform.localPosition.x, height_Water, MainManager.Instance.playerBody.transform.localPosition.z), MainManager.Instance.playerBody.transform.localRotation);
        }
        else
        {
            MainManager.Instance.playerBody.transform.SetLocalPositionAndRotation(new Vector3(MainManager.Instance.playerBody.transform.localPosition.x, height_Normal, MainManager.Instance.playerBody.transform.localPosition.z), MainManager.Instance.playerBody.transform.localRotation);
        }
    }
}
