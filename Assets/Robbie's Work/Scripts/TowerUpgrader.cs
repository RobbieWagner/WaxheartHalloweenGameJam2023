using System.Collections;
using System.Collections.Generic;
using GameJam.Towers;
using UnityEngine;

public class TowerUpgrader : MonoBehaviour
{
    [SerializeField] public GenericTowerBehaviour tower;

    [SerializeField] protected List<float> cooldownUpgradeValues;
    [SerializeField] protected List<int> attackPowerUpgradeValues;
}
