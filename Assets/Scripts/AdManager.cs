using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager AdManagerInstance;

    public RewardedAd rewardedAd;
    public InterstitialAd interstitialAd;

    public bool isRewardedAdShown = false;


    private void Awake()
    {
        if (AdManagerInstance != null)
            Destroy(gameObject);
        else
        {
            AdManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

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
        if (rewardedAd != null && rewardedAd.IsLoaded())
            rewardedAd.Show();
        else
            EarnReward();

        isRewardedAdShown = true;
    }

    private void RequestRewardedAd()
    {
        string adUnitID = "ca-app-pub-3940256099942544/5224354917";  //test id
        rewardedAd = new RewardedAd(adUnitID);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);

        rewardedAd.OnUserEarnedReward += HandleOnRewardedAdWatched;
        rewardedAd.OnAdClosed += HandleOnRewardedAdClosed;
    }

    private void HandleOnRewardedAdClosed(object sender, EventArgs e)
    {
        RequestRewardedAd();
        isRewardedAdShown = false;
    }

    private void HandleOnRewardedAdWatched(object sender, Reward e)
    {
        EarnReward();
        RequestRewardedAd();
        isRewardedAdShown = false;
    }

    private void EarnReward()
    {
        GameManager.instance.ResumeGame();
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.IsLoaded())
            interstitialAd.Show();
    }

    private void RequestInterstitialAd()
    {
        string adUnitID = "ca-app-pub-3940256099942544/1033173712";  //test id

        interstitialAd = new InterstitialAd(adUnitID);
        AdRequest request = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(request);
        interstitialAd.OnAdClosed += HandleInterstitialAdClosed;
    }

    private void HandleInterstitialAdClosed(object sender, EventArgs e)
    {
        DestroyInterstitialAd();
        RequestInterstitialAd();
        GameManager.instance.RestartGame();
    }

    private void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
            interstitialAd.Destroy();
    }

}
