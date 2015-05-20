using UnityEngine;
using System.Collections;

public class EnemySpawning : MonoBehaviour
{

    //A 2D array to contain the empty game objects that we use to get their positions to tell enemies where to swarm to
    GameObject[,] m_grid;
	//A 2D array that tells us whether or not the corresponding spots on m_grid currently have an enemy trying to go to them
	bool[,] m_bOccupied;
	//The spot that the grid's hub moves around in a circle
    Vector3 m_FocusPoint;
	//The direction the grid is going to be moving
    Vector3 m_Movement;
	//How fast the swarm grid moves
    float m_fSpeed;

    // Use this for initialization
    void Start()
    {
		//Initializing the sizes of m_bOccuppied and m_grid (we could just do this when we declare them
        m_bOccupied = new bool[6,10];
        m_grid = new GameObject[6, 10];

		//Looping through m_grid to assign all the empty game objects and set everything in m_bOccupied to false
		//This won't work if we change the dimensions of the grid
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                m_bOccupied[i, j] = false;
                m_grid[i, j] = transform.GetChild((i * 10) + j).gameObject;
            }
        }
		//set mFocusPoint to the spawn grid's intial position
        m_FocusPoint = transform.position;

		//Initialize m_Movement and m_fSpeed.  This could be done when we declare them
        m_Movement = new Vector3(5, 1, 0);

        m_fSpeed = 3.0f;
        //Debug.Break();

        
    }//END of Start()

    // Update is called once per frame
    void Update()
    {

        //Instantiate()

		//Moving around in a circle
        m_Movement += m_FocusPoint - transform.position;
        m_Movement.Normalize();
        m_Movement *= m_fSpeed;

        transform.position += (m_Movement * Time.deltaTime);
    }//END of Update()

	//Finding the next empty position in the grid
    public GameObject GetGridPosition()
    {

		//set these values as negative
        int row = -1;
        int col = -1;

		//Loop through m_bOccupied until you find an empty spot(false value)
		//Won't work if we change the dimensions of the grid
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (m_bOccupied[i, j] == false)
                {
                    m_bOccupied[i, j] = true;

                    row = i;
                    col = j;

                    break;
                }
            }

            if (row > -1)
			{
                break; 
			}
        }

        //Debug.Log("row: " + row);
        //Debug.Log("col: " + col);
        //Debug.Break();

        return m_grid[row, col];
	}//END of GetGridPosition()


	//These copies of the function don't seem to be getting any use.
//    public GameObject GetGridPosition(int number)
//    {
//        m_bOccupied[number / 6, number % 6] = true;
//        return m_grid[number / 6, number % 6];
//    }
//
//    public GameObject GetGridPosition(int row, int col)
//    {
//        m_bOccupied[row, col] = true;
//        return m_grid[row, col];
//    }



}
