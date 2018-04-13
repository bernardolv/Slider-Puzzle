using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BotMovement : MonoBehaviour {

	public Vector3 currenttile;
	public Vector3 startingposition;
	public bool cantakeinput;
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
	public GameObject lastSeed;
	Seed_Behaviour myseedbehaviour;
	bool firstmove; //used to count turns
	public GameObject levelWonBoard;
	public GameObject LevelLostBoard;
	RatingPopUp PopupScript;
	string myswipe;
	bool outofmap;
	public KeySimulator mykeysimulator;
	public BotTurns myturns;
	public Brain mybrain;
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
		levelWonBoard = GameObject.Find ("GameWon");
		if (levelWonBoard != null) {
			SceneLoading.gamewon = levelWonBoard;
		}
		levelWonBoard.SetActive (false);
		LevelLostBoard = GameObject.Find ("GameLost");
		if (LevelLostBoard != null) {
			SceneLoading.gamelost = LevelLostBoard;
		}
		LevelLostBoard.SetActive (false);
		Debug.Log ("TURNEDTOFF");
		outofmap = false;
		myturns = GetComponentInChildren<BotTurns> ();
	}

	// Update is called once per frame
	void Update () {
		myswipe = Swiping.mydirection;

//		Debug.Log (levelWonBoard);
		if (levelWonBoard == null) {
			levelWonBoard = SceneLoading.gamewon;
		}
		if (LevelLostBoard == null) {
			LevelLostBoard = SceneLoading.gamelost;
			LevelLostBoard.SetActive (false);
		}
//		Debug.Log (TurnBehaviour.turn + "BEHAV");
		/*if (transform.position != startingposition && TurnBehaviour.turn == 0) {
			TurnBehaviour.turn = 1;

		}
		if (TurnBehaviour.turn == 1 && transform.position == startingposition) {
			TurnBehaviour.turn = 0; 
		}*/
		if (currenttile == transform.position) {
			//Debug.Log (tilescript.myTaker.tag);
			if (lastFragile != null && lastFragile.transform.position == transform.position) {
				Debug.Log ("UNUL");
				//this.enabled = false;
				int nextlevel = LevelManager.levelnum;
				LevelManager.NextLevel (nextlevel);
			}
			else if (nextaction == null) {
				cantakeinput = true;
				canmove = true;
				isspeeding = false;
			}  
			else if (nextaction == "Goal_Action") {
				int mynumofsolution = SolveMethod.solutions.Count() + 1;
				int goalturns = myturns.turns;
				Solution mysolution = new Solution(mynumofsolution, goalturns, mybrain.genes, SolveMethod.currenttest);
				Debug.Log("Added solution number " + mynumofsolution + " with Turns: " + goalturns);
				//Debug.Log("Goal");
				//Debug.Log(mybrain.genes);

				//Give Data to Population Manager, with DNA, turns.
				//levelWonBoard.SetActive (true);
				//RatingPopUp.GiveRating ();
				//this.enabled = false;
				//Debug.Log ("Whatsgoingon");
			}			
			else if (nextaction == "Hole_Action") {
				LevelLostBoard.SetActive (true);
				//int nextlevel = LevelManager.levelnum;
				this.enabled=false;
				//LevelManager.NextLevel (nextlevel);
				//here popup "Gameover" try again

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
		Movement ();
	}

	void QWERTYMove(){
		if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || myswipe == "Up" || mykeysimulator.W==true ) {
			tiletotest = currenttile;
			if (canmove = true) {
				firstmove = true;
			}
			while (canmove == true) {
				character_direction = "Up";
				tiletotest += Vector3.up;
				FindTileTag ();
				ActOnTile ();
			}
			mykeysimulator.W = false;
		}
		if (Input.GetKeyDown (KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow) || myswipe == "Left" || mykeysimulator.A == true) {
			tiletotest = currenttile;
			if (canmove = true) {
				firstmove = true;
			}
			while (canmove == true) {	
				character_direction = "Left";

				tiletotest += Vector3.left;
				FindTileTag ();
				ActOnTile ();
			}
			mykeysimulator.A = false;
		}
		if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || myswipe == "Down" || mykeysimulator.S == true ) {
			tiletotest = currenttile;
			if (canmove = true) {
				firstmove = true;
			}
			while (canmove == true) {
				character_direction = "Down";
				tiletotest += Vector3.down;
				FindTileTag ();
				ActOnTile ();
			}
			mykeysimulator.S = false;
		}
		if (Input.GetKeyDown (KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow) || myswipe == "Right" || mykeysimulator.D == true) {
			Debug.Log ("Moving right");
			tiletotest = currenttile;
			if (canmove = true) {
				firstmove = true;
			}
			while (canmove == true) {
				character_direction = "Right";
				tiletotest += Vector3.right;
				FindTileTag ();
				ActOnTile ();
			}
			Debug.Log ("unpressed right");
			mykeysimulator.D = false;
		}
	}
	void FindTileTag(){
		Collider2D[] colliders = Physics2D.OverlapCircleAll(tiletotest, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
		if(colliders.Length == 0){
			istiletaken = true;
			outofmap = true;
		}
		else{
			foreach (Collider2D component in colliders) {
				if (component.tag == "Ground") {
					tileobject = component.gameObject;
					tilescript = tileobject.GetComponent<TileHandler> ();
					istiletaken = tilescript.isTaken;
				} 
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
			cantakeinput = false;
			Swiping.mydirection = "Null";
		}
	}
	void Count(){
		if(firstmove == true && canmove == true){
			//TurnCounter.turncount++;
			myturns.AddTurn();    
			Debug.Log (TurnCounter.turncount);
		}
		if(firstmove == true){
			firstmove = false;
		}
		RatingBehaviour.CalculateRating ();
	}
	//Individual Behaviours to be stored in the following.
	void ActOnTile(){
		if (istiletaken == false) {
			//move and keep moving i	f theres nothing but ice
			Count ();
			currenttile = tiletotest;
		} 
		else {
			if(outofmap == true){
				canmove = false;
				outofmap = false;
			}
			else if (tilescript.myTaker.tag == "Wall") {
				//the desired tile is the previous one and u stop looking for next tiles.
				canmove = false;
				Count ();
			} else if (tilescript.myTaker.tag == "Goal") {
				Debug.Log("GOGOGOL");
				//you'll stop in the tile you checked and stop moving.
				if (myturns.turns == 0) {
					canmove = false;
				} else {
					Count ();
					currenttile = tiletotest;
					canmove = false;
					//Qeue up an action when reaching the tile
					nextaction = "Goal_Action";
				}
			} else if (tilescript.myTaker.tag == "Hole") {
				//you'll stop in the tile you checked and stop moving.
				currenttile = tiletotest;
				canmove = false;
				//Qeue up an action when reaching the tile
				nextaction = "Hole_Action";
			} else if (tilescript.myTaker.tag == "Wood") {
				Count ();
				currenttile = tiletotest;
				Debug.Log ("Pink");
				//canmove = true;
			} else if (tilescript.myTaker.tag == "Left") {
				Count ();
				currenttile = tiletotest;
				canmove = false;
				nextaction = "Left_Action";
				isspeeding = true;

			} else if (tilescript.myTaker.tag == "Right") {
				Count ();
				currenttile = tiletotest;
				canmove = false;
				nextaction = "Right_Action";
				isspeeding = true;
			} else if (tilescript.myTaker.tag == "Up") {
				Count ();
				currenttile = tiletotest;
				canmove = false;
				nextaction = "Up_Action";
				isspeeding = true;

			} else if (tilescript.myTaker.tag == "Down") {
				Count ();
				currenttile = tiletotest;
				canmove = false;
				nextaction = "Down_Action";
				isspeeding = true;

			} else if (tilescript.myTaker.tag == "Fragile") {
				Count ();
				currenttile = tiletotest;
				lastFragile = tilescript.myTaker;
				tilescript.myTaker.tag = "Hole";

			} else if (tilescript.myTaker.tag == "Quicksand") {
				currenttile = tiletotest;
				lastFragile = tilescript.myTaker;
				Count ();
				if (isspeeding == false) {
					currenttile = tiletotest;
					canmove = false;
					nextaction = "Hole_action";
				}

			}
			else if (tilescript.myTaker.tag == "Seed") {
				currenttile = tiletotest;
				lastSeed = tilescript.myTaker;
				myseedbehaviour = lastSeed.GetComponent<Seed_Behaviour> ();
				myseedbehaviour.Unseed ();


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
			Count ();

		}
	}
}
