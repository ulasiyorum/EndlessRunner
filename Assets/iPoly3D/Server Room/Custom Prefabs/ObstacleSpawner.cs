using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private GameObject[] Prefabs { get => AssetsHandler.Instance.Obstacles; }
    // Start is called before the first frame update
    void Start()
    {
        GenerateObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateObstacle()
    {
        int id = GroundMotor.roomID;

        if (id == 0)
        {
            int random = Random.Range(0, 4);

            if (Prefabs[random].tag == "ClosingWall")
            {
                GameObject obstacle = Instantiate(Prefabs[random]);
                obstacle.transform.parent = transform;
                obstacle.transform.position = transform.position;
                obstacle.transform.rotation = transform.rotation;
                obstacle.transform.Rotate(180, 0, 0);
            }
            else if (Prefabs[random].tag == "Laser")
            {
                GameObject obstacle = Instantiate(Prefabs[random]);
                obstacle.transform.parent = transform;
                obstacle.transform.position = new Vector3(transform.position.x, Random.Range(0.5f, 2f), transform.position.z);
                obstacle.transform.rotation = transform.rotation;
                obstacle.transform.Rotate(0, 90, 0);
            }
            else if (Prefabs[random].tag == "Desk")
            {
                GameObject obstacle = Instantiate(Prefabs[random]);
                obstacle.transform.parent = transform;
                obstacle.transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
                obstacle.transform.rotation = transform.rotation;
                obstacle.transform.Rotate(0, 90, 0);
            }
            else if (Prefabs[random].tag == "Server")
            {
                GameObject obstacle = Instantiate(Prefabs[random]);
                obstacle.transform.parent = transform;
                obstacle.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
                obstacle.transform.rotation = transform.rotation;
                obstacle.transform.Rotate(0, 90, 0);
            }
        } else if(id == 1)
        {
            int random = Random.Range(4, 8);

            if (Prefabs[random].tag == "ClosingWall")
            {
                GameObject obstacle = Instantiate(Prefabs[random]);
                obstacle.transform.parent = transform;
                obstacle.transform.position = transform.position;
                obstacle.transform.rotation = transform.rotation;
                obstacle.transform.Rotate(180, 0, 0);
            }
            else if (Prefabs[random].tag == "Laser")
            {
                GameObject obstacle = Instantiate(Prefabs[random]);
                obstacle.transform.parent = transform;
                obstacle.transform.position = new Vector3(transform.position.x, Random.Range(0.5f, 2f), transform.position.z);
                obstacle.transform.rotation = transform.rotation;
                obstacle.transform.Rotate(0, 90, 0);
            }
            else if (Prefabs[random].tag == "Web")
            {
                GameObject obstacle = Instantiate(Prefabs[random]);
                obstacle.transform.parent = transform;
                obstacle.transform.position = new Vector3(transform.position.x, 3.5f, transform.position.z);
                obstacle.transform.rotation = transform.rotation;
                obstacle.transform.Rotate(0, 0, 90);
            }
            else if (Prefabs[random].tag == "Spikes")
            {
                GameObject obstacle = Instantiate(Prefabs[random]);
                obstacle.transform.parent = transform;
                obstacle.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                obstacle.transform.rotation = transform.rotation;
                obstacle.transform.Rotate(0, 90, 0);
            }
        } else
        {
            int random = Random.Range(8, 12);

            if (Prefabs[random].tag == "ClosingWall")
            {
                GameObject obstacle = Instantiate(Prefabs[random]);
                obstacle.transform.parent = transform;
                obstacle.transform.position = transform.position;
                obstacle.transform.rotation = transform.rotation;
                obstacle.transform.Rotate(180, 0, 0);
            }
            else if (Prefabs[random].tag == "Barrier")
            {
                GameObject obstacle = Instantiate(Prefabs[random]);
                obstacle.transform.parent = transform;
                obstacle.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
                obstacle.transform.rotation = transform.rotation;
            }
            else if (Prefabs[random].tag == "Web")
            {
                GameObject obstacle = Instantiate(Prefabs[random]);
                obstacle.transform.parent = transform;
                obstacle.transform.position = new Vector3(transform.position.x, 3.5f, transform.position.z);
                obstacle.transform.rotation = transform.rotation;
                obstacle.transform.Rotate(0, 0, 90);
            }
            else if (Prefabs[random].tag == "Spikes")
            {
                GameObject obstacle = Instantiate(Prefabs[random]);
                obstacle.transform.parent = transform;
                obstacle.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                obstacle.transform.rotation = transform.rotation;
                obstacle.transform.Rotate(0, 90, 0);
            }
        }
    }
}
