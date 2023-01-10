using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

public class AdManager : MonoBehaviour
{
    public RewardedAd rewardedAd;
    public InterstitialAd interstitialAd;

    [SerializeField] private AudioSource clickAudio;

    private void Start()
    {

        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
                .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
                .build();

        MobileAds.Initialize(initstatus =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                RequestRewardedAd();
                RequestInterstitialAd();
            });
        });
    }

    public void ShowRewardedAd()
    {
        clickAudio.Play();

        if (rewardedAd != null && rewardedAd.IsLoaded())
            rewardedAd.Show();
    }

    private void RequestRewardedAd()
    {
        string adUnitID = "ca-app-pub-3004880773065199/6582737567";
        rewardedAd = new RewardedAd(adUnitID);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);

        rewardedAd.OnUserEarnedReward += HandleOnRewardedAdWatched;
        rewardedAd.OnAdClosed += HandleOnRewardedAdClosed;
    }

    private void HandleOnRewardedAdClosed(object sender, EventArgs e)
    {
        RequestRewardedAd();
    }

    private void HandleOnRewardedAdWatched(object sender, Reward e)
    {
        EarnReward();
        RequestRewardedAd();
    }

    private void EarnReward()
    {
        PlayerPrefs.SetString("EarnedHeart","true");
        GameObject.Find("GameManager").GetComponent<GameManager>().ResumeGame();
    }

    public void ShowInterstitialAd()
    {
        clickAudio.Play();
        if (interstitialAd != null && interstitialAd.IsLoaded())
            interstitialAd.Show();
        else
            GameObject.Find("GameManager").GetComponent<GameManager>().RestartGame();
    }

    private void RequestInterstitialAd()
    {
        string adUnitID = "ca-app-pub-3004880773065199/2600380085";

        interstitialAd = new InterstitialAd(adUnitID);
        AdRequest request = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(request);
        interstitialAd.OnAdClosed += HandleInterstitialAdClosed;
    }

    private void HandleInterstitialAdClosed(object sender, EventArgs e)
    {
        DestroyInterstitialAd();
        RequestInterstitialAd();
        GameObject.Find("GameManager").GetComponent<GameManager>().RestartGame();
    }

    private void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
            interstitialAd.Destroy();
    }

}
