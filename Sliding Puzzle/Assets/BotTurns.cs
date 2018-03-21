using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BotTurns : MonoBehaviour {

	public Text myText;
	public int turns;
	// Use this for initialization
	void Start () {
		if (turns == null) {
			turns = 0;
			myText.text = turns + " Turns";
		}
	}

	// Update is called once per frame
	void Update () {
	}
	public void AddTurn(){
		turns++;
		myText.text = turns + " Turns";
	}
}
