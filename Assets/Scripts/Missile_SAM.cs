using System.Xml.Serialization;
using UnityEngine;
using System.Collections;

public class Missile_SAM : MonoBehaviour
{
    public int damage;
    public float speed = 10f;
    public float range = 30f;
    public GameObject explosion;

    private float distanceTravelled;
    public static Transform Target;
    private Transform myTarget;

	void Start ()
	{
	    myTarget = Target;
	}
	


	void Update () 
    {
        if (myTarget)
	    {
            transform.LookAt(myTarget);
	        transform.Translate(0, 0, Time.deltaTime*speed);
	        distanceTravelled += Time.deltaTime*speed;

	        if (distanceTravelled >= range)
	            Explode();
	    }
	    else
	    {
	        Explode();
	    }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Air Enemy")
        {
            other.gameObject.SendMessage("takeDamage", damage, SendMessageOptions.DontRequireReceiver);
            Explode();

        }
            
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
