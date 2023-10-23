using System.Collections;
using System.Collections.Generic;
using GameJam.Towers;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    None = 0,
    Idle = 1,
    Moving = 2,
    Attacking = 3,
}

public class TDEnemy: MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int attackPower = 1;
    [SerializeField] private float attackingDistance = 1f;
    [HideInInspector] public float idleTimeAfterSpawn;
    [SerializeField] private int currencyDrop = 1;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private float pVelocity;
    [SerializeField] AudioSource sound;

    [SerializeField] private int health;
    public int Health
    {
        get { return health; }
        set
        {
            if(value == health) return;
            health = value;
            OnHealthChanged?.Invoke(health);
        }
    }
    public delegate void OnHealthChangedDelegate(int newHealth);
    public event OnHealthChangedDelegate OnHealthChanged;

    private IEnumerator currentStateCoroutine;
    private EnemyState currentState;
    public EnemyState CurrentState
    {
        get { return currentState; }
        set
        {
            if(value == currentState) return;
            currentState = value;
            OnStateChanged?.Invoke(currentState);
        }
    }
    public delegate void OnStateChangedDelegate(EnemyState state);
    public event OnStateChangedDelegate OnStateChanged;

    void Awake()
    {
        OnStateChanged += ChangeState;
        OnHealthChanged += CheckForDeath;
        agent.enabled = false;
        StartCoroutine(DelayNavMeshEnable(true));
    }

    private void ChangeState(EnemyState state)
    {
        if(currentStateCoroutine != null) StopCoroutine(currentStateCoroutine);

        switch(state)
        {
            case EnemyState.Idle:
            currentStateCoroutine = StandIdle(idleTimeAfterSpawn);
            break;
            case EnemyState.Moving:
            currentStateCoroutine = ChaseAfterHeart();
            break;
            case EnemyState.Attacking:
            currentStateCoroutine = AttackHeart();
            break;
        }
        if(currentStateCoroutine != null) StartCoroutine(currentStateCoroutine);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private IEnumerator StandIdle(float idleTime)
    {
        yield return new WaitForSeconds(idleTime);
        CurrentState = EnemyState.Moving;
    }

    private IEnumerator ChaseAfterHeart()
    {
        //Debug.Log("heart chase");
        agent.isStopped = false;
        agent.destination = Heart.Instance.transform.position;

        while (Vector3.Distance(Heart.Instance.transform.position, transform.position) > attackingDistance)
        {
            yield return null;
        }

        CurrentState = EnemyState.Attacking;
    }

    private IEnumerator AttackHeart()
    {
        agent.isStopped = true;
        //Debug.Log("heart attack");
        while(true)
        {
            Heart.Instance.Health -= attackPower;
            if(sound != null) sound.Play();
            if(bulletPrefab != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPos.position, bulletSpawnPos.rotation);
                Vector3 launchForceVector = Vector3.forward * pVelocity;
                bullet.GetComponent<Rigidbody>().AddRelativeForce(launchForceVector, ForceMode.Impulse);
            }
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private void CheckForDeath(int newHealth)
    {
        if(newHealth <= 0)
        {
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        OnKillEnemy?.Invoke(this);
        OnDropCurrency?.Invoke(currencyDrop);
    }
    public delegate void OnKillEnemyDelegate(TDEnemy enemy);
    public event OnKillEnemyDelegate OnKillEnemy;
    public delegate void OnCurrencyDropDelegate(int currency);
    public event OnCurrencyDropDelegate OnDropCurrency;

    private IEnumerator DelayNavMeshEnable(bool isStopped)
    {
        yield return new WaitForSeconds(.025f);
        agent.enabled = true;
        agent.isStopped = isStopped;
        StopCoroutine(DelayNavMeshEnable(isStopped));
    }
}