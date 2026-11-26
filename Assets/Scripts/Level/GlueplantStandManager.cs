using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueplantStandManager : Singleton<GlueplantStandManager>
{
    internal bool TryPlaceGlueplant(int standNumber)
    {
        MapManager map = MapManager.Instance;
        if (!map || map.IsCompletingLevel || map.mapInfo_ToSave == null)
            return false;

        StandStats stats = DataManager.Instance.glueplantStandStats_Store?.GetForLevel(map.levelName);
        if (stats == null || !StandStats.IsValidStandNumber(standNumber) || stats.IsTaken(standNumber))
            return false;

        bool completed = stats.CountTaken == StandStats.StandsPerLevel - 1;
        if (completed && !map.TryBeginLevelCompletion())
            return false;

        stats.TryPlace(standNumber);
        if (completed)
        {
            map.mapInfo_ToSave.isCompleted = true;
            Player_Statistics_Manager.Active?.RecordLevelCompletion();
        }

        map.SaveMapInfo();

        if (completed)
        {
            PlayerManager.Instance.PauseGame();
            map.StartCoroutine(GlueplantExitLevel(1.5f));
        }

        return true;
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

        PlayerManager.Instance.FinishLevelExit();
    }
    void PlayPickupSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource == null || audioSource.clip == null || !gameObject.activeInHierarchy)
            return;

        audioSource.PlayOneShot(audioSource.clip);
    }
}
