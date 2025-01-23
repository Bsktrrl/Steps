using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : Singleton<BlockManager>
{
    [Header("Material Darkening Value")]
    public float materialDarkeningValue = 0.5f;

    [Header("Darkening Text Colors")]
    public Color cheap_TextColor;
    public Color expensive_TextColor;
}

public enum BlockType
{
    None,

    Cube,
    Stair,
    Ladder,
    Chest,

    Fence,
    Slope,
}