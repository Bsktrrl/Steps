using System.Collections;
using UnityEngine;

public class StepsDisplay : MonoBehaviour
{
    [Header("Parents")]
    [SerializeField] GameObject footsteps_Parent;

    [SerializeField] GameObject stepDisplay_Steps;
    [SerializeField] GameObject stepDisplay_Number;
    [SerializeField] GameObject stepDisplay_NumbersSteps;

    [Header("Footsteps Sprites")]
    public Sprite normalFootstep_Active;
    public Sprite normalFootstep_Used;

    public Sprite extraFootstep_Passive;
    public Sprite extraFootstep_Active;
    public Sprite extraFootstep_Used;

    [Header("Numbers Sprites")]
    public Sprite number_0;
    public Sprite number_1;
    public Sprite number_2;
    public Sprite number_3;
    public Sprite number_4;
    public Sprite number_5;
    public Sprite number_6;
    public Sprite number_7;
    public Sprite number_8;
    public Sprite number_9;
    public Sprite number_10;

    private const float IntroFadeDuration = 0.35f;
    private CanvasGroup introCanvasGroup;
    private float introVisibleAlpha;
    private bool introBlocksRaycasts;


    //--------------------


    private void Awake()
    {
        if (!TryGetComponent(out introCanvasGroup))
            introCanvasGroup = gameObject.AddComponent<CanvasGroup>();

        introVisibleAlpha = introCanvasGroup.alpha;
        introBlocksRaycasts = introCanvasGroup.blocksRaycasts;
        introCanvasGroup.alpha = 0f;
        introCanvasGroup.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        MapManager.Action_EndIntroSequence += SetStepsDisplay_FromSceneStart;
        SettingsManager.Action_SetNewStepDisplay += SetStepsDisplay_FromSettings;
    }

    private void OnDisable()
    {
        MapManager.Action_EndIntroSequence -= SetStepsDisplay_FromSceneStart;
        SettingsManager.Action_SetNewStepDisplay -= SetStepsDisplay_FromSettings;
    }


    //--------------------


    private IEnumerator FadeInAfterIntro()
    {
        float elapsed = 0f;
        while (elapsed < IntroFadeDuration)
        {
            yield return null;

            elapsed += Time.deltaTime;
            introCanvasGroup.alpha = Mathf.SmoothStep(0f, introVisibleAlpha, elapsed / IntroFadeDuration);
        }

        introCanvasGroup.alpha = introVisibleAlpha;
        introCanvasGroup.blocksRaycasts = introBlocksRaycasts;
        introCanvasGroup = null;
    }

    void SetStepsDisplay_FromSceneStart()
    {
        SetStepsDisplayBase();

        if (ShouldShowFootprints() && TryGetComponent(out StepsHUD hud))
        {
            // Scene start should fill one-by-one, like respawn.
            hud.UpdateStepsDisplay_Respawn();
        }

        if (introCanvasGroup != null)
            StartCoroutine(FadeInAfterIntro());
    }

    void SetStepsDisplay_FromSettings()
    {
        SetStepsDisplayBase();

        if (ShouldShowFootprints() && TryGetComponent(out StepsHUD hud))
        {
            // Settings change should snap to current state.
            hud.RefreshAllFootprintsImmediate(false);
        }
    }

    #region Helpers

    void SetStepsDisplayBase()
    {
        StepDisplay display = DataManager.Instance.settingData_StoreList.currentStepDisplay;
        if (stepDisplay_Steps) stepDisplay_Steps.SetActive(display == StepDisplay.Steps);
        if (stepDisplay_Number) stepDisplay_Number.SetActive(display == StepDisplay.Number);
        if (stepDisplay_NumbersSteps) stepDisplay_NumbersSteps.SetActive(display == StepDisplay.NumberSteps);
        if (footsteps_Parent) footsteps_Parent.SetActive(ShouldShowFootprints());
    }

    bool ShouldShowFootprints()
    {
        return DataManager.Instance.settingData_StoreList.currentStepDisplay == StepDisplay.Steps ||
               DataManager.Instance.settingData_StoreList.currentStepDisplay == StepDisplay.NumberSteps;
    }

    #endregion
}
