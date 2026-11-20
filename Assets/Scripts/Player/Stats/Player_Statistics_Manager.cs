using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Statistics_Manager : MonoBehaviour
{
    //This script should run once each time the player enters the Statistics menu so the data is fully updated
    //Run this once when ending the game, so all data gets updated correctly
    //Also use the functions in this script to update different part of the project, where they should update, so the saveData is always up to date
    //Run this update when a level exites also, so the data isn't lost

    public void SaveStatistics()
    {
        //Save the updated statistics to the DataManager
    }


    //--------------------


    public void CheckProgress()
    {
        //One level have 15 tings to add to the progress 1 finish, 3 footprints, 1 skin and 10 essenceCubes
        //The game have 31 levels

        //Also count 1 for each skin bought in the game (32 skins)

        //Also count for all NPC dialogues (1 dialogue per level in 25 levels, then Larry in 5 Metalworks levels

        //Add all this together, and make a percentage float to send to save
    }

    public void AddPlayTime(float time)
    {
        //Check the play time since last checkup here or since startup of the game and add it to the saveData. Reset counter to 0 again so it starts counting from 0, so when adding onto the data, it is added correctly
    }

    public void AddSessions()
    {
        //Run when the game starts to add 1 to the session counter
    }
    public void AddLongestSession()
    {
        //Run when the game ends to compare if this session has been the longest.
        //If the menu opens and this is currently the longest session, update the data to the time of this session at the time of opening the menu
    }
    public void AddStepsTaken(int steps)
    {
        //Add the Steps Counter from this session from all leveles. Reset the counter to 0 again, so the next import is correct.
    }
    public void AddStepsReplenished(int steps)
    {
        //Add the Steps Replenished from this session from all leveles. Each time steps are refilled, this counts how many are filled (excluded the ones that is already filled)
    }
    public void AddStepsEmptied(int steps)
    {
        //Add the Steps Emptied from this session from all leveles. The counter moves up each time the player must respawn due to having 0 steps, not by reaching 0 steps when walking onto a checkpoint, since it is not respawning
    }
    public void AddRegionsComplete(int regions)
    {
        //Add when the last level of a region (Lv.5) is completed
    }
    public void AddLevelsComplete(int levels)
    {
        //Add when a level that has not yet been completed, is completed
    }
    public void AddLevelsReplayed(int levels)
    {
        //Add when a level that has been entered before is entered again. Don't count for the first time entering a level, but all other thimes from the second and onward
    }

    public void AddFastestLevelCOmpletion(string name, float time)
    {
        //Add when a level is completed as the fastest time compared to the stored value. Include the name of the level and the time (number) then completed. A level can be completed several times.
    }
    public void AddSlowestLevelCOmpletion(string name, float time)
    {
        //Add when a level is completed as the slowest time compared to the stored value. Include the name of the level and the time (number) then completed. A level can be completed several times.
    }
    public void AddAverageLevelCompletionTime(float time)
    {
        //Add the time used when completing a level to a total level time counter List and find the average based on the amount of entries in the list
    }
    public void AddRespawns(int respawn)
    {
        //Add the respawn counter at this time. Reset the counter to 0 after adding, to add correctly next time
    }
    public void AddTimeStandingStill(float time)
    {
        //Each time the player is standing still, count the time. Add the time to a total. Then reset this counter, to add correctly next time
    }
    public void AddAverageStepsPerLevel(int steps)
    {
        //Add the stepcount after exiting or completing a level to a total steps counter List and find the average based on the amount of entries in the list
    }
    public void AddTimeInFreeCam(float time)
    {
        //Add the time used in freeCam when completing a level to a total freeCam time counter total
    }
    public void AddHintsUsed(int hints)
    {
        //When getting a hint, add +1 to the total hint counter in save
    }


    public void AddAbilitiesPerformed(int abilites)
    {
        //When performing an ability, add +1 to the total ability counter in save
    }
    public void AddAbilitiesPerformed_Jump(int jump)
    {
        //When performing a jump, add +1 to the total jump counter in save
    }
    public void AddAbilitiesPerformed_GrapplingHook(int grapplingHook)
    {
        //When performing grapplingHook ability, add +1 to the total grapplingHook counter in save
    }
    public void AddAbilitiesPerformed_HandDrill(int handDrill)
    {
        //When performing a handDrill ability, add +1 to the total handDrill counter in save
    }
    public void AddAbilitiesPerformed_DrillHelmet(int drillHelmet)
    {
        //When performing a derillHelmet ability, add +1 to the total derillHelmet counter in save
    }
    public void AddAbilitiesPerformed_DrillBoots(int drillBoots)
    {
        //When performing a drillBoots ability, add +1 to the total drillBoots counter in save
    }
    public void AddAbilitiesPerformed_SwimDistance(int swim)
    {
        //When moving 1 space in water, add +1 to the total swimDistance counter in save
    }
    public void AddAbilitiesPerformed_CeilingClimb(int climb)
    {
        //When moving 1 space with the ceilingGrab ability, add +1 to the total ceilingGrab counter in save
    }

    public void AddPickupEssence(int essence)
    {
        //When picking up an essence, add +1 to the essence counter in save
    }
    public void AddPickupSkin(int skin)
    {
        //When picking up a skin, add +1 to the skin counter in save
    }
    public void AddSkinBought(int skinBought)
    {
        //When buying a skin, add +1 to the skinBought counter in save
    }
    public void AddTabletRed(int tablet)
    {
        //When reading a specific tablet for the first time, add +1 to the tablet counter in save
    }
    public void AddDialogueInterracted(int dialogue)
    {
        //When interracting with an NPC for the first time in a level by pressing "talk"-button, add +1 to the NCPDialogue counter in save
    }
}

[Serializable]
public class PlayerStatistics
{
    public float progress;
    public float playTime;
    public int sessions;
    public float longestSession;

    public int stepsTaken;
    public int stepsReplenished;
    public int stepsEmptied;

    public int regionsCompleted;
    public int levelsCompleted;
    public int levelsReplayed;
    public float fastestLevelCompletion_Name;
    public float fastestLevelCompletion_Time;
    public float slowestLevelCompletion_Name;
    public float slowestLevelCompletion_Time;
    public int respawnsTaken;
    public float averageLevelCompletionTime;
    public float timeStandingStill;
    public float averageStepsPerLevel;
    public float TimeInFreeCam;
    public int hintsUsed;

    public int abilitiesPerformed;
    public int ability_Jump_Performed;
    public int ability_GrapplingHook_Performed;
    public int ability_HandDrill_Performed;
    public int ability_DrillHelmet_Performed;
    public int ability_DrillBoots_Performed;
    public int ability_SwimDistance_Performed;
    public int ability_CeilingClimbDisatnace_Performed;

    public int pickup_EssencePickedUp;
    public int pickup_SkinsFound;
    public int pickup_SkinsBought;
    public int pickup_TabletsRed;
    public int pickup_DialoguesInterracted;
}