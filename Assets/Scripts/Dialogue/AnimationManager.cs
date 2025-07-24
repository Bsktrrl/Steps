using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    public string walk;
    public string swim;

    public string blink;
    public bool talking;

    public List<string> animationList = new List<string>();
}
