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

}
public enum BlockType
{
    None,

    Cube,
    Stair,
    Ladder,
    Chest,
}