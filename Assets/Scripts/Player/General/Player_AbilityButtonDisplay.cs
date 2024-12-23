using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_AbilityButtonDisplay : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] GameObject buttonDisplay_Canvas;
    [SerializeField] GameObject buttonDisplay_FieldParent;

    [Header("Prefab")]
    [SerializeField] GameObject buttonDisplay_Prefab;

    [Header("Lists")]
    [SerializeField] List<GameObject> buttonDisplayList;
    Block_Moveable[] moveableObjectsList;
    [SerializeField] List<GameObject> buttonDisplay_InSceneList;

    [Header("Sprites")]
    [SerializeField] Sprite button_Space;
    [SerializeField] Sprite button_C;
    [SerializeField] Sprite button_F;
    [SerializeField] Sprite button_ArrowLeftSide;
    [SerializeField] Sprite button_ArrowRightSide;
    [SerializeField] Sprite button_ArrowUp;
    [SerializeField] Sprite button_ArrowDown;

    [Header("Abilities")]
    [SerializeField] Sprite abilitySprite_Ascend;
    [SerializeField] Sprite abilitySprite_ClimbingGear;
    [SerializeField] Sprite abilitySprite_ControlStick;
    [SerializeField] Sprite abilitySprite_Dash;
    [SerializeField] Sprite abilitySprite_Descend;
    [SerializeField] Sprite abilitySprite_FenceSneak;
    [SerializeField] Sprite abilitySprite_Flippers;
    [SerializeField] Sprite abilitySprite_GrapplingHook;
    [SerializeField] Sprite abilitySprite_Hammer;
    [SerializeField] Sprite abilitySprite_HikersGear;
    [SerializeField] Sprite abilitySprite_IceSpikes;
    [SerializeField] Sprite abilitySprite_LavaSuit;
    [SerializeField] Sprite abilitySprite_lavaSwiftSwim;
    [SerializeField] Sprite abilitySprite_SwiftSwim;
    [SerializeField] Sprite abilitySprite_SwimSuit;

    [SerializeField] Sprite abilitySprite_Move;


    //--------------------


    private void Start()
    {
        Cameras.rotateCamera += RotateAbilityDisplay;

        FindAllMoveableBlocks();
    }
    private void Update()
    {
        UpdateButtonDisplay();
    }


    //--------------------


    void UpdateButtonDisplay()
    {
        Ability("Swift Swim Up", Player_SwiftSwim.Instance.canSwiftSwim_Up, button_ArrowUp, abilitySprite_SwiftSwim);
        Ability("Swift Swim Down", Player_SwiftSwim.Instance.canSwiftSwim_Down, button_ArrowUp, abilitySprite_SwiftSwim);
        Ability("Ascend", Player_Ascend.Instance.playerCanAscend, button_ArrowUp, abilitySprite_Ascend);
        Ability("Descend", Player_Descend.Instance.playerCanDescend, button_ArrowDown, abilitySprite_Descend);
        Ability("Dash", Player_Dash.Instance.playerCanDash, button_Space, abilitySprite_Dash);
        Ability("Hammer", Player_Hammer.Instance.playerCanHammer, button_C, abilitySprite_Hammer);

        #region Moveable Block
        bool canMoveBlock = false;
        for (int i = 0; i < buttonDisplay_InSceneList.Count; i++)
        {
            if (buttonDisplay_InSceneList[i].GetComponent<Block_Moveable>().canMove)
            {
                canMoveBlock = true;
                break;
            }
        }

        if (canMoveBlock)
        {
            if (!CheckIfAbilityIsAlreadyIncluded("Push Block"))
            {
                AddPrefab(button_C, abilitySprite_Move, "Push Block");
            }
        }
        else
        {
            RemovePrefab("Push Block");
        }
        #endregion
    }

    void Ability(string _abilityName, bool canPerformAbility, Sprite _buttonSprite, Sprite _abilitySprite)
    {
        if (canPerformAbility)
            AddPrefab(_buttonSprite, _abilitySprite, _abilityName);
        else
            RemovePrefab(_abilityName);
    }
    void AddPrefab(Sprite _buttonSprite, Sprite _abilitySprite, string _abilityName)
    {
        for (int i = 0; i < buttonDisplayList.Count; i++)
        {
            if (buttonDisplayList[i].GetComponent<AbilityDisplay>().abilityName.text == _abilityName)
            {
                return;
            }
        }

        buttonDisplayList.Add(Instantiate(buttonDisplay_Prefab));
        buttonDisplayList[buttonDisplayList.Count - 1].GetComponent<AbilityDisplay>().SetupAbilityDisplay(_buttonSprite, _abilitySprite, _abilityName);
        buttonDisplayList[buttonDisplayList.Count - 1].transform.parent = buttonDisplay_FieldParent.transform;

        buttonDisplayList[buttonDisplayList.Count - 1].transform.SetLocalPositionAndRotation(new Vector3(buttonDisplay_FieldParent.transform.position.x, buttonDisplay_FieldParent.transform.position.y, 0), Quaternion.identity);
    }
    void RemovePrefab(string _name)
    {
        for (int i = 0; i < buttonDisplayList.Count; i++)
        {
            if (buttonDisplayList[i].GetComponent<AbilityDisplay>().abilityName.text == _name)
            {
                buttonDisplayList[i].GetComponent<AbilityDisplay>().DestroyPrefab();
                buttonDisplayList.RemoveAt(i);
                break;
            }
        }
    }
    bool CheckIfAbilityIsAlreadyIncluded(string _name)
    {
        for (int i = 0; i < buttonDisplayList.Count; i++)
        {
            if (buttonDisplayList[i].GetComponent<AbilityDisplay>().abilityName.text == _name)
            {
                return true;
            }
        }

        return false;
    }


    //--------------------


    void RotateAbilityDisplay()
    {
        switch (Cameras.Instance.cameraState)
        {
            case CameraState.Forward:
                buttonDisplay_Canvas.transform.SetLocalPositionAndRotation(new Vector3(1.9f, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            case CameraState.Backward:
                buttonDisplay_Canvas.transform.SetLocalPositionAndRotation(new Vector3(-1.9f, 0, 0), Quaternion.Euler(0, 180, 0));
                break;
            case CameraState.Left:
                buttonDisplay_Canvas.transform.SetLocalPositionAndRotation(new Vector3(-0.1f, 0, -2), Quaternion.Euler(0, 90, 0));
                break;
            case CameraState.Right:
                buttonDisplay_Canvas.transform.SetLocalPositionAndRotation(new Vector3(-0.1f, 0, 2), Quaternion.Euler(0, -90, 0));
                break;

            default:
                break;
        }
    }
    void FindAllMoveableBlocks()
    {
        moveableObjectsList = FindObjectsOfType<Block_Moveable>();

        foreach (Block_Moveable moveable in moveableObjectsList)
        {
            buttonDisplay_InSceneList.Add(moveable.gameObject);
        }
    }
}
