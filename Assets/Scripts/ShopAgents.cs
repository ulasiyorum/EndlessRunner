using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;

public class ShopAgents : MonoBehaviour
{
    public static ShopAgents instance;
    [SerializeField] GameObject[] Potions;
    [SerializeField] Animator shop;
    [SerializeField] TMP_Text[] agentPriceTexts;
    [SerializeField] GameObject[] PirateCoins;
    [SerializeField] GameObject[] agentsIdle;
    void Start()
    {
        instance = this;
    }
    public void OpenTheAgent(int id)
    {
        Array.ForEach(agentsIdle, ((value) =>
        {
            value.SetActive(false);
        }));
        agentsIdle[id].SetActive(true);
    }
    public void GoToAgents()
    {
        DBUser user = MainMenuManager.current;
        shop.enabled = false;
        Array.ForEach(Potions, ((value) => { value.SetActive(false); }));

        for(int i = 0; i < agentPriceTexts.Length; i++)
        {
            if (user.agents[i]) 
            {
                agentPriceTexts[i].text = "USE";
                PirateCoins[i].SetActive(false);
            }
        }



    }
    public void GoBackToPotions()
    {
        shop.enabled = true;
        Array.ForEach(Potions, ((value) => { value.SetActive(true); }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Buy(int id)
    {
        DBUser current = MainMenuManager.current;
        if(id == current.selected)
        {
            StartPopUpMessage.MessageNormal("You're already selected this agent",Color.gray);
            return;
        }

        if(id == 0)
        {
            current.selected = id;
            OpenTheAgent(id);
        }

        if(id == 1 && !current.agents[id])
        {
            if (current.balance < 6000)
            {
                StartPopUpMessage.Message("Not Enough Balance!", Color.red);
            } else
            {
                current.agents[id] = true;
                current.balance -= 6000;
                current.selected = id;
                FirebaseManager.SendPlayer(current);
                MainMenuManager.instance.ChangeBalance();
                Handheld.Vibrate();
                AudioManager.PlayOneShot(4);
                OpenTheAgent(id);
            }

            return;
        } else
        {
            current.selected = id;
            OpenTheAgent(id);
            return;
        }


    }
}
