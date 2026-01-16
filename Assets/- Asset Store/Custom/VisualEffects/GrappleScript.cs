using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GrappleScript : MonoBehaviour
{
    ParticleSystem PS;
    List<ParticleCollisionEvent> collisionEvent;
    [SerializeField] LineRenderer line;
    [SerializeField] GameObject decal;
    void Start()
    {
        PS = GetComponent<ParticleSystem>();
        collisionEvent = new List<ParticleCollisionEvent>();
    }

    void Update()
    {
        //Throw the grappling hook when pressing '1'
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PS.Play();
        }

        //Reset the line renderer and decal when pressing '2'
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Reset the line renderer
            line.SetPosition(1, Vector3.zero);

            //Reset the decal
            decal.SetActive(false);
            decal.transform.localPosition = Vector3.zero;
        }
    }

    //Activate the line renderer and decal when particle collides
    void OnParticleCollision(GameObject other)
    {
        //Set the position of point 1 of the line renderer
        PS.GetCollisionEvents(other, collisionEvent);
        line.SetPosition(1, transform.InverseTransformPoint(collisionEvent[0].intersection));

        //Move and activate the decal
        decal.transform.position = collisionEvent[0].intersection;
        decal.SetActive(true);
    }
}
