using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttonbuilder : MonoBehaviour {

	public GameObject Button;
	float posx;
	float posy;
	// Use this for initialization
	void Start () {
		float width = Screen.width;
		float heigth = Screen.height;
		Debug.Log (heigth);
		for(int y = 1; y<9; y++){
			for(int x = 1; x<6; x++){
				float posx = (x/5f)*width;
				float posy = (y/8f)*heigth;
				Debug.Log (posx);
				Instantiate (Button, new Vector3 (posx, posy, 0), Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
