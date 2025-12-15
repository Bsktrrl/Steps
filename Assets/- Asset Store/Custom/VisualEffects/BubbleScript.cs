using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    Animator anim;

    [SerializeField] float rangeMax = 0.5f;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("CycleOffset", Random.Range(0f, rangeMax));
    }
}
