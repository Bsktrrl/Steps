using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepParticleScript : MonoBehaviour
{
    ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        //Press '1' to activate the particle effect
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            particle.Play();
        }

        gameObject.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
    }
}
