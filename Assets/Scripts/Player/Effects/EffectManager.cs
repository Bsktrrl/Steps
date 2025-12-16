using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] GameObject hitEffect_Walk_Object;
    [SerializeField] GameObject hitEffect_Dash_Object;
    [SerializeField] GameObject hitEffect_Respawn_Object;
    [SerializeField] GameObject hitEffect_Fire_Object;
    [SerializeField] GameObject hitEffect_Pickup_Object;
    [SerializeField] GameObject hitEffect_Teleport_Object;

    [SerializeField] GameObject hitEffect_Splash_Water_Object;
    [SerializeField] GameObject hitEffect_Splash_Long_Water_Object;
    [SerializeField] GameObject hitEffect_Splash_SwampWater_Object;
    [SerializeField] GameObject hitEffect_Splash_Long_SwampWater_Object;
    [SerializeField] GameObject hitEffect_Splash_Mud_Object;
    [SerializeField] GameObject hitEffect_Splash_Long_Mud_Object;
    [SerializeField] GameObject hitEffect_Splash_Quicksand_Object;
    [SerializeField] GameObject hitEffect_Splash_Long_Quicksand_Object;

    [Header("Walk HitGround")]
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

    [Header("Other effects")]
    float DashEffect_Delay = 0.4f;
    float AscendEffect_Delay = 0.4f;
    float DescendEffect_Delay = 0.5f;

    float PickupEffect_Delay = 0.2f;
    float TeleportEffect_Delay = 0f;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_StepTaken += WalkHitGorund_Effect;

        Movement.Action_isDashing += Set_isDashing;
        Movement.Action_isJumping += Set_isJumping;
        Movement.Action_isAscending += Set_isAscending;
        Movement.Action_isDescending += Set_isDescending;
        Movement.Action_isGrapplingHooking += Set_isGrapplingHooking;

        Interactable_Pickup.Action_EssencePickupGot += PerformPickupEffect;
        Interactable_Pickup.Action_StepsUpPickupGot += PerformPickupEffect;
        Interactable_Pickup.Action_SkinPickupGot += PerformPickupEffect;
        Interactable_Pickup.Action_AbilityPickupGot += PerformPickupEffect;

        Player_KeyInputs.Action_RespawnHold += StartRespawn;
        Player_KeyInputs.Action_RespawnCanceled += EndRespawn;
        Movement.Action_RespawnPlayerEarly += EndRespawn;

        Movement.Action_isSwitchingBlocks += SplashEffect;
    }
    private void OnDisable()
    {
        Movement.Action_StepTaken -= WalkHitGorund_Effect;

        Movement.Action_isDashing -= Set_isDashing;
        Movement.Action_isJumping -= Set_isJumping;
        Movement.Action_isAscending -= Set_isAscending;
        Movement.Action_isDescending -= Set_isDescending;
        Movement.Action_isGrapplingHooking -= Set_isGrapplingHooking;

        Interactable_Pickup.Action_EssencePickupGot -= PerformPickupEffect;
        Interactable_Pickup.Action_StepsUpPickupGot -= PerformPickupEffect;
        Interactable_Pickup.Action_SkinPickupGot -= PerformPickupEffect;
        Interactable_Pickup.Action_AbilityPickupGot -= PerformPickupEffect;

        Player_KeyInputs.Action_RespawnHold -= StartRespawn;
        Player_KeyInputs.Action_RespawnCanceled -= EndRespawn;
        Movement.Action_RespawnPlayerEarly -= EndRespawn;
    }


    //--------------------


    #region HitEffect

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
            hitEffect_Walk_Object.GetComponent<HitParticleScript>().particle.Play();
        }
    }

    #endregion

    #region Respawn

    public void StartRespawn()
    {
        hitEffect_Respawn_Object.GetComponent<RespawnEffectScript>().particle.Play();
    }
    public void EndRespawn()
    {
        hitEffect_Respawn_Object.GetComponent<RespawnEffectScript>().particle.Stop();
    }

    #endregion

    #region Splash

    public void SplashEffect()
    {
        //Water
        if (Movement.Instance.blockStandingOn_Previous && Movement.Instance.blockStandingOn_Previous.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
        {
            if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
            {
                hitEffect_Splash_Long_Water_Object.GetComponent<HitParticleScript>().particle.Play();
            }
        }
        else
        {
            if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
            {
                hitEffect_Splash_Water_Object.GetComponent<HitParticleScript>().particle.Play();
            }
        }

        //SwampWater
        if (Movement.Instance.blockStandingOn_Previous && Movement.Instance.blockStandingOn_Previous.GetComponent<BlockInfo>().blockElement == BlockElement.SwampWater)
        {
            if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.SwampWater)
            {
                hitEffect_Splash_Long_SwampWater_Object.GetComponent<HitParticleScript>().particle.Play();
            }
        }
        else
        {
            if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.SwampWater)
            {
                hitEffect_Splash_SwampWater_Object.GetComponent<HitParticleScript>().particle.Play();
            }
        }

        //Mud
        if (Movement.Instance.blockStandingOn_Previous && Movement.Instance.blockStandingOn_Previous.GetComponent<BlockInfo>().blockElement == BlockElement.Mud)
        {
            if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Mud)
            {
                hitEffect_Splash_Long_Mud_Object.GetComponent<HitParticleScript>().particle.Play();
            }
        }
        else
        {
            if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Mud)
            {
                hitEffect_Splash_Mud_Object.GetComponent<HitParticleScript>().particle.Play();
            }
        }

        //Quicksand
        if (Movement.Instance.blockStandingOn_Previous && Movement.Instance.blockStandingOn_Previous.GetComponent<BlockInfo>().blockElement == BlockElement.Quicksand)
        {
            if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Quicksand)
            {
                hitEffect_Splash_Long_Quicksand_Object.GetComponent<HitParticleScript>().particle.Play();
            }
        }
        else
        {
            if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Quicksand)
            {
                hitEffect_Splash_Quicksand_Object.GetComponent<HitParticleScript>().particle.Play();
            }
        }
    }

    #endregion

    #region Other effect

    public void PerformDashEffect()
    {
        StartCoroutine(Effect_Delay(hitEffect_Dash_Object, DashEffect_Delay));
    }
    public void PerformAscendEffect()
    {
        StartCoroutine(Effect_Delay(hitEffect_Dash_Object, AscendEffect_Delay));
    }
    public void PerformDescendEffect()
    {
        StartCoroutine(Effect_Delay(hitEffect_Dash_Object, DescendEffect_Delay));
    }

    public void PerformPickupEffect()
    {
        StartCoroutine(Effect_Delay(hitEffect_Pickup_Object, PickupEffect_Delay));
    }
    public void PerformTeleportEffect()
    {
        StartCoroutine(Effect_Delay(hitEffect_Teleport_Object, TeleportEffect_Delay));
        print("1. Teleport");
    }

    IEnumerator Effect_Delay(GameObject effectObject, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (effectObject && effectObject.GetComponent<HitParticleScript>())
        {
            effectObject.GetComponent<HitParticleScript>().particle.Play();
        }
    }

    #endregion
}
