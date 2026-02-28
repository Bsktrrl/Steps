using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class FeedbackForm : Singleton<FeedbackForm>
{
    string formURL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSef7zFzWRFbkhTYw5zZDvnoDST2ZNDY3j4FUz9NcJc1uIApxQ/formResponse";

    [SerializeField] PlayerStatsData playerStatsData = new PlayerStatsData();

    List<LevelField_Path> levelField_PathsList = new List<LevelField_Path>();


    //--------------------


    private void Start()
    {
        SetupLevelPaths();
    }
    private void OnEnable()
    {
        //DataManager.Action_dataHasLoaded += ResetSessionStats;
    }
    private void OnDisable()
    {
        //DataManager.Action_dataHasLoaded -= ResetSessionStats;
    }


    //--------------------


    void SetupLevelPaths()
    {
        for (int i = 0; i < 30; i++)
        {
            levelField_PathsList.Add(new LevelField_Path());
        }

        int index = 0;

        #region Rivergreen Lv.1
        levelField_PathsList[index].path_TimeUsed = "entry.1689027362";
        levelField_PathsList[index].path_StepsTaken = "entry.1447210031";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.1079943535";
        levelField_PathsList[index].path_QuitTimer = "entry.1800837785";
        levelField_PathsList[index].path_RespawnTaken = "entry.673851242";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1137183166";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1581533742";

        levelField_PathsList[index].path_Ability_Swim = "entry.1786057615";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.783939847";
        levelField_PathsList[index].path_Ability_Ascend = "entry.510403430";
        levelField_PathsList[index].path_Ability_Descend = "entry.1388112400";
        levelField_PathsList[index].path_Ability_Dash = "entry.163477586";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.2105486132";
        levelField_PathsList[index].path_Ability_Jump = "entry.797390186";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.944978932";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.714993801";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.569553625";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.729176783";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.161518432";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.906810834";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.2081527921";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1435207882";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.899606085";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.818149302";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.522533854";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.542219994";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.854207696";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1109714976";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1897850417";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Rivergreen Lv.2
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1088333264";
        levelField_PathsList[index].path_StepsTaken = "entry.2093440614";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.527504893";
        levelField_PathsList[index].path_QuitTimer = "entry.927557475";
        levelField_PathsList[index].path_RespawnTaken = "entry.1595397881";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1203258864";
        levelField_PathsList[index].path_FreeCamTimer = "entry.510614793";

        levelField_PathsList[index].path_Ability_Swim = "entry.1626717733";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.1565648219";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1503489575";
        levelField_PathsList[index].path_Ability_Descend = "entry.1672437585";
        levelField_PathsList[index].path_Ability_Dash = "entry.1383982329";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.187253854";
        levelField_PathsList[index].path_Ability_Jump = "entry.235869041";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.576474447";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1043475143";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1297067320";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.2057452031";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.1505756054";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1897780007";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.2096823712";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.275051765";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.268449402";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.2119759627";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.730390048";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1202067956";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.50670729";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1026749301";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1150302289";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.825571877";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Rivergreen Lv.3
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.466155704";
        levelField_PathsList[index].path_StepsTaken = "entry.559274352";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.596119727";
        levelField_PathsList[index].path_QuitTimer = "entry.1606359731";
        levelField_PathsList[index].path_RespawnTaken = "entry.1435294474";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1805041080";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1971295432";

        levelField_PathsList[index].path_Ability_Swim = "entry.1546842576";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.1498575380";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1542126223";
        levelField_PathsList[index].path_Ability_Descend = "entry.1331759746";
        levelField_PathsList[index].path_Ability_Dash = "entry.1179669584";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.879218144";
        levelField_PathsList[index].path_Ability_Jump = "entry.1428008737";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.1561051362";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1751490934";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.768944102";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.994776623";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.136927272";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1206912292";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.323915966";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1872221886";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.445975497";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.1010395670";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1077501998";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1737273662";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1979161116";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1492861307";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.757086161";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.1805290995";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "entry.1923175399";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Rivergreen Lv.4
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.275423907";
        levelField_PathsList[index].path_StepsTaken = "entry.649250298";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.1193692580";
        levelField_PathsList[index].path_QuitTimer = "entry.933108437";
        levelField_PathsList[index].path_RespawnTaken = "entry.1547414153";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.983319959";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1575535652";

        levelField_PathsList[index].path_Ability_Swim = "entry.2060120279";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.1006954835";
        levelField_PathsList[index].path_Ability_Ascend = "entry.344117007";
        levelField_PathsList[index].path_Ability_Descend = "entry.126342421";
        levelField_PathsList[index].path_Ability_Dash = "entry.1095432506";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1947711761";
        levelField_PathsList[index].path_Ability_Jump = "entry.1413944675";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.248776292";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.2133688322";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1238274464";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.770852460";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.1860182025";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1472287236";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.668919846";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.950789740";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1214064682";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.915586860";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1046609318";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1701349048";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1842316613";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1571262443";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1985868633";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.1273754573";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "entry.461978382";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "entry.2073140631";
        #endregion
        #region Rivergreen Lv.5
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1203395836";
        levelField_PathsList[index].path_StepsTaken = "entry.371346564";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.23455516";
        levelField_PathsList[index].path_QuitTimer = "entry.1875506135";
        levelField_PathsList[index].path_RespawnTaken = "entry.700113969";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1026559115";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1631291945";

        levelField_PathsList[index].path_Ability_Swim = "entry.1060554516";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.564972786";
        levelField_PathsList[index].path_Ability_Ascend = "entry.540137486";
        levelField_PathsList[index].path_Ability_Descend = "entry.1308795690";
        levelField_PathsList[index].path_Ability_Dash = "entry.1011681284";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1144511555";
        levelField_PathsList[index].path_Ability_Jump = "entry.122965579";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.1232881714";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1427339208";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.104613767";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1476421917";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.576998539";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1417363526";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.1067598188";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1601604851";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.272920332";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.685771594";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1619149750";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1093857787";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.2135948752";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1797012498";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1860879052";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.328028451";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "entry.906472657";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "entry.153295770";
        #endregion

        #region Sandlands Lv.1
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1005533849";
        levelField_PathsList[index].path_StepsTaken = "entry.752660038";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.1588009662";
        levelField_PathsList[index].path_QuitTimer = "entry.466281041";
        levelField_PathsList[index].path_RespawnTaken = "entry.411648931";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1068036485";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1453263511";

        levelField_PathsList[index].path_Ability_Swim = "entry.1602194220";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.164266244";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1016073754";
        levelField_PathsList[index].path_Ability_Descend = "entry.1578447882";
        levelField_PathsList[index].path_Ability_Dash = "entry.740175828";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.2111035183";
        levelField_PathsList[index].path_Ability_Jump = "entry.198798359";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.437694579";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.614367364";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1324232639";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.500724790";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.809741683";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.825515295";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.194850051";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.769166825";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1726089551";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.596172869";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1445030644";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1389843543";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.751626353";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.817473230";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1133612008";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Sandlands Lv.2
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.2101874603";
        levelField_PathsList[index].path_StepsTaken = "entry.1043027523";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.793679318";
        levelField_PathsList[index].path_QuitTimer = "entry.722897019";
        levelField_PathsList[index].path_RespawnTaken = "entry.2070783901";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.420437198";
        levelField_PathsList[index].path_FreeCamTimer = "entry.820198592";

        levelField_PathsList[index].path_Ability_Swim = "entry.549147944";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.2053548788";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1363399488";
        levelField_PathsList[index].path_Ability_Descend = "entry.1772079941";
        levelField_PathsList[index].path_Ability_Dash = "entry.1129490951";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.665279790";
        levelField_PathsList[index].path_Ability_Jump = "entry.1067535536";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.1608070788";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.920932666";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1936905464";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.110402058";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.615651540";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.194692378";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.1674033535";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.628420370";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.785186690";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.252038179";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.2135458203";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.550300473";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1981720400";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.618366787";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1289004178";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Sandlands Lv.3
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1794445566";
        levelField_PathsList[index].path_StepsTaken = "entry.39168325";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.807501438";
        levelField_PathsList[index].path_QuitTimer = "entry.818146076";
        levelField_PathsList[index].path_RespawnTaken = "entry.611547687";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.631067036";
        levelField_PathsList[index].path_FreeCamTimer = "entry.529670627";

        levelField_PathsList[index].path_Ability_Swim = "entry.1914295352";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.1519016928";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1066071305";
        levelField_PathsList[index].path_Ability_Descend = "entry.1638781773";
        levelField_PathsList[index].path_Ability_Dash = "entry.969978031";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1446192146";
        levelField_PathsList[index].path_Ability_Jump = "entry.979582315";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.1995699792";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.547737253";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1225577154";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1174609356";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.1373706884";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.464168259";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.1583514361";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1269988152";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1775826930";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.12846827";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.127729704";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.529059223";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.2076015642";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.197159773";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1602642850";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.343272426";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Sandlands Lv.4
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1473814027";
        levelField_PathsList[index].path_StepsTaken = "entry.1479679149";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.247964947";
        levelField_PathsList[index].path_QuitTimer = "entry.32465777";
        levelField_PathsList[index].path_RespawnTaken = "entry.911493409";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.799727503";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1845396580";

        levelField_PathsList[index].path_Ability_Swim = "entry.835208999";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.818631208";
        levelField_PathsList[index].path_Ability_Ascend = "entry.179441642";
        levelField_PathsList[index].path_Ability_Descend = "entry.1515648813";
        levelField_PathsList[index].path_Ability_Dash = "entry.1185665538";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1236928533";
        levelField_PathsList[index].path_Ability_Jump = "entry.655162285";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.280741510";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.166386201";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1879441342";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1045933020";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.467569789";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.898942101";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.2145905309";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.582694200";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1854895827";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.1471978725";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1756593123";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.758699846";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1993126247";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.304691139";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.643080151";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.1789634768";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "entry.136742916";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Sandlands Lv.5
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.2135922117";
        levelField_PathsList[index].path_StepsTaken = "entry.70287636";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.782793433";
        levelField_PathsList[index].path_QuitTimer = "entry.1149714976";
        levelField_PathsList[index].path_RespawnTaken = "entry.1468109886";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.40836679";
        levelField_PathsList[index].path_FreeCamTimer = "entry.2001796393";

        levelField_PathsList[index].path_Ability_Swim = "entry.234665284";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.1482981039";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1245176785";
        levelField_PathsList[index].path_Ability_Descend = "entry.811900800";
        levelField_PathsList[index].path_Ability_Dash = "entry.894653024";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.329947241";
        levelField_PathsList[index].path_Ability_Jump = "entry.1301673876";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.145131470";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1751208741";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.2141146827";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1048701262";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.67279346";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1133116571";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.1177335198";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.601375519";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1045962422";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.438913308";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1600999564";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1046573106";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1782670067";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1120606404";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.405458399";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.1542557980";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "entry.930705920";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion

        #region Frostfield Lv.1
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.2097580985";
        levelField_PathsList[index].path_StepsTaken = "entry.151313092";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.632847272";
        levelField_PathsList[index].path_QuitTimer = "entry.1339051844";
        levelField_PathsList[index].path_RespawnTaken = "entry.666571304";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1009741074";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1690355685";

        levelField_PathsList[index].path_Ability_Swim = "entry.463408050";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.275293268";
        levelField_PathsList[index].path_Ability_Ascend = "entry.80993002";
        levelField_PathsList[index].path_Ability_Descend = "entry.1734329391";
        levelField_PathsList[index].path_Ability_Dash = "entry.90730224";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.838200930";
        levelField_PathsList[index].path_Ability_Jump = "entry.1493812378";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.220379243";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1523962122";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1268512342";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.903230777";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.998455670";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.100452763";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.199020175";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1632519640";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.200117610";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.391458866";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1242097446";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.795560656";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.334097868";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1656132678";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1138585789";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Frostfield Lv.2
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.931618437";
        levelField_PathsList[index].path_StepsTaken = "entry.366953354";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.1394294158";
        levelField_PathsList[index].path_QuitTimer = "entry.1514679250";
        levelField_PathsList[index].path_RespawnTaken = "entry.1168235485";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1748651282";
        levelField_PathsList[index].path_FreeCamTimer = "entry.2002844332";

        levelField_PathsList[index].path_Ability_Swim = "entry.664460769";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.193361129";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1328686670";
        levelField_PathsList[index].path_Ability_Descend = "entry.470683773";
        levelField_PathsList[index].path_Ability_Dash = "entry.1469192932";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1961964752";
        levelField_PathsList[index].path_Ability_Jump = "entry.1397302048";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.302077133";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.2064622083";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1744673781";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.2033677597";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.869348736";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.670266175";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.669768321";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.299500322";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.147049481";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.2141316505";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1391651236";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.439749893";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.132892148";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.898307188";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1424230259";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Frostfield Lv.3
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.569836201";
        levelField_PathsList[index].path_StepsTaken = "entry.1352321821";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.893392982";
        levelField_PathsList[index].path_QuitTimer = "entry.153710906";
        levelField_PathsList[index].path_RespawnTaken = "entry.184282263";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.135069280";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1910919553";

        levelField_PathsList[index].path_Ability_Swim = "entry.1178039421";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.628584658";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1692286478";
        levelField_PathsList[index].path_Ability_Descend = "entry.1225706947";
        levelField_PathsList[index].path_Ability_Dash = "entry.405574956";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.2116232841";
        levelField_PathsList[index].path_Ability_Jump = "entry.1200941385";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.634614601";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.160711719";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.196756014";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1185014591";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.550706497";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.967415610";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.1615645504";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.515277132";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.792638882";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.820237911";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.2016667411";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.812743071";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.2061346064";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1272893847";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1850049103";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.901707548";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Frostfield Lv.4
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.2112542236";
        levelField_PathsList[index].path_StepsTaken = "entry.88103252";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.1659390858";
        levelField_PathsList[index].path_QuitTimer = "entry.211805916";
        levelField_PathsList[index].path_RespawnTaken = "entry.1206504661";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.213283244";
        levelField_PathsList[index].path_FreeCamTimer = "entry.46825809";

        levelField_PathsList[index].path_Ability_Swim = "entry.382082603";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.611282152";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1016226311";
        levelField_PathsList[index].path_Ability_Descend = "entry.2140662923";
        levelField_PathsList[index].path_Ability_Dash = "entry.751051150";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1465290997";
        levelField_PathsList[index].path_Ability_Jump = "entry.1207626956";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.532590451";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1101164603";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1861162011";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1717135888";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.324271096";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1027814637";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.839281142";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.178655960";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1249842819";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.112937499";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1438215693";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.722286754";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1578130748";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1312616077";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1490465456";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.1642141856";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Frostfield Lv.5
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1248512523";
        levelField_PathsList[index].path_StepsTaken = "entry.1781776020";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.765017777";
        levelField_PathsList[index].path_QuitTimer = "entry.829933367";
        levelField_PathsList[index].path_RespawnTaken = "entry.1805644825";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1133269269";
        levelField_PathsList[index].path_FreeCamTimer = "entry.2050714085";

        levelField_PathsList[index].path_Ability_Swim = "entry.637494335";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.996990987";
        levelField_PathsList[index].path_Ability_Ascend = "entry.287055497";
        levelField_PathsList[index].path_Ability_Descend = "entry.1714614215";
        levelField_PathsList[index].path_Ability_Dash = "entry.1537736853";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1204227727";
        levelField_PathsList[index].path_Ability_Jump = "entry.718381495";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.939492391";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1635569722";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.907887759";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1755461473";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.292860089";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.2139608995";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.513474481";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1797589521";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1110933731";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.730103024";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.367517019";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1913941807";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1471725734";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.116242955";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1746937484";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.835647461";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion

        #region Firevein Lv.1
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1739899260";
        levelField_PathsList[index].path_StepsTaken = "entry.1374254680";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.1587169937";
        levelField_PathsList[index].path_QuitTimer = "entry.622708344";
        levelField_PathsList[index].path_RespawnTaken = "entry.752897889";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.735301699";
        levelField_PathsList[index].path_FreeCamTimer = "entry.831012364";

        levelField_PathsList[index].path_Ability_Swim = "entry.313139522";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.1482907896";
        levelField_PathsList[index].path_Ability_Ascend = "entry.41367433";
        levelField_PathsList[index].path_Ability_Descend = "entry.1851014937";
        levelField_PathsList[index].path_Ability_Dash = "entry.1228677399";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1290946158";
        levelField_PathsList[index].path_Ability_Jump = "entry.257538732";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.1263373835";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1615567106";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1738407250";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.237176134";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.1907360483";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.404337573";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.1217427602";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.977228816";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.743280415";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.1649388433";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.940373262";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1916621530";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1813923506";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.97410660";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1336360137";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Firevein Lv.2
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.221847335";
        levelField_PathsList[index].path_StepsTaken = "entry.428092644";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.333792651";
        levelField_PathsList[index].path_QuitTimer = "entry.2129288641";
        levelField_PathsList[index].path_RespawnTaken = "entry.934976507";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1201967645";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1912012418";

        levelField_PathsList[index].path_Ability_Swim = "entry.103889961";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.1974001866";
        levelField_PathsList[index].path_Ability_Ascend = "entry.289677234";
        levelField_PathsList[index].path_Ability_Descend = "entry.1149835986";
        levelField_PathsList[index].path_Ability_Dash = "entry.510581522";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1624449882";
        levelField_PathsList[index].path_Ability_Jump = "entry.136375604";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.1696488095";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.662390932";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.386084586";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.289214544";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.492809017";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1406294331";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.1515522500";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1712680414";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1566459062";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.1783169157";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.2039341410";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.944928498";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1470022627";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1416578712";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.820341965";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Firevein Lv.3
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.170907253";
        levelField_PathsList[index].path_StepsTaken = "entry.2113057432";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.555062662";
        levelField_PathsList[index].path_QuitTimer = "entry.1482477250";
        levelField_PathsList[index].path_RespawnTaken = "entry.2127956620";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1051476429";
        levelField_PathsList[index].path_FreeCamTimer = "entry.2016027112";

        levelField_PathsList[index].path_Ability_Swim = "entry.2142985026";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.6086307";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1810949701";
        levelField_PathsList[index].path_Ability_Descend = "entry.16205099";
        levelField_PathsList[index].path_Ability_Dash = "entry.904191115";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.84468034";
        levelField_PathsList[index].path_Ability_Jump = "entry.1123828619";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.1508307633";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1361162105";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.625339902";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1080276251";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.2040601275";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1652016365";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.1642789847";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.210142837";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.896037361";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.1249759186";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.484799627";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1515929768";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1824912923";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.237753115";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.2054931401";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.1247067271";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Firevein Lv.4
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1438853862";
        levelField_PathsList[index].path_StepsTaken = "entry.606177498";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.1943766013";
        levelField_PathsList[index].path_QuitTimer = "entry.7584043";
        levelField_PathsList[index].path_RespawnTaken = "entry.230432348";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1147986440";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1763676456";

        levelField_PathsList[index].path_Ability_Swim = "entry.342035863";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.1110712415";
        levelField_PathsList[index].path_Ability_Ascend = "entry.225097744";
        levelField_PathsList[index].path_Ability_Descend = "entry.715806307";
        levelField_PathsList[index].path_Ability_Dash = "entry.2091907857";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.442396274";
        levelField_PathsList[index].path_Ability_Jump = "entry.333112006";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.7139675";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1949146149";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.252865499";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1414348668";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.339300817";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.794180056";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.910442783";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1757084609";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1148739438";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.207645958";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.785423496";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1225913823";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1997050416";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.720537942";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.91106040";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.2146459159";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Firevein Lv.5
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1212535265";
        levelField_PathsList[index].path_StepsTaken = "entry.525765730";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.2015059906";
        levelField_PathsList[index].path_QuitTimer = "entry.614042246";
        levelField_PathsList[index].path_RespawnTaken = "entry.1829169485";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1344320249";
        levelField_PathsList[index].path_FreeCamTimer = "entry.295720216";

        levelField_PathsList[index].path_Ability_Swim = "entry.1189446630";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.906404462";
        levelField_PathsList[index].path_Ability_Ascend = "entry.30402816";
        levelField_PathsList[index].path_Ability_Descend = "entry.1768031357";
        levelField_PathsList[index].path_Ability_Dash = "entry.649730395";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.820707021";
        levelField_PathsList[index].path_Ability_Jump = "entry.171082407";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.1850612078";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1275496795";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.474227084";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1638828285";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.701983082";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.635296792";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.1690718411";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.105821626";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1533745654";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.735999586";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1961287475";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1838881601";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.968704919";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1620714456";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.980376264";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.2094575621";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion

        #region Witchmire Lv.1
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.460623701";
        levelField_PathsList[index].path_StepsTaken = "entry.885049176";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.2079596943";
        levelField_PathsList[index].path_QuitTimer = "entry.1210320064";
        levelField_PathsList[index].path_RespawnTaken = "entry.1833132803";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1025884141";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1500925986";

        levelField_PathsList[index].path_Ability_Swim = "entry.2107041701";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.669551322";
        levelField_PathsList[index].path_Ability_Ascend = "entry.954819733";
        levelField_PathsList[index].path_Ability_Descend = "entry.1114125601";
        levelField_PathsList[index].path_Ability_Dash = "entry.584420386";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1331832835";
        levelField_PathsList[index].path_Ability_Jump = "entry.548028074";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.2037810981";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.594842053";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.517683767";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.119298037";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.1626338215";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.2007168963";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.2017427118";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1892872130";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.16165934";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.4992638";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.2012636330";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.44062202";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1243751256";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.270232068";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1616097799";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Witchmire Lv.2
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1392081099";
        levelField_PathsList[index].path_StepsTaken = "entry.1539877555";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.719679497";
        levelField_PathsList[index].path_QuitTimer = "entry.302909751";
        levelField_PathsList[index].path_RespawnTaken = "entry.422035324";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.86014986";
        levelField_PathsList[index].path_FreeCamTimer = "entry.492442475";

        levelField_PathsList[index].path_Ability_Swim = "entry.1010006346";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.869457866";
        levelField_PathsList[index].path_Ability_Ascend = "entry.129344366";
        levelField_PathsList[index].path_Ability_Descend = "entry.832717242";
        levelField_PathsList[index].path_Ability_Dash = "entry.988794436";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1459571800";
        levelField_PathsList[index].path_Ability_Jump = "entry.2083817522";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.164981346";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1340631398";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1209691646";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.292389904";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.2068540433";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1621323533";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.1042345204";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1001942836";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1050509467";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.1979708914";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.749320912";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.277442405";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1447368669";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.2014466198";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.329276482";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Witchmire Lv.3
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1825209564";
        levelField_PathsList[index].path_StepsTaken = "entry.1613832892";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.901173653";
        levelField_PathsList[index].path_QuitTimer = "entry.1132349341";
        levelField_PathsList[index].path_RespawnTaken = "entry.117032358";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.500254173";
        levelField_PathsList[index].path_FreeCamTimer = "entry.343754038";

        levelField_PathsList[index].path_Ability_Swim = "entry.1295967888";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.1460517231";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1390622554";
        levelField_PathsList[index].path_Ability_Descend = "entry.393264357";
        levelField_PathsList[index].path_Ability_Dash = "entry.1156270719";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.696377638";
        levelField_PathsList[index].path_Ability_Jump = "entry.755323409";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.1059497781";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.741130433";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.329777138";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1129654512";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.1499899121";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.160028178";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.2053570858";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.931582105";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1853484435";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.617875528";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.2130424340";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.977545805";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1644605238";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.809069150";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.303285739";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.1749899712";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Witchmire Lv.4
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1504608970";
        levelField_PathsList[index].path_StepsTaken = "entry.1566319793";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.397631574";
        levelField_PathsList[index].path_QuitTimer = "entry.503331762";
        levelField_PathsList[index].path_RespawnTaken = "entry.915814940";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.129935334";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1154496417";

        levelField_PathsList[index].path_Ability_Swim = "entry.1255056110";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.331192495";
        levelField_PathsList[index].path_Ability_Ascend = "entry.332771044";
        levelField_PathsList[index].path_Ability_Descend = "entry.2106718733";
        levelField_PathsList[index].path_Ability_Dash = "entry.1563037050";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1533016345";
        levelField_PathsList[index].path_Ability_Jump = "entry.1604724052";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.871067896";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1210359052";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.505232719";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.871496147";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.820642350";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.203343284";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.991284328";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.451380599";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.885547182";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.852723678";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.554764843";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.913884604";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1682339537";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.24894706";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1938664671";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.772995408";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Witchmire Lv.5
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.1241388345";
        levelField_PathsList[index].path_StepsTaken = "entry.89506874";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.1626116254";
        levelField_PathsList[index].path_QuitTimer = "entry.535771782";
        levelField_PathsList[index].path_RespawnTaken = "entry.1035179126";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1390288206";
        levelField_PathsList[index].path_FreeCamTimer = "entry.906763614";

        levelField_PathsList[index].path_Ability_Swim = "entry.1216382827";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.250054342";
        levelField_PathsList[index].path_Ability_Ascend = "entry.269636418";
        levelField_PathsList[index].path_Ability_Descend = "entry.798999587";
        levelField_PathsList[index].path_Ability_Dash = "entry.1177623743";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.566079610";
        levelField_PathsList[index].path_Ability_Jump = "entry.1926889257";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.488352781";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.677485517";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.128999983";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1802311999";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.1744821439";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1065892262";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.1209964624";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.688641365";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1615024408";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.349021180";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1630927590";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.448470941";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1003279933";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.427623758";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1706167852";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.1979287675";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion

        #region Metalworks Lv.1
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.2123497026";
        levelField_PathsList[index].path_StepsTaken = "entry.558774139";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.2058334635";
        levelField_PathsList[index].path_QuitTimer = "entry.1614576355";
        levelField_PathsList[index].path_RespawnTaken = "entry.2127386204";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1974080110";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1601450992";

        levelField_PathsList[index].path_Ability_Swim = "entry.1051179991";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.682503614";
        levelField_PathsList[index].path_Ability_Ascend = "entry.543824498";
        levelField_PathsList[index].path_Ability_Descend = "entry.1703588935";
        levelField_PathsList[index].path_Ability_Dash = "entry.1411211329";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.311335727";
        levelField_PathsList[index].path_Ability_Jump = "entry.494977820";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.1686372292";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1202879566";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.904508859";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1988872164";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.1483668795";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1618275337";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.417157494";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1785644380";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.2080416329";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.2017658296";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1726940236";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1522782511";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.2088384616";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.458438021";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1632025202";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.2094551086";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Metalwors Lv.2
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.2065985685";
        levelField_PathsList[index].path_StepsTaken = "entry.612001541";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.921571924";
        levelField_PathsList[index].path_QuitTimer = "entry.1658613932";
        levelField_PathsList[index].path_RespawnTaken = "entry.1379899784";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.317834758";
        levelField_PathsList[index].path_FreeCamTimer = "entry.376780371";

        levelField_PathsList[index].path_Ability_Swim = "entry.1190808749";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.1146119283";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1876112761";
        levelField_PathsList[index].path_Ability_Descend = "entry.118473551";
        levelField_PathsList[index].path_Ability_Dash = "entry.1971475946";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.107774336";
        levelField_PathsList[index].path_Ability_Jump = "entry.826717935";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.439616824";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.1539573808";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1717348642";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.752068906";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.129966294";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.37522286";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.1611447717";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1952908224";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1997666427";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.1181181544";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.898193654";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.53215808";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1529758378";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1954427889";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1473016987";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.175866023";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Metalworks Lv.3
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.534327506";
        levelField_PathsList[index].path_StepsTaken = "entry.1548548552";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.1658582249";
        levelField_PathsList[index].path_QuitTimer = "entry.837048953";
        levelField_PathsList[index].path_RespawnTaken = "entry.1455839908";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1538114691";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1536496700";

        levelField_PathsList[index].path_Ability_Swim = "entry.2135633433";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.2009909901";
        levelField_PathsList[index].path_Ability_Ascend = "entry.2106785600";
        levelField_PathsList[index].path_Ability_Descend = "entry.1422256166";
        levelField_PathsList[index].path_Ability_Dash = "entry.762853399";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1545605370";
        levelField_PathsList[index].path_Ability_Jump = "entry.79069586";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.193877732";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.978092140";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.70106875";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.852870859";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.1007273169";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1242535366";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.126465721";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.1369043111";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1017959069";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.1873904919";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.905553874";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.1941874924";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1429360510";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1086877004";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.590833341";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.974016394";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Metalworks Lv.4
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.51736351";
        levelField_PathsList[index].path_StepsTaken = "entry.503826065";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.451715581";
        levelField_PathsList[index].path_QuitTimer = "entry.1461393004";
        levelField_PathsList[index].path_RespawnTaken = "entry.1137405475";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.1328123723";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1705616597";

        levelField_PathsList[index].path_Ability_Swim = "entry.1029755335";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.167867341";
        levelField_PathsList[index].path_Ability_Ascend = "entry.489661912";
        levelField_PathsList[index].path_Ability_Descend = "entry.1347820051";
        levelField_PathsList[index].path_Ability_Dash = "entry.2040611723";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.1901735696";
        levelField_PathsList[index].path_Ability_Jump = "entry.1691682079";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.1416262531";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.982111026";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.1332663128";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.1122100320";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.1339314512";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.441663177";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.316234226";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.604958710";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1159486829";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.1775012158";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1559244198";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.292093209";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.1551404430";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.221250144";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.48188880";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.141344187";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
        #region Metalworks Lv.5
        index++;
        levelField_PathsList[index].path_TimeUsed = "entry.410542941";
        levelField_PathsList[index].path_StepsTaken = "entry.717990592";
        levelField_PathsList[index].path_GoalReachedTimer = "entry.2138740491";
        levelField_PathsList[index].path_QuitTimer = "entry.2121582633";
        levelField_PathsList[index].path_RespawnTaken = "entry.606856664";
        levelField_PathsList[index].path_CameraRotationTaken = "entry.883277101";
        levelField_PathsList[index].path_FreeCamTimer = "entry.1607777529";

        levelField_PathsList[index].path_Ability_Swim = "entry.2140136645";
        levelField_PathsList[index].path_Ability_SwiftSwim = "entry.340049011";
        levelField_PathsList[index].path_Ability_Ascend = "entry.1609065989";
        levelField_PathsList[index].path_Ability_Descend = "entry.1216402461";
        levelField_PathsList[index].path_Ability_Dash = "entry.867325324";
        levelField_PathsList[index].path_Ability_GrapplingHook = "entry.498774808";
        levelField_PathsList[index].path_Ability_Jump = "entry.2025610923";
        levelField_PathsList[index].path_Ability_CeilingGrab = "entry.1564132972";

        levelField_PathsList[index].path_Essence_1_TotalTimer = "entry.536175636";
        levelField_PathsList[index].path_Essence_2_TotalTimer = "entry.2019567326";
        levelField_PathsList[index].path_Essence_3_TotalTimer = "entry.626249548";
        levelField_PathsList[index].path_Essence_4_TotalTimer = "entry.1615461177";
        levelField_PathsList[index].path_Essence_5_TotalTimer = "entry.1257708341";
        levelField_PathsList[index].path_Essence_6_TotalTimer = "entry.477220870";
        levelField_PathsList[index].path_Essence_7_TotalTimer = "entry.290975906";
        levelField_PathsList[index].path_Essence_8_TotalTimer = "entry.1220582628";
        levelField_PathsList[index].path_Essence_9_TotalTimer = "entry.368368858";
        levelField_PathsList[index].path_Essence_10_TotalTimer = "entry.1390534948";

        levelField_PathsList[index].path_Footprint_1_TotalTimer = "entry.483202784";
        levelField_PathsList[index].path_Footprint_2_TotalTimer = "entry.96210917";
        levelField_PathsList[index].path_Footprint_3_TotalTimer = "entry.1948997635";

        levelField_PathsList[index].path_Skin_TotalTimer = "entry.1139877466";

        levelField_PathsList[index].path_Ability_1_TotalTimer = "entry.855389617";
        levelField_PathsList[index].path_Ability_2_TotalTimer = "";
        levelField_PathsList[index].path_Ability_3_TotalTimer = "";
        #endregion
    }

    public void ResetSessionStats()
    {
        if (DataManager.Instance.oneTimeRunData_Store.playerData_StartOfGame) return;

        playerStatsData.sessionStats.session_No = 0;
        playerStatsData.sessionStats.totalTimeUsed = 0;

        playerStatsData.sessionStats.totalTimeUsed_Menus = 0;
        playerStatsData.sessionStats.totalTimeUsed_MainMenu = 0;
        playerStatsData.sessionStats.totalTimeUsed_OverworldMenu = 0;
        playerStatsData.sessionStats.totalTimeUsed_WardrobeMenu = 0;
        playerStatsData.sessionStats.totalTimeUsed_OptionsMenu = 0;
        playerStatsData.sessionStats.totalTimeUsed_InPauseMenu = 0;
        playerStatsData.sessionStats.totalTimeUsed_InLevels = 0;
        playerStatsData.sessionStats.totalTimeUsed_InFreeCam = 0;

        playerStatsData.sessionStats.totalLevelsVisited = 0; ;
        playerStatsData.sessionStats.totalLevelExited = 0;
        playerStatsData.sessionStats.totalLevelsCleared = 0;
        playerStatsData.sessionStats.totalStepsTaken = 0;
        playerStatsData.sessionStats.totalRespawnTaken = 0;
        playerStatsData.sessionStats.totalCameraRotationTaken = 0;

        playerStatsData.sessionStats.totalTimeEquippedInLevels_Default = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv1 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv2 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv3 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv4 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv5 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv1 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv2 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv3 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv4 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv5 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv1 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv2 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv3 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv4 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv5 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv1 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv2 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv3 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv4 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv5 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv1 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv2 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv3 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv4 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv5 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv1 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv2 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv3 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv4 = 0;
        playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv5 = 0;

        //Make it so that StatsData isn't reset during the game
        DataManager.Instance.oneTimeRunData_Store.playerData_StartOfGame = true;
        DataPersistanceManager.instance.SaveGame();
    }
    public void ResetLevelStats(Level_Stats levelStats)
    {
        levelStats.timeUsed = 0;
        levelStats.stepsTaken = 0;
        levelStats.goalReachedTimer = 0;
        levelStats.quitTimer = 0;
        levelStats.respawnTaken = -1;
        levelStats.cameraRotationTaken = 0;
        levelStats.freeCamTimer = 0;

        levelStats.ability_Swim = 0;
        levelStats.ability_SwiftSwim = 0;
        levelStats.ability_Ascend = 0;
        levelStats.ability_Descend = 0;
        levelStats.ability_Dash = 0;
        levelStats.ability_GrapplingHook = 0;
        levelStats.ability_Jump = 0;
        levelStats.ability_CeilingGrab = 0;

        if (levelStats.essence_1_TotalTimer_Check)
            levelStats.essence_1_TotalTimer = 0;
        if (levelStats.essence_2_TotalTimer_Check)
            levelStats.essence_2_TotalTimer = 0;
        if (levelStats.essence_3_TotalTimer_Check)
            levelStats.essence_3_TotalTimer = 0;
        if (levelStats.essence_4_TotalTimer_Check)
            levelStats.essence_4_TotalTimer = 0;
        if (levelStats.essence_5_TotalTimer_Check)
            levelStats.essence_5_TotalTimer = 0;
        if (levelStats.essence_6_TotalTimer_Check)
            levelStats.essence_6_TotalTimer = 0;
        if (levelStats.essence_7_TotalTimer_Check)
            levelStats.essence_7_TotalTimer = 0;
        if (levelStats.essence_8_TotalTimer_Check)
            levelStats.essence_8_TotalTimer = 0;
        if (levelStats.essence_9_TotalTimer_Check)
            levelStats.essence_9_TotalTimer = 0;
        if (levelStats.essence_10_TotalTimer_Check)
            levelStats.essence_10_TotalTimer = 0;

        if (levelStats.footprint_1_TotalTimer_Check)
            levelStats.footprint_1_TotalTimer = 0;
        if (levelStats.footprint_2_TotalTimer_Check)
            levelStats.footprint_2_TotalTimer = 0;
        if (levelStats.footprint_3_TotalTimer_Check)
            levelStats.footprint_3_TotalTimer = 0;

        if (levelStats.skin_TotalTimer_Check)
            levelStats.skin_TotalTimer = 0;

        if (levelStats.ability_1_TotalTimer_Check)
            levelStats.ability_1_TotalTimer = 0;
        if (levelStats.ability_2_TotalTimer_Check)
            levelStats.ability_2_TotalTimer = 0;
        if (levelStats.ability_3_TotalTimer_Check)
            levelStats.ability_3_TotalTimer = 0;
    }


    //--------------------


    #region Submit Feedback
    public void SubmitFeedback_Session()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Session(DataManager.Instance.PlayerStatsData_Store.sessionStats, ref playerStatsData.sessionStats);
        StartCoroutine(AddSessionField());
    }

    public void SubmitFeedback_Rivergreen_Lv1()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.rivergreenLv1_Stats, ref playerStatsData.rivergreenLv1_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.rivergreenLv1_Stats, levelField_PathsList[0]));

        print("300000. SubmitFeedback_Rivergreen_Lv1");
    }
    public void SubmitFeedback_Rivergreen_Lv2()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.rivergreenLv2_Stats, ref playerStatsData.rivergreenLv2_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.rivergreenLv2_Stats, levelField_PathsList[1]));
    }
    public void SubmitFeedback_Rivergreen_Lv3()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.rivergreenLv3_Stats, ref playerStatsData.rivergreenLv3_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.rivergreenLv3_Stats, levelField_PathsList[2]));
    }
    public void SubmitFeedback_Rivergreen_Lv4()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.rivergreenLv4_Stats, ref playerStatsData.rivergreenLv4_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.rivergreenLv4_Stats, levelField_PathsList[3]));
    }
    public void SubmitFeedback_Rivergreen_Lv5()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.rivergreenLv5_Stats, ref playerStatsData.rivergreenLv5_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.rivergreenLv5_Stats, levelField_PathsList[4]));
    }

    public void SubmitFeedback_Sandlands_Lv1()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.sandlandsLv1_Stats, ref playerStatsData.sandlandsLv1_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.sandlandsLv1_Stats, levelField_PathsList[5]));
    }
    public void SubmitFeedback_Sandlands_Lv2()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.sandlandsLv2_Stats, ref playerStatsData.sandlandsLv2_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.sandlandsLv2_Stats, levelField_PathsList[6]));
    }
    public void SubmitFeedback_Sandlands_Lv3()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.sandlandsLv3_Stats, ref playerStatsData.sandlandsLv3_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.sandlandsLv3_Stats, levelField_PathsList[7]));
    }
    public void SubmitFeedback_Sandlands_Lv4()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.sandlandsLv4_Stats, ref playerStatsData.sandlandsLv4_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.sandlandsLv4_Stats, levelField_PathsList[8]));
    }
    public void SubmitFeedback_Sandlands_Lv5()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.sandlandsLv5_Stats, ref playerStatsData.sandlandsLv5_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.sandlandsLv5_Stats, levelField_PathsList[9]));
    }

    public void SubmitFeedback_Frostfield_Lv1()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.frostfieldLv1_Stats, ref playerStatsData.frostfieldLv1_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.frostfieldLv1_Stats, levelField_PathsList[10]));
    }
    public void SubmitFeedback_Frostfield_Lv2()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.frostfieldLv2_Stats, ref playerStatsData.frostfieldLv2_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.frostfieldLv2_Stats, levelField_PathsList[11]));
    }
    public void SubmitFeedback_Frostfield_Lv3()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.frostfieldLv3_Stats, ref playerStatsData.frostfieldLv3_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.frostfieldLv3_Stats, levelField_PathsList[12]));
    }
    public void SubmitFeedback_Frostfield_Lv4()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.frostfieldLv4_Stats, ref playerStatsData.frostfieldLv4_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.frostfieldLv4_Stats, levelField_PathsList[13]));
    }
    public void SubmitFeedback_Frostfield_Lv5()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.frostfieldLv5_Stats, ref playerStatsData.frostfieldLv5_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.frostfieldLv5_Stats, levelField_PathsList[14]));
    }

    public void SubmitFeedback_Firevein_Lv1()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.fireveinLv1_Stats, ref playerStatsData.fireveinLv1_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.fireveinLv1_Stats, levelField_PathsList[15]));
    }
    public void SubmitFeedback_Firevein_Lv2()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.fireveinLv2_Stats, ref playerStatsData.fireveinLv2_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.fireveinLv2_Stats, levelField_PathsList[16]));
    }
    public void SubmitFeedback_Firevein_Lv3()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.fireveinLv3_Stats, ref playerStatsData.fireveinLv3_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.fireveinLv3_Stats, levelField_PathsList[17]));
    }
    public void SubmitFeedback_Firevein_Lv4()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.fireveinLv4_Stats, ref playerStatsData.fireveinLv4_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.fireveinLv4_Stats, levelField_PathsList[18]));
    }
    public void SubmitFeedback_Firevein_Lv5()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.fireveinLv5_Stats, ref playerStatsData.fireveinLv5_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.fireveinLv5_Stats, levelField_PathsList[19]));
    }

    public void SubmitFeedback_Witchmire_Lv1()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.witchmireLv1_Stats, ref playerStatsData.witchmireLv1_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.witchmireLv1_Stats, levelField_PathsList[20]));
    }
    public void SubmitFeedback_Witchmire_Lv2()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.witchmireLv2_Stats, ref playerStatsData.witchmireLv2_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.witchmireLv2_Stats, levelField_PathsList[21]));
    }
    public void SubmitFeedback_Witchmire_Lv3()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.witchmireLv3_Stats, ref playerStatsData.witchmireLv3_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.witchmireLv3_Stats, levelField_PathsList[22]));
    }
    public void SubmitFeedback_Witchmire_Lv4()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.witchmireLv4_Stats, ref playerStatsData.witchmireLv4_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.witchmireLv4_Stats, levelField_PathsList[23]));
    }
    public void SubmitFeedback_Witchmire_Lv5()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.witchmireLv5_Stats, ref playerStatsData.witchmireLv5_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.witchmireLv5_Stats, levelField_PathsList[24]));
    }

    public void SubmitFeedback_Metalworks_Lv1()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.metalworksLv1_Stats, ref playerStatsData.metalworksLv1_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.metalworksLv1_Stats, levelField_PathsList[25]));
    }
    public void SubmitFeedback_Metalworks_Lv2()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.metalworksLv2_Stats, ref playerStatsData.metalworksLv2_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.metalworksLv2_Stats, levelField_PathsList[26]));
    }
    public void SubmitFeedback_Metalworks_Lv3()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.metalworksLv3_Stats, ref playerStatsData.metalworksLv3_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.metalworksLv3_Stats, levelField_PathsList[27]));
    }
    public void SubmitFeedback_Metalworks_Lv4()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.metalworksLv4_Stats, ref playerStatsData.metalworksLv4_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.metalworksLv4_Stats, levelField_PathsList[28]));
    }
    public void SubmitFeedback_Metalworks_Lv5()
    {
        if (!PlayerDataManager.Instance.collectData) return;

        UpdatePlayerFeedbackSave_Level(DataManager.Instance.PlayerStatsData_Store.metalworksLv5_Stats, ref playerStatsData.metalworksLv5_Stats);
        StartCoroutine(Level_Feedback(playerStatsData.metalworksLv5_Stats, levelField_PathsList[29]));
    }
    #endregion


    //--------------------


    void UpdatePlayerFeedbackSave_Session(Session_Stats levelStats_DataManager, ref Session_Stats levelStats)
    {
        levelStats.session_No = levelStats_DataManager.session_No;
        levelStats.totalTimeUsed = levelStats_DataManager.totalTimeUsed;

        levelStats.totalTimeUsed_Menus = levelStats_DataManager.totalTimeUsed_Menus;
        levelStats.totalTimeUsed_MainMenu = levelStats_DataManager.totalTimeUsed_MainMenu;
        levelStats.totalTimeUsed_OverworldMenu = levelStats_DataManager.totalTimeUsed_OverworldMenu;
        levelStats.totalTimeUsed_WardrobeMenu = levelStats_DataManager.totalTimeUsed_WardrobeMenu;
        levelStats.totalTimeUsed_OptionsMenu = levelStats_DataManager.totalTimeUsed_OptionsMenu;
        levelStats.totalTimeUsed_InPauseMenu = levelStats_DataManager.totalTimeUsed_InPauseMenu;
        levelStats.totalTimeUsed_InLevels = levelStats_DataManager.totalTimeUsed_InLevels;
        levelStats.totalTimeUsed_InFreeCam = levelStats_DataManager.totalTimeUsed_InFreeCam;

        levelStats.totalLevelsVisited = levelStats_DataManager.totalLevelsVisited;
        levelStats.totalLevelExited = levelStats_DataManager.totalLevelExited;
        levelStats.totalLevelsCleared = levelStats_DataManager.totalLevelsCleared;
        levelStats.totalStepsTaken = levelStats_DataManager.totalStepsTaken;
        levelStats.totalRespawnTaken = levelStats_DataManager.totalRespawnTaken;
        levelStats.totalCameraRotationTaken = levelStats_DataManager.totalCameraRotationTaken;
        
        levelStats.totalTimeEquippedInLevels_Default = levelStats_DataManager.totalTimeEquippedInLevels_Default;
        levelStats.totalTimeEquippedInLevels_RivergreenLv1 = levelStats_DataManager.totalTimeEquippedInLevels_RivergreenLv1;
        levelStats.totalTimeEquippedInLevels_RivergreenLv2 = levelStats_DataManager.totalTimeEquippedInLevels_RivergreenLv2;
        levelStats.totalTimeEquippedInLevels_RivergreenLv3 = levelStats_DataManager.totalTimeEquippedInLevels_RivergreenLv3;
        levelStats.totalTimeEquippedInLevels_RivergreenLv4 = levelStats_DataManager.totalTimeEquippedInLevels_RivergreenLv4;
        levelStats.totalTimeEquippedInLevels_RivergreenLv5 = levelStats_DataManager.totalTimeEquippedInLevels_RivergreenLv5;
        levelStats.totalTimeEquippedInLevels_SandlandsLv1 = levelStats_DataManager.totalTimeEquippedInLevels_SandlandsLv1;
        levelStats.totalTimeEquippedInLevels_SandlandsLv2 = levelStats_DataManager.totalTimeEquippedInLevels_SandlandsLv2;
        levelStats.totalTimeEquippedInLevels_SandlandsLv3 = levelStats_DataManager.totalTimeEquippedInLevels_SandlandsLv3;
        levelStats.totalTimeEquippedInLevels_SandlandsLv4 = levelStats_DataManager.totalTimeEquippedInLevels_SandlandsLv4;
        levelStats.totalTimeEquippedInLevels_SandlandsLv5 = levelStats_DataManager.totalTimeEquippedInLevels_SandlandsLv5;
        levelStats.totalTimeEquippedInLevels_FrostfieldLv1 = levelStats_DataManager.totalTimeEquippedInLevels_FrostfieldLv1;
        levelStats.totalTimeEquippedInLevels_FrostfieldLv2 = levelStats_DataManager.totalTimeEquippedInLevels_FrostfieldLv2;
        levelStats.totalTimeEquippedInLevels_FrostfieldLv3 = levelStats_DataManager.totalTimeEquippedInLevels_FrostfieldLv3;
        levelStats.totalTimeEquippedInLevels_FrostfieldLv4 = levelStats_DataManager.totalTimeEquippedInLevels_FrostfieldLv4;
        levelStats.totalTimeEquippedInLevels_FrostfieldLv5 = levelStats_DataManager.totalTimeEquippedInLevels_FrostfieldLv5;
        levelStats.totalTimeEquippedInLevels_FireveinLv1 = levelStats_DataManager.totalTimeEquippedInLevels_FireveinLv1;
        levelStats.totalTimeEquippedInLevels_FireveinLv2 = levelStats_DataManager.totalTimeEquippedInLevels_FireveinLv2;
        levelStats.totalTimeEquippedInLevels_FireveinLv3 = levelStats_DataManager.totalTimeEquippedInLevels_FireveinLv3;
        levelStats.totalTimeEquippedInLevels_FireveinLv4 = levelStats_DataManager.totalTimeEquippedInLevels_FireveinLv4;
        levelStats.totalTimeEquippedInLevels_FireveinLv5 = levelStats_DataManager.totalTimeEquippedInLevels_FireveinLv5;
        levelStats.totalTimeEquippedInLevels_WitchmireLv1 = levelStats_DataManager.totalTimeEquippedInLevels_WitchmireLv1;
        levelStats.totalTimeEquippedInLevels_WitchmireLv2 = levelStats_DataManager.totalTimeEquippedInLevels_WitchmireLv2;
        levelStats.totalTimeEquippedInLevels_WitchmireLv3 = levelStats_DataManager.totalTimeEquippedInLevels_WitchmireLv3;
        levelStats.totalTimeEquippedInLevels_WitchmireLv4 = levelStats_DataManager.totalTimeEquippedInLevels_WitchmireLv4;
        levelStats.totalTimeEquippedInLevels_WitchmireLv5 = levelStats_DataManager.totalTimeEquippedInLevels_WitchmireLv5;
        levelStats.totalTimeEquippedInLevels_MetalworksLv1 = levelStats_DataManager.totalTimeEquippedInLevels_MetalworksLv1;
        levelStats.totalTimeEquippedInLevels_MetalworksLv2 = levelStats_DataManager.totalTimeEquippedInLevels_MetalworksLv2;
        levelStats.totalTimeEquippedInLevels_MetalworksLv3 = levelStats_DataManager.totalTimeEquippedInLevels_MetalworksLv3;
        levelStats.totalTimeEquippedInLevels_MetalworksLv4 = levelStats_DataManager.totalTimeEquippedInLevels_MetalworksLv4;
        levelStats.totalTimeEquippedInLevels_MetalworksLv5 = levelStats_DataManager.totalTimeEquippedInLevels_MetalworksLv5;
    }
    void UpdatePlayerFeedbackSave_Level(Level_Stats levelStats_DataManager, ref Level_Stats levelStats)
    {
        levelStats.timeUsed = levelStats_DataManager.timeUsed;
        levelStats.stepsTaken = levelStats_DataManager.stepsTaken;
        levelStats.goalReachedTimer = levelStats_DataManager.goalReachedTimer;
        levelStats.quitTimer = levelStats_DataManager.quitTimer;
        levelStats.respawnTaken = levelStats_DataManager.respawnTaken;
        levelStats.cameraRotationTaken = levelStats_DataManager.cameraRotationTaken;
        levelStats.freeCamTimer = levelStats_DataManager.freeCamTimer;

        levelStats.ability_Swim = levelStats_DataManager.ability_Swim;
        levelStats.ability_SwiftSwim = levelStats_DataManager.ability_SwiftSwim;
        levelStats.ability_Ascend = levelStats_DataManager.ability_Ascend;
        levelStats.ability_Descend = levelStats_DataManager.ability_Descend;
        levelStats.ability_Dash = levelStats_DataManager.ability_Dash;
        levelStats.ability_GrapplingHook = levelStats_DataManager.ability_GrapplingHook;
        levelStats.ability_Jump = levelStats_DataManager.ability_Jump;
        levelStats.ability_CeilingGrab = levelStats_DataManager.ability_CeilingGrab;

        levelStats.essence_1_TotalTimer = levelStats_DataManager.essence_1_TotalTimer;
        levelStats.essence_2_TotalTimer = levelStats_DataManager.essence_2_TotalTimer;
        levelStats.essence_3_TotalTimer = levelStats_DataManager.essence_3_TotalTimer;
        levelStats.essence_4_TotalTimer = levelStats_DataManager.essence_4_TotalTimer;
        levelStats.essence_5_TotalTimer = levelStats_DataManager.essence_5_TotalTimer;
        levelStats.essence_6_TotalTimer = levelStats_DataManager.essence_6_TotalTimer;
        levelStats.essence_7_TotalTimer = levelStats_DataManager.essence_7_TotalTimer;
        levelStats.essence_8_TotalTimer = levelStats_DataManager.essence_8_TotalTimer;
        levelStats.essence_9_TotalTimer = levelStats_DataManager.essence_9_TotalTimer;
        levelStats.essence_10_TotalTimer = levelStats_DataManager.essence_10_TotalTimer;

        levelStats.essence_1_TotalTimer_Check = levelStats_DataManager.essence_1_TotalTimer_Check;
        levelStats.essence_2_TotalTimer_Check = levelStats_DataManager.essence_2_TotalTimer_Check;
        levelStats.essence_3_TotalTimer_Check = levelStats_DataManager.essence_3_TotalTimer_Check;
        levelStats.essence_4_TotalTimer_Check = levelStats_DataManager.essence_4_TotalTimer_Check;
        levelStats.essence_5_TotalTimer_Check = levelStats_DataManager.essence_5_TotalTimer_Check;
        levelStats.essence_6_TotalTimer_Check = levelStats_DataManager.essence_6_TotalTimer_Check;
        levelStats.essence_7_TotalTimer_Check = levelStats_DataManager.essence_7_TotalTimer_Check;
        levelStats.essence_8_TotalTimer_Check = levelStats_DataManager.essence_8_TotalTimer_Check;
        levelStats.essence_9_TotalTimer_Check = levelStats_DataManager.essence_9_TotalTimer_Check;
        levelStats.essence_10_TotalTimer_Check = levelStats_DataManager.essence_10_TotalTimer_Check;

        levelStats.footprint_1_TotalTimer = levelStats_DataManager.footprint_1_TotalTimer;
        levelStats.footprint_2_TotalTimer = levelStats_DataManager.footprint_2_TotalTimer;
        levelStats.footprint_3_TotalTimer = levelStats_DataManager.footprint_3_TotalTimer;
        levelStats.footprint_1_TotalTimer_Check = levelStats_DataManager.footprint_1_TotalTimer_Check;
        levelStats.footprint_2_TotalTimer_Check = levelStats_DataManager.footprint_2_TotalTimer_Check;
        levelStats.footprint_3_TotalTimer_Check = levelStats_DataManager.footprint_3_TotalTimer_Check;

        levelStats.skin_TotalTimer = levelStats_DataManager.skin_TotalTimer;
        levelStats.skin_TotalTimer_Check = levelStats_DataManager.skin_TotalTimer_Check;

        levelStats.ability_1_TotalTimer = levelStats_DataManager.ability_1_TotalTimer;
        levelStats.ability_2_TotalTimer = levelStats_DataManager.ability_2_TotalTimer;
        levelStats.ability_3_TotalTimer = levelStats_DataManager.ability_3_TotalTimer;
        levelStats.ability_1_TotalTimer_Check = levelStats_DataManager.ability_1_TotalTimer_Check;
        levelStats.ability_2_TotalTimer_Check = levelStats_DataManager.ability_2_TotalTimer_Check;
        levelStats.ability_3_TotalTimer_Check = levelStats_DataManager.ability_3_TotalTimer_Check;
    }


    //--------------------


    IEnumerator AddSessionField()
    {
        WWWForm form = new WWWForm();

        //General
        if (playerStatsData.sessionStats.session_No > 0)
            form.AddField("entry.1252827554", playerStatsData.sessionStats.session_No.ToString());
        if (playerStatsData.sessionStats.totalTimeUsed > 0)
            form.AddField("entry.240663572", playerStatsData.sessionStats.totalTimeUsed.ToString("F2"));

        //Time
        if (playerStatsData.sessionStats.totalTimeUsed_Menus > 0)
            form.AddField("entry.1416356909", playerStatsData.sessionStats.totalTimeUsed_Menus.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeUsed_MainMenu > 0)
            form.AddField("entry.1534190894", playerStatsData.sessionStats.totalTimeUsed_MainMenu.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeUsed_OverworldMenu > 0)
            form.AddField("entry.272428771", playerStatsData.sessionStats.totalTimeUsed_OverworldMenu.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeUsed_WardrobeMenu > 0)
            form.AddField("entry.3208872", playerStatsData.sessionStats.totalTimeUsed_WardrobeMenu.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeUsed_OptionsMenu > 0)
            form.AddField("entry.873769790", playerStatsData.sessionStats.totalTimeUsed_OptionsMenu.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeUsed_InLevels > 0)
            form.AddField("entry.1186270066", playerStatsData.sessionStats.totalTimeUsed_InLevels.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeUsed_InPauseMenu > 0)
            form.AddField("entry.1243543042", playerStatsData.sessionStats.totalTimeUsed_InPauseMenu.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeUsed_InFreeCam > 0)
            form.AddField("entry.725598216", playerStatsData.sessionStats.totalTimeUsed_InFreeCam.ToString("F2"));

        //Counts
        if (playerStatsData.sessionStats.totalLevelsVisited > 0)
            form.AddField("entry.1362508840", playerStatsData.sessionStats.totalLevelsVisited.ToString());
        if (playerStatsData.sessionStats.totalLevelExited > 0)
            form.AddField("entry.1284439705", playerStatsData.sessionStats.totalLevelExited.ToString());
        if (playerStatsData.sessionStats.totalLevelsCleared > 0)
            form.AddField("entry.1709317079", playerStatsData.sessionStats.totalLevelsCleared.ToString());
        if (playerStatsData.sessionStats.totalStepsTaken > 0)
            form.AddField("entry.525598876", playerStatsData.sessionStats.totalStepsTaken.ToString());
        if (playerStatsData.sessionStats.totalRespawnTaken > 0)
            form.AddField("entry.219862598", playerStatsData.sessionStats.totalRespawnTaken.ToString());
        if (playerStatsData.sessionStats.totalCameraRotationTaken > 0)
            form.AddField("entry.1285447355", playerStatsData.sessionStats.totalCameraRotationTaken.ToString());

        //Wardrobe
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_Default > 0)
            form.AddField("entry.1206439869", playerStatsData.sessionStats.totalTimeEquippedInLevels_Default.ToString("F2"));

        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv1 > 0)
            form.AddField("entry.1343052800", playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv1.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv2 > 0)
            form.AddField("entry.458440323", playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv2.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv3 > 0)
            form.AddField("entry.1037629915", playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv3.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv4 > 0)
            form.AddField("entry.932052857", playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv4.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv5 > 0)
            form.AddField("entry.1721013310", playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv5.ToString("F2"));

        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv1 > 0)
            form.AddField("entry.1949758081", playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv1.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv2 > 0)
            form.AddField("entry.191480969", playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv2.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv3 > 0)
            form.AddField("entry.625830122", playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv3.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv4 > 0)
            form.AddField("entry.874509393", playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv4.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv5 > 0)
            form.AddField("entry.1854135177", playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv5.ToString("F2"));

        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv1 > 0)
            form.AddField("entry.1347517780", playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv1.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv2 > 0)
            form.AddField("entry.1939783691", playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv2.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv3 > 0)
            form.AddField("entry.1107810740", playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv3.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv4 > 0)
            form.AddField("entry.1727229317", playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv4.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv5 > 0)
            form.AddField("entry.606740502", playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv5.ToString("F2"));

        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv1 > 0)
            form.AddField("entry.1090823188", playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv1.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv2 > 0)
            form.AddField("entry.2049915036", playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv2.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv3 > 0)
            form.AddField("entry.514067095", playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv3.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv4 > 0)
            form.AddField("entry.102125640", playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv4.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv5 > 0)
            form.AddField("entry.805850594", playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv5.ToString("F2"));

        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv1 > 0)
            form.AddField("entry.1027326274", playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv1.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv2 > 0)
            form.AddField("entry.607439042", playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv2.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv3 > 0)
            form.AddField("entry.1553823692", playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv3.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv4 > 0)
            form.AddField("entry.1671659912", playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv4.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv5 > 0)
            form.AddField("entry.1261180017", playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv5.ToString("F2"));

        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv1 > 0)
            form.AddField("entry.1409198516", playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv1.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv2 > 0)
            form.AddField("entry.1029303544", playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv2.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv3 > 0)
            form.AddField("entry.25719402", playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv3.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv4 > 0)
            form.AddField("entry.763513455", playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv4.ToString("F2"));
        if (playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv5 > 0)
            form.AddField("entry.1183476573", playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv5.ToString("F2"));

        using (UnityWebRequest www = UnityWebRequest.Post(formURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                //Debug.Log("Feedback submitted successfully");

                //ResetSessionStats();
            }
            else
            {
                //Debug.Log("Error in feedback submission: " + www.error);
            }
        }
    }
    IEnumerator Level_Feedback(Level_Stats levelStats, LevelField_Path levelField_Path)
    {
        WWWForm form = new WWWForm();

        if (levelStats.timeUsed > 0)
            form.AddField(levelField_Path.path_TimeUsed, levelStats.timeUsed.ToString("F2"));
        if (levelStats.stepsTaken > 0)
            form.AddField(levelField_Path.path_StepsTaken, levelStats.stepsTaken.ToString());
        if (levelStats.goalReachedTimer > 0)
            form.AddField(levelField_Path.path_GoalReachedTimer, levelStats.goalReachedTimer.ToString("F2"));
        if (levelStats.quitTimer > 0)
            form.AddField(levelField_Path.path_QuitTimer, levelStats.quitTimer.ToString("F2"));
        if (levelStats.respawnTaken > 0)
            form.AddField(levelField_Path.path_RespawnTaken, levelStats.respawnTaken.ToString());
        if (levelStats.cameraRotationTaken > 0)
            form.AddField(levelField_Path.path_CameraRotationTaken, levelStats.cameraRotationTaken.ToString());
        if (levelStats.freeCamTimer > 0)
            form.AddField(levelField_Path.path_FreeCamTimer, levelStats.freeCamTimer.ToString("F2"));

        if (levelStats.ability_Swim > 0)
            form.AddField(levelField_Path.path_Ability_Swim, levelStats.ability_Swim.ToString());
        if (levelStats.ability_SwiftSwim > 0)
            form.AddField(levelField_Path.path_Ability_SwiftSwim, levelStats.ability_SwiftSwim.ToString());
        if (levelStats.ability_Ascend > 0)
            form.AddField(levelField_Path.path_Ability_Ascend, levelStats.ability_Ascend.ToString());
        if (levelStats.ability_Descend > 0)
            form.AddField(levelField_Path.path_Ability_Descend, levelStats.ability_Descend.ToString());
        if (levelStats.ability_Dash > 0)
            form.AddField(levelField_Path.path_Ability_Dash, levelStats.ability_Dash.ToString());
        if (levelStats.ability_GrapplingHook > 0)
            form.AddField(levelField_Path.path_Ability_GrapplingHook, levelStats.ability_GrapplingHook.ToString());
        if (levelStats.ability_Jump > 0)
            form.AddField(levelField_Path.path_Ability_Jump, levelStats.ability_Jump.ToString());
        if (levelStats.ability_CeilingGrab > 0)
            form.AddField(levelField_Path.path_Ability_CeilingGrab, levelStats.ability_CeilingGrab.ToString());

        if (levelStats.essence_1_TotalTimer_Check && levelStats.essence_1_TotalTimer > 0)
            form.AddField(levelField_Path.path_Essence_1_TotalTimer, levelStats.essence_1_TotalTimer.ToString("F2"));
        if (levelStats.essence_2_TotalTimer_Check && levelStats.essence_2_TotalTimer > 0)
            form.AddField(levelField_Path.path_Essence_2_TotalTimer, levelStats.essence_2_TotalTimer.ToString("F2"));
        if (levelStats.essence_3_TotalTimer_Check && levelStats.essence_3_TotalTimer > 0)
            form.AddField(levelField_Path.path_Essence_3_TotalTimer, levelStats.essence_3_TotalTimer.ToString("F2"));
        if (levelStats.essence_4_TotalTimer_Check && levelStats.essence_4_TotalTimer > 0)
            form.AddField(levelField_Path.path_Essence_4_TotalTimer, levelStats.essence_4_TotalTimer.ToString("F2"));
        if (levelStats.essence_5_TotalTimer_Check && levelStats.essence_5_TotalTimer > 0)
            form.AddField(levelField_Path.path_Essence_5_TotalTimer, levelStats.essence_5_TotalTimer.ToString("F2"));
        if (levelStats.essence_6_TotalTimer_Check && levelStats.essence_6_TotalTimer > 0)
            form.AddField(levelField_Path.path_Essence_6_TotalTimer, levelStats.essence_6_TotalTimer.ToString("F2"));
        if (levelStats.essence_7_TotalTimer_Check && levelStats.essence_7_TotalTimer > 0)
            form.AddField(levelField_Path.path_Essence_7_TotalTimer, levelStats.essence_7_TotalTimer.ToString("F2"));
        if (levelStats.essence_8_TotalTimer_Check && levelStats.essence_8_TotalTimer > 0)
            form.AddField(levelField_Path.path_Essence_8_TotalTimer, levelStats.essence_8_TotalTimer.ToString("F2"));
        if (levelStats.essence_9_TotalTimer_Check && levelStats.essence_9_TotalTimer > 0)
            form.AddField(levelField_Path.path_Essence_9_TotalTimer, levelStats.essence_9_TotalTimer.ToString("F2"));
        if (levelStats.essence_10_TotalTimer_Check && levelStats.essence_10_TotalTimer > 0)
            form.AddField(levelField_Path.path_Essence_10_TotalTimer, levelStats.essence_10_TotalTimer.ToString("F2"));

        if (levelStats.footprint_1_TotalTimer_Check && levelStats.footprint_1_TotalTimer > 0)
            form.AddField(levelField_Path.path_Footprint_1_TotalTimer, levelStats.footprint_1_TotalTimer.ToString("F2"));
        if (levelStats.footprint_2_TotalTimer_Check && levelStats.footprint_2_TotalTimer > 0)
            form.AddField(levelField_Path.path_Footprint_2_TotalTimer, levelStats.footprint_2_TotalTimer.ToString("F2"));
        if (levelStats.footprint_3_TotalTimer_Check && levelStats.footprint_3_TotalTimer > 0)
            form.AddField(levelField_Path.path_Footprint_3_TotalTimer, levelStats.footprint_3_TotalTimer.ToString("F2"));

        if (levelStats.skin_TotalTimer_Check && levelStats.skin_TotalTimer > 0)
            form.AddField(levelField_Path.path_Skin_TotalTimer, levelStats.skin_TotalTimer.ToString("F2"));

        if (levelField_Path.path_Ability_1_TotalTimer != "" && levelStats.ability_1_TotalTimer_Check && levelStats.ability_1_TotalTimer > 0)
            form.AddField(levelField_Path.path_Ability_1_TotalTimer, levelStats.ability_1_TotalTimer.ToString("F2"));
        if (levelField_Path.path_Ability_2_TotalTimer != "" && levelStats.ability_2_TotalTimer_Check && levelStats.ability_2_TotalTimer > 0)
            form.AddField(levelField_Path.path_Ability_2_TotalTimer, levelStats.ability_2_TotalTimer.ToString("F2"));
        if (levelField_Path.path_Ability_3_TotalTimer != "" && levelStats.ability_3_TotalTimer_Check && levelStats.ability_3_TotalTimer > 0)
            form.AddField(levelField_Path.path_Ability_3_TotalTimer, levelStats.ability_3_TotalTimer.ToString("F2"));

        using (UnityWebRequest www = UnityWebRequest.Post(formURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                //Debug.Log("Feedback submitted successfully");

                //ResetLevelStats(levelStats);
            }
            else
            {
                //Debug.Log("Error in feedback submission: " + www.error);
            }
        }
    }
}

