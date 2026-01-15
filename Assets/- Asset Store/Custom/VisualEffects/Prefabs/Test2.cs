using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    ParticleSystem PS;
    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PS.Clear();
            print("Cleared");
        }
    }
}
