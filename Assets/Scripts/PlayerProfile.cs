using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerProfile : MonoBehaviour
{
    public int balance;
    public int highestScore;

    public bool claimed;
    public int points;
    public DateTime progressStart;
    [SerializeField] Animator animator;
    private int score;
    private double sc;

    private int matIndex;

    public int Score { get => score; }

    private TMP_Text ScoreText { get => AssetsHandler.Instance.scoreText; }
    private TMP_Text BalanceText { get => AssetsHandler.Instance.balanceText; }

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    private Vector3 lastAnimPosition;
    private Quaternion lastAnimRotation;
    // Start is called before the first frame update
    void Start()
    {
        SavePosition();
        string name = PlayerPrefs.GetString("name", "");
        if (string.IsNullOrEmpty(name))
            name = FirebaseManager._name;

        DBUser current = FirebaseManager.Find(name, FirebaseManager.db);
        if (current != null)
        {
            balance = current.balance;
            points = current.points;
            progressStart = current.progressStart;
            claimed = current.claimed;
            foreach (int sc in current.score)
            {
                if (sc > PlayerPrefs.GetInt("HighScore", 0))
                    PlayerPrefs.SetInt("HighScore", sc);
            }
            Potions.SetPotionInventory(current);
        }

        score = 0;
        ScoreText.text = "STARTING".ToUpper();
        highestScore = PlayerPrefs.GetInt("HighScore",0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;
        score = (int)sc;
        AssetsHandler.Instance.endScoreText.text = score.ToString();
        if (highestScore > score)
            AssetsHandler.Instance.highScoreText.text = highestScore.ToString();
        else
            AssetsHandler.Instance.highScoreText.text = score.ToString();

    }
    private PlayerMotor motor;



    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
    }
    public async void Respawn(GameObject counter)
    {
        transform.position = lastPosition;
        transform.rotation = lastRotation;
        // SavePosition();
        GetComponent<PlayerMotor>().ChangeDirection();
        AssetsHandler.Instance.endUI.SetActive(false);
        animator.SetTrigger("respawn");
        await Task.Delay(400);
        animator.enabled = false;
        await Task.Delay(2600);
        animator.enabled = true;
        counter.SetActive(false);
        GameHandler.Instance.player.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (motor.isStopped)
            return;

        ScoreText.text = ("Score: " + (int)sc).ToUpper();
        BalanceText.text = balance.ToString();


        if (!motor.isStopped)
            sc += Time.deltaTime * (GroundMotor.roomCounter+1 / 13) * Potions.DoubleScore;
    }

    public void CollectCoin(int score)
    {
        balance += score * Potions.DoubleCoins;
    }

    private async void SavePosition()
    {
        try
        {
            if (GameHandler.Instance.player.isStopped)
                return;

            await Task.Delay(2000);

            if (GameHandler.Instance.player.isStopped)
                return;
            lastAnimPosition = animator.transform.position;
            lastAnimRotation = animator.transform.rotation;
            lastPosition = transform.position;
            lastRotation = transform.rotation;
            SavePosition();
        } catch
        {
            Debug.Log("Can't Save Position");
        }
    }

}
