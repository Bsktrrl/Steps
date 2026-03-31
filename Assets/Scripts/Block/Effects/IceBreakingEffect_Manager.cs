using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IceBreakingEffect_Manager : Singleton<IceBreakingEffect_Manager>
{
    [SerializeField] List<GameObject> IceBreakObject_List = new List<GameObject>();

    float CrackEffectDuration = 1.5f;
    float BreakEffectDuration = 1.5f;

    public GameObject blockToEffect;

    bool crack_isGenerated;
    bool break_isGenerated;


    //--------------------


    private void OnEnable()
    {
        Block_Weak.Action_WalkedOnCrackedIce += CrackIce_Start;
        Block_Weak.Action_WalkedOffCrackedIce += BreakIce_Start;
        Movement.Action_RespawnPlayerEarly -= ResetEffects;
    }
    private void OnDisable()
    {
        Block_Weak.Action_WalkedOnCrackedIce -= CrackIce_Start;
        Block_Weak.Action_WalkedOffCrackedIce -= BreakIce_Start;
        Movement.Action_RespawnPlayerEarly -= ResetEffects;
    }


    //--------------------


    public void CrackIce_Start()
    {
        if (Movement.Instance.blockStandingOn == null) return;

        if (!crack_isGenerated)
        {
            crack_isGenerated = true;

            blockToEffect = Movement.Instance.blockStandingOn;

            for (int i = 0; i < IceBreakObject_List.Count; i++)
            {
                if (!IceBreakObject_List[i].activeInHierarchy)
                {
                    IceBreakObject_List[i].transform.SetPositionAndRotation(blockToEffect.transform.position, blockToEffect.transform.rotation);
                    IceBreakObject_List[i].SetActive(true);
                    IceBreakObject_List[i].GetComponent<CrackedIceScript>().Crack();

                    StartCoroutine(Ice_End(CrackEffectDuration, IceBreakObject_List[i]));

                    break;
                }
            }

            StartCoroutine(TurnOFFCracked());
        }
    }
    public void BreakIce_Start()
    {
        //if (Movement.Instance.blockStandingOn == null) return;

        if (!break_isGenerated)
        {
            break_isGenerated = true;

            //blockToEffect = Movement.Instance.blockStandingOn;

            for (int i = 0; i < IceBreakObject_List.Count; i++)
            {
                if (!IceBreakObject_List[i].activeInHierarchy)
                {
                    IceBreakObject_List[i].transform.SetPositionAndRotation(blockToEffect.transform.position, blockToEffect.transform.rotation);
                    IceBreakObject_List[i].SetActive(true);
                    IceBreakObject_List[i].GetComponent<CrackedIceScript>().Break();

                    StartCoroutine(Ice_End(BreakEffectDuration, IceBreakObject_List[i]));

                    break;
                }
            }

            StartCoroutine(TurnOFFBreaking());
        }
    }


    //--------------------


    IEnumerator Ice_End(float duration, GameObject effectObject)
    {
        yield return new WaitForSeconds(CrackEffectDuration);

        effectObject.SetActive(false);
    }
    IEnumerator TurnOFFCracked()
    {
        yield return new WaitForSeconds(0.1f);

        crack_isGenerated = false;
    }
    IEnumerator TurnOFFBreaking()
    {
        yield return new WaitForSeconds(0.1f);

        break_isGenerated = false;
    }


    //--------------------


    void ResetEffects()
    {
        for (int i = 0; i < IceBreakObject_List.Count; i++)
        {
            IceBreakObject_List[i].SetActive(false);
        }
    }
}
