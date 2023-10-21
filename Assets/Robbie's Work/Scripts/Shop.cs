using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobbieWagnerGames.UI;
using UnityEngine.UI;
using TMPro;

public class Shop : MenuWithTabs
{
    [SerializeField] private TextMeshProUGUI enemiesText;

    public static Shop Instance {get; private set;}

    protected override void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 

        base.Awake();
        Debug.Log("subscribed");
        GameManager.Instance.OnEnemyListUpdated += UpdateEnemyCount;
    }

    private void UpdateEnemyCount(List<TDEnemy> enemies)
    {
        Debug.Log("invoked");
        enemiesText.text = "Enemies Left: " + enemies.Count;
    }
}
