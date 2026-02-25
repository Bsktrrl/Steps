using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RainParticleScript : MonoBehaviour
{
    [SerializeField] float height;
    float actualHeight;
    void Update()
    {
        if (transform.position.y > height - 5 || transform.position.y < height - 50)
        {
            actualHeight = Camera.main.transform.position.y + 5;
        }
        else
        {
            actualHeight = height;
        }

            transform.position = new Vector3(Camera.main.transform.position.x, actualHeight, Camera.main.transform.position.z);
        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
    }
}
