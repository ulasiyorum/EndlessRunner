using System.Collections;
using System.Collections.Generic;
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



    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
        GroundMotor.currentAngle = -90;
        GroundMotor.currentCount = 0;
        VolumetricLineBehavior.Reset();
    }

    public void RespawnGame()
    {
        // Request ad and everything
        RespawnSuc();
    }

    private void RespawnSuc()
    {
        counter.SetActive(true);
        profile.Respawn(counter);
    }
}
