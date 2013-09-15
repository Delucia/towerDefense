using Microsoft.Win32;
using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{

    public GameObject enemy;
    public float respawnRate;

    //GUI
    public UILabel UIhealthText;
    public UILabel UIscoreText;
    public UILabel UIwaveText;

    public static int gameHealth = 10;
    public static int gameScore;

    //States
    private bool waveActive = false;
    private bool spawnEnemies;

    // Wave
    public int[] waveGroundedNumber;
    public int[] waveAirNumber;
    public int maxWaveLevel;
    public float intermissionTime;
    public float difficultyStart = 1f;
    
    public static float difficultyMultiplier;
    private float nextSpawnTime = 1f;
    private int waveLevel;

    private int totalEnemyToSpawn;
    private int enemySpawnedThisWave;
    public static int enemyCountInGame;
    private float nextWaveTime;

    void Start ()
    {
        SetNextWave();
        StartNextWave();
    }
	
	void Update ()
	{
	    UIscoreText.text = gameScore.ToString();
	    UIhealthText.text = gameHealth.ToString();

        if (Time.time >= nextWaveTime && !waveActive)
	    {
	        waveActive = true;
	        spawnEnemies = true;
            SetNextWave();
            StartNextWave();
	    }
        
        if (spawnEnemies)
	    {
	        if (enemySpawnedThisWave < totalEnemyToSpawn)
	        {
	            if (Time.time >= nextSpawnTime)
	                SpawnNewEnemy();
	        }
	        else spawnEnemies = false;
	    }
	    else
	    {
            if (enemyCountInGame == 0 && waveActive)
            {
                FinishWave();
            }
	    }
    }

    void SetNextWave()
    {
        waveLevel++;
        difficultyMultiplier = (Mathf.Pow(waveLevel, 2)*.005f) + difficultyStart;
        totalEnemyToSpawn = waveGroundedNumber[waveLevel-1] + waveAirNumber[waveLevel-1];
    }

    void StartNextWave()
    {
        UpdateHUD();
        waveActive = true;
        spawnEnemies = true;
        SpawnNewEnemy();
    }

    void UpdateHUD()
    {
        UIwaveText.text = waveLevel.ToString() + " / " + maxWaveLevel.ToString();
    }

    void SpawnNewEnemy()
    {
        // Grounded vs air ayrýmý yapýlacak
        Instantiate(enemy, transform.position, transform.rotation);
        nextSpawnTime = Time.time + respawnRate;
        enemyCountInGame++;
        enemySpawnedThisWave++;
    }

    void FinishWave()
    {
        waveActive = false;
        nextWaveTime = Time.time + intermissionTime;
        enemySpawnedThisWave = 0;
    }
}
