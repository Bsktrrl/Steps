using System.Collections;
using UnityEngine;

public class Player_Animations : Singleton<Player_Animations>
{
    public Animator anim;

    public float abilityChargeTime_Dash = 0.5f;
    public float abilityChargeTime_Jump = 0.38f;
    public float abilityChargeTime_Ascend = 0.48f;
    public float abilityChargeTime_Descend = 0.51f;

    [SerializeField] bool isWalkGliding;


    //--------------------


    private void Start()
    {
        StartCoroutine(RandomBlink());
        StartCoroutine(RandomIdle());
    }

    private void Update()
    {
        //Swim Animation
        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() &&
            (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Water || Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.SwampWater || Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Mud))
        {
            Set_SwimAnimation(true);
        }
        else
        {
            Set_SwimAnimation(false);
        }

        //Walk Glide Animation
        if ((Player_KeyInputs.Instance.forward_isHold && Movement.Instance.moveToBlock_Forward.canMoveTo)
            || (Player_KeyInputs.Instance.back_isHold && Movement.Instance.moveToBlock_Back.canMoveTo)
            || (Player_KeyInputs.Instance.left_isHold && Movement.Instance.moveToBlock_Left.canMoveTo)
            || (Player_KeyInputs.Instance.right_isHold && Movement.Instance.moveToBlock_Right.canMoveTo))
        {
            Set_WalkGlideAnimation(true);
        }
        else
        {
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
    }
    private void OnDisable()
    {
        Player_KeyInputs.Action_WalkButton_isPressed -= UpdateAnimator;
        DataManager.Action_dataHasLoaded -= UpdateAnimator;

        Movement.Action_RespawnPlayer -= ResetAnimations;
    }


    //--------------------


    public void UpdateAnimator()
    {
        anim = Player_Body.Instance.selectedSkinBlock.GetComponent<Animator>();
    }

    #region ResetAnimations

    public void ResetAnimations()
    {
        Set_SwimAnimation(false);
        Set_WalkGlideAnimation(false);

        ForceStopAllAnimations("", AnimationManager.Instance.walk);
        ForceStopAllAnimations("", AnimationManager.Instance.swim);

        ForceStopAllAnimations(AnimationManager.Instance.ability_AscendDescend, "");
        ForceStopAllAnimations(AnimationManager.Instance.ability_Dash, "");
        ForceStopAllAnimations(AnimationManager.Instance.ability_Jump, "");
        ForceStopAllAnimations(AnimationManager.Instance.ability_CeilingGrab, "");
        ForceStopAllAnimations(AnimationManager.Instance.ability_GrapplingHook, "");
    }
    public void ForceStopAllAnimations(string triggerName, string stateName)
    {
        if (!anim) return;

        // Only reset if the parameter exists
        if (Animator_HasParameter(anim, triggerName))
            anim.ResetTrigger(triggerName);

        // Only crossfade if the state exists
        if (Animator_HasState(anim, stateName))
        {
            anim.CrossFade(stateName, 0f);
        }
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
    
    #endregion


    //--------------------


    public void Perform_WalkingAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        if (!isWalkGliding && !Movement.Instance.isDashing && !Movement.Instance.isJumping && !Movement.Instance.isAscending && !Movement.Instance.isDecending)
        {
            anim.SetTrigger(AnimationManager.Instance.walk);
        }
    }
    public void Perform_StairSlopeWalkingAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        anim.SetTrigger(AnimationManager.Instance.walk);
    }
    public void Perform_SlopeDownAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        //anim.SetTrigger(AnimationManager.Instance.walk);
    }


    //--------------------


    public void Set_SwimAnimation(bool state)
    {
        if (Movement.Instance.isMoving) { return; }

        anim.SetBool("InWater", state);
    }
    public void Set_WalkGlideAnimation(bool state)
    {
        anim.SetBool("Sliding", state);
        isWalkGliding = state;
    }


    //--------------------


    public void Perform_AscendDescendAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        anim.SetTrigger(AnimationManager.Instance.ability_AscendDescend);
    }
    public void Perform_DashAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        anim.SetLayerWeight(1, 1);
        anim.SetTrigger(AnimationManager.Instance.ability_Dash);
    }
    public void Perform_JumpAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        anim.SetTrigger(AnimationManager.Instance.ability_Jump);
    }
    public void Perform_CeilingGrabAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        anim.SetTrigger(AnimationManager.Instance.ability_CeilingGrab);
    }
    public void Perform_GrapplingHookAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        anim.SetTrigger(AnimationManager.Instance.ability_GrapplingHook);
    }


    //--------------------


    IEnumerator RandomBlink()
    {
        float time = Random.Range(0, 10);

        yield return new WaitForSeconds(time);

        anim.SetTrigger(AnimationManager.Instance.blink);

        StartCoroutine(RandomBlink());
    }
    IEnumerator RandomIdle()
    {
        float time = Random.Range(15, 30);

        yield return new WaitForSeconds(time);

        if (Random.Range(0, 1) > 0.5f)
            anim.SetTrigger("Confused");
        else
            anim.SetTrigger("Confident");

        Invoke(nameof(StartSecondaryIdle), 0);
    }

    void StartSecondaryIdle()
    {
        StartCoroutine(RandomIdle());
    }
}
