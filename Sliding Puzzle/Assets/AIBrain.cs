using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour {

	public static List<GameObject> pieces = new List <GameObject>();

	public static string [,] tiles = new string[8,8];

	public static int Goalx;
	public static int Goaly;

	public bool canstilltestposs;
	bool haventturnedon;
	bool haspath = false;
	
	public SpriteRenderer tilesprite;

	public static GameObject goaltile;

	public static List <GameObject> stoppedtiles = new List<GameObject>();
	public static List <GameObject> possibletiles = new List <GameObject>(); //Shows tiles where you can land before the desired goal.

	public PopulationManager mypopmanager;


	// Use this for initialization
	void Start () {
		haventturnedon = true;
	}
	
	public void DoAll(){
		StartCoroutine(Turn());
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.J)) {
			if(haventturnedon){
			turnOnPossibilities("Up");//shows where up would have to go to get to it.
			turnOnPossibilities("Down");
			turnOnPossibilities("Left");
			turnOnPossibilities("Right");
			haventturnedon = false;
			Debug.Log(PopulationManager.populationsize);
		}
			ConvertStoppedtoGameObjects(PopulationManager.UniqueStoppedTiles);
			filterandorder();
		} 	
			if (Input.GetKeyDown (KeyCode.K)) {
				PrintList(tiles);
			}

	}
	void PrintList(string[,] mylist){
		for(int x = 0; x< 8; x++){
			for(int y = 0; y< 8 ; y++){
				Debug.Log(mylist[x,y]);
			}
		}
	}
		void PrintPieces(List <GameObject> mylist){
		for(int i = 0; i< mylist.Count; i++){
				Debug.Log(mylist[i].tag);
		}
	}
	public IEnumerator Turn(){
		mypopmanager.turnOnBrain();
		PrintPieces(pieces);
		yield return new WaitForSeconds(1);
		mypopmanager.AWholeturn();
		yield return new WaitForSeconds(8);
		while(PopulationManager.populationsize>0){
			mypopmanager.AWholeturn();
			yield return new WaitForSeconds(8);
		}
		if(haventturnedon){
			turnOnPossibilities("Up");//shows where up would have to go to get to it.
			turnOnPossibilities("Down");
			turnOnPossibilities("Left");
			turnOnPossibilities("Right");
			haventturnedon = false;
			Debug.Log(PopulationManager.populationsize);
		}
			ConvertStoppedtoGameObjects(PopulationManager.UniqueStoppedTiles);
			filterandorder();



	}

	public void turnOnPossibilities(string direction){//checks all directions
		Vector3 origin = new Vector3(Goalx,-Goaly,0);
//		Debug.Log("Position" + origin);
		if (direction =="Left"){
			testdirect(origin,"Left", -1, 0);
		}
		if (direction =="Right"){
			testdirect(origin,"Right",1,0);
		}
		if (direction =="Up"){
			testdirect(origin, "Up", 0, -1);
			
		}
		if (direction =="Down"){
			testdirect(origin,"Down",0,1);
		}
	}
	public void testdirect(Vector3 position, string direction, int x, int y){  //Gives tiles lined up in the stated direction.
		//Debug.Log ("Trying to clone");
		int myx = x;
		int myy = y;
		canstilltestposs = true;
		Vector3 origin = position;
		while(canstilltestposs==true){//Do this if theres a
			Vector3 newposition = position + new Vector3(myx,myy,0);
			Collider2D[] colliders = null;
			colliders = Physics2D.OverlapCircleAll(newposition, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
			//Debug.Log("Testing " + position);
			if (colliders.Length == 0) {
				canstilltestposs =false;
			} else {
				foreach (Collider2D component in colliders) {
					if (component.tag == "Ground") {
						GameObject Ground = component.gameObject;
						TileHandler tilescript = Ground.GetComponent<TileHandler> ();
						if (tilescript.myTaker != null ) {
							GameObject Taker = tilescript.myTaker;
							if (Taker.tag == "Wall") {
								canstilltestposs=false;
							} else {
								tilesprite = Ground.GetComponent<SpriteRenderer>();
								tilesprite.color = Color.green;
								possibletiles.Add(Ground);
							}
						}
						else{
								tilesprite = Ground.GetComponent<SpriteRenderer>();
								tilesprite.color = Color.green;
								possibletiles.Add(Ground);
						}
					}
				}
			}
			myy = myy+y;
			myx = myx+x;
//			Debug.Log("Bouttodrop");

		//	filterandorder();
		}
	}
	public void filterandorder(){
			for(int i = 0; i < possibletiles.Count; i++){
				haspath = false;
				checkifinline(possibletiles[i]);
				//checkifinline();
			}
	}
	public void checkifinline(GameObject tiletotest){
		Debug.Log("BOUTTOTOTOTO");
		if(stoppedtiles.Count<1){
				Debug.Log("empty tiles");
				tilesprite = tiletotest.GetComponent<SpriteRenderer>();
				tilesprite.color = Color.white;
			}
		for(int i = 0; i<stoppedtiles.Count; i++){
			if(tiletotest.transform.position.x == stoppedtiles[i].transform.position.x || tiletotest.transform.position.y == stoppedtiles[i].transform.position.y){
				tilesprite = tiletotest.GetComponent<SpriteRenderer>();
				tilesprite.color = Color.green;
				haspath = true;
				Debug.Log(stoppedtiles[i].transform.position + "+" + tiletotest.transform.position);
			}
			else if(haspath == false){
				Debug.Log("GETOUT");
				tilesprite = tiletotest.GetComponent<SpriteRenderer>();
				tilesprite.color = Color.white;
			}
		}
	}
	public void ConvertStoppedtoGameObjects(List<Vector3> Vectorlist){
		for(int i = 0; i<Vectorlist.Count; i++){
			Vector3 newposition = Vectorlist[i];
			Collider2D[] colliders = null;
			colliders = Physics2D.OverlapCircleAll(newposition, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
			//Debug.Log("Testing " + position);
			if (colliders.Length == 0) {
				canstilltestposs =false;
			} else {
				foreach (Collider2D component in colliders) {
					if (component.tag == "Ground") {
						GameObject Ground = component.gameObject;
						TileHandler tilescript = Ground.GetComponent<TileHandler> ();
						if (tilescript.myTaker != null ) {
							GameObject Taker = tilescript.myTaker;
							if (Taker.tag == "Wall") {

							} else {
								stoppedtiles.Add(Ground);
							}
						}
						else{
								stoppedtiles.Add(Ground);
						}
					}
				}
			}
		}
	}
	public void placepossible(){

	}
}
