using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDisplay : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] Image abilityImage;
    public TextMeshProUGUI abilityName;


    //--------------------


    public void SetupAbilityDisplay(Sprite _buttonSprite, Sprite _abilitySprite, string _abilityName)
    {
        buttonImage.sprite = _buttonSprite;
        abilityImage.sprite = _abilitySprite;
        abilityName.text = _abilityName;
    }


    //--------------------


    public void DestroyPrefab()
    {
        Destroy(gameObject);
    }
}
