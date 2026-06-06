using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueplantPickedScript : MonoBehaviour
{
    [SerializeField] bool picked;

    [SerializeField] GameObject glueplant;
    [SerializeField] GameObject glueplantPicked;

    void Start()
    {
        if (picked && glueplant.activeSelf)
        {
            Pick();
        }
    }

    void Pick()
    {
        glueplant.SetActive(false);
        glueplantPicked.SetActive(true);
    }
}
