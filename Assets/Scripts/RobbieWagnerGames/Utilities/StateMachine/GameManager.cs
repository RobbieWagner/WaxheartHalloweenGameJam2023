using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames.StrategyCombat.Units;
using UnityEngine;

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
    [SerializeField] private Vector3 spawnSpot;


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

    
    [SerializeField] private int currency;
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

    void Start()
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

        Currency = 0;
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
        Debug.Log("prep time");
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
        Debug.Log("combat time");
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
        bool isGameOver = CheckForGameOver();
        if(isGameOver)
        {
            if(Heart.Instance.Health <= 0) Debug.Log("LOSE");
            else Debug.Log("WIN");
        }
        else
        {
            CurrentState = GameState.Prep;
        }
    }

    public TDEnemy SpawnEnemy(TDEnemy enemy, float idleTime = 0f)
    {
        TDEnemy spawnedEnemy = Instantiate(enemy).GetComponent<TDEnemy>();
        spawnedEnemy.transform.position = spawnSpot;
        spawnedEnemy.idleTimeAfterSpawn = idleTime;
        spawnedEnemy.CurrentState = EnemyState.Idle;
        waveEnemies.Add(spawnedEnemy);
        return spawnedEnemy;
    }

    public void ClearEnemies()
    {
        foreach(TDEnemy enemy in waveEnemies)
        {
            enemy.DestroyEnemy();
        }
        waveEnemies.Clear();
    }

    public void DestroyEnemy(TDEnemy enemy)
    {
        enemy.DestroyEnemy();
        waveEnemies.Remove(enemy);
    }
    
    private bool CheckForGameOver()
    {
        if(Heart.Instance.Health <= 0) return true;
        return false;
    }
}
