using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hammer : Singleton<Player_Hammer>
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
        if (MainManager.Instance.block_LookingAt_Horizontal)
        {
            if (MainManager.Instance.block_LookingAt_Horizontal.GetComponent<Block_Weak>())
            {
                playerCanHammer = true;
            }
        }
    }

    public void Hammer()
    {
        if (MainManager.Instance.block_LookingAt_Horizontal && Player_Stats.Instance.stats.abilities.Hammer)
        {
            if (MainManager.Instance.block_LookingAt_Horizontal.GetComponent<Block_Weak>())
            {
                MainManager.Instance.block_LookingAt_Horizontal.GetComponent<Block_Weak>().DestroyWeakBlock();
            }
        }
    }
}