[Serializable]
public class LevelField_Path
{
    public string path_TimeUsed;
    public string path_StepsTaken;
    public string path_GoalReachedTimer;
    public string path_QuitTimer;
    public string path_RespawnTaken;
    public string path_CameraRotationTaken;
    public string path_FreeCamTimer;

    public string path_Ability_Swim;
    public string path_Ability_SwiftSwim;
    public string path_Ability_Ascend;
    public string path_Ability_Descend;
    public string path_Ability_Dash;
    public string path_Ability_GrapplingHook;
    public string path_Ability_Jump;
    public string path_Ability_CeilingGrab;

    public string path_Essence_1_TotalTimer;
    public string path_Essence_2_TotalTimer;
    public string path_Essence_3_TotalTimer;
    public string path_Essence_4_TotalTimer;
    public string path_Essence_5_TotalTimer;
    public string path_Essence_6_TotalTimer;
    public string path_Essence_7_TotalTimer;
    public string path_Essence_8_TotalTimer;
    public string path_Essence_9_TotalTimer;
    public string path_Essence_10_TotalTimer;

    public string path_Footprint_1_TotalTimer;
    public string path_Footprint_2_TotalTimer;
    public string path_Footprint_3_TotalTimer;

    public string path_Skin_TotalTimer;

    public string path_Ability_1_TotalTimer;
    public string path_Ability_2_TotalTimer;
    public string path_Ability_3_TotalTimer;
}


