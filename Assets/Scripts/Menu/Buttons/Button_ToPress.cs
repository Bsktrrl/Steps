using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_ToPress : MonoBehaviour
{
    [Header("MenuState")]
    [SerializeField] MenuState newMenuState;

    [Header("Setup")]
    [SerializeField] EventSystem eventSystem;
    [SerializeField] Selectable uiElementToSelect;

    [Header("Visualization")]
    [SerializeField] bool showVisualization;
    [SerializeField] Color navigationColor = Color.cyan;



    //--------------------


    private void OnDrawGizmos()
    {
        if (!showVisualization) { return; }
        if (uiElementToSelect == null) { return; }

        Gizmos.color = navigationColor;
        Gizmos.DrawLine(gameObject.transform.position, uiElementToSelect.gameObject.transform.position);
    }

    private void Reset()
    {
       eventSystem = FindObjectOfType<EventSystem>();

        if (eventSystem == null)
        {
            Debug.Log("Did not find an EventSystem in the Scene. ", this);
        }
    }

    void JumpToElement()
    {
        if (eventSystem == null)
        {
            Debug.Log("This item has no EventSystem referenced yet.");
        }

        if (uiElementToSelect == null)
        {
            Debug.Log("This should jump where? ", this);
        }

        eventSystem.SetSelectedGameObject(uiElementToSelect.gameObject);
    }


    //--------------------


    public void Button_isPressed()
    {
        MenuStates.Instance.ChangeMenuState(newMenuState);

        JumpToElement();
    }
}
