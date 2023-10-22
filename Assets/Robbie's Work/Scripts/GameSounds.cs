using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSounds : MonoBehaviour
{
    int heartHealth;

    [SerializeField] private AudioSource heartDamageSound;

    public static GameSounds Instance {get; private set;}

    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 

        heartHealth = Heart.Instance.Health;
    }

    private void PlayHeartDamageSound(int newHealth)
    {
        if(newHealth < heartHealth)
        {
            heartDamageSound.Play();
        }

        heartHealth = Heart.Instance.Health;
    }
}
