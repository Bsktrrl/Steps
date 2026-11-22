using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuBackNavigation : MonoBehaviour, ISelectHandler
{
    [Serializable]
    struct TabLink
    {
        public MenuCategories category;
        public Selectable tab;
        public Selectable upTarget;
    }

    [SerializeField] TabLink[] tabLinks = Array.Empty<TabLink>();

    Button backButton;
    Navigation defaultNavigation;

    private void Awake()
    {
        backButton = GetComponent<Button>();
        defaultNavigation = backButton.navigation;
    }

    private void OnDisable()
    {
        if (backButton)
            backButton.navigation = defaultNavigation;
    }

    public void OnSelect(BaseEventData eventData)
    {
        MenuCategories category = MenuManager.Instance.currentMenuCategorySelected;
        Navigation navigation = defaultNavigation;
        foreach (TabLink link in tabLinks)
        {
            if (link.category != category || !link.tab || !link.tab.IsActive())
                continue;

            navigation.selectOnDown = link.tab;
            if (link.upTarget && link.upTarget.IsActive())
                navigation.selectOnUp = link.upTarget;
            break;
        }

        backButton.navigation = navigation;
    }
}
