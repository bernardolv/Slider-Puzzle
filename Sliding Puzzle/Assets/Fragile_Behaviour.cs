using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragile_Behaviour : MonoBehaviour {

	public Sprite after;
	public Sprite before;
	public bool isafter;
	public bool ishole;
	public Rock_Behaviour mybehaviour;

	// Use this for initialization
	void Start () {
		isafter = false;
		ishole = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.tag == "Hole" && ishole!= true) {
			SpriteRenderer mySpriter = GetComponent<SpriteRenderer> ();
			mySpriter.sprite = after;
			ishole = true;
			mybehaviour.MakeHole ();	
		}
	}

}
