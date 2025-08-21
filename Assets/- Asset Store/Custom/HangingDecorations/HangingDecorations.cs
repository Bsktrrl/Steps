using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingDecorations : MonoBehaviour
{
    Animator anim;
    [SerializeField] [Range(0, 3)] int SwingStrength = 1;
    [SerializeField] [Range(1, 2)] float SwingSpeed = 1;
    float RandomSpeed;
    float RandomSpeedMinMax = 0.15f;
    void Start()
    {
        anim = GetComponent<Animator>();
        RandomSpeed = Random.Range(-RandomSpeedMinMax, RandomSpeedMinMax);
    }

    void Update()
    {
        anim.SetInteger("SwingStrength", SwingStrength);
        float ActualSwingSpeed = SwingSpeed + RandomSpeed;
        anim.speed = ActualSwingSpeed;
    }
}
