using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Button_SelectOnCancel : MonoBehaviour
{
    [Header("Select On Cancel")]
    [SerializeField] Button selectOnCancel;

    [Header("Menus SetActive")]
    [SerializeField] GameObject menuToOpen;
    [SerializeField] GameObject menuToClose;

    [Header("MenuStates")]
    [SerializeField] public MenuState menuState_ToSelect;
    [SerializeField] public LevelState levelState_ToSelect;
    [SerializeField] public RegionState regionState_ToSelect;


    //--------------------


    private void Reset()
    {
        if (ActionButtonsManager.Instance.eventSystem == null)
        {
            Debug.Log("Did not find an EventSystem in the Scene. ", this);
        }
    }

    void Update()
    {
        if (ActionButtonsManager.Instance.cancel_Button.action.WasPressedThisFrame() && /*RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement == gameObject*/ ActionButtonsManager.Instance.eventSystem.currentSelectedGameObject == gameObject)
        {
            SelectCancelTarget();
        }
    }


    //--------------------


    private void SelectCancelTarget()
    {
        if (ActionButtonsManager.Instance.eventSystem == null)
        {
            Debug.Log("This item has no EventSystem referenced yet.");
            return;
        }

        if (selectOnCancel == null)
        {
            Debug.Log("This should jump where? ", this);
            return;
        }


        //-----


        ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(selectOnCancel.gameObject);

        //Open/Close menus
        if (menuToOpen)
            menuToOpen.SetActive(true);

        if (menuToClose)
            menuToClose.SetActive(false);

        MainMenuManager.Instance.menuState = menuState_ToSelect;
        OverWorldManager.Instance.regionState = regionState_ToSelect;
        OverWorldManager.Instance.levelState = levelState_ToSelect;
    }
}
