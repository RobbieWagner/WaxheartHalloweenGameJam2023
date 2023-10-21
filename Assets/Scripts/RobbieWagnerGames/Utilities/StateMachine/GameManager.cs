using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Heart heart;
    private IEnumerator currentPhaseCoroutine;

    [Header("Setup")]

    [Header("Prep")]
    [SerializeField] private float prepTime = 5f;

    [Header("Wave")]


    private GameState currentState;
    public GameState CurrentState
    {
        get { return currentState; }
        set
        {
            if(value == currentState) return;
            currentState = value;
            OnStateChanged(currentState);
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
            OnCurrencyChanged(currency);
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
        CurrentState = GameState.Prep;
    }

    private IEnumerator BeginPrep()
    {
        yield return new WaitForSeconds(1f);
        CurrentState = GameState.Wave;
    }

    private IEnumerator BeginWave()
    {
        yield return new WaitForSeconds(1f);
        CurrentState = GameState.Resolve;
    }

    private IEnumerator ResolveRound()
    {
        yield return null;
        CurrentState = GameState.Prep;
    }
    
    void Update()
    {

    }
}
