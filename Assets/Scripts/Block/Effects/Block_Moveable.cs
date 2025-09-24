using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Block_Moveable : MonoBehaviour
{
    MovementDirection movementDirection;

    /*[HideInInspector]*/ public bool canMove;
    bool isMoving;
    bool isIceGliding;

    RaycastHit hit;
    Vector3 startPos;
    Vector3 savePos;


    //--------------------


    private void Start()
    {
        startPos = transform.position;
        savePos = transform.position;
    }
    private void Update()
    {
        if (isMoving)
        {
            if (isIceGliding)
                PerformMovement(10);
            else
                PerformMovement(gameObject.GetComponent<BlockInfo>().movementSpeed);
        }
    }

    private void OnEnable()
    {
        Movement.Action_StepTaken += RaycastForThePlayer;
        Movement.Action_BodyRotated += RaycastForThePlayer;
        Movement.Action_RespawnPlayer += ResetBlockPos;
        Movement.Action_LandedFromFalling += RaycastForThePlayer;
    }
    private void OnDisable()
    {
        Movement.Action_StepTaken -= RaycastForThePlayer;
        Movement.Action_BodyRotated -= RaycastForThePlayer;
        Movement.Action_RespawnPlayer -= ResetBlockPos;
        Movement.Action_LandedFromFalling -= RaycastForThePlayer;
    }


    //--------------------


    void RaycastForThePlayer()
    {
        movementDirection = MovementDirection.None;

        PerformRaycast_Player(Vector3.forward);
        PerformRaycast_Player(Vector3.back);
        PerformRaycast_Player(Vector3.left);
        PerformRaycast_Player(Vector3.right);

        PerformRaycast_Horizontal();

        PerformRaycast_Vertical();
    }
    void PerformRaycast_Player(Vector3 direction)
    {
        if (movementDirection != MovementDirection.None) { return; }

        if (Physics.Raycast(gameObject.transform.position, direction, out hit, 1, MapManager.Instance.pickup_LayerMask))
        {
            Debug.DrawRay(gameObject.transform.position, direction * hit.distance, Color.green);

            if (hit.transform.gameObject == PlayerManager.Instance.player)
            {
                if (direction == Vector3.forward)
                    movementDirection = MovementDirection.Backward;
                else if (direction == Vector3.back)
                    movementDirection = MovementDirection.Forward;
                else if (direction == Vector3.left)
                    movementDirection = MovementDirection.Right;
                else if (direction == Vector3.right)
                    movementDirection = MovementDirection.Left;
            }
            else
            {
                movementDirection = MovementDirection.None;
            }
        }
        else
        {
            movementDirection = MovementDirection.None;
        }
    }
    void PerformRaycast_Horizontal()
    {
        if (movementDirection == MovementDirection.None)
            canMove = false;
        else if (movementDirection == MovementDirection.Forward)
            Raycast_Horizontal(Vector3.forward);
        else if (movementDirection == MovementDirection.Backward)
            Raycast_Horizontal(Vector3.back);
        else if (movementDirection == MovementDirection.Left)
            Raycast_Horizontal(Vector3.left);
        else if (movementDirection == MovementDirection.Right)
            Raycast_Horizontal(Vector3.right);
    }
    void Raycast_Horizontal(Vector3 direction)
    {
        if (Physics.Raycast(gameObject.transform.position, direction, out hit, 1, MapManager.Instance.pickup_LayerMask))
        {
            Debug.DrawRay(gameObject.transform.position, direction, Color.green);

            if (hit.transform.gameObject)
            {
                canMove = false;
            }
            else
            {
                if (PlayerManager.Instance.block_LookingAt_Horizontal == gameObject && !isIceGliding)
                {
                    canMove = true;
                }
                else if (isIceGliding)
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }
            }
        }
        else
        {
            if (PlayerManager.Instance.block_LookingAt_Horizontal == gameObject && !isIceGliding)
            {
                canMove = true;
            }
            else if (isIceGliding)
            {
                canMove = true;
            }
            else
            {
                canMove = false;
            }
        }
    }

    void PerformRaycast_Vertical()
    {
        if (!canMove) { return; }

        if (movementDirection == MovementDirection.None)
            canMove = false;
        else if (movementDirection == MovementDirection.Forward)
            Raycast_Vertical(Vector3.forward);
        else if (movementDirection == MovementDirection.Backward)
            Raycast_Vertical(Vector3.back);
        else if (movementDirection == MovementDirection.Left)
            Raycast_Vertical(Vector3.left);
        else if (movementDirection == MovementDirection.Right)
            Raycast_Vertical(Vector3.right);
    }
    void Raycast_Vertical(Vector3 direction)
    {
        if (Physics.Raycast(gameObject.transform.position + direction, Vector3.down, out hit, 1, MapManager.Instance.pickup_LayerMask))
        {
            Debug.DrawRay(gameObject.transform.position + direction, Vector3.down, Color.green);

            if (hit.transform.gameObject)
            {
                if (hit.transform.gameObject.GetComponent<BlockInfo>().blockType == BlockType.Cube)
                {
                    if (PlayerManager.Instance.block_LookingAt_Horizontal == gameObject && !isIceGliding)
                    {
                        canMove = true;
                    }
                    else if (isIceGliding)
                    {
                        canMove = true;
                    }
                    else
                    {
                        canMove = false;
                    }

                    if (hit.transform.gameObject.GetComponent<Block_IceGlide>())
                    {
                        isIceGliding = true;
                    }
                    else
                    {
                        isIceGliding = false;
                    }

                    return;
                }
            }
        }

        canMove = false;
    }

    void ActivateBlockMovement()
    {
        if (movementDirection == MovementDirection.None) { return; }
        if (!canMove) { return; }

        //if (movementDirection == MovementDirection.Forward && PlayerManager.Instance.lookingDirection != Vector3.forward) { return; }
        //if (movementDirection == MovementDirection.Backward && PlayerManager.Instance.lookingDirection != Vector3.back) { return; }
        //if (movementDirection == MovementDirection.Left && PlayerManager.Instance.lookingDirection != Vector3.left) { return; }
        //if (movementDirection == MovementDirection.Right && PlayerManager.Instance.lookingDirection != Vector3.right) { return; }

        //PlayerManager.Instance.isTransportingPlayer = true;
        isMoving = true;
    }
    void PerformMovement(float movementSpeed)
    {
        Vector3 dirTemp = Vector3.zero;

        if (movementDirection == MovementDirection.Forward)
            dirTemp = Vector3.forward;
        else if (movementDirection == MovementDirection.Backward)
            dirTemp = Vector3.back;
        else if (movementDirection == MovementDirection.Left)
            dirTemp = Vector3.left;
        else if (movementDirection == MovementDirection.Right)
            dirTemp = Vector3.right;

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, savePos + dirTemp, movementSpeed * Time.deltaTime);
        
        //Snap into place when close enough
        if (Vector3.Distance(gameObject.transform.position, savePos + dirTemp) <= 0.03f)
        {
            gameObject.transform.position = savePos + dirTemp;
            savePos = transform.position;

            if (!IceGlide())
            {
                isMoving = false;
                isIceGliding = false;
               // PlayerManager.Instance.isTransportingPlayer = false;

                RaycastForThePlayer();
            }
        }
    }

    bool IceGlide()
    {
        PerformRaycast_Horizontal();
        PerformRaycast_Vertical();

        if (movementDirection == MovementDirection.None) { return false; }
        if (!canMove) { return false; }

        //Raycast the block under to see if there is an IceBlock there
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 1, MapManager.Instance.pickup_LayerMask))
        {
            Debug.DrawRay(gameObject.transform.position, Vector3.down, Color.green);

            if (hit.transform.gameObject)
            {
                if (hit.transform.gameObject.GetComponent<Block_IceGlide>())
                {
                    isIceGliding = true;
                    return true;
                }
                else
                {
                    canMove = false;
                }
            }
            else
            {
                canMove = false;
            }
        }
        else
        {
            canMove = false;
        }

        return false;
    }

    void ResetBlockPos()
    {
        gameObject.transform.position = startPos;

        savePos = startPos;
    }
}