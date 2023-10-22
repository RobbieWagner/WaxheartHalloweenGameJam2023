using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private Slider slider;
    [SerializeField] private int maxValue = 25;

    private void Awake()
    {
       GameManager.Instance.OnCurrencyChanged += UpdateCurrencyUI; 
       UpdateCurrencyUI(GameManager.Instance.Currency);

       slider.maxValue = maxValue;
       slider.minValue = 0;
       UpdateCurrencyUI(GameManager.Instance.Currency);
    }

    private void UpdateCurrencyUI(int currency)
    {
        slider.value = currency;
        currencyText.text = "Proteins: " + currency;
    }
}
