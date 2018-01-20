using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingPopUp : MonoBehaviour {
	public Text myText;
	static int myrating;
	public Text mySecondText;
	//int previous rating;

	// Use this for initialization

	public static void GiveRating () {
		
		RatingBehaviour.CalculateRating ();
		myrating = RatingBehaviour.rating;
		string prefname = "Level" + LevelManager.levelnum + "Rating";
		int previousrating = PlayerPrefs.GetInt (prefname);
		if (myrating > previousrating) {
			PlayerPrefs.SetInt (prefname, myrating);

		}
		//PlayerPrefs.Save();
		Debug.Log ("GIVEN");
//		myText.text = "You got " + RatingBehaviour.rating + " Stars";
		//mySecondText.text = "You finished level " + LevelManager.levelnum + "!";
	}

	void OnEnable(){
		GiveRating ();
		myText.text = "You got " + RatingBehaviour.rating + " Stars";
		mySecondText.text = "You finished level " + LevelManager.levelnum + "!";
	}
	// Update is called once per frame
	void Update () {
		if (myrating != RatingBehaviour.rating) {
			myText.text = "You got " + RatingBehaviour.rating + " Stars";
			mySecondText.text = "You finished level " + LevelManager.levelnum + "!";
		}
	}
}
