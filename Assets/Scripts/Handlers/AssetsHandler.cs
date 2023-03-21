using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssetsHandler : MonoBehaviour
{
    private static AssetsHandler i;

    public static AssetsHandler Instance
    {
        get
        {
            if (i == null)
                i = FindObjectOfType<AssetsHandler>();

            return i;
        }
    }

    public GameObject endUI;
    public TMP_Text scoreText;
    public TMP_Text endScoreText;
    public TMP_Text highScoreText;
    public TMP_Text balanceText;
    public GameObject popUpPrefab;
    public GroundMotor[] prefabMotors;
    public GameObject popUpNormalPrefab;
    public Material[] agentMaterials;

    public GameObject[] agents;


    private void Start()
    {
        prefabMotors = new GroundMotor[groundPrefabs.Length];
        for (int i = 0; i < groundPrefabs.Length; i++)
        {
            prefabMotors[i] = groundPrefabs[i].GetComponentInChildren<GroundMotor>();
        }
    }

    public GameObject CoinPrefab { get => coinPrefab; }

    [SerializeField] GameObject coinPrefab;
    public GameObject[] GroundPrefabs { get => groundPrefabs; }
    [SerializeField] GameObject[] groundPrefabs;

    [SerializeField] GameObject[] obstacles;
    public GameObject[] Obstacles { get => obstacles; }
}