//--------------------


[Serializable]
public class PlayerStatsData
{
    public Session_Stats sessionStats = new Session_Stats();

    public Level_Stats rivergreenLv1_Stats = new Level_Stats();
    public Level_Stats rivergreenLv2_Stats = new Level_Stats();
    public Level_Stats rivergreenLv3_Stats = new Level_Stats();
    public Level_Stats rivergreenLv4_Stats = new Level_Stats();
    public Level_Stats rivergreenLv5_Stats = new Level_Stats();

    public Level_Stats sandlandsLv1_Stats = new Level_Stats();
    public Level_Stats sandlandsLv2_Stats = new Level_Stats();
    public Level_Stats sandlandsLv3_Stats = new Level_Stats();
    public Level_Stats sandlandsLv4_Stats = new Level_Stats();
    public Level_Stats sandlandsLv5_Stats = new Level_Stats();

    public Level_Stats frostfieldLv1_Stats = new Level_Stats();
    public Level_Stats frostfieldLv2_Stats = new Level_Stats();
    public Level_Stats frostfieldLv3_Stats = new Level_Stats();
    public Level_Stats frostfieldLv4_Stats = new Level_Stats();
    public Level_Stats frostfieldLv5_Stats = new Level_Stats();

    public Level_Stats fireveinLv1_Stats = new Level_Stats();
    public Level_Stats fireveinLv2_Stats = new Level_Stats();
    public Level_Stats fireveinLv3_Stats = new Level_Stats();
    public Level_Stats fireveinLv4_Stats = new Level_Stats();
    public Level_Stats fireveinLv5_Stats = new Level_Stats();

