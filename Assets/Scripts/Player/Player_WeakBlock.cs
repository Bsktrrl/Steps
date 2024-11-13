using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WeakBlock : Singleton<Player_WeakBlock>
{
    public GameObject blockCrack;
    public bool playerCanHammer;


    //--------------------


    private void Update()
    {
        CheckIfBlockCanBeHammered();
    }


    //--------------------


    void CheckIfBlockCanBeHammered()
    {
        if (MainManager.Instance.block_LookingAt)
        {
            if (MainManager.Instance.block_LookingAt.GetComponent<Block_Weak>())
            {
                playerCanHammer = true;
            }
        }
    }

    public void Hammer()
    {
        if (MainManager.Instance.block_LookingAt && Player_Stats.Instance.stats.abilities.Hammer)
        {
            if (MainManager.Instance.block_LookingAt.GetComponent<Block_Weak>())
            {
                MainManager.Instance.block_LookingAt.GetComponent<Block_Weak>().DestroyWeakBlock();
            }
        }
    }
}
