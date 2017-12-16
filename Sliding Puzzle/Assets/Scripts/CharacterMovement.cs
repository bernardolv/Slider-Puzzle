﻿using System.Collections;
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
	public GameObject lastFragile;
	public static bool isspeeding;
	public static string character_direction;
	// Use this for initialization
	void Start () {
		//current tile works as a target to move to
		startingposition = transform.position;
		currenttile = transform.position;
		cantakeinput = true;
		canmove = true;
		nextaction = null;
		beingdragged = false;
		lastFragile = null;
		isspeeding = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position != startingposition && TurnBehaviour.turn == 0) {
			TurnBehaviour.turn = 1;
		}
			Movement ();
		if (currenttile == transform.position) {
			//Debug.Log (tilescript.myTaker.tag);
			if (lastFragile != null && lastFragile.transform.position == transform.position) {
				Debug.Log ("Kaplunk");
				int nextlevel = LevelManager.levelnum;
				LevelManager.NextLevel (nextlevel);
			}
			else if (nextaction == null) {
				cantakeinput = true;
				canmove = true;
				isspeeding = false;
			}  
			else if (nextaction == "Goal_Action") {
				/*currenttile = startingposition;
				transform.position = startingposition;
				cantakeinput = true;
				canmove = true;
				nextaction = null;*/
				LevelManager.levelnum++;
				int nextlevel = LevelManager.levelnum;
				LevelManager.NextLevel (nextlevel);

			}			
			else if (nextaction == "Hole_Action") {
				/*currenttile = startingposition;
				transform.position = startingposition;
				cantakeinput = true;
				canmove = true;
				nextaction = null;*/
				int nextlevel = LevelManager.levelnum;
				LevelManager.NextLevel (nextlevel);

			}
			else if (nextaction == "Left_Action") {
				tiletotest = currenttile;
				canmove = true;
				while (canmove == true) {
					tiletotest += Vector3.left;
					FindTileTag ();
					ActOnTile ();
					isspeeding = true;
				}
				if (nextaction == "Left_Action") {
					nextaction = null;
				}
			}
			else if (nextaction == "Right_Action") {
				tiletotest = currenttile;
				canmove = true;
				while (canmove == true) {
					tiletotest += Vector3.right;
					FindTileTag ();
					ActOnTile ();
					isspeeding = true;
				}
				if (nextaction == "Right_Action") {
					nextaction = null;
				}
			}
			else if (nextaction == "Up_Action") {
				tiletotest = currenttile;
				canmove = true;
				while (canmove == true) {
					tiletotest += Vector3.up;
					FindTileTag ();
					ActOnTile ();
					isspeeding = true;
				}
				if (nextaction == "Up_Action") {
					nextaction = null;
				}
			}
			else if (nextaction == "Down_Action") {
				tiletotest = currenttile;
				canmove = true;
				while (canmove == true) {
					tiletotest += Vector3.down;
					FindTileTag ();
					ActOnTile ();
					isspeeding = true;
				}
				if (nextaction == "Down_Action") {
					nextaction = null;
				}
			}
		}
	}

	void QWERTYMove(){
		if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
			tiletotest = currenttile;
			while (canmove == true) {
				character_direction = "Up";
				tiletotest += Vector3.up;
				FindTileTag ();
				ActOnTile ();
			}
		}
		if (Input.GetKeyDown (KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow)) {
			tiletotest = currenttile;
			while (canmove == true) {	
				character_direction = "Left";

				tiletotest += Vector3.left;
				FindTileTag ();
				ActOnTile ();
			}
		}
		if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
			tiletotest = currenttile;
			while (canmove == true) {
				character_direction = "Down";
				tiletotest += Vector3.down;
				FindTileTag ();
				ActOnTile ();
			}
		}
		if (Input.GetKeyDown (KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow)) {
			tiletotest = currenttile;
			while (canmove == true) {
				character_direction = "Right";
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
			} else if (tilescript.myTaker.tag == "Goal") {
				//you'll stop in the tile you checked and stop moving.
				currenttile = tiletotest;
				canmove = false;
				//Qeue up an action when reaching the tile
				nextaction = "Goal_Action";
			} else if (tilescript.myTaker.tag == "Hole") {
				//you'll stop in the tile you checked and stop moving.
				currenttile = tiletotest;
				canmove = false;
				//Qeue up an action when reaching the tile
				nextaction = "Hole_Action";
			} else if (tilescript.myTaker.tag == "Wood") {
				currenttile = tiletotest;
				Debug.Log ("Pink");
				//canmove = true;
			} else if (tilescript.myTaker.tag == "Left") {
				currenttile = tiletotest;
				canmove = false;
				nextaction = "Left_Action";
				isspeeding = true;

			} else if (tilescript.myTaker.tag == "Right") {
				currenttile = tiletotest;
				canmove = false;
				nextaction = "Right_Action";
				isspeeding = true;
			} else if (tilescript.myTaker.tag == "Up") {
				currenttile = tiletotest;
				canmove = false;
				nextaction = "Up_Action";
				isspeeding = true;

			} else if (tilescript.myTaker.tag == "Down") {
				currenttile = tiletotest;
				canmove = false;
				nextaction = "Down_Action";
				isspeeding = true;

			} else if (tilescript.myTaker.tag == "Fragile") {
				currenttile = tiletotest;
				lastFragile = tilescript.myTaker;
				tilescript.myTaker.tag = "Hole";
		
			} else if (tilescript.myTaker.tag == "Quicksand") {
				currenttile = tiletotest;
				lastFragile = tilescript.myTaker;
				if (isspeeding == false) {
					currenttile = tiletotest;
					canmove = false;
					nextaction = "Hole_action";
				}

			} else if (tilescript.myTaker.tag == "Boss") {
				currenttile = tiletotest;
				canmove = false;
				/*if (character_direction == "Up") {
					Boss_Behaviour.bosstile.y++;
				}
				if (character_direction == "Right") {
					Boss_Behaviour.bosstile.x++;
				}
				if (character_direction == "Left") {
					Boss_Behaviour.bosstile.x--;
				}
				if (character_direction == "Down") {
					Boss_Behaviour.bosstile.y--;
				}*/


			}
				else {
				Debug.Log ("Dong");
				canmove = false;
			}
		}
	}
}
