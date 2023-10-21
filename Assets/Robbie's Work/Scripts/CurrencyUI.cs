using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;

    private void Awake()
    {
       GameManager.Instance.OnCurrencyChanged += UpdateCurrencyUI; 
       UpdateCurrencyUI(GameManager.Instance.Currency);
    }

    private void UpdateCurrencyUI(int currency)
    {
        currencyText.text = "Currency: " + currency;
    }
}
