using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuCategorySelected : MonoBehaviour, IPointerDownHandler, ISelectHandler, IDeselectHandler
{
    [Header("SettingState to activate")]
    public SettingState settingState;

    [Header("SettingState to activate")]
    [SerializeField] Image selectedImage;

    [SerializeField] bool isSelected;

    //--------------------


    void OnEnable()
    {
        isSelected = EventSystem.current && EventSystem.current.currentSelectedGameObject == gameObject;
        if (isSelected)
            ChangeSelectedSettingsButtonSegment();
    }

    void OnDisable()
    {
        isSelected = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left &&
            ActionButtonsManager.CanSelectWithMouse(GetComponent<Button>()))
            eventData.selectedObject = gameObject;
    }


    //--------------------


    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        ChangeSelectedSettingsButtonSegment();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
    }

    void ChangeSelectedSettingsButtonSegment()
    {
        SettingsManager.Instance.settingState = settingState;
    }
}
