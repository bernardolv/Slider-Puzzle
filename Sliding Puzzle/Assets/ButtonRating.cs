using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRating : MonoBehaviour {
	Text mytext;
	public int myrating;
	int mynumber;
	LevelButton mylevelbutton;

	// Use this for initialization
	void Start () {
		mytext = GetComponent<Text> ();
		mylevelbutton = GetComponentInParent<LevelButton> ();
		mynumber = mylevelbutton.mylevelnumber;
		//myrating = LevelHandler.leveldic[mynumber]
		string prefcheck = "Level"+mynumber+"Rating";
		myrating  = PlayerPrefs.GetInt(prefcheck);
		//LevelHandler.leveldic[
		Debug.Log ("prefc    " + prefcheck);
		Debug.Log (myrating);
		mytext.text = myrating.ToString();
		this.gameObject.gameObject.name = "Rating" + mynumber;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
