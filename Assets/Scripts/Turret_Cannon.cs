using UnityEngine;
using System.Collections;

public class Turret_Cannon : MonoBehaviour
{

    public GameObject Projectile;                   //Projectile to be instantiated
    public GameObject MuzzleEffects;
    private Transform Target;                       //Enemy
    public Transform turretBall;                    //The part the will be turned
    public Transform[] muzzlePositions;             //Positions for muzzle effects and projectiles to be instantiated

    public float turnSpeed = 10f;                   //How fast turret can turn around      
    public float reloadTime = 0.5f;                 //Reload time in seconds
    public float firePauseTime = .25f;              //the time the will take the turret to to fire. in this process it wont be able to turn
    public float errorAmount = .5f;                 //Error turret will make in aiming

    private float nextMoveTime;                     //will hold the time when it will be able to turn again
    private float nextFireTime;                     //will hold the time when it will be able to fire again
    private Quaternion desiredRotation;             //the new rotation at which the enemy exists.
    
    
    
    void Start () 
    {
	
	}
	
	
    void Update () 
    {
        //if there's a target in range
        if (Target != null)
        {
            //Are we able to turn again?
            if (Time.time >= nextMoveTime)
            {
                //then bring a new rotation
                CalculateAimPosition(Target.position);
                //and interpolate to that 
                turretBall.rotation = Quaternion.Lerp(turretBall.rotation, desiredRotation, Time.deltaTime*turnSpeed);

                //are we able to shoot?
                if (Time.time >= nextFireTime)
                {
                    fireProjectile();
                }
            }
        }

	}

    void OnTriggerEnter(Collider other)
    {
        if( other.tag == "enemy")
        {
            nextFireTime = Time.time + reloadTime;
            //This is our target
            Target = other.gameObject.transform;
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == Target)
            Target = null;

        turretBall.eulerAngles = Vector3.Lerp(turretBall.eulerAngles, new Vector3(0, 90, 0), Time.deltaTime*turnSpeed);
    }

    void fireProjectile()
    {
        audio.Play();
        nextFireTime = Time.time + reloadTime;
        nextMoveTime = Time.time + firePauseTime;

        foreach (Transform element in muzzlePositions)
        {
            Instantiate(Projectile, element.position, element.rotation);
            Instantiate(MuzzleEffects, element.position, element.rotation);
        }
    }

    void CalculateAimPosition(Vector3 targetPos)
    {
        float aimError = Random.Range(-errorAmount, errorAmount);
        var aimPoint = new Vector3(targetPos.x + aimError, targetPos.y + aimError, targetPos.z + aimError);
        desiredRotation = Quaternion.LookRotation(aimPoint);
    }


}
