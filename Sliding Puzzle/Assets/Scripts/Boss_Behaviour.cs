using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Behaviour : MonoBehaviour {

	Vector3 myPosition;
	GameObject tileobject;
	TileHandler tilescript;
	public static Vector3 bosstile;
	GameObject mytile;
	GameObject player;
	int speed = 5;
	public float x;
	public float y;
	bool isclose;
	bool isnotmoving;

	//bool istiletaken;


	// Use this for initialization
	void Start () {

		bosstile = transform.position;
		myPosition = transform.position;
		Invoke ("FindTileTag", .2f);
		//tilescript.isTaken = true;
		//tilescript.myTaker = this.gameObject;
		player = GameObject.FindGameObjectWithTag ("Player");
		isnotmoving = true;


	}

	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}
		if (Vector3.Distance (transform.position, player.transform.position) < 1 && isnotmoving) {
			Debug.Log (Vector3.Distance (transform.position, player.transform.position));
			Move ();
		}

		if (transform.position != bosstile) {// && isclosejoh
			myPosition = bosstile;
			FindTileTag ();
			transform.position = Vector3.MoveTowards (transform.position, bosstile, Time.deltaTime * speed); 
		}
		if (transform.position == bosstile && isnotmoving == false) {
			isnotmoving = true;
		}
	}
	void FindTileTag(){
		Collider2D[] colliders = Physics2D.OverlapCircleAll(myPosition, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
		foreach(Collider2D component in colliders){
			if (component.tag == "Ground") {
				tileobject = component.gameObject;
				tilescript = tileobject.GetComponent<TileHandler> ();
				tilescript.myTaker = this.gameObject;
				tilescript.isTaken = true;
				Debug.Log ("chanchanchaaaan");
				mytile = tileobject;

			}
		}
	}
	void Move(){
		if ( CharacterMovement.character_direction == "Up") {
			Boss_Behaviour.bosstile.y++;
		}
		if (CharacterMovement.character_direction == "Right") {
			Boss_Behaviour.bosstile.x++;
		}
		if (CharacterMovement.character_direction == "Left") {
			Boss_Behaviour.bosstile.x--;
		}
		if (CharacterMovement.character_direction == "Down") {
			Boss_Behaviour.bosstile.y--;
		}
		isnotmoving = false;

	}
}
