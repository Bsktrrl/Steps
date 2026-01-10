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

    public bool tutorial_isRunning;


    //-----


    [Header("Parents")]
    [SerializeField] GameObject Tutorial_Parent;

    [SerializeField] GameObject Tutorial_Movement_Parent;
    [SerializeField] GameObject Tutorial_CameraRotation_Parent;
    [SerializeField] GameObject Tutorial_Respawn_Parent;

    [Header("Texts")]
    [SerializeField] List<TextMeshProUGUI> Tutorial_Movement_TextList;
    [SerializeField] List<TextMeshProUGUI> Tutorial_CameraRotation_TextList;
    [SerializeField] List<TextMeshProUGUI> Tutorial_Respawn_TextList;

    [Header("Image Lists")]
    [SerializeField] List<Image> Tutorial_Movement_ImageList;
    [SerializeField] List<Image> Tutorial_CameraRotation_ImageList;
    [SerializeField] List<Image> Tutorial_Respawn_ImageList;


    //-----


    [Header("Fade Settings")]
    [SerializeField] float fadeDuration = 0.85f;

    // Keep track of running fades so we can stop/replace them per parent
    private readonly Dictionary<GameObject, Coroutine> _runningFades = new();


    //-----


    int respawnCounter;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += Start_Tutorial;

        Movement.Action_RespawnPlayerLate += End_Tutorial_Respawn;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= Start_Tutorial;
        Movement.Action_RespawnPlayerLate -= End_Tutorial_Respawn;
    }


    //--------------------

    void Start_Tutorial()
    {
        print("111. DataManager.Instance.tutorial_Finished: " + DataManager.Instance.tutorial_Finished);
        if (!DataManager.Instance.tutorial_Finished)
        {
            PlayerManager.Instance.PauseGame();
            tutorial_isRunning = true;

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
            StartCoroutine(Movement_Start(1f));
        else
            StartCoroutine(Movement_End(0f));
    }
    IEnumerator Movement_Start(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ShowDisplay(Tutorial_Movement_Parent, Tutorial_Movement_TextList, Tutorial_Movement_ImageList);

        yield return new WaitForSeconds(fadeDuration);

        state_Movement = true;
    }
    IEnumerator Movement_End(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        HideDisplay(Tutorial_Movement_Parent, Tutorial_Movement_TextList, Tutorial_Movement_ImageList);

        state_Movement = false;

        Tutorial_CameraRotation(true);
    }
    #endregion

    #region Tutorial_CameraRotation
    public void Tutorial_CameraRotation(bool active)
    {
        if (active)
            StartCoroutine(CameraRotation_Start(1.5f));
        else
            StartCoroutine(CameraRotation_End(0f));
    }
    IEnumerator CameraRotation_Start(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ShowDisplay(Tutorial_CameraRotation_Parent, Tutorial_CameraRotation_TextList, Tutorial_CameraRotation_ImageList);

        yield return new WaitForSeconds(fadeDuration);

        state_CameraRotation = true;
    }
    IEnumerator CameraRotation_End(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        HideDisplay(Tutorial_CameraRotation_Parent, Tutorial_CameraRotation_TextList, Tutorial_CameraRotation_ImageList);

        state_CameraRotation = false;

        Start_Tutorial_Respawn();
    }
    #endregion

    #region Tutorial_Respawn
    public void Start_Tutorial_Respawn()
    {
        StartCoroutine(Tutorial_Respawn_Start(1.5f));
    }
    IEnumerator Tutorial_Respawn_Start(float waitTime)
    {
        state_CameraRotation = false;

        yield return new WaitForSeconds(waitTime);

        ShowDisplay(Tutorial_Respawn_Parent, Tutorial_Respawn_TextList, Tutorial_Respawn_ImageList);

        yield return new WaitForSeconds(fadeDuration);

        state_Respawn = true;
    }
    public void End_Tutorial_Respawn()
    {
        respawnCounter++;

        if (respawnCounter > 1)
        {
            StartCoroutine(Tutorial_Respawn_End(0.5f));
        }
    }
    IEnumerator Tutorial_Respawn_End(float waitTime)
    {
        state_CameraRotation = false;

        yield return new WaitForSeconds(waitTime);

        HideDisplay(Tutorial_Respawn_Parent, Tutorial_Respawn_TextList, Tutorial_Respawn_ImageList);

        yield return new WaitForSeconds(fadeDuration);

        state_Respawn = false;

        EndTutorial();

        yield return new WaitForSeconds(0.5f);
    }
    #endregion

    public void EndTutorial()
    {
        DataManager.Instance.tutorial_Finished = true;

        //Save DataManager state
        DataPersistanceManager.instance.SaveGame();

        tutorial_isRunning = false;
        PlayerManager.Instance.UnpauseGame();
    }


    //--------------------


    void ShowDisplay(GameObject parent, List<TextMeshProUGUI> textList, List<Image> imageList)
    {
        //Fade in all images from the list from 0 to 1
        if (parent == null) return;

        Tutorial_Parent.SetActive(true);

        // Stop any previous fade on this parent
        StopFadeIfRunning(parent);

        // Ensure it's active before fading in
        parent.SetActive(true);

        // Start fully transparent (optional but usually desired)
        SetTextsAlpha(textList, 0f);
        SetImagesAlpha(imageList, 0f);

        if (ControllerState.Instance.activeController == InputType.PlayStation)
        {
            textList[1].gameObject.SetActive(true);
        }
        else if (ControllerState.Instance.activeController == InputType.Xbox)
        {
            textList[2].gameObject.SetActive(true);
        }
        else
        {
            textList[0].gameObject.SetActive(true);
        }

        _runningFades[parent] = StartCoroutine(FadeUI(textList, imageList, 0f, 1f, fadeDuration, disableParentAtEnd: false, parentToDisable: null));
    }
    void HideDisplay(GameObject parent, List<TextMeshProUGUI> textList, List<Image> imageList)
    {
        if (parent == null) return;

        StopFadeIfRunning(parent);

        if (!parent.activeInHierarchy) return;

        _runningFades[parent] = StartCoroutine(FadeUI(textList, imageList, 1f, 0f, fadeDuration, disableParentAtEnd: true, parentToDisable: parent));

        for (int i = 0; i < textList.Count; i++)
        {
            textList[i].gameObject.SetActive(false);
        }

        Tutorial_Parent.SetActive(true);
    }

    private void StopFadeIfRunning(GameObject parent)
    {
        if (_runningFades.TryGetValue(parent, out var c) && c != null)
            StopCoroutine(c);

        _runningFades[parent] = null;
    }

    private static void SetImagesAlpha(List<Image> images, float a)
    {
        if (images == null) return;

        for (int i = 0; i < images.Count; i++)
        {
            var img = images[i];
            if (img == null) continue;

            var c = img.color;
            c.a = a;
            img.color = c;
        }
    }

    private static void SetTextsAlpha(List<TextMeshProUGUI> texts, float a)
    {
        if (texts == null) return;

        for (int i = 0; i < texts.Count; i++)
        {
            var tmp = texts[i];
            if (tmp == null) continue;

            var c = tmp.color;
            c.a = a;
            tmp.color = c;
        }
    }

    private static IEnumerator FadeUI(List<TextMeshProUGUI> texts, List<Image> images, float from, float to, float duration, bool disableParentAtEnd, GameObject parentToDisable)
    {
        // Snap if duration is 0
        if (duration <= 0f)
        {
            SetTextsAlpha(texts, to);
            SetImagesAlpha(images, to);

            if (disableParentAtEnd && parentToDisable != null)
                parentToDisable.SetActive(false);

            yield break;
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float eased = Mathf.SmoothStep(0f, 1f, t);
            float a = Mathf.Lerp(from, to, eased);

            SetTextsAlpha(texts, a);
            SetImagesAlpha(images, a);

            yield return null;
        }

        SetTextsAlpha(texts, to);
        SetImagesAlpha(images, to);

        if (disableParentAtEnd && parentToDisable != null)
            parentToDisable.SetActive(false);
    }
}
