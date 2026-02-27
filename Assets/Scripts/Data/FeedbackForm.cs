using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Windows;
using static UnityEngine.Rendering.DebugUI;

public class FeedbackForm : MonoBehaviour
{
    string formURL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSef7zFzWRFbkhTYw5zZDvnoDST2ZNDY3j4FUz9NcJc1uIApxQ/formResponse";

    PlayerStatsData playerStatsData = new PlayerStatsData();

    //--------------------


    private void Start()
    {
        SubmitFeedback();
    }


    //--------------------


    public void SubmitFeedback()
    {
        StartCoroutine(AddSessionField());
    }

    IEnumerator Post(string data1, string data2)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1252827554", data1);
        form.AddField("entry.240663572", data2);

        using (UnityWebRequest www = UnityWebRequest.Post(formURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Feedback submitted successfully");
            }
            else
            {
                Debug.Log("Error in feedback submission: " + www.error);
            }
        }
    }


    //-----


    IEnumerator AddSessionField()
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1252827554", playerStatsData.sessionStats.session_No.ToString());
        form.AddField("entry.240663572", playerStatsData.sessionStats.totalTimeUsed.ToString());

        //form.AddField(, playerStatsData.sessionStats.totalTimeUsed_Menus.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeUsed_MainMenu.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeUsed_OverworldMenu.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeUsed_WardrobeMenu.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeUsed_OptionsMenu.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeUsed_InPauseMenu.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeUsed_InLevels.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeUsed_InFreeCam.ToString());

        //form.AddField(, playerStatsData.sessionStats.totalLevelsVisited.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalLevelExited.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalLevelsCleared.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalStepsTaken.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalRespawnTaken.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalCameraRotationTaken.ToString());

        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_Default.ToString());

        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv1.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv2.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv3.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv4.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_RivergreenLv5.ToString());

        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv1.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv2.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv3.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv4.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_SandlandsLv5.ToString());

        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv1.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv2.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv3.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv4.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_FrostfieldLv5.ToString());

        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv1.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv2.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv3.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv4.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_FireveinLv5.ToString());

        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv1.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv2.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv3.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv4.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_WitchmireLv5.ToString());

        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv1.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv2.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv3.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv4.ToString());
        //form.AddField(, playerStatsData.sessionStats.totalTimeEquippedInLevels_MetalworksLv5.ToString());


             //"entry.1416356909" 
             //"entry.1534190894" 
             //"entry.272428771" 
             //"entry.3208872" 
             //"entry.873769790" 
             //"entry.1186270066" 
             //"entry.1243543042" 
             //"entry.725598216" 
             //"entry.1362508840" 
             //"entry.1284439705" 
             //"entry.1709317079" 
             //"entry.525598876" 
             //"entry.219862598" 
             //"entry.1285447355" 
             //"entry.1206439869" 
             //"entry.1343052800" 
             //"entry.458440323" 
             //"entry.1037629915" 
             //"entry.932052857" 
             //"entry.1721013310" 
             //"entry.1949758081" 
             //"entry.191480969" 
             //"entry.625830122" 
             //"entry.874509393" 
             //"entry.1854135177" 
             //"entry.1347517780" 
             //"entry.1939783691" 
             //"entry.1107810740" 
             //"entry.1727229317" 
             //"entry.606740502" 
             //"entry.1090823188" 
             //"entry.2049915036" 
             //"entry.514067095" 
             //"entry.102125640" 
             //"entry.805850594" 
             //"entry.1027326274" 
             //"entry.607439042" 
             //"entry.1553823692" 
             //"entry.1671659912" 
             //"entry.1261180017" 
             //"entry.1409198516" 
             //"entry.1029303544" 
             //"entry.25719402" 
             //"entry.763513455" 
             //"entry.1183476573" 
             //"entry.1689027362" 
             //"entry.1447210031" 
             //"entry.1079943535" 
             //"entry.1800837785" 
             //"entry.673851242" 
             //"entry.1137183166" 
             //"entry.1581533742" 
             //"entry.1786057615" 
             //"entry.783939847" 
             //"entry.510403430" 
             //"entry.1388112400" 
             //"entry.163477586" 
             //"entry.2105486132" 
             //"entry.797390186" 
             //"entry.944978932" 
             //"entry.714993801" 
             //"entry.569553625" 
             //"entry.729176783" 
             //"entry.161518432" 
             //"entry.906810834" 
             //"entry.2081527921" 
             //"entry.1435207882" 
             //"entry.899606085" 
             //"entry.818149302" 
             //"entry.522533854" 
             //"entry.542219994" 
             //"entry.854207696" 
             //"entry.1109714976" 
             //"entry.1897850417" 
             //"entry.1088333264" 
             //"entry.2093440614" 
             //"entry.527504893" 
             //"entry.927557475" 
             //"entry.1595397881" 
             //"entry.1203258864" 
             //"entry.510614793" 
             //"entry.1626717733" 
             //"entry.1565648219" 
             //"entry.1503489575" 
             //"entry.1672437585" 
             //"entry.1383982329" 
             //"entry.187253854" 
             //"entry.235869041" 
             //"entry.576474447" 
             //"entry.1043475143" 
             //"entry.1297067320" 
             //"entry.2057452031" 
             //"entry.1505756054" 
             //"entry.1897780007" 
             //"entry.2096823712" 
             //"entry.275051765" 
             //"entry.268449402" 
             //"entry.2119759627" 
             //"entry.730390048" 
             //"entry.1202067956" 
             //"entry.50670729" 
             //"entry.1026749301" 
             //"entry.1150302289" 
             //"entry.825571877" 
             //"entry.466155704" 
             //"entry.559274352" 
             //"entry.596119727" 
             //"entry.1606359731" 
             //"entry.1435294474" 
             //"entry.1805041080" 
             //"entry.1971295432" 
             //"entry.1546842576" 
             //"entry.1498575380" 
             //"entry.1542126223" 
             //"entry.1331759746" 
             //"entry.1179669584" 
             //"entry.879218144" 
             //"entry.1428008737" 
             //"entry.1561051362" 
             //"entry.1751490934" 
             //"entry.768944102" 
             //"entry.994776623" 
             //"entry.136927272" 
             //"entry.1206912292" 
             //"entry.323915966" 
             //"entry.1872221886" 
             //"entry.445975497" 
             //"entry.1010395670" 
             //"entry.1077501998" 
             //"entry.1737273662" 
             //"entry.1979161116" 
             //"entry.1492861307" 
             //"entry.757086161" 
             //"entry.1805290995" 
             //"entry.1923175399" 
             //"entry.275423907" 
             //"entry.649250298" 
             //"entry.1193692580" 
             //"entry.933108437" 
             //"entry.1547414153" 
             //"entry.983319959" 
             //"entry.1575535652" 
             //"entry.2060120279" 
             //"entry.1006954835" 
             //"entry.344117007" 
             //"entry.126342421" 
             //"entry.1095432506" 
             //"entry.1947711761" 
             //"entry.1413944675" 
             //"entry.248776292" 
             //"entry.2133688322" 
             //"entry.1238274464" 
             //"entry.770852460" 
             //"entry.1860182025" 
             //"entry.1472287236" 
             //"entry.668919846" 
             //"entry.950789740" 
             //"entry.1214064682" 
             //"entry.915586860" 
             //"entry.1046609318" 
             //"entry.1701349048" 
             //"entry.1842316613" 
             //"entry.1571262443" 
             //"entry.1985868633" 
             //"entry.1273754573" 
             //"entry.461978382" 
             //"entry.2073140631" 
             //"entry.1203395836" 
             //"entry.371346564" 
             //"entry.23455516" 
             //"entry.1875506135" 
             //"entry.700113969" 
             //"entry.1026559115" 
             //"entry.1631291945" 
             //"entry.1060554516" 
             //"entry.564972786" 
             //"entry.540137486" 
             //"entry.1308795690" 
             //"entry.1011681284" 
             //"entry.1144511555" 
             //"entry.122965579" 
             //"entry.1232881714" 
             //"entry.1427339208" 
             //"entry.104613767" 
             //"entry.1476421917" 
             //"entry.576998539" 
             //"entry.1417363526" 
             //"entry.1067598188" 
             //"entry.1601604851" 
             //"entry.272920332" 
             //"entry.685771594" 
             //"entry.1619149750" 
             //"entry.1093857787" 
             //"entry.2135948752" 
             //"entry.1797012498" 
             //"entry.1860879052" 
             //"entry.328028451" 
             //"entry.906472657" 
             //"entry.153295770" 
             //"entry.1005533849" 
             //"entry.752660038" 
             //"entry.1588009662" 
             //"entry.466281041" 
             //"entry.411648931" 
             //"entry.1068036485" 
             //"entry.1453263511" 
             //"entry.1602194220" 
             //"entry.164266244" 
             //"entry.1016073754" 
             //"entry.1578447882" 
             //"entry.740175828" 
             //"entry.2111035183" 
             //"entry.198798359" 
             //"entry.437694579" 
             //"entry.614367364" 
             //"entry.1324232639" 
             //"entry.500724790" 
             //"entry.809741683" 
             //"entry.825515295" 
             //"entry.194850051" 
             //"entry.769166825" 
             //"entry.1726089551" 
             //"entry.596172869" 
             //"entry.1445030644" 
             //"entry.1389843543" 
             //"entry.751626353" 
             //"entry.817473230" 
             //"entry.1133612008" 
             //"entry.2101874603" 
             //"entry.1043027523" 
             //"entry.793679318" 
             //"entry.722897019" 
             //"entry.2070783901" 
             //"entry.420437198" 
             //"entry.820198592" 
             //"entry.549147944" 
             //"entry.2053548788" 
             //"entry.1363399488" 
             //"entry.1772079941" 
             //"entry.1129490951" 
             //"entry.665279790" 
             //"entry.1067535536" 
             //"entry.1608070788" 
             //"entry.920932666" 
             //"entry.1936905464" 
             //"entry.110402058" 
             //"entry.615651540" 
             //"entry.194692378" 
             //"entry.1674033535" 
             //"entry.628420370" 
             //"entry.785186690" 
             //"entry.252038179" 
             //"entry.2135458203" 
             //"entry.550300473" 
             //"entry.1981720400" 
             //"entry.618366787" 
             //"entry.1289004178" 
             //"entry.1794445566" 
             //"entry.39168325" 
             //"entry.807501438" 
             //"entry.818146076" 
             //"entry.611547687" 
             //"entry.631067036" 
             //"entry.529670627" 
             //"entry.1914295352" 
             //"entry.1519016928" 
             //"entry.1066071305" 
             //"entry.1638781773" 
             //"entry.969978031" 
             //"entry.1446192146" 
             //"entry.979582315" 
             //"entry.1995699792" 
             //"entry.547737253" 
             //"entry.1225577154" 
             //"entry.1174609356" 
             //"entry.1373706884" 
             //"entry.464168259" 
             //"entry.1583514361" 
             //"entry.1269988152" 
             //"entry.1775826930" 
             //"entry.12846827" 
             //"entry.127729704" 
             //"entry.529059223" 
             //"entry.2076015642" 
             //"entry.197159773" 
             //"entry.1602642850" 
             //"entry.343272426" 
             //"entry.1473814027" 
             //"entry.1479679149" 
             //"entry.247964947" 
             //"entry.32465777" 
             //"entry.911493409" 
             //"entry.799727503" 
             //"entry.1845396580" 
             //"entry.835208999" 
             //"entry.818631208" 
             //"entry.179441642" 
             //"entry.1515648813" 
             //"entry.1185665538" 
             //"entry.1236928533" 
             //"entry.655162285" 
             //"entry.280741510" 
             //"entry.166386201" 
             //"entry.1879441342" 
             //"entry.1045933020" 
             //"entry.467569789" 
             //"entry.898942101" 
             //"entry.2145905309" 
             //"entry.582694200" 
             //"entry.1854895827" 
             //"entry.1471978725" 
             //"entry.1756593123" 
             //"entry.758699846" 
             //"entry.1993126247" 
             //"entry.304691139" 
             //"entry.643080151" 
             //"entry.1789634768" 
             //"entry.136742916" 
             //"entry.2135922117" 
             //"entry.70287636" 
             //"entry.782793433" 
             //"entry.1149714976" 
             //"entry.1468109886" 
             //"entry.40836679" 
             //"entry.2001796393" 
             //"entry.234665284" 
             //"entry.1482981039" 
             //"entry.1245176785" 
             //"entry.811900800" 
             //"entry.894653024" 
             //"entry.329947241" 
             //"entry.1301673876" 
             //"entry.145131470" 
             //"entry.1751208741" 
             //"entry.2141146827" 
             //"entry.1048701262" 
             //"entry.67279346" 
             //"entry.1133116571" 
             //"entry.1177335198" 
             //"entry.601375519" 
             //"entry.1045962422" 
             //"entry.438913308" 
             //"entry.1600999564" 
             //"entry.1046573106" 
             //"entry.1782670067" 
             //"entry.1120606404" 
             //"entry.405458399" 
             //"entry.1542557980" 
             //"entry.930705920" 
             //"entry.2097580985" 
             //"entry.151313092" 
             //"entry.632847272" 
             //"entry.1339051844" 
             //"entry.666571304" 
             //"entry.1009741074" 
             //"entry.1690355685" 
             //"entry.463408050" 
             //"entry.275293268" 
             //"entry.80993002" 
             //"entry.1734329391" 
             //"entry.90730224" 
             //"entry.838200930" 
             //"entry.1493812378" 
             //"entry.220379243" 
             //"entry.1523962122" 
             //"entry.1268512342" 
             //"entry.903230777" 
             //"entry.998455670" 
             //"entry.100452763" 
             //"entry.199020175" 
             //"entry.1632519640" 
             //"entry.200117610" 
             //"entry.391458866" 
             //"entry.1242097446" 
             //"entry.795560656" 
             //"entry.334097868" 
             //"entry.1656132678" 
             //"entry.1138585789" 
             //"entry.931618437" 
             //"entry.366953354" 
             //"entry.1394294158" 
             //"entry.1514679250" 
             //"entry.1168235485" 
             //"entry.1748651282" 
             //"entry.2002844332" 
             //"entry.664460769" 
             //"entry.193361129" 
             //"entry.1328686670" 
             //"entry.470683773" 
             //"entry.1469192932" 
             //"entry.1961964752" 
             //"entry.1397302048" 
             //"entry.302077133" 
             //"entry.2064622083" 
             //"entry.1744673781" 
             //"entry.2033677597" 
             //"entry.869348736" 
             //"entry.670266175" 
             //"entry.669768321" 
             //"entry.299500322" 
             //"entry.147049481" 
             //"entry.2141316505" 
             //"entry.1391651236" 
             //"entry.439749893" 
             //"entry.132892148" 
             //"entry.898307188" 
             //"entry.1424230259" 
             //"entry.569836201" 
             //"entry.1352321821" 
             //"entry.893392982" 
             //"entry.153710906" 
             //"entry.184282263" 
             //"entry.135069280" 
             //"entry.1910919553" 
             //"entry.1178039421" 
             //"entry.628584658" 
             //"entry.1692286478" 
             //"entry.1225706947" 
             //"entry.405574956" 
             //"entry.2116232841" 
             //"entry.1200941385" 
             //"entry.634614601" 
             //"entry.160711719" 
             //"entry.196756014" 
             //"entry.1185014591" 
             //"entry.550706497" 
             //"entry.967415610" 
             //"entry.1615645504" 
             //"entry.515277132" 
             //"entry.792638882" 
             //"entry.820237911" 
             //"entry.2016667411" 
             //"entry.812743071" 
             //"entry.2061346064" 
             //"entry.1272893847" 
             //"entry.1850049103" 
             //"entry.901707548" 
             //"entry.2112542236" 
             //"entry.88103252" 
             //"entry.1659390858" 
             //"entry.211805916" 
             //"entry.1206504661" 
             //"entry.213283244" 
             //"entry.46825809" 
             //"entry.382082603" 
             //"entry.611282152" 
             //"entry.1016226311" 
             //"entry.2140662923" 
             //"entry.751051150" 
             //"entry.1465290997" 
             //"entry.1207626956" 
             //"entry.532590451" 
             //"entry.1101164603" 
             //"entry.1861162011" 
             //"entry.1717135888" 
             //"entry.324271096" 
             //"entry.1027814637" 
             //"entry.839281142" 
             //"entry.178655960" 
             //"entry.1249842819" 
             //"entry.112937499" 
             //"entry.1438215693" 
             //"entry.722286754" 
             //"entry.1578130748" 
             //"entry.1312616077" 
             //"entry.1490465456" 
             //"entry.1642141856" 
             //"entry.1248512523" 
             //"entry.1781776020" 
             //"entry.765017777" 
             //"entry.829933367" 
             //"entry.1805644825" 
             //"entry.1133269269" 
             //"entry.2050714085" 
             //"entry.637494335" 
             //"entry.996990987" 
             //"entry.287055497" 
             //"entry.1714614215" 
             //"entry.1537736853" 
             //"entry.1204227727" 
             //"entry.718381495" 
             //"entry.939492391" 
             //"entry.1635569722" 
             //"entry.907887759" 
             //"entry.1755461473" 
             //"entry.292860089" 
             //"entry.2139608995" 
             //"entry.513474481" 
             //"entry.1797589521" 
             //"entry.1110933731" 
             //"entry.730103024" 
             //"entry.367517019" 
             //"entry.1913941807" 
             //"entry.1471725734" 
             //"entry.116242955" 
             //"entry.1746937484" 
             //"entry.835647461" 
             //"entry.1739899260" 
             //"entry.1374254680" 
             //"entry.1587169937" 
             //"entry.622708344" 
             //"entry.752897889" 
             //"entry.735301699" 
             //"entry.831012364" 
             //"entry.313139522" 
             //"entry.1482907896" 
             //"entry.41367433" 
             //"entry.1851014937" 
             //"entry.1228677399" 
             //"entry.1290946158" 
             //"entry.257538732" 
             //"entry.1263373835" 
             //"entry.1615567106" 
             //"entry.1738407250" 
             //"entry.237176134" 
             //"entry.1907360483" 
             //"entry.404337573" 
             //"entry.1217427602" 
             //"entry.977228816" 
             //"entry.743280415" 
             //"entry.1649388433" 
             //"entry.940373262" 
             //"entry.1916621530" 
             //"entry.1813923506" 
             //"entry.97410660" 
             //"entry.1336360137" 
             //"entry.221847335" 
             //"entry.428092644" 
             //"entry.333792651" 
             //"entry.2129288641" 
             //"entry.934976507" 
             //"entry.1201967645" 
             //"entry.1912012418" 
             //"entry.103889961" 
             //"entry.1974001866" 
             //"entry.289677234" 
             //"entry.1149835986" 
             //"entry.510581522" 
             //"entry.1624449882" 
             //"entry.136375604" 
             //"entry.1696488095" 
             //"entry.662390932" 
             //"entry.386084586" 
             //"entry.289214544" 
             //"entry.492809017" 
             //"entry.1406294331" 
             //"entry.1515522500" 
             //"entry.1712680414" 
             //"entry.1566459062" 
             //"entry.1783169157" 
             //"entry.2039341410" 
             //"entry.944928498" 
             //"entry.1470022627" 
             //"entry.1416578712" 
             //"entry.820341965" 
             //"entry.170907253" 
             //"entry.2113057432" 
             //"entry.555062662" 
             //"entry.1482477250" 
             //"entry.2127956620" 
             //"entry.1051476429" 
             //"entry.2016027112" 
             //"entry.2142985026" 
             //"entry.6086307" 
             //"entry.1810949701" 
             //"entry.16205099" 
             //"entry.904191115" 
             //"entry.84468034" 
             //"entry.1123828619" 
             //"entry.1508307633" 
             //"entry.1361162105" 
             //"entry.625339902" 
             //"entry.1080276251" 
             //"entry.2040601275" 
             //"entry.1652016365" 
             //"entry.1642789847" 
             //"entry.210142837" 
             //"entry.896037361" 
             //"entry.1249759186" 
             //"entry.484799627" 
             //"entry.1515929768" 
             //"entry.1824912923" 
             //"entry.237753115" 
             //"entry.2054931401" 
             //"entry.1247067271" 
             //"entry.1438853862" 
             //"entry.606177498" 
             //"entry.1943766013" 
             //"entry.7584043" 
             //"entry.230432348" 
             //"entry.1147986440" 
             //"entry.1763676456" 
             //"entry.342035863" 
             //"entry.1110712415" 
             //"entry.225097744" 
             //"entry.715806307" 
             //"entry.2091907857" 
             //"entry.442396274" 
             //"entry.333112006" 
             //"entry.7139675" 
             //"entry.1949146149" 
             //"entry.252865499" 
             //"entry.1414348668" 
             //"entry.339300817" 
             //"entry.794180056" 
             //"entry.910442783" 
             //"entry.1757084609" 
             //"entry.1148739438" 
             //"entry.207645958" 
             //"entry.785423496" 
             //"entry.1225913823" 
             //"entry.1997050416" 
             //"entry.720537942" 
             //"entry.91106040" 
             //"entry.2146459159" 
             //"entry.1212535265" 
             //"entry.525765730" 
             //"entry.2015059906" 
             //"entry.614042246" 
             //"entry.1829169485" 
             //"entry.1344320249" 
             //"entry.295720216" 
             //"entry.1189446630" 
             //"entry.906404462" 
             //"entry.30402816" 
             //"entry.1768031357" 
             //"entry.649730395" 
             //"entry.820707021" 
             //"entry.171082407" 
             //"entry.1850612078" 
             //"entry.1275496795" 
             //"entry.474227084" 
             //"entry.1638828285" 
             //"entry.701983082" 
             //"entry.635296792" 
             //"entry.1690718411" 
             //"entry.105821626" 
             //"entry.1533745654" 
             //"entry.735999586" 
             //"entry.1961287475" 
             //"entry.1838881601" 
             //"entry.968704919" 
             //"entry.1620714456" 
             //"entry.980376264" 
             //"entry.2094575621" 
             //"entry.460623701" 
             //"entry.885049176" 
             //"entry.2079596943" 
             //"entry.1210320064" 
             //"entry.1833132803" 
             //"entry.1025884141" 
             //"entry.1500925986" 
             //"entry.2107041701" 
             //"entry.669551322" 
             //"entry.954819733" 
             //"entry.1114125601" 
             //"entry.584420386" 
             //"entry.1331832835" 
             //"entry.548028074" 
             //"entry.2037810981" 
             //"entry.594842053" 
             //"entry.517683767" 
             //"entry.119298037" 
             //"entry.1626338215" 
             //"entry.2007168963" 
             //"entry.2017427118" 
             //"entry.1892872130" 
             //"entry.16165934" 
             //"entry.4992638" 
             //"entry.2012636330" 
             //"entry.44062202" 
             //"entry.1243751256" 
             //"entry.270232068" 
             //"entry.1616097799" 
             //"entry.1392081099" 
             //"entry.1539877555" 
             //"entry.719679497" 
             //"entry.302909751" 
             //"entry.422035324" 
             //"entry.86014986" 
             //"entry.492442475" 
             //"entry.1010006346" 
             //"entry.869457866" 
             //"entry.129344366" 
             //"entry.832717242" 
             //"entry.988794436" 
             //"entry.1459571800" 
             //"entry.2083817522" 
             //"entry.164981346" 
             //"entry.1340631398" 
             //"entry.1209691646" 
             //"entry.292389904" 
             //"entry.2068540433" 
             //"entry.1621323533" 
             //"entry.1042345204" 
             //"entry.1001942836" 
             //"entry.1050509467" 
             //"entry.1979708914" 
             //"entry.749320912" 
             //"entry.277442405" 
             //"entry.1447368669" 
             //"entry.2014466198" 
             //"entry.329276482" 
             //"entry.1825209564" 
             //"entry.1613832892" 
             //"entry.901173653" 
             //"entry.1132349341" 
             //"entry.117032358" 
             //"entry.500254173" 
             //"entry.343754038" 
             //"entry.1295967888" 
             //"entry.1460517231" 
             //"entry.1390622554" 
             //"entry.393264357" 
             //"entry.1156270719" 
             //"entry.696377638" 
             //"entry.755323409" 
             //"entry.1059497781" 
             //"entry.741130433" 
             //"entry.329777138" 
             //"entry.1129654512" 
             //"entry.1499899121" 
             //"entry.160028178" 
             //"entry.2053570858" 
             //"entry.931582105" 
             //"entry.1853484435" 
             //"entry.617875528" 
             //"entry.2130424340" 
             //"entry.977545805" 
             //"entry.1644605238" 
             //"entry.809069150" 
             //"entry.303285739" 
             //"entry.1749899712" 
             //"entry.1504608970" 
             //"entry.1566319793" 
             //"entry.397631574" 
             //"entry.503331762" 
             //"entry.915814940" 
             //"entry.129935334" 
             //"entry.1154496417" 
             //"entry.1255056110" 
             //"entry.331192495" 
             //"entry.332771044" 
             //"entry.2106718733" 
             //"entry.1563037050" 
             //"entry.1533016345" 
             //"entry.1604724052" 
             //"entry.871067896" 
             //"entry.1210359052" 
             //"entry.505232719" 
             //"entry.871496147" 
             //"entry.820642350" 
             //"entry.203343284" 
             //"entry.991284328" 
             //"entry.451380599" 
             //"entry.885547182" 
             //"entry.852723678" 
             //"entry.554764843" 
             //"entry.913884604" 
             //"entry.1682339537" 
             //"entry.24894706" 
             //"entry.1938664671" 
             //"entry.772995408" 
             //"entry.1241388345" 
             //"entry.89506874" 
             //"entry.1626116254" 
             //"entry.535771782" 
             //"entry.1035179126" 
             //"entry.1390288206" 
             //"entry.906763614" 
             //"entry.1216382827" 
             //"entry.250054342" 
             //"entry.269636418" 
             //"entry.798999587" 
             //"entry.1177623743" 
             //"entry.566079610" 
             //"entry.1926889257" 
             //"entry.488352781" 
             //"entry.677485517" 
             //"entry.128999983" 
             //"entry.1802311999" 
             //"entry.1744821439" 
             //"entry.1065892262" 
             //"entry.1209964624" 
             //"entry.688641365" 
             //"entry.1615024408" 
             //"entry.349021180" 
             //"entry.1630927590" 
             //"entry.448470941" 
             //"entry.1003279933" 
             //"entry.427623758" 
             //"entry.1706167852" 
             //"entry.1979287675" 
             //"entry.2123497026" 
             //"entry.558774139" 
             //"entry.2058334635" 
             //"entry.1614576355" 
             //"entry.2127386204" 
             //"entry.1974080110" 
             //"entry.1601450992" 
             //"entry.1051179991" 
             //"entry.682503614" 
             //"entry.543824498" 
             //"entry.1703588935" 
             //"entry.1411211329" 
             //"entry.311335727" 
             //"entry.494977820" 
             //"entry.1686372292" 
             //"entry.1202879566" 
             //"entry.904508859" 
             //"entry.1988872164" 
             //"entry.1483668795" 
             //"entry.1618275337" 
             //"entry.417157494" 
             //"entry.1785644380" 
             //"entry.2080416329" 
             //"entry.2017658296" 
             //"entry.1726940236" 
             //"entry.1522782511" 
             //"entry.2088384616" 
             //"entry.458438021" 
             //"entry.1632025202" 
             //"entry.2094551086" 
             //"entry.2065985685" 
             //"entry.612001541" 
             //"entry.921571924" 
             //"entry.1658613932" 
             //"entry.1379899784" 
             //"entry.317834758" 
             //"entry.376780371" 
             //"entry.1190808749" 
             //"entry.1146119283" 
             //"entry.1876112761" 
             //"entry.118473551" 
             //"entry.1971475946" 
             //"entry.107774336" 
             //"entry.826717935" 
             //"entry.439616824" 
             //"entry.1539573808" 
             //"entry.1717348642" 
             //"entry.752068906" 
             //"entry.129966294" 
             //"entry.37522286" 
             //"entry.1611447717" 
             //"entry.1952908224" 
             //"entry.1997666427" 
             //"entry.1181181544" 
             //"entry.898193654" 
             //"entry.53215808" 
             //"entry.1529758378" 
             //"entry.1954427889" 
             //"entry.1473016987" 
             //"entry.175866023" 
             //"entry.534327506" 
             //"entry.1548548552" 
             //"entry.1658582249" 
             //"entry.837048953" 
             //"entry.1455839908" 
             //"entry.1538114691" 
             //"entry.1536496700" 
             //"entry.2135633433" 
             //"entry.2009909901" 
             //"entry.2106785600" 
             //"entry.1422256166" 
             //"entry.762853399" 
             //"entry.1545605370" 
             //"entry.79069586" 
             //"entry.193877732" 
             //"entry.978092140" 
             //"entry.70106875" 
             //"entry.852870859" 
             //"entry.1007273169" 
             //"entry.1242535366" 
             //"entry.126465721" 
             //"entry.1369043111" 
             //"entry.1017959069" 
             //"entry.1873904919" 
             //"entry.905553874" 
             //"entry.1941874924" 
             //"entry.1429360510" 
             //"entry.1086877004" 
             //"entry.590833341" 
             //"entry.974016394" 
             //"entry.51736351" 
             //"entry.503826065" 
             //"entry.451715581" 
             //"entry.1461393004" 
             //"entry.1137405475" 
             //"entry.1328123723" 
             //"entry.1705616597" 
             //"entry.1029755335" 
             //"entry.167867341" 
             //"entry.489661912" 
             //"entry.1347820051" 
             //"entry.2040611723" 
             //"entry.1901735696" 
             //"entry.1691682079" 
             //"entry.1416262531" 
             //"entry.982111026" 
             //"entry.1332663128" 
             //"entry.1122100320" 
             //"entry.1339314512" 
             //"entry.441663177" 
             //"entry.316234226" 
             //"entry.604958710" 
             //"entry.1159486829" 
             //"entry.1775012158" 
             //"entry.1559244198" 
             //"entry.292093209" 
             //"entry.1551404430" 
             //"entry.221250144" 
             //"entry.48188880" 
             //"entry.141344187" 
             //"entry.410542941" 
             //"entry.717990592" 
             //"entry.2138740491" 
             //"entry.2121582633" 
             //"entry.606856664" 
             //"entry.883277101" 
             //"entry.1607777529" 
             //"entry.2140136645" 
             //"entry.340049011" 
             //"entry.1609065989" 
             //"entry.1216402461" 
             //"entry.867325324" 
             //"entry.498774808" 
             //"entry.2025610923" 
             //"entry.1564132972" 
             //"entry.536175636" 
             //"entry.2019567326" 
             //"entry.626249548" 
             //"entry.1615461177" 
             //"entry.1257708341" 
             //"entry.477220870" 
             //"entry.290975906" 
             //"entry.1220582628" 
             //"entry.368368858" 
             //"entry.1390534948" 
             //"entry.483202784" 
             //"entry.96210917" 
             //"entry.1948997635" 
             //"entry.1139877466" 
             //"entry.855389617" 
             //"dlut" value = "1772145592327" >

























        using (UnityWebRequest www = UnityWebRequest.Post(formURL, form))
        {
            yield return www.SendWebRequest();
        }
    }


    //-----


    public void AddRivergreenLv1_Field()
    {

    }
    public void AddRivergreenLv2_Field()
    {

    }
    public void AddRivergreenLv3_Field()
    {

    }
    public void AddRivergreenLv4_Field()
    {

    }
    public void AddRivergreenLv5_Field()
    {

    }


    //-----


    public void AddSandlandsLv1_Field()
    {

    }
    public void AddSandlandsLv2_Field()
    {

    }
    public void AddSandlandsLv3_Field()
    {

    }
    public void AddSandlandsLv4_Field()
    {

    }
    public void AddSandlandsLv5_Field()
    {

    }


    //-----


    public void AddFrostfieldLv1_Field()
    {

    }
    public void AddFrostfieldLv2_Field()
    {

    }
    public void AddFrostfieldLv3_Field()
    {

    }
    public void AddFrostfieldLv4_Field()
    {

    }
    public void AddFrostfieldLv5_Field()
    {

    }


    //-----


    public void AddFireveinLv1_Field()
    {

    }
    public void AddFireveinLv2_Field()
    {

    }
    public void AddFireveinLv3_Field()
    {

    }
    public void AddFireveinLv4_Field()
    {

    }
    public void AddFireveinLv5_Field()
    {

    }


    //-----


    public void AddWitchmireLv1_Field()
    {

    }
    public void AddWitchmireLv2_Field()
    {

    }
    public void AddWitchmireLv3_Field()
    {

    }
    public void AddWitchmireLv4_Field()
    {

    }
    public void AddWitchmireLv5_Field()
    {

    }


    //-----


    public void AddMetalworksLv1_Field()
    {

    }
    public void AddMetalworksLv2_Field()
    {

    }
    public void AddMetalworksLv3_Field()
    {

    }
    public void AddMetalworksLv4_Field()
    {

    }
    public void AddMetalworksLv5_Field()
    {

    }
}

