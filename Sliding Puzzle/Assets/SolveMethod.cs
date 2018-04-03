using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SolveMethod : MonoBehaviour {
	public static string[,] ogtiles = new string[8,8]; // Initial Map to test (Without Piece tiles)

	public static List<Solution> solutions= new List<Solution>(); //

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





	void Start(){
		Debug.Log(solutions.Count());
		ogtiles = AIBrain.tiles;
		Solve(ogtiles);
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
		initialgenes = new List<string>(); 														//initializes genes
		workers = new List<Worker>(); 		
		totalstoppedtiles = new List<Vector2>();												//initializes list of workers or "bots"
		startingposition = new Vector2(startx,starty);
		totalstoppedtiles.Add(startingposition);
		CheckAndCreate(startx, starty, initialgenes, totalstoppedtiles);						//Creates first worker (Unless he can move more than one place)
		if(workers.Count == 0){ 																//If Start tile is Walled up, No solution.
			Debug.Log("Cant");
			return;
		}
		for(int i= 0; i< workers.Count ;i++){													//Move all clones from CheckandCreate
			workers[i].Move();
		}
		lastgen = new List<Worker>(workers);	
		while(lastgen.Count>0){
			AWholeTurn();
		}
	}
	public void CheckAndCreate(int x, int y, List<string> newgenes, List<Vector2> newstoppedtiles){
		//workers.Clear();
		List <string> genes = newgenes;
		List <Vector2> mystoppedtiles = newstoppedtiles;
		if(ogtiles[x+1,y] == "Ice"){//Checking Right
		Debug.Log("Cloning Right");
		Worker worker = new Worker(0, x, y, ogtiles, "Right", genes, newstoppedtiles);
		workers.Add(worker);
		}
		if(ogtiles[x-1,y] == "Ice"){//Checking Left
			Debug.Log("Cloning Left");
		Worker worker = new Worker(0, x, y, ogtiles, "Left", genes, newstoppedtiles);
		workers.Add(worker);
		}
		if(ogtiles[x,y+1] == "Ice"){//Checking Down
		Debug.Log("Cloning Down");
		Worker worker = new Worker(0, x, y, ogtiles, "Down", genes, newstoppedtiles);
		workers.Add(worker);
		}
		if(ogtiles[x,y-1] == "Ice"){//Checking Up
		Debug.Log("Cloning Up");
		Worker worker = new Worker(0, x, y, ogtiles, "Up", genes, newstoppedtiles);
		workers.Add(worker);
		}
	}
	public void Anotherturn(){
		workers.Clear();																	//Clears worker class to repopulate with clones
			for(int i=0; i<lastgen.Count ; i++){ 												//for every one in previous gen
				curgenes = lastgen[i].mygenes;		
				currentstoppedtiles = new List<Vector2>(lastgen[i].stoppedtiles);											//Copy the genes of the worker for the new bots
				CheckAndCreate(lastgen[i].x, lastgen[i].y, curgenes, currentstoppedtiles);	
				Debug.Log(workers.Count);
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
}