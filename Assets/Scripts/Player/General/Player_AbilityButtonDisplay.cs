using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_AbilityButtonDisplay : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] GameObject buttonDisplay_Parent;
    [SerializeField] GameObject buttonDisplay_FieldParent;

    [Header("Prefab")]
    [SerializeField] GameObject buttonDisplay_Prefab;

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


    //--------------------


    private void Start()
    {
        Cameras.rotateCamera += RotateAbilityDisplay;
    }


    //--------------------


    void RotateAbilityDisplay()
    {
        switch (Cameras.Instance.cameraState)
        {
            case CameraState.Forward:
                buttonDisplay_Parent.transform.SetLocalPositionAndRotation(new Vector3(1.9f, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            case CameraState.Backward:
                buttonDisplay_Parent.transform.SetLocalPositionAndRotation(new Vector3(-1.9f, 0, 0), Quaternion.Euler(0, 180, 0));
                break;
            case CameraState.Left:
                buttonDisplay_Parent.transform.SetLocalPositionAndRotation(new Vector3(-0.1f, 0, -2), Quaternion.Euler(0, 90, 0));
                break;
            case CameraState.Right:
                buttonDisplay_Parent.transform.SetLocalPositionAndRotation(new Vector3(-0.1f, 0, 2), Quaternion.Euler(0, -90, 0));
                break;

            default:
                break;
        }
    }
}
