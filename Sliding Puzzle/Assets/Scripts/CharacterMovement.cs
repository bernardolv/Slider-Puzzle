using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	public Vector3 currenttile;
	public Vector3 startingposition;
	bool cantakeinput;
	public int speed;
	public string direction;
	bool istiletaken;
	public GameObject tileobject;
	TileHandler tilescript;
	Vector3 tiletotest;
	bool canmove;
	GameObject tiletaker;
	public string nextaction;
	public bool beingdragged;
	// Use this for initialization
	void Start () {
		//current tile works as a target to move to
		startingposition = transform.position;
		currenttile = transform.position;
		cantakeinput = true;
		canmove = true;
		nextaction = null;
		beingdragged = false;


	}
	
	// Update is called once per frame
	void Update () {
			Movement ();
		if (currenttile == transform.position) {
			if (nextaction == null) {
				cantakeinput = true;
				canmove = true;
			}
			if (nextaction == "Goal_Action") {
				/*currenttile = startingposition;
				transform.position = startingposition;
				cantakeinput = true;
				canmove = true;
				nextaction = null;*/
				LevelManager.levelnum++;
				int nextlevel = LevelManager.levelnum;
				LevelManager.NextLevel (nextlevel);

			}
		}
	}

	void QWERTYMove(){
		if (Input.GetKeyDown (KeyCode.W)) {
			tiletotest = currenttile;
			while (canmove == true) {
				tiletotest += Vector3.up;
				FindTileTag ();
				ActOnTile ();
			}
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			tiletotest = currenttile;
			while (canmove == true) {
				tiletotest += Vector3.left;
				FindTileTag ();
				ActOnTile ();
			}
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			tiletotest = currenttile;
			while (canmove == true) {
				tiletotest += Vector3.down;
				FindTileTag ();
				ActOnTile ();
			}
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			tiletotest = currenttile;
			while (canmove == true) {
				tiletotest += Vector3.right;
				FindTileTag ();
				ActOnTile ();
			}
		}
	}
	void FindTileTag(){
		Collider2D[] colliders = Physics2D.OverlapCircleAll(tiletotest, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
		foreach(Collider2D component in colliders){
			if (component.tag == "Ground") {
				tileobject = component.gameObject;
				tilescript = tileobject.GetComponent<TileHandler> ();
				istiletaken = tilescript.isTaken;
			}
		}
	}
	void Movement(){  
		//check for input when its your turn.
		if (cantakeinput) {
			QWERTYMove ();
		}
		//if the desired tile is not the place you're standing in it moves there
		if (currenttile != transform.position && beingdragged == false ) {
			transform.position = Vector3.MoveTowards (transform.position, currenttile, Time.deltaTime * speed); 
		}
	}
	//Individual Behaviours to be stored in the following.
	void ActOnTile(){
		if (istiletaken == false) {
			//move and keep moving i	f theres nothing but ice
			currenttile = tiletotest;
		} 
		else {
			if (tilescript.myTaker.tag == "Wall") {
				//the desired tile is the previous one and u stop looking for next tiles.
				canmove = false;
			}
			if (tilescript.myTaker.tag == "Goal") {
				//you'll stop in the tile you checked and stop moving.
				currenttile = tiletotest;
				canmove = false;
				//Qeue up an action when reaching the tile
				nextaction = "Goal_Action";
			}
			if (tilescript.myTaker.tag ==	 "Wood"){
				currenttile = tiletotest;
			}
				else {
				canmove = false;
			}
		}
	}
}
