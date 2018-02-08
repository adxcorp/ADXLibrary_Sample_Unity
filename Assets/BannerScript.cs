using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID
		var allBannerAdUnits = new string[] { "f86ac2fde1d4480e95f1f0d80710a2fc" };
		MoPub.loadBannerPluginsForAdUnits(allBannerAdUnits);
		MoPub.createBanner ("f86ac2fde1d4480e95f1f0d80710a2fc", MoPubAdPosition.BottomCenter);  
		#elif UNITY_IPHONE
		var allBannerAdUnits = new string[] { "a9bcfae03030442da3ed277aff98713c" };
		MoPub.loadPluginsForAdUnits(allBannerAdUnits);
		MoPub.createBanner ("a9bcfae03030442da3ed277aff98713c", MoPubAdPosition.BottomCenter);  
		#endif  

		MoPubManager.onAdLoadedEvent += onAdLoadedEvent;
		MoPubManager.onAdFailedEvent += onAdFailedEvent;


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onAdLoadedEvent(float f) {
		Debug.Log("onAdLoadedEvent ::: ");

	}

	public void onAdFailedEvent(string s) {
		Debug.Log("onAdFailedEvent ::: ");

	}
}
