using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeScript : MonoBehaviour
{
    //Hole Shader
    GameObject player;

    //Depth of Field
    Vector3 blurDirection;
    float blurDistance;
    float conversionValue = 2.13f;
    float conversionCalculation;
    float blurSpeed = 10f;
    float directionDistance = 5.6f;

    void Start()
    {
        //Hole Shader
        player = GameObject.FindWithTag("Player");

        //Depth of Field
        Shader.SetGlobalFloat("_DOFEnabled", 1);
    }

    void Update()
    {
        //Hole Shader
        if(player != null)
        {
            Shader.SetGlobalVector("_PlayerPosition", player.transform.position);
        }

        //Depth of Field
        RaycastHit hit;

        blurDirection = Camera.main.transform.forward;
        blurDirection = Vector3.RotateTowards(blurDirection, -Camera.main.transform.up, 0.1f, 0f);

        conversionCalculation = conversionValue / (Camera.main.fieldOfView / 58);

        if (Physics.Raycast(Camera.main.transform.position, blurDirection, out hit, directionDistance))
        {
            blurDistance = Mathf.Lerp(Shader.GetGlobalFloat("_BlurDistance"), Vector3.Distance(Camera.main.transform.position, hit.point) / conversionCalculation, blurSpeed * Time.deltaTime);
        }
        else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            blurDistance = Mathf.Lerp(Shader.GetGlobalFloat("_BlurDistance"), Vector3.Distance(Camera.main.transform.position, hit.point) / conversionCalculation, blurSpeed * Time.deltaTime);
        }

        blurDistance = Mathf.Clamp(blurDistance, 1, 100);
        Shader.SetGlobalFloat("_BlurDistance", blurDistance);
    }

    void OnDisable()
    {
        //Hole Shader
        Shader.SetGlobalFloat("_HoleShaderEnabled", 0);

        //Depth of Field
        Shader.SetGlobalFloat("_DOFEnabled", 0);
    }
}
