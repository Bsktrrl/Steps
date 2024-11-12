using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Descend : MonoBehaviour
{
    [Header("Descending")]
    public bool playerCanDescend;
    public GameObject descendingBlock_Previous;
    public GameObject descendingBlock_Current;
    public float descendingDistance = 4;

    RaycastHit hit;


    //--------------------


    private void Update()
    {
        if (RaycastForDescending())
        {
            playerCanDescend = true;
        }
        else
        {
            playerCanDescend = false;
        }
    }


    //--------------------


    bool RaycastForDescending()
    {
        if (gameObject.GetComponent<Player_Stats>().stats.abilities.Descend)
        {
            if (Physics.Raycast(transform.position + Vector3.down, Vector3.down, out hit, descendingDistance))
            {
                Debug.DrawRay(transform.position + Vector3.down, Vector3.down * descendingDistance, Color.yellow);

                if (hit.transform.GetComponent<BlockInfo>())
                {
                    if (!hit.transform.GetComponent<BlockInfo>().upper_Center)
                    {
                        descendingBlock_Previous = descendingBlock_Current;
                        descendingBlock_Current = hit.transform.gameObject;

                        if (descendingBlock_Current != descendingBlock_Previous)
                        {
                            if (descendingBlock_Previous)
                            {
                                if (descendingBlock_Previous.GetComponent<BlockInfo>())
                                {
                                    descendingBlock_Previous.GetComponent<BlockInfo>().ResetColor();
                                }
                            }
                        }

                        descendingBlock_Current.GetComponent<BlockInfo>().DarkenColors();

                        return true;
                    }
                }
            }

            if (descendingBlock_Current)
            {
                if (descendingBlock_Current.GetComponent<BlockInfo>())
                {
                    descendingBlock_Current.GetComponent<BlockInfo>().ResetColor();
                    descendingBlock_Current = null;
                }
            }
            if (descendingBlock_Previous)
            {
                if (descendingBlock_Previous.GetComponent<BlockInfo>())
                {
                    descendingBlock_Previous.GetComponent<BlockInfo>().ResetColor();
                    descendingBlock_Previous = null;
                }
            }
        }

        return false;
    }

    public void Descend()
    {
        if (gameObject.GetComponent<Player_Stats>().stats.abilities.Descend)
        {
            StartCoroutine(DescendingWait(0.01f));
        }
    }
    public IEnumerator DescendingWait(float waitTime)
    {
        gameObject.GetComponent<Player_Teleport>().isTeleporting = true;

        yield return new WaitForSeconds(waitTime);

        if (descendingBlock_Current)
        {
            Vector3 newPos = descendingBlock_Current.transform.position;

            gameObject.transform.position = new Vector3(newPos.x, newPos.y + gameObject.GetComponent<Player_Movement>().heightOverBlock, newPos.z);
        }

        gameObject.GetComponent<Player_Teleport>().isTeleporting = false;
    }
}
