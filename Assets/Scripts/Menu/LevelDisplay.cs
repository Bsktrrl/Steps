using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    [Header("Amount")]
    [SerializeField] TextMeshProUGUI coinAmount_Text;
    [SerializeField] TextMeshProUGUI collectableAmount_Text;

    [Header("Requirement Display")]
    [SerializeField] TextMeshProUGUI coinRequirement_Text;
    [SerializeField] TextMeshProUGUI collectableRequirement_Text;

    [Header("Display")]
    [SerializeField] GameObject playBlocker;
    [SerializeField] GameObject completedMarker;


    //--------------------

    private void Update()
    {
        SetLevelDisplay();
        CanPlayDisplay();
    }
    //private void OnEnable()
    //{
    //    MapManager.mapInfo_hasLoaded += LoadDisplay;
    //    SaveLoad_PlayerStats.playerStats_hasLoaded += LoadDisplay;

    //    SetLevelDisplay();
    //    CanPlayDisplay();
    //}
    //private void OnDisable()
    //{
    //    MapManager.mapInfo_hasLoaded -= LoadDisplay;
    //    SaveLoad_PlayerStats.playerStats_hasLoaded -= LoadDisplay;
    //}
    //void LoadDisplay()
    //{
    //    SetLevelDisplay();
    //    CanPlayDisplay();
    //}


    //--------------------


    void SetLevelDisplay()
    {
        string levelName = gameObject.GetComponent<LoadLevel>().levelToPlay;

        MenuLevelInfo levelinfo = FindObjectOfType<MenuLevelInfo>();

        if (levelinfo)
        {
            if (levelinfo.mapInfo_ToSave != null)
            {
                if (levelinfo.mapInfo_ToSave.map_SaveInfo_List != null)
                {
                    if (levelinfo.mapInfo_ToSave.map_SaveInfo_List.Count > 0)
                    {
                        foreach (Map_SaveInfo map in levelinfo.mapInfo_ToSave.map_SaveInfo_List)
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

                                //Check if level is completed
                                if (map.isCompleted)
                                    completedMarker.SetActive(true);
                                else
                                    completedMarker.SetActive(false);
                            }
                        }
                    }
                }
            }
        }

        coinRequirement_Text.text = gameObject.GetComponent<LoadLevel>().coinsRequirement.ToString();
        collectableRequirement_Text.text = gameObject.GetComponent<LoadLevel>().collectableRequirement.ToString();
    }


    //--------------------


    void CanPlayDisplay()
    {
        if (gameObject.GetComponent<LoadLevel>().CheckIfCanBePlayed())
        {
            playBlocker.SetActive(false);
        }
        else
        {
            playBlocker.SetActive(true);
        }
    }
}
