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

    [SerializeField] TextMeshProUGUI stepsCurrentText;
    [SerializeField] TextMeshProUGUI coinTakenText;
    [SerializeField] TextMeshProUGUI collectableTakenText;
    [SerializeField] TextMeshProUGUI stepsTakenText;

    [Header("Abilities")]
    [SerializeField] GameObject FenceSneak;
    [SerializeField] GameObject SwimSuit;
    [SerializeField] GameObject SwiftSwim;
    [SerializeField] GameObject Flippers;
    [SerializeField] GameObject LavaSuit;
    [SerializeField] GameObject LavaSwiftSwim;
    [SerializeField] GameObject HikerGear;
    [SerializeField] GameObject IceSpikes;
    [SerializeField] GameObject GrapplingHook;
    [SerializeField] GameObject Hammer;
    [SerializeField] GameObject ClimbingGear;
    [SerializeField] GameObject Dash;
    [SerializeField] GameObject Ascend;
    [SerializeField] GameObject Descend;
    [SerializeField] GameObject ControlStick;


    //--------------------


    private void Start()
    {
        HideAbilityUI();
        UpdateMapName();
    }
    private void Update()
    {
        UpdateUI();
    }
    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += ShowAbilityUI;
        Interactable_Pickup.Action_PickupGot += ShowAbilityUI;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= ShowAbilityUI;
        Interactable_Pickup.Action_PickupGot -= ShowAbilityUI;
    }


    //--------------------


    void UpdateMapName()
    {
        mapNameText.text = SceneManager.GetActiveScene().name;
    }
    public void UpdateUI()
    {
        UpdateCurrentStepsUI();
        UpdateCoinsTakenUI();
        UpdateCollectableTakenUI();
        UpdateStepsTakenUI();
    }
    public void UpdateCurrentStepsUI()
    {
        stepsCurrentText.text = PlayerStats.Instance.stats.steps_Current.ToString() + " / " + PlayerStats.Instance.stats.steps_Max.ToString();
    }
    void UpdateCoinsTakenUI()
    {
        int counter = 0;

        for (int i = 0; i < MapManager.Instance.mapInfo_ToSave.coinList.Count; i++)
        {
            if (MapManager.Instance.mapInfo_ToSave.coinList[i].isTaken)
            {
                counter++;
            }
        }

        coinTakenText.text = counter.ToString() + " / 10";
    }
    void UpdateCollectableTakenUI()
    {
        int counter = 0;

        for (int i = 0; i < MapManager.Instance.mapInfo_ToSave.collectableList.Count; i++)
        {
            if (MapManager.Instance.mapInfo_ToSave.collectableList[i].isTaken)
            {
                counter++;
            }
        }

        collectableTakenText.text = counter.ToString() + " / 3";
    }
    void UpdateStepsTakenUI()
    {
        int counter = 0;

        for (int i = 0; i < MapManager.Instance.mapInfo_ToSave.maxStepList.Count; i++)
        {
            if (MapManager.Instance.mapInfo_ToSave.maxStepList[i].isTaken)
            {
                counter++;
            }
        }

        stepsTakenText.text = counter.ToString() + " / 3";
    }


    //--------------------


    void HideAbilityUI()
    {
        FenceSneak.SetActive(false);
        SwimSuit.SetActive(false);
        SwiftSwim.SetActive(false);
        Flippers.SetActive(false);
        LavaSuit.SetActive(false);
        LavaSwiftSwim.SetActive(false);
        HikerGear.SetActive(false);
        IceSpikes.SetActive(false);
        GrapplingHook.SetActive(false);
        Hammer.SetActive(false);
        ClimbingGear.SetActive(false);
        Dash.SetActive(false);
        Ascend.SetActive(false);
        Descend.SetActive(false);
        ControlStick.SetActive(false);
    }
    void ShowAbilityUI()
    {
        if (PlayerStats.Instance.stats != null)
        {
            if (PlayerStats.Instance.stats.abilitiesGot_Temporary != null)
            {
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.FenceSneak)
                    FenceSneak.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.SwimSuit)
                    SwimSuit.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.SwiftSwim)
                    SwiftSwim.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers)
                    Flippers.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.LavaSuit)
                    LavaSuit.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.LavaSwiftSwim)
                    LavaSwiftSwim.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.HikerGear)
                    HikerGear.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.IceSpikes)
                    IceSpikes.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook)
                    GrapplingHook.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.Hammer)
                    Hammer.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.ClimbingGear)
                    ClimbingGear.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash)
                    Dash.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.Ascend)
                    Ascend.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.Descend)
                    Descend.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.ControlStick)
                    ControlStick.SetActive(true);
            }

            if (PlayerStats.Instance.stats.abilitiesGot_Permanent != null)
            {
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.FenceSneak)
                    FenceSneak.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.SwimSuit)
                    SwimSuit.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim)
                    SwiftSwim.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers)
                    Flippers.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.LavaSuit)
                    LavaSuit.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.LavaSwiftSwim)
                    LavaSwiftSwim.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.HikerGear)
                    HikerGear.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.IceSpikes)
                    IceSpikes.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook)
                    GrapplingHook.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Hammer)
                    Hammer.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.ClimbingGear)
                    ClimbingGear.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash)
                    Dash.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Ascend)
                    Ascend.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Descend)
                    Descend.SetActive(true);
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.ControlStick)
                    ControlStick.SetActive(true);
            }
        }
    }
}
