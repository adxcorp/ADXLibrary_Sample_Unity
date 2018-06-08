using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterstitialScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID
		var allInterstitialAdUnits = new string[] { "7758ce2f590f43fd8ba9f03d14fc8b78" };
		MoPub.loadInterstitialPluginsForAdUnits(allInterstitialAdUnits); 
		#elif UNITY_IPHONE
		var allInterstitialAdUnits = new string[] { "f6110c24fa8a4daf9c6159f5ea181e7d" };
		MoPub.loadPluginsForAdUnits(allInterstitialAdUnits);
		#endif  

		MoPubManager.onInterstitialLoadedEvent += onInterstitialLoadedEvent;
		MoPubManager.onInterstitialFailedEvent += onInterstitialFailedEvent;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick () {

		#if UNITY_ANDROID
		MoPub.requestInterstitialAd ("7758ce2f590f43fd8ba9f03d14fc8b78");  
		#elif UNITY_IPHONE
		MoPub.requestInterstitialAd ("f6110c24fa8a4daf9c6159f5ea181e7d");  
		#endif  
	}

	public void onInterstitialFailedEvent(string s) {
		Debug.Log("onInterstitialFailedEvent ::: ");
	}

	public void onInterstitialLoadedEvent(string s) {
		Debug.Log("onInterstitialLoadedEvent ::: ");

		#if UNITY_ANDROID
		MoPub.showInterstitialAd ("7758ce2f590f43fd8ba9f03d14fc8b78");    
		#elif UNITY_IPHONE
		MoPub.showInterstitialAd ("f6110c24fa8a4daf9c6159f5ea181e7d");  
		#endif  

	}
}
