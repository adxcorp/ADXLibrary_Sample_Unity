using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAdScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick() {
		Debug.Log ("OnClick");
		ADXSampleScript.LoadAd ();
	}
}
