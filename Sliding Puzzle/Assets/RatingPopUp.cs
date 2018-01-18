using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingPopUp : MonoBehaviour {
	public Text myText;
	static int myrating;
	// Use this for initialization
	public static void GiveRating () {
		RatingBehaviour.CalculateRating ();
		myrating = RatingBehaviour.rating;
		Debug.Log ("GIVEN");
	}

	// Update is called once per frame
	void Update () {
		if (myrating != RatingBehaviour.rating) {
			myText.text = "You got " + RatingBehaviour.rating + " Stars";
		}
	}
}
