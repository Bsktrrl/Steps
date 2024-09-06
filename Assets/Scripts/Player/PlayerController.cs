using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [Header("Player Object")]
    public GameObject playerObject;

    [Header("Center PlayerDetector")]
    public GameObject detector_Center;

    [Header("Vertical PlayerDetector")]
    public GameObject detector_Vertical_Forward;
    public GameObject detector_Vertical_Backward;
    public GameObject detector_Vertical_Right;
    public GameObject detector_Vertical_Left;

    [Header("Horizontal PlayerDetector")]
    public GameObject detector_Horizontal_Forward;
    public GameObject detector_Horizontal_Backward;
    public GameObject detector_Horizontal_Right;
    public GameObject detector_Horizontal_Left;

    [Header("Platforms Detected")]
    public GameObject platform_Center_Previous;
    public GameObject platform_Center;

    public GameObject platform_Vertical_Forward;
    public GameObject platform_Vertical_Backward;
    public GameObject platform_Vertical_Right;
    public GameObject platform_Vertical_Left;

    public GameObject platform_Horizontal_Forward;
    public GameObject platform_Horizontal_Backward;
    public GameObject platform_Horizontal_Right;
    public GameObject platform_Horizontal_Left;
}