    public Level_Stats witchmireLv1_Stats = new Level_Stats();
    public Level_Stats witchmireLv2_Stats = new Level_Stats();
    public Level_Stats witchmireLv3_Stats = new Level_Stats();
    public Level_Stats witchmireLv4_Stats = new Level_Stats();
    public Level_Stats witchmireLv5_Stats = new Level_Stats();

    public Level_Stats metalworksLv1_Stats = new Level_Stats();
    public Level_Stats metalworksLv2_Stats = new Level_Stats();
    public Level_Stats metalworksLv3_Stats = new Level_Stats();
    public Level_Stats metalworksLv4_Stats = new Level_Stats();
    public Level_Stats metalworksLv5_Stats = new Level_Stats();
}


//--------------------


[Serializable]
public class Session_Stats
{
    [Header("General")]
    public int session_No;
    public float totalTimeUsed;

    [Header("Timers")]
    public float totalTimeUsed_Menus;
    public float totalTimeUsed_MainMenu;
    public float totalTimeUsed_OverworldMenu;
    public float totalTimeUsed_WardrobeMenu;
    public float totalTimeUsed_OptionsMenu;
    public float totalTimeUsed_InPauseMenu;
    public float totalTimeUsed_InLevels;
    public float totalTimeUsed_InFreeCam;

