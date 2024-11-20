using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Stat Text")]
    [SerializeField] TextMeshProUGUI mapNameText;

    [SerializeField] TextMeshProUGUI stepsText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI collectableText;


    //--------------------


    private void Start()
    {
        UpdateMapName();
    }
    private void Update()
    {
        UpdateUI();
    }


    //--------------------

    void UpdateMapName()
    {
        mapNameText.text = SceneManager.GetActiveScene().name;
    }
    public void UpdateUI()
    {
        UpdateStepsUI();
        UpdateCoinsUI();
        UpdateCollectableUI();
    }
    public void UpdateStepsUI()
    {
        stepsText.text = PlayerStats.Instance.stats.steps_Current.ToString();
    }
    void UpdateCoinsUI()
    {
        int counter = 0;

        for (int i = 0; i < MapManager.Instance.mapInfo_ToSave.coinList.Count; i++)
        {
            if (MapManager.Instance.mapInfo_ToSave.coinList[i].isTaken)
            {
                counter++;
            }
        }

        coinText.text = counter.ToString();
    }
    void UpdateCollectableUI()
    {
        int counter = 0;

        for (int i = 0; i < MapManager.Instance.mapInfo_ToSave.collectableList.Count; i++)
        {
            if (MapManager.Instance.mapInfo_ToSave.collectableList[i].isTaken)
            {
                counter++;
            }
        }

        collectableText.text = counter.ToString();
    }
}
