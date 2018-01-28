using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swipedisplay : MonoBehaviour {

	public Text myText;
	// Use this for initialization
	void Start () {
		myText.text = Swiping.mydirection;
	}

	// Update is called once per frame
	void Update () {
			myText.text = Swiping.mydirection;
	}
}
