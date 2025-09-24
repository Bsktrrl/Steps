using System.Collections;
using System.Collections.Generic;
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
    }
    public void Perform_StairSlopeWalkingAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        anim.SetTrigger(AnimationManager.Instance.walk);
    }
    public void Perform_SwimAnimation()
    {
        if (Movement.Instance.isMoving) { return; }

        anim.SetTrigger(AnimationManager.Instance.swim);
    }


    //--------------------


    IEnumerator RandomBlink()
    {
        float time = UnityEngine.Random.Range(0, 10);

        yield return new WaitForSeconds(time);

        anim.SetTrigger(AnimationManager.Instance.blink);

        StartCoroutine(RandomBlink());
    }
    IEnumerator RandomIdle()
    {
        float time = UnityEngine.Random.Range(15, 30);

        yield return new WaitForSeconds(time);

        if (UnityEngine.Random.Range(0, 1) > 0.5f)
            anim.SetTrigger("Confused");
        else
            anim.SetTrigger("Confident");

        //print("2.2. SecondaryIdle");

        Invoke(nameof(StartSecondaryIdle), 0);
    }

    void StartSecondaryIdle()
    {
        StartCoroutine(RandomIdle());
    }
}
