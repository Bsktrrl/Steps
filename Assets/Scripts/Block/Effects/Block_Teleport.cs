using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class Block_Teleport : MonoBehaviour
{
    public static event Action Action_StartTeleport;
    public static event Action Action_EndTeleport;

    public GameObject newLandingSpot;
    GameObject newLandingSpot_Temp;
    public Color teleport_Color;
    Color teleport_Color_Temp;

    EffectBlockManager effectBlockManager;

    [Header("Material Rendering")]
    [SerializeField] List<MeshRenderer> objectRenderers = new List<MeshRenderer>();
    [HideInInspector] public List<MaterialPropertyBlock> propertyBlocks = new List<MaterialPropertyBlock>();


    //--------------------


    private void Awake()
    {
        effectBlockManager = FindObjectOfType<EffectBlockManager>();
    }

    private void Start()
    {
        SetupTeleporter();
    }

    private void Update()
    {
        SetTeleportLinkColors();
    }


    //--------------------


    private void OnEnable()
    {
        Movement.Action_StepTaken += TeleportPlayer;
        Action_StartTeleport += StartTeleport_Action;
        Action_EndTeleport += EndTeleport_Action;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= TeleportPlayer;
        Action_StartTeleport -= StartTeleport_Action;
        Action_EndTeleport -= EndTeleport_Action;
    }


    //--------------------


    public void SetupTeleporter()
    {
        SetObjectRendererRecursively(transform);
        SetPropertyBlock();

        SetIconColor(gameObject, teleport_Color);
    }


    //--------------------


    void SetObjectRendererRecursively(Transform parent)
    {
        if (objectRenderers.Count > 0) { return; }

        foreach (Transform child in parent)
        {
            //Check if this child has the component
            GameObject block = child.gameObject;

            if (block != null)
            {
                if (block.GetComponent<EffectBlock_Reference>())
                {
                    objectRenderers.Add(block.GetComponent<MeshRenderer>());
                }
            }

            //Recurse into this child
            SetObjectRendererRecursively(child);
        }
    }
    void SetPropertyBlock()
    {
        // Initialize property blocks and get original colors
        for (int i = 0; i < objectRenderers.Count; i++)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            objectRenderers[i].GetPropertyBlock(block);
            propertyBlocks.Add(block);
        }
    }


    //--------------------


    void SetTeleportLinkColors()
    {
        if (newLandingSpot)
        {
            //Check if Teleporter has been linked
            if (newLandingSpot_Temp != newLandingSpot)
            {
                newLandingSpot_Temp = newLandingSpot;

                if (newLandingSpot.GetComponent<Block_Teleport>())
                {
                    newLandingSpot.GetComponent<Block_Teleport>().newLandingSpot = gameObject;
                    newLandingSpot.GetComponent<Block_Teleport>().teleport_Color = teleport_Color;

                    SetIconColor(newLandingSpot, teleport_Color);
                }
            }

            //Check if teleportImageColor has changed
            if (teleport_Color_Temp != teleport_Color)
            {
                teleport_Color_Temp = teleport_Color;

                if (newLandingSpot.GetComponent<Block_Teleport>())
                {
                    newLandingSpot.GetComponent<Block_Teleport>().teleport_Color = teleport_Color;

                    SetIconColor(newLandingSpot, teleport_Color);
                }
            }
        }
        else
        {
            SetIconColor(gameObject, teleport_Color);
        }
    }
    void SetIconColor(GameObject obj, Color color)
    {
        if (!obj.GetComponent<Block_Teleport>()) { return; }

        for (int i = 0; i < obj.GetComponent<Block_Teleport>().propertyBlocks.Count; i++)
        {
            // Set the original color in the MaterialPropertyBlock
            obj.GetComponent<Block_Teleport>().propertyBlocks[i].SetColor("_BaseColor", color);

            // Apply the MaterialPropertyBlock to the renderer
            obj.GetComponent<Block_Teleport>().objectRenderers[i].SetPropertyBlock(propertyBlocks[i]);
        }
    }


    //--------------------


    void TeleportPlayer()
    {
        if (Movement.Instance.blockStandingOn == gameObject && newLandingSpot)
        {
            Vector3 movementDelta = transform.position - Movement.Instance.previousPosition;
            Vector3 horizontalDirection = new Vector3(movementDelta.x, 0, movementDelta.z);
            Movement.Instance.teleportMovementDir = Movement.Instance.GetMovingDirection(horizontalDirection);

            StartCoroutine(TeleportWait(0.005f));
        }
    }

    IEnumerator TeleportWait(float waitTime)
    {
        int stepTemp = PlayerStats.Instance.stats.steps_Current;

        //PlayerManager.Instance.isTransportingPlayer = true;
        PlayerManager.Instance.pauseGame = true;
        Movement.Instance.SetMovementState(MovementStates.Moving);

        Action_StartTeleport?.Invoke();

        yield return new WaitForSeconds(waitTime);

        Vector3 newPos = gameObject.GetComponent<Block_Teleport>().newLandingSpot.transform.position;
        PlayerManager.Instance.player.transform.position = new Vector3(newPos.x, newPos.y + PlayerManager.Instance.player.GetComponent<Movement>().heightOverBlock, newPos.z);

        yield return new WaitForSeconds(waitTime);

        Movement.Instance.SetMovementState(MovementStates.Still);
        //PlayerManager.Instance.isTransportingPlayer = false;
        PlayerManager.Instance.pauseGame = false;

        PlayerStats.Instance.stats.steps_Current = stepTemp - gameObject.GetComponent<Block_Teleport>().newLandingSpot.GetComponent<BlockInfo>().movementCost;

        yield return new WaitForSeconds(waitTime);

        Movement.Instance.UpdateBlockStandingOn();

        Movement.Instance.Action_StepTaken_Invoke();
        
        Action_EndTeleport?.Invoke();

        Movement.Instance.IceGlideMovement(true);
    }

    void StartTeleport_Action()
    {
        Movement.Action_StepTaken -= TeleportPlayer;
    }
    void EndTeleport_Action()
    {
        Movement.Action_StepTaken += TeleportPlayer;
    }
}
