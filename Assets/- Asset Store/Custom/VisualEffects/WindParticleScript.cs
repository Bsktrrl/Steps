using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticleScript : MonoBehaviour
{
    Vector3 partPos;

    void Update()
    {
        Camera camera = Camera.main;
        if (!camera) return;

        Transform view = camera.transform;
        partPos = view.position + view.forward * 10;
        partPos.y -= (partPos.y - view.position.y) * 0.25f;
        transform.position = partPos;
    }
}
