using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepParticleScript : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;


    //--------------------


    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
    }


    //--------------------


    public void Perform_CheckpointEffect()
    {
        particle.Play();
    }
}
