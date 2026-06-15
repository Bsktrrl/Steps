using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleShaderOnOffScript : Singleton<HoleShaderOnOffScript>
{
    [SerializeField] private Camera cameraObject;

    [Header("Raycast Settings")]
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private bool checkEveryFrame = true;

    public float PlayerBody_offset;

    private bool holeShaderIsOn;
    bool transitionBool;


    //--------------------


    private void Awake()
    {
        if (cameraObject == null)
            cameraObject = Camera.main;
    }
    private void Update()
    {
        if (checkEveryFrame)
        {
            CheckForBlockInfoBetweenPlayerAndCamera();
        }
    }


    //--------------------


    private void CheckForBlockInfoBetweenPlayerAndCamera()
    {
        if (PlayerManager.Instance == null ||
            PlayerManager.Instance.playerBody == null ||
            cameraObject == null)
        {
            HoleShader_Off();
            return;
        }

        //Vector3 playerPosition = PlayerManager.Instance.playerBody.transform.position + (Vector3.up * (Mathf.Abs(Player_BodyHeight.Instance.height_Normal) + Mathf.Abs(PlayerBody_offset)));
        Vector3 playerPosition = PlayerManager.Instance.playerBody.transform.position;

        Vector3 cameraPosition = cameraObject.transform.position;

        Vector3 direction = cameraPosition - playerPosition;
        float distance = direction.magnitude;

        if (distance <= 0.01f)
        {
            HoleShader_Off();
            return;
        }

        direction.Normalize();

        bool foundBlockInfo = false;

        RaycastHit[] hits = Physics.RaycastAll(
            playerPosition,
            direction,
            distance,
            obstacleLayers,
            QueryTriggerInteraction.Ignore
        );

        foreach (RaycastHit hit in hits)
        {
            BlockInfo blockInfo = hit.collider.GetComponentInParent<BlockInfo>();

            if (blockInfo != null)
            {
                foundBlockInfo = true;
                break;
            }
        }

        if (foundBlockInfo)
        {
            SetHoleShaderState(true);
        }
        else
        {
            SetHoleShaderState(false);
        }
    }

    private void SetHoleShaderState(bool active)
    {
        if (holeShaderIsOn == active)
            return;

        holeShaderIsOn = active;

        if (active)
        {
            HoleShader_On();
        }
        else
        {
            HoleShader_Off();
        }
    }


    //--------------------


    public void HoleShader_On()
    {
        if (FreeCam.Instance._isActive) return;

        transitionBool = true;
        Shader.SetGlobalFloat("_HoleShaderEnabled", 1);
        Shader.SetGlobalFloat("_CameraHoleEnabled", 1);
    }
    public void HoleShader_Off()
    {
        transitionBool = false;
        Shader.SetGlobalFloat("_HoleShaderEnabled", 0);
        Shader.SetGlobalFloat("_CameraHoleEnabled", 0);
    }

    public void SetHullShaderPosY(float y)
    {
        Shader.SetGlobalFloat("_PlayerOffset", y);
    }


    //--------------------


    private void OnDrawGizmosSelected()
    {
        if (cameraObject == null)
            return;

        if (PlayerManager.Instance == null || PlayerManager.Instance.playerBody == null)
            return;

        Gizmos.DrawLine(
            PlayerManager.Instance.playerBody.transform.position,
            cameraObject.transform.position
        );
    }
}
