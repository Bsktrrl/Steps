using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : Singleton<BlockManager>
{
    [Header("Material Darkening Value")]
    public float materialDarkeningValue = 0.5f;
}

public enum BlockElement
{
    None,

    Grass,
    Sand,
    Stone,
    Ice,

    Water,
    Lava,

    Brick,
    Candle,
    Cloud,
    Wood,
    Dirt,

    LavaCube,

    Metal,
    Coal,
    Ore,

    Bush,
    Sandstone,
    Snow,
    Rock,

    RefillSteps,
    Teleporter,

}
public enum BlockType
{
    None,

    Cube,
    Stair,
    Ladder,
    Chest,

    GetOnto_Cube, //A cube, but it can be entered even if it's at the same level as the player

    Fence,
}