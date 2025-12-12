using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    public string walk;

    public string ability_AscendDescend;
    public string ability_Dash;
    public string ability_Jump;
    public string ability_CeilingGrab;
    public string ability_GrapplingHook;

    public string blink;
    public bool talking;

    public List<string> animationList = new List<string>();
}
