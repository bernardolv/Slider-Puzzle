using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SolveMethod : MonoBehaviour {
	public static string[,] ogtiles = new string[8,8]; // Initial Map to test (Without Piece tiles)

	public static string[,] newtiles = new string[8,8];

	public static string[,] solvingtiles = new string[8,8];

	public static List<Solution> solutions = new List<Solution>(); //

	public static int numberofsolutions;

	public static int startx;

	public static int starty;

	public static List <Worker> workers;

	public static List <string> initialgenes;


	public static List <Worker> lastgen;

	public static List<string> curgenes;

	public static List<Vector2> totalstoppedtiles;

	public static List<Vector2> currentstoppedtiles;

	public static Vector2 startingposition;

	public static List<Worker> workersalive;

	public static int turns;

	public static GameObject[] tiles;

	public static List <Vector2> Icetiles;

	public static string[,] currenttiles = new string[8,8];

	public static Vector2 currentGoal;

	public static List<Vector2> inLineTiles;

	public static Vector2 currenttest;




	void Start(){
			TryEverything();
		//Debug.Log("Solve");
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.O)){
			Anotherturn();
		}
		if(Input.GetKeyDown(KeyCode.P)){
			Moveworkers();
		}
		if(Input.GetKeyDown(KeyCode.Q)){
			CreateLastGen();
			Debug.Log(lastgen.Count);
		}
		if(Input.GetKeyDown(KeyCode.T)){
			AWholeTurn();
		}
	}

	public void Solve(string[,] tiles){		
		workersalive = new List<Worker>();		
		initialgenes = new List<string>(); 		
		lastgen = new List<Worker>();												//initializes genes
		workers = new List<Worker>(); 		
		curgenes = new List<string>();
		totalstoppedtiles = new List<Vector2>();												//initializes list of workers or "bots"
		startingposition = new Vector2(startx,starty);
		currenttiles = tiles;
		totalstoppedtiles.Add(startingposition);
		CheckAndCreate(0, startx, starty, currenttiles, initialgenes, totalstoppedtiles);						//Creates first worker (Unless he can move more than one place)
		if(workers.Count == 0){ 																//If Start tile is Walled up, No solution.
			Debug.Log("Cant");
			return;
		}
		for(int i= 0; i< workers.Count ;i++){													//Move all clones from CheckandCreate
			workers[i].Move();
		}
		lastgen = new List<Worker>(workersalive);	
		while(lastgen.Count>0){
			AWholeTurn();
		}
	}
	public void CheckAndCreate(int newturns, int x, int y,string[,] thistiles, List<string> newgenes, List<Vector2> newstoppedtiles){
		//workers.Clear();
		List <string> genes = newgenes;
		List <Vector2> mystoppedtiles = newstoppedtiles;
		turns = newturns;

		if(thistiles[x+1,y] != "Wall"){//Checking Right
		Debug.Log("Cloning Right");
		Worker worker = new Worker(turns, x, y, thistiles, "Right", genes, newstoppedtiles);
		workers.Add(worker);
		}
		if(thistiles[x-1,y] != "Wall" ){//Checking Left
			Debug.Log("Cloning Left");
		Worker worker = new Worker(turns, x, y, thistiles, "Left", genes, newstoppedtiles);
		workers.Add(worker);
		}
		if(thistiles[x,y+1] != "Wall"){//Checking Down
		Debug.Log("Cloning Down");
		Worker worker = new Worker(turns, x, y, thistiles, "Down", genes, newstoppedtiles);
		workers.Add(worker);
		}
		if(thistiles[x,y-1] != "Wall"){//Checking Up
		Debug.Log("Cloning Up");
		Worker worker = new Worker(turns, x, y, thistiles, "Up", genes, newstoppedtiles);
		workers.Add(worker);
		}
	}
	public void Anotherturn(){
		workers.Clear();																	//Clears worker class to repopulate with clones
			for(int i=0; i<lastgen.Count ; i++){ 												//for every one in previous gen
				curgenes = lastgen[i].mygenes;
				turns = lastgen[i].turns;		
				currentstoppedtiles = new List<Vector2>(lastgen[i].stoppedtiles);	
																		//Copy the genes of the worker for the new bots
				//Debug.Log(lastgen[i].mytiles[2,2]);
				CheckAndCreate(turns, lastgen[i].x, lastgen[i].y, lastgen[i].mytiles, curgenes, currentstoppedtiles);	
				//Debug.Log(workers.Count);
																								//Create new workers with genes + new gene depending on where they can go
				//lastgen = new List<Worker>(workers);											//New Generation Becomes the past.
			}
	}
	public void Moveworkers(){
		workersalive.Clear();
		for(int j = 0;j<workers.Count; j++ ){											//For every new worker
			workers[j].Move();										//Move new worker to the new gene
		}
	}
	public void CreateLastGen(){
		lastgen = new List<Worker>(workersalive);
		workersalive.Clear();
	}
	public void AWholeTurn(){
		Anotherturn();
		Moveworkers();
		CreateLastGen();
	}



	public void CyclePieceSolution(){
		string piecetag;
		//Debug.Log(AIBrain.pieces.Count);
		for(int i = 0; i<AIBrain.pieces.Count;i++){
			for(int j = 0; j<8; j++){
				for(int k= 0; k<8; k++){ //loop through all tiles ogtiles
					if(ogtiles[k,j]=="Ice"){
						//lastgen.Clear();
						Debug.Log("Solve at "+ k + "+" + j);
						currenttest = new Vector2(k, j);
						string[,] newtiles = (string[,]) AIBrain.tiles.Clone();
						newtiles[k,j] = AIBrain.pieces[i].tag;
						Debug.Log(startx + "+" + starty);
						Solve(newtiles);

					}
				}
			}
		}
	}
	public void CheckInLineStoppedTiles(){

	}
	public void TryEverything(){
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Ground");
		foreach(GameObject tileP in tiles){
			tileP.GetComponent<TileProperties>().GatherData();
		}
		turns = 0;
		Debug.Log(solutions.Count());
		ogtiles = AIBrain.tiles;
		Solve(ogtiles);
		CyclePieceSolution();
		if(solutions.Count == 0){
			Debug.Log("No solutions");
		}
		else{
			for(int i =0; i<solutions.Count; i++){
			Debug.Log(solutions[i].myturns + "Turns" + "with piece at" + solutions[i].x +  "+" + solutions[i].y );
			}
		}
	}
	/*public void findPossibletiles(Vector2 origin){

		CheckInLineTiles("Left", origin);
		CheckInLineTiles("Right", origin);
		CheckInLineTiles("Up", origin);
		CheckInLineTiles("Down", origin);

	}
	public void CheckInLineTiles(string direction, Vector2 origin){
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
	public void testdirect(Vector2 position, string direction, int x, int y){  //Gives tiles lined up in the stated direction.
		int myx = x;
		int myy = y;
		bool canstilltestposs = true;
		Vector2 origin = position;
		while(canstilltestposs==true){//Do this if theres a
			Vector2 newposition = position + new Vector2(myx,myy);
			//Collider2D[] colliders = null;
			//colliders = Physics2D.OverlapCircleAll(newposition, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
			//Debug.Log("Testing " + position);
			string newtiletag = solvingtiles[(int)newposition.x,(int)newposition.y];



			if (newtiletag == null || newtiletag == "Wall" || newtiletag == "Hole") {
				canstilltestposs =false;
			} else {	
				if (newtiletag == "Ice") {

				}
				else{
					tilesprite = Ground.GetComponent<SpriteRenderer>();
					tilesprite.color = Color.green;
					possibletiles.Add(Ground);
				}
			}
		}
		myy = myy+y;
		myx = myx+x;
	}
}
	/*public void solveBackwards(){
		for(int i = 0; i<8;i++){
			for(int j = 0; j<9; j++){
				solvingtiles[j,i] = ogtiles[j,i];
			}
		}
	}*/
}