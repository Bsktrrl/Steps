using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Block_Teleport : MonoBehaviour
{
    public static event Action Action_StartTeleport;
    public static event Action Action_EndTeleport;

    public GameObject newLandingSpot;
    GameObject newLandingSpot_Temp;
    public Color teleport_Color;
    Color teleport_Color_Temp;
    Color localTintAtLastSync;
    Color sharedTint;
    bool colorsInitialized;
    BlockInfo blockInfo;

    [Header("Material Rendering")]
    [SerializeField] List<MeshRenderer> objectRenderers = new List<MeshRenderer>();
    readonly List<MeshRenderer> foundRenderers = new List<MeshRenderer>();
    MaterialPropertyBlock propertyBlock;
    bool renderersDirty = true;
    static readonly int PortalColor = Shader.PropertyToID("_PortalColor");
    static readonly int BaseColor = Shader.PropertyToID("_BaseColor");


    //--------------------


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
        renderersDirty = true;
        Movement.Action_StepTaken_Late += TeleportPlayer;
        Movement.Action_LandedFromFalling += TeleportPlayer;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken_Late -= TeleportPlayer;
        Movement.Action_LandedFromFalling -= TeleportPlayer;
    }


    //--------------------


    public void SetupTeleporter()
    {
        renderersDirty = true;
        SetTeleportLinkColors();
    }

    private void OnTransformChildrenChanged()
    {
        renderersDirty = true;
    }

    private void OnValidate()
    {
        renderersDirty = true;
    }

    Color LocalTint
    {
        get
        {
            if (!blockInfo)
                TryGetComponent(out blockInfo);
            return blockInfo ? blockInfo.stepCostText_Color : Color.white;
        }
    }

    void SetTeleportLinkColors()
    {
        Color localTint = LocalTint;
        bool linkChanged = newLandingSpot_Temp != newLandingSpot;
        if (colorsInitialized && !linkChanged && teleport_Color_Temp == teleport_Color && localTintAtLastSync == localTint)
        {
            if (renderersDirty)
                ApplyIconColors();
            return;
        }

        SyncIconColors(teleport_Color, localTint);
        if (newLandingSpot && newLandingSpot != gameObject && newLandingSpot.TryGetComponent(out Block_Teleport linked))
        {
            if (linkChanged)
                linked.newLandingSpot = gameObject;
            if (linked.newLandingSpot == gameObject)
                linked.SyncIconColors(teleport_Color, localTint);
        }
    }

    void SyncIconColors(Color color, Color tint)
    {
        newLandingSpot_Temp = newLandingSpot;
        teleport_Color_Temp = teleport_Color = color;
        localTintAtLastSync = LocalTint;
        sharedTint = tint;
        colorsInitialized = true;
        ApplyIconColors();
    }

    void ApplyIconColors()
    {
        if (renderersDirty)
        {
            foundRenderers.Clear();
            foreach (MeshRenderer renderer in objectRenderers)
                if (renderer)
                    foundRenderers.Add(renderer);

            if (foundRenderers.Count == 0)
                foreach (var marker in GetComponentsInChildren<EffectBlock_Reference>(true))
                    if (marker.TryGetComponent(out MeshRenderer renderer))
                        foundRenderers.Add(renderer);

            renderersDirty = false;
        }

        propertyBlock ??= new MaterialPropertyBlock();
        foreach (MeshRenderer renderer in foundRenderers)
        {
            if (!renderer) continue;

            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(PortalColor, teleport_Color);
            propertyBlock.SetColor(BaseColor, sharedTint);
            renderer.SetPropertyBlock(propertyBlock);
        }
    }


    //--------------------


    void TeleportPlayer()
    {
        Movement movement = Movement.ExistingInstance;
        if (movement && movement.blockStandingOn == gameObject && newLandingSpot && newLandingSpot != gameObject &&
            newLandingSpot.TryGetComponent(out BlockInfo destination))
            movement.TryStartTeleport(destination, transform.position, PlayTeleportEffects, NotifyTeleportEnded);
    }

    void PlayTeleportEffects()
    {
        Action_StartTeleport?.Invoke();
        ActivatePortalsEffect();
    }

    static void NotifyTeleportEnded() => Action_EndTeleport?.Invoke();

    public void ActivatePortalsEffect()
    {
        if (GetComponentInChildren<PortalScript>())
            GetComponentInChildren<PortalScript>().ActivatePortalEffect();

        if (newLandingSpot && newLandingSpot.GetComponentInChildren<PortalScript>())
            newLandingSpot.GetComponentInChildren<PortalScript>().ActivatePortalEffect();
    }
    public void ActivateRootTeleportEffect()
    {
        ActivatePortalsEffect();

        // If your teleport sound is played inside PortalScript.ActivatePortalEffect(),
        // you do not need anything else here.
        //
        // If the teleport sound is played somewhere else for the player,
        // call that same sound method here too.
    }

}
