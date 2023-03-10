using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{

    private int score;
    private TMP_Text ScoreText { get => AssetsHandler.Instance.scoreText; }

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    // Start is called before the first frame update
    void Start()
    {
        SavePosition();
        score = 0;
        ScoreText.text = "Collect coins to get points!".ToUpper();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        AssetsHandler.Instance.endScoreText.text = score.ToString();
        AssetsHandler.Instance.highScoreText.text = "999";
    }

    public async void Respawn(GameObject counter)
    {
        transform.position = lastPosition;
        transform.rotation = lastRotation;
        // SavePosition();
        GetComponent<PlayerMotor>().ChangeDirection();
        AssetsHandler.Instance.endUI.SetActive(false);
        Animator animator = GetComponent<Animator>();
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
        
    }

    public void CollectCoin(int score)
    {
        this.score += score;
        ScoreText.text = ("Score: " + this.score).ToUpper();
    }

    private async void SavePosition()
    {
        if (GameHandler.Instance.player.isStopped)
            return;

        await Task.Delay(2000);

        if (GameHandler.Instance.player.isStopped)
            return;

        lastPosition = transform.position;
        lastRotation = transform.rotation;
        SavePosition();
    }
}
