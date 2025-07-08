using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueColors : Singleton<DialogueColors>
{
    [Header("Floriel DialogueBox Colors")]
    public Color floriel_DialogueBox_Normal;
    public Color floriel_DialogueBox_Highlighted;
    public Color floriel_DialogueBox_Pressed;
    public Color floriel_DialogueBox_Selected;

    [Header("Granith DialogueBox Colors")]
    public Color granith_DialogueBox_Normal;
    public Color granith_DialogueBox_Highlighted;
    public Color granith_DialogueBox_Pressed;
    public Color granith_DialogueBox_Selected;

    [Header("Archie​ DialogueBox Colors")]
    public Color archie_DialogueBox_Normal;
    public Color archie_DialogueBox_Highlighted;
    public Color archie_DialogueBox_Pressed;
    public Color archie_DialogueBox_Selected;

    [Header("Aisa​ DialogueBox Colors")]
    public Color aisa_DialogueBox_Normal;
    public Color aisa_DialogueBox_Highlighted;
    public Color aisa_DialogueBox_Pressed;
    public Color aisa_DialogueBox_Selected;

    [Header("Mossy​ DialogueBox Colors")]
    public Color mossy_DialogueBox_Normal;
    public Color mossy_DialogueBox_Highlighted;
    public Color mossy_DialogueBox_Pressed;
    public Color mossy_DialogueBox_Selected;

    [Header("Larry​ DialogueBox Colors")]
    public Color larry_DialogueBox_Normal;
    public Color larry_DialogueBox_Highlighted;
    public Color larry_DialogueBox_Pressed;
    public Color larry_DialogueBox_Selected;

    [Header("Stepellier​ DialogueBox Colors")]
    public Color stepellier_DialogueBox_Normal;
    public Color stepellier_DialogueBox_Highlighted;
    public Color stepellier_DialogueBox_Pressed;
    public Color stepellier_DialogueBox_Selected;
}

public enum NPCs
{
    None,

    Floriel,
    Granith,
    Archie,
    Aisa,
    Mossy,
    Larry,

    Stepellier
}
