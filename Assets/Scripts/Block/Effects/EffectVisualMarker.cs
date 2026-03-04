using UnityEngine;

public enum EffectVisualType
{
    SpawnPoint,
    RefillSteps,
    Pusher,
    Teleporter,
    Moveable,
    MushroomCircle
}

/// <summary>
/// Add this to the ROOT of each effect visual prefab (Smoke_Portal, Smoke_Checkpoint, etc).
/// </summary>
public class EffectVisualMarker : MonoBehaviour
{
    public EffectVisualType type;
}