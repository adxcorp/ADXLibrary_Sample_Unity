using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterstitialScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick () {
		Debug.Log ("OnClick");

		#if UNITY_ANDROID
		MoPub.ShowInterstitialAd ("7758ce2f590f43fd8ba9f03d14fc8b78");    
		#elif UNITY_IPHONE
		MoPub.ShowInterstitialAd ("f6110c24fa8a4daf9c6159f5ea181e7d");  
		#endif 

	}


}
