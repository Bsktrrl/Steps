using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplay_PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject ability_Swim;
    [SerializeField] GameObject ability_SwiftSwim;
    [SerializeField] GameObject ability_FreeSwim;

    [SerializeField] GameObject ability_Ascend;
    [SerializeField] GameObject ability_Descend;

    [SerializeField] GameObject ability_Dash;
    [SerializeField] GameObject ability_Jump;

    [SerializeField] GameObject ability_GrapplingHook;
    [SerializeField] GameObject ability_CeilingGrab;


    //--------------------


    private void OnEnable()
    {
        ShowAbilities();
    }


    //--------------------


    void ShowAbilities()
    {
        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.SwimSuit || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.SwimSuit)
            ability_Swim.SetActive(true);
        else
            ability_Swim.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.SwiftSwim || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.SwiftSwim)
            ability_SwiftSwim.SetActive(true);
        else
            ability_SwiftSwim.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.Flippers || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.Flippers)
            ability_FreeSwim.SetActive(true);
        else
            ability_FreeSwim.SetActive(false);


        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.Ascend || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.Ascend)
            ability_Ascend.SetActive(true);
        else
            ability_Ascend.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.Descend || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.Descend)
            ability_Descend.SetActive(true);
        else
            ability_Descend.SetActive(false);


        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.Dash || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.Dash)
            ability_Dash.SetActive(true);
        else
            ability_Dash.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.Jumping || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.Jumping)
            ability_Jump.SetActive(true);
        else
            ability_Jump.SetActive(false);


        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.GrapplingHook || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.GrapplingHook)
            ability_GrapplingHook.SetActive(true);
        else
            ability_GrapplingHook.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.CeilingGrab || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.CeilingGrab)
            ability_CeilingGrab.SetActive(true);
        else
            ability_CeilingGrab.SetActive(false);
    }
}
