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
    float sphereCastRadius = 0.25f;

    PlayerStats playerStats;
    float vignetteStrength;
    float vignetteSpeed = 25;

    void Start()
    {
        //Hole Shader
        player = GameObject.FindWithTag("Player");

        //Depth of Field
        Shader.SetGlobalFloat("_DOFEnabled", 1);

        playerStats = PlayerStats.Instance;
    }

    void Update()
    {
        //Hole Shader
        if(player != null)
        {
            Shader.SetGlobalVector("_PlayerPosition", player.transform.position);
        }

        //Depth of Field
        Camera camera = Camera.main;
        if (!camera) return;

        Transform view = camera.transform;
        RaycastHit hit;

        blurDirection = view.forward;
        blurDirection = Vector3.RotateTowards(blurDirection, -view.up, 0.1f, 0f);

        conversionCalculation = conversionValue / (camera.fieldOfView / 58);

        if (Physics.SphereCast(view.position, sphereCastRadius, blurDirection, out hit, directionDistance, LayerMask.GetMask("Player")))
        {
            blurDistance = Mathf.Lerp(Shader.GetGlobalFloat("_BlurDistance"), Vector3.Distance(view.position, hit.point) / conversionCalculation, blurSpeed * Time.deltaTime);
        }
        else if (Physics.Raycast(view.position, view.forward, out hit))
        {
            blurDistance = Mathf.Lerp(Shader.GetGlobalFloat("_BlurDistance"), Vector3.Distance(view.position, hit.point) / conversionCalculation, blurSpeed * Time.deltaTime);
        }

        blurDistance = Mathf.Clamp(blurDistance, 1, 100);
        Shader.SetGlobalFloat("_BlurDistance", blurDistance);

        //Low Steps Vignette
        vignetteStrength = Mathf.Lerp(vignetteStrength, playerStats.stats.steps_Current, vignetteSpeed * Time.deltaTime);
        Shader.SetGlobalFloat("_StepsAmount", vignetteStrength);
    }

    void OnDisable()
    {
        //Hole Shader
        Shader.SetGlobalFloat("_HoleShaderEnabled", 0);
        Shader.SetGlobalFloat("_CameraHoleEnabled", 0);
        Shader.SetGlobalFloat("_PlayerOffset", 0);

        //Depth of Field
        Shader.SetGlobalFloat("_DOFEnabled", 0);

        //Low Steps Vignette
        Shader.SetGlobalFloat("_StepsAmount", 7);
    }
}
