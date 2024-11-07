using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Falling : MonoBehaviour
{
    float floatCounter_Current;
    float floatCounter_Max = 0.5f;
    public bool isSteppedOn;


    //--------------------


    private void Update()
    {
        if (CheckIfReadyToFall())
        {
            Falling();
        }
    }


    //--------------------


    bool CheckIfReadyToFall()
    {
        if (isSteppedOn)
        {
            floatCounter_Current += Time.deltaTime;

            if (floatCounter_Current >= floatCounter_Max)
            {
                return true;
            }
        }

        return false;
    }
    void Falling()
    {
        gameObject.transform.position = gameObject.transform.position + (Vector3.down * MainManager.Instance.player.GetComponent<Player_Movement>().fallSpeed * Time.deltaTime);
    }
}
