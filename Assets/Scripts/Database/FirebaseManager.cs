using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
using Newtonsoft;
using Newtonsoft.Json;
using Firebase.Extensions;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Security.Claims;
using UnityEditor;

public class FirebaseManager : MonoBehaviour
{
    public static DBUser[] db;
    private static MonoBehaviour instance;
    private static string userID;
    public static string _name;
    public static DatabaseReference reference;

    private DependencyStatus dependencyStatus;
    private FirebaseApp app;


    private void Awake()
    {
        instance = this;


        AppOptions dbOptions = new AppOptions
        {
            DatabaseUrl = new System.Uri("https://agentrunner07-default-rtdb.europe-west1.firebasedatabase.app/"),
            ApiKey = "AIzaSyBG54aCpXze6x0vI9n_DbDKtImu2hIuS74",
            AppId = "1:961378080326:android:91e4c944e766f7b6bf1c47",
            ProjectId = "agentrunner07",
            StorageBucket = "agentrunner07.appspot.com",
            MessageSenderId = "961378080326"
        };
        app = FirebaseApp.Create(dbOptions);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                reference = FirebaseDatabase.GetInstance(app).RootReference;
                db = Fetch();
                _name = PlayerPrefs.GetString("name","");
            }
            else
            {

            }
        });


    }
    void Start()
    {

        userID = SystemInfo.deviceUniqueIdentifier;


    }


    public static void CreateUser(string name,int score,DateTime time, int[] potions, int balance, int points, DateTime startProgress, bool claimed)
    {
        DBUser[] users = Fetch();

        DBUser user = Find(name,users);

        if(user == null) 
        {
            user = new DBUser(new int[0], new DateTime[0],balance, name, potions, points, startProgress, claimed).Init(name);
        } else
        {
            user.Add(score, time);
        }

        string js = JsonConvert.SerializeObject(user);
        reference.Child("users").Child(userID).SetRawJsonValueAsync(js);
        PutValue(user, users);
    }

    public static void CreateUser(DBUser user)
    {
        string js = JsonConvert.SerializeObject(user);
        reference.Child("users").Child(userID).SetRawJsonValueAsync(js);
    }


    private static void PutValue(DBUser user, DBUser[] users)
    {
        if (users == null || user == null)
            return;
        
        for(int i = 0; i < users.Length; i++)
        {
            if (users[i].name == user.name)
            {
                users[i] = user;
                return;
            }
        }
    }

    public static DBUser Find(string name, DBUser[] users)
    {
        if (string.IsNullOrEmpty(name))
            return null;
        
        if (users == null)
            return null;


        foreach(DBUser user in users)
        {
            if (user.name == name)
                return user;
        }
        return null;
    }


    public static IEnumerator GetUsers(Action<string[]> onCallback)
    {

        var userData = reference.Child("users").GetValueAsync();

        yield return new WaitUntil(predicate: () => userData.IsCompleted);

        if (userData.Result.Value != null)
        {
            List<string> jsArr = new List<string>();
            DataSnapshot sh = userData.Result;
            if (sh.ChildrenCount == 0)
            {
                onCallback.Invoke(new string[0]);
            }
            else
            {
                var list = sh.Value as Dictionary<string, object>;
                int i = 0;

                if(string.IsNullOrEmpty(PlayerPrefs.GetString("name","")))
                {
                    foreach(KeyValuePair<string,object> id in list)
                    {
                        if (id.Key == SystemInfo.deviceUniqueIdentifier)
                        {
                            string value = JsonConvert.SerializeObject(id.Value);
                            DBUser user = new DBUser(new int[4], new DateTime[4], 0, "", new int[4], 0, DateTime.Now, false);
                            JsonConvert.PopulateObject(value, user);
                            _name = user.name;
                            
                            PlayerPrefs.SetString("name", _name);
                        }
                    }
                }


                foreach (KeyValuePair<string, object> item in list)
                {
                    jsArr.Add("");
                    jsArr[i] = JsonConvert.SerializeObject(item.Value);
                    i++;
                }

                onCallback.Invoke(jsArr.ToArray());
            }
        }
    }

    public static void SendPlayer(DBUser player)
    {
        string js = JsonConvert.SerializeObject(player);
        reference.Child("users").Child(userID).SetRawJsonValueAsync(js);
    }


    public static DBUser[] Fetch()
    {
        return FetchFromDB();

        string fetchedTime = PlayerPrefs.GetString("fetched","");

        if (string.IsNullOrEmpty(fetchedTime))
        {
            Debug.Log("Empty Fetch!");
            return FetchFromDB();
        }

        string[] time = fetchedTime.Split('-');

        if (int.Parse(time[1]) < DateTime.Now.Day)
        {
            Debug.Log("Day Fetch!");
            return FetchFromDB();
        }
        else if (DateTime.Now.Hour - int.Parse(time[0]) > 4)
        {
            Debug.Log("Hour Fetch!");
            return FetchFromDB();
        }
        else if(!LocalSave.FileExists)
        {
            Debug.Log("Non-Local Fetch!");
            return FetchFromDB();
        }
        else
        {
            Debug.Log("Local Fetch!");
            return FetchFromLocal();
        }
    }


    private static DBUser[] FetchFromLocal()
    {
        db = LocalSave.LoadLocally();

        return db;
    }


    private static DBUser[] FetchFromDB()
    {
        instance.StartCoroutine(GetUsers((string[] users) =>
        {
            db = new DBUser[users.Length];
            for (int i = 0; i < users.Length; i++)
            {
                db[i] = new DBUser(new int[4], new DateTime[4], 0, "", new int[4], 0, DateTime.Now, false);
                JsonConvert.PopulateObject(users[i], db[i]);
                if ((DateTime.Now - db[i].progressStart).TotalSeconds > 86400)
                {
                    db[i].progressStart = DateTime.Now;
                    db[i].points = 0;
                    db[i].claimed = false;
                }
                //PlayerPrefs.SetString("fetched",DateTime.Now.Hour + "-" + DateTime.Now.Day);
                //PlayerPrefs.Save();
            }

            if(SceneManager.GetActiveScene().name == "Start")
                MainMenuManager.instance.StartAttributes();
            //LocalSave.SaveLocally(db);
        }));

        return db;
    }

    public static void GameOver(string name, int score)
    {
        if (score > PlayerPrefs.GetInt("HighScore",0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            CreateUser(name, score, DateTime.Now, Potions.PotionInventory,GameHandler.Instance.profile.balance, GameHandler.Instance.profile.points + score, GameHandler.Instance.profile.progressStart,GameHandler.Instance.profile.claimed);
        }
        
        SendScore(name, score, DateTime.Now, GameHandler.Instance.profile.balance, Potions.PotionInventory, GameHandler.Instance.profile.points + score, GameHandler.Instance.profile.progressStart, GameHandler.Instance.profile.claimed);

    }


    public static void SendScore(string name, int score, DateTime date, int balance, int[] potions, int points, DateTime progress, bool claimed)
    {
        DBUser[] users = db;
        foreach(DBUser user in users)
        {
            if (user.name == name)
            {
                user.Check();
                for (int i = 0; i < user.score.Length; i++)
                {
                    if (user.score[i] < score)
                    {
                        user.score[i] = score;
                        user.date[i] = date;
                    }
                }
                user.points = points;
                user.progressStart = progress;
                user.balance = balance;
                user.potions = potions;
                user.claimed = claimed;
                string js = JsonConvert.SerializeObject(user);
                reference.Child("users").Child(userID).SetRawJsonValueAsync(js);
                return;
            }
        }


    }
}
