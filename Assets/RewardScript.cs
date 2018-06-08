using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using GoogleMobileAds.Api.Mediation.Vungle;
using GoogleMobileAds.Api;

public class RewardScript : MonoBehaviour {

	private RewardBasedVideoAd rewardBasedVideo;

	// Use this for initialization
	void Start () {

		#if UNITY_ANDROID
		string appId = "ca-app-pub-7466439784264697~3084726285";
		#elif UNITY_IPHONE
		string appId = "ca-app-pub-7466439784264697~7972777801";
		#else
		string appId = "unexpected_platform";
		#endif

		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize(appId);

		// Get singleton reward based video ad reference.
		this.rewardBasedVideo = RewardBasedVideoAd.Instance;

		// Called when an ad request has successfully loaded.
		rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
		// Called when an ad request failed to load.
		rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
		// Called when an ad is shown.
		rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
		// Called when the ad starts to play.
		rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
		// Called when the user should be rewarded for watching a video.
		rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
		// Called when the ad is closed.
		rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
		// Called when the ad click caused the user to leave the application.
		rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

		this.RequestRewardedVideo ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick () {
		Debug.Log ("OnClick");

		if (rewardBasedVideo.IsLoaded ()) {
			Debug.Log ("IsLoaded");
			rewardBasedVideo.Show ();
		} else {
			Debug.Log ("Is NOT Loaded");
		}
	}

	private void RequestRewardedVideo() {
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-7466439784264697/2318439525";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-7466439784264697/6572954274";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		VungleRewardedVideoMediationExtras extras = new VungleRewardedVideoMediationExtras();
		#if UNITY_ANDROID
		extras.SetAllPlacements(new string[] { "DEFAULT-0339375", "SAMPLE_ANDROID_INTERSTITIAL-0969912", "SAMPLE_ANDROID_REWARDED_VIDEO-3138664" });
		#elif UNITY_IPHONE
		extras.SetAllPlacements(new string[] { "DEFAULT-4197699", "SAMPLE_IOS_REWARDED_VIDEO-2228390" });
		#endif

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().AddMediationExtras(extras).Build();
		this.rewardBasedVideo.LoadAd(request, adUnitId);
	}

	public void HandleRewardBasedVideoLoaded(object sender, EventArgs args) {
		MonoBehaviour.print("HandleRewardBasedVideoLoaded event received: " + rewardBasedVideo.MediationAdapterClassName());

	}

	public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
		MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
	}

	public void HandleRewardBasedVideoOpened(object sender, EventArgs args) {
		MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
	}

	public void HandleRewardBasedVideoStarted(object sender, EventArgs args) {
		MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
	}

	public void HandleRewardBasedVideoClosed(object sender, EventArgs args) {
		MonoBehaviour.print("HandleRewardBasedVideoClosed event received");

		this.RequestRewardedVideo();
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args) {
		string type = args.Type;
		double amount = args.Amount;
		MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
	}

	public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args) {
		MonoBehaviour.print ("HandleRewardBasedVideoLeftApplication event received");
	}
}
