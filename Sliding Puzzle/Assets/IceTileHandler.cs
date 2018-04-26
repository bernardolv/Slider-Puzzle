using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTileHandler : MonoBehaviour {
	public  Sprite[] vanillasprites;
	public  Sprite leftuppercorner;
	public  Sprite leftbottomcorner;
	public  Sprite rightuppercorner;
	public  Sprite rightbottomcorner;
	public  Sprite[] leftwalls; 
	public  Sprite[] upperwalls;
	public  Sprite[] bottomwalls;
	public  Sprite[] rightwalls;
	public  Sprite verticalpath;
	public  Sprite horizontalpath;
	public Vector3 tiletotest;
	bool leftwall;
	bool upperwall;
	bool rightwall;
	bool bottomwall;
	//bool leftgoal
	int truecounter;

	// Use this for initialization
	void Start () {
		//LevelManager.myicehandler = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Z)){
			GiveIce();
		}
	}

	public void GiveIce(){
		Debug.Log("givingice");
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Ground");
		foreach (GameObject icetile in tiles){
			//Debug.Log("BOOP");
			icetile.GetComponent<SpriteRenderer>().sprite = vanillasprites[Random.Range(0, vanillasprites.Length-1)];
//			Debug.Log(icetile.GetComponent<SpriteRenderer>().sprite);
			if(icetile.GetComponent<TileHandler>().isTaken == true){
				Debug.Log("Taken");
			}
			else{
				Checksidesandplace(icetile.transform.position, icetile);
			}
		}

	}
	public void Checksidesandplace(Vector3 tocheck, GameObject tile){
		truecounter = 0;
		leftwall = false;
		upperwall = false;
		rightwall = false;
		bottomwall = false;
		if(FindTileTag(tocheck+Vector3.left)){
			leftwall = true;
		}
		if(FindTileTag(tocheck+Vector3.right)){
			rightwall = true;
		}
		if(FindTileTag(tocheck+Vector3.up)){
			upperwall = true;
		}
		if(FindTileTag(tocheck+Vector3.down)){
			bottomwall = true;
		}
		/*if(FindGoalTag(tocheck+Vector3.left)){
			leftwall = true;
		}
		if(FindGoalTag(tocheck+Vector3.right)){
			rightwall = true;
		}
		if(FindGoalTag(tocheck+Vector3.up)){
			upperwall = true;
		}
		if(FindGoalTag(tocheck+Vector3.down)){
			bottomwall = true;
		}*/
		if(truecounter == 1){
			if(upperwall)
			{
				tile.GetComponent<SpriteRenderer>().sprite = upperwalls[Random.Range(0, upperwalls.Length-1)];
			}
			if(bottomwall)
			{
				tile.GetComponent<SpriteRenderer>().sprite = bottomwalls[Random.Range(0, bottomwalls.Length-1)];
			}
			if(leftwall)
			{
				tile.GetComponent<SpriteRenderer>().sprite = leftwalls[Random.Range(0, leftwalls.Length-1)];
			}
			if(rightwall)
			{
				tile.GetComponent<SpriteRenderer>().sprite = rightwalls[Random.Range(0, rightwalls.Length-1)];
			}
		}
		Debug.Log(tocheck.x);
		Debug.Log(tocheck.y);
		Debug.Log(leftwall + "+" + upperwall + "+" + rightwall + "+" + bottomwall );

	}

	public void CheckTakenSides(){

	}

	public bool FindTileTag(Vector3 origin){
		Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
		if(colliders.Length == 0){
			//Debug.Log("1");
			return false;
		}
		else{
			foreach (Collider2D component in colliders) {
				if (component.tag == "Ground") {
					if(component.GetComponent<TileHandler>().isTaken == true && component.GetComponent<TileHandler>().myTaker.tag == "Wall"){
						truecounter++;
						return true;
					}
					else
						//Debug.Log("2");
						return false;
				} 
				/*else{
					Debug.Log("3");
					return false;
				}*/
			}
			//Debug.Log("4");
			return false;
		}
	}
	/*public bool FindGoalTag(Vector3 origin){
		Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
		if(colliders.Length == 0){
			return false;
		}
		else{
			foreach (Collider2D component in colliders) {
				if (component.tag == "Goal" || component.tag == "Start") {
					if(component.GetComponent<TileHandler>().isTaken == true){
						
					return true;
					}
					else
						return false;
				} 
				else{
					return false;
				}
			}
			return false;
		}
	}*/

}
