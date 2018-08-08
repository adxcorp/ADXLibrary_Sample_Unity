using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Api.Mediation;

public class ADXSampleScript : MonoBehaviour {
	
	public static Boolean IS_FORCED_SET = false;
	public static Boolean IS_MOPUB_RV = false;

	#if UNITY_ANDROID
	string bannerAdUnitID = "c0a9aaa3404f4d7db48ca71ddb647502";
	string interstitialAdUnitID = "a8e0e1ff9de44766be2102e354fe3f92";
	string mopubRvAdUnitID = "56ceda53658b48198de5993ff3010487";

	string appId = "ca-app-pub-7466439784264697~3084726285";
	string admobRvAdUnitID = "ca-app-pub-7466439784264697/2318439525";

	#elif UNITY_IPHONE
	string bannerAdUnitID = "619fff25829c4a6bb707d14726d3fbe8";
	string interstitialAdUnitID = "7495f98d9e534cb78aa274259248f3ef";
	string mopubRvAdUnitID = "4cbf780ea73e4e218c79a37c50c6eb8e";

	string appId = "ca-app-pub-7466439784264697~7972777801";
	string admobRvAdUnitID = "ca-app-pub-7466439784264697/6572954274";
	#endif 

	public static RewardBasedVideoAd rewardedVideo;

	// Use this for initialization
	void Start () {

		if (IS_FORCED_SET) {
			ADXGDPRManager.InitializeWithSetConsentState (bannerAdUnitID, 1);
		} else {
			ADXGDPRManager.InitializeWithShowADXConsent (bannerAdUnitID, 0);
		}

		ADXGDPR.OnADXConsentCompleted += onADXConsentCompleted;
	}

	void Update () {
		
	}

	public void onADXConsentCompleted(string s) {
		Debug.Log("onADXConsentCompleted ::: ");

		InitStandard ();

		if (IS_MOPUB_RV) {
			InitMoPubRV ();
		} else {
			InitAdMobRV ();
		}
	}

	public void GetData() {
		Debug.Log ("GetConsentState : " + ADXGDPRManager.GetConsentState ());
		Debug.Log ("GetPrivacyPolicyURL : " + ADXGDPRManager.GetPrivacyPolicyURL ());

		ADXGDPRManager.SetConsentState (3);
		Debug.Log ("GetConsentState : " + ADXGDPRManager.GetConsentState());
	}

	public void InitStandard() {
		
		//*** Banner
		var allBannerAdUnits = new string[] { bannerAdUnitID };
		MoPub.LoadBannerPluginsForAdUnits(allBannerAdUnits);

		//*** Interstitial
		var allInterstitialAdUnits = new string[] { interstitialAdUnitID };
		MoPub.LoadInterstitialPluginsForAdUnits(allInterstitialAdUnits);

		//*** Banner
		MoPubManager.OnAdLoadedEvent += onAdLoadedEvent;
		MoPubManager.OnAdFailedEvent += onAdFailedEvent;

		//*** Interstitial
		MoPubManager.OnInterstitialLoadedEvent += onInterstitialLoadedEvent;
		MoPubManager.OnInterstitialFailedEvent += onInterstitialFailedEvent;
		MoPubManager.OnInterstitialDismissedEvent += onInterstitialDismissedEvent;
	}

	public void InitMoPubRV() {

		var allRewardedAdUnits = new string[] { mopubRvAdUnitID };
		MoPub.LoadRewardedVideoPluginsForAdUnits(allRewardedAdUnits);

		MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
		MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
		MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;
		MoPubManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReceivedRewardEvent;
		MoPubManager.OnRewardedVideoExpiredEvent += OnRewardedVideoExpiredEvent;
		MoPubManager.OnRewardedVideoShownEvent += OnRewardedVideoShownEvent;
		MoPubManager.OnRewardedVideoClickedEvent += OnRewardedVideoClickedEvent;
		MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;
		MoPubManager.OnRewardedVideoLeavingApplicationEvent += OnRewardedVideoLeavingApplicationEvent;
	}

	public void InitAdMobRV() {

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

	public void LoadStandard() {

		MoPub.CreateBanner (bannerAdUnitID, MoPub.AdPosition.BottomCenter); 
		MoPub.RequestInterstitialAd (interstitialAdUnitID);  
	}

	public void LoadRV() {
		if (IS_MOPUB_RV) {
			LoadMoPubRV ();
		} else {
			LoadAdMobRV ();
		}
	}
		
	public void LoadMoPubRV() {
		MoPub.RequestRewardedVideo (mopubRvAdUnitID); 
	}

	public void LoadAdMobRV() {

		if (ADXGDPRManager.GetConsentState () == 2) {
			// Create an empty ad request.
			AdRequest request = new AdRequest.Builder ()
				.AddExtra ("npa", "1")
				.Build ();
			rewardedVideo.LoadAd (request, admobRvAdUnitID);
		} else {
			// Create an empty ad request.
			AdRequest request = new AdRequest.Builder ()
				.Build ();
			rewardedVideo.LoadAd (request, admobRvAdUnitID);
		}
	}
		
	public void ShowInterstitial(){
		MoPub.ShowInterstitialAd (interstitialAdUnitID);    
	}

	public void ShowRV() {
		if (IS_MOPUB_RV) {
			ShowMoPubRV ();
		} else {
			ShowAdMobRV ();
		}
	}

	public void ShowMoPubRV() {
		MoPub.ShowRewardedVideo (mopubRvAdUnitID); 
	}

	public void ShowAdMobRV() {
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

	//*** MoPub RV Callback
	public void OnRewardedVideoLoadedEvent (string s) {
		Debug.Log ("OnRewardedVideoLoadedEvent : " + s);
	}

	public void OnRewardedVideoFailedEvent (string s1, string s2) {
		Debug.Log ("OnRewardedVideoFailedEvent : " + s1 + "/" + s2);
	}

	public void OnRewardedVideoReceivedRewardEvent (string s1, string s2, float f) {
		Debug.Log ("OnRewardedVideoReceivedRewardEvent : " + s1 + "/" + s2 + "/" + f);
	}

	public void OnRewardedVideoClosedEvent (string s) {
		Debug.Log ("OnRewardedVideoClosedEvent : " + s);
	}

	public void OnRewardedVideoExpiredEvent (string s) {
		Debug.Log ("OnRewardedVideoExpiredEvent : " + s);
	}

	public void OnRewardedVideoShownEvent (string s) {
		Debug.Log ("OnRewardedVideoShownEvent : " + s);
	}

	public void OnRewardedVideoClickedEvent (string s) {
		Debug.Log ("OnRewardedVideoClickedEvent : " + s);
	}

	public void OnRewardedVideoFailedToPlayEvent (string s1, string s2) {
		Debug.Log ("OnRewardedVideoFailedToPlayEvent : " + s1 + "/" + s2);
	}

	public void OnRewardedVideoLeavingApplicationEvent (string s) {
		Debug.Log ("OnRewardedVideoLeavingApplicationEvent : " + s);
	}

	//*** AdMob RV Callback
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
