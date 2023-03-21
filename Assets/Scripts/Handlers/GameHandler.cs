using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VolumetricLines;

public class GameHandler : MonoBehaviour
{
    private static GameHandler i;
    public static GameHandler Instance
    {
        get
        {
            if (i == null)
                i = FindObjectOfType<GameHandler>();

            return i;
        }
    }

    [SerializeField] GameObject counter;

    public Canvas canvas;
    public PlayerMotor player;
    public PlayerProfile profile;
    public CameraMotor cameraMotor;

    public async void MainMenu()
    {
        player.GameOver();
        await Task.Delay(15);
        SceneManager.LoadScene("Start");
        await Task.Delay(200);
        Potions.Reset();
        GroundMotor.roomCounter = 0;
        GroundMotor.currentAngle = -90;
        GroundMotor.currentCount = 0;
        CoinBehaviour.Reset();
        VolumetricLineBehavior.Reset();
    }

    public async void RestartGame()
    {
        player.GameOver();
        await Task.Delay(15);
        SceneManager.LoadScene("SampleScene");
        await Task.Delay(200);
        CoinBehaviour.Reset();
        Potions.Reset();
        GroundMotor.currentAngle = -90;
        GroundMotor.roomCounter = 0;
        GroundMotor.currentCount = 0;
        VolumetricLineBehavior.Reset();
    }

    public void RespawnGame()
    {
        // Request ad and everything
        Rewarded.SetReward(RespawnSuc);
        Rewarded.instance.ShowRewardedAd();
    }

    private void RespawnSuc()
    {
        counter.SetActive(true);
        profile.Respawn(counter);
        Potions.SetInvincible(3);
    }
}
