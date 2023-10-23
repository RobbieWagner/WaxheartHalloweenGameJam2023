using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{

    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    // Start is called before the first frame update
    private void Awake()
    {
        Heart.Instance.OnHealthChanged += UpdateSlider;

        healthSlider.maxValue = Heart.Instance.maxHealth;
        healthSlider.minValue = 0;
        UpdateSlider(Heart.Instance.Health);
    }

    private void UpdateSlider(int health)
    {
        healthSlider.maxValue = Heart.Instance.maxHealth;
        healthSlider.value = health;
        healthText.text = "Calories: " + healthSlider.value; 
    }
}
