using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class DisplayLeaderboards : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;


    Transform parent;
    public GameObject prefab;
    private int choice = 0;

    private void Start()
    {
        parent = transform;

        GetLeaderboards();
    }

    private void Update()
    {

    }

    private async void GetLeaderboards()
    {
        DBUser[] users = FirebaseManager.db;

        if(users == null)
        {
            await Task.Delay(10);
            GetLeaderboards();
            return;
        }

        IComparer<DBUser> comparer = choice switch
        {
            0 => new DBUserComparerGlobal(),
            1 => new DBUserComparerMonthly(),
            2 => new DBUserComparerWeekly(),
            3 => new DBUserComparerDaily(),
            _ => new DBUserComparerGlobal()
        };

        Array.Sort(users, comparer);
        RectTransform prefabTransform = prefab.GetComponent<RectTransform>();
        Component[] children = parent.gameObject.GetComponentsInChildren<Image>();

        int j = 0;
        foreach (Component child in children)
        {
            if (child.gameObject != gameObject)
            {
                Destroy(child.gameObject);
                if (j > 7)
                {
                    contentPanel.offsetMax = new Vector2(contentPanel.offsetMax.x, contentPanel.offsetMax.y - (prefabTransform.rect.height * 1.25f));
                }
            }

            j++;
        }

        for (int i = 0; i < users.Length; i++)
        {
            users[i].Check();

            if (i > 6)
            {
                contentPanel.offsetMax = new Vector2(contentPanel.offsetMax.x, contentPanel.offsetMax.y + (prefabTransform.rect.height * 1.25f));
            }
            GameObject go = Instantiate(prefab, parent);
            go.GetComponentsInChildren<Text>()[0].text = "" + (i + 1);
            go.GetComponentsInChildren<Text>()[1].text = users[i].name;
            go.GetComponentsInChildren<Text>()[2].text = "" + users[i].score[choice];


            if (users[i].name.ToLower() == PlayerPrefs.GetString("name", "User" + UnityEngine.Random.Range(0, 19543)).ToLower())
            {

                go.GetComponentsInChildren<Text>()[0].color = Color.red;
                go.GetComponentsInChildren<Text>()[1].color = Color.red;
                go.GetComponentsInChildren<Text>()[2].color = Color.red;

                Canvas.ForceUpdateCanvases();
                contentPanel.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(go.transform.position);
            }
        }
    }

    public void Choice(int ch)
    {
        choice = ch;
        GetLeaderboards();
    }
}