    [Header("Counters")]
    public int totalLevelsVisited;
    public int totalLevelExited;
    public int totalLevelsCleared;
    public int totalStepsTaken;
    public int totalRespawnTaken;
    public int totalCameraRotationTaken;

    [Header("Wardrobe skin wearing timer")]
    public float totalTimeEquippedInLevels_Default;
    public float totalTimeEquippedInLevels_RivergreenLv1;
    public float totalTimeEquippedInLevels_RivergreenLv2;
    public float totalTimeEquippedInLevels_RivergreenLv3;
    public float totalTimeEquippedInLevels_RivergreenLv4;
    public float totalTimeEquippedInLevels_RivergreenLv5;
    public float totalTimeEquippedInLevels_SandlandsLv1;
    public float totalTimeEquippedInLevels_SandlandsLv2;
    public float totalTimeEquippedInLevels_SandlandsLv3;
    public float totalTimeEquippedInLevels_SandlandsLv4;
    public float totalTimeEquippedInLevels_SandlandsLv5;
    public float totalTimeEquippedInLevels_FrostfieldLv1;
    public float totalTimeEquippedInLevels_FrostfieldLv2;
    public float totalTimeEquippedInLevels_FrostfieldLv3;
    public float totalTimeEquippedInLevels_FrostfieldLv4;
    public float totalTimeEquippedInLevels_FrostfieldLv5;
    public float totalTimeEquippedInLevels_FireveinLv1;
    public float totalTimeEquippedInLevels_FireveinLv2;
    public float totalTimeEquippedInLevels_FireveinLv3;
    public float totalTimeEquippedInLevels_FireveinLv4;
    public float totalTimeEquippedInLevels_FireveinLv5;
    public float totalTimeEquippedInLevels_WitchmireLv1;
    public float totalTimeEquippedInLevels_WitchmireLv2;
    public float totalTimeEquippedInLevels_WitchmireLv3;
    public float totalTimeEquippedInLevels_WitchmireLv4;
    public float totalTimeEquippedInLevels_WitchmireLv5;
    public float totalTimeEquippedInLevels_MetalworksLv1;
    public float totalTimeEquippedInLevels_MetalworksLv2;
    public float totalTimeEquippedInLevels_MetalworksLv3;
    public float totalTimeEquippedInLevels_MetalworksLv4;
    public float totalTimeEquippedInLevels_MetalworksLv5;
}

