using System.Collections;
using UnityEngine;

public class Player_Animations : Singleton<Player_Animations>
{
    public Animator playerAnimator;

    public float abilityChargeTime_Dash = 0.5f;
    public float abilityChargeTime_Jump = 0.48f;
    public float abilityChargeTime_Ascend = 0.48f;
    public float abilityChargeTime_Descend = 0.51f;

    public float abilityChargeTime_CeilingGrab = 0.5f;
    public float abilityChargeTime_GrapplingHook = 0.5f;

    public float effectChargeTime_Pickup_Small = 0.75f;
    public float effectChargeTime_Pickup_Big = 0.75f;
    public float effectChargeTime_Pickup_Teleport = 0.23f;

    [SerializeField] bool isWalkGliding;
    [SerializeField] bool walkGlidingCheck;


    //--------------------


    private void Start()
    {
        playerAnimator = GetComponent<Animator>();

        StartCoroutine(RandomBlink());
        StartCoroutine(RandomIdle());
    }

    private void Update()
    {
        //Swim Animation
        if (/*Player_CeilingGrab.Instance.isCeilingGrabbing || */(Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() &&
            (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Water || Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.SwampWater || Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Mud)))
        {
            Set_SwimAnimation(true);
        }
        else
        {
            Set_SwimAnimation(false);
        }

        //Extra safety to standing still glidingBug
        if (Movement.Instance.movementStates == MovementStates.Still ||
            (!Player_KeyInputs.Instance.forward_isHold &&
            !Player_KeyInputs.Instance.back_isHold &&
            !Player_KeyInputs.Instance.left_isHold &&
            !Player_KeyInputs.Instance.right_isHold))
        {
            Set_WalkGlideAnimation(false);
        }

        //Walk Glide Animation
        else if (/*Movement.Instance.movementStates == MovementStates.Moving &&*/
            (Player_KeyInputs.Instance.forward_isHold && Movement.Instance.moveToBlock_Forward.canMoveTo)
            || (Player_KeyInputs.Instance.back_isHold && Movement.Instance.moveToBlock_Back.canMoveTo)
            || (Player_KeyInputs.Instance.left_isHold && Movement.Instance.moveToBlock_Left.canMoveTo)
            || (Player_KeyInputs.Instance.right_isHold && Movement.Instance.moveToBlock_Right.canMoveTo))
        {
            //print("1. isWalkGliding");
            //Set_WalkGlideAnimation(true);

            if (!walkGlidingCheck)
            {
                //print("1. isWalkGliding");
                Set_WalkGlideAnimation(true);
            }
            //else
            //{
            //    print("2. isWalkGliding");
            //    Set_WalkGlideAnimation(false);
            //}
        }
        else
        {
            //print("3. isWalkGliding");
            Set_WalkGlideAnimation(false);
        }

        //If gliding on ice, reset animations
        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Ice)
        {
            Player_KeyInputs.Instance.forward_isHold = false;
            Player_KeyInputs.Instance.back_isHold = false;
            Player_KeyInputs.Instance.left_isHold = false;
            Player_KeyInputs.Instance.right_isHold = false;
        }
    }


    //--------------------


    private void OnEnable()
    {
        Player_KeyInputs.Action_WalkButton_isPressed += UpdateAnimator;
        DataManager.Action_dataHasLoaded += UpdateAnimator;

        Movement.Action_RespawnPlayer += ResetAnimations;
        //Movement.Action_RespawnPlayerEarly += Trigger_RespawnAnimation;

        Player_KeyInputs.Action_RespawnHold += Start_RespawnAnimation;
        Player_KeyInputs.Action_RespawnCanceled += End_RespawnAnimation;
        SFX_Respawn.Action_RespawnPlayerAnimation += Trigger_RespawnAnimation_ButtonHold;

        Interactable_Pickup.Action_EssencePickupGot += PickUpAnimation_Small;
        Interactable_Pickup.Action_SkinPickupGot += PickUpAnimation_Small;
        Interactable_Pickup.Action_StepsUpPickupGot += PickUpAnimation_Big;
        Interactable_Pickup.Action_AbilityPickupGot += PickUpAnimation_Big;
    }
    private void OnDisable()
    {
        Player_KeyInputs.Action_WalkButton_isPressed -= UpdateAnimator;
        DataManager.Action_dataHasLoaded -= UpdateAnimator;

        Movement.Action_RespawnPlayer -= ResetAnimations;
        //Movement.Action_RespawnPlayerEarly -= Trigger_RespawnAnimation;

        Player_KeyInputs.Action_RespawnHold -= Start_RespawnAnimation;
        Player_KeyInputs.Action_RespawnCanceled -= End_RespawnAnimation;
        SFX_Respawn.Action_RespawnPlayerAnimation -= Trigger_RespawnAnimation_ButtonHold;

        Interactable_Pickup.Action_EssencePickupGot -= PickUpAnimation_Small;
        Interactable_Pickup.Action_SkinPickupGot -= PickUpAnimation_Small;
        Interactable_Pickup.Action_StepsUpPickupGot -= PickUpAnimation_Big;
        Interactable_Pickup.Action_AbilityPickupGot -= PickUpAnimation_Big;
    }


    //--------------------


    public void UpdateAnimator()
    {
        playerAnimator = Player_Body.Instance.selectedSkinBlock.GetComponent<Animator>();
    }

    #region ResetAnimations

    public void ResetAnimations()
    {
        Set_SwimAnimation(false);
        Set_WalkGlideAnimation(false);

        ForceStopAllAnimations("", AnimationManager.Instance.walk);

        ForceStopAllAnimations(AnimationManager.Instance.ability_Ascend, "");
        ForceStopAllAnimations(AnimationManager.Instance.ability_Descend, "");
        ForceStopAllAnimations(AnimationManager.Instance.ability_Dash, "");
        ForceStopAllAnimations(AnimationManager.Instance.ability_Jump, "");
        ForceStopAllAnimations(AnimationManager.Instance.ability_CeilingGrab, "");
        ForceStopAllAnimations(AnimationManager.Instance.ability_GrapplingHook, "");
    }
    public void ForceStopAllAnimations(string triggerName, string stateName)
    {
        if (!playerAnimator) return;

        //playerAnimator.Play("Idle", 0, 0f);
        playerAnimator.SetTrigger("Confident");
    }
    bool Animator_HasParameter(Animator anim, string paramName)
    {
        foreach (var p in anim.parameters)
            if (p.name == paramName)
                return true;

        return false;
    }
    bool Animator_HasState(Animator anim, string stateName)
    {
        int hash = Animator.StringToHash(stateName);

        for (int i = 0; i < anim.layerCount; i++)
        {
            if (anim.HasState(i, hash))
                return true;
        }

        return false;
    }

    public void StopAllAnimationsImmediately()
    {
        // 1. Reset all triggers so nothing queues
        foreach (var p in playerAnimator.parameters)
            if (p.type == AnimatorControllerParameterType.Trigger)
                playerAnimator.ResetTrigger(p.name);

        // 2. Force jump to Idle or a neutral state
        playerAnimator.Play("Idle", 0, 0f);
    }

    #endregion


    //--------------------


    public void Trigger_WalkingAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        if (!isWalkGliding && !Movement.Instance.isDashing && !Movement.Instance.isJumping && !Movement.Instance.isAscending && !Movement.Instance.isDescending)
        {
            playerAnimator.speed = 1.0f;
            playerAnimator.SetTrigger(AnimationManager.Instance.walk);
        }
    }
    public void Trigger_StairSlopeWalkingAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        playerAnimator.speed = 1.0f;
        playerAnimator.SetTrigger(AnimationManager.Instance.walk);
    }
    public void Trigger_SlopeDownAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        playerAnimator.speed = 1.0f;
        //anim.SetTrigger(AnimationManager.Instance.walk);
    }


    //--------------------


    public void Set_SwimAnimation(bool state)
    {
        if (Movement.Instance.isMoving) { return; }

        //anim.speed = 1.0f;
        playerAnimator.SetBool("InWater", state);
    }
    public void Set_WalkGlideAnimation(bool state)
    {
        if (state /*&& Movement.Instance.movementStates == MovementStates.Still*/)
        {
            //print("4. isWalkGliding");
            walkGlidingCheck = true;
            //anim.speed = 1.0f;
            playerAnimator.SetBool("Sliding", true);
            isWalkGliding = true;
        }
        else
        {
            //print("5. isWalkGliding");
            walkGlidingCheck = false;
            playerAnimator.SetBool("Sliding", false);
            isWalkGliding = false;
        }
    }

    public void Start_RespawnAnimation()
    {
        playerAnimator.speed = 1.6f;
        playerAnimator.SetBool("Respawn", true);
    }
    public void End_RespawnAnimation()
    {
        playerAnimator.speed = 1.0f;
        playerAnimator.SetBool("Respawn", false);
    }


    //--------------------


    public void Trigger_AscendAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        playerAnimator.speed = 1.0f;
        playerAnimator.SetTrigger(AnimationManager.Instance.ability_Ascend);
        EffectManager.Instance.PerformAscendEffect();
    }
    public void Trigger_DescendAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        playerAnimator.speed = 1.0f;
        playerAnimator.SetTrigger(AnimationManager.Instance.ability_Descend);
        EffectManager.Instance.PerformDescendEffect();
    }
    public void Trigger_DashAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        playerAnimator.speed = 1.0f;
        //anim.SetLayerWeight(1, 1);
        playerAnimator.SetTrigger(AnimationManager.Instance.ability_Dash);
        EffectManager.Instance.PerformDashEffect();
    }
    public void Trigger_JumpAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        playerAnimator.speed = 1.0f;
        playerAnimator.SetTrigger(AnimationManager.Instance.ability_Jump);
    }
    public void Trigger_CeilingGrabAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        playerAnimator.speed = 1.0f;
        playerAnimator.SetTrigger(AnimationManager.Instance.ability_CeilingGrab);
    }
    public void Trigger_GrapplingHookAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        playerAnimator.speed = 1.0f;
        playerAnimator.SetTrigger(AnimationManager.Instance.ability_GrapplingHook);
    }
    public void Trigger_GrapplingHookDraggingAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        playerAnimator.speed = 1.0f;
        //anim.SetTrigger(AnimationManager.Instance.);
    }


    //--------------------


    public void PickUpAnimation_Small()
    {
        PlayerManager.Instance.PauseGame();

        StartCoroutine(RunPickupAnimation_Small());
    }
    IEnumerator RunPickupAnimation_Small()
    {
        //print("1. Pickup - Small");

        yield return new WaitForSeconds(0.1f);
        Trigger_PickupSmallAnimation();
        EffectManager.Instance.PickupSmallHitGorund_Effect();
        yield return new WaitForSeconds(effectChargeTime_Pickup_Small);

        PlayerManager.Instance.UnpauseGame();
    }
    public void PickUpAnimation_Big()
    {
        PlayerManager.Instance.PauseGame();

        StartCoroutine(RunPickupAnimation_Big());
    }
    IEnumerator RunPickupAnimation_Big()
    {
        //print("2. Pickup - Big");

        yield return new WaitForSeconds(0.1f);
        Trigger_PickupBigAnimation();
        EffectManager.Instance.PickupSmallHitGorund_Effect();
        yield return new WaitForSeconds(effectChargeTime_Pickup_Small);

        PlayerManager.Instance.UnpauseGame();
    }

    public void Trigger_PickupSmallAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        playerAnimator.speed = 1.0f;
        playerAnimator.SetTrigger(AnimationManager.Instance.effect_PickupSmall);
    }
    public void Trigger_PickupBigAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        playerAnimator.speed = 1.0f;
        playerAnimator.SetTrigger(AnimationManager.Instance.effect_PickupBig);
    }

    public void Trigger_TeleportAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        playerAnimator.speed = 1.0f;
        playerAnimator.SetTrigger(AnimationManager.Instance.effect_Teleport);

        EffectManager.Instance.PerformTeleportEffect();
    }
    public void Trigger_RespawnAnimation()
    {
        if (Movement.Instance.isMoving || Player_KeyInputs.Instance.respawn_isPressed) { return; }

        print("3. Respawn - Animation");

        playerAnimator.speed = 1.0f;
        playerAnimator.SetTrigger(AnimationManager.Instance.effect_Teleport);
    }
    public void Trigger_RespawnAnimation_ButtonHold()
    {
        if (Movement.Instance.isMoving) { return; }

        print("4. Respawn - Animation");

        playerAnimator.speed = 1.0f;
        playerAnimator.SetTrigger(AnimationManager.Instance.effect_Teleport);
    }


    //--------------------


    IEnumerator RandomBlink()
    {
        float time = Random.Range(0, 10);

        yield return new WaitForSeconds(time);

        playerAnimator.SetTrigger(AnimationManager.Instance.blink);

        StartCoroutine(RandomBlink());
    }
    IEnumerator RandomIdle()
    {
        float time = Random.Range(15, 30);

        yield return new WaitForSeconds(time);

        if (Random.Range(0, 1) > 0.5f)
            playerAnimator.SetTrigger("Confused");
        else
            playerAnimator.SetTrigger("Confident");

        Invoke(nameof(StartSecondaryIdle), 0);
    }

    void StartSecondaryIdle()
    {
        StartCoroutine(RandomIdle());
    }
}
