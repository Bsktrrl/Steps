using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    public string walk;

    public string ability_Ascend;
    public string ability_Descend;
    public string ability_Dash;
    public string ability_Jump;
    public string ability_CeilingGrab;
    public string ability_GrapplingHook;

    public string effect_Teleport;
    public string effect_PickupSmall;
    public string effect_PickupBig;

    public string blink;
    public bool talking;

    public List<string> animationList = new List<string>();
}
