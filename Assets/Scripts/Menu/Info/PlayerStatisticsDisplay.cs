using TMPro;
using UnityEngine;

public class PlayerStatisticsDisplay : MonoBehaviour
{
    public TextMeshProUGUI stats_LeftSide;
    public TextMeshProUGUI stats_RightSide;

    public void SetupStatsDisplayLeft()
    {
        //Fill the stats_LeftSide with text, using the text from textData.options_Info_StatsLeft_Text

        //Format the text to become in this format:
        //        "<size=120%><u>General</u>
        //< size = 100 %> Progress: 
        //Playtime:
        //    Sessions:
        //        Longest session: 

        //< size = 120 %>< u > Steps </ u >
        //< size = 100 %> Steps taken:
        //        Steps replenished: 
        //Steps emptied: 
        //Average steps per level: 

        //< size = 120 %>< u > Interactables </ u >
        //< size = 100 %> Essence gathered:
        //        Skins found: 
        //Skins bought: 
        //Tablets red: 
        //Dialogue interactions: "

        //Add one line at the time to the text, and after each ": " insert the value for that entry.
        //Insert all time values as Hours:Minute.
        //Insert averages with 2 decimals.
        //Insert all int as on number with no decimals
        //Insert Progress with % after and 0 decimals.
    }
    public void SetupStatsDisplayRight()
    {
        //Fill the stats_RightSide with text textData.options_Info_StatsRight_Text

        //Format the text to become in this format:
        //"<size=120%><u>Levels</u>
        //< size = 100 %> Regions completed:
        //        Levels completed: 
        //Levels revisited: 
        //Fastest level completion:
        //        Slowest level completion:
        //        Average level completion:
        //    Respawns:
        //        Time standing still:
        //        Time in free camera: 
        //Hints used: 

        //< size = 120 %>< u > Abilities </ u >
        //< size = 100 %> Abilities Performed:
        //    Jumps:
        //    Grapples:
        //        Hand Drills: 
        //Drill Helmet: 
        //Drill Boots: 
        //Swim Distance: 
        //Climb Distance: "

        //Add one line at the time to the text, and after each ": " insert the value for that entry.
        //Insert all time values as Hours:Minute.
        //Insert averages with 2 decimals.
        //Insert all int as on number with no decimals
    }
}
