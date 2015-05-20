using UnityEngine;
using System.Collections;

public class UnleashTheHorde : MonoBehaviour
{
	//The speed at which enemies are spawned
    public float m_fInterval;
	//How long to wait before enemies are first spawned
    public float m_fWave;
	//Which type of enemy to spawn
    public GameObject m_ToSpawn;
	//How many enemies to spawn per wave
    private int m_nCounter;

    // Use this for initialization
    void Start()
    {
        m_fInterval = 1.0f;
		m_fWave = Random.Range (1, 30);
    }

    // Update is called once per frame
    void Update()
    {
        m_fWave -= Time.deltaTime;

        if (m_fWave <= 0.0f)
        {
            Spawning();
        }
    }

    public void Spawning()
    {
        m_fInterval -= Time.deltaTime;

        if (m_fInterval <= 0.0f)
        {
            Instantiate(m_ToSpawn, transform.position, Quaternion.identity);
            m_nCounter++;
            m_fInterval += 1.0f;

            if (m_nCounter >= 5)
            {
                m_nCounter = 0;
                //m_bSpawning = false;
				m_fWave = Random.Range(30, 60);
            }
        }
    }
}
