using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using RobbieWagnerGames.UI;

public class UpgradesMenu : MenuWithTabs
{

    private TowerUpgrader towerUpgrader;
    public TowerUpgrader Upgrader
    {
        get { return towerUpgrader; }
        set
        {
            if(value == towerUpgrader) return;
            towerUpgrader = value;
            OnTowerUpgraderChanged(value);
        } 
    }
    public delegate void OnTowerUpgraderChangedDelegate(TowerUpgrader newUpgrader);
    public event OnTowerUpgraderChangedDelegate OnTowerUpgraderChanged;

    public static UpgradesMenu Instance {get; private set;}

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
    }
}