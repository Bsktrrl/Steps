using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Number_BlendShape : MonoBehaviour
{
    SkinnedMeshRenderer SMR;

    [SerializeField] float duration = 0.25f;
    float currentValue = 100f;

    bool animation_isRunning;


    //--------------------


    void Start()
    {
        SMR = GetComponent<SkinnedMeshRenderer>();
        SMR.SetBlendShapeWeight(0, 100);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && !animation_isRunning)
        {
            if (currentValue <= 0)
            {
                StartCoroutine(NumberAnimation(0, 100, duration));
            }
            else if (currentValue >= 100)
            {
                StartCoroutine(NumberAnimation(100, 0, duration));
            }
        }
    }


    //--------------------


    private IEnumerator NumberAnimation(float start, float end, float time)
    {
        animation_isRunning = true;

        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            currentValue = Mathf.Lerp(start, end, elapsedTime / time);
            SMR.SetBlendShapeWeight(0, currentValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentValue = end;

        animation_isRunning = false;
    }
}
