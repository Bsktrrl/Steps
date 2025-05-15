using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_BlockDetector : Singleton<Player_BlockDetector>
{
    public static event Action Action_isSwitchingBlocks;
    public static event Action Action_madeFirstRaycast;

    [Header("Player BlockDetector Parent")]
    public GameObject blockDetector_Parent;

    [Header("DetectorSpots upper_Center")]
    public GameObject detectorSpot_Vertical_Center;

    [Header("DetectorSpots Horizontal")]
    [SerializeField] GameObject detectorSpot_Horizontal_Front;
    [SerializeField] GameObject detectorSpot_Horizontal_Back;
    [SerializeField] GameObject detectorSpot_Horizontal_Left;
    [SerializeField] GameObject detectorSpot_Horizontal_Right;

    [Header("DetectorSpots Vertical")]
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
    float maxDistance_Horizontal = 0.5f;
    float maxDistance_Vertical;
    float maxDistance_Vertical_Normal = 0.9f;
    float maxDistance_Vertical_Stair = 1.6f;
    float maxDistance_Stair = 0.75f;
    RaycastHit hit;

    [HideInInspector] public Vector3 lookDir;
    [HideInInspector] public float lookDir_Temp;

    bool canRunObjects;


    //--------------------


    private void Start()
    {
        RaycastSetup();
        PerformRaycast_Center_Vertical(detectorSpot_Vertical_Center, Vector3.down);

        StartCoroutine(startRaycastSetup(0.2f));
    }
    IEnumerator startRaycastSetup(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        RaycastSetup();
    }

    private void Update()
    {
        if (!canRunObjects) { return; }
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        UpdateBlockLookingAt();

        RaycastSetup();

        UpdateVerticalRaycastLength();
    }

    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += StartRunningObject;
        PlayerStats.Action_RespawnPlayer += RaycastSetup;
        PlayerStats.Action_RespawnPlayer += Update_BlockStandingOn;

        Player_Movement.Action_BodyRotated += UpdateBlockLookingAt;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= StartRunningObject;
        PlayerStats.Action_RespawnPlayer -= RaycastSetup;
        PlayerStats.Action_RespawnPlayer -= Update_BlockStandingOn;

        Player_Movement.Action_BodyRotated -= UpdateBlockLookingAt;
    }
    void StartRunningObject()
    {
        canRunObjects = true;
    }


    //--------------------


    public void RaycastSetup()
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        //Check which block the player stands on
        if (gameObject.GetComponent<Player_Movement>().movementStates == MovementStates.Moving)
        {
            Update_BlockStandingOn();
        }

        //Check if something is in the way of movement
        if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
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
        else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
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
        else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
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
        else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
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

        //Check for Stair Edge, to prevent moving off the Stair
        UpdateRaycastingFromStair();

        Action_MadeFirstRaycast_Invoke();
    }
    public void ReycastReset()
    {
        PlayerManager.Instance.block_MovingTowards = null;

        PlayerManager.Instance.block_LookingAt_Horizontal = null;
        PlayerManager.Instance.block_LookingAt_Vertical = null;

        //PlayerManager.Instance.block_StandingOn_Current = null;
        //PlayerManager.Instance.block_StandingOn_Previous = null;

        //PlayerManager.Instance.block_Horizontal_InFront = null;
        //PlayerManager.Instance.block_Horizontal_InBack = null;
        //PlayerManager.Instance.block_Horizontal_ToTheLeft = null;
        //PlayerManager.Instance.block_Horizontal_ToTheRight = null;

        //PlayerManager.Instance.block_Vertical_InFront = null;
        //PlayerManager.Instance.block_Vertical_InBack = null;
        //PlayerManager.Instance.block_Vertical_ToTheLeft = null;
        //PlayerManager.Instance.block_Vertical_ToTheRight = null;

        PlayerManager.Instance.canMove_Forward = false;
        PlayerManager.Instance.canMove_Back = false;
        PlayerManager.Instance.canMove_Left = false;
        PlayerManager.Instance.canMove_Right = false;
    }

    public void Update_BlockStandingOn()
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        PerformRaycast_Center_Vertical(detectorSpot_Vertical_Center, Vector3.down);
    }

    public void PerformRaycast_Center_Vertical(GameObject rayPointObject, Vector3 direction)
    {
        if (Physics.Raycast(rayPointObject.transform.position, direction, out hit, maxDistance_Horizontal))
        {
            Debug.DrawRay(rayPointObject.transform.position, direction * hit.distance, Color.green);

            if (hit.transform.GetComponent<BlockInfo>())
            {
                PlayerManager.Instance.block_StandingOn_Current.block = hit.transform.gameObject;
                PlayerManager.Instance.block_StandingOn_Current.blockPosition = hit.transform.position;
                //MainManager.Instance.block_StandingOn_Current.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
                PlayerManager.Instance.block_StandingOn_Current.blockType = hit.transform.GetComponent<BlockInfo>().blockType;

                if (PlayerManager.Instance.block_StandingOn_Previous != PlayerManager.Instance.block_StandingOn_Current.block /*&& !isSwitchingBlock*/)
                {
                    Action_isSwitchingBlocks_Invoke();
                    PlayerManager.Instance.block_StandingOn_Previous = PlayerManager.Instance.block_StandingOn_Current.block;
                }
            }
        }
        else
        {
            Debug.DrawRay(rayPointObject.transform.position, direction * maxDistance_Horizontal, Color.red);

            PlayerManager.Instance.block_StandingOn_Current.block = null;
            PlayerManager.Instance.block_StandingOn_Current.blockPosition = Vector3.zero;
            //MainManager.Instance.block_StandingOn_Current.blockElement = BlockElement.None;
            PlayerManager.Instance.block_StandingOn_Current.blockType = BlockType.None;
        }
    }

    void PerformRaycast_Horizontal(GameObject rayPointObject, Vector3 direction)
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (Physics.Raycast(rayPointObject.transform.position, direction, out hit, maxDistance_Horizontal))
        {
            Raycast_Horizontal_Hit(rayPointObject, direction);
        }
        else
        {
            Raycast_Horizontal_NotHit(rayPointObject, direction);
        }
    }
    void Raycast_Horizontal_Hit(GameObject rayPointObject, Vector3 direction)
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        Debug.DrawRay(rayPointObject.transform.position, direction * hit.distance, Color.red);

        if (hit.transform.GetComponent<BlockInfo>())
        {
            //if (hit.transform.GetComponent<BlockInfo>().blockType == BlockType.Fence)
            //{
            //    return;
            //}

            if (rayPointObject == detectorSpot_Horizontal_Front)
            {
                PlayerManager.Instance.block_Horizontal_InFront.block = hit.transform.gameObject;
                PlayerManager.Instance.block_Horizontal_InFront.blockPosition = hit.transform.position;
                PlayerManager.Instance.block_Horizontal_InFront.blockType = hit.transform.GetComponent<BlockInfo>().blockType;

                CheckRaycastDirection_Horizontal(direction);
            }
            else if (rayPointObject == detectorSpot_Horizontal_Back)
            {
                PlayerManager.Instance.block_Horizontal_InBack.block = hit.transform.gameObject;
                PlayerManager.Instance.block_Horizontal_InBack.blockPosition = hit.transform.position;
                PlayerManager.Instance.block_Horizontal_InBack.blockType = hit.transform.GetComponent<BlockInfo>().blockType;

                CheckRaycastDirection_Horizontal(direction);
            }
            else if (rayPointObject == detectorSpot_Horizontal_Left)
            {
                PlayerManager.Instance.block_Horizontal_ToTheLeft.block = hit.transform.gameObject;
                PlayerManager.Instance.block_Horizontal_ToTheLeft.blockPosition = hit.transform.position;
                PlayerManager.Instance.block_Horizontal_ToTheLeft.blockType = hit.transform.GetComponent<BlockInfo>().blockType;

                CheckRaycastDirection_Horizontal(direction);
            }
            else if (rayPointObject == detectorSpot_Horizontal_Right)
            {
                PlayerManager.Instance.block_Horizontal_ToTheRight.block = hit.transform.gameObject;
                PlayerManager.Instance.block_Horizontal_ToTheRight.blockPosition = hit.transform.position;
                PlayerManager.Instance.block_Horizontal_ToTheRight.blockType = hit.transform.GetComponent<BlockInfo>().blockType;

                CheckRaycastDirection_Horizontal(direction);
            }
        }

        //Prevent movement if something is blocking the way for the player
        else
        {
            CheckRaycastDirection_Horizontal(direction);
        }
    }
    void Raycast_Horizontal_NotHit(GameObject rayPointObject, Vector3 direction)
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        Debug.DrawRay(rayPointObject.transform.position, direction * maxDistance_Horizontal, Color.green);

        if (rayPointObject == detectorSpot_Horizontal_Front)
        {
            PlayerManager.Instance.block_Horizontal_InFront.block = null;
            PlayerManager.Instance.block_Horizontal_InFront.blockPosition = Vector3.zero;
            //MainManager.Instance.block_Horizontal_InFront.blockElement = BlockElement.None;
            PlayerManager.Instance.block_Horizontal_InFront.blockType = BlockType.None;
        }
        else if (rayPointObject == detectorSpot_Horizontal_Back)
        {
            PlayerManager.Instance.block_Horizontal_InBack.block = null;
            PlayerManager.Instance.block_Horizontal_InBack.blockPosition = Vector3.zero;
            //MainManager.Instance.block_Horizontal_InBack.blockElement = BlockElement.None;
            PlayerManager.Instance.block_Horizontal_InBack.blockType = BlockType.None;
        }
        else if (rayPointObject == detectorSpot_Horizontal_Left)
        {
            PlayerManager.Instance.block_Horizontal_ToTheLeft.block = null;
            PlayerManager.Instance.block_Horizontal_ToTheLeft.blockPosition = Vector3.zero;
            //MainManager.Instance.block_Horizontal_ToTheLeft.blockElement = BlockElement.None;
            PlayerManager.Instance.block_Horizontal_ToTheLeft.blockType = BlockType.None;
        }
        else if (rayPointObject == detectorSpot_Horizontal_Right)
        {
            PlayerManager.Instance.block_Horizontal_ToTheRight.block = null;
            PlayerManager.Instance.block_Horizontal_ToTheRight.blockPosition = Vector3.zero;
            //MainManager.Instance.block_Horizontal_ToTheRight.blockElement = BlockElement.None;
            PlayerManager.Instance.block_Horizontal_ToTheRight.blockType = BlockType.None;
        }

        ResetRaycastDirection_Horizontal(direction);
    }

    void CheckRaycastDirection_Horizontal(Vector3 direction)
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (direction == Vector3.forward)
        {
            switch (CameraController.Instance.cameraRotationState)
            {
                case CameraRotationState.Forward:
                    PlayerManager.Instance.canMove_Forward = false;
                    break;
                case CameraRotationState.Backward:
                    PlayerManager.Instance.canMove_Back = false;
                    break;
                case CameraRotationState.Left:
                    PlayerManager.Instance.canMove_Left = false;
                    break;
                case CameraRotationState.Right:
                    PlayerManager.Instance.canMove_Right = false;
                    break;

                default:
                    break;
            }
        }
        if (direction == Vector3.back)
        {
            switch (CameraController.Instance.cameraRotationState)
            {
                case CameraRotationState.Forward:
                    PlayerManager.Instance.canMove_Back = false;
                    break;
                case CameraRotationState.Backward:
                    PlayerManager.Instance.canMove_Forward = false;
                    break;
                case CameraRotationState.Left:
                    PlayerManager.Instance.canMove_Right = false;
                    break;
                case CameraRotationState.Right:
                    PlayerManager.Instance.canMove_Left = false;
                    break;

                default:
                    break;
            }
        }
        if (direction == Vector3.left)
        {
            switch (CameraController.Instance.cameraRotationState)
            {
                case CameraRotationState.Forward:
                    PlayerManager.Instance.canMove_Left = false;
                    break;
                case CameraRotationState.Backward:
                    PlayerManager.Instance.canMove_Right = false;
                    break;
                case CameraRotationState.Left:
                    PlayerManager.Instance.canMove_Back = false;
                    break;
                case CameraRotationState.Right:
                    PlayerManager.Instance.canMove_Forward = false;
                    break;

                default:
                    break;
            }
        }
        if (direction == Vector3.right)
        {
            switch (CameraController.Instance.cameraRotationState)
            {
                case CameraRotationState.Forward:
                    PlayerManager.Instance.canMove_Right = false;
                    break;
                case CameraRotationState.Backward:
                    PlayerManager.Instance.canMove_Left = false;
                    break;
                case CameraRotationState.Left:
                    PlayerManager.Instance.canMove_Forward = false;
                    break;
                case CameraRotationState.Right:
                    PlayerManager.Instance.canMove_Back = false;
                    break;

                default:
                    break;
            }
        }
    }
    void ResetRaycastDirection_Horizontal(Vector3 direction)
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (direction == Vector3.forward)
        {
            switch (CameraController.Instance.cameraRotationState)
            {
                case CameraRotationState.Forward:
                    PlayerManager.Instance.canMove_Forward = true;
                    break;
                case CameraRotationState.Backward:
                    PlayerManager.Instance.canMove_Back = true;
                    break;
                case CameraRotationState.Left:
                    PlayerManager.Instance.canMove_Left = true;
                    break;
                case CameraRotationState.Right:
                    PlayerManager.Instance.canMove_Right = true;
                    break;

                default:
                    break;
            }
        }
        if (direction == Vector3.back)
        {
            switch (CameraController.Instance.cameraRotationState)
            {
                case CameraRotationState.Forward:
                    PlayerManager.Instance.canMove_Back = true;
                    break;
                case CameraRotationState.Backward:
                    PlayerManager.Instance.canMove_Forward = true;
                    break;
                case CameraRotationState.Left:
                    PlayerManager.Instance.canMove_Right = true;
                    break;
                case CameraRotationState.Right:
                    PlayerManager.Instance.canMove_Left = true;
                    break;

                default:
                    break;
            }
        }
        if (direction == Vector3.left)
        {
            switch (CameraController.Instance.cameraRotationState)
            {
                case CameraRotationState.Forward:
                    PlayerManager.Instance.canMove_Left = true;
                    break;
                case CameraRotationState.Backward:
                    PlayerManager.Instance.canMove_Right = true;
                    break;
                case CameraRotationState.Left:
                    PlayerManager.Instance.canMove_Back = true;
                    break;
                case CameraRotationState.Right:
                    PlayerManager.Instance.canMove_Forward = true;
                    break;

                default:
                    break;
            }
        }
        if (direction == Vector3.right)
        {
            switch (CameraController.Instance.cameraRotationState)
            {
                case CameraRotationState.Forward:
                    PlayerManager.Instance.canMove_Right = true;
                    break;
                case CameraRotationState.Backward:
                    PlayerManager.Instance.canMove_Left = true;
                    break;
                case CameraRotationState.Left:
                    PlayerManager.Instance.canMove_Forward = true;
                    break;
                case CameraRotationState.Right:
                    PlayerManager.Instance.canMove_Back = true;
                    break;

                default:
                    break;
            }
        }
    }

    void PerformRaycast_Vertical(GameObject rayPointObject, Vector3 direction)
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (Physics.Raycast(rayPointObject.transform.position, Vector3.down, out hit, maxDistance_Vertical))
        {
            Debug.DrawRay(rayPointObject.transform.position, Vector3.down * maxDistance_Vertical, Color.green);

            if (hit.transform.GetComponent<BlockInfo>())
            {
                if (hit.transform.GetComponent<BlockInfo>().blockType == BlockType.Cube || hit.transform.GetComponent<BlockInfo>().blockType == BlockType.Slab || hit.transform.GetComponent<BlockInfo>().blockType == BlockType.Stair || hit.transform.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                {
                    Raycast_Vertical_Hit(rayPointObject, direction);
                }
                else
                {
                    Raycast_Vertical_NotHit(rayPointObject, direction);
                }
            }
        }
        else
        {
            Raycast_Vertical_NotHit(rayPointObject, direction);
        }

        ResetRaycastDirection_Vertical(direction);
    }
    void Raycast_Vertical_Hit(GameObject rayPointObject, Vector3 direction)
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (rayPointObject == detectorSpot_Vertical_Front)
        {
            PlayerManager.Instance.block_Vertical_InFront.block = hit.transform.gameObject;
            PlayerManager.Instance.block_Vertical_InFront.blockPosition = hit.transform.position;
            //MainManager.Instance.block_Vertical_InFront.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
            PlayerManager.Instance.block_Vertical_InFront.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
        }
        else if (rayPointObject == detectorSpot_Vertical_Back)
        {
            PlayerManager.Instance.block_Vertical_InBack.block = hit.transform.gameObject;
            PlayerManager.Instance.block_Vertical_InBack.blockPosition = hit.transform.position;
            //MainManager.Instance.block_Vertical_InBack.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
            PlayerManager.Instance.block_Vertical_InBack.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
        }
        else if (rayPointObject == detectorSpot_Vertical_Left)
        {
            PlayerManager.Instance.block_Vertical_ToTheLeft.block = hit.transform.gameObject;
            PlayerManager.Instance.block_Vertical_ToTheLeft.blockPosition = hit.transform.position;
            //MainManager.Instance.block_Vertical_ToTheLeft.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
            PlayerManager.Instance.block_Vertical_ToTheLeft.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
        }
        else if (rayPointObject == detectorSpot_Vertical_Right)
        {
            PlayerManager.Instance.block_Vertical_ToTheRight.block = hit.transform.gameObject;
            PlayerManager.Instance.block_Vertical_ToTheRight.blockPosition = hit.transform.position;
            //MainManager.Instance.block_Vertical_ToTheRight.blockElement = hit.transform.GetComponent<BlockInfo>().blockElement;
            PlayerManager.Instance.block_Vertical_ToTheRight.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
        }
    }
    void Raycast_Vertical_NotHit(GameObject rayPointObject, Vector3 direction)
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        Debug.DrawRay(rayPointObject.transform.position, Vector3.down * maxDistance_Vertical, Color.red);

        if (rayPointObject == detectorSpot_Vertical_Front)
        {
            PlayerManager.Instance.block_Vertical_InFront.block = null;
            PlayerManager.Instance.block_Vertical_InFront.blockPosition = Vector3.zero;
            //MainManager.Instance.block_Vertical_InFront.blockElement = BlockElement.None;
            PlayerManager.Instance.block_Vertical_InFront.blockType = BlockType.None;
        }
        else if (rayPointObject == detectorSpot_Vertical_Back)
        {
            PlayerManager.Instance.block_Vertical_InBack.block = null;
            PlayerManager.Instance.block_Vertical_InBack.blockPosition = Vector3.zero;
            //MainManager.Instance.block_Vertical_InBack.blockElement = BlockElement.None;
            PlayerManager.Instance.block_Vertical_InBack.blockType = BlockType.None;
        }
        else if (rayPointObject == detectorSpot_Vertical_Left)
        {
            PlayerManager.Instance.block_Vertical_ToTheLeft.block = null;
            PlayerManager.Instance.block_Vertical_ToTheLeft.blockPosition = Vector3.zero;
            //MainManager.Instance.block_Vertical_ToTheLeft.blockElement = BlockElement.None;
            PlayerManager.Instance.block_Vertical_ToTheLeft.blockType = BlockType.None;
        }
        else if (rayPointObject == detectorSpot_Vertical_Right)
        {
            PlayerManager.Instance.block_Vertical_ToTheRight.block = null;
            PlayerManager.Instance.block_Vertical_ToTheRight.blockPosition = Vector3.zero;
            //MainManager.Instance.block_Vertical_ToTheRight.blockElement = BlockElement.None;
            PlayerManager.Instance.block_Vertical_ToTheRight.blockType = BlockType.None;
        }
    }
    void ResetRaycastDirection_Vertical(Vector3 direction)
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (direction == Vector3.forward)
        {
            ResetRaycastDirection(PlayerManager.Instance.block_Vertical_InFront, PlayerManager.Instance.block_Horizontal_InFront, Vector3.forward);
        }
        else if (direction == Vector3.back)
        {
            ResetRaycastDirection(PlayerManager.Instance.block_Vertical_InBack, PlayerManager.Instance.block_Horizontal_InBack, Vector3.back);
        }
        else if (direction == Vector3.left)
        {
            ResetRaycastDirection(PlayerManager.Instance.block_Vertical_ToTheLeft, PlayerManager.Instance.block_Horizontal_ToTheLeft, Vector3.left);
        }
        else if (direction == Vector3.right)
        {
            ResetRaycastDirection(PlayerManager.Instance.block_Vertical_ToTheRight, PlayerManager.Instance.block_Horizontal_ToTheRight, Vector3.right);
        }
    }

    void ResetRaycastDirection(DetectedBlockInfo blockType_Vertical, DetectedBlockInfo blockType_Horizontal, Vector3 direction)
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (blockType_Vertical.block)
        {
            #region Water Block
            //If block is Water, you cannot move into it before having the Swimsuit Ability

            if (blockType_Vertical.block.GetComponent<Block_Water>())
            {
                if (PlayerStats.Instance.stats != null)
                {
                    if (PlayerStats.Instance.stats.abilitiesGot_Permanent != null || PlayerStats.Instance.stats.abilitiesGot_Temporary != null)
                    {
                        if (PlayerStats.Instance.stats.abilitiesGot_Permanent.SwimSuit || PlayerStats.Instance.stats.abilitiesGot_Temporary.SwimSuit || PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers || PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers || PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim || PlayerStats.Instance.stats.abilitiesGot_Temporary.SwiftSwim)
                        {
                            canMove(direction, true);
                        }
                        else
                        {
                            canMove(direction, false);
                        }
                    }
                }
            }
            #endregion

            #region Lava Block
            //If block is Lava, you cannot move into it
            else if (blockType_Vertical.block.GetComponent<Block_Lava>())
            {
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Flameable || PlayerStats.Instance.stats.abilitiesGot_Temporary.Flameable)
                {
                    canMove(direction, false);
                }
                else
                {
                    canMove(direction, false);
                }
            }
            #endregion

            #region No Block
            //if there isn't any block to move to
            else if (blockType_Vertical.blockType == BlockType.None)
            {
                canMove(direction, false);
            }
            #endregion

            #region On Stair
            //If standing on a Stair, and move into a wall
            else if ((PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Stair && blockType_Horizontal.blockType == BlockType.Cube)
                || (PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Slope && blockType_Horizontal.blockType == BlockType.Cube))
            {
                canMove(direction, false);
            }

            //If standing on a Stair, and there is possible to move further up it
            else if (PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Stair
                     || PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Slope)
            {
                if (blockType_Vertical.blockType == BlockType.Stair || blockType_Vertical.blockType == BlockType.Slope || blockType_Vertical.blockType == BlockType.Cube || blockType_Vertical.blockType == BlockType.Slab)
                {
                    canMove(direction, true);
                }
                else
                {
                    canMove(direction, false);
                }
            }
            #endregion
        }
    }

    void canMove(Vector3 direction, bool value)
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (direction == Vector3.forward)
        {
            PlayerManager.Instance.canMove_Forward = value;
        }
        else if (direction == Vector3.back)
        {
            PlayerManager.Instance.canMove_Back = value;
        }
        else if (direction == Vector3.left)
        {
            PlayerManager.Instance.canMove_Left = value;
        }
        else if (direction == Vector3.right)
        {
            PlayerManager.Instance.canMove_Right = value;
        }
    }


    void UpdateRaycastingFromStair()
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Stair || PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Slope)
        {
            //Front
            if (Physics.Raycast(detectorSpot_Stair_Front.transform.position, Vector3.down, out hit, maxDistance_Stair))
            {
                if (PlayerManager.Instance.block_Horizontal_InFront.block == null && PlayerManager.Instance.block_Vertical_InFront.block != null)
                {
                    if (PlayerManager.Instance.block_Vertical_InFront.blockType != BlockType.Stair && PlayerManager.Instance.block_Vertical_InFront.blockType != BlockType.Slope)
                    {
                        Debug.DrawRay(detectorSpot_Stair_Front.transform.position, Vector3.down * hit.distance, Color.red);
                        PlayerManager.Instance.canMove_Forward = false;
                    }
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
                if (PlayerManager.Instance.block_Horizontal_InBack.block == null && PlayerManager.Instance.block_Vertical_InBack.block != null)
                {
                    if (PlayerManager.Instance.block_Vertical_InBack.blockType != BlockType.Stair && PlayerManager.Instance.block_Vertical_InBack.blockType != BlockType.Slope)
                    {
                        Debug.DrawRay(detectorSpot_Stair_Back.transform.position, Vector3.down * hit.distance, Color.red);
                        PlayerManager.Instance.canMove_Back = false;
                    }
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
                if (PlayerManager.Instance.block_Horizontal_ToTheLeft.block == null && PlayerManager.Instance.block_Vertical_ToTheLeft.block != null)
                {
                    if (PlayerManager.Instance.block_Vertical_ToTheLeft.blockType != BlockType.Stair && PlayerManager.Instance.block_Vertical_ToTheLeft.blockType != BlockType.Slope)
                    {
                        Debug.DrawRay(detectorSpot_Stair_Left.transform.position, Vector3.down * hit.distance, Color.red);
                        PlayerManager.Instance.canMove_Left = false;
                    }
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
                if (PlayerManager.Instance.block_Horizontal_ToTheRight.block == null && PlayerManager.Instance.block_Vertical_ToTheRight.block != null)
                {
                    if (PlayerManager.Instance.block_Vertical_ToTheRight.blockType != BlockType.Stair && PlayerManager.Instance.block_Vertical_ToTheRight.blockType != BlockType.Slope)
                    {
                        Debug.DrawRay(detectorSpot_Stair_Right.transform.position, Vector3.down * hit.distance, Color.red);
                        PlayerManager.Instance.canMove_Right = false;
                    }
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


    void UpdateVerticalRaycastLength()
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Stair || PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Slope)
        {
            maxDistance_Vertical = maxDistance_Vertical_Stair;
        }
        else
        {
            maxDistance_Vertical = maxDistance_Vertical_Normal;
        }
    }

    public void UpdateBlockLookingAt()
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        //lookDir_Temp = MainManager.Instance.playerBody.transform.rotation.eulerAngles.y;
        if (PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y == 0)
            lookDir_Temp = 0;
        else if (PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y == 90)
            lookDir_Temp = 90;
        else if (PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y == 180)
            lookDir_Temp = 180;
        else if (PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y == 270)
            lookDir_Temp = 270;
        else if (PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y == 360)
            lookDir_Temp = 360;
        else if (PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y == -90)
            lookDir_Temp = -90;
        else if (PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y == -180)
            lookDir_Temp = -180;
        else if (PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y == -270)
            lookDir_Temp = -270;
        else
            lookDir_Temp = 0;

        if (lookDir_Temp == 0 || lookDir_Temp == 360)
            lookDir = Vector3.forward;
        else if (lookDir_Temp == 180 || lookDir_Temp == -180)
            lookDir = Vector3.back;
        else if (lookDir_Temp == 270 || lookDir_Temp == -90)
            lookDir = Vector3.left;
        else if (lookDir_Temp == 90 || lookDir_Temp == -270)
            lookDir = Vector3.right;

        //Get BlockLookingAt - Horizontal
        if (Physics.Raycast(transform.position, lookDir, out hit, 1))
        {
            Debug.DrawRay(transform.position, lookDir, Color.blue);

            if (hit.transform.GetComponent<BlockInfo>())
            {
                PlayerManager.Instance.block_LookingAt_Horizontal = hit.transform.gameObject;
            }
            else
            {
                PlayerManager.Instance.block_LookingAt_Horizontal = null;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, lookDir, Color.blue);

            PlayerManager.Instance.block_LookingAt_Horizontal = null;
        }

        //Get BlockLookingAt - Vertical
        if (Physics.Raycast((transform.position + (Vector3.up * 0.25f)) + lookDir, Vector3.down, out hit, 1.5f))
        {
            Debug.DrawRay((transform.position + (Vector3.up * 0.25f)) + lookDir, Vector3.down, Color.blue);

            if (hit.transform.GetComponent<BlockInfo>())
            {
                PlayerManager.Instance.block_LookingAt_Vertical = hit.transform.gameObject;
            }
            else
            {
                PlayerManager.Instance.block_LookingAt_Vertical = null;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, lookDir, Color.blue);

            PlayerManager.Instance.block_LookingAt_Vertical = null;
        }

        PlayerManager.Instance.lookingDirection = lookDir;
    }

    public void Action_isSwitchingBlocks_Invoke()
    {
        Action_isSwitchingBlocks?.Invoke();
    }
    public void Action_MadeFirstRaycast_Invoke()
    {
        Action_madeFirstRaycast?.Invoke();
    }
}
