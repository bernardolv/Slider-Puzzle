using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Behaviour : MonoBehaviour {

	public Vector3 myPosition;
	GameObject tileobject;
	TileHandler tilescript;
	bool readyfordrag;
	//bool istiletaken;
	bool stillastarttile;
	public GameObject myPlayer;
	Vector3 difference; 
	SpriteRenderer mySprite;
	public Sprite newSprite;
	public Sprite ogSprite;
	Sprite currentsprite; 
	Wall_Behaviour myWallBehaviour;
	public float playerx;
	public float playery;
	public float mypositionx;
	public float mypositiony;
	SpriteRenderer mysr;
	GameObject mytile;
	// Use this for initialization
	void Start () {
		myPosition = transform.position;
		//Invoke ("FindTileTag", .3f);
		readyfordrag = false;
		stillastarttile = true;
		//tilescript.isTaken = true;
		//tilescript.myTaker = this.gameObject;
		mySprite = GetComponent<SpriteRenderer>();
		myWallBehaviour = GetComponent<Wall_Behaviour>();
		ogSprite = GetComponent<SpriteRenderer>().sprite;
		//currentsprite = GetComponent<SpriteRenderer>().Sprite;
	}

	// Update is called once per frame
	void Update () {
		if (stillastarttile&&myPlayer!=null) {
			FindTileTag ();
			difference = transform.position - myPlayer.transform.position;
			//Debug.Log(difference);
			float xdif = difference.x;
			float ydif = difference.y;
			if (Mathf.Abs (xdif) + Mathf.Abs (ydif) >= 1f) {
				mysr = GetComponent<SpriteRenderer>();
				mysr.enabled = true;
				Debug.Log ("Too big man");
				mySprite.sprite = newSprite;
				gameObject.tag = "Wall";
				myWallBehaviour.enabled = true;
				stillastarttile = false;
				FindTileTag();
				GameObject objectp = GameObject.Find("TheCanvas");
				IceTileHandler myhandler = objectp.GetComponent<IceTileHandler>();	
				myhandler.GiveIce();	
			}
		}

		//playerx= myPlayer.transform.position.x;
		//playery = myPlayer.transform.position.y;
		mypositionx = myPosition.x;
		mypositiony = myPosition.y;
		if (mypositionx == playerx && mypositiony == playery && stillastarttile == false) {
			Debug.Log ("BISH");
			mySprite.sprite = ogSprite;
			myWallBehaviour.enabled = false;
			stillastarttile = true;
		}
		//	CheckForPlayerAndLeave ();

	}
	void FindTileTag(){
		Collider2D[] colliders = Physics2D.OverlapCircleAll(myPosition, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
		foreach(Collider2D component in colliders){
			if (component.tag == "Player") {
				//Debug.Log ("Aooga Aooga");
				myPlayer = component.gameObject;

			}
			if (component.tag == "Ground" ) {
				//Debug.Log ("Bye");
				tileobject = component.gameObject;
				tilescript = tileobject.GetComponent<TileHandler> ();
				tilescript.myTaker = this.gameObject;
				tilescript.isTaken = true;
			}





		}
	}
	void TouchAndDrag(){
	}
	
	/*void CheckForPlayerAndLeave(){
		Collider2D[] colliders = Phy	sics2D.OverlapCircleAll(myPosition, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
		foreach(Collider2D component in colliders){
			if (component.tag == "Ground") {
				tileobject = component.gameObject;
				tilescript = tileobject.GetComponent<TileHandler> ();
				tilescript.myTaker = this.gameObject;
				tilescript.isTaken = true;
				Debug.Log ("Pew");
			}
		}
	}*/
}