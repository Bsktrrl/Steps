using System.Collections;
using UnityEngine;

public class Player_Animations : Singleton<Player_Animations>
{
    public Animator anim;


    //--------------------


    private void Start()
    {
        StartCoroutine(RandomBlink());
        StartCoroutine(RandomIdle());
    }

    private void Update()
    {
        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() &&
            (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Water || Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.SwampWater || Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Mud))
        {
            Perform_SwimAnimation(true);
        }
        else
        {
            Perform_SwimAnimation(false);
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

        anim.SetTrigger(AnimationManager.Instance.walk);

        print("1. Set Walk Animation");
    }
    public void Perform_StairSlopeWalkingAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        anim.SetTrigger(AnimationManager.Instance.walk);

        print("2. Set Stair/Slope Animation");
    }
    public void Perform_SwimAnimation(bool state)
    {
        if (Movement.Instance.isMoving) { return; }

        anim.SetBool("InWater", state);

        //anim.SetTrigger(AnimationManager.Instance.swim);

        print("3. Set Swim Animation");
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
