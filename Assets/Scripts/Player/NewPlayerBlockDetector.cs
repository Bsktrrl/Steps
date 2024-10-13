using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewPlayerBlockDetector : Singleton<NewPlayerBlockDetector>
{
    [Header("Player BlockDetector Parent")]
    public GameObject blockDetector_Parent;

    [Header("DetectorSpots Horizontal")]
    [SerializeField] GameObject detectorSpot_Horizontal_Front;
    [SerializeField] GameObject detectorSpot_Horizontal_Back;
    [SerializeField] GameObject detectorSpot_Horizontal_Left;
    [SerializeField] GameObject detectorSpot_Horizontal_Right;

    [Header("DetectorSpots Vertical")]
    [SerializeField] GameObject detectorSpot_Vertical_Center;

    [SerializeField] GameObject detectorSpot_Vertical_Front;
    [SerializeField] GameObject detectorSpot_Vertical_Back;
    [SerializeField] GameObject detectorSpot_Vertical_Left;
    [SerializeField] GameObject detectorSpot_Vertical_Right;

    [Header("DetectorSpots for Stairs")]
    [SerializeField] GameObject detectorSpot_Stair_Front;
    [SerializeField] GameObject detectorSpot_Stair_Back;
    [SerializeField] GameObject detectorSpot_Stair_Left;
    [SerializeField] GameObject detectorSpot_Stair_Right;

    [Header("Raycast")]
    [SerializeField] float maxDistance_Horizontal = 0.5f;
    [SerializeField] float maxDistance_Vertical = 0.8f;
    [SerializeField] float maxDistance_Stair = 0.75f;
    RaycastHit hit;


    //--------------------


    private void Update()
    {
        RaycastSetup();

        UpdateVerticalRacastLength();
    }


    //--------------------


    void RaycastSetup()
    {
        //Check which block the player stands on
        PerformRaycast_Center_Vertical(detectorSpot_Vertical_Center, Vector3.down);

        //Check if something is in the way of movement
        if (gameObject.GetComponent<PlayerCamera>().cameraState == CameraState.Forward)
        {
            //Check if something is in the way
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Front, Vector3.forward);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Back, Vector3.back);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Left, Vector3.left);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Right, Vector3.right);

            //Check if there is a block where the player can go
            PerformRaycast_Vertical(detectorSpot_Vertical_Front, Vector3.forward);
            PerformRaycast_Vertical(detectorSpot_Vertical_Back, Vector3.back);
            PerformRaycast_Vertical(detectorSpot_Vertical_Left, Vector3.left);
            PerformRaycast_Vertical(detectorSpot_Vertical_Right, Vector3.right);
        }
        else if (gameObject.GetComponent<PlayerCamera>().cameraState == CameraState.Backward)
        {
            //Check if something is in the way
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Front, Vector3.back);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Back, Vector3.forward);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Left, Vector3.right);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Right, Vector3.left);

            //Check if there is a block where the player can go
            PerformRaycast_Vertical(detectorSpot_Vertical_Front, Vector3.back);
            PerformRaycast_Vertical(detectorSpot_Vertical_Back, Vector3.forward);
            PerformRaycast_Vertical(detectorSpot_Vertical_Left, Vector3.right);
            PerformRaycast_Vertical(detectorSpot_Vertical_Right, Vector3.left);
        }
        else if (gameObject.GetComponent<PlayerCamera>().cameraState == CameraState.Left)
        {
            //Check if something is in the way
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Front, Vector3.left);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Back, Vector3.right);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Left, Vector3.back);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Right, Vector3.forward);

            //Check if there is a block where the player can go
            PerformRaycast_Vertical(detectorSpot_Vertical_Front, Vector3.left);
            PerformRaycast_Vertical(detectorSpot_Vertical_Back, Vector3.right);
            PerformRaycast_Vertical(detectorSpot_Vertical_Left, Vector3.back);
            PerformRaycast_Vertical(detectorSpot_Vertical_Right, Vector3.forward);
        }
        else if (gameObject.GetComponent<PlayerCamera>().cameraState == CameraState.Right)
        {
            //Check if something is in the way
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Front, Vector3.right);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Back, Vector3.left);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Left, Vector3.forward);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Right, Vector3.back);

            //Check if there is a block where the player can go
            PerformRaycast_Vertical(detectorSpot_Vertical_Front, Vector3.right);
            PerformRaycast_Vertical(detectorSpot_Vertical_Back, Vector3.left);
            PerformRaycast_Vertical(detectorSpot_Vertical_Left, Vector3.forward);
            PerformRaycast_Vertical(detectorSpot_Vertical_Right, Vector3.back);
        }

        //Check for Stair Edge, to prevent moving off the Stair
        UpdateStairRaycast();
    }

    void PerformRaycast_Center_Vertical(GameObject rayPointObject, Vector3 direction)
    {
        if (Physics.Raycast(rayPointObject.transform.position, direction, out hit, maxDistance_Horizontal))
        {
            Debug.DrawRay(rayPointObject.transform.position, direction * hit.distance, Color.green);

            if (hit.transform.GetComponent<BlockInfo>())
            {
                MainManager.Instance.block_StandingOn.block = hit.transform.gameObject;
                MainManager.Instance.block_StandingOn.blockPosition = hit.transform.position;
                MainManager.Instance.block_StandingOn.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
                MainManager.Instance.block_StandingOn.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
            }
        }
        else
        {
            Debug.DrawRay(rayPointObject.transform.position, direction * maxDistance_Horizontal, Color.red);

            MainManager.Instance.block_StandingOn.block = null;
            MainManager.Instance.block_StandingOn.blockPosition = Vector3.zero;
            MainManager.Instance.block_StandingOn.blockElement = BlockElement.None;
            MainManager.Instance.block_StandingOn.blockType = BlockType.None;
        }
    }

    void PerformRaycast_Horizontal(GameObject rayPointObject, Vector3 direction)
    {
        if (Physics.Raycast(rayPointObject.transform.position, direction, out hit, maxDistance_Horizontal))
        {
            Debug.DrawRay(rayPointObject.transform.position, direction * hit.distance, Color.red);

            if (hit.transform.GetComponent<BlockInfo>())
            {
                if (rayPointObject == detectorSpot_Horizontal_Front)
                {
                    MainManager.Instance.block_Horizontal_InFront.block = hit.transform.gameObject;
                    MainManager.Instance.block_Horizontal_InFront.blockPosition = hit.transform.position;
                    MainManager.Instance.block_Horizontal_InFront.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
                    MainManager.Instance.block_Horizontal_InFront.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
                }
                else if (rayPointObject == detectorSpot_Horizontal_Back)
                {
                    MainManager.Instance.block_Horizontal_InBack.block = hit.transform.gameObject;
                    MainManager.Instance.block_Horizontal_InBack.blockPosition = hit.transform.position;
                    MainManager.Instance.block_Horizontal_InBack.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
                    MainManager.Instance.block_Horizontal_InBack.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
                }
                else if (rayPointObject == detectorSpot_Horizontal_Left)
                {
                    MainManager.Instance.block_Horizontal_ToTheLeft.block = hit.transform.gameObject;
                    MainManager.Instance.block_Horizontal_ToTheLeft.blockPosition = hit.transform.position;
                    MainManager.Instance.block_Horizontal_ToTheLeft.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
                    MainManager.Instance.block_Horizontal_ToTheLeft.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
                }
                else if (rayPointObject == detectorSpot_Horizontal_Right)
                {
                    MainManager.Instance.block_Horizontal_ToTheRight.block = hit.transform.gameObject;
                    MainManager.Instance.block_Horizontal_ToTheRight.blockPosition = hit.transform.position;
                    MainManager.Instance.block_Horizontal_ToTheRight.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
                    MainManager.Instance.block_Horizontal_ToTheRight.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
                }
            }

            CheckRaycastDirection_Horizontal(direction);
        }

        else
        {
            Debug.DrawRay(rayPointObject.transform.position, direction * maxDistance_Horizontal, Color.green);

            if (rayPointObject == detectorSpot_Horizontal_Front)
            {
                MainManager.Instance.block_Horizontal_InFront.block = null;
                MainManager.Instance.block_Horizontal_InFront.blockPosition = Vector3.zero;
                MainManager.Instance.block_Horizontal_InFront.blockElement = BlockElement.None;
                MainManager.Instance.block_Horizontal_InFront.blockType = BlockType.None;
            }
            else if (rayPointObject == detectorSpot_Horizontal_Back)
            {
                MainManager.Instance.block_Horizontal_InBack.block = null;
                MainManager.Instance.block_Horizontal_InBack.blockPosition = Vector3.zero;
                MainManager.Instance.block_Horizontal_InBack.blockElement = BlockElement.None;
                MainManager.Instance.block_Horizontal_InBack.blockType = BlockType.None;
            }
            else if (rayPointObject == detectorSpot_Horizontal_Left)
            {
                MainManager.Instance.block_Horizontal_ToTheLeft.block = null;
                MainManager.Instance.block_Horizontal_ToTheLeft.blockPosition = Vector3.zero;
                MainManager.Instance.block_Horizontal_ToTheLeft.blockElement = BlockElement.None;
                MainManager.Instance.block_Horizontal_ToTheLeft.blockType = BlockType.None;
            }
            else if (rayPointObject == detectorSpot_Horizontal_Right)
            {
                MainManager.Instance.block_Horizontal_ToTheRight.block = null;
                MainManager.Instance.block_Horizontal_ToTheRight.blockPosition = Vector3.zero;
                MainManager.Instance.block_Horizontal_ToTheRight.blockElement = BlockElement.None;
                MainManager.Instance.block_Horizontal_ToTheRight.blockType = BlockType.None;
            }

            ResetRaycastDirection_Horizontal(direction);
        }
    }
    void CheckRaycastDirection_Horizontal(Vector3 direction)
    {
        if (direction == Vector3.forward)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Forward = false;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Back = false;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Right = false;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Left = false;
                    break;

                default:
                    break;
            }
        }
        if (direction == Vector3.back)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Back = false;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Forward = false;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Left = false;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Right = false;
                    break;

                default:
                    break;
            }
        }
        if (direction == Vector3.left)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Left = false;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Right = false;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Forward = false;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Back = false;
                    break;

                default:
                    break;
            }
        }
        if (direction == Vector3.right)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Right = false;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Left = false;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Back = false;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Forward = false;
                    break;

                default:
                    break;
            }
        }
    }
    void ResetRaycastDirection_Horizontal(Vector3 direction)
    {
        if (direction == Vector3.forward)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Forward = true;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Back = true;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Right = true;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Left = true;
                    break;

                default:
                    break;
            }
        }
        if (direction == Vector3.back)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Back = true;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Forward = true;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Left = true;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Right = true;
                    break;

                default:
                    break;
            }
        }
        if (direction == Vector3.left)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Left = true;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Right = true;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Forward = true;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Back = true;
                    break;

                default:
                    break;
            }
        }
        if (direction == Vector3.right)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Right = true;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Left = true;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Back = true;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Forward = true;
                    break;

                default:
                    break;
            }
        }
    }

    void PerformRaycast_Vertical(GameObject rayPointObject, Vector3 direction)
    {
        if (Physics.Raycast(rayPointObject.transform.position, Vector3.down, out hit, maxDistance_Vertical))
        {
            Debug.DrawRay(rayPointObject.transform.position, Vector3.down * maxDistance_Vertical, Color.green);

            if (hit.transform.GetComponent<BlockInfo>())
            {
                if (rayPointObject == detectorSpot_Vertical_Front)
                {
                    MainManager.Instance.block_Vertical_InFront.block = hit.transform.gameObject;
                    MainManager.Instance.block_Vertical_InFront.blockPosition = hit.transform.position;
                    MainManager.Instance.block_Vertical_InFront.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
                    MainManager.Instance.block_Vertical_InFront.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
                }
                else if (rayPointObject == detectorSpot_Vertical_Back)
                {
                    MainManager.Instance.block_Vertical_InBack.block = hit.transform.gameObject;
                    MainManager.Instance.block_Vertical_InBack.blockPosition = hit.transform.position;
                    MainManager.Instance.block_Vertical_InBack.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
                    MainManager.Instance.block_Vertical_InBack.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
                }
                else if (rayPointObject == detectorSpot_Vertical_Left)
                {
                    MainManager.Instance.block_Vertical_ToTheLeft.block = hit.transform.gameObject;
                    MainManager.Instance.block_Vertical_ToTheLeft.blockPosition = hit.transform.position;
                    MainManager.Instance.block_Vertical_ToTheLeft.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
                    MainManager.Instance.block_Vertical_ToTheLeft.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
                }
                else if (rayPointObject == detectorSpot_Vertical_Right)
                {
                    MainManager.Instance.block_Vertical_ToTheRight.block = hit.transform.gameObject;
                    MainManager.Instance.block_Vertical_ToTheRight.blockPosition = hit.transform.position;
                    MainManager.Instance.block_Vertical_ToTheRight.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
                    MainManager.Instance.block_Vertical_ToTheRight.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
                }
            }
        }
        else
        {
            Debug.DrawRay(rayPointObject.transform.position, Vector3.down * maxDistance_Vertical, Color.red);

            if (rayPointObject == detectorSpot_Vertical_Front)
            {
                MainManager.Instance.block_Vertical_InFront.block = null;
                MainManager.Instance.block_Vertical_InFront.blockPosition = Vector3.zero;
                MainManager.Instance.block_Vertical_InFront.blockElement = BlockElement.None;
                MainManager.Instance.block_Vertical_InFront.blockType = BlockType.None;
            }
            else if (rayPointObject == detectorSpot_Vertical_Back)
            {
                MainManager.Instance.block_Vertical_InBack.block = null;
                MainManager.Instance.block_Vertical_InBack.blockPosition = Vector3.zero;
                MainManager.Instance.block_Vertical_InBack.blockElement = BlockElement.None;
                MainManager.Instance.block_Vertical_InBack.blockType = BlockType.None;
            }
            else if (rayPointObject == detectorSpot_Vertical_Left)
            {
                MainManager.Instance.block_Vertical_ToTheLeft.block = null;
                MainManager.Instance.block_Vertical_ToTheLeft.blockPosition = Vector3.zero;
                MainManager.Instance.block_Vertical_ToTheLeft.blockElement = BlockElement.None;
                MainManager.Instance.block_Vertical_ToTheLeft.blockType = BlockType.None;
            }
            else if (rayPointObject == detectorSpot_Vertical_Right)
            {
                MainManager.Instance.block_Vertical_ToTheRight.block = null;
                MainManager.Instance.block_Vertical_ToTheRight.blockPosition = Vector3.zero;
                MainManager.Instance.block_Vertical_ToTheRight.blockElement = BlockElement.None;
                MainManager.Instance.block_Vertical_ToTheRight.blockType = BlockType.None;
            }
        }

        ResetRaycastDirection_Vertical(direction);
    }
    void ResetRaycastDirection_Vertical(Vector3 direction)
    {
        if (direction == Vector3.forward)
        {
            if (MainManager.Instance.block_Vertical_InFront.blockType == BlockType.None)
            {
                MainManager.Instance.canMove_Forward = false;
            }
            else if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair && MainManager.Instance.block_Horizontal_InFront.blockType == BlockType.Cube)
            {
                MainManager.Instance.canMove_Forward = false;
            }
            //else if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair && MainManager.Instance.block_Horizontal_InFront.blockType == BlockType.None && MainManager.Instance.block_Vertical_InFront.blockType != BlockType.None)
            //{
            //    MainManager.Instance.canMove_Forward = false;
            //}
            else if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair)
            {
                if (MainManager.Instance.block_Vertical_InFront.blockType == BlockType.Stair || MainManager.Instance.block_Vertical_InFront.blockType == BlockType.Cube)
                    MainManager.Instance.canMove_Forward = true;
                else
                    MainManager.Instance.canMove_Forward = false;
            }
        }
        else if (direction == Vector3.back)
        {
            if (MainManager.Instance.block_Vertical_InBack.blockType == BlockType.None)
            {
                MainManager.Instance.canMove_Back = false;
            }
            else if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair && MainManager.Instance.block_Horizontal_InBack.blockType == BlockType.Cube)
            {
                MainManager.Instance.canMove_Back = false;
            }
            //else if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair && MainManager.Instance.block_Horizontal_InBack.blockType == BlockType.None && MainManager.Instance.block_Vertical_InBack.blockType != BlockType.None)
            //{
            //    MainManager.Instance.canMove_Back = false;
            //}
            else if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair)
            {
                if (MainManager.Instance.block_Vertical_InBack.blockType == BlockType.Stair || MainManager.Instance.block_Vertical_InBack.blockType == BlockType.Cube)
                    MainManager.Instance.canMove_Back = true;
                else
                    MainManager.Instance.canMove_Back = false;
            }
        }
        else if (direction == Vector3.left)
        {
            if (MainManager.Instance.block_Vertical_ToTheLeft.blockType == BlockType.None)
            {
                MainManager.Instance.canMove_Left = false;
            }
            else if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair && MainManager.Instance.block_Horizontal_ToTheLeft.blockType == BlockType.Cube)
            {
                MainManager.Instance.canMove_Left = false;
            }
            //else if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair && MainManager.Instance.block_Horizontal_ToTheLeft.blockType == BlockType.None && MainManager.Instance.block_Vertical_ToTheLeft.blockType != BlockType.None)
            //{
            //    MainManager.Instance.canMove_Left = false;
            //}
            else if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair)
            {
                if (MainManager.Instance.block_Vertical_ToTheLeft.blockType == BlockType.Stair || MainManager.Instance.block_Vertical_ToTheLeft.blockType == BlockType.Cube)
                    MainManager.Instance.canMove_Left = true;
                else
                    MainManager.Instance.canMove_Left = false;
            }
        }
        else if (direction == Vector3.right)
        {
            if (MainManager.Instance.block_Vertical_ToTheRight.blockType == BlockType.None)
            {
                MainManager.Instance.canMove_Right = false;
            }
            else if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair && MainManager.Instance.block_Horizontal_ToTheRight.blockType == BlockType.Cube)
            {
                MainManager.Instance.canMove_Right = false;
            }
            //else if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair && MainManager.Instance.block_Horizontal_ToTheRight.blockType == BlockType.None && MainManager.Instance.block_Vertical_ToTheRight.blockType != BlockType.None)
            //{
            //    MainManager.Instance.canMove_Right = false;
            //}
            else if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair)
            {
                if (MainManager.Instance.block_Vertical_ToTheRight.blockType == BlockType.Stair || MainManager.Instance.block_Vertical_ToTheRight.blockType == BlockType.Cube)
                    MainManager.Instance.canMove_Right = true;
                else
                    MainManager.Instance.canMove_Right = false;
            }
        }
    }
    void UpdateStairRaycast()
    {
        if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair)
        {
            //Front
            if (Physics.Raycast(detectorSpot_Stair_Front.transform.position, Vector3.down, out hit, maxDistance_Stair))
            {
                if (MainManager.Instance.block_Horizontal_InFront.block == null)
                {
                    Debug.DrawRay(detectorSpot_Stair_Front.transform.position, Vector3.down * hit.distance, Color.red);
                    MainManager.Instance.canMove_Forward = false;
                }
                else
                {
                    Debug.DrawRay(detectorSpot_Stair_Front.transform.position, Vector3.down * hit.distance, Color.green);
                }
            }
            else
            {
                Debug.DrawRay(detectorSpot_Stair_Front.transform.position, Vector3.down * maxDistance_Stair, Color.cyan);
            }

            //Back
            if (Physics.Raycast(detectorSpot_Stair_Back.transform.position, Vector3.down, out hit, maxDistance_Stair))
            {
                if (MainManager.Instance.block_Horizontal_InBack.block == null)
                {
                    Debug.DrawRay(detectorSpot_Stair_Back.transform.position, Vector3.down * hit.distance, Color.red);
                    MainManager.Instance.canMove_Back = false;
                }
                else
                {
                    Debug.DrawRay(detectorSpot_Stair_Back.transform.position, Vector3.down * hit.distance, Color.green);
                }
            }
            else
            {
                Debug.DrawRay(detectorSpot_Stair_Back.transform.position, Vector3.down * maxDistance_Stair, Color.cyan);
            }

            //Left
            if (Physics.Raycast(detectorSpot_Stair_Left.transform.position, Vector3.down, out hit, maxDistance_Stair))
            {
                if (MainManager.Instance.block_Horizontal_ToTheLeft.block == null)
                {
                    Debug.DrawRay(detectorSpot_Stair_Left.transform.position, Vector3.down * hit.distance, Color.red);
                    MainManager.Instance.canMove_Left = false;
                }
                else
                {
                    Debug.DrawRay(detectorSpot_Stair_Left.transform.position, Vector3.down * hit.distance, Color.green);
                }
            }
            else
            {
                Debug.DrawRay(detectorSpot_Stair_Left.transform.position, Vector3.down * maxDistance_Stair, Color.cyan);
            }

            //Right
            if (Physics.Raycast(detectorSpot_Stair_Right.transform.position, Vector3.down, out hit, maxDistance_Stair))
            {
                if (MainManager.Instance.block_Horizontal_ToTheRight.block == null)
                {
                    Debug.DrawRay(detectorSpot_Stair_Right.transform.position, Vector3.down * hit.distance, Color.red);
                    MainManager.Instance.canMove_Right = false;
                }
                else
                {
                    Debug.DrawRay(detectorSpot_Stair_Right.transform.position, Vector3.down * hit.distance, Color.green);
                }
            }
            else
            {
                Debug.DrawRay(detectorSpot_Stair_Right.transform.position, Vector3.down * maxDistance_Stair, Color.cyan);
            }
        }
    }


    //--------------------


    void UpdateVerticalRacastLength()
    {
        if (MainManager.Instance.block_StandingOn.blockType == BlockType.Stair)
        {
            maxDistance_Vertical = 1.5f;
        }
        else
        {
            maxDistance_Vertical = 0.8f;
        }
    }
}
