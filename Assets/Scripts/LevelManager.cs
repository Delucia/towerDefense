//using Microsoft.Win32;
using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public Camera camera;
    public GameObject enemy;
    public float respawnRate;

    /////////////////  GUI
    public UILabel UIhealthText;
    public UILabel UIscoreText;
    public UILabel UIwaveText;
    public UILabel UIcashText;
    public UILabel[] UITurretCosts;

    public static int gameHealth = 10;
    public static int gameScore;
    public static int gameCash;

    // GUI button Tween things
    public TweenPosition buildPanelTweener;
    private bool buildPanelOpen = false;
    public TweenRotation buildPanelArrowTweener;

    // Color changes for buttons
    public Color onColor;
    public Color offColor;
    public UISlicedSprite[] buildBtnGraphics;

    //Plane Materials
    private Material originalMat;
    public Material hoverMat;

    //turrets
    public GameObject[] structures;
    private int structureIndex;


    int layerMask = 1 << 8;
    private GameObject lastHitObj;
    //////////////////////////////////////

    public int[] turretCosts;

    //States
    private bool waveActive = false;
    private bool spawnEnemies;

    // Wave
    public int[] waveGroundedNumber;
    public int[] waveAirNumber;
    public int maxWaveLevel;
    public float intermissionTime;
    public float difficultyStart = 1f;
    public int startingCash;
    
    public static float difficultyMultiplier;
    private float nextSpawnTime = 1f;
    private int waveLevel;

    private int totalEnemyToSpawn;
    private int enemySpawnedThisWave;
    public static int enemyCountInGame;
    private float nextWaveTime;

    void Start ()
    {
        WriteTurretCostText();
        structureIndex = 0;
        buildPanelOpen = false;
        gameCash = startingCash;
        
        SetNextWave();
        StartNextWave();
        
    }
	
	void Update ()
	{
	    UIscoreText.text = gameScore.ToString();
	    UIhealthText.text = gameHealth.ToString();
	    UIcashText.text = gameCash.ToString();

        ///////////////------------GUI--------------/////////////
        if (buildPanelOpen == true)
        {
            
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            //Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (lastHitObj)
                {
                    lastHitObj.renderer.material = originalMat;
                }

                lastHitObj = hit.collider.gameObject;
                originalMat = lastHitObj.renderer.material;
                lastHitObj.renderer.material = hoverMat;
            }
            else
            {
                if (lastHitObj)
                {
                    lastHitObj.renderer.material = originalMat;
                    lastHitObj = null;
                }
            }
        }
        
        if (Input.GetMouseButtonDown(0) && lastHitObj)
        {
            if (lastHitObj.tag == "PlacementPlane_Open")
            {
                if (turretCosts[structureIndex] <= gameCash)
                {
                    int rotationY = Random.Range(0, 360);

                    GameObject newStructure =Instantiate(structures[structureIndex], lastHitObj.transform.position, lastHitObj.transform.rotation) as GameObject;
                    newStructure.transform.eulerAngles = new Vector3(0, rotationY, 0);
                    lastHitObj.tag = "PlacementPlane_Taken";

                    gameCash -= turretCosts[structureIndex];
                    UpdateGUI();
                }
                else Debug.Log("not enough money");
            }
        }
        
        /////////-----GUI-----------//////////////
        
        
        
        
        
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
        UpdateGUI();
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



    /////// ------------- GUI Methods------------------- /////

    void toggleBuildPanel()
    {
        if (buildPanelOpen == false)
        {
            buildPanelTweener.Play(true);
            buildPanelArrowTweener.Play(true);
            buildPanelOpen = true;
        }
        else
        {
            buildPanelTweener.Play(false);
            buildPanelArrowTweener.Play(false);
            buildPanelOpen = false;
        }
    }

    void SetBuildChoice(GameObject btnObj)
    {
        string btnName = btnObj.name;

        if (btnName == "Btn_Cannon")
            structureIndex = 0;
        if (btnName == "Btn_Missile")
            structureIndex = 1;
        if (btnName == "Btn_Mine")
            structureIndex = 2;

        UpdateGUI();
    }



    void UpdateGUI()
    {
        UIwaveText.text = waveLevel.ToString() + " / " + maxWaveLevel.ToString();
        if (buildPanelOpen)
        {
            foreach (var btn in buildBtnGraphics)
            {
                btn.color = offColor;
            }

            buildBtnGraphics[structureIndex].color = onColor;
        }
        //Debug.Log(onColor);
        // Debug.Log(buildBtnGraphics[structureIndex].color);

        CheckTurretCosts();
    }

    void CheckTurretCosts()
    {
        Debug.Log(gameCash);
        for(int i=0; i<structures.Length; i++)
        {
            if (turretCosts[i] > gameCash)
            {
                UITurretCosts[i].color = Color.red;
                buildBtnGraphics[i].color = new Color(.5f, .5f, .5f, .5f);
                buildBtnGraphics[i].transform.parent.gameObject.collider.enabled = false;
            }
        }
    }

    void WriteTurretCostText()
    {
        for (int i = 0; i < turretCosts.Length; i++)
        {
            UITurretCosts[i].text = "$" + turretCosts[i];
        }
    }
}
