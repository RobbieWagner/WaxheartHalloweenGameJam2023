using System.Collections;
using System.Collections.Generic;
using GameJam.Towers;
using UnityEngine;

public class TowerUpgrader : MonoBehaviour
{
    [SerializeField] public GenericTowerBehaviour tower;

    public List<float> cooldownUpgradeValues;
    public List<int> cooldownUpgradeCosts;
    public List<int> attackPowerUpgradeValues;
    public List<int> attackUpgradeCosts;

    private int currentCooldownUpgrade = 0;
    public int CurrentCooldownUpgrade
    {
        get { return currentCooldownUpgrade; } 
        set
        {
            if(value == currentCooldownUpgrade) return;
            currentCooldownUpgrade = value;
            OnChangeCooldownLevel(cooldownUpgradeValues[value]);
        }
    }
    public delegate void OnChangeCooldownLevelDelegate(float level);
    public event OnChangeCooldownLevelDelegate OnChangeCooldownLevel;

    private int currentAttackUpgrade = 0;
    public int CurrentAttackUpgrade
    {
        get { return currentAttackUpgrade; } 
        set
        {
            if(value == currentAttackUpgrade) return;
            currentAttackUpgrade = value;
            OnChangeAttackLevel(attackPowerUpgradeValues[value]);
        }
    }
    public delegate void OnChangeAttackLevelDelegate(int level);
    public event OnChangeAttackLevelDelegate OnChangeAttackLevel;
}
