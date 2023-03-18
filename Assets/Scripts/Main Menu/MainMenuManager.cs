using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] TMP_InputField input;
    [SerializeField] GameObject nameField;
    [SerializeField] TMP_Text balance;
    [SerializeField] GameObject progressBar;
    public static DBUser current;
    public static MainMenuManager instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public void Start()
    {
        string name = PlayerPrefs.GetString("name", "");
        if (string.IsNullOrEmpty(name))
            name = FirebaseManager._name;
        nameField.SetActive(string.IsNullOrEmpty(name));
        current = FirebaseManager.Find(name, FirebaseManager.db);
        if (current != null)
        {
            balance.text = current.balance.ToString();
            foreach (int sc in current.score)
            {
                if (sc > PlayerPrefs.GetInt("HighScore", 0))
                    PlayerPrefs.SetInt("HighScore", sc);
            }
            Potions.SetPotionInventory(current);
            progressBar.SetActive(true);
        }
        else
            balance.text = "0";
    }

    public void ChangeBalance()
    {
        balance.text = current.balance.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoGuest()
    {
        DBUser[] users = FirebaseManager.db;
        if (users == null)
            users = FirebaseManager.Fetch();

        string name = "Player " + UnityEngine.Random.Range(0,9999);
        if (users != null)
            foreach (var user in users)
            {
                if (user.name == name)
                {
                    GoGuest();
                    return;
                }
            }

        nameField.SetActive(false);
        StartPopUpMessage.Message("Welcome " + name, Color.green);
        PlayerPrefs.SetString("name", name);
        FirebaseManager.CreateUser(new DBUser(new int[4],new System.DateTime[4],0,name,new int[4],0,DateTime.Now, false));
    }

    public void SetName()
    {
        DBUser[] users = FirebaseManager.db;
        if (users == null)
            users = FirebaseManager.Fetch();

        string name = input.text;

        if(users != null)
            foreach(var user in users)
            {
                if(user.name == name)
                {
                    StartPopUpMessage.Message("That name is taken!", Color.red);
                    return;
                }
            }
        nameField.SetActive(false);
        StartPopUpMessage.Message("Welcome " + name, Color.green);
        PlayerPrefs.SetString("name", name);
        FirebaseManager.CreateUser(new DBUser(new int[4], new System.DateTime[4], 0, name, new int[4],0,DateTime.Now,false));
    }
    public async void StartGame()
    {
        await Task.Delay(1000);
        SceneManager.LoadScene("SampleScene");
    }
}
