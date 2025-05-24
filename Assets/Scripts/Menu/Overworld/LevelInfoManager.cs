using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoManager : Singleton<LevelInfoManager>
{
    [Header("LevelDisplay")]
    [SerializeField] GameObject levelDisplay_Parent;
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] Image levelImage;
    [SerializeField] Image skinImage;

    [SerializeField] TextMeshProUGUI glueplant_Aquired;
    [SerializeField] TextMeshProUGUI Essence_Aquired;
    [SerializeField] TextMeshProUGUI Skin_Aquired;
    [SerializeField] TextMeshProUGUI StepsMax_Aquired;

    [SerializeField] GameObject ability_Swimming;
    [SerializeField] GameObject ability_SwiftSwim;
    [SerializeField] GameObject ability_Flippers;
    [SerializeField] GameObject ability_Ascend;
    [SerializeField] GameObject ability_Descend;
    [SerializeField] GameObject ability_Dash;
    [SerializeField] GameObject ability_Jumping;
    [SerializeField] GameObject ability_GrapplingHook;
    [SerializeField] GameObject ability_CeilingGrab;


    [Header("Sprites")]
    #region Sprites
    public Sprite Water_Grass;
    public Sprite Water_Water;
    public Sprite Water_Wood;
    public Sprite Water_4;
    public Sprite Water_5;
    public Sprite Water_6;

    public Sprite Cave_Stone;
    public Sprite Cave_Stone_Brick;
    public Sprite Cave_Lava;
    public Sprite Cave_Rock;
    public Sprite Cave_Brick_Brown;
    public Sprite Cave_Brick_Black;

    public Sprite Desert_Sand;
    public Sprite Desert_Clay;
    public Sprite Desert_Clay_Tiles;
    public Sprite Desert_Sandstone;
    public Sprite Desert_Sandstone_Swirl;
    public Sprite Desert_Quicksand;

    public Sprite Winter_Snow;
    public Sprite Winter_Ice;
    public Sprite Winter_ColdWood;
    public Sprite Winter_FrozenGrass;
    public Sprite Winter_CrackedIce;
    public Sprite Winter_Crocked;

    public Sprite Swamp_SwampWater;
    public Sprite Swamp_Mud;
    public Sprite Swamp_SwampGrass;
    public Sprite Swamp_JungleWood;
    public Sprite Swamp_SwampWood;
    public Sprite Swamp_TempleBlock;

    public Sprite Industrial_Metal;
    public Sprite Industrial_Brass;
    public Sprite Industrial_Gold;
    public Sprite Industrial_Casing_Metal;
    public Sprite Industria_Casingl_Brass;
    public Sprite Industrial_Casing_Gold;
    #endregion


    //--------------------


    private void OnEnable()
    {
        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement += SetupLevelDisplay;
    }
    private void OnDisable()
    {
        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement -= SetupLevelDisplay;
    }


    //--------------------


    public void SetupLevelDisplay()
    {
        GameObject activeLevelObject = RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement;

        if (activeLevelObject.GetComponent<LoadLevel>())
        {
            bool foundLevel = false;

            //Get Level Image
            if (activeLevelObject.GetComponent<LoadLevel>().levelSprite)
                levelImage.sprite = activeLevelObject.GetComponent<LoadLevel>().levelSprite;
            else
                levelImage.sprite = null;

                //Level Name
                levelName.text = activeLevelObject.GetComponent<LoadLevel>().levelToPlay;

            //Find the correct mapInfo
            if (MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List.Count > 0)
            {
                for (global::System.Int32 i = 0; i < MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List.Count; i++)
                {
                    if (MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[i].mapName == activeLevelObject.GetComponent<LoadLevel>().levelToPlay)
                    {
                        //Glueplant aquired
                        if (MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[i].isCompleted)
                            glueplant_Aquired.text = "1 / 1";
                        else
                            glueplant_Aquired.text = "0 / 1";

                        //Essence aquired
                        int essenceCounter = 0;
                        for (int j = 0; j < MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[i].coinList.Count; j++)
                        {
                            if (MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[i].coinList[j].isTaken)
                            {
                                essenceCounter++;
                            }
                        }
                        Essence_Aquired.text = essenceCounter + " / 10";

                        //Skin aquired
                        int skinCounter = 0;
                        for (int j = 0; j < MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[i].collectableList.Count; j++)
                        {
                            if (MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[i].collectableList[j].isTaken)
                            {
                                skinCounter++;
                            }
                        }
                        Skin_Aquired.text = skinCounter + " / 1";

                        //StepMax aquired
                        int stepsCounter = 0;
                        for (int j = 0; j < MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[i].maxStepList.Count; j++)
                        {
                            if (MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[i].maxStepList[j].isTaken)
                            {
                                stepsCounter++;
                            }
                        }
                        StepsMax_Aquired.text = stepsCounter + " / 3";

                        foundLevel = true;
                        break;
                    }
                }
            }
            
            //Get Skin Type to get in the Level
            skinImage.sprite = SelectSpriteForLevel(activeLevelObject.GetComponent<LoadLevel>().skinTypeInLevel);

            //Get Abilities to get in the Level
            ability_Swimming.SetActive(false);
            ability_SwiftSwim.SetActive(false);
            ability_Flippers.SetActive(false);

            ability_Ascend.SetActive(false);
            ability_Descend.SetActive(false);

            ability_Dash.SetActive(false);
            ability_Jumping.SetActive(false);
            ability_GrapplingHook.SetActive(false);
            ability_CeilingGrab.SetActive(false);

            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.SwimSuit)
                ability_Swimming.SetActive(true);
            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.SwiftSwim)
                ability_SwiftSwim.SetActive(true);
            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.Flippers)
                ability_Flippers.SetActive(true);

            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.Ascend)
                ability_Ascend.SetActive(true);
            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.Descend)
                ability_Descend.SetActive(true);

            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.Dash)
                ability_Dash.SetActive(true);
            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.Jumping)
                ability_Jumping.SetActive(true);
            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.GrapplingHook)
                ability_GrapplingHook.SetActive(true);
            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.CeilingGrab)
                ability_CeilingGrab.SetActive(true);

            if (!foundLevel)
            {
                SetupDefaultLevelDisplay();
            }

            if (activeLevelObject.GetComponent<LoadLevel>().readyToBePlayedAndDisplayed)
                levelDisplay_Parent.SetActive(true);
            else
                levelDisplay_Parent.SetActive(false);

            return;
        }

        levelDisplay_Parent.SetActive(false);
        return;
    }

    void SetupDefaultLevelDisplay()
    {
        //Glueplant aquired
        glueplant_Aquired.text = "0 / 1";

        //Essence aquired
        Essence_Aquired.text = "0 / 10";

        //Skin aquired
        Skin_Aquired.text = "0 / 1";

        //StepMax aquired
        StepsMax_Aquired.text = "0 / 3";
    }

    public Sprite SelectSpriteForLevel(SkinType skinType)
    {
        switch (skinType)
        {
            case SkinType.None:
                return null;

            case SkinType.Water_Grass:
                return Water_Grass;
            case SkinType.Water_Water:
                return Water_Water;
            case SkinType.Water_Wood:
                return Water_Wood;
            case SkinType.Water_4:
                return Water_4;
            case SkinType.Water_5:
                return Water_5;
            case SkinType.Water_6:
                return Water_6;

            case SkinType.Cave_Stone:
                return Cave_Stone;
            case SkinType.Cave_Stone_Brick:
                return Cave_Stone_Brick;
            case SkinType.Cave_Lava:
                return Cave_Lava;
            case SkinType.Cave_Rock:
                return Cave_Rock;
            case SkinType.Cave_Brick_Brown:
                return Cave_Brick_Brown;
            case SkinType.Cave_Brick_Black:
                return Cave_Brick_Black;

            case SkinType.Desert_Sand:
                return Desert_Sand;
            case SkinType.Desert_Clay:
                return Desert_Clay;
            case SkinType.Desert_Clay_Tiles:
                return Desert_Clay_Tiles;
            case SkinType.Desert_Sandstone:
                return Desert_Sandstone;
            case SkinType.Desert_Sandstone_Swirl:
                return Desert_Sandstone_Swirl;
            case SkinType.Desert_Quicksand:
                return Desert_Quicksand;

            case SkinType.Winter_Snow:
                return Winter_Snow;
            case SkinType.Winter_Ice:
                return Winter_Ice;
            case SkinType.Winter_ColdWood:
                return Winter_ColdWood;
            case SkinType.Winter_FrozenGrass:
                return Winter_FrozenGrass;
            case SkinType.Winter_CrackedIce:
                return Winter_CrackedIce;
            case SkinType.Winter_Crocked:
                return Winter_Crocked;

            case SkinType.Swamp_SwampWater:
                return Swamp_SwampWater;
            case SkinType.Swamp_Mud:
                return Swamp_Mud;
            case SkinType.Swamp_SwampGrass:
                return Swamp_SwampGrass;
            case SkinType.Swamp_JungleWood:
                return Swamp_JungleWood;
            case SkinType.Swamp_SwampWood:
                return Swamp_SwampWood;
            case SkinType.Swamp_TempleBlock:
                return Swamp_TempleBlock;

            case SkinType.Industrial_Metal:
                return Industrial_Metal;
            case SkinType.Industrial_Brass:
                return Industrial_Brass;
            case SkinType.Industrial_Gold:
                return Industrial_Gold;
            case SkinType.Industrial_Casing_Metal:
                return Industrial_Casing_Metal;
            case SkinType.Industria_Casingl_Brass:
                return Industria_Casingl_Brass;
            case SkinType.Industrial_Casing_Gold:
                return Industrial_Casing_Gold;

            default:
                return null;
        }
    }
}

public enum SkinType
{
    None,

    Water_Grass,
    Water_Water,
    Water_Wood,
    Water_4,
    Water_5,
    Water_6,

    Cave_Stone,
    Cave_Stone_Brick,
    Cave_Lava,
    Cave_Rock,
    Cave_Brick_Brown,
    Cave_Brick_Black,

    Desert_Sand,
    Desert_Clay,
    Desert_Clay_Tiles,
    Desert_Sandstone,
    Desert_Sandstone_Swirl,
    Desert_Quicksand,

    Winter_Snow,
    Winter_Ice,
    Winter_ColdWood,
    Winter_FrozenGrass,
    Winter_CrackedIce,
    Winter_Crocked,

    Swamp_SwampWater,
    Swamp_Mud,
    Swamp_SwampGrass,
    Swamp_JungleWood,
    Swamp_SwampWood,
    Swamp_TempleBlock,

    Industrial_Metal,
    Industrial_Brass,
    Industrial_Gold,
    Industrial_Casing_Metal,
    Industria_Casingl_Brass,
    Industrial_Casing_Gold,
}