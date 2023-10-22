using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames.StrategyCombat.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

// In Setup, setup the arena
// In Prep, player gets a break before the wave
// In wave, enemies spawn and try to kill the heart
// In Resolve, the game checks for victory

public enum GameState
{
    None = 0,
    Setup = 1,
    Prep = 2,
    Wave = 3,
    Resolve = 4,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    private IEnumerator currentPhaseCoroutine;

    [Header("Setup")]

    [Header("Prep")]
    [SerializeField] private float prepTime = 5f;

    [Header("Wave")]
    [SerializeField] private int enemiesPerRound = 3;
    [SerializeField] private float timeBetweenEnemies = 3f;
    private List<TDEnemy> waveEnemies;
    [SerializeField] private TDEnemy enemyToSpawn;
    [SerializeField] private Transform spawnSpot;


    private GameState currentState;
    public GameState CurrentState
    {
        get { return currentState; }
        set
        {
            if(value == currentState) return;
            currentState = value;
            Debug.Log(value);
            OnStateChanged?.Invoke(currentState);
        }
    }
    public delegate void OnStateChangedDelegate(GameState state);
    public event OnStateChangedDelegate OnStateChanged;

    [SerializeField] private int startingCurrency = 0;
    private int currency;
    public int Currency
    {
        get { return currency; }
        set
        {
            if(value == currency) return;
            currency = value;
            OnCurrencyChanged?.Invoke(currency);
        }
    }
    public delegate void OnCurrencyChangedDelegate(int newCurrency);
    public event OnCurrencyChangedDelegate OnCurrencyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 

        OnStateChanged += ApplyGameState;

        Currency = startingCurrency;
        CurrentState = GameState.Setup;
    }
    public delegate void OnStartGameDelegate();
    public event OnStartGameDelegate OnStartGame;

    private void ApplyGameState(GameState gameState)
    {
        if(currentPhaseCoroutine != null) StopCoroutine(currentPhaseCoroutine);

        switch(gameState)
        {
            case GameState.Setup:
            currentPhaseCoroutine = SetupGame();
            break;
            case GameState.Prep:
            currentPhaseCoroutine = BeginPrep();
            break;
            case GameState.Wave:
            currentPhaseCoroutine = BeginWave();
            break;
            case GameState.Resolve:
            currentPhaseCoroutine = ResolveRound();
            break;
        }

        StartCoroutine(currentPhaseCoroutine);
    }

    private IEnumerator SetupGame()
    {
        yield return null;
        waveEnemies = new List<TDEnemy>();
        CurrentState = GameState.Prep;
    }

    private IEnumerator BeginPrep()
    {
        //Debug.Log("prep time");
        float time = 0f;

        while(time < prepTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        
        CurrentState = GameState.Wave;
    }

    private IEnumerator BeginWave()
    {
        //Debug.Log("combat time");
        ClearEnemies();

        for(int i = 0; i < enemiesPerRound; i++)
        {
            TDEnemy newEnemy = SpawnEnemy(enemyToSpawn, timeBetweenEnemies * i);
            newEnemy.OnKillEnemy += DestroyEnemy;
            yield return null;
        }

        while(waveEnemies.Count > 0 && Heart.Instance.Health > 0)
        {
            yield return null;
        }

        CurrentState = GameState.Resolve;
    }

    private IEnumerator ResolveRound()
    {
        yield return null;
        if(Heart.Instance.Health <= 0)
        {
            StartCoroutine(FinishGame(false));
        }
        //else if(win condition)
        // {
            //StartCoroutine(FinishGame(true));
        // }
        else
        {
            enemiesPerRound++;
            CurrentState = GameState.Prep;
        }
    }

    public TDEnemy SpawnEnemy(TDEnemy enemy, float idleTime = 0f)
    {
        TDEnemy spawnedEnemy = Instantiate(enemy).GetComponent<TDEnemy>();
        spawnedEnemy.transform.position = spawnSpot.position;
        spawnedEnemy.idleTimeAfterSpawn = idleTime;
        spawnedEnemy.CurrentState = EnemyState.Idle;
        waveEnemies.Add(spawnedEnemy);
        OnEnemySpawned?.Invoke(spawnedEnemy);
        OnEnemyListUpdated?.Invoke(waveEnemies);
        spawnedEnemy.OnDropCurrency += AddCurrency;
        return spawnedEnemy;
    }
    public delegate void OnEnemySpawnedDelegate(TDEnemy newEnemy);
    public event OnEnemySpawnedDelegate OnEnemySpawned;
    public delegate void OnEnemyListUpdatedDelegate(List<TDEnemy> enemies);
    public event OnEnemyListUpdatedDelegate OnEnemyListUpdated;

    public void ClearEnemies()
    {
        foreach(TDEnemy enemy in waveEnemies)
        {
            enemy.DestroyEnemy();
        }
        waveEnemies.Clear();
        OnEnemyListUpdated?.Invoke(waveEnemies);
    }

    public void DestroyEnemy(TDEnemy enemy)
    {
        enemy.DestroyEnemy();
        waveEnemies.Remove(enemy);
        OnEnemyListUpdated?.Invoke(waveEnemies);
    }

    private IEnumerator FinishGame(bool win)
    {
        if(win) Debug.Log("win");
        else Debug.Log("lose");

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }
    
    void Update()
    {

    }

    private void AddCurrency(int currencyAddition)
    {
        Currency += currencyAddition;
    }
}
