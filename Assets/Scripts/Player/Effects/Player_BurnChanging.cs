using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BurnChanging : Singleton<Player_BurnChanging>
{
    [SerializeField] List<GameObject> meltedBlocksList = new List<GameObject>();


    //--------------------


    private void OnEnable()
    {
        PlayerStats.Action_RespawnPlayer += RemoveMeltedBlocks;
        PlayerStats.Action_RespawnPlayerLate += RemoveMeltedBlocks;
        meltedBlocksList.Clear();
    }

    private void OnDisable()
    {
        PlayerStats.Action_RespawnPlayer -= RemoveMeltedBlocks;
        PlayerStats.Action_RespawnPlayerLate -= RemoveMeltedBlocks;
        meltedBlocksList.Clear();
    }


    //--------------------


    public void AddMeltedBlockToList(GameObject obj)
    {
        meltedBlocksList.Add(obj);
    }

    void RemoveMeltedBlocks()
    {
        foreach (var block in meltedBlocksList)
        {
            if (block != null)
            {
                Destroy(block);
            }
        }

        meltedBlocksList.Clear();
    }
}
