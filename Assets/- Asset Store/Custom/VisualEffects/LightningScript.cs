using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightningScript : MonoBehaviour
{
    ParticleSystem PS;

    float waitTime = 0.015f;

    [SerializeField] Light light;
    float lightIntensity;
    float lightMaxIntensity = 0.5f;
    float lightSpeed;

    float ambientIntensity;
    float ambientBaseIntensity;

    float shaderIntensity;
    float shaderTargetIntensity;
    float shaderMaxIntensity = 0.25f;

    Vector3 lightningPosition;
    void Start()
    {
        PS = GetComponent<ParticleSystem>();
        ambientBaseIntensity = UnityEngine.RenderSettings.ambientIntensity;
        ambientIntensity = ambientBaseIntensity;

        //Reset global shader values when play starts
        Shader.SetGlobalVector("_LightningDirection", Vector3.zero);
        Shader.SetGlobalFloat("_LightningIntensity", 0);
    }

    void Update()
    {
        //Summon lightning by pressing '1'
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(Lightning());
        }

        //Intensity of the directional light attached to the lightning
        if (light.intensity != lightIntensity)
        {
            light.intensity = Mathf.MoveTowards(light.intensity, lightIntensity, lightSpeed * Time.deltaTime);
        }

        //Intensity of the ambient light in the scene
        ambientIntensity = ambientBaseIntensity * (1 + lightIntensity);
        if (UnityEngine.RenderSettings.ambientIntensity != ambientIntensity)
        {
            UnityEngine.RenderSettings.ambientIntensity = Mathf.MoveTowards(UnityEngine.RenderSettings.ambientIntensity, ambientIntensity, lightSpeed * Time.deltaTime);
        }

        //Intensity of the lightning on the skybox shader
        if (shaderIntensity != shaderTargetIntensity)
        {
            shaderIntensity = Mathf.MoveTowards(shaderIntensity, shaderTargetIntensity, lightSpeed * Time.deltaTime);
        }

        if (Shader.GetGlobalFloat("_LightningIntensity") != shaderIntensity)
        {
            Shader.SetGlobalFloat("_LightningIntensity", shaderIntensity);
        }
    }

    //Lightning animation
    IEnumerator Lightning()
    {
        lightningPosition = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        lightningPosition.Normalize();
        lightningPosition *= 1000;
        lightningPosition += Vector3.up * 500;
        transform.position = lightningPosition;
        light.transform.LookAt(Vector3.zero);
        PS.Play();
        lightSpeed = 7;
        lightIntensity = lightMaxIntensity;
        Shader.SetGlobalVector("_LightningDirection", Vector3.Normalize(light.transform.position));
        shaderTargetIntensity = shaderMaxIntensity;

        yield return new WaitForSeconds(waitTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(waitTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(waitTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(waitTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(waitTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(waitTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(waitTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(waitTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(waitTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(waitTime);

        PS.TriggerSubEmitter(0);
        lightIntensity = 0;
        shaderTargetIntensity = shaderMaxIntensity * 0.5f;

        yield return new WaitForSeconds(0.1f);

        lightIntensity = lightMaxIntensity;
        shaderTargetIntensity = shaderMaxIntensity;

        yield return new WaitForSeconds(0.5f);

        lightIntensity = 0;
        lightSpeed = 1;
        shaderTargetIntensity = 0;
    }


    //Reset global shader values when play ends
    private void OnDisable()
    {
        Shader.SetGlobalVector("_LightningDirection", Vector3.zero);
        Shader.SetGlobalFloat("_LightningIntensity", 0);
    }
}
