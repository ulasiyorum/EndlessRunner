using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Canvas canvas;
    public PlayerMotor player;
    public PlayerProfile profile;
    public CameraMotor cameraMotor;
}
