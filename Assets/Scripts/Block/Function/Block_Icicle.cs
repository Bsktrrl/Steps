using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Icicle : MonoBehaviour
{
    [Header("Falling Parameters")]
    [SerializeField] float endPosHeightOverBlock = 1;
    float raycastDistance_Down = 4.5f;
    float waitTime_BeforeFalling = 0.3f;
    float waitCounter = 0;

    [Header("Raycast")]
    RaycastHit hit;

    [Header("Ending Position")]
    Vector3 startPos;
    Vector3 endPos;
    GameObject endBlock;

    [Header("FallingAnimation")]
    float shakingIntensity = 3;
    float shakingSpeed = 50;
    List<GameObject> LOD_ObjectsList;
    Quaternion objectInitialRotation;

    [SerializeField] bool isDetectingPlayer;
    [SerializeField] bool hasFallen;


    //--------------------


    private void Awake()
    {
        startPos = transform.position;
    }
    private void Start()
    {
        GetLODObjects();
        GetEndPos();
    }
    private void Update()
    {
        if (CheckIfReadyToFall() && !hasFallen)
        {
            Falling();
        }
    }


    //--------------------


    private void OnEnable()
    {
        PlayerStats.Action_RespawnPlayer += ResetBlock;
    }

    private void OnDisable()
    {
        PlayerStats.Action_RespawnPlayer -= ResetBlock;
    }


    //--------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Player"))
        {
            StartCoroutine(WaitBeforeRespean(0.25f));
        }
    }
    IEnumerator WaitBeforeRespean(float waitTime)
    {
        PlayerManager.Instance.pauseGame = true;

        yield return new WaitForSeconds(waitTime);

        PlayerManager.Instance.pauseGame = false;

        PlayerStats.Instance.RespawnPlayer();
    }


    //--------------------


    void GetLODObjects()
    {
        LOD_ObjectsList = new List<GameObject>();

        // Find all child GameObjects of the parent that have the LoadLevel script
        foreach (Transform child in gameObject.transform)
        {
            // Check if the child has the LoadLevel script
            if (child.GetComponent<MeshRenderer>() != null)
            {
                // Add the child GameObject to the list
                LOD_ObjectsList.Add(child.gameObject);
            }

            objectInitialRotation = child.transform.rotation;
        }
    }

    void GetEndPos()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance_Down + 1, MapManager.Instance.pickup_LayerMask))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                endPos = hit.transform.position + (Vector3.up * endPosHeightOverBlock);
                endBlock = hit.transform.gameObject;
            }
        }
    }


    //--------------------


    bool CheckIfReadyToFall()
    {
        if (isDetectingPlayer)
        {
            FallingAlertAnimation();

            if (waitCounter < waitTime_BeforeFalling)
                waitCounter += Time.deltaTime;

            if (waitCounter >= waitTime_BeforeFalling)
            {
                return true;
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance_Down, MapManager.Instance.pickup_LayerMask))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    isDetectingPlayer = true;

                    FallingAlertAnimation();

                    if (waitCounter < waitTime_BeforeFalling)
                        waitCounter += Time.deltaTime;

                    if (waitCounter >= waitTime_BeforeFalling)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    void FallingAlertAnimation()
    {
        //When falling, straighten up the rotation from the shaking
        if (waitCounter >= waitTime_BeforeFalling)
        {
            for (int i = 0; i < LOD_ObjectsList.Count; i++)
            {
                LOD_ObjectsList[i].transform.SetPositionAndRotation(LOD_ObjectsList[i].transform.position, objectInitialRotation);
            }

            return;
        }

        //Shake the block
        if (LOD_ObjectsList.Count > 0)
        {
            for (int i = 0; i < LOD_ObjectsList.Count; i++)
            {
                float shakeValue = Mathf.Sin(Time.time * shakingSpeed) * shakingIntensity;
                Vector3 currentRotation = transform.localEulerAngles;
                currentRotation.x = objectInitialRotation.x + shakeValue;

                LOD_ObjectsList[i].transform.localEulerAngles = currentRotation;
            }
        }
    }
    void Falling()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPos, PlayerManager.Instance.player.GetComponent<Player_Movement>().fallSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPos) <= 0.03f)
        {
            gameObject.transform.position = endPos;

            hasFallen = true;

            Player_BlockDetector.Instance.RaycastSetup();

            if (endBlock)
            {
                if (endBlock.GetComponent<BlockInfo>())
                {
                    endBlock.GetComponent<BlockInfo>().ResetDarkenColor();
                }
            }
        }
    }


    //--------------------


    void HideBlock()
    {
        gameObject.SetActive(false);
    }
    public void ShowBlock()
    {
        gameObject.SetActive(true);
    }


    //--------------------


    public void ResetBlock()
    {
        HideBlock();

        waitCounter = 0;

        transform.position = startPos;

        isDetectingPlayer = false;
        hasFallen = false;

        ShowBlock();
    }
}
