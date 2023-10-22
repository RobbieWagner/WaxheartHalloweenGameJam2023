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
    [SerializeField] private TextMeshProUGUI waveText;
    private bool isActiveTabTowers = false;

    [SerializeField] private MenuWithTabs upgradeMenu;

    [SerializeField] private MenuWithTabs towerMenu;

    [HideInInspector] public TowerUpgrader tower;
    [HideInInspector] public TowerSpawnSpot spawnSpot;

    private bool showingUpgradeMenu;
    private bool showingTowerMenu;

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
        GameManager.Instance.OnWaveChange += UpdateWaveText;

        inputActions = new PlayerInputActions();

        showingTowerMenu = false;
        showingUpgradeMenu = false;
    }

    private void UpdateEnemyCount(List<TDEnemy> enemies)
    {
        if(enemies.Count == 0) enemiesText.enabled = false;
        else enemiesText.enabled = true;
        enemiesText.text = "Enemies Left: " + enemies.Count;
    }

    private void UpdateWaveText(int wave)
    {
        if(wave == 0)
        {
            waveText.enabled = false;
        }
        else
        {
            waveText.enabled = true;
        }
        waveText.text = "Wave: " + wave;
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

        if(tower != null && !showingUpgradeMenu)
        {
            DisplayMenu(upgradeMenu);
            HideMenu(towerMenu);
            showingUpgradeMenu = true;
            showingTowerMenu = false;
        }

        else if(spawnSpot != null && !showingTowerMenu)
        {
            DisplayMenu(towerMenu);
            HideMenu(upgradeMenu);
            showingTowerMenu = true;
            showingUpgradeMenu = false;
        }

        else if(spawnSpot == null && tower == null)
        {
            HideMenu(upgradeMenu);
            HideMenu(towerMenu);
            showingTowerMenu = false;
            showingUpgradeMenu = false;
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
}
