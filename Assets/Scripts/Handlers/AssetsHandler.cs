using System.Collections;
using System.Collections.Generic;
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

    public GameObject CoinPrefab { get => coinPrefab; }

    [SerializeField] GameObject coinPrefab;
    public GameObject[] GroundPrefabs { get => groundPrefabs; }
    [SerializeField] GameObject[] groundPrefabs;
}
