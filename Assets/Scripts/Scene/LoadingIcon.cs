using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIcon : MonoBehaviour
{
    [Header("LoadingSprite Object")]
    public GameObject loadingIcon;

    [Header("Parameters")]
    public float amplitude = 20f;  // How far up/down (in pixels)
    public float frequency = 3f;   // Speed of the movement

    Vector3 startPos;

    [Header("Sprites")]
    [SerializeField] Sprite sprite_Default;

    [SerializeField] Sprite sprite_Rivergreen_Lv1;
    [SerializeField] Sprite sprite_Rivergreen_Lv2;
    [SerializeField] Sprite sprite_Rivergreen_Lv3;
    [SerializeField] Sprite sprite_Rivergreen_Lv4;
    [SerializeField] Sprite sprite_Rivergreen_Lv5;

    [SerializeField] Sprite sprite_Sandlands_Lv1;
    [SerializeField] Sprite sprite_Sandlands_Lv2;
    [SerializeField] Sprite sprite_Sandlands_Lv3;
    [SerializeField] Sprite sprite_Sandlands_Lv4;
    [SerializeField] Sprite sprite_Sandlands_Lv5;

    [SerializeField] Sprite sprite_Frostfield_Lv1;
    [SerializeField] Sprite sprite_Frostfield_Lv2;
    [SerializeField] Sprite sprite_Frostfield_Lv3;
    [SerializeField] Sprite sprite_Frostfield_Lv4;
    [SerializeField] Sprite sprite_Frostfield_Lv5;

    [SerializeField] Sprite sprite_Firevein_Lv1;
    [SerializeField] Sprite sprite_Firevein_Lv2;
    [SerializeField] Sprite sprite_Firevein_Lv3;
    [SerializeField] Sprite sprite_Firevein_Lv4;
    [SerializeField] Sprite sprite_Firevein_Lv5;

    [SerializeField] Sprite sprite_Witchmire_Lv1;
    [SerializeField] Sprite sprite_Witchmire_Lv2;
    [SerializeField] Sprite sprite_Witchmire_Lv3;
    [SerializeField] Sprite sprite_Witchmire_Lv4;
    [SerializeField] Sprite sprite_Witchmire_Lv5;

    [SerializeField] Sprite sprite_Metalworks_Lv1;
    [SerializeField] Sprite sprite_Metalworks_Lv2;
    [SerializeField] Sprite sprite_Metalworks_Lv3;
    [SerializeField] Sprite sprite_Metalworks_Lv4;
    [SerializeField] Sprite sprite_Metalworks_Lv5;


    //--------------------


    void Start()
    {
        loadingIcon.SetActive(false);

        // Save the original position
        startPos = loadingIcon.GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        // Move up and down using a sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        loadingIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(startPos.x, newY, startPos.z);
    }


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += LoadingSprite;
        SkinWardrobeButton.Action_SkinIsSelected += LoadingSpriteInGame;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= LoadingSprite;
        SkinWardrobeButton.Action_SkinIsSelected -= LoadingSpriteInGame;
    }


    //--------------------

    void LoadingSprite()
    {
        loadingIcon.GetComponent<Image>().sprite = SetLoadingSprite();
        loadingIcon.SetActive(true);
    }
    void LoadingSpriteInGame()
    {
        print("0000. LoadingSpriteInGame_Delay");

        StartCoroutine(LoadingSpriteInGame_Delay(0.25f));
    }
    IEnumerator LoadingSpriteInGame_Delay(float waitTime)
    {
        print("1000. LoadingSpriteInGame_Delay");

        yield return new WaitForSeconds(waitTime);

        loadingIcon.SetActive(true);
        loadingIcon.GetComponent<Image>().sprite = SetLoadingSprite();
        print("2000. LoadingSpriteInGame_Delay");
    }
    Sprite SetLoadingSprite()
    {
        switch (DataManager.Instance.skinsInfo_Store.activeSkinType)
        {
            case SkinType.None:
                return sprite_Default;

            case SkinType.Default:
                return sprite_Default;

            case SkinType.Rivergreen_Lv1:
                return sprite_Rivergreen_Lv1;
            case SkinType.Rivergreen_Lv2:
                return sprite_Rivergreen_Lv2;
            case SkinType.Rivergreen_Lv3:
                return sprite_Rivergreen_Lv3;
            case SkinType.Rivergreen_Lv4:
                return sprite_Rivergreen_Lv4;
            case SkinType.Rivergreen_Lv5:
                return sprite_Rivergreen_Lv5;

            case SkinType.Sandlands_Lv1:
                return sprite_Sandlands_Lv1;
            case SkinType.Sandlands_Lv2:
                return sprite_Sandlands_Lv2;
            case SkinType.Sandlands_Lv3:
                return sprite_Sandlands_Lv3;
            case SkinType.Sandlands_Lv4:
                return sprite_Sandlands_Lv4;
            case SkinType.Sandlands_Lv5:
                return sprite_Sandlands_Lv5;

            case SkinType.Frostfield_Lv1:
                return sprite_Frostfield_Lv1;
            case SkinType.Frostfield_Lv2:
                return sprite_Frostfield_Lv2;
            case SkinType.Frostfield_Lv3:
                return sprite_Frostfield_Lv3;
            case SkinType.Frostfield_Lv4:
                return sprite_Frostfield_Lv4;
            case SkinType.Frostfield_Lv5:
                return sprite_Frostfield_Lv5;

            case SkinType.Firevein_Lv1:
                return sprite_Firevein_Lv1;
            case SkinType.Firevein_Lv2:
                return sprite_Firevein_Lv2;
            case SkinType.Firevein_Lv3:
                return sprite_Firevein_Lv3;
            case SkinType.Firevein_Lv4:
                return sprite_Firevein_Lv4;
            case SkinType.Firevein_Lv5:
                return sprite_Firevein_Lv5;

            case SkinType.Witchmire_Lv1:
                return sprite_Witchmire_Lv1;
            case SkinType.Witchmire_Lv2:
                return sprite_Witchmire_Lv2;
            case SkinType.Witchmire_Lv3:
                return sprite_Witchmire_Lv3;
            case SkinType.Witchmire_Lv4:
                return sprite_Witchmire_Lv4;
            case SkinType.Witchmire_Lv5:
                return sprite_Witchmire_Lv5;

            case SkinType.Metalworks_Lv1:
                return sprite_Metalworks_Lv1;
            case SkinType.Metalworks_Lv2:
                return sprite_Metalworks_Lv2;
            case SkinType.Metalworks_Lv3:
                return sprite_Metalworks_Lv3;
            case SkinType.Metalworks_Lv4:
                return sprite_Metalworks_Lv4;
            case SkinType.Metalworks_Lv5:
                return sprite_Metalworks_Lv5;

            default:
                return sprite_Default;
        }
    }
}
