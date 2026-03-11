using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingDecorations : MonoBehaviour
{
    Animator anim;

    [SerializeField] [Range(0, 3)] int swingStrength = 1;
    [SerializeField] [Range(1, 3)] float swingSpeed = 1;
    [SerializeField] [Range(0, 1)] float windStrength;

    float randomSpeed;
    float randomSpeedMinMax = 0.15f;
    void Start()
    {
        anim = GetComponent<Animator>();
        randomSpeed = Random.Range(-randomSpeedMinMax, randomSpeedMinMax);
    }

    void Update()
    {
        anim.SetLayerWeight(1, windStrength);
        anim.SetInteger("SwingStrength", swingStrength);

        float ActualSwingSpeed = swingSpeed + randomSpeed;
        anim.speed = ActualSwingSpeed;
    }
}
