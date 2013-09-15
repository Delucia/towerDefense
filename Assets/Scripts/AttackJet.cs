using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

public class AttackJet : MonoBehaviour
{
    public int ScoreValue;
    public Vector2 heightRange;
    public float speed;
    public int maxHealth;
    public int currentHealth;
    public GameObject explosion;


    
    void Start ()
    {
        LevelManager manager = GetComponent("LevelManager") as LevelManager;
        currentHealth = (int)(maxHealth * LevelManager.difficultyMultiplier);
        transform.position = new Vector3(transform.position.x, Random.Range(heightRange.x, heightRange.y), transform.position.z);
        
    }
	
	void Update () 
    {
	    transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}

    void takeDamage(int dmgAmount)
    {
        currentHealth -= dmgAmount;

        if (currentHealth <= 0)
        {
            
            explode();
            return;
        }
    }

    void explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
        LevelManager.enemyCountInGame--;
        LevelManager.gameScore += ScoreValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gate")
        {
            LevelManager.gameHealth--;
            explode();
        }
    }
}

