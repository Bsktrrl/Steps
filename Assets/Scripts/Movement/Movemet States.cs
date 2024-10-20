using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    Still,

    Moving
}

public enum MovementDirection
{
    None,

    Forward,
    Backward,
    Left,
    Right
}

public enum DetectorPoint
{
    Center, 

    Front,
    Back,
    Right,
    Left
}