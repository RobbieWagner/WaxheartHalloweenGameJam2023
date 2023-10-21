using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart: MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int health;
    public int Health
    {
        get { return health; }
        set
        {
            if(value == health) return;
            health = value;
            Debug.Log("heart health: " + health);
            OnHealthChanged?.Invoke(health);
        }
    }
    public delegate void OnHealthChangedDelegate(int newHealth);
    public event OnHealthChangedDelegate OnHealthChanged;

    public static Heart Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    public void Initialize()
    {
        Health = maxHealth;
    }
}