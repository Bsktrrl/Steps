using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightningScript : MonoBehaviour
{
    ParticleSystem PS;
    AudioSource source;

    float pitchMin = 0.9f;
    float pitchMax = 1.1f;

    bool lightning;

    float subEmitterTime = 0.015f;
    float waitTimeMin = 1f;
    float waitTimeMax = 10f;

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
        source = GetComponent<AudioSource>();
        ambientBaseIntensity = UnityEngine.RenderSettings.ambientIntensity;
        ambientIntensity = ambientBaseIntensity;

        //Reset global shader values when play starts
        Shader.SetGlobalVector("_LightningDirection", Vector3.zero);
        Shader.SetGlobalFloat("_LightningIntensity", 0);
    }

    void Update()
    {
        //Summon lightning by pressing '1'
        if (lightning == false)
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
        lightning = true;

        source.pitch = Random.Range(pitchMin, pitchMax);
        source.Play();

        lightningPosition = Camera.main.transform.forward;
        lightningPosition.y = 0;
        lightningPosition.Normalize();
        lightningPosition = Quaternion.AngleAxis(Random.Range(-90f, 90f), Vector3.up) * lightningPosition;
        lightningPosition *= 1000;
        lightningPosition += Vector3.up * 333;
        transform.position = lightningPosition;

        light.transform.LookAt(Vector3.zero);
        PS.Play();
        lightSpeed = 7;
        lightIntensity = lightMaxIntensity;
        Shader.SetGlobalVector("_LightningDirection", Vector3.Normalize(light.transform.position));
        shaderTargetIntensity = shaderMaxIntensity;

        yield return new WaitForSeconds(subEmitterTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(subEmitterTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(subEmitterTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(subEmitterTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(subEmitterTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(subEmitterTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(subEmitterTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(subEmitterTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(subEmitterTime);

        PS.TriggerSubEmitter(0);

        yield return new WaitForSeconds(subEmitterTime);

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

        yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));

        lightning = false;
    }


    //Reset global shader values when play ends
    private void OnDisable()
    {
        Shader.SetGlobalVector("_LightningDirection", Vector3.zero);
        Shader.SetGlobalFloat("_LightningIntensity", 0);
        UnityEngine.RenderSettings.ambientIntensity = ambientBaseIntensity;
    }
}
