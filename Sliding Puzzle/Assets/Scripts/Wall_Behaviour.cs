using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Behaviour : MonoBehaviour {

	Vector3 myPosition;
	GameObject tileobject;
	TileHandler tilescript;
	GameObject mytile;
	//bool istiletaken;


	// Use this for initialization
	void Start () {
		myPosition = transform.position;
		Invoke ("FindTileTag", .1f);
		//tilescript.isTaken = true;
		//tilescript.myTaker = this.gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {
		/*if (tilescript != null) {
			FindmyTileHandler ();
		}
	*/
	}
	void FindTileTag(){
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
	void FindmyTileHandler(){
		if (tilescript.myTaker != this.gameObject) {
			Debug.Log ("WHOUTHINKUIS");
		}
	}
}
