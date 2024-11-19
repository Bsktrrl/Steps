using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_ToPress : MonoBehaviour
{
    [SerializeField] MenuState newMenuState;


    //--------------------


    public void Button_isPressed()
    {
        MenuStates.Instance.ChangeMenuState(newMenuState);
    }
}
