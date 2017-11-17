using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBuilder : MonoBehaviour {

	public GameObject myCanvas;
	public Camera myCamera;
	public GameObject thisButton;
	public float screenheigth;
	public float screenwidth;

	// Use this for initialization
	void Start () {
		screenwidth = myCamera.pixelWidth;
		screenheigth = myCamera.pixelHeight;
		BuildButton (1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void BuildButton(int buttonnum){
		Debug.Log (screenwidth);
		Debug.Log (screenheigth);
		Instantiate (thisButton, new Vector3 (-1, -1, 0), Quaternion.identity);

	}
}
