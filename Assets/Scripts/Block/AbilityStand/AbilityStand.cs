using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityStand : MonoBehaviour
{
    [SerializeField] Abilities ability;
    [SerializeField] GameObject Ability;

    [Header("Show Animation")]
    float showDelay = 0f;
    float zoomOutTime = 3.25f;
    float settleTime = 0.8f;

    [SerializeField] float overshootMultiplier = 1.2f;

    Coroutine showRoutine;
    Vector3 initialScale;
    bool hasStoredInitialScale = false;


    //--------------------


    private void Update()
    {
        if (!Input.GetKey(KeyCode.G))
            return;

        bool unlockedAbility = false;

        switch (ability)
        {
            case Abilities.None:
                break;

            case Abilities.Snorkel:
            case Abilities.OxygenTank:
            case Abilities.Flippers:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Rivregreen = true;
                    unlockedAbility = true;
                }
                break;

            case Abilities.DrillHelmet:
            case Abilities.DrillBoots:
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Sandlands = true;
                    unlockedAbility = true;
                }
                break;

            case Abilities.GrapplingHook:
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Firevein = true;
                    unlockedAbility = true;
                }
                break;

            case Abilities.HandDrill:
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Frostfield = true;
                    unlockedAbility = true;
                }
                break;

            case Abilities.SpringShoes:
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Witchmire = true;
                    unlockedAbility = true;
                }
                break;

            case Abilities.ClimingGloves:
                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Metalworks = true;
                    unlockedAbility = true;
                }
                break;
        }

        if (unlockedAbility)
        {
            DataPersistanceManager.instance.SaveGame();
            SetAbilityVisibility();
        }
    }


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetAbilityVisibility;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetAbilityVisibility;
    }


    //--------------------


    public void SetAbilityVisibility()
    {
        switch (ability)
        {
            case Abilities.None:
                break;

            case Abilities.Snorkel:
                if (DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Rivregreen)
                    ShowAbility();
                else
                    HideAbility();
                break;
            case Abilities.OxygenTank:
                if (DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Rivregreen)
                    ShowAbility();
                else
                    HideAbility();
                break;
            case Abilities.Flippers:
                if (DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Rivregreen)
                    ShowAbility();
                else
                    HideAbility();
                break;
            case Abilities.DrillHelmet:
                if (DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Sandlands)
                    ShowAbility();
                else
                    HideAbility();
                break;
            case Abilities.DrillBoots:
                if (DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Sandlands)
                    ShowAbility();
                else
                    HideAbility();
                break;
            case Abilities.HandDrill:
                if (DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Frostfield)
                    ShowAbility();
                else
                    HideAbility();
                break;
            case Abilities.SpringShoes:
                if (DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Witchmire)
                    ShowAbility();
                else
                    HideAbility();
                break;
            case Abilities.ClimingGloves:
                if (DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Metalworks)
                    ShowAbility();
                else
                    HideAbility();
                break;
            case Abilities.GrapplingHook:
                if (DataManager.Instance.permanentAbilitiesReceived_Store.permanentAbility_Firevein)
                    ShowAbility();
                else
                    HideAbility();
                break;

            default:
                break;
        }
    }

    void ShowAbility()
    {
        if (!hasStoredInitialScale)
        {
            initialScale = Ability.transform.localScale;
            hasStoredInitialScale = true;
        }

        // Already visible and not currently animating
        if (Ability.activeSelf && showRoutine == null)
            return;

        if (showRoutine != null)
            StopCoroutine(showRoutine);

        showRoutine = StartCoroutine(ShowAbilityRoutine());
    }

    IEnumerator ShowAbilityRoutine()
    {
        Vector3 hiddenScale = Vector3.zero;
        Vector3 overshootScale = initialScale * overshootMultiplier;
        Vector3 finalScale = initialScale;

        Ability.SetActive(false);
        Ability.transform.localScale = hiddenScale;

        yield return new WaitForSeconds(showDelay);

        Ability.SetActive(true);

        // Scale from 0% to 120% of whatever its original scale was
        yield return ScaleAbility(hiddenScale, overshootScale, zoomOutTime);

        // Scale from 120% back to 100% of whatever its original scale was
        yield return ScaleAbility(overshootScale, finalScale, settleTime);

        Ability.transform.localScale = finalScale;
        showRoutine = null;
    }

    IEnumerator ScaleAbility(Vector3 from, Vector3 to, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            // Smooth easing
            t = Mathf.SmoothStep(0f, 1f, t);

            Ability.transform.localScale = Vector3.LerpUnclamped(from, to, t);

            yield return null;
        }

        Ability.transform.localScale = to;
    }

    void HideAbility()
    {
        if (showRoutine != null)
        {
            StopCoroutine(showRoutine);
            showRoutine = null;
        }

        Ability.SetActive(false);
    }
}