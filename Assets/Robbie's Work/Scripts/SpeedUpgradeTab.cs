using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobbieWagnerGames.UI;
using GameJam.Towers; 
using TMPro;
using UnityEngine.InputSystem;
using RobbieWagnerGames;
public class SpeedUpgradeTab : MenuTab
{
    public GenericTowerBehaviour tower;
    private TowerInfo towerInfo;
    TowerUpgrader currentUpgrader;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI beforeText;
    [SerializeField] private TextMeshProUGUI afterText;

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
        Debug.Log("build tab");
        if(currentUpgrader != null)
        {
            if(currentUpgrader.CurrentCooldownUpgrade + 1 < currentUpgrader.cooldownUpgradeCosts.Count)
            {
                towerInfo = tower.GetTowerInfo();

                nameText.text = towerInfo.name;
                description.text = "Lower Cooldown Time";
                beforeText.text = towerInfo.attackCooldown.ToString();
                afterText.text = currentUpgrader.cooldownUpgradeValues[currentUpgrader.CurrentCooldownUpgrade + 1].ToString();
                costText.text = "Cost: " + currentUpgrader.cooldownUpgradeCosts[currentUpgrader.CurrentCooldownUpgrade + 1].ToString();
            }
            else
            {
                towerInfo = tower.GetTowerInfo();

                nameText.text = towerInfo.name;
                description.text = "Lower Cooldown Time";
                costText.text = "MAX";

                beforeText.text = "";
                afterText.text = "";
            }
        }
    }

    public void OnMakePurchase(InputValue inputValue)
    {
        if(currentUpgrader.cooldownUpgradeCosts[currentUpgrader.CurrentCooldownUpgrade + 1] <= GameManager.Instance.Currency)
        {
            PurchaseUpgrade();
        }

        BuildTab();
    }

    public void PurchaseUpgrade()
    {
        currentUpgrader.CurrentCooldownUpgrade += 1;
        GameManager.Instance.Currency -= currentUpgrader.cooldownUpgradeCosts[currentUpgrader.CurrentCooldownUpgrade];
    }
}