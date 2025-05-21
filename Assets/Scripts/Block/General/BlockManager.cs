using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockManager : Singleton<BlockManager>
{
    [Header("Material Darkening Value")]
    public float materialDarkeningValue = 0.25f;

    [Header("Darkening Text Colors")]
    public Color cheap_TextColor;
    public Color expensive_TextColor;
}

public enum BlockType
{
    None,

    Cube,
    Slab,
    Stair,
    Slope,

    Fence,
}

public enum BlockElement
{
    None,

    Grass,
    Wood,
    Sand,
    Sandstone,

    Stone,
    Lava,
    Quicksand,
    Mud,
    SwampWater,

    Rock,
    Clay,
    Brick,
    SwampGrass,
    ColdWood,

    Brass,
    Gold,
    Metal,

    Wood_SwampWater,
    Wood_Jungle,

    Ice,
    Snow,
    Ice_Grass
}