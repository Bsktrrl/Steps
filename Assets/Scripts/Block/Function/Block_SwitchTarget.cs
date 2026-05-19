using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Block_SwitchTarget : MonoBehaviour
{
    [Header("Movement Info")]
    public MoveDirection movementDirection;
    public int movementDistance = 1;
    public float movementSpeed = 2f;

    [Header("Sound")]
    public AudioSource audioSource;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Vector3 currentTargetPosition;

    private bool isMoving;
    private bool hasInitialized;

    private Block_Switch_Manager block_Switch_Manager;


    //--------------------


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.minDistance = 0;
        audioSource.maxDistance = 12;

        audioSource.rolloffMode = AudioRolloffMode.Linear;

        closedPosition = transform.position;
        openPosition = closedPosition + GetMovementVector() * movementDistance;

        currentTargetPosition = closedPosition;
        hasInitialized = true;
    }

    private void Start()
    {
        block_Switch_Manager = FindAnyObjectByType<Block_Switch_Manager>();
    }

    private void Update()
    {
        if (!hasInitialized)
            return;

        if (!isMoving)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTargetPosition,
            movementSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, currentTargetPosition) <= 0.01f)
        {
            transform.position = currentTargetPosition;
            isMoving = false;
        }
    }


    //--------------------


    public void MoveTarget(bool sound)
    {
        MoveToOpenPosition(sound);
    }

    public void MoveTargetBack(bool sound)
    {
        MoveToClosedPosition(sound);
    }

    public void MoveToOpenPosition(bool sound)
    {
        SetTargetPosition(openPosition, sound);
    }

    public void MoveToClosedPosition(bool sound)
    {
        SetTargetPosition(closedPosition, sound);
    }

    private void SetTargetPosition(Vector3 newTargetPosition, bool sound)
    {
        currentTargetPosition = newTargetPosition;

        if (Vector3.Distance(transform.position, currentTargetPosition) <= 0.01f)
        {
            transform.position = currentTargetPosition;
            isMoving = false;

            //If ladder, setup the ladders again
            if (GetComponent<Block_Ladder>())
                GetComponent<Block_Ladder>().SetupLadder();

            return;
        }

        isMoving = true;

        if (sound)
        {
            PlayMovingSound();
        }
    }

    private Vector3 GetMovementVector()
    {
        switch (movementDirection)
        {
            case MoveDirection.Forward:
                return Vector3.forward;

            case MoveDirection.Backward:
                return Vector3.back;

            case MoveDirection.Right:
                return Vector3.right;

            case MoveDirection.Left:
                return Vector3.left;

            case MoveDirection.Up:
                return Vector3.up;

            case MoveDirection.Down:
                return Vector3.down;

            default:
                return Vector3.zero;
        }
    }


    //--------------------


    private void PlayMovingSound()
    {
        if (block_Switch_Manager == null)
            return;

        audioSource.clip = block_Switch_Manager.fence_Sound;

        if (block_Switch_Manager.fence_Sound == null)
            return;

        audioSource.PlayOneShot(block_Switch_Manager.fence_Sound);
    }
}