[Serializable]
public class PlayerStatsData
{
    public Session_Stats sessionStats;

    public Level_Stats rivergreenLv1_Stats;
    public Level_Stats rivergreenLv2_Stats;
    public Level_Stats rivergreenLv3_Stats;
    public Level_Stats rivergreenLv4_Stats;
    public Level_Stats rivergreenLv5_Stats;

    public Level_Stats sandlandsLv1_Stats;
    public Level_Stats sandlandsLv2_Stats;
    public Level_Stats sandlandsLv3_Stats;
    public Level_Stats sandlandsLv4_Stats;
    public Level_Stats sandlandsLv5_Stats;

    public Level_Stats frostfieldLv1_Stats;
    public Level_Stats frostfieldLv2_Stats;
    public Level_Stats frostfieldLv3_Stats;
    public Level_Stats frostfieldLv4_Stats;
    public Level_Stats frostfieldLv5_Stats;

    public Level_Stats fireveinLv1_Stats;
    public Level_Stats fireveinLv2_Stats;
    public Level_Stats fireveinLv3_Stats;
    public Level_Stats fireveinLv4_Stats;
    public Level_Stats fireveinLv5_Stats;

    public Level_Stats witchmireLv1_Stats;
    public Level_Stats witchmireLv2_Stats;
    public Level_Stats witchmireLv3_Stats;
    public Level_Stats witchmireLv4_Stats;
    public Level_Stats witchmireLv5_Stats;

