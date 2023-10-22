using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobbieWagnerGames.UI;
using GameJam.Towers; 
using TMPro;
using UnityEngine.InputSystem;
using RobbieWagnerGames;
public class PowerUpgradeTab : MenuTab
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
        //Debug.Log("build tab");
        if(currentUpgrader != null)
        {
            if(currentUpgrader.CurrentAttackUpgrade + 1 < currentUpgrader.attackUpgradeCosts.Count)
            {
                towerInfo = tower.GetTowerInfo();

                nameText.text = towerInfo.name;
                description.text = "Raise Attack Power";
                beforeText.text = towerInfo.attackPower.ToString();
                afterText.text = currentUpgrader.attackPowerUpgradeValues[currentUpgrader.CurrentAttackUpgrade + 1].ToString();
                costText.text = "Cost: " + currentUpgrader.attackUpgradeCosts[currentUpgrader.CurrentAttackUpgrade + 1].ToString();
            }
            else
            {
                towerInfo = tower.GetTowerInfo();

                nameText.text = towerInfo.name;
                description.text = "Raise Attack Power";
                costText.text = "MAX";

                beforeText.text = "";
                afterText.text = "";
            }
        }
    }

    public void OnMakePurchase(InputValue inputValue)
    {
        if(currentUpgrader.attackUpgradeCosts[currentUpgrader.CurrentAttackUpgrade + 1] <= GameManager.Instance.Currency)
        {
            PurchaseUpgrade();
        }

        BuildTab();
    }

    public void PurchaseUpgrade()
    {
        currentUpgrader.CurrentAttackUpgrade += 1;
        GameManager.Instance.Currency -= currentUpgrader.attackUpgradeCosts[currentUpgrader.CurrentAttackUpgrade];
    }
}