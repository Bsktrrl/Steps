using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu_BackToGame_Button : MonoBehaviour
{
    [Header("Animator - Closing Animation")]
    [SerializeField] List<Animator> closingPauseMenuAnimatorList = new List<Animator>();
    float closingMenuDelay = 0.2f;


    //--------------------


    public void BackToGameButton_isPressed()
    {
        StartCoroutine(ClosePauseMenuDelay(closingMenuDelay));
    }


    //--------------------


    IEnumerator ClosePauseMenuDelay(float waitTime)
    {
        for (int i = 0; i < closingPauseMenuAnimatorList.Count; i++)
        {
            closingPauseMenuAnimatorList[i].SetTrigger("Close");
        }

        yield return new WaitForSeconds(waitTime);

        PauseMenuManager.Instance.ClosePauseMenu();
        PlayerManager.Instance.pauseGame = false;
    }
}
