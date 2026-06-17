using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionLevelImages : MonoBehaviour
{
    [Header("Image Components")]
    [SerializeField] Image image_Level_1;
    [SerializeField] Image image_Level_2;
    [SerializeField] Image image_Level_3;
    [SerializeField] Image image_Level_4;
    [SerializeField] Image image_Level_5;

    [SerializeField] Image image_Level_HUB;

    [Header("Level Sprites")]
    [SerializeField] Sprite sprite_HUB;

    [SerializeField] Sprite sprite_Rivergreen_lv_1;
    [SerializeField] Sprite sprite_Rivergreen_lv_2;
    [SerializeField] Sprite sprite_Rivergreen_lv_3;
    [SerializeField] Sprite sprite_Rivergreen_lv_4;
    [SerializeField] Sprite sprite_Rivergreen_lv_5;

    [SerializeField] Sprite sprite_Sandlands_lv_1;
    [SerializeField] Sprite sprite_Sandlands_lv_2;
    [SerializeField] Sprite sprite_Sandlands_lv_3;
    [SerializeField] Sprite sprite_Sandlands_lv_4;
    [SerializeField] Sprite sprite_Sandlands_lv_5;

    [SerializeField] Sprite sprite_Frostfield_lv_1;
    [SerializeField] Sprite sprite_Frostfield_lv_2;
    [SerializeField] Sprite sprite_Frostfield_lv_3;
    [SerializeField] Sprite sprite_Frostfield_lv_4;
    [SerializeField] Sprite sprite_Frostfield_lv_5;

    [SerializeField] Sprite sprite_Firevein_lv_1;
    [SerializeField] Sprite sprite_Firevein_lv_2;
    [SerializeField] Sprite sprite_Firevein_lv_3;
    [SerializeField] Sprite sprite_Firevein_lv_4;
    [SerializeField] Sprite sprite_Firevein_lv_5;

    [SerializeField] Sprite sprite_Witchmire_lv_1;
    [SerializeField] Sprite sprite_Witchmire_lv_2;
    [SerializeField] Sprite sprite_Witchmire_lv_3;
    [SerializeField] Sprite sprite_Witchmire_lv_4;
    [SerializeField] Sprite sprite_Witchmire_lv_5;

    [SerializeField] Sprite sprite_Metalworks_lv_1;
    [SerializeField] Sprite sprite_Metalworks_lv_2;
    [SerializeField] Sprite sprite_Metalworks_lv_3;
    [SerializeField] Sprite sprite_Metalworks_lv_4;
    [SerializeField] Sprite sprite_Metalworks_lv_5;


    //--------------------


    private void OnEnable()
    {
        
        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement += SetImages;
        CancelPauseMenuByButtonPress.Action_BackButton_IsPressed += HideAllImages_Delay;
    }
    private void OnDisable()
    {
        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement -= SetImages;
        CancelPauseMenuByButtonPress.Action_BackButton_IsPressed -= HideAllImages_Delay;
    }


    //--------------------


    void SetImages()
    {
        if (RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement && RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement.GetComponent<Button_ToPress>())
        {
            MainMenuManager.Instance.subMenuState = RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement.GetComponent<Button_ToPress>().newMenuState;
        }

        HideAllImages();

        switch (MainMenuManager.Instance.subMenuState)
        {
            case MenuState.RegionMenu_Water:
                image_Level_1.sprite = sprite_Rivergreen_lv_1;
                image_Level_2.sprite = sprite_Rivergreen_lv_2;
                image_Level_3.sprite = sprite_Rivergreen_lv_3;
                image_Level_4.sprite = sprite_Rivergreen_lv_4;
                image_Level_5.sprite = sprite_Rivergreen_lv_5;
                ShowAllImages();
                break;
            case MenuState.RegionMenu_Sand:
                image_Level_1.sprite = sprite_Sandlands_lv_1;
                image_Level_2.sprite = sprite_Sandlands_lv_2;
                image_Level_3.sprite = sprite_Sandlands_lv_3;
                image_Level_4.sprite = sprite_Sandlands_lv_4;
                image_Level_5.sprite = sprite_Sandlands_lv_5;
                ShowAllImages();
                break;
            case MenuState.RegionMenu_Ice:
                image_Level_1.sprite = sprite_Frostfield_lv_1;
                image_Level_2.sprite = sprite_Frostfield_lv_2;
                image_Level_3.sprite = sprite_Frostfield_lv_3;
                image_Level_4.sprite = sprite_Frostfield_lv_4;
                image_Level_5.sprite = sprite_Frostfield_lv_5;
                ShowAllImages();
                break;
            case MenuState.RegionMenu_Lava:
                image_Level_1.sprite = sprite_Firevein_lv_1;
                image_Level_2.sprite = sprite_Firevein_lv_2;
                image_Level_3.sprite = sprite_Firevein_lv_3;
                image_Level_4.sprite = sprite_Firevein_lv_4;
                image_Level_5.sprite = sprite_Firevein_lv_5;
                ShowAllImages();
                break;
            case MenuState.RegionMenu_Swamp:
                image_Level_1.sprite = sprite_Witchmire_lv_1;
                image_Level_2.sprite = sprite_Witchmire_lv_2;
                image_Level_3.sprite = sprite_Witchmire_lv_3;
                image_Level_4.sprite = sprite_Witchmire_lv_4;
                image_Level_5.sprite = sprite_Witchmire_lv_5;
                ShowAllImages();
                break;
            case MenuState.RegionMenu_Metal:
                image_Level_1.sprite = sprite_Metalworks_lv_1;
                image_Level_2.sprite = sprite_Metalworks_lv_2;
                image_Level_3.sprite = sprite_Metalworks_lv_3;
                image_Level_4.sprite = sprite_Metalworks_lv_4;
                image_Level_5.sprite = sprite_Metalworks_lv_5;
                ShowAllImages();
                break;
            case MenuState.RegionMenu_HUB:
                image_Level_HUB.sprite = sprite_HUB;
                ShowHUBImage();
                break;

            default:
                break;
        }
    }

    void HideAllImages()
    {
        image_Level_1.gameObject.SetActive(false);
        image_Level_2.gameObject.SetActive(false);
        image_Level_3.gameObject.SetActive(false);
        image_Level_4.gameObject.SetActive(false);
        image_Level_5.gameObject.SetActive(false);
        image_Level_HUB.gameObject.SetActive(false);
    }
    void HideAllImages_Delay()
    {
        StartCoroutine(HideAllImages_PerformDelay(0.15f));
    }
    IEnumerator HideAllImages_PerformDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        HideAllImages();
    }
    void ShowAllImages()
    {
        image_Level_1.gameObject.SetActive(true);
        image_Level_2.gameObject.SetActive(true);
        image_Level_3.gameObject.SetActive(true);
        image_Level_4.gameObject.SetActive(true);
        image_Level_5.gameObject.SetActive(true);
    }
    void ShowHUBImage()
    {
        image_Level_HUB.gameObject.SetActive(true);
    }
}
