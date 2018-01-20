using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCamera : MonoBehaviour {
	public Canvas mycanvas;


	// Use this for initialization
	void Start () {
		mycanvas = GetComponent<Canvas> ();
		GameObject camera = GameObject.Find ("Main Camera");
		Camera mycamera = camera.GetComponent<Camera>();
		mycanvas.worldCamera = mycamera;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
