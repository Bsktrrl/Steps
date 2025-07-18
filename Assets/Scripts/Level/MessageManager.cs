using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : Singleton<MessageManager>
{
    [Header("Interact")]
    public List<string> interact_Talk_Message = new List<string>();
    public List<string> interact_Push_Message = new List<string>();


    //--------------------


    public string Show_Message(List<string> stringList)
    {
        return stringList[(int)SettingsMenu.Instance.settingsData.currentLanguage];
    }
}
