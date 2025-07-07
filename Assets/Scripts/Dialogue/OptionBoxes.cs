using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionBoxes : Singleton<OptionBoxes>
{
    public Button optionButton_1;
    public Button optionButton_2;
    public Button optionButton_3;
    public Button optionButton_4;


    //--------------------


    public void SetupOptions(NPCs npc, string option_1, string option_2, string option_3, string option_4)
    {
        SetupOptionTexts(optionButton_1, option_1);
        SetupOptionTexts(optionButton_2, option_2);
        SetupOptionTexts(optionButton_3, option_3);
        SetupOptionTexts(optionButton_4, option_4);

        SetupOptionColors(ref optionButton_1, npc);
        SetupOptionColors(ref optionButton_2, npc);
        SetupOptionColors(ref optionButton_3, npc);
        SetupOptionColors(ref optionButton_4, npc);

        SetupOptionBoxesLinked();
    }
    void SetupOptionColors(ref Button button, NPCs npc)
    {
        ColorBlock cb = new ColorBlock();

        cb.colorMultiplier = 1;
        cb.fadeDuration = 0.1f;
        cb.disabledColor = new Color(0, 0, 0, 0.25f);

        switch (npc)
        {
            case NPCs.None:
                break;

            case NPCs.Floriel:
                cb.normalColor = DialogueColors.Instance.floriel_DialogueBox_Normal;
                cb.highlightedColor = DialogueColors.Instance.floriel_DialogueBox_Highlighted;
                cb.pressedColor = DialogueColors.Instance.floriel_DialogueBox_Pressed;
                cb.selectedColor = DialogueColors.Instance.floriel_DialogueBox_Selected;
                button.colors = cb;
                break;
            case NPCs.Granith:
                cb.normalColor = DialogueColors.Instance.granith_DialogueBox_Normal;
                cb.highlightedColor = DialogueColors.Instance.granith_DialogueBox_Highlighted;
                cb.pressedColor = DialogueColors.Instance.granith_DialogueBox_Pressed;
                cb.selectedColor = DialogueColors.Instance.granith_DialogueBox_Selected;
                button.colors = cb;
                break;
            case NPCs.Archie:
                cb.normalColor = DialogueColors.Instance.archie_DialogueBox_Normal;
                cb.highlightedColor = DialogueColors.Instance.archie_DialogueBox_Highlighted;
                cb.pressedColor = DialogueColors.Instance.archie_DialogueBox_Pressed;
                cb.selectedColor = DialogueColors.Instance.archie_DialogueBox_Selected;
                button.colors = cb;
                break;
            case NPCs.Aisa:
                cb.normalColor = DialogueColors.Instance.aisa_DialogueBox_Normal;
                cb.highlightedColor = DialogueColors.Instance.aisa_DialogueBox_Highlighted;
                cb.pressedColor = DialogueColors.Instance.aisa_DialogueBox_Pressed;
                cb.selectedColor = DialogueColors.Instance.aisa_DialogueBox_Selected;
                button.colors = cb;
                break;
            case NPCs.Mossy:
                cb.normalColor = DialogueColors.Instance.mossy_DialogueBox_Normal;
                cb.highlightedColor = DialogueColors.Instance.mossy_DialogueBox_Highlighted;
                cb.pressedColor = DialogueColors.Instance.mossy_DialogueBox_Pressed;
                cb.selectedColor = DialogueColors.Instance.mossy_DialogueBox_Selected;
                button.colors = cb;
                break;
            case NPCs.Larry:
                cb.normalColor = DialogueColors.Instance.larry_DialogueBox_Normal;
                cb.highlightedColor = DialogueColors.Instance.larry_DialogueBox_Highlighted;
                cb.pressedColor = DialogueColors.Instance.larry_DialogueBox_Pressed;
                cb.selectedColor = DialogueColors.Instance.larry_DialogueBox_Selected;
                button.colors = cb;
                break;
            case NPCs.Stepellier:
                cb.normalColor = DialogueColors.Instance.stepellier_DialogueBox_Normal;
                cb.highlightedColor = DialogueColors.Instance.stepellier_DialogueBox_Highlighted;
                cb.pressedColor = DialogueColors.Instance.stepellier_DialogueBox_Pressed;
                cb.selectedColor = DialogueColors.Instance.stepellier_DialogueBox_Selected;
                button.colors = cb;
                break;

            default:
                break;
        }
    }
    void SetupOptionTexts(Button button, string option)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = option;
    }
    void SetupOptionBoxesLinked()
    {
        // Reset existing navigation
        optionButton_1.navigation = new Navigation();
        optionButton_2.navigation = new Navigation();
        optionButton_3.navigation = new Navigation();
        optionButton_4.navigation = new Navigation();

        if (optionButton_4.GetComponentInChildren<TextMeshProUGUI>().text != "")
        {
            var nav1 = optionButton_1.navigation;
            nav1.mode = Navigation.Mode.Explicit;
            nav1.selectOnUp = optionButton_2;
            nav1.selectOnDown = optionButton_4;
            optionButton_1.navigation = nav1;

            var nav2 = optionButton_2.navigation;
            nav2.mode = Navigation.Mode.Explicit;
            nav2.selectOnUp = optionButton_3;
            nav2.selectOnDown = optionButton_1;
            optionButton_2.navigation = nav2;

            var nav3 = optionButton_3.navigation;
            nav3.mode = Navigation.Mode.Explicit;
            nav3.selectOnUp = optionButton_4;
            nav3.selectOnDown = optionButton_2;
            optionButton_3.navigation = nav3;

            var nav4 = optionButton_4.navigation;
            nav4.mode = Navigation.Mode.Explicit;
            nav4.selectOnUp = optionButton_1;
            nav4.selectOnDown = optionButton_3;
            optionButton_4.navigation = nav4;
        }
        else if (optionButton_3.GetComponentInChildren<TextMeshProUGUI>().text != "" && optionButton_4.GetComponentInChildren<TextMeshProUGUI>().text == "")
        {
            var nav1 = optionButton_1.navigation;
            nav1.mode = Navigation.Mode.Explicit;
            nav1.selectOnUp = optionButton_2;
            nav1.selectOnDown = optionButton_3;
            optionButton_1.navigation = nav1;

            var nav2 = optionButton_2.navigation;
            nav2.mode = Navigation.Mode.Explicit;
            nav2.selectOnUp = optionButton_3;
            nav2.selectOnDown = optionButton_1;
            optionButton_2.navigation = nav2;

            var nav3 = optionButton_3.navigation;
            nav3.mode = Navigation.Mode.Explicit;
            nav3.selectOnUp = optionButton_1;
            nav3.selectOnDown = optionButton_2;
            optionButton_3.navigation = nav3;
        }
        else if (optionButton_2.GetComponentInChildren<TextMeshProUGUI>().text != "" && optionButton_3.GetComponentInChildren<TextMeshProUGUI>().text == "" && optionButton_4.GetComponentInChildren<TextMeshProUGUI>().text == "")
        {
            var nav1 = optionButton_1.navigation;
            nav1.mode = Navigation.Mode.Explicit;
            nav1.selectOnUp = optionButton_2;
            nav1.selectOnDown = optionButton_2;
            optionButton_1.navigation = nav1;

            var nav2 = optionButton_2.navigation;
            nav2.mode = Navigation.Mode.Explicit;
            nav2.selectOnUp = optionButton_1;
            nav2.selectOnDown = optionButton_1;
            optionButton_2.navigation = nav2;
        }
        else if (optionButton_1.GetComponentInChildren<TextMeshProUGUI>().text != "" && optionButton_2.GetComponentInChildren<TextMeshProUGUI>().text == "" && optionButton_3.GetComponentInChildren<TextMeshProUGUI>().text == "" && optionButton_4.GetComponentInChildren<TextMeshProUGUI>().text == "")
        {
            var nav1 = optionButton_1.navigation;
            nav1.mode = Navigation.Mode.Explicit;
            nav1.selectOnUp = optionButton_1;
            nav1.selectOnDown = optionButton_1;
            optionButton_1.navigation = nav1;
        }

        //Set the first selected button for controller input
        EventSystem.current.SetSelectedGameObject(optionButton_1.gameObject);
    }


    //--------------------


    public void ShowHideOptions()
    {
        SetupOptionVisibility(optionButton_1, optionButton_1.GetComponentInChildren<TextMeshProUGUI>().text);
        SetupOptionVisibility(optionButton_2, optionButton_2.GetComponentInChildren<TextMeshProUGUI>().text);
        SetupOptionVisibility(optionButton_3, optionButton_3.GetComponentInChildren<TextMeshProUGUI>().text);
        SetupOptionVisibility(optionButton_4, optionButton_4.GetComponentInChildren<TextMeshProUGUI>().text);
    }
    void SetupOptionVisibility(Button button, string option)
    {
        if (option == "")
            button.gameObject.SetActive(false);
        else
        {
            button.gameObject.SetActive(true);
            DialogueManager.Instance.selectedButton = 1;
        }
    }
    public void HideOptions()
    {
        optionButton_1.gameObject.SetActive(false);
        optionButton_2.gameObject.SetActive(false);
        optionButton_3.gameObject.SetActive(false);
        optionButton_4.gameObject.SetActive(false);
    }
}
