using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplayScript : MonoBehaviour {
	public Text myText;
	int mylevelnum;
	// Use this for initialization
	void Start () {
		mylevelnum = LevelManager.levelnum;
		myText.text = "The level is " + mylevelnum;
	}
	
	// Update is called once per frame
	void Update () {
		if (mylevelnum != LevelManager.levelnum) {
			mylevelnum = LevelManager.levelnum;
			myText.text = "The level is " + mylevelnum;
		}
	}
}
