using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EffectManager : Singleton<EffectManager>
{
    float Walk_HitGroundEffect_Delay = 0f;
    float pickupSmall_HitGroundEffect_Delay = 0.15f;
    float pickupBig_HitGroundEffect_Delay = 0.18f;

    float dash_HitGroundEffect_Delay = 0.26f;
    float jump_HitGroundEffect_Delay = 0.3f;
    float ascend_HitGroundEffect_Delay = 0.25f;
    float descend_HitGroundEffect_Delay = 0.25f;
    float grapplingHook_HitGroundEffect_Delay = 0f;

    bool isDashing;
    bool isJumping;
    bool isAscending;
    bool isDescending;
    bool isGrapplingHooking;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_StepTaken += WalkHitGorund_Effect;

        Movement.Action_isDashing += Set_isDashing;
        Movement.Action_isJumping += Set_isJumping;
        Movement.Action_isAscending += Set_isAscending;
        Movement.Action_isDescending += Set_isDescending;
        Movement.Action_isGrapplingHooking += Set_isGrapplingHooking;
    }
    private void OnDisable()
    {
        Movement.Action_StepTaken -= WalkHitGorund_Effect;

        Movement.Action_isDashing -= Set_isDashing;
        Movement.Action_isJumping -= Set_isJumping;
        Movement.Action_isAscending -= Set_isAscending;
        Movement.Action_isDescending -= Set_isDescending;
        Movement.Action_isGrapplingHooking -= Set_isGrapplingHooking;
    }


    //--------------------


    void Set_isDashing()
    {
        isDashing = true;
    }
    void Set_isJumping()
    {
        isJumping = true;
    }
    void Set_isAscending()
    {
        isAscending = true;
    }
    void Set_isDescending()
    {
        isDescending = true;
    }
    void Set_isGrapplingHooking()
    {
        isGrapplingHooking = true;
    }


    //--------------------


    public void WalkHitGorund_Effect()
    {
        StartCoroutine(Effect_HitGround(Walk_HitGroundEffect_Delay));
    }
    public void PickupSmallHitGorund_Effect()
    {
        StartCoroutine(Effect_HitGround(Player_Animations.Instance.effectChargeTime_Pickup_Small - pickupSmall_HitGroundEffect_Delay));
    }
    public void PickupBigHitGorund_Effect()
    {
        StartCoroutine(Effect_HitGround(Player_Animations.Instance.effectChargeTime_Pickup_Big - pickupBig_HitGroundEffect_Delay));
    }

    public void Dash_HitGorund_Effect()
    {
        StartCoroutine(Effect_HitGround(dash_HitGroundEffect_Delay));
    }
    public void Jump_HitGorund_Effect()
    {
        StartCoroutine(Effect_HitGround(jump_HitGroundEffect_Delay));
    }
    public void Ascend_HitGorund_Effect()
    {
        StartCoroutine(Effect_HitGround(ascend_HitGroundEffect_Delay));
    }
    public void Descend_HitGorund_Effect()
    {
        StartCoroutine(Effect_HitGround(descend_HitGroundEffect_Delay));
    }
    public void GrapplingHook_HitGorund_Effect()
    {
        StartCoroutine(Effect_HitGround(grapplingHook_HitGroundEffect_Delay));
    }

    IEnumerator Effect_HitGround(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        BlockInfo blockInfo = null;

        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>())
            blockInfo = Movement.Instance.blockStandingOn.GetComponent<BlockInfo>();
        else
        {
            yield break;
        }


        if (isDashing)
        {
            isDashing = false;
            Dash_HitGorund_Effect();
        }
        else if (isJumping)
        {
            isJumping = false;
            Jump_HitGorund_Effect();
        }
        else if (isAscending)
        {
            isAscending = false;
            Ascend_HitGorund_Effect();
        }
        else if (isDescending)
        {
            isDescending = false;
            Descend_HitGorund_Effect();
        }
        else if (isGrapplingHooking)
        {
            isGrapplingHooking = false;
            GrapplingHook_HitGorund_Effect();
        }

        else if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
        }
        else if (blockInfo.blockElement == BlockElement.Water)
        {
        }
        else if (blockInfo.blockElement == BlockElement.Mud)
        {
        }
        else if (blockInfo.blockElement == BlockElement.SwampWater)
        {
        }
        else
        {
            HitParticleScript.Instance.particle.Play();
        }

        print("1. Effect_HitGround");
    }
}
