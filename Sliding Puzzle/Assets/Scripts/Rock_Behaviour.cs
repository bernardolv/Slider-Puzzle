using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Behaviour : MonoBehaviour {

	public Vector3 myPosition;
	GameObject tileobject;
	TileHandler tilescript;
	bool readyfordrag;
	GameObject mytile;
	public bool notmoving;
	//bool istiletaken;
	GameObject pasttile;
	TileHandler pasttilescript;
	public Dragger myDragger;


	// Use this for initialization
	void Start () {
		myPosition = transform.position;
		Invoke ("GiveTileTag", .3f);
		//readyfordrag = false;

		//tilescript.isTaken = true;
		//tilescript.myTaker = this.gameObject;

	}

	// Update is called once per frame
	void Update () {
		if (myPosition != transform.position && notmoving) {

			//myPosition = transform.position;
		}
		if (myDragger.newtile == null && mytile != null) {
			myDragger.newtile = mytile;
		}
		//if (myDragger.newtile != mytile) {
			//Debug.Log ("Not my tile");
		//}
		if(myDragger.needtooccupy ==true){
			Debug.Log ("occupy");
			myPosition = transform.position;
			GiveTileTag ();
			myDragger.needtooccupy = false;
		}

	}
	void GiveTileTag(){
		Collider2D[] colliders = Physics2D.OverlapCircleAll(myPosition, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
		foreach(Collider2D component in colliders){
			if (component.tag == "Ground") {
				tileobject = component.gameObject;
				tilescript = tileobject.GetComponent<TileHandler> ();
				tilescript.myTaker = this.gameObject;
				tilescript.isTaken = true;
				Debug.Log ("Pew");
				mytile = tileobject;

			}
		}
	}
	void TouchAndDrag(){
	}
}
