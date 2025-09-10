using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SkinsManager : Singleton<SkinsManager>
{
    [Header("Shop")]
    [SerializeField] SkinWardrobeManager SkinWardrobeCostManager;

    public SkinInfo skinInfo;

    [SerializeField] PauseMenuManager pauseMenuManager;


    //--------------------


    private void OnEnable()
    {
        DataPersistanceManager.Action_NewGame += ResetSkinInfo;
    }
    private void OnDisable()
    {
        DataPersistanceManager.Action_NewGame -= ResetSkinInfo;
    }


    //--------------------


    private void Update()
    {
        if (pauseMenuManager && SkinWardrobeCostManager)
        {
            if (SkinWardrobeCostManager.GetComponent<SkinWardrobeManager>().wardrobeParent.activeInHierarchy)
            {
                PauseMenuManager.Instance.levelDisplay_Parent.SetActive(false);
            }
            else
            {
                PauseMenuManager.Instance.levelDisplay_Parent.SetActive(true);
            }
        }
    }


    //--------------------


    public void LoadData()
    {
        skinInfo = DataManager.Instance.skinsInfo_Store;
    }
    public void SaveData()
    {
        DataManager.Instance.skinsInfo_Store = skinInfo;

        DataPersistanceManager.instance.SaveGame();
    }


    //--------------------


    void ResetSkinInfo()
    {
        skinInfo.activeSkinType = SkinType.None;
        skinInfo.activeHatType = HatType.None;

        skinInfo.skinWardrobeInfo = new SkinsWardrobeInfo();
        skinInfo.skinHatInfo = new SkinsHatInfo();

        skinInfo.skinWardrobeInfo.skin_Default = WardrobeSkinState.Selected;
        SaveData();
    }
}

[Serializable]
public class SkinInfo
{
    public SkinType activeSkinType;
    public HatType activeHatType;

    public SkinsWardrobeInfo skinWardrobeInfo;
    public SkinsHatInfo skinHatInfo;
}
