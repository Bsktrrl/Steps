using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueplantStandManager : MonoBehaviour
{
    [SerializeField] RegionState regionState;


    //--------------------


    private void OnEnable()
    {
        Interactable_GlueplantStand.Action_Glueplant_isPlaced += CheckIfLevelIsComplete;
    }
    private void OnDisable()
    {
        Interactable_GlueplantStand.Action_Glueplant_isPlaced -= CheckIfLevelIsComplete;
    }


    //--------------------


    void CheckIfLevelIsComplete()
    {
        switch (regionState)
        {
            case RegionState.None:
                break;

            case RegionState.Rivergreen:
                if (DataManager.Instance.glueplantStandStats_Store.standStats_Rivergreen.stand_1_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Rivergreen.stand_2_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Rivergreen.stand_3_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Rivergreen.stand_4_isTaken)
                {
                    StartExitingGame();
                }
                break;
            case RegionState.Sandlands:
                if (DataManager.Instance.glueplantStandStats_Store.standStats_Sandlands.stand_1_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Sandlands.stand_2_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Sandlands.stand_3_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Sandlands.stand_4_isTaken)
                {
                    StartExitingGame();
                }
                break;
            case RegionState.Frostfields:
                if (DataManager.Instance.glueplantStandStats_Store.standStats_Frostfield.stand_1_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Frostfield.stand_2_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Frostfield.stand_3_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Frostfield.stand_4_isTaken)
                {
                    StartExitingGame();
                }
                break;
            case RegionState.Firevein:
                if (DataManager.Instance.glueplantStandStats_Store.standStats_Firevein.stand_1_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Firevein.stand_2_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Firevein.stand_3_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Firevein.stand_4_isTaken)
                {
                    StartExitingGame();
                }
                break;
            case RegionState.Witchmire:
                if (DataManager.Instance.glueplantStandStats_Store.standStats_Witchmire.stand_1_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Witchmire.stand_2_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Witchmire.stand_3_isTaken
                    && DataManager.Instance.glueplantStandStats_Store.standStats_Witchmire.stand_4_isTaken)
                {
                    StartExitingGame();
                }
                break;

            case RegionState.Metalworks:
                break;

            default:
                break;
        }
    }

    void StartExitingGame()
    {
        PlayerManager.Instance.PauseGame();

        StartCoroutine(GlueplantExitLevel(1.5f));
    }

    IEnumerator GlueplantExitLevel(float waitTime)
    {
        yield return new WaitForSeconds(0.15f);

        PlayPickupSound();

        //PlayGlueplantEffect
        if (gameObject.GetComponent<PickupAndSmokeScript>() && gameObject.GetComponent<PickupAndSmokeScript>().gatherEffectObject)
        {
            gameObject.GetComponent<PickupAndSmokeScript>().gatherEffectObject.SetActive(true);
            gameObject.GetComponent<PickupAndSmokeScript>().gatherEffectObject.GetComponent<ParticleSystem>().Play();

            yield return new WaitForSeconds(0.05f);

            gameObject.GetComponent<PickupAndSmokeScript>().glueplantObject.SetActive(false);
        }

        yield return new WaitForSeconds(waitTime);

        MapManager.Instance.FadeInBlackScreen();

        yield return new WaitForSeconds(1f);

        MapStatsGathered.Instance.CompleteLevel();

        PlayerManager.Instance.player.GetComponent<PlayerManager>().QuitLevel();
    }
    void PlayPickupSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource == null || audioSource.clip == null || !gameObject.activeInHierarchy)
            return;

        audioSource.PlayOneShot(audioSource.clip);
    }
}
