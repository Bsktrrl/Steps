using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionCategoryButton : MonoBehaviour
{
    public static event Action Action_CategoryButtonHasBeenSelected;

    [Header("Option Category")]
    public MenuCategories optionCategory;

    GameObject lastSelected = null;


    //--------------------


    void Update()
    {
        ChangeCurrentCategory();
    }

    void ChangeCurrentCategory()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current == gameObject && lastSelected != gameObject)
        {
            MenuManager.Instance.ChangeMenuCategory(optionCategory);
        }

        if (current == null)
            lastSelected = null;
        else
            lastSelected = current;
    }
}
