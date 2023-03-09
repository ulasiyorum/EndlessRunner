using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundMotor : MonoBehaviour
{
    private static int roomCounter = 0;
    private int roomNumber = 0;
    public static int currentAngle = -90;

    private static int currentCount = 0;
    public static int roomCount = 3;
    public static GameObject latestObj;

    public Transform parent;
    public enum Type
    {
        frontOpen,
        backOpen,
        leftOpen,
        rightOpen,
        none
    };

    public Type type;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        roomNumber = roomCounter;
        roomCounter++;
        latestObj = parent.gameObject;
        CoinBehaviour.CoinDirection dir = CoinBehaviour.DecideAxis(currentAngle);
        CoinBehaviour.GenerateCoins(Random.Range(2, 6), latestObj.transform, transform.rotation, roomNumber,dir);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        Generate();
        currentCount--;
    }

    public void Generate()
    {
        if (latestObj == null)
            latestObj = parent.gameObject;

        int nextAngle = DecideAngle();
        Type nextType = DecideType();

        GameObject go = Instantiate(FindPrefab(nextType, id));

        float pivotSize = DecidePivotSize(nextAngle, go);

        float size = go.GetComponent<BoxCollider>().bounds.size.x;

        go.transform.position = CalculateNextRoomPosition(pivotSize,size);

        go.transform.rotation = Quaternion.AngleAxis(nextAngle, Vector3.up) * latestObj.transform.rotation;


        currentAngle += nextAngle;

        if (currentAngle >= 180)
            currentAngle -= 360;
        else if (currentAngle < -180)
            currentAngle += 360;

        latestObj = go;

        currentCount++;
        if (currentCount < roomCount)
            LatestMotor.Generate();

        UnloadObject(parent.gameObject);
    }

    private async void UnloadObject(GameObject go)
    {
        await Task.Delay(2860 * currentCount);
        if (!GameHandler.Instance.player.isStopped)
            Destroy(go);
        else
            StartCoroutine(UnloadObj(go));
    }

    private static bool HasStopped()
    {
        return GameHandler.Instance.player.isStopped;
    }
    private IEnumerator UnloadObj(GameObject go)
    {
        yield return new WaitWhile(HasStopped);
        UnloadObject(go);
    }

    private Vector3 CalculateNextRoomPosition(float pivotSize, float size)
    {
        Vector3 nextPos = Vector3.zero;
        Vector3 v = latestObj.transform.position;
        Type type = LatestMotor.type;

        if (type == Type.leftOpen && currentAngle == -90)
            nextPos = new Vector3(v.x - pivotSize, v.y, v.z + size - pivotSize);
        else if (type == Type.rightOpen && currentAngle == -90)
            nextPos = new Vector3(v.x + pivotSize, v.y, v.z + size - pivotSize);
        else if ((type == Type.backOpen || type == Type.frontOpen) && currentAngle == -90)
            nextPos = new Vector3(v.x + pivotSize, v.y, v.z + size);
        else if (currentAngle == 90 && type == Type.leftOpen)
            nextPos = new Vector3(v.x + pivotSize, v.y, v.z - size + pivotSize);
        else if (currentAngle == 90 && type == Type.rightOpen)
            nextPos = new Vector3(v.x - pivotSize, v.y, v.z - size + pivotSize);
        else if (currentAngle == 90 && (type == Type.backOpen || type == Type.frontOpen))
            nextPos = new Vector3(v.x - pivotSize, v.y, v.z - size);
        else if (currentAngle == 0 && type == Type.leftOpen)
            nextPos = new Vector3(v.x + size - pivotSize, v.y, v.z + pivotSize);
        else if(currentAngle == 0 && type == Type.rightOpen)
            nextPos = new Vector3(v.x + size - pivotSize, v.y, v.z - pivotSize);
        else if (currentAngle == 0 && (type == Type.backOpen || type == Type.frontOpen))
            nextPos = new Vector3(v.x + size, v.y, v.z - pivotSize);
        else if(currentAngle == -180 && type == Type.leftOpen)
            nextPos = new Vector3(v.x - size + pivotSize, v.y, v.z - pivotSize);
        else if(currentAngle == -180 && type == Type.rightOpen)
            nextPos = new Vector3(v.x - size + pivotSize, v.y, v.z + pivotSize);
        else if (currentAngle == -180 && (type == Type.backOpen || type == Type.frontOpen))
            nextPos = new Vector3(v.x - size + pivotSize, v.y, v.z);

        return nextPos;
    }

    private float DecidePivotSize(float angle, GameObject next)
    {
        if (angle == 0)
            return 0;

        float size = next.GetComponent<BoxCollider>().bounds.size.z;

        return size / 2;
    }

    private static GameObject FindPrefab(Type type, int id)
    {
        GameObject[] prefabs = AssetsHandler.Instance.GroundPrefabs;

        foreach(GameObject prefab in prefabs)
        {
            GroundMotor motor = prefab.GetComponentInChildren<GroundMotor>();
            if(motor.type == type && motor.id == id)
            {
                return prefab;
            }
        }

        throw new System.Exception("Ground Prefab Cannot Be Found");
    }

    private Type DecideType()
    {
        int random = Random.Range(0, 4);

        if(random == 0)
        {
            return Type.leftOpen;
        }
        else if(random == 1)
        {
            return Type.rightOpen;
        }
        else if (random == 2 && type != Type.backOpen)
        {
            return type;
        }
        else
        {
            return Type.frontOpen;
        }
    }

    private static GroundMotor LatestMotor { get => latestObj.GetComponentInChildren<GroundMotor>(); }


    private int DecideAngle()
    {
        Type current = LatestMotor.type;
        int angle;
        switch (current)
        {
            case Type.backOpen:
                angle = 0;
            break;

            case Type.leftOpen:
                angle = -90;
            break;

            case Type.rightOpen:
                angle = 90;
            break;
            
            default:
                angle = 0;
            break;
        }
        return angle;
    }
}
