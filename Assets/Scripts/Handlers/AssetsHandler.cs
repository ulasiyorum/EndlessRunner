using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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


    public GameObject CoinPrefab { get => coinPrefab; }

    [SerializeField] GameObject coinPrefab;
    public GameObject[] GroundPrefabs { get => groundPrefabs; }
    [SerializeField] GameObject[] groundPrefabs;

    [SerializeField] GameObject[] obstacles;
    public GameObject[] Obstacles { get => obstacles; }
}
