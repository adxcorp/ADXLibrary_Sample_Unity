using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Api.Mediation.Vungle;

public class ADXSampleScript : MonoBehaviour {

	public static RewardBasedVideoAd rewardedVideo;

	// Use this for initialization
	void Start () {
		ADXGDPRManager.Initialize("a9bcfae03030442da3ed277aff98713c");

		//*** 0 : DEFAULT
		//*** 1 : DEBUG (InEEA)
//		ADXGDPRManager.SetDebugState(1);

		ADXGDPR.OnADXConsentCompleted += onADXConsentCompleted;

	}

	void Update () {
		
	}

	public void ShowConsent() {
		ADXGDPRManager.ShowADXConsent();
	}

	public void onADXConsentCompleted(string s) {
		Debug.Log("onADXConsentCompleted ::: ");

		#if UNITY_ANDROID

		//*** Banner
		var allBannerAdUnits = new string[] { "f86ac2fde1d4480e95f1f0d80710a2fc" };
		MoPub.LoadBannerPluginsForAdUnits(allBannerAdUnits);
		MoPub.CreateBanner ("f86ac2fde1d4480e95f1f0d80710a2fc", MoPub.AdPosition.BottomCenter);  

		//*** Interstitial
		var allInterstitialAdUnits = new string[] { "7758ce2f590f43fd8ba9f03d14fc8b78" };
		MoPub.LoadInterstitialPluginsForAdUnits(allInterstitialAdUnits);

		//*** Rewarded Video
		string appId = "ca-app-pub-7466439784264697~3084726285";

		#elif UNITY_IPHONE

		//*** Banner
		var allBannerAdUnits = new string[] { "a9bcfae03030442da3ed277aff98713c" };
		MoPub.LoadBannerPluginsForAdUnits(allBannerAdUnits);
		MoPub.CreateBanner ("a9bcfae03030442da3ed277aff98713c", MoPub.AdPosition.BottomCenter); 

		//*** Interstitial
		var allInterstitialAdUnits = new string[] { "f6110c24fa8a4daf9c6159f5ea181e7d" };
		MoPub.LoadInterstitialPluginsForAdUnits(allInterstitialAdUnits);

		//*** Rewarded Video
		string appId = "ca-app-pub-7466439784264697~7972777801";

		#endif  


		//*** Banner
		MoPubManager.OnAdLoadedEvent += onAdLoadedEvent;
		MoPubManager.OnAdFailedEvent += onAdFailedEvent;

		//*** Interstitial
		MoPubManager.OnInterstitialLoadedEvent += onInterstitialLoadedEvent;
		MoPubManager.OnInterstitialFailedEvent += onInterstitialFailedEvent;
		MoPubManager.OnInterstitialDismissedEvent += onInterstitialDismissedEvent;

		//*** Rewarded Video
		MobileAds.Initialize(appId);
		rewardedVideo = RewardBasedVideoAd.Instance;

		// Called when an ad request has successfully loaded.
		rewardedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
		// Called when an ad request failed to load.
		rewardedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
		// Called when an ad is shown.
		rewardedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
		// Called when the ad starts to play.
		rewardedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
		// Called when the user should be rewarded for watching a video.
		rewardedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
		// Called when the ad is closed.
		rewardedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
		// Called when the ad click caused the user to leave the application.
		rewardedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

	}

	public static void LoadAd() {

		#if UNITY_ANDROID
		MoPub.RequestInterstitialAd ("7758ce2f590f43fd8ba9f03d14fc8b78");  
		#elif UNITY_IPHONE
		MoPub.RequestInterstitialAd ("f6110c24fa8a4daf9c6159f5ea181e7d");  
		#endif  
		
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-7466439784264697/2318439525";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-7466439784264697/6572954274";
		#endif

		VungleRewardedVideoMediationExtras extras = new VungleRewardedVideoMediationExtras();
		#if UNITY_ANDROID
		extras.SetAllPlacements(new string[] { "DEFAULT-0339375", "SAMPLE_ANDROID_INTERSTITIAL-0969912", "SAMPLE_ANDROID_REWARDED_VIDEO-3138664" });
		#elif UNITY_IPHONE
		extras.SetAllPlacements(new string[] { "DEFAULT-4197699", "SAMPLE_IOS_REWARDED_VIDEO-2228390" });
		#endif

		if (ADXGDPRManager.GetConsentState() == 2) {
			extras.Extras.Add ("npa", "1");
		}

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().AddMediationExtras(extras).Build();
		rewardedVideo.LoadAd(request, adUnitId);

	}

	public static void ShowRewardedVideo() {
		rewardedVideo.Show ();
	}

	//*** Banner Callback
	public void onAdLoadedEvent(string s, float f) {
		Debug.Log("onAdLoadedEvent ::: " + "s : " + s + "f : " + f);
	}

	public void onAdFailedEvent(string s1, string s2) {
		Debug.Log("onAdFailedEvent ::: " + "s1 : " + s1 + "s2 : " + s2);
	}

	//*** Interstitial Callback
	public void onInterstitialFailedEvent(string s1, string s2) {
		Debug.Log("onInterstitialFailedEvent ::: " + "s1 : " + s1 + "s2 : " + s2);
	}

	public void onInterstitialLoadedEvent(string s) {
		Debug.Log("onInterstitialLoadedEvent ::: " + "s : " + s);
	}

	public void onInterstitialDismissedEvent(string s) {
		Debug.Log("onInterstitialDismissedEvent ::: " + "s : " + s);
	}

	//*** Rewarded Video Callback
	public void HandleRewardBasedVideoLoaded(object sender, EventArgs args) {
		Debug.Log("HandleRewardBasedVideoLoaded event received: " + rewardedVideo.MediationAdapterClassName());
	}

	public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
		Debug.Log("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
	}

	public void HandleRewardBasedVideoOpened(object sender, EventArgs args) {
		Debug.Log("HandleRewardBasedVideoOpened event received");
	}

	public void HandleRewardBasedVideoStarted(object sender, EventArgs args) {
		Debug.Log("HandleRewardBasedVideoStarted event received");
	}

	public void HandleRewardBasedVideoClosed(object sender, EventArgs args) {
		Debug.Log("HandleRewardBasedVideoClosed event received");
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args) {
		string type = args.Type;
		double amount = args.Amount;
		Debug.Log("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
	}

	public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args) {
		Debug.Log ("HandleRewardBasedVideoLeftApplication event received");
	}
}
