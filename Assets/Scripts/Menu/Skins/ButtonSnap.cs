using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSnap : MonoBehaviour
{
    [SerializeField] bool isHeaderSnap;

    [SerializeField] bool isSelected;


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

            if (current == GetComponent<Button>().gameObject)
            {
                isSelected = true;

                if (isHeaderSnap)
                {
                    SkinShopManager.Instance.UpdateSnapHeader(current);
                }
                else
                {
                    SkinShopManager.Instance.UpdateSnapBack(current);
                }
            }
            else
            {
                isSelected = false;
            }

            yield return new WaitForSeconds(0.1f); // or yield return new WaitForSeconds(0.1f) for better performance
        }
    }
}
