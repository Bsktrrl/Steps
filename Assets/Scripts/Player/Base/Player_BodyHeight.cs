using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BodyHeight : MonoBehaviour
{
    [Header("Local PlayerBody height level")]
    [SerializeField] float normalHeight = 0;

    [SerializeField] float stairHeight = 0.5f;
    [SerializeField] float waterHeight = -0.8f;
    [SerializeField] float lavaHeight = -0.9f;


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
                MainManager.Instance.playerBody.transform.SetLocalPositionAndRotation(new Vector3(MainManager.Instance.playerBody.transform.localPosition.x, waterHeight, MainManager.Instance.playerBody.transform.localPosition.z), MainManager.Instance.playerBody.transform.localRotation);
            }
            else if (MainManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Lava>())
            {
                MainManager.Instance.playerBody.transform.SetLocalPositionAndRotation(new Vector3(MainManager.Instance.playerBody.transform.localPosition.x, lavaHeight, MainManager.Instance.playerBody.transform.localPosition.z), MainManager.Instance.playerBody.transform.localRotation);
            }
            else
            {
                MainManager.Instance.playerBody.transform.SetLocalPositionAndRotation(new Vector3(MainManager.Instance.playerBody.transform.localPosition.x, normalHeight, MainManager.Instance.playerBody.transform.localPosition.z), MainManager.Instance.playerBody.transform.localRotation);
            }
        }
        else
        {
            MainManager.Instance.playerBody.transform.SetLocalPositionAndRotation(new Vector3(MainManager.Instance.playerBody.transform.localPosition.x, normalHeight, MainManager.Instance.playerBody.transform.localPosition.z), MainManager.Instance.playerBody.transform.localRotation);
        }
    }
}
