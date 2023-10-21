using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RobbieWagnerGames.UI;
using UnityEngine.UI;
using TMPro;
using RobbieWagnerGames.FirstPerson;
using GameJam.Towers;
using UnityEngine.InputSystem;
using RobbieWagnerGames;

[System.Serializable]
public class TowerShopItem
{
    [SerializeField] public TowerInfo towerInfo;
    [SerializeField] public GenericTowerBehaviour tower;
}

public class Shop : MenuWithTabs
{
    [SerializeField] private TextMeshProUGUI enemiesText;
    private bool isActiveTabTowers = false;

    [Header("Upgrades")]
    [SerializeField] private LayoutGroup upgradeMenu;
    [SerializeField] private TextMeshProUGUI upgradeTowerNameText;
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private List<TowerUpgrader> upgradeOptions;
    [SerializeField] private int upgradeSelection = 0;

    [Header("Towers")]
    [SerializeField] private LayoutGroup towerMenu;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private List<TowerShopItem> towerOptions;
    [SerializeField] private int towerSelection = 0;

    private TowerUpgrader tower;
    private TowerSpawnSpot spawnSpot;

    private PlayerInputActions inputActions;

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
        GameManager.Instance.OnEnemyListUpdated += UpdateEnemyCount;

        inputActions = new PlayerInputActions();
    }

    private void UpdateEnemyCount(List<TDEnemy> enemies)
    {
        enemiesText.text = "Enemies Left: " + enemies.Count;
    }

    private void Update()
    {
        tower = null;
        spawnSpot = null;

        isActiveTabTowers = ActiveTab == 0;

        Ray ray = new Ray(SimpleFirstPersonMouseLook.Instance.transform.position, SimpleFirstPersonMouseLook.Instance.transform.forward);
        Debug.DrawRay(SimpleFirstPersonMouseLook.Instance.transform.position, SimpleFirstPersonMouseLook.Instance.transform.forward, Color.red);

        List<RaycastHit> hits= Physics.RaycastAll(ray, 5).ToList<RaycastHit>();
        List<RaycastHit> sortedHits = hits.OrderBy(obj => Vector3.Distance(
                                                    SimpleFirstPersonPlayerMovement.Instance.transform.position,
                                                    obj.transform.position)).ToList<RaycastHit>();

        foreach(RaycastHit hit in sortedHits)
        {
            if(!isActiveTabTowers && hit.collider.GetComponent<TowerUpgrader>())
            {
                tower = hit.collider.GetComponent<TowerUpgrader>();
                break;
            }
            else if(isActiveTabTowers && hit.collider.GetComponent<TowerSpawnSpot>())
            {
                spawnSpot = hit.collider.GetComponent<TowerSpawnSpot>();
                break;
            }
        }

        if(tower != null)
        {
            DisplayMenu(upgradeMenu);
            HideMenu(towerMenu);
            DisplayUpgradeSelection(upgradeSelection);
        }

        else if(spawnSpot != null)
        {
            DisplayMenu(towerMenu);
            HideMenu(upgradeMenu);
            DisplayTowerSelection(towerSelection);
        }

        else
        {
            HideMenu(upgradeMenu);
            HideMenu(towerMenu);
        }
    }

    private void OnMakePurchase(InputValue value)
    {
        if(tower != null) Debug.Log("purchase upgrade");
        else if(spawnSpot != null) Debug.Log("purchase tower");
    }

    private void DisplayMenu(LayoutGroup menu)
    {
        menu.gameObject.SetActive(true);
    }

    private void HideMenu(LayoutGroup menu)
    {
        menu.gameObject.SetActive(false);
    }

    private void DisplayUpgradeSelection(int upgrade)
    {
        TowerInfo info = tower.tower.GetTowerInfo();

        upgradeTowerNameText.text = info.name;
    }

    private void DisplayTowerSelection(int tower)
    {
        TowerInfo info = towerOptions[tower].towerInfo;

        towerName.text = info.name;
        powerText.text = "Power: " + info.attackPower.ToString();
        cooldownText.text = "Cooldown: " + RoundToTenths(info.attackCooldown);
    }

    private float RoundToTenths(float number)
    {
        return ((float)((int) (number * 10))) /10;
    }

    private bool PurchaseNewTower(int index)
    {
        if(index > 0 && index < towerOptions.Count)
        {
            GenericTowerBehaviour newTower = Instantiate(towerOptions[index].tower, spawnSpot.transform);
            newTower.SetTowerInfo(towerOptions[index].towerInfo);
            spawnSpot.enabled = false;
        }

        return true;
    }
}
