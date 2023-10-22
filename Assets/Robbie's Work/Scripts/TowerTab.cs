using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobbieWagnerGames.UI;
using GameJam.Towers; 
using TMPro;
using UnityEngine.InputSystem;
using RobbieWagnerGames;

[System.Serializable]
public class TowerShopItem
{
    [SerializeField] public TowerInfo towerInfo;
    [SerializeField] public GenericTowerBehaviour tower;
    [SerializeField] public int cost;
}

public class TowerTab : MenuTab
{
    
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI cooldownText;
    public TowerShopItem shopItem;
    private PlayerInputActions inputActions; 

    void Awake()
    {
        inputActions = new PlayerInputActions();
        BuildTab();
    }

    public override void BuildTab()
    {
        base.BuildTab();

        DisplayTowerInfo();
    }

    private void OnMakePurchase(InputValue value)
    {
        if(shopItem.cost <= GameManager.Instance.Currency)
        {
            PurchaseNewTower();
        }
    }

    private void PurchaseNewTower()
    {
        GenericTowerBehaviour newTower = Instantiate(shopItem.tower, Shop.Instance.spawnSpot.transform);
        newTower.transform.position = Shop.Instance.spawnSpot.spawnPos.position;
        newTower.SetTowerInfo(shopItem.towerInfo);
        Shop.Instance.spawnSpot.isEmpty = false;
        GameManager.Instance.Currency -= shopItem.cost;
    }

    private void DisplayTowerInfo()
    {
        TowerInfo info = shopItem.towerInfo;
        nameText.text = info.name;
        costText.text = "Cost: " + shopItem.cost;
        powerText.text = "Power: " + info.attackPower.ToString();
        cooldownText.text = "Cooldown: " + RoundToTenths(info.attackCooldown);
    }

    private float RoundToTenths(float number)
    {
        return ((float)((int) (number * 10))) /10;
    }
}

