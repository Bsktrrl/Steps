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
        if (PlayerManager.Instance.block_LookingAt_Horizontal)
        {
            if (PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<Block_Weak>())
            {
                playerCanHammer = true;
            }
        }
    }

    public void Hammer()
    {
        if (PlayerManager.Instance.block_LookingAt_Horizontal && PlayerStats.Instance.stats.abilitiesGot.Hammer)
        {
            if (PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<Block_Weak>())
            {
                PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<Block_Weak>().DestroyWeakBlock();
            }
        }
    }
}
