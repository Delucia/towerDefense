using UnityEngine;
using System.Collections;

public class Turret_SAM : MonoBehaviour
{
    public float turnSpeed;
    public float reloadTime = 1f;
    public Transform[] MuzzlePositions;
    public GameObject Missile;
    public Transform pivot_pan;
    public Transform pivot_tilt;


    private Transform Target;
    private float nextFireTime;
    
    
    void Start () 
    {
	
	}
	
	void Update () 
    {
	    if (Target != null)
	    {
	        //rotation
	        Vector3 aimPoint = Target.position - transform.position;
	        Quaternion desiredRot = Quaternion.LookRotation(aimPoint);

            pivot_tilt.rotation = Quaternion.Lerp(pivot_tilt.rotation, desiredRot, Time.deltaTime * turnSpeed);
            pivot_pan.rotation = Quaternion.Lerp(pivot_pan.rotation, desiredRot, Time.deltaTime * turnSpeed);

            pivot_pan.eulerAngles = new Vector3(0, pivot_pan.eulerAngles.y, 0);
            pivot_tilt.eulerAngles = new Vector3(pivot_tilt.eulerAngles.x, pivot_tilt.eulerAngles.y, 0);


            //fire
            if (Time.time >= nextFireTime)
	            fireProjectile();
	    }
	}
    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            nextFireTime = Time.time + reloadTime;
            Target = other.gameObject.transform;
        }
    }
    */

    void OnTriggerStay(Collider other)
    {
        if (!Target)
        {
            if (other.tag == "Air Enemy")
            {
                nextFireTime = Time.time + reloadTime;
                Target = other.gameObject.transform;
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == Target)
            Target = null;
    }

    void fireProjectile()
    {
        audio.Play();
        nextFireTime = Time.time + reloadTime;
        int m = Random.Range(0, 5);
        
        Instantiate(Missile, MuzzlePositions[m].position, MuzzlePositions[m].rotation);

        //Missile scripte target ver
        Missile_SAM missile = GetComponent("Missile_SAM") as Missile_SAM;
        Missile_SAM.Target = Target;
    }
}
