using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDisplay_PauseMenu : MonoBehaviour
{
    [Header("Main Buttons")]
    [SerializeField] GameObject main_Continue;
    [SerializeField] GameObject main_Wardrobe;
    [SerializeField] GameObject main_Setting;
    [SerializeField] GameObject main_Exit;

    [Header("Ability Buttons")]
    [SerializeField] GameObject ability_Swim;
    [SerializeField] GameObject ability_SwiftSwim;
    [SerializeField] GameObject ability_FreeSwim;

    [SerializeField] GameObject ability_Ascend;
    [SerializeField] GameObject ability_Descend;

    [SerializeField] GameObject ability_Dash;
    [SerializeField] GameObject ability_Jump;

    [SerializeField] GameObject ability_GrapplingHook;
    [SerializeField] GameObject ability_CeilingGrab;

    [Header("Ability Boxes")]
    [SerializeField] GameObject abilityBox_Swim;
    [SerializeField] GameObject abilityBox_SwiftSwim;
    [SerializeField] GameObject abilityBox_FreeSwim;

    [SerializeField] GameObject abilityBox_Ascend;
    [SerializeField] GameObject abilityBox_Descend;

    [SerializeField] GameObject abilityBox_Dash;
    [SerializeField] GameObject abilityBox_Jump;

    [SerializeField] GameObject abilityBox_GrapplingHook;
    [SerializeField] GameObject abilityBox_CeilingGrab;

    [Header("Active Ability Buttons")]
    public List<GameObject> active_AbilityButtons = new List<GameObject>();


    //--------------------


    private void OnEnable()
    {
        PauseMenuManager.Instance.ResetActivePauseMenuInfoButtons();
        UpdateAbilityLinks();

        PauseMenu_MainButton.Action_ButtonIsSelected += UpdateAbilityLinks;
    }
    private void OnDisable()
    {
        PauseMenu_MainButton.Action_ButtonIsSelected -= UpdateAbilityLinks;
    }


    //--------------------


    void UpdateAbilityLinks()
    {
        //AllignBoxes();

        ResetActiveAbilityList();
        ShowAbilities();
        LinkAbilities();

        ResetMainButtonsToAbilities();
        LinkMainButtonsToAbilities();
    }


    //--------------------


    void ShowAbilities()
    {
        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.Snorkel || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.Snorkel)
        {
            active_AbilityButtons.Add(ability_Swim);
            ability_Swim.SetActive(true);
        }
        else
            ability_Swim.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.Flippers || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.Flippers)
        {
            active_AbilityButtons.Add(ability_SwiftSwim);
            ability_SwiftSwim.SetActive(true);
        }
        else
            ability_SwiftSwim.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.OxygenTank || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.OxygenTank)
        {
            active_AbilityButtons.Add(ability_FreeSwim);
            ability_FreeSwim.SetActive(true);
        }
        else
            ability_FreeSwim.SetActive(false);


        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.DrillHelmet || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.DrillHelmet)
        {
            active_AbilityButtons.Add(ability_Ascend);
            ability_Ascend.SetActive(true);
        }
        else
            ability_Ascend.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.DrillBoots || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.DrillBoots)
        {
            active_AbilityButtons.Add(ability_Descend);
            ability_Descend.SetActive(true);
        }
        else
            ability_Descend.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.SpringShoes || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.SpringShoes)
        {
            active_AbilityButtons.Add(ability_Jump);
            ability_Jump.SetActive(true);
        }
        else
            ability_Jump.SetActive(false);


        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.GrapplingHook || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.GrapplingHook)
        {
            active_AbilityButtons.Add(ability_GrapplingHook);
            ability_GrapplingHook.SetActive(true);
        }
        else
            ability_GrapplingHook.SetActive(false);


        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.HandDrill || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.HandDrill)
        {
            active_AbilityButtons.Add(ability_Dash);
            ability_Dash.SetActive(true);
        }
        else
            ability_Dash.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.ClimingGloves || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.ClimingGloves)
        {
            active_AbilityButtons.Add(ability_CeilingGrab);
            ability_CeilingGrab.SetActive(true);
        }
        else
            ability_CeilingGrab.SetActive(false);
    }

    void ResetActiveAbilityList()
    {
        active_AbilityButtons.Clear();
    }
    void LinkAbilities()
    {
        for (int i = 0; i < active_AbilityButtons.Count; i++)
        {
            Button current = active_AbilityButtons[i].GetComponent<Button>();

            Button previous = active_AbilityButtons[(i - 1 + active_AbilityButtons.Count) % active_AbilityButtons.Count].GetComponent<Button>();
            Button next = active_AbilityButtons[(i + 1) % active_AbilityButtons.Count].GetComponent<Button>();

            Navigation nav = current.navigation;
            nav.mode = Navigation.Mode.Explicit;

            nav.selectOnDown = next;
            nav.selectOnUp = previous;
            nav.selectOnLeft = PauseMenuManager.Instance.activeMainButton;
            nav.selectOnRight = PauseMenuManager.Instance.activeMainButton;

            current.navigation = nav;
        }
    }

    void ResetMainButtonsToAbilities()
    {
        ResetMainButtons(main_Continue.GetComponent<Button>());
        ResetMainButtons(main_Wardrobe.GetComponent<Button>());
        ResetMainButtons(main_Setting.GetComponent<Button>());
        ResetMainButtons(main_Exit.GetComponent<Button>());
    }
    void ResetMainButtons(Button button)
    {
        Navigation nav = button.navigation;
        nav.mode = Navigation.Mode.Explicit;

        nav.selectOnRight = null;
        nav.selectOnLeft = null;

        button.navigation = nav;
    }
    void LinkMainButtonsToAbilities()
    {
        if (active_AbilityButtons.Count > 0)
        {
            LinkMainButtons(main_Continue.GetComponent<Button>());
            LinkMainButtons(main_Wardrobe.GetComponent<Button>());
            LinkMainButtons(main_Setting.GetComponent<Button>());
            LinkMainButtons(main_Exit.GetComponent<Button>());
        }
    }
    void LinkMainButtons(Button button)
    {
        Navigation nav = button.navigation;
        nav.mode = Navigation.Mode.Explicit;

        if (PauseMenuManager.Instance.activeAbilityButton)
        {
            nav.selectOnRight = PauseMenuManager.Instance.activeAbilityButton;
            nav.selectOnLeft = PauseMenuManager.Instance.activeAbilityButton;
        }
        else
        {
            nav.selectOnRight = active_AbilityButtons[0].GetComponent<Button>();
            nav.selectOnLeft = active_AbilityButtons[0].GetComponent<Button>();
        }

        button.navigation = nav;
    }

    //void AllignBoxes()
    //{
    //    AllignBox(abilityBox_Swim.GetComponent<RectTransform>(), ability_Swim.GetComponent<RectTransform>());
    //    AllignBox(abilityBox_SwiftSwim.GetComponent<RectTransform>(), ability_SwiftSwim.GetComponent<RectTransform>());
    //    AllignBox(abilityBox_FreeSwim.GetComponent<RectTransform>(), ability_FreeSwim.GetComponent<RectTransform>());

    //    AllignBox(abilityBox_Ascend.GetComponent<RectTransform>(), ability_Ascend.GetComponent<RectTransform>());
    //    AllignBox(abilityBox_Descend.GetComponent<RectTransform>(), ability_Descend.GetComponent<RectTransform>());

    //    AllignBox(abilityBox_Dash.GetComponent<RectTransform>(), ability_Dash.GetComponent<RectTransform>());
    //    AllignBox(abilityBox_Jump.GetComponent<RectTransform>(), ability_Jump.GetComponent<RectTransform>());

    //    AllignBox(abilityBox_GrapplingHook.GetComponent<RectTransform>(), ability_GrapplingHook.GetComponent<RectTransform>());
    //    AllignBox(abilityBox_CeilingGrab.GetComponent<RectTransform>(), ability_CeilingGrab.GetComponent<RectTransform>());
    //}
    //void AllignBox(RectTransform box, RectTransform referenceObject)
    //{
    //    if (!box || !referenceObject) return;

    //    // 1) Reference world position (global in canvas)
    //    Vector3 refWorldPos = referenceObject.position;

    //    // 2) Convert world -> popup parent local space
    //    RectTransform popupParent = box.parent as RectTransform;
    //    if (!popupParent) return;

    //    Vector2 localPoint;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        popupParent,
    //        RectTransformUtility.WorldToScreenPoint(null, refWorldPos),
    //        null,
    //        out localPoint
    //    );

    //    // 3) Apply only Y (keep popup’s current X)
    //    Vector2 ap = box.anchoredPosition;
    //    ap.y = localPoint.y;
    //    box.anchoredPosition = ap;
    //}
}