[Serializable]
public class Level_Stats
{
    [Header("General")]
    public float timeUsed;
    public int stepsTaken;
    public float goalReachedTimer;
    public float quitTimer;
    public int respawnTaken;
    public int cameraRotationTaken;
    public float freeCamTimer;

    [Header("Abilities used")]
    public int ability_Swim;
    public int ability_SwiftSwim;
    public int ability_Ascend;
    public int ability_Descend;
    public int ability_Dash;
    public int ability_GrapplingHook;
    public int ability_Jump;
    public int ability_CeilingGrab;

    [Header("Essence Picked Up")]
    public float essence_1_TotalTimer;
    public bool essence_1_TotalTimer_Check;
    public float essence_2_TotalTimer;
    public bool essence_2_TotalTimer_Check;
    public float essence_3_TotalTimer;
    public bool essence_3_TotalTimer_Check;
    public float essence_4_TotalTimer;
    public bool essence_4_TotalTimer_Check;
    public float essence_5_TotalTimer;
    public bool essence_5_TotalTimer_Check;
    public float essence_6_TotalTimer;
    public bool essence_6_TotalTimer_Check;
    public float essence_7_TotalTimer;
    public bool essence_7_TotalTimer_Check;
    public float essence_8_TotalTimer;
    public bool essence_8_TotalTimer_Check;
    public float essence_9_TotalTimer;
    public bool essence_9_TotalTimer_Check;
    public float essence_10_TotalTimer;
    public bool essence_10_TotalTimer_Check;

    [Header("Footprints Picked Up")]
    public float footprint_1_TotalTimer;
    public bool footprint_1_TotalTimer_Check;
    public float footprint_2_TotalTimer;
    public bool footprint_2_TotalTimer_Check;
    public float footprint_3_TotalTimer;
    public bool footprint_3_TotalTimer_Check;

    [Header("Skin Picked Up")]
    public float skin_TotalTimer;
    public bool skin_TotalTimer_Check;

    [Header("Abilities Picked Up")]
    public float ability_1_TotalTimer;
    public bool ability_1_TotalTimer_Check;
    public float ability_2_TotalTimer;
    public bool ability_2_TotalTimer_Check;
    public float ability_3_TotalTimer;
    public bool ability_3_TotalTimer_Check;
}