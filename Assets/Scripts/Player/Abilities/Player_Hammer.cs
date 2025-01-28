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
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        CheckIfBlockCanBeHammered();
    }


    //--------------------


    void CheckIfBlockCanBeHammered()
    {
        if (PlayerManager.Instance.block_LookingAt_Horizontal)
        {
            if (PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<Block_Weak>()
                && (PlayerStats.Instance.stats.abilitiesGot_Temporary.Hammer || PlayerStats.Instance.stats.abilitiesGot_Permanent.Hammer))
            {
                playerCanHammer = true;
                return;
            }
        }

        playerCanHammer = false;
    }

    public void Hammer()
    {
        if (PlayerManager.Instance.block_LookingAt_Horizontal && (PlayerStats.Instance.stats.abilitiesGot_Permanent.Hammer || PlayerStats.Instance.stats.abilitiesGot_Temporary.Hammer))
        {
            if (PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<Block_Weak>())
            {
                PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<Block_Weak>().DestroyWeakBlock();
            }
        }
    }
}
