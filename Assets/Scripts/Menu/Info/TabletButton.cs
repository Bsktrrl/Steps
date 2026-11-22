using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(CanvasGroup), typeof(CancelPauseMenuByButtonPress))]
[RequireComponent(typeof(ButtonSound))]
public class TabletButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image background;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite highlightedSprite;

    TabletMenuManager menu;
    CanvasGroup canvasGroup;

    internal Button SelectionButton { get; private set; }
    internal int TextEntry { get; private set; }
    internal bool IsRead { get; private set; }

    internal void Bind(TabletMenuManager owner, int textEntry, Button tabletTab)
    {
        menu = owner;
        TextEntry = textEntry;
        SelectionButton = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
        GetComponent<CancelPauseMenuByButtonPress>().SetLocalCancelSelection(tabletTab, owner.CancelTabletSelection);
    }

    internal void SetRead(bool isRead, float unreadAlpha)
    {
        IsRead = isRead;
        SelectionButton.interactable = isRead;
        canvasGroup.alpha = isRead ? 1f : unreadAlpha;
        canvasGroup.blocksRaycasts = isRead;
    }

    internal void SetHighlighted(bool highlighted)
    {
        background.sprite = highlighted ? highlightedSprite : normalSprite;
    }

    internal void PlayLockSound(bool locked)
    {
        GetComponent<ButtonSound>().TryButtonPressSound(
            locked ? ButtonSoundStates.ButtonEquip_On : ButtonSoundStates.ButtonEquip_Off);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (menu)
            menu.SelectTablet(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (menu)
            menu.DeselectTablet(this);
    }

    public void TabletButton_isPressed()
    {
        if (menu && IsRead)
            menu.PressTablet(this);
    }
}
