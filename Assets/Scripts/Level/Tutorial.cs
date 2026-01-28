using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : Singleton<Tutorial>
{
    [Header("States")]
    public bool state_Movement;
    public bool state_CameraRotation;
    public bool state_Respawn;
    public bool state_FreeCam_1;
    public bool state_FreeCam_2;
    public bool state_DemoMessage;

    public bool tutorial_isRunning;

    [SerializeField] float fadeDuration_In = 0.35f;
    [SerializeField] float fadeDuration_Out = 0.25f;


    //-----


    [Header("Parents")]
    [SerializeField] GameObject Tutorial_Parent;

    [SerializeField] GameObject Tutorial_Movement_Parent;
    [SerializeField] GameObject Tutorial_CameraRotation_Parent;
    [SerializeField] GameObject Tutorial_Respawn_Parent;
    [SerializeField] GameObject Tutorial_FreeCam_1_Parent;
    [SerializeField] GameObject Tutorial_FreeCam_2_Parent;
    [SerializeField] GameObject Tutorial_DemoMessage_Parent;

    [Header("Children")]
    [SerializeField] List<GameObject> Tutorial_Movement_Child;
    [SerializeField] List<GameObject> Tutorial_CameraRotation_Child;
    [SerializeField] List<GameObject> Tutorial_Respawn_Child;
    [SerializeField] List<GameObject> Tutorial_FreeCam_1_Child;
    [SerializeField] List<GameObject> Tutorial_FreeCam_2_Child;
    [SerializeField] List<GameObject> Tutorial_DemoMessage_Child;

    int respawnCounter;


    //--------------------


    private void Start()
    {
        if (!DataManager.Instance.tutorial_Finished)
        {
            PlayerManager.Instance.PauseGame();
        }
    }


    //--------------------


    private void OnEnable()
    {
        if (MapManager.Instance.haveIntroSequence)
            MapManager.Action_EndIntroSequence += Start_Tutorial;
        else
            DataManager.Action_dataHasLoaded += Start_Tutorial;

        Movement.Action_RespawnPlayerLate += End_Tutorial_Respawn;
    }
    private void OnDisable()
    {
        if (MapManager.Instance.haveIntroSequence)
            MapManager.Action_EndIntroSequence -= Start_Tutorial;
        else
            DataManager.Action_dataHasLoaded -= Start_Tutorial;

        Movement.Action_RespawnPlayerLate -= End_Tutorial_Respawn;
    }


    //--------------------

    void Start_Tutorial()
    {
        //print("111. DataManager.Instance.tutorial_Finished: " + DataManager.Instance.tutorial_Finished);
        if (!DataManager.Instance.tutorial_Finished)
        {
            PlayerManager.Instance.PauseGame();
            
            state_Movement = false;
            state_CameraRotation = false;
            state_Respawn = false;

            Tutorial_Movement(true);
        }
    }

    #region Tutorial_Movement
    public void Tutorial_Movement(bool active)
    {
        if (active)
            StartCoroutine(Movement_Start(fadeDuration_In * 2));
        else
            StartCoroutine(Movement_End(fadeDuration_Out));
    }
    IEnumerator Movement_Start(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PopUpManager.Instance.ShowDisplay(Tutorial_Parent, Tutorial_Movement_Parent, Tutorial_Movement_Child);

        yield return new WaitForSeconds(PopUpManager.Instance.fadeDuration_In);

        tutorial_isRunning = true;
        state_Movement = true;
    }
    IEnumerator Movement_End(float waitTime)
    {
        yield return new WaitForSeconds(waitTime / 2);

        PopUpManager.Instance.HideDisplay(Tutorial_Parent, Tutorial_Movement_Parent, Tutorial_Movement_Child);

        yield return new WaitForSeconds(waitTime/2);

        Tutorial_CameraRotation(true);
    }
    #endregion

    #region Tutorial_CameraRotation
    public void Tutorial_CameraRotation(bool active)
    {
        if (active)
            StartCoroutine(CameraRotation_Start(fadeDuration_In));
        else
            StartCoroutine(CameraRotation_End(fadeDuration_Out));
    }
    IEnumerator CameraRotation_Start(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PopUpManager.Instance.ShowDisplay(Tutorial_Parent,Tutorial_CameraRotation_Parent, Tutorial_CameraRotation_Child);

        yield return new WaitForSeconds(PopUpManager.Instance.fadeDuration_In);

        state_CameraRotation = true;
        state_Movement = false;
    }
    IEnumerator CameraRotation_End(float waitTime)
    {
        yield return new WaitForSeconds(waitTime / 2);

        PopUpManager.Instance.HideDisplay(Tutorial_Parent,Tutorial_CameraRotation_Parent, Tutorial_CameraRotation_Child);

        yield return new WaitForSeconds(waitTime / 2);

        Start_Tutorial_Respawn();
    }
    #endregion

    #region Tutorial_Respawn
    public void Start_Tutorial_Respawn()
    {
        StartCoroutine(Tutorial_Respawn_Start(fadeDuration_In));
    }
    IEnumerator Tutorial_Respawn_Start(float waitTime)
    {
        state_CameraRotation = false;

        yield return new WaitForSeconds(waitTime);

        PopUpManager.Instance.ShowDisplay(Tutorial_Parent,Tutorial_Respawn_Parent, Tutorial_Respawn_Child);

        yield return new WaitForSeconds(PopUpManager.Instance.fadeDuration_In);

        state_Respawn = true;
        state_CameraRotation = false;
    }
    public void End_Tutorial_Respawn()
    {
        if (state_Respawn /*tutorial_isRunning*/)
        {
            StartCoroutine(Tutorial_Respawn_End(0.1f));

            //respawnCounter++;

            //if (respawnCounter > 1)
            //{
            //    print("2. Tutorial - Raspawn");
            //    StartCoroutine(Tutorial_Respawn_End(0.1f));
            //}
        }
    }
    IEnumerator Tutorial_Respawn_End(float waitTime)
    {
        state_CameraRotation = false;

        yield return new WaitForSeconds(waitTime / 2);

        PopUpManager.Instance.HideDisplay(Tutorial_Parent, Tutorial_Respawn_Parent, Tutorial_Respawn_Child);

        yield return new WaitForSeconds(waitTime / 2);

        Tutorial_FreeCam_1(true);

        //yield return new WaitForSeconds(0.5f);
    }
    #endregion

    #region Tutorial_FreeCam 1
    public void Tutorial_FreeCam_1(bool active)
    {
        if (active)
            StartCoroutine(Tutorial_FreeCam_1_Start(fadeDuration_In));
        else
            StartCoroutine(Tutorial_FreeCam_1_End(fadeDuration_Out));
    }
    IEnumerator Tutorial_FreeCam_1_Start(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PopUpManager.Instance.ShowDisplay(Tutorial_Parent, Tutorial_FreeCam_1_Parent, Tutorial_FreeCam_1_Child);

        yield return new WaitForSeconds(PopUpManager.Instance.fadeDuration_In);

        state_FreeCam_1 = true;
        state_Respawn = false;
        print("3. Tutorial - Raspawn");
    }
    IEnumerator Tutorial_FreeCam_1_End(float waitTime)
    {
        yield return new WaitForSeconds(waitTime / 2);

        PopUpManager.Instance.HideDisplay(Tutorial_Parent, Tutorial_FreeCam_1_Parent, Tutorial_FreeCam_1_Child);

        yield return new WaitForSeconds(waitTime / 2);

        Tutorial_FreeCam_2(true);

        yield return new WaitForSeconds(0.5f);
    }
    #endregion

    #region Tutorial_FreeCam 2
    public void Tutorial_FreeCam_2(bool active)
    {
        if (active)
            StartCoroutine(Tutorial_FreeCam_2_Start(fadeDuration_In));
        else
            StartCoroutine(Tutorial_FreeCam_2_End(fadeDuration_Out));
    }
    IEnumerator Tutorial_FreeCam_2_Start(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PopUpManager.Instance.ShowDisplay(Tutorial_Parent, Tutorial_FreeCam_2_Parent, Tutorial_FreeCam_2_Child);

        yield return new WaitForSeconds(PopUpManager.Instance.fadeDuration_In /*+ 0.3f*/);

        state_FreeCam_2 = true;
        state_FreeCam_1 = false;
    }
    IEnumerator Tutorial_FreeCam_2_End(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PopUpManager.Instance.HideDisplay(Tutorial_Parent, Tutorial_FreeCam_2_Parent, Tutorial_FreeCam_2_Child);

        yield return new WaitForSeconds(PopUpManager.Instance.fadeDuration_Out);

        state_DemoMessage = true;
        state_FreeCam_2 = false;

        //EndTutorial();
        Tutorial_DemoMessage(true);

        yield return new WaitForSeconds(0.5f);
    }
    #endregion

    #region Tutorial_DemoMessage
    public void Tutorial_DemoMessage(bool active)
    {
        if (active)
            StartCoroutine(DemoMessage_Start(fadeDuration_In));
        else
            StartCoroutine(DemoMessage_End(fadeDuration_Out));
    }
    IEnumerator DemoMessage_Start(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PopUpManager.Instance.ShowDisplay(Tutorial_Parent, Tutorial_DemoMessage_Parent, Tutorial_DemoMessage_Child);

        yield return new WaitForSeconds(PopUpManager.Instance.fadeDuration_In);
    }
    IEnumerator DemoMessage_End(float waitTime)
    {
        yield return new WaitForSeconds(waitTime / 2);

        PopUpManager.Instance.HideDisplay(Tutorial_Parent, Tutorial_DemoMessage_Parent, Tutorial_DemoMessage_Child);

        yield return new WaitForSeconds(waitTime / 2);

        EndTutorial();
    }
    #endregion


    //-----


    public void EndTutorial()
    {
        DataManager.Instance.tutorial_Finished = true;

        //Save DataManager state
        DataPersistanceManager.instance.SaveGame();

        tutorial_isRunning = false;
        PlayerManager.Instance.UnpauseGame();
    }
}
