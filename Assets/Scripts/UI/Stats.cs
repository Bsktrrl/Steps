using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;

    private void Update()
    {
        coinText.text = "Coin: " + MainManager.Instance.collectables.coin;
    }
}
