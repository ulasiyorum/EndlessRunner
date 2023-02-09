using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{

    private int score;
    [SerializeField] Text scoreText;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "Collect coins to get points!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectCoin(int score)
    {
        this.score += score;
        scoreText.text = "Current Score: " + this.score;
    }
}
