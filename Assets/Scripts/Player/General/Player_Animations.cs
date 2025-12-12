using System.Collections;
using UnityEngine;

public class Player_Animations : Singleton<Player_Animations>
{
    public Animator anim;

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
    }


    //--------------------


    private void OnEnable()
    {
        Player_KeyInputs.Action_WalkButton_isPressed += UpdateAnimator;
        DataManager.Action_dataHasLoaded += UpdateAnimator;
    }
    private void OnDisable()
    {
        Player_KeyInputs.Action_WalkButton_isPressed -= UpdateAnimator;
        DataManager.Action_dataHasLoaded -= UpdateAnimator;
    }


    //--------------------


    public void UpdateAnimator()
    {
        anim = Player_Body.Instance.selectedSkinBlock.GetComponent<Animator>();
    }


    //--------------------


    public void Perform_WalkingAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        if (!isWalkGliding)
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
