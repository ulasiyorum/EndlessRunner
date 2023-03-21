using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinChanger : MonoBehaviour
{
    public static int selectedSkin = 0;
    public static bool[] skins = new bool[2];
    private GameObject[] skinObj;
    [SerializeField] CinemachineVirtualCamera cam;

    private void Awake()
    {
        selectedSkin = MainMenuManager.current.selected;
        if (SceneManager.GetActiveScene().name != "SampleScene")
            return;

        skinObj = AssetsHandler.Instance.agents;

        GameObject go = Instantiate(skinObj[selectedSkin]);
        go.transform.position = new Vector3(0, 0, 4.15f);
        cam.LookAt = go.transform;
        cam.Follow = go.transform;
        GameHandler.Instance.player = go.GetComponent<PlayerMotor>();
        GameHandler.Instance.profile = go.GetComponent<PlayerProfile>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
