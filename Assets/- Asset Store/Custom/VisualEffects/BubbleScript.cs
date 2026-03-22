using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    Animator anim;

    [SerializeField] float newSpeed;
    [SerializeField] float maxOffset;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = newSpeed + Random.Range(-0.1f, 0.1f);
        anim.SetFloat("CycleOffset", Random.Range(0f, maxOffset));
    }
}