using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobbieWagnerGames.UI;
using RobbieWagnerGames;
using GameJam.Towers; 
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SellTab : MenuTab
{
    TowerUpgrader currentUpgrader;
    public GenericTowerBehaviour tower;
    private TowerInfo towerInfo;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI currencyText;

    bool tabIsBuilt = false;

    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void Update()
    {
        if(Shop.Instance.tower != null && !tabIsBuilt)
        {
            currentUpgrader = Shop.Instance.tower;
            tower = currentUpgrader.tower;
            BuildTab();
            tabIsBuilt = true;
        }
        else if(Shop.Instance.tower == null)
        {
            tabIsBuilt = false;
        }
    }

    public override void BuildTab()
    {
        base.BuildTab();
        //Debug.Log("build tab");
        if(currentUpgrader != null)
        {
            towerInfo = tower.GetTowerInfo();

            titleText.text = "Sell " + towerInfo.name;
            currencyText.text = "+"+ (currentUpgrader.GetSellPrice() + 2) +" Proteins";
        }
    }

    public void OnMakePurchase(InputValue inputValue)
    {
        if(tower != null)
        {
            GameManager.Instance.Currency += currentUpgrader.GetSellPrice() + 2;
            tower.DestroyTower();
        }
    }
}
