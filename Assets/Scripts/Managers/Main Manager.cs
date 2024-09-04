using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    public Collectables collectables;
}

[Serializable]
public class Collectables
{
    public int coin;
}