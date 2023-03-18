using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Potions : MonoBehaviour
{
    private static int doubleScore = 1;
    private float timer = 0;
    private TMP_Text timerText;
    private static int doubleCoins = 1;
    private static float swiftness = 1;
    private static bool invicible;
    private bool potionActive = false;
    private static int potionID;
    public static bool Invicible { get => invicible; }
    public static float Swiftness { get => swiftness; }

    public static int DoubleCoins { get => doubleCoins; }
    public static int DoubleScore { get => doubleScore; }

    public static int[] PotionInventory { get => potionInventory; }
    public static void SetPotionInventory(DBUser current)
    {
        potionInventory = current.potions;
    }

    private static Potions instance;
    private static int[] potionInventory; // 0 => score 1 => coins 2 => invis 3 => swift
    private readonly int[] potionPrices = { 120,120,240,50 };
    private readonly int[] potionDurations = { 30,9999,4,8 };
    [SerializeField] GameObject[] timers;
    [SerializeField] TMP_Text[] counts;

    [SerializeField] Material mat;
    [SerializeField] Material invisMat;
    private PlayerProfile Player { get => GameHandler.Instance.profile; }

    private void Awake()
    {
        potionInventory = new int[4];
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (counts == null)
            return;

        if (MainMenuManager.current != null)
            potionInventory = MainMenuManager.current.potions;

        int i = 0;
        foreach (TMP_Text text in counts)
        {
            text.text = "" + potionInventory[i];
            i++;
        }
    }

    public static void Reset()
    {
        doubleCoins = 1;
        doubleScore = 1;
        swiftness = 1;
        invicible = false;
        instance.Player.GetComponentInChildren<Renderer>().material = instance.mat;
        instance.timers[potionID].SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        if(potionActive)
        {
            timer += Time.deltaTime;
            timerText.text = ((int)(potionDurations[potionID] - timer)).ToString();
            if(timer >= potionDurations[potionID])
            {
                timer = 0;
                potionActive = false;
                doubleCoins = 1;
                doubleScore = 1;
                swiftness = 1;
                invicible = false;
                Player.GetComponentInChildren<Renderer>().material = mat;
                timers[potionID].SetActive(false);
            }
        }
    }

    public void UsePotion(int id)
    {
        // use potion sound
        if (potionInventory[id] <= 0)
        {
            StartPopUpMessage.Message("You don't have the potion!", Color.red);
            return;
        }
        if(potionActive)
        {
            StartPopUpMessage.Message("You already have a potion active", Color.yellow);
            return;
        }

        timers[id].SetActive(true);
        potionInventory[id]--;
        timerText = timers[id].GetComponentInChildren<TMP_Text>();
        potionActive = true;
        potionID = id;

        int i = 0;
        foreach(TMP_Text text in counts)
        {
            text.text = "" + potionInventory[i];
            i++;
        }

        switch(id)
        {
            case 0:
                doubleScore = 2;
                break;
            case 1:
                doubleCoins = 2;
                break;
            case 2:
                invicible = true;
                Player.GetComponentInChildren<Renderer>().material = invisMat;
                break;
            case 3:
                swiftness = 1.25f;
                break;
        }

        AudioManager.PlayOneShot(5);
    }

    public static async void SetInvincible(int sec)
    {
        invicible = true;
        instance.Player.GetComponentInChildren<Renderer>().material = instance.invisMat;
        await Task.Delay((sec + 3) * 1000);
        invicible = false;
        instance.Player.GetComponentInChildren<Renderer>().material = instance.mat;
    }

    public void BuyPotion(int id)
    {
        if(MainMenuManager.current.balance < potionPrices[id])
        {
            StartPopUpMessage.Message("Not enough balance", Color.red);
            return;
        }
        StartPopUpMessage.Message("Bought for " + potionPrices[id], Color.green);
        potionInventory[id]++;
        MainMenuManager.current.balance -= potionPrices[id];
        FirebaseManager.SendScore(MainMenuManager.current.name, 0, new System.DateTime(), MainMenuManager.current.balance, potionInventory, MainMenuManager.current.points, MainMenuManager.current.progressStart, MainMenuManager.current.claimed); ;
        MainMenuManager.instance.ChangeBalance();
        Handheld.Vibrate();
        AudioManager.PlayOneShot(4);
    }
}
