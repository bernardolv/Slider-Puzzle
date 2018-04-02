using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dragger : MonoBehaviour {
	private Vector3 screenPoint; 
	private Vector3 offset; 
	public GameObject tentativetile;
	Vector3 myPosition;
	SpriteRenderer tiletodropsprite;
	TileHandler tilescript;
	public bool needtooccupy;
	public GameObject newtile;
	private Vector3 restingpoint;

	void Start(){
		restingpoint = transform.position;
		AIBrain.pieces.Add(this.gameObject);

	}

	void Update(){
	}

	 void OnMouseDown() {
    //screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position); // I removed this line to prevent centring 
   // _lockedYPosition = screenPoint.y;
		if (TurnBehaviour.turn == 0) {
			Swiping.canswipe = false;
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
			Cursor.visible = false;
			Debug.Log (this.gameObject);
		}
		//notmoving = false;
 }
 
 void OnMouseDrag()
	{ 
		if (TurnBehaviour.turn == 0) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			// curPosition.x = _lockedYPosition;
			transform.position = curPosition;
			myPosition = transform.position;
			FindHoveredTile ();
			Touch t = Input.GetTouch(0);
				//save began touch 2d point
			Swiping.firstPressPos = new Vector2(t.position.x,t.position.y);
		}

	}
 
 void OnMouseUp()
 {
		Swiping.mydirection = "Null";
		//Swiping.canswipe = true;
		//Touch t = Input.GetTouch(0);
		//save began touch 2d point
		//Swiping.firstPressPos = new Vector2(transform.position.x,transform.position.y);


		if (TurnBehaviour.turn == 0) {
			
   	 Cursor.visible = true;
		if (tilescript.myTaker == null) {
			needtooccupy = true;
			transform.position = tentativetile.transform.position + new Vector3 (0, 0, -.01f);
			newtile = tentativetile;
		} 
		else {
			transform.position = restingpoint;
		}
		//float z = -.01f;
		//transform.position.z = z;
		}
		Touch t = Input.GetTouch(0);

		Swiping.firstPressPos = new Vector2(t.position.x,t.position.y);

		Swiping.canswipe = true;

 }
	void FindHoveredTile(){
		Collider2D[] colliders = Physics2D.OverlapCircleAll(myPosition, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
		foreach(Collider2D component in colliders){
			if (component.tag == "Ground") {
				tentativetile = component.gameObject;
				tilescript = tentativetile.GetComponent<TileHandler> ();
				//newtile = tentativetile;
				//tiletodropsprite = tentativetile.GetComponent<SpriteRenderer> ();
				//tiletodropsprite.color = Color.black;

				//tilescript.myTaker = this.gameObject;
				//tilescript.isTaken = true;
				//Debug.Log ("Pew");
				//mytile = tileobject;

			}
		}
	}
}