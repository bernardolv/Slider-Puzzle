using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
	public int mylevelnumber;
	public Button myButton;
	SceneLoading mysceneloading;
	public Text mytext;
	public Text myratingnumber;
	public ButtonRating myBR;
	public GameObject myrater;
	int rating;
	int pastrating;
	// Use this for initialization

	void Start () {
		mysceneloading = GetComponentInParent<SceneLoading> ();
		myButton = GetComponent<Button> ();
		myButton.onClick.AddListener (Load);
		mytext = GetComponentInChildren<Text>();
		mytext.text = "Level " + mylevelnumber;
		myrater = GameObject.Find ("Rating" + mylevelnumber);
		int mypreviousnumber = mylevelnumber - 1;
		string pastratingstring = "Level" + mypreviousnumber + "Rating";
		pastrating = PlayerPrefs.GetInt (pastratingstring);
		//Debug.Log ("PASTRATING" + pastrating);

		//myratingnumber = GameObject.f
	}
	
	// Update is called once per frame
	void Update () {
		ChangeColor (mylevelnumber);
	}

	void Load (){
		CheckIfLocked (mylevelnumber);
	}
	void CheckIfLocked(int num){
		myrater = GameObject.Find ("Rating" + mylevelnumber);
		myBR = myrater.GetComponent <ButtonRating> ();
		rating = myBR.myrating;
		bool locked = LevelHandler.leveldic [num].islocked;
		Debug.Log (LevelHandler.leveldic [num].islocked);
		Debug.Log (pastrating);
		if (pastrating > 0) {
			locked = false;
		}
		if (locked && pastrating <1) {
			
			Debug.Log ("Do nothing" + pastrating); //User hasn't 	unlocked the level
		}
		if (locked == false) {
			mysceneloading.LoadScene (mylevelnumber);
			Debug.Log ("Load");
		}
	}
	void ChangeColor(int num){
		myrater = GameObject.Find ("Rating" + mylevelnumber);
		myBR = myrater.GetComponent <ButtonRating> ();
		rating = myBR.myrating;
		bool locked = LevelHandler.leveldic [num].islocked;

//		Debug.Log (LevelHandler.leveldic [num].islocked);
		if (locked && rating == 0 && pastrating <1) {
			//Debug.Log ("Do nothing" + rating + mylevelnumber); //User hasn't unlocked the level
			GetComponent<Image>().color = Color.gray;
			myButton.interactable = false;
		}
	}
}
