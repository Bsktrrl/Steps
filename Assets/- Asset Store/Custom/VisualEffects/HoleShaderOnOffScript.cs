using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleShaderOnOffScript : Singleton<HoleShaderOnOffScript>
{
    float transition;
    bool transitionBool;
    float transitionSpeed = 7;


    //--------------------


    //private void OnEnable()
    //{
    //    CameraController.Action_RotateCamera_Start += HoleShader_On;
    //    CameraController.Action_RotateCamera_End += HoleShader_Off;
    //}
    //private void OnDisable()
    //{
    //    CameraController.Action_RotateCamera_Start -= HoleShader_On;
    //    CameraController.Action_RotateCamera_End -= HoleShader_Off;
    //}

    private void Update()
    {
        if (transitionBool)
        {
            transition = Mathf.Lerp(transition, (transitionBool ? 1 : 0), transitionSpeed * Time.deltaTime);

            Shader.SetGlobalFloat("_HoleShaderEnabled", transition);
        }
        else
        {
            transition = Mathf.Lerp(transition, (transitionBool ? 1 : 0), transitionSpeed * Time.deltaTime);

            Shader.SetGlobalFloat("_HoleShaderEnabled", transition);
        }
    }


    //--------------------


    public void HoleShader_On()
    {
        print("1. HoleShader_On");

        transitionBool = true;
    }
    public void HoleShader_Off()
    {
        print("2. HoleShader_Off");

        transitionBool = false;
    }
}
