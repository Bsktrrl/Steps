using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    public void button_isPressed()
    {
        print("Button: " + gameObject.name + " is pressed");
    }
}
