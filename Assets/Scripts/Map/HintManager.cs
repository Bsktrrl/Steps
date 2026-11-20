using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public List<HintSetup> HintSetups_List = new List<HintSetup>();

    public float hintDuration = 4;

    public bool CheckIfHint()
    {
        //Check if one entry on the list matches the player's situation

        return true;
    }

    public void PerformHint()
    {
        if (CheckIfHint())
        {
            //Perform the Hint effect

            //Let the effect be active for "hintDuration" time

            //If hint-button is pressed before this is finished, end this and start a new hint session, checking for which hint to activate 
        }
    }
}

[Serializable]
public class HintSetup
{
    public int maxStepsGot;
    public List<Abilities> abilitiesGot = new List<Abilities>();

    public GameObject checkpointToLighten;
}