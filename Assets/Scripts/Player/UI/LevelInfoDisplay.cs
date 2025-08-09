using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoDisplay : Singleton<LevelInfoDisplay>
{
    [Header("LevelInfo Parent")]
    [SerializeField] GameObject LevelInfo_Parent;

    [Header("Info")]
    public GameObject levelName;
    public GameObject AbilityHeader;
    [SerializeField] GameObject coinAmount;
    [SerializeField] GameObject collectableAmount;
    [SerializeField] GameObject stepAmount;

    [SerializeField] Image levelImage;

    [Header("Ability Sprites")]
    [SerializeField] Image ability_FenceSneak;
    [SerializeField] Image ability_SwimSuit;
    [SerializeField] Image ability_SwiftSwim;
    [SerializeField] Image ability_Flippers;
    [SerializeField] Image ability_LavaSuit;
    [SerializeField] Image ability_LavaSwiftSwim;
    [SerializeField] Image ability_HikerGear;
    [SerializeField] Image ability_IceSpikes;
    [SerializeField] Image ability_GrapplingHook;
    [SerializeField] Image ability_Hammer;
    [SerializeField] Image ability_ClimbingGear;
    [SerializeField] Image ability_Dash;
    [SerializeField] Image ability_Ascend;
    [SerializeField] Image ability_Descend;
    [SerializeField] Image ability_ControlStick;

    [Header("Ability Got Sprites")]
    [SerializeField] Image ability_FenceSneak_Got;
    [SerializeField] Image ability_SwimSuit_Got;
    [SerializeField] Image ability_SwiftSwim_Got;
    [SerializeField] Image ability_Flippers_Got;
    [SerializeField] Image ability_LavaSuit_Got;
    [SerializeField] Image ability_LavaSwiftSwim_Got;
    [SerializeField] Image ability_HikerGear_Got;
    [SerializeField] Image ability_IceSpikes_Got;
    [SerializeField] Image ability_GrapplingHook_Got;
    [SerializeField] Image ability_Hammer_Got;
    [SerializeField] Image ability_ClimbingGear_Got;
    [SerializeField] Image ability_Dash_Got;
    [SerializeField] Image ability_Ascend_Got;
    [SerializeField] Image ability_Descend_Got;
    [SerializeField] Image ability_ControlStick_Got;


    //--------------------


    private void OnEnable()
    {
        HideDisplayLevelInfo();
    }


    //--------------------


    public void ShowDisplayLevelInfo(Map_SaveInfo mapInfo, LoadLevel level)
    {
        HideDisplayLevelInfo();

        //Display MapName
        if (mapInfo != null)
            levelName.GetComponent<TextMeshProUGUI>().text = mapInfo.mapName;
        else
            levelName.GetComponent<TextMeshProUGUI>().text = level.levelToPlay;


        //Display Level Image
        levelImage.sprite = level.levelSprite;

        //Display Coins got
        if (mapInfo != null)
        {
            int coinCounter = 0;
            for (int i = 0; i < mapInfo.coinList.Count; i++)
            {
                if (mapInfo.coinList[i].isTaken)
                {
                    coinCounter++;
                }
            }

            coinAmount.GetComponentInChildren<TextMeshProUGUI>().text = coinCounter + " / 10";
        }
        else
        {
            coinAmount.GetComponentInChildren<TextMeshProUGUI>().text = "0 / 10";
        }


        //Display Coins got
        if (mapInfo != null)
        {
            int collectionCounter = 0;
            for (int i = 0; i < mapInfo.skinsList.Count; i++)
            {
                if (mapInfo.skinsList[i].isTaken)
                {
                    collectionCounter++;
                }
            }

            collectableAmount.GetComponentInChildren<TextMeshProUGUI>().text = collectionCounter + " / 3";
        }
        else
        {
            collectableAmount.GetComponentInChildren<TextMeshProUGUI>().text = "0 / 3";
        }


        //Display Coins got
        if (mapInfo != null)
        {
            int stepCounter = 0;
            for (int i = 0; i < mapInfo.maxStepList.Count; i++)
            {
                if (mapInfo.maxStepList[i].isTaken)
                {
                    stepCounter++;
                }
            }

            stepAmount.GetComponentInChildren<TextMeshProUGUI>().text = stepCounter + " / 3";
        }
        else
        {
            stepAmount.GetComponentInChildren<TextMeshProUGUI>().text = "0 / 3";
        }

        //Display all abilities in the Level
        if (level.abilitiesInLevel.SwimSuit)
            ability_SwimSuit.gameObject.SetActive(true);
        if (level.abilitiesInLevel.SwiftSwim)
            ability_SwiftSwim.gameObject.SetActive(true);
        if (level.abilitiesInLevel.Flippers)
            ability_Flippers.gameObject.SetActive(true);
        if (level.abilitiesInLevel.Jumping)
            ability_LavaSwiftSwim.gameObject.SetActive(true);
        if (level.abilitiesInLevel.GrapplingHook)
            ability_GrapplingHook.gameObject.SetActive(true);
        if (level.abilitiesInLevel.CeilingGrab)
            ability_ClimbingGear.gameObject.SetActive(true);
        if (level.abilitiesInLevel.Dash)
            ability_Dash.gameObject.SetActive(true);
        if (level.abilitiesInLevel.Ascend)
            ability_Ascend.gameObject.SetActive(true);
        if (level.abilitiesInLevel.Descend)
            ability_Descend.gameObject.SetActive(true);

        //Show all Displays
        LevelInfo_Parent.SetActive(true);
    }

    public void HideDisplayLevelInfo()
    {
        LevelInfo_Parent.SetActive(false);

        ability_FenceSneak.gameObject.SetActive(false);
        ability_SwimSuit.gameObject.SetActive(false);
        ability_SwiftSwim.gameObject.SetActive(false);
        ability_Flippers.gameObject.SetActive(false);
        ability_LavaSuit.gameObject.SetActive(false);
        ability_LavaSwiftSwim.gameObject.SetActive(false);
        ability_HikerGear.gameObject.SetActive(false);
        ability_IceSpikes.gameObject.SetActive(false);
        ability_GrapplingHook.gameObject.SetActive(false);
        ability_Hammer.gameObject.SetActive(false);
        ability_ClimbingGear.gameObject.SetActive(false);
        ability_Dash.gameObject.SetActive(false);
        ability_Ascend.gameObject.SetActive(false);
        ability_Descend.gameObject.SetActive(false);
        ability_ControlStick.gameObject.SetActive(false);

        ability_FenceSneak_Got.gameObject.SetActive(false);
        ability_SwimSuit_Got.gameObject.SetActive(false);
        ability_SwiftSwim_Got.gameObject.SetActive(false);
        ability_Flippers_Got.gameObject.SetActive(false);
        ability_LavaSuit_Got.gameObject.SetActive(false);
        ability_LavaSwiftSwim_Got.gameObject.SetActive(false);
        ability_HikerGear_Got.gameObject.SetActive(false);
        ability_IceSpikes_Got.gameObject.SetActive(false);
        ability_GrapplingHook_Got.gameObject.SetActive(false);
        ability_Hammer_Got.gameObject.SetActive(false);
        ability_ClimbingGear_Got.gameObject.SetActive(false);
        ability_Dash_Got.gameObject.SetActive(false);
        ability_Ascend_Got.gameObject.SetActive(false);
        ability_Descend_Got.gameObject.SetActive(false);
        ability_ControlStick_Got.gameObject.SetActive(false);
    }
}
