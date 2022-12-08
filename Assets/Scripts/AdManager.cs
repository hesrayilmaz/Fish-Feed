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
        //else
          //  EarnReward();
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
        GameObject.Find("GameManager").GetComponent<GameManager>().RestartGame();
    }

    private void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
            interstitialAd.Destroy();
    }

}
