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

	//string appId = "ca-app-pub-3940256099942544~3347511713";
	string admobRvAdUnitID = "ca-app-pub-3940256099942544/5224354917";

	#elif UNITY_IPHONE
	string bannerAdUnitID = "619fff25829c4a6bb707d14726d3fbe8";
	string interstitialAdUnitID = "7495f98d9e534cb78aa274259248f3ef";
	string mopubRvAdUnitID = "4cbf780ea73e4e218c79a37c50c6eb8e";

	//string appId = "ca-app-pub-3940256099942544~1458002511";
	string admobRvAdUnitID = "ca-app-pub-3940256099942544/1712485313";
	#endif 

	private RewardedAd rewardedAd;

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
    
    void OnGUI () {
        
        var fontSize = (int)(0.035f * Screen.width);
        
        GUI.skin.box.fontSize = fontSize;
        GUI.skin.button.fontSize = fontSize;
        
        var buttonWidth = 0.35f * Screen.width;
        var buttonHeight = 0.15f * Screen.height;
        var buttonRowCount = 3;
        
        var groupWidth = buttonWidth * 2 + 30;
        var groupHeight = fontSize + (buttonHeight * buttonRowCount) + (buttonRowCount * 10) + 10;

        var screenWidth = Screen.width;
        var screenHeight = Screen.height;

        var groupX = ( screenWidth - groupWidth ) / 2;
        var groupY = ( screenHeight - groupHeight ) / 2;

        GUI.BeginGroup(new Rect( groupX, groupY, groupWidth, groupHeight ) );
        GUI.Box(new Rect( 0, 0, groupWidth, groupHeight ), "Select ADXLibrary function" );

        if ( GUI.Button(new Rect( 10, fontSize + 10, buttonWidth, buttonHeight ), "GetData" ) )
        {
            GetData();
        }
        if ( GUI.Button(new Rect( 10, fontSize + 20 + buttonHeight, buttonWidth, buttonHeight ), "Load Interstitial" ) )
        {
            MoPub.RequestInterstitialAd (interstitialAdUnitID);
        }
        if ( GUI.Button(new Rect( 10, fontSize + 30 + buttonHeight * 2, buttonWidth, buttonHeight ), "Load RV" ) )
        {
            LoadRV();
        }
        if ( GUI.Button(new Rect( 20 + buttonWidth, fontSize + 10, buttonWidth, buttonHeight ), "Load Banner" ) )
        {
            MoPub.RequestBanner (bannerAdUnitID, MoPub.AdPosition.BottomCenter, MoPub.MaxAdSize.ScreenWidthHeight50);
        }
        if ( GUI.Button(new Rect( 20 + buttonWidth, fontSize + 20 + buttonHeight, buttonWidth, buttonHeight ), "Show Interstitial" ) )
        {
            ShowInterstitial();
        }
        if ( GUI.Button(new Rect( 20 + buttonWidth, fontSize + 30 + buttonHeight * 2, buttonWidth, buttonHeight ), "Show RV" ) )
        {
            ShowRV();
        }

        GUI.EndGroup();
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
		MobileAds.Initialize(initStatus => {
			Debug.Log("MobileAds Initialize Completed");
		});
	}

	public void LoadStandard() {
		MoPub.RequestBanner (bannerAdUnitID, MoPub.AdPosition.BottomCenter, MoPub.MaxAdSize.ScreenWidthHeight50); 
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
        rewardedAd = new RewardedAd(admobRvAdUnitID);
        
         // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        
		if (ADXGDPRManager.GetConsentState () == 2) {
			AdRequest request = new AdRequest.Builder ()
				.AddExtra ("npa", "1")
				.Build ();
            rewardedAd.LoadAd(request);
		} else {
            AdRequest request = new AdRequest.Builder().Build();
            rewardedAd.LoadAd(request);
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
        if(rewardedAd == null) {
            return;
        }
        
		if (rewardedAd.IsLoaded()) {
            rewardedAd.Show();
        }
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
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
		Debug.Log("HandleRewardedAdFailedToLoad event received");
	}

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
		Debug.Log("HandleRewardedAdFailedToShow event received");
	}

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
    }
}
