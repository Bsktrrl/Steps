using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticleScript : MonoBehaviour
{
    Vector3 partPos;
    void Update()
    {
        partPos = Camera.main.transform.position + Camera.main.transform.forward * 10;
        partPos.y -= (partPos.y - Camera.main.transform.position.y) * 0.25f;
        transform.position = partPos;
    }
}
