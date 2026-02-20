using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleShaderOnOffScript : MonoBehaviour
{
    float transition;
    bool transitionBool;
    float transitionSpeed = 7;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transitionBool = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transitionBool = false;
        }

        transition = Mathf.Lerp(transition, (transitionBool ? 1 : 0), transitionSpeed * Time.deltaTime);

        Shader.SetGlobalFloat("_HoleShaderEnabled", transition);
    }
}
