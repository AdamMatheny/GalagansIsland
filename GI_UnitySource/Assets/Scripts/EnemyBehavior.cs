using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{

    public Vector2 m_vVel;
    public Transform m_Swarm;
    public Transform m_Player;
    public bool m_bHasLooped;
    public float m_fSpeed;
    public Vector3 m_stopLoopPos;
    public float m_fSwitchCoolDown;
    public GameObject m_SwarmGridPosition;
    bool m_bGrabber;
    public float m_fAttackTimer;
    public GameObject m_DeathEffect;
    public Canvas m_HUD;


    enum AIState
    {
        eLoop, eApproach, eSwarm, eAttack
    };

    [SerializeField] private AIState m_eCurrentState;

    // Use this for initialization
    void Start()
    {
        m_vVel = new Vector2(0, 1);
        m_Swarm = GameObject.FindGameObjectWithTag("Swarm").transform;
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_bHasLooped = false;
        m_eCurrentState = AIState.eApproach;
        m_fSpeed = 5.0f;
        m_stopLoopPos = Vector3.zero;
        m_SwarmGridPosition = null;
        m_bGrabber = false;
        m_fAttackTimer = 5.0f;

    }

    // Update is called once per frame
    void Update()
    {
        m_fSwitchCoolDown -= Time.deltaTime;
        //Debug.Log(m_eCurrentState);

        switch (m_eCurrentState)
        {
            case AIState.eLoop:
                Summersalt();
                break;

            case AIState.eApproach:
                MoveToAllies();
                break;

            case AIState.eSwarm:
                PartOfTheSwarm();
                break;

            case AIState.eAttack:
                AttackPlayer();
                break;

            default:
                break;
        }
    }

    void MoveToAllies()
    {
        Vector3 toSwarm;
        float dist;

        if (m_bHasLooped == false)
        {
            toSwarm = m_Swarm.position - transform.position;
        }
        else
        {
            toSwarm = m_SwarmGridPosition.transform.position - transform.position;
        }

        dist = toSwarm.magnitude;
        toSwarm.Normalize();

        transform.up += toSwarm;
        transform.up.Normalize();

        GetComponent<Rigidbody>().velocity = transform.up * m_fSpeed;


        //Transitions to other states


        if (m_fSwitchCoolDown <= 0.0f && (m_bHasLooped == false && (transform.position.x > 0.7f || transform.position.x < 0.7f)))
        {
            m_eCurrentState = AIState.eLoop;
            m_stopLoopPos = transform.position;
            EnemySpawning spawnScript = m_Swarm.gameObject.GetComponent<EnemySpawning>();
            m_SwarmGridPosition = spawnScript.GetGridPosition();
            m_fSwitchCoolDown = 1.0f;
        }

        else if (m_fSwitchCoolDown <= 0.0f && (dist < 0.5f))
        {
            m_eCurrentState = AIState.eSwarm;
            m_fSwitchCoolDown = 1.0f;
        }

    }

    void Summersalt()
    {
        m_vVel += new Vector2(transform.right.x, transform.right.y) * Time.deltaTime;

        m_vVel.Normalize();
        transform.up += new Vector3(m_vVel.x, m_vVel.y, 0);
        transform.up.Normalize();

        GetComponent<Rigidbody>().velocity = transform.up * m_fSpeed;

        Vector3 toSpot = m_SwarmGridPosition.transform.position - transform.position;
        toSpot.Normalize();
        float difference = Vector3.Dot(toSpot, transform.up);


        //Transitions to other states
        if (m_fSwitchCoolDown <= 0.0f && (difference > 0.97f))
        {
            m_bHasLooped = true;
            m_eCurrentState = AIState.eApproach;
            m_fSwitchCoolDown = 0.5f;
        }
    }

    void AttackPlayer()
    {
        Vector3 toPlayer = m_Player.transform.position - transform.position;
        toPlayer.Normalize();
        Vector3 vel = transform.gameObject.GetComponent<Rigidbody>().velocity;

        vel += toPlayer;
        vel.Normalize();
        vel *= m_fSpeed;

        transform.gameObject.GetComponent<Rigidbody>().velocity = vel;

        if (m_bGrabber)
        {

        }

        if (transform.position.y <= -30.0f
            || transform.position.x >= 30.0f
            || transform.position.x <= -30.0f)
        {
            //Out of bounds, delete self
            Destroy(gameObject);
        }
    }

    void PartOfTheSwarm()
    {
        m_fAttackTimer -= Time.deltaTime;

        transform.position = m_SwarmGridPosition.transform.position;
        transform.up = m_SwarmGridPosition.transform.up;

        //If the player is in front of the enemy, there's a chance it'll fire

        Vector3 toPlayer = m_Player.transform.position - transform.position;
        toPlayer.Normalize();
        float dotPlayer = Vector3.Dot(toPlayer, transform.up);

        if (dotPlayer > 0.9f && Random.Range(1, 10) > 7)
        {
            //Attack the player
            if (m_bGrabber)
            {

            }
            else
            {
                //Instantiate a bullet and fire it

            }
        }

        if (m_fAttackTimer <= 0.0f)
        {
            m_eCurrentState = AIState.eAttack;
            m_fSwitchCoolDown = 1.0f;
        }

    }

    void OnCollisionEnter(Collision other)
    {
        Instantiate(m_DeathEffect, transform.position, Quaternion.identity);
        
        //m_HUD.GetComponent<ScoreWatcher>().addScore(25);
		//Debug.Break ();
		if (other.gameObject.tag != "Player")
        	Destroy(other.gameObject);

        Destroy(gameObject);
    }

}