    public Level_Stats metalworksLv1_Stats;
    public Level_Stats metalworksLv2_Stats;
    public Level_Stats metalworksLv3_Stats;
    public Level_Stats metalworksLv4_Stats;
    public Level_Stats metalworksLv5_Stats;
}


//--------------------


[Serializable]
public class Session_Stats
{
    public int session_No;
    public float totalTimeUsed;

    public float totalTimeUsed_Menus;
    public float totalTimeUsed_MainMenu;
    public float totalTimeUsed_OverworldMenu;
    public float totalTimeUsed_WardrobeMenu;
    public float totalTimeUsed_OptionsMenu;
    public float totalTimeUsed_InPauseMenu;
    public float totalTimeUsed_InLevels;
    public float totalTimeUsed_InFreeCam;

    public int totalLevelsVisited;
    public int totalLevelExited;
    public int totalLevelsCleared;
    public int totalStepsTaken;
    public int totalRespawnTaken;
    public int totalCameraRotationTaken;

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
    public float timeUsed;
    public int stepsTaken;
    public float goalReachedTimer;
    public float QuitTimer;
    public float respawnTimer;
    public int cameraRotationCounter;
    public float freeCamTimer;

    public int ability_Swim;
    public int ability_SwiftSwim;
    public int ability_Ascend;
    public int ability_Descend;
    public int ability_Dash;
    public int ability_GrapplingHook;
    public int ability_Jump;
    public int ability_CeilingGrab;

    public float essence_1_TotalTimer;
    public float essence_2_TotalTimer;
    public float essence_3_TotalTimer;
    public float essence_4_TotalTimer;
    public float essence_5_TotalTimer;
    public float essence_6_TotalTimer;
    public float essence_7_TotalTimer;
    public float essence_8_TotalTimer;
    public float essence_9_TotalTimer;
    public float essence_10_TotalTimer;

    public float footprint_1_TotalTimer;
    public float footprint_2_TotalTimer;
    public float footprint_3_TotalTimer;

    public float skin_TotalTimer;

    public float ability_1_TotalTimer;
    public float ability_2_TotalTimer;
    public float ability_3_TotalTimer;
}