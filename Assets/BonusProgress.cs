using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusProgress : MonoBehaviour
{
    private const float MaxPoints = 25000;
    [SerializeField] Image progressBar;
    [SerializeField] TMP_Text progressText_text;
    [SerializeField] TMP_Text progressText_number;
    [SerializeField] GameObject claimPrizeButton;
    [SerializeField] GameObject claimDoubleButton;
    void Start()
    {
        claimPrizeButton.SetActive(false);
        claimDoubleButton.SetActive(false);
        progressBar.color = Color.red;
        DBUser current = MainMenuManager.current;

        if (current.claimed)
        {
            progressText_text.text = "CLAIMED";
            progressText_number.text = "ALREADY";
            progressBar.fillAmount = 1;
            progressBar.color = Color.gray;
            return;
        }

        float percentage = CalculateProgress(current.points);
        if (percentage > 1)
        {
            progressBar.fillAmount = 1;
            claimPrizeButton.SetActive(true);
            claimDoubleButton.SetActive(true);
            progressText_number.text = "READY TO CLAIM!";
            progressText_text.text = "";
        } else
        {
            progressBar.fillAmount = percentage;
            progressText_number.text = (MaxPoints - current.points).ToString();
        }
    }

    public void ClaimClick(int choice)
    {
        if (choice == 1)
        {
            DBUser current = MainMenuManager.current;
            int prize = 1000;
            current.balance += prize;
            FirebaseManager.SendScore(current.name, 0, DateTime.Now, current.balance, current.potions, current.points, current.progressStart, true);
            StartPopUpMessage.MessageNormal("CLAIMED " + prize, Color.green);
            Claimed();
        }
        else
        {
            Rewarded.SetReward(RewardClaim);
            Rewarded.instance.ShowRewardedAd();
            Claimed();
        }
    }
    private void RewardClaim()
    {
        DBUser current = MainMenuManager.current;
        int prize = 2500;
        current.balance += prize;
        FirebaseManager.SendScore(current.name, 0, DateTime.Now, current.balance, current.potions, current.points, current.progressStart, true);
        StartPopUpMessage.MessageNormal("CLAIMED " + prize, Color.green);
        AudioManager.PlayOneShot(4);
        MainMenuManager.instance.ChangeBalance();
    }
    private void Claimed()
    {
        claimDoubleButton.SetActive(false);
        claimPrizeButton.SetActive(false);
        progressText_text.text = "CLAIMED";
        progressText_number.text = "ALREADY";
        progressBar.fillAmount = 1;
        progressBar.color = Color.gray;
        MainMenuManager.instance.ChangeBalance();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private float CalculateProgress(int points)
    {
        return points / MaxPoints;
    }
}
