using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

public class AdManager : MonoBehaviour
{
    public InterstitialAd interstitialAd;

    [SerializeField] private AudioSource clickAudio;
    private string adPurpose;

    private void Start()
    {
        MobileAds.Initialize(initstatus =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                RequestInterstitialAd();
            });
        });
    }

    private void EarnReward()
    {
        PlayerPrefs.SetString("EarnedHeart", "true");
        GameObject.Find("GameManager").GetComponent<GameManager>().ResumeGame();
    }

    public void ShowInterstitialAd(string purpose)
    {
        adPurpose = purpose;
        clickAudio.Play();

        if (interstitialAd != null && interstitialAd.CanShowAd())
            interstitialAd.Show();
        else if (adPurpose == "restart")
            GameObject.Find("GameManager").GetComponent<GameManager>().RestartGame();
    }

    private void RequestInterstitialAd()
    {
        string adUnitID = "ca-app-pub-3004880773065199/2600380085";

        DestroyInterstitialAd();

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        InterstitialAd.Load(adUnitID, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            if (error != null)
            {
                Debug.LogError("Interstitial ad failed to load an ad with error : " + error);
                return;
            }
            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (ad == null)
            {
                Debug.LogError("Unexpected error: Interstitial load event fired with null ad and null error.");
                return;
            }

            // The operation completed successfully.
            Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());
            interstitialAd = ad;

            // Register to ad events to extend functionality.
            RegisterEventHandlers(ad);

        });
    }



    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");

            DestroyInterstitialAd();
            RequestInterstitialAd();

            if (adPurpose == "restart")
                GameObject.Find("GameManager").GetComponent<GameManager>().RestartGame();
            else if (adPurpose == "heart")
                EarnReward();

            // Reload the ad so that we can show another as soon as possible.
            RequestInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            RequestInterstitialAd();
        };
    }
    private void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
            interstitialAd.Destroy();
    }

}
