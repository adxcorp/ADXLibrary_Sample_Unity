using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

		#if UNITY_ANDROID
		var allRewardedVideoAdUnits = new string[] { "cba24206db20482db27b815b1deec598" };
		MoPub.loadRewardedVideoPluginsForAdUnits(allRewardedVideoAdUnits);
		#elif UNITY_IPHONE
		var allRewardedVideoAdUnits = new string[] { "756f6366d71f4bef88ae55c5ac13ac36" };
		MoPub.loadPluginsForAdUnits(allRewardedVideoAdUnits);
		#endif  

		MoPub.initializeRewardedVideo();

		MoPubManager.onRewardedVideoLoadedEvent += onRewardedVideoLoadedEvent;
		MoPubManager.onRewardedVideoFailedEvent += onRewardedVideoFailedEvent;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick () {
		Debug.Log ("OnClick");

		#if UNITY_ANDROID
		MoPub.requestRewardedVideo ("cba24206db20482db27b815b1deec598");  
		#elif UNITY_IPHONE
		MoPub.requestRewardedVideo ("756f6366d71f4bef88ae55c5ac13ac36");  
		#endif 

	}

	public void onRewardedVideoFailedEvent(string s) {
		Debug.Log("onRewardedVideoFailedEvent ::: ");

	}

	public void onRewardedVideoLoadedEvent(string s) {
		Debug.Log("onRewardedVideoLoadedEvent ::: ");


		#if UNITY_ANDROID
		MoPub.showRewardedVideo ("cba24206db20482db27b815b1deec598");    
		#elif UNITY_IPHONE
		MoPub.showRewardedVideo ("756f6366d71f4bef88ae55c5ac13ac36");  
		#endif  

	}
}
