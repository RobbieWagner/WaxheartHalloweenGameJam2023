using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

[System.Serializable]
public class EnemyWaveGenerationInfo
{
    [SerializeField] public EnemyWaveInfo info;
    public int difficultyClass = 1;
    public int waveModulus = 1;
}

public class WaveGenerator : MonoBehaviour
{
    [SerializeField] List<EnemyWaveGenerationInfo> enemies;
    [SerializeField] private int wavesBetweenLanes;
    private int wavesToNewLane;
    private int openLanes = 2;

    public static WaveGenerator Instance {get; private set;}
    
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

        enemies = enemies.OrderBy(enemy => enemy.difficultyClass).ToList<EnemyWaveGenerationInfo>();
        wavesToNewLane = wavesBetweenLanes;
    }

    public Wave GenerateWave(int wave)
    {
        wavesToNewLane--;
        if(wavesToNewLane == 0)
        {
            wavesToNewLane = wavesBetweenLanes;
            if(openLanes < 4) openLanes++;
        }
        int difficultyLimit = wave * 2;
        int difficultyRank = 0;

        Wave returnWave = new Wave();

        while(difficultyRank < difficultyLimit)
        {
            int index = UnityEngine.Random.Range(0, enemies.Count);
            EnemyWaveGenerationInfo enemy = enemies[index];

            while(enemy.difficultyClass + difficultyRank > difficultyLimit && index >= 0 && enemy.info.spawnSpot >= openLanes && wave % enemy.waveModulus != 0)
            {
                index--;
                enemy = enemies[index];
            }
            if(index == -1) return returnWave;

            returnWave.enemies.Add(enemy.info);
            difficultyRank += enemy.difficultyClass;
        }

        return returnWave;
    }
}
