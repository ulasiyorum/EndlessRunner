using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    private int roomNumber = 0;
    private CoinDirection direction;

    private float up = 0f;

    public enum CoinDirection
    {
        xAxisNegative,
        xAxisPositive,
        zAxisNegative,
        zAxisPositive
    }

    public static CoinDirection currentDirection;

    private static Dictionary<int,Stack<CoinBehaviour>> coins = new Dictionary<int,Stack<CoinBehaviour>>();

    public static void Reset()
    {
        coins = new Dictionary<int, Stack<CoinBehaviour>>();
    }

    void Start()
    {
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        GameHandler.Instance.profile.CollectCoin(1);
        AudioManager.PlayCollect();
        if (IsLast(gameObject, roomNumber))
            ClearCoins(roomNumber);
        else
            Destroy(gameObject);
        
    }

    private static bool IsLast(GameObject go, int numb)
    {
        try
        {
            return coins[numb].Peek() == go;
        }
        catch
        {
            return false;
        }
    }
    public static CoinDirection DecideAxis(int angle)
    {
        switch(angle)
        {
            case 0:
                currentDirection = CoinDirection.xAxisPositive;
                break;
            case -180:
                currentDirection = CoinDirection.xAxisNegative;
                break;
            case -90:
                currentDirection = CoinDirection.zAxisPositive;
                break;
            case 90:
                currentDirection = CoinDirection.zAxisNegative;
                break;
        }

        return currentDirection;
    }

    public static void ClearCoins(int currentRoom)
    {
        if (coins[currentRoom].Count > 0)
        {
            CoinBehaviour coin = coins[currentRoom].Pop();
            coins[currentRoom].Clear();
            Destroy(coin.transform.parent.gameObject, 0.01f);
        }

    }


    public static void GenerateCoins(int count, Transform startPosition, Quaternion rotation, int current, CoinDirection direction)
    {
        if (!coins.ContainsKey(current))
            coins[current] = new Stack<CoinBehaviour>();

        GameObject parent = new GameObject("coinsParent");

        for(int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(AssetsHandler.Instance.CoinPrefab);

            go.GetComponent<CoinBehaviour>().roomNumber = current;
            go.GetComponent<CoinBehaviour>().direction = direction;

            if (coins[current].Count == 0)
            {
                go.transform.position += new Vector3(0,1.25f,0);
            }
            else
            {
                Transform last = coins[current].Peek().transform;
                go.transform.position = last.transform.position;
            }



            float zSize = go.GetComponent<MeshCollider>().bounds.size.z * 16;

            if (direction == CoinDirection.zAxisPositive)
            {
                go.transform.position += new Vector3(0, 0, zSize);

            }
            else if(direction == CoinDirection.zAxisNegative)
            {
                go.transform.position += new Vector3(0, 0, -1 * zSize);

            }
            else if(direction == CoinDirection.xAxisPositive)
            {
                go.transform.position += new Vector3(zSize, 0, 0);

            } 
            else if(direction == CoinDirection.xAxisNegative)
            {
                go.transform.position += new Vector3(zSize, 0, 0);
            }
            go.transform.parent = parent.transform;


            coins[current].Push(go.GetComponent<CoinBehaviour>());
        }

        parent.transform.position = startPosition.position;
        parent.transform.rotation = rotation;

        if (direction == CoinDirection.zAxisPositive)
            parent.transform.Rotate(0, 90, 0);
        else if (direction == CoinDirection.xAxisPositive)
            parent.transform.Rotate(0, 180, 0);
        else if (direction == CoinDirection.zAxisNegative)
            parent.transform.Rotate(0, -90, 0);
        else
            parent.transform.Rotate(0, 180, 0);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,Time.deltaTime * 200f);

        if (up + Time.deltaTime < 0.8f)
        {
            transform.position += new Vector3(0, Time.deltaTime/1.125f, 0);
            up += Time.deltaTime;
        } 
        else if(up + Time.deltaTime < 1.6f)
        {
            transform.position -= new Vector3(0, Time.deltaTime/1.125f, 0);
            up += Time.deltaTime;
        } else
        {
            up = 0;
        }

    }
    

}
