using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuCategorySelected : MonoBehaviour
{
    [Header("SettingState to activate")]
    public SettingState settingState;

    [Header("SettingState to activate")]
    [SerializeField] Image selectedImage;

    [SerializeField] bool isSelected;

    [HideInInspector] public GameObject lastSelected;


    //--------------------


    void OnEnable()
    {
        StartCoroutine(WatchSelection());
    }


    //--------------------


    public IEnumerator WatchSelection()
    {
        while (true)
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;

            if (current != lastSelected)
            {
                if (current == GetComponent<Button>().gameObject)
                {
                    isSelected = true;
                    AddImageColor();
                    ChangeSelectedSettingsButtonSegment();
                }
                else
                {
                    isSelected = false;
                    RemoveImageColor();
                }

                lastSelected = current;
            }

            yield return null; // or yield return new WaitForSeconds(0.1f) for better performance
        }
    }

    void AddImageColor()
    {
        //if (selectedImage)
        //    selectedImage.color = SettingsManager.Instance.activeSettingSegmentColor;
    }
    void RemoveImageColor()
    {
        //if (selectedImage)
        //    selectedImage.color = Color.white;
    }

    void ChangeSelectedSettingsButtonSegment()
    {
        SettingsManager.Instance.settingState = settingState;
    }
}
