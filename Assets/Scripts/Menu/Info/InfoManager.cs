using UnityEngine;
using UnityEngine.EventSystems;

public class InfoManager : MonoBehaviour
{
    [Header("Info Panels")]
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject statsPanel;
    [SerializeField] GameObject tabletPanel;

    [Header("First Tab")]
    [SerializeField] GameObject controlsTab;

    internal GameObject FirstTab => controlsTab;

    internal void ShowPanel(MenuCategories category)
    {
        controlsPanel.SetActive(category == MenuCategories.Info_Controls);
        statsPanel.SetActive(category == MenuCategories.Info_Stats);
        tabletPanel.SetActive(category == MenuCategories.Info_Tablet);
    }

    internal void SelectFirstTab()
    {
        if (EventSystem.current && controlsTab)
            EventSystem.current.SetSelectedGameObject(controlsTab);
    }
}
