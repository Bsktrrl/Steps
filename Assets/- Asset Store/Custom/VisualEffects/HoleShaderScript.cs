using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleShaderScript : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if(player != null)
        {
            Shader.SetGlobalVector("_PlayerPosition", player.transform.position);
        }
    }

    void OnDisable()
    {
        Shader.SetGlobalFloat("_HoleShaderEnabled", 0);
    }
}
