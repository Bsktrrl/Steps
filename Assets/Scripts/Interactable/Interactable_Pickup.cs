using System;
using System.Collections;
using UnityEngine;

public class Interactable_Pickup : MonoBehaviour
{
    public static event Action Action_PickupGot;
    public static event Action Action_EssencePickupGot;
    public static event Action Action_FootprintPickupGot;
    public static event Action<SkinType> Action_SkinPickupGot;
    public static event Action Action_AbilityPickupGot;

    public static event Action Action_SnorkelGot;
    public static event Action Action_FlippersGot;
    public static event Action Action_JumpingGot;

    public static event Action Action_goalReached_Early;
    public static event Action Action_goalReached;

    MapManager mapManager;

    [Header("Object Type")]
    public SkinType skinReceived;
    public Items itemReceived;
    public Abilities abilityReceived;

    public bool goal;
    public GlueplantType glueplantType;

    internal bool IsNumberedPickup => !goal && (itemReceived != Items.None || abilityReceived != Abilities.None);

    [Header("Glueplant Versions")]
    [SerializeField] GameObject glueplant;
    [SerializeField] GameObject glueplantPicked;

    //--------------------


    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
    }

    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += HideGlueplant;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= HideGlueplant;
    }


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //PlayerManager.Instance.PauseGame();
            //print("Pickup entered");

            if (!TryCollectPickup())
                return;

            mapManager.SaveMapInfo();

            if (goal)
            {
                PlayerManager.Instance.PauseGame();

                //If glueplant is from HUB, update spawnPoint in HUB-level
                if (glueplantType == GlueplantType.HUB)
                {
                    DataManager.Instance.oneTimeRunData_Store.glueplantHUB_PickedUp = true;
                    DataPersistanceManager.instance.SaveGame();
                }

                StartCoroutine(GlueplantExitLevel(1.5f));
            }
            else
            {
                StartCoroutine(DeactivateAfterPickupSound());
            }
        }
    }


    //--------------------


    IEnumerator DeactivateAfterPickupSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        // Hide pickup visuals/collider immediately, but keep this GameObject active so audio can play
        Collider pickupCollider = GetComponent<Collider>();
        if (pickupCollider != null)
            pickupCollider.enabled = false;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        Movement.Instance.Action_PickupAnimation_Complete_Invoke();

        gameObject.SetActive(false);
    }


    //--------------------


    IEnumerator GlueplantExitLevel(float waitTime)
    {
        yield return new WaitForSeconds(0.15f);

        Action_goalReached_Early?.Invoke();
        PlayPickupSound();
        HideGlueplant();

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
        Action_goalReached?.Invoke();

        yield return new WaitForSeconds(1f);

        PlayerManager.Instance.player.GetComponent<PlayerManager>().CompleteLevelAndLeave();
    }


    //--------------------


    public void ShowPickup()
    {
        gameObject.gameObject.SetActive(true);
    }


    bool TryCollectPickup()
    {
        if (!mapManager || mapManager.mapInfo_ToSave == null)
            return false;

        Map_SaveInfo mapInfo = mapManager.mapInfo_ToSave;
        if (goal)
        {
            mapInfo.isCompleted = true;
            Action_PickupGot_isActive();
            Player_Statistics_Manager.Active?.RecordLevelCompletion();
            DataManager.Instance.playerStats_Store = PlayerStats.Instance.stats;
            return true;
        }

        GameObject player = PlayerManager.Instance.player;
        if (!player || !player.TryGetComponent(out PlayerStats playerStats) || playerStats.stats?.itemsGot == null ||
            !TryGetComponent(out PickupData pickupData))
            return false;

        Stats stats = playerStats.stats;
        int pickupNo = pickupData.GetPickupNumber(itemReceived);
        if (!mapInfo.TryMarkPickupTaken(this, pickupNo))
            return false;

        // Store the reward and the collected pickup together when the map is saved.
        DataManager.Instance.playerStats_Store = stats;
        switch (itemReceived)
        {
            case Items.Essence:
                stats.itemsGot.essence_Max++;
                stats.itemsGot.essence_Current++;
                MapStatsGathered.Instance.UpdateEssencePickedUp_Stats(pickupNo);
                Action_EssencePickupGot_isActive();
                break;
            case Items.Footprint:
                stats.steps_Max++;
                stats.steps_Current++;
                MapStatsGathered.Instance.UpdateFootprintPickedUp_Stats(pickupNo);
                Action_FootprintPickupGot_isActive();
                break;
            case Items.Skin:
                stats.itemsGot.skin++;
                MapStatsGathered.Instance.UpdateSkinPickedUp_Stats(pickupNo);
                SkinType skin = UpdateSkinTaken();
                Player_Statistics_Manager.Active?.RecordSkinFound();
                Action_SkinPickupGot?.Invoke(skin);
                break;
            case Items.None:
                GetAbility(stats.abilitiesGot_Temporary, mapInfo.abilitiesGotInLevel, pickupNo);
                break;
        }

        Action_PickupGot_isActive();
        if (abilityReceived != Abilities.None)
            Movement.Instance.UpdateLookDir();
        return true;
    }

    SkinType UpdateSkinTaken()
    {
        Map_SaveInfo mapInfo = mapManager.mapInfo_ToSave;
        SkinType tempType = mapInfo.skintype;
        if (mapInfo.skintype_2 == SkinType.SourceBlock && skinReceived == SkinType.SourceBlock)
            tempType = SkinType.SourceBlock;

        switch (tempType)
        {
            case SkinType.None:
                break;

            case SkinType.Rivergreen_Lv1:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Rivergreen_Lv2:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Rivergreen_Lv3:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Rivergreen_Lv4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Rivergreen_Lv5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level5 = WardrobeSkinState.Available;
                break;

            case SkinType.Sandlands_Lv1:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Sandlands_Lv2:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Sandlands_Lv3:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Sandlands_Lv4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Sandlands_Lv5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level5 = WardrobeSkinState.Available;
                break;

            case SkinType.Frostfield_Lv1:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Frostfield_Lv2:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Frostfield_Lv3:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Frostfield_Lv4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Frostfield_Lv5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level5 = WardrobeSkinState.Available;
                break;

            case SkinType.Firevein_Lv1:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Firevein_Lv2:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Firevein_Lv3:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Firevein_Lv4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Firevein_Lv5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level5 = WardrobeSkinState.Available;
                break;

            case SkinType.Witchmire_Lv1:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Witchmire_Lv2:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Witchmire_Lv3:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Witchmire_Lv4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Witchmire_Lv5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level5 = WardrobeSkinState.Available;
                break;

            case SkinType.Metalworks_Lv1:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Metalworks_Lv2:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Metalworks_Lv3:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Metalworks_Lv4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Metalworks_Lv5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level5 = WardrobeSkinState.Available;
                break;

            case SkinType.Default:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default = WardrobeSkinState.Available;
                break;
            case SkinType.Quartz:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default_Quartz = WardrobeSkinState.Available;
                break;
            case SkinType.SourceBlock:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default_SourceBlock = WardrobeSkinState.Available;
                break;

            default:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default = WardrobeSkinState.Available;
                break;
        }


        return tempType;
    }

    void GetAbility(AbilitiesGot temporaryAbilities, AbilitiesGot levelAbilities, int pickupNo)
    {
        switch (abilityReceived)
        {
            case Abilities.Snorkel:
                temporaryAbilities.Snorkel = levelAbilities.Snorkel = true;
                break;
            case Abilities.Flippers:
                temporaryAbilities.Flippers = levelAbilities.Flippers = true;
                break;
            case Abilities.OxygenTank:
                temporaryAbilities.OxygenTank = levelAbilities.OxygenTank = true;
                break;
            case Abilities.SpringShoes:
                temporaryAbilities.SpringShoes = levelAbilities.SpringShoes = true;
                break;
            case Abilities.GrapplingHook:
                temporaryAbilities.GrapplingHook = levelAbilities.GrapplingHook = true;
                break;
            case Abilities.ClimingGloves:
                temporaryAbilities.ClimingGloves = levelAbilities.ClimingGloves = true;
                break;
            case Abilities.HandDrill:
                temporaryAbilities.HandDrill = levelAbilities.HandDrill = true;
                break;
            case Abilities.DrillHelmet:
                temporaryAbilities.DrillHelmet = levelAbilities.DrillHelmet = true;
                break;
            case Abilities.DrillBoots:
                temporaryAbilities.DrillBoots = levelAbilities.DrillBoots = true;
                break;
            default:
                return;
        }

        MapStatsGathered.Instance.UpdateAbilityPickedUp_Stats(pickupNo);
        Action_AbilityPickupGot_isActive();
        switch (abilityReceived)
        {
            case Abilities.Snorkel:
                Action_SnorkelGot?.Invoke();
                break;
            case Abilities.Flippers:
                Action_FlippersGot?.Invoke();
                break;
            case Abilities.SpringShoes:
                Action_JumpingGot?.Invoke();
                break;
        }

        PopUpManager.Instance.ShowAbilityPopup(abilityReceived);
    }


    //--------------------

    void HideGlueplant()
    {
        if (goal)
        {
            if (mapManager.levelName == LevelNames.HUB)
            {
                glueplant.SetActive(true);
                glueplantPicked.SetActive(false);

                return;
            }

            if (mapManager && mapManager.mapInfo_ToSave != null && mapManager.mapInfo_ToSave.isCompleted)
            {
                glueplant.SetActive(false);
                glueplantPicked.SetActive(true);
            }
            else
            {
                glueplant.SetActive(true);
                glueplantPicked.SetActive(false);
            }
        }
    }


    //--------------------


    void PlayPickupSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource == null || audioSource.clip == null || !gameObject.activeInHierarchy)
            return;

        audioSource.PlayOneShot(audioSource.clip);
    }


    //--------------------


    #region Actions
    public void Action_PickupGot_isActive()
    {
        Action_PickupGot?.Invoke();
    }
    public void Action_EssencePickupGot_isActive()
    {
        Action_EssencePickupGot?.Invoke();
    }
    public void Action_FootprintPickupGot_isActive()
    {
        Action_FootprintPickupGot?.Invoke();
    }
    public void Action_AbilityPickupGot_isActive()
    {
        Action_AbilityPickupGot?.Invoke();
    }
    #endregion
}

[Serializable]
public class ItemStats
{
    public Items item;
    public int amount;
}

public enum Items
{
    None,

    Essence,
    Skin,
    Footprint
}

public enum Abilities
{
    None,

    Snorkel,
    OxygenTank,
    Flippers,

    DrillHelmet,
    DrillBoots,

    HandDrill,

    SpringShoes,

    ClimingGloves,

    GrapplingHook,
}
