using UnityEngine;
using System.Collections;

public class EnemyControllerScript : MonoBehaviour 
{

    //public float walkingSpeed = .45f;
    //private bool walkingLeft = true;
    public TakeDamageFromPlayerBullet bulletColliderListener = null;
    public ParticleSystem deathFxParticlePrefab = null;

    //States to allow objects to know when an enemy dies
    public delegate void enemyEventHandler(int scoreMod);
    public static event enemyEventHandler enemyDied;

    void OnEnable()
    {
        //Subscribe to events from the bullet collider
        bulletColliderListener.hitByBullet += hitByPlayerBullet;
    }

    void OnDisable()
    {
        //Unsubscribe from events
        bulletColliderListener.hitByBullet -= hitByPlayerBullet;
    }

    public void hitByPlayerBullet()
    {

        //Get the enemy position
        Vector3 enemyPos = transform.position;

        //Create a new vector that is in front of the enemy
        Vector3 particlePosition = new Vector3(enemyPos.x, enemyPos.y, enemyPos.z + 1.0f);

        //Create the particle emitter object;
        /*Object deathFxParticle =*/ Instantiate(deathFxParticlePrefab,particlePosition,transform.rotation);
        
        //Reposition the particle emitter at this new position
        //deathFxParticle.transform.position = particlePosition;
        
        //Call the EnemyDied even and give it a score of 25
        if (enemyDied != null)
            enemyDied(25);
        //Wait a moment to ensure we are clear, then destroy the enemy object
        Destroy(gameObject, 0.1f);
    }

    
 
	// Use this for initialization
	
}
