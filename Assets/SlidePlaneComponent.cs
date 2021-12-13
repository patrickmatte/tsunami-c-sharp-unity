using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePlaneComponent : MonoBehaviour {

	public Action mouseDownCallBack;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {
		mouseDownCallBack ();
	}

}
