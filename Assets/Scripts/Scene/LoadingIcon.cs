using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIcon : Singleton<LoadingIcon>
{
    public static event Action Action_BlackScreenIsGone;

    [Header("LoadingSprite Object")]
    public GameObject loadingIcon;

    [Header("Parameters")]
    public float amplitude = 20f;  // How far up/down (in pixels)
    public float frequency = 3f;   // Speed of the movement

    private const float MaxAnimationFrameTime = 1f / 20f;
    private static float animationPhase;
    private static int lastAnimationFrame = -1;
    private static LoadingIcon lastAnimatedIcon;
    private static Sprite lastSprite;

    private RectTransform iconRect;
    private Image iconImage;
    private Vector2 startPos;

    [Header("Sprites")]
    [SerializeField] Sprite sprite_Default;
    [SerializeField] Sprite sprite_HUB_Quartz;
    [SerializeField] Sprite sprite_HUB_SourceBlock;

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

    public bool blackscreenIsFading;


    //--------------------


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetAnimation()
    {
        animationPhase = 0f;
        lastAnimationFrame = -1;
        lastAnimatedIcon = null;
        lastSprite = null;
    }

    private void LateUpdate()
    {
        if (!iconRect || !loadingIcon.activeInHierarchy) return;

        int frame = Time.frameCount;
        // Start each scene at the last displayed position. Skip time spent
        // waiting on scene activation instead of jumping ahead in the bob.
        if (lastAnimatedIcon == this && lastAnimationFrame == frame - 1)
        {
            float elapsed = Mathf.Min(Time.unscaledDeltaTime, MaxAnimationFrameTime);
            animationPhase = Mathf.Repeat(animationPhase + elapsed * frequency, 2f * Mathf.PI);
        }

        lastAnimatedIcon = this;
        lastAnimationFrame = frame;
        PositionIcon();
    }

    private void PositionIcon()
    {
        iconRect.anchoredPosition = startPos + Vector2.up * (Mathf.Sin(animationPhase) * amplitude);
    }

    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += LoadingSprite;
        SkinWardrobeButton.Action_SkinIsSelected += LoadingSpriteInGame;

        blackscreenIsFading = true;

        if (!loadingIcon) return;
        if (!iconRect)
        {
            iconRect = loadingIcon.GetComponent<RectTransform>();
            iconImage = loadingIcon.GetComponent<Image>();
            startPos = iconRect.anchoredPosition;
        }

        PositionIcon();
        DataManager data = FindAnyObjectByType<DataManager>();
        Sprite sprite = data && data.gameObject.scene == gameObject.scene && data.dataHasLoaded_Check
            ? SetLoadingSprite()
            : lastSprite ? lastSprite : sprite_Default;
        ShowSprite(sprite);
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= LoadingSprite;
        SkinWardrobeButton.Action_SkinIsSelected -= LoadingSpriteInGame;

        blackscreenIsFading = false;

        Action_BlackScreenIsGone?.Invoke();
    }


    //--------------------

    void LoadingSprite()
    {
        ShowSprite(SetLoadingSprite());
    }

    private void ShowSprite(Sprite sprite)
    {
        if (!iconImage) return;

        iconImage.sprite = sprite;
        lastSprite = sprite;
        loadingIcon.SetActive(true);
    }
    void LoadingSpriteInGame()
    {
        StartCoroutine(LoadingSpriteInGame_Delay(0.25f));
    }
    IEnumerator LoadingSpriteInGame_Delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        LoadingSprite();
    }
    Sprite SetLoadingSprite()
    {
        switch (DataManager.Instance.skinsInfo_Store.activeSkinType)
        {
            case SkinType.None:
                return sprite_Default;

            case SkinType.Default:
                return sprite_Default;
            case SkinType.Quartz:
                return sprite_HUB_Quartz;
            case SkinType.SourceBlock:
                return sprite_HUB_SourceBlock;

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
