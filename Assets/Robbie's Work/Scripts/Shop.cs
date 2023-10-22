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

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemiesText;
    private bool isActiveTabTowers = false;

    [SerializeField] private MenuWithTabs upgradeMenu;

    [SerializeField] private MenuWithTabs towerMenu;

    [HideInInspector] public TowerUpgrader tower;
    [HideInInspector] public TowerSpawnSpot spawnSpot;

    private PlayerInputActions inputActions;

    public static Shop Instance {get; private set;}

    protected void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 

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

        Ray ray = new Ray(SimpleFirstPersonMouseLook.Instance.transform.position, SimpleFirstPersonMouseLook.Instance.transform.forward);
        //Debug.DrawRay(SimpleFirstPersonMouseLook.Instance.transform.position, SimpleFirstPersonMouseLook.Instance.transform.forward, Color.red);

        List<RaycastHit> hits= Physics.RaycastAll(ray, 5).ToList<RaycastHit>();
        List<RaycastHit> sortedHits = hits.OrderBy(obj => Vector3.Distance(
                                                    SimpleFirstPersonPlayerMovement.Instance.transform.position,
                                                    obj.transform.position)).ToList<RaycastHit>();

        foreach(RaycastHit hit in sortedHits)
        {
            if(hit.collider.GetComponent<TowerUpgrader>() != null)
            {
                tower = hit.collider.GetComponent<TowerUpgrader>();
                break;
            }
            else if(hit.collider.GetComponent<TowerSpawnSpot>() != null)
            {
                spawnSpot = hit.collider.GetComponent<TowerSpawnSpot>();
                if(!spawnSpot.isEmpty) spawnSpot = null;
                else break;
            }
        }

        if(tower != null)
        {
            DisplayMenu(upgradeMenu);
            HideMenu(towerMenu);
            //DisplayUpgradeSelection(upgradeSelection);
        }

        else if(spawnSpot != null)
        {
            DisplayMenu(towerMenu);
            HideMenu(upgradeMenu);
            //DisplayTowerSelection(towerSelection);
        }

        else
        {
            HideMenu(upgradeMenu);
            HideMenu(towerMenu);
        }
    }

    private void DisplayMenu(MenuWithTabs menu)
    {
        menu.gameObject.SetActive(true);
    }

    private void HideMenu(MenuWithTabs menu)
    {
        menu.gameObject.SetActive(false);
    }

    // private void DisplayUpgradeSelection(int upgrade)
    // {
    //     TowerInfo info = tower.tower.GetTowerInfo();

    //     upgradeTowerNameText.text = info.name;
    // }


}
