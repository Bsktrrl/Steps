using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Movement : Singleton<Player_Movement>
{
    public static event Action Action_StepTaken;
    public static event Action Action_BodyRotated;
    public static event Action Action_resetBlockColor;

    [Header("Current Movement Cost")]
    public int currentMovementCost;

    [Header("Movement State")]
    public MovementStates movementStates;
    public ButtonsToPress lastMovementButtonPressed;

    [Header("Player Movement over Blocks")]
    [HideInInspector] public float heightOverBlock = 0.95f;
    public float fallSpeed = 6f;

    //Other
    Vector3 endDestination;
    public bool iceGliding;


    //--------------------


    private void Update()
    {
        KeyInputs();

        if (movementStates == MovementStates.Moving /*&& endDestination != (Vector3.zero + (Vector3.up * heightOverBlock))*/
            && !Player_SwiftSwim.Instance.isSwiftSwimming_Up && !Player_SwiftSwim.Instance.isSwiftSwimming_Down)
        {
            MovePlayer();
        }
        else if (Player_SwiftSwim.Instance.isSwiftSwimming_Up || Player_SwiftSwim.Instance.isSwiftSwimming_Down)
        {

        }
        else
        {
            movementStates = MovementStates.Still;
        }

        PlayerHover();
    }


    //--------------------


    private void OnEnable()
    {
        Action_StepTaken += IceGlide;
    }

    private void OnDisable()
    {
        Action_StepTaken -= IceGlide;
    }


    //--------------------


    void KeyInputs()
    {
        if (movementStates == MovementStates.Moving) { return; }
        if (PlayerManager.Instance.pauseGame) { return; }
        if (Cameras.Instance.isRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }

        //If pressing UP - Movement
        if (Input.GetKey(KeyCode.W))
        {
            lastMovementButtonPressed = ButtonsToPress.W;
            MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
        }

        //If pressing DOWN - Movement
        else if (Input.GetKey(KeyCode.S))
        {
            lastMovementButtonPressed = ButtonsToPress.S;
            MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
        }

        //If pressing LEFT - Movement
        else if (Input.GetKey(KeyCode.A))
        {
            lastMovementButtonPressed = ButtonsToPress.A;
            MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
        }

        //If pressing RIGHT - Movement
        else if (Input.GetKey(KeyCode.D))
        {
            lastMovementButtonPressed = ButtonsToPress.D;
            MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
        }


        //--------------------


        //If pressing - E - ASCEND
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (gameObject.GetComponent<Player_SwiftSwim>().canSwiftSwim_Up)
            {
                gameObject.GetComponent<Player_SwiftSwim>().SwiftSwim_Up();
            }
            else if (gameObject.GetComponent<Player_Ascend>().playerCanAscend)
            {
                gameObject.GetComponent<Player_Ascend>().Ascend();
            }
        }
        //If pressing - Q - DESCEND
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (gameObject.GetComponent<Player_SwiftSwim>().canSwiftSwim_Down)
            {
                gameObject.GetComponent<Player_SwiftSwim>().SwiftSwim_Down();
            }
            else if (gameObject.GetComponent<Player_Descend>().playerCanDescend)
            {
                gameObject.GetComponent<Player_Descend>().Descend();
            }
        }

        //If pressing - Dash - Space
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.GetComponent<Player_Dash>().playerCanDash)
            {
                gameObject.GetComponent<Player_Dash>().Dash();
            }
        }

        //If pressing - Hammer - Enter
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (gameObject.GetComponent<Player_Hammer>().playerCanHammer)
            {
                gameObject.GetComponent<Player_Hammer>().Hammer();
            }
        }

        //If pressing - Respawn
        else if (Input.GetKeyDown(KeyCode.R))
        {
            print("KeyCode.R is pressed");

            PlayerStats.Instance.RespawnPlayer();
        }

        //If pressing - Quit
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitLevel();
        }
    }
    void MovementKeyIsPressed(bool canMove, DetectedBlockInfo block_Vertical, int rotation)
    {
        if (PlayerStats.Instance.stats.steps_Current <= 0)
        { 
            PlayerStats.Instance.RespawnPlayer(); 
            return; 
        }

        if (canMove)
        {
            if (block_Vertical != null)
            {
                if (block_Vertical.block != null)
                {
                    if (block_Vertical.block.GetComponent<BlockInfo>())
                    {
                        if (block_Vertical.block.GetComponent<BlockInfo>().movementCost > PlayerStats.Instance.stats.steps_Current)
                        {
                            SetPlayerBodyRotation(rotation);
                            return;
                        }

                        PlayerManager.Instance.block_MovingTowards = block_Vertical;

                        //block_Vertical.block.GetComponent<BlockInfo>().movementCost = block_Vertical.block.GetComponent<BlockInfo>().movementCost;

                        endDestination = block_Vertical.blockPosition + (Vector3.up * heightOverBlock);
                        //SetPlayerBodyRotation(rotation);
                        movementStates = MovementStates.Moving;

                        Action_resetBlockColor?.Invoke();
                    }
                }
            }
        }

        SetPlayerBodyRotation(rotation);
    }
    public void SetPlayerBodyRotation(int rotationValue)
    {
        //Set new Rotation - Based on the key input
        switch (Cameras.Instance.cameraState)
        {
            case CameraState.Forward:
                PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 0 + rotationValue, 0));
                
                if (rotationValue == 0 || rotationValue == 360)
                    Cameras.Instance.directionFacing = Vector3.forward;
                else if (rotationValue == 180)
                    Cameras.Instance.directionFacing = Vector3.back;
                else if (rotationValue == 90)
                    Cameras.Instance.directionFacing = Vector3.right;
                else if (rotationValue == -90 || rotationValue == 270)
                    Cameras.Instance.directionFacing = Vector3.left;
                break;
            case CameraState.Backward:
                PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 180 + rotationValue, 0));
                
                if (180 + rotationValue == 0 || 180 + rotationValue == 360)
                    Cameras.Instance.directionFacing = Vector3.back;
                else if (180 + rotationValue == 180)
                    Cameras.Instance.directionFacing = Vector3.forward;
                else if (180 + rotationValue == 90)
                    Cameras.Instance.directionFacing = Vector3.left;
                else if (180 + rotationValue == -90 || 180 + rotationValue == 270)
                    Cameras.Instance.directionFacing = Vector3.right;
                break;
            case CameraState.Left:
                PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 90 + rotationValue, 0));
                
                if (90 + rotationValue == 0 || 90 + rotationValue == 360)
                    Cameras.Instance.directionFacing = Vector3.left;
                else if (90 + rotationValue == 180)
                    Cameras.Instance.directionFacing = Vector3.right;
                else if (90 + rotationValue == 90)
                    Cameras.Instance.directionFacing = Vector3.forward;
                else if (90 + rotationValue == -90 || 90 + rotationValue == 270)
                    Cameras.Instance.directionFacing = Vector3.back;
                break;
            case CameraState.Right:
                PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, Quaternion.Euler(0, -90 + rotationValue, 0));
                
                if (-90 + rotationValue == 0 || -90 + rotationValue == 360)
                    Cameras.Instance.directionFacing = Vector3.right;
                else if (-90 + rotationValue == 180 || -90 + rotationValue == -180)
                    Cameras.Instance.directionFacing = Vector3.left;
                else if (-90 + rotationValue == 90)
                    Cameras.Instance.directionFacing = Vector3.back;
                else if (-90 + rotationValue == -90 || -90 + rotationValue == 270)
                    Cameras.Instance.directionFacing = Vector3.forward;
                break;

            default:
                break;
        }

        Action_BodyRotated?.Invoke();
    }

    void MovePlayer()
    {
        //Move with a set speed
        if (PlayerManager.Instance.block_MovingTowards != null)
        {
            if (PlayerManager.Instance.block_MovingTowards.block != null)
            {
                //Check if the block standing on is different from the one entering, to move with what the player stands on
                if (PlayerManager.Instance.block_StandingOn_Current.block != PlayerManager.Instance.block_MovingTowards.block && PlayerManager.Instance.block_StandingOn_Current.block)
                {
                    if (PlayerManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed <= 0)
                        PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);
                    else
                    {
                        if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_IceGlide>() && PlayerStats.Instance.stats.abilitiesGot.IceSpikes)
                            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementSpeed / 2) * Time.deltaTime);
                        else
                            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementSpeed * Time.deltaTime);

                    }
                }
                else
                {
                    if (PlayerManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed <= 0)
                        PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);
                    else
                    {
                        if (PlayerManager.Instance.block_MovingTowards.block.GetComponent<Block_IceGlide>() && PlayerStats.Instance.stats.abilitiesGot.IceSpikes)
                            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, (PlayerManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed / 2) * Time.deltaTime);
                        else
                            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, PlayerManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed * Time.deltaTime);

                    }
                }
            }
            else
            {
                PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);
            }
        }
        else
        {
            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);
        }

        //Snap into place when close enough
        if (Vector3.Distance(PlayerManager.Instance.player.transform.position, endDestination) <= 0.03f)
        {
            PlayerManager.Instance.player.transform.position = endDestination;
            movementStates = MovementStates.Still;

            Action_StepTakenInvoke();
        }
    }
    void PlayerHover()
    {
        //Don't hover if teleporting
        if (PlayerManager.Instance.isTeleporting) { return; }

        //Don't fall if moving
        if (movementStates == MovementStates.Moving)
        {
            return;
        }

        //Fall if standing still and no block is under the player
        else if (movementStates == MovementStates.Still && !PlayerManager.Instance.block_StandingOn_Current.block)
        {
            gameObject.transform.position = gameObject.transform.position + (Vector3.down * fallSpeed * Time.deltaTime);
        }

        //Hover over blocks you're standing on
        else if (movementStates == MovementStates.Still && PlayerManager.Instance.block_StandingOn_Current.block)
        {
            gameObject.transform.position = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * heightOverBlock);
        }
    }

    //Begin Ice Gliding
    void IceGlide()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_IceGlide>() && !PlayerStats.Instance.stats.abilitiesGot.IceSpikes)
            {
                iceGliding = true;
                PlayerStats.Instance.stats.steps_Current += PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;

                switch (lastMovementButtonPressed)
                {
                    case ButtonsToPress.W:
                        if (PlayerManager.Instance.canMove_Forward)
                            MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
                        break;
                    case ButtonsToPress.S:
                        if (PlayerManager.Instance.canMove_Back)
                            MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
                        break;
                    case ButtonsToPress.A:
                        if (PlayerManager.Instance.canMove_Left)
                            MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                        break;
                    case ButtonsToPress.D:
                        if (PlayerManager.Instance.canMove_Right)
                            MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                iceGliding = false;
            }
        }
        else
        {
            iceGliding = false;
        }
    }


    //--------------------


    public void Action_StepTakenInvoke()
    {
        Action_StepTaken?.Invoke();
    }
    public void Action_ResetBlockColorInvoke()
    {
        Action_resetBlockColor?.Invoke();
    }


    //--------------------


    public void QuitLevel()
    {
        if (!string.IsNullOrEmpty("MainMenu"))
        {
            StartCoroutine(LoadSceneCoroutine("MainMenu"));
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            Debug.Log($"Loading progress: {operation.progress * 100}%");
            yield return null;
        }
    }
}

public enum MovementStates
{
    Still,
    Moving
}
public enum ButtonsToPress
{
    None,

    W,
    S,
    A,
    D,

    Arrow_Left,
    ArrowRight,

    Space,
    X,

}
public enum MovementState
{
    Still,

    Moving
}

public enum MovementDirection
{
    None,

    Forward,
    Backward,
    Left,
    Right
}

public enum DetectorPoint
{
    Center,

    Front,
    Back,
    Right,
    Left
}