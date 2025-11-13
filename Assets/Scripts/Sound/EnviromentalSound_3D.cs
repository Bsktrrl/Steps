using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnviromentalSound_3D : Singleton<EnviromentalSound_3D>
{
    [Header("Global Parameters")]
    [SerializeField] float sphereCastRadius = 5f;

    [Header("Sound Layer")]
    [SerializeField] LayerMask layer_EnviromentalSound;


    //-----


    [Header("Water Parameters")]
    [SerializeField] AudioSource audioSource_Water;
    GameObject blockSelected_Water;

    [Header("Waterfall Parameters")]
    [SerializeField] AudioSource audioSource_Waterfall;
    GameObject blockSelected_Waterfall;

    [Header("Swampwater Parameters")]
    [SerializeField] AudioSource audioSource_Swampwater;
    GameObject blockSelected_Swampwater;

    [Header("Swampwaterfall Parameters")]
    [SerializeField] AudioSource audioSource_Swampwaterfall;
    GameObject blockSelected_Swampwaterfall;

    [Header("Mud Parameters")]
    [SerializeField] AudioSource audioSource_Mud;
    GameObject blockSelected_Mud;

    [Header("Mudfall Parameters")]
    [SerializeField] AudioSource audioSource_Mudfall;
    GameObject blockSelected_Mudfall;

    [Header("Lava Parameters")]
    [SerializeField] AudioSource audioSource_Lava;
    GameObject blockSelected_Lava;

    [Header("Lavafall Parameters")]
    [SerializeField] AudioSource audioSource_Lavafall;
    GameObject blockSelected_Lavafall;

    [Header("Quicksand Parameters")]
    [SerializeField] AudioSource audioSource_Quicksand;
    GameObject blockSelected_Quicksand;

    [Header("Quicksandfall Parameters")]
    [SerializeField] AudioSource audioSource_Quicksandfall;
    GameObject blockSelected_Quicksandfall;

    [Header("IceCrack Parameters")]
    [SerializeField] AudioSource audioSource_IceCrack;
    GameObject blockSelected_IceCrack;

    [Header("Effect Blocks")]
    [SerializeField] AudioSource audioSource_EffectBlock_Checkpoint;
    GameObject blockSelected_EffectBlock_Checkpoint;
    [SerializeField] AudioSource audioSource_EffectBlock_RefillSteps;
    GameObject blockSelected_EffectBlock_RefillSteps;
    [SerializeField] AudioSource audioSource_EffectBlock_Teleporter;
    GameObject blockSelected_EffectBlock_Teleporter;
    [SerializeField] AudioSource audioSource_EffectBlock_Pusher;
    GameObject blockSelected_EffectBlock_Pusher;
    [SerializeField] AudioSource audioSource_EffectBlock_MushroomCircle;
    GameObject blockSelected_EffectBlock_MushroomCircle;

    //-----


    List<Collider> hits = new List<Collider>();

    Dictionary<SoundObjectType, List<GameObject>> blocksByType = new Dictionary<SoundObjectType, List<GameObject>>();
    Dictionary<SoundObjectType, GameObject> closestByType = new Dictionary<SoundObjectType, GameObject>();


    //--------------------


    void Update()
    {
        SoundSetup();
        SoundGroup();
    }


    //--------------------


    void SoundSetup()
    {
        hits.Clear();
        hits.AddRange(Physics.OverlapSphere(transform.position, sphereCastRadius, layer_EnviromentalSound));

        blocksByType.Clear();
        closestByType.Clear();

        foreach (var col in hits)
        {
            var so = col.GetComponent<SoundObject>();
            if (so == null) continue;

            var type = so.soundObjectType;
            if (!blocksByType.TryGetValue(type, out var list))
            {
                list = new List<GameObject>();
                blocksByType[type] = list;
            }
            list.Add(col.gameObject);

            // track closest for this type
            float d = Vector3.Distance(transform.position, col.transform.position);
            if (!closestByType.ContainsKey(type) || d < Vector3.Distance(transform.position, closestByType[type].transform.position))
            {
                closestByType[type] = col.gameObject;
            }
        }
    }
    void SoundGroup()
    {
        PlaySound(audioSource_Water, SoundObjectType.Water, ref blockSelected_Water);
        PlaySound(audioSource_Waterfall, SoundObjectType.Waterfall, ref blockSelected_Waterfall);

        PlaySound(audioSource_Swampwater, SoundObjectType.Swampwater, ref blockSelected_Swampwater);
        PlaySound(audioSource_Swampwaterfall, SoundObjectType.Swampwaterfall, ref blockSelected_Swampwaterfall);

        PlaySound(audioSource_Mud, SoundObjectType.Mud, ref blockSelected_Mud);
        PlaySound(audioSource_Mudfall, SoundObjectType.Mudfall, ref blockSelected_Mudfall);

        PlaySound(audioSource_Lava, SoundObjectType.Lava, ref blockSelected_Lava);
        PlaySound(audioSource_Lavafall, SoundObjectType.Lavafall, ref blockSelected_Lavafall);

        PlaySound(audioSource_Quicksand, SoundObjectType.Quicksand, ref blockSelected_Quicksand);
        PlaySound(audioSource_Quicksandfall, SoundObjectType.Quicksandfall, ref blockSelected_Quicksandfall);

        PlaySound(audioSource_IceCrack, SoundObjectType.IceCrack, ref blockSelected_IceCrack);

        PlaySound(audioSource_EffectBlock_Checkpoint, SoundObjectType.Checkpoint, ref blockSelected_EffectBlock_Checkpoint);
        PlaySound(audioSource_EffectBlock_RefillSteps, SoundObjectType.RefillSteps, ref blockSelected_EffectBlock_RefillSteps);
        PlaySound(audioSource_EffectBlock_Teleporter, SoundObjectType.Teleporter, ref blockSelected_EffectBlock_Teleporter);
        PlaySound(audioSource_EffectBlock_Pusher, SoundObjectType.Pusher, ref blockSelected_EffectBlock_Pusher);
        PlaySound(audioSource_EffectBlock_MushroomCircle, SoundObjectType.MushroomCircle, ref blockSelected_EffectBlock_MushroomCircle);
    }
   
    void PlaySound(AudioSource audioSource, SoundObjectType type, ref GameObject closestObjectOut)
    {
        if (!audioSource || type == SoundObjectType.None) return;

        if (blocksByType.TryGetValue(type, out var blocks) && blocks.Count > 0)
        {
            closestObjectOut = closestByType[type];

            float volume = CalculateVolumeValueFromClosestBlock(closestObjectOut);
            float pan = CalculateSterioPanValue(blocks);

            audioSource.volume = volume;
            audioSource.panStereo = pan;

            if (!audioSource.isPlaying) audioSource.Play();
        }
        else
        {
            closestObjectOut = null;
            if (audioSource.isPlaying) audioSource.Stop();
        }
    }
   
    float CalculateVolumeValueFromClosestBlock(GameObject closest)
    {
        if (!closest) return 0f;
        float d = Vector3.Distance(transform.position, closest.transform.position);
        return 1f - Mathf.Clamp01(d / sphereCastRadius);
    }
    float CalculateSterioPanValue(List<GameObject> blocks)
    {
        if (blocks == null || blocks.Count == 0) return 0f;

        // Use camera orientation so left/right matches what the player sees.
        Transform camT = CameraController.Instance?.cameraAnchor?.transform;
        Transform refT = camT != null ? camT : transform;

        float sum = 0f;
        float weightSum = 0f;

        foreach (var go in blocks)
        {
            if (!go) continue;

            Vector3 toObj = go.transform.position - refT.position;

            // horizontal only for pan
            Vector3 flat = Vector3.ProjectOnPlane(toObj, Vector3.up);
            if (flat.sqrMagnitude < 1e-6f) continue;

            float pan = Vector3.Dot(refT.right, flat.normalized);           // [-1, 1]
            float d = toObj.magnitude;
            float w = Mathf.Max(0f, 1f - (d / sphereCastRadius));         // same curve as volume

            sum += pan * w;
            weightSum += w;
        }

        if (weightSum <= 0f) return 0f;
        return Mathf.Clamp(sum / weightSum, -1f, 1f);
    }
}
public enum SoundObjectType
{
    None,

    Water,
    Waterfall,

    Swampwater,
    Swampwaterfall,

    Mud,
    Mudfall,

    Lava,
    Lavafall,

    Quicksand,
    Quicksandfall,

    IceCrack,

    Checkpoint,
    RefillSteps,
    Teleporter,
    Pusher,
    MushroomCircle
}
