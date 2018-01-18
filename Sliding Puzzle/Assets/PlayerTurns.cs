using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerTurns : MonoBehaviour {

	public Text myText;
	int turns;
	// Use this for initialization
	void Start () {
		turns = TurnCounter.turncount;
		myText.text = turns + " Turns";
	}

	// Update is called once per frame
	void Update () {
		if (turns != TurnCounter.turncount) {
			turns = TurnCounter.turncount;
			myText.text = turns + " Turns";
		}
	}
}
