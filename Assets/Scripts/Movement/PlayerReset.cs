using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    private void Start()
    {
        MainManager.noMoreSteps += ResetPlayerPos;
    }


    //--------------------


    void ResetPlayerPos()
    {
        transform.position = MainManager.Instance.startPos;
    }
}
