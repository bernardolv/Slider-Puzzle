using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
	public int mylevelnumber;
	public Button myButton;
	SceneLoading mysceneloading;
	public Text mytext;
	// Use this for initialization

	void Start () {
		mysceneloading = GetComponentInParent<SceneLoading> ();
		myButton = GetComponent<Button> ();
		myButton.onClick.AddListener (Load);
		mytext = GetComponentInChildren<Text>();
		mytext.text = "Level " + mylevelnumber;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Load (){
		//LevelHandler.leveldic
		CheckIfLocked (mylevelnumber);
	}
	void CheckIfLocked(int num){
		bool locked = LevelHandler.leveldic [num].islocked;
		Debug.Log (LevelHandler.leveldic [num].islocked);
		if (locked) {
			Debug.Log ("Do nothing"); //User hasn't unlocked the level
		}
		if (locked == false) {
			mysceneloading.LoadScene (mylevelnumber);
			Debug.Log ("Load");
		}
	}
}
