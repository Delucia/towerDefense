using UnityEngine;
using System.Collections;

public class Projectile_Cannon : MonoBehaviour
{

    public float range;
    public float speed;

    private float distanceTravelled;


    void Start () 
    {
	
	}
	
	void Update () 
    {
	    transform.Translate(Vector3.forward * Time.deltaTime * speed);
	    distanceTravelled += Time.deltaTime*speed;
        if(distanceTravelled >= range)
            Destroy(gameObject);
        
	}
}
