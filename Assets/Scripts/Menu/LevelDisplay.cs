using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinAmount_Text;
    [SerializeField] TextMeshProUGUI collectableAmount_Text;

    private void Update()
    {
        SetLevelDisplay();
    }
    private void OnEnable()
    {
        //MenuLevelInfo.menuLevelInfo_hasLoaded += SetLevelDisplay;
        //DataManager.datahasLoaded += SetLevelDisplay;
    }

    private void OnDisable()
    {
        //MenuLevelInfo.menuLevelInfo_hasLoaded -= SetLevelDisplay;
        //DataManager.datahasLoaded -= SetLevelDisplay;
    }

    void SetLevelDisplay()
    {
        string levelName = gameObject.GetComponent<LoadLevel>().levelToPlay;

        if (MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List.Count > 0)
        {
            foreach (Map_SaveInfo map in MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List)
            {
                if (map.mapName == levelName)
                {
                    int counter = 0;
                    for (int i = 0; i < map.coinList.Count; i++)
                    {
                        if (map.coinList[i].isTaken)
                        {
                            counter++;
                        }
                    }

                    coinAmount_Text.text = counter.ToString();

                    counter = 0;
                    for (int i = 0; i < map.collectableList.Count; i++)
                    {
                        if (map.collectableList[i].isTaken)
                        {
                            counter++;
                        }
                    }

                    collectableAmount_Text.text = counter.ToString();
                }
            }
        }
    }
}
