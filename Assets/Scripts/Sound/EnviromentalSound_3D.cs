using System.Collections.Generic;
using UnityEngine;

public class EnviromentalSound_3D : Singleton<EnviromentalSound_3D>
{
    [Header("Global Parameters")]
    [SerializeField] float sphereCastRadius = 5f;

    [Header("Sound Layer")]
    [SerializeField] LayerMask layer_EnviromentalSound;

    [Header("Water Parameters")]
    [SerializeField] AudioSource audioSource_Water;
    [SerializeField] GameObject blockSelected_Water; // closest, for debugging/inspector

    [Header("Waterfall Parameters")]
    [SerializeField] AudioSource audioSource_Waterfall;
    [SerializeField] GameObject blockSelected_Waterfall;

    void Update()
    {
        SoundSetup(audioSource_Water, SoundObjectType.Water, ref blockSelected_Water);
        SoundSetup(audioSource_Waterfall, SoundObjectType.Waterfall, ref blockSelected_Waterfall);
    }

    void SoundSetup(AudioSource audioSource, SoundObjectType soundObjectType, ref GameObject closestObjectOut)
    {
        var blocks = FindBlocks(soundObjectType, out GameObject closest);

        if (blocks.Count > 0)
        {
            closestObjectOut = closest;

            float volume = CalculateVolumeFromClosest(closest);
            float pan = CalculateAggregatePan(blocks);  // <- NEW: pan from all blocks

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

    // Collect all matching blocks inside the sphere; also return the closest one.
    List<GameObject> FindBlocks(SoundObjectType type, out GameObject closest)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, sphereCastRadius, layer_EnviromentalSound);

        float closestDist = Mathf.Infinity;
        closest = null;

        var list = new List<GameObject>(hits.Length);
        foreach (var col in hits)
        {
            var so = col.GetComponent<SoundObject>();
            if (so != null && so.soundObjectType == type)
            {
                list.Add(col.gameObject);

                float d = Vector3.Distance(transform.position, col.transform.position);
                if (d < closestDist)
                {
                    closestDist = d;
                    closest = col.gameObject;
                }
            }
        }

        return list;
    }

    // Volume from the single closest object
    float CalculateVolumeFromClosest(GameObject closest)
    {
        if (!closest) return 0f;
        float d = Vector3.Distance(transform.position, closest.transform.position);
        return 1f - Mathf.Clamp01(d / sphereCastRadius);
    }

    // Pan from ALL blocks: weighted average of each block's pan using the same distance→volume weight.
    float CalculateAggregatePan(List<GameObject> blocks)
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
