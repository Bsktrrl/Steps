using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : Singleton<BlockManager>
{
    [SerializeField] int blockMovementCost_Grass = 1;
    [SerializeField] int blockMovementCost_Sand = 0;
    [SerializeField] int blockMovementCost_Stone = 2;
    [SerializeField] int blockMovementCost_Ice = 1;

    [SerializeField] int blockMovementCost_Water = 1;
    [SerializeField] int blockMovementCost_Lava = 2;

    public float materialDarkenAmount = 0.5f;


    //--------------------


    public int GetMovementCost(BlockElement blockElement)
    {
        switch (blockElement)
        {
            case BlockElement.None:
                return 0;

            case BlockElement.Grass:
                return blockMovementCost_Grass;
            case BlockElement.Sand:
                return blockMovementCost_Sand;
            case BlockElement.Stone:
                return blockMovementCost_Stone;
            case BlockElement.Ice:
                return blockMovementCost_Ice;

            case BlockElement.Water:
                return blockMovementCost_Water;
            case BlockElement.Lava:
                return blockMovementCost_Lava;

            default:
                return 1;
        }
    }
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
}
public enum BlockType
{
    None,

    Cube,
    Stair,
    Ladder,
}