using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SolveMethod : MonoBehaviour {
	public static string[,] ogtiles = new string[8,8]; // Initial Map to test (Without Piece tiles)

	//public static string[,] newtiles = new string[8,8];

	public static string[,] solvingtiles = new string[8,8];

	public static List<Solution> solutions = new List<Solution>(); //

	public static List<int> bestsolutions  = new List<int>();//shows string of numerical value forbest turn solution with 0 tiles, 1 tiles ... n tiles.

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

	public static Vector2 currenttest = new Vector2(0,0);

	public static Vector2 currenttest2 = new Vector2(0,0);

	public static bool cycling;

	public static int bestsol;

	public static float[] classifier;

	public static float dsol;

	public static List<Vector2> possibletiles = new List<Vector2>();

	public static bool presolutions;

	public static bool repeatedbestsol =false;

	public static List<Vector2> solutionpieceposition = new List<Vector2>();

	public static List<string> solutionpiecenames = new List<string>();

	public static List<bool> besthaswallhug = new List<bool>();

	public static Solution bestsolution;


	void Start(){
			//TryEverything();
		//Debug.Log("Solve");
	} 

	void Update(){
		if(Input.GetKeyDown(KeyCode.O)){
			//Anotherturn();
		}
		if(Input.GetKeyDown(KeyCode.P)){
			//Moveworkers();
		}
		if(Input.GetKeyDown(KeyCode.Q)){
			//CreateLastGen();
			Debug.Log(lastgen.Count);
		}
		if(Input.GetKeyDown(KeyCode.T)){
			//AWholeTurn();
		}
	}

	public void Solve(string[,] tiles, Vector2 testv){	//single solution
		//B();	
		workersalive = new List<Worker>();		
		initialgenes = new List<string>(); 		
		lastgen = new List<Worker>();												//initializes genes
		workers = new List<Worker>(); 		
		curgenes = new List<string>();
		totalstoppedtiles = new List<Vector2>();												//initializes list of workers or "bots"
		startingposition = new Vector2(startx,starty);
		currenttiles = tiles;
		totalstoppedtiles.Add(startingposition);
		CheckAndCreate(0, startx, starty, currenttiles, initialgenes, totalstoppedtiles,testv);						//Creates first worker (Unless he can move more than one place)
		if(workers.Count == 0){ 																//If Start tile is Walled up, No solution.
//			Debug.Log("Cant");
			return;
		}
		for(int i= 0; i< workers.Count ;i++){													//Move all clones from CheckandCreate
			workers[i].Move(testv);
		}
		lastgen = new List<Worker>(workersalive);	
		while(lastgen.Count>0){
			AWholeTurn(testv);
		}
		/*if(solutions.Count == 0){
			Debug.Log("No solutions");
		}
		else{
			for(int i =0; i<solutions.Count; i++){
				Debug.Log(solutions[i].myturns + "Turns" + "with piece at" + solutions[i].x +  "+" + solutions[i].y );
			}
		}*/
	}
	public void CheckAndCreate(int newturns, int x, int y,string[,] thistiles, List<string> newgenes, List<Vector2> newstoppedtiles, Vector2 testv){
		//workers.Clear();
		List <string> genes = newgenes;
		List <Vector2> mystoppedtiles = newstoppedtiles;
		turns = newturns;

		if(x<7){
			if(thistiles[x+1,y] != "Wall"){//Checking Right
				if(turns == 0 && thistiles[x+1,y] == "Goal"){
				}
				else{
		//			Debug.Log("Cloning Right");
					Worker worker = new Worker(turns, x, y, thistiles, "Right", genes, newstoppedtiles, testv);
					workers.Add(worker);
				}		
			}
		}
		if(x>0){
			if(thistiles[x-1,y] != "Wall" ){//Checking Left
				if(turns == 0 && thistiles[x-1,y] == "Goal"){
				}
				else{
		//			Debug.Log("Cloning Left");
					Worker worker = new Worker(turns, x, y, thistiles, "Left", genes, newstoppedtiles, testv);
					workers.Add(worker);
				}
			}
		}
		if(y<7){
			if(thistiles[x,y+1] != "Wall"){//Checking Down
				if(turns == 0 && thistiles[x,y+1] == "Goal"){
				}
				else{
	//				Debug.Log("Cloning Down");
					Worker worker = new Worker(turns, x, y, thistiles, "Down", genes, newstoppedtiles, testv);
					workers.Add(worker);
				}
			}
		}
		if(y>0){
			if(thistiles[x,y-1] != "Wall"){//Checking Up
				if(turns == 0 && thistiles[x,y-1] == "Goal"){
				}
				else{
					//				Debug.Log("Cloning Up");
					Worker worker = new Worker(turns, x, y, thistiles, "Up", genes, newstoppedtiles, testv);
					workers.Add(worker);
				}
			}	
		}
	}
	public void Anotherturn(Vector2 testv){
		workers.Clear();																	//Clears worker class to repopulate with clones
			for(int i=0; i<lastgen.Count ; i++){ 												//for every one in previous gen
				curgenes = lastgen[i].mygenes;
				turns = lastgen[i].turns;		
				currentstoppedtiles = new List<Vector2>(lastgen[i].stoppedtiles);	
																		//Copy the genes of the worker for the new bots
				//Debug.Log(lastgen[i].mytiles[2,2]);
				CheckAndCreate(turns, lastgen[i].x, lastgen[i].y, lastgen[i].mytiles, curgenes, currentstoppedtiles, testv);	
				//Debug.Log(workers.Count);
																								//Create new workers with genes + new gene depending on where they can go
				//lastgen = new List<Worker>(workers);											//New Generation Becomes the past.
			}
	}
	public void Moveworkers(Vector2 testv){
		workersalive.Clear();
		for(int j = 0;j<workers.Count; j++ ){											//For every new worker
			workers[j].Move(testv);										//Move new worker to the new gene
		}
	}
	public void CreateLastGen(){
		lastgen = new List<Worker>(workersalive);
		workersalive.Clear();
	}
	public void AWholeTurn(Vector2 testv){
		Anotherturn(testv);
		Moveworkers(testv);
		CreateLastGen();
	}



	/*public void CyclePieceSolution(){
		string piecetag;
		//Debug.Log(AIBrain.pieces.Count);
		for(int i = 0; i<AIBrain.pieces.Count;i++){
			for(int j = 0; j<8; j++){
				for(int k= 0; k<8; k++){ //loop through all tiles ogtiles
					if(ogtiles[k,j]=="Ice"){
						//lastgen.Clear();
						//Debug.Log("Solve at "+ k + "+" + j);
						currenttest = new Vector2(k, j);
						string[,] newtiles = (string[,]) AIBrain.tiles.Clone();
						newtiles[k,j] = AIBrain.pieces[i].tag;
						Debug.Log(startx + "+" + starty);
						//Solve(newtiles);

					}
				}
			}
		}
	}
	public void NewCycle(){
		bool cycling = true;
		string piecetag;
		//Debug.Log(AIBrain.pieces.Count);
		for(int i = 0; i<CreateMethod.piecetiles.Count;i++){
			for(int j = 0; j<8; j++){
				for(int k= 0; k<8; k++){ //loop through all tiles ogtiles
					if(ogtiles[k,j]=="Ice"){
						//lastgen.Clear();
//						Debug.Log("Solve at "+ k + "+" + j);
						currenttest = new Vector2(k, j);
						string[,] newtiles = (string[,]) CreateMethod.generatedmap.Clone();
						newtiles[k,j] = CreateMethod.piecetiles[i];
//						Debug.Log(startx + "+" + starty);
						//Solve(newtiles);

					}
				}
			}
		}
	}*/
	public void CycleAll(){
		bestsol = 0;
		//solutions.Clear();
//		Debug.Log(CreateMethod.piecetiles.Count + "Pieces, probably pedros");
		switch(CreateMethod.piecetiles.Count){
			case 0:
				break;
			case 1:
				Debug.Log("Solving for one piece");
				for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
						solutionpieceposition = new List <Vector2>();
						solutionpiecenames = new List<string>();
						string[,] newtiles = (string[,]) CreateMethod.generatedmap.Clone();
						int x = (int)possibletiles[i].x;
						int y = (int)possibletiles[i].y;
						currenttest = new Vector2(x,y);
						solutionpieceposition.Add(new Vector2(x,y)); 
						solutionpiecenames.Add(CreateMethod.piecetiles[i]);
						newtiles[x,y] = CreateMethod.piecetiles[0];
						Solve(newtiles, new Vector2(0,0));
					}
				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
					//Debug.Log(solutions[i].myturns + "Turns with one piece" );
					CountOrStay(solutions[i].myturns, solutions[i]); //sets best sol turn number.
				}	
				bestsolutions.Add(bestsol);
				break;
			case 2:
				//Try solving for one piece individually to find redundancies.
				for(int a = 0; a<CreateMethod.piecetiles.Count; a++){
					for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
						solutionpiecenames = new List<string>();
						solutionpieceposition = new List <Vector2>();
						string[,] newtiles = (string[,]) CreateMethod.generatedmap.Clone();
						int x = (int)possibletiles[i].x;
						int y = (int)possibletiles[i].y;
						currenttest = new Vector2(x,y);	
						solutionpiecenames.Add(CreateMethod.piecetiles[a]);
						newtiles[x,y] = CreateMethod.piecetiles[a];
						solutionpieceposition.Add(new Vector2(x,y)); 
						Solve(newtiles, new Vector2(0,0));
					}
				}
				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
					//Debug.Log(solutions[i].myturns + "Turns with one piece" );
					CountOrStay(solutions[i].myturns, solutions[i]);
				}
				bestsolutions.Add(bestsol);
				//int solsatone = bestsol;
				if(bestsolutions[1]!= 0){
					bestsolutions.Add(0);
					break;
				}
				bestsol = 0;
				solutions = new List<Solution>();
				Debug.Log("Solving for two pieces and " +possibletiles.Count + "ice");
				for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
					solutionpiecenames = new List<string>();
					solutionpieceposition = new List <Vector2>();
					string[,] newtiles = (string[,]) ogtiles.Clone();
					int x = (int)possibletiles[i].x;
					int y = (int)possibletiles[i].y;
					currenttest.Set(x,y);
					solutionpieceposition.Add(new Vector2(x,y)); 
					solutionpiecenames.Add(CreateMethod.piecetiles[0]);
					newtiles[x,y] = CreateMethod.piecetiles[0];
					//solutionpieceposition.Add(new Vector2(x,y));
					solutionpiecenames.Add(CreateMethod.piecetiles[1]);
					//List<Vector2> firstLayer = new List<Vector2>(newtile)
					bool stop = false;
					for(int j = 0; j<possibletiles.Count; j++){
						if(j!=i){
							if(CreateMethod.piecetiles[0] == "Up" && CreateMethod.piecetiles[1] == "Down"){
								if(x==(int)possibletiles[j].x && (y > (int)possibletiles[j].y)){
									stop =true;
								}
							}
							if(CreateMethod.piecetiles[0] == "Down" && CreateMethod.piecetiles[1] == "Up"){
								if(x==(int)possibletiles[j].x && (y <(int)possibletiles[j].y)){
									stop =true;
								}
							}
							if(CreateMethod.piecetiles[0] == "Left" && CreateMethod.piecetiles[1] == "Right"){
								if(y==(int)possibletiles[j].y && (x > (int)possibletiles[j].x)){
									stop =true;
								}
							}
							if(CreateMethod.piecetiles[0] == "Right" && CreateMethod.piecetiles[1] == "Left"){
								if(y==(int)possibletiles[j].y && (x <(int)possibletiles[j].x)){
									stop =true;
								}
							}
							if(!stop){

								string[,] newertiles = (string[,]) newtiles.Clone();
								int x2 = (int)possibletiles[j].x;
								int y2 = (int)possibletiles[j].y;
								if(solutionpieceposition.Count<2){
									solutionpieceposition.Add(new Vector2(x2,y2));
								}
								currenttest2.Set(x2,y2);
								solutionpieceposition[1] = new Vector2(x2,y2);
//								Debug.Log(solutionpieceposition[1]);
								newertiles[x2,y2] = CreateMethod.piecetiles[1];
								//Debug.Log("Solving for " +x+y+x2+y2);
								Vector2 test2 = new Vector2(x2,y2);
								Solve(newertiles, test2);
							}
							stop = false;
						}
					}
				}
				Debug.Log(solutions.Count + "is the solution bank"); 
				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
					//Debug.Log(solutions[i].myturns + "Turns with one piece" );
					Debug.Log(solutions[i].solutionpositions[0] + "" + solutions[i].solutionpositions[1] + solutions[i].myturns + solutions[i].solutionpositions.Count);
					int bestturns = bestsol;
					CountOrStay(solutions[i].myturns, solutions[i]);
					if((bestsol < bestturns) || (bestturns == 0 && bestsol != 0) || (bestsol == solutions[i].myturns)){
					CheckWallHug(solutions[i]);
					}
					//if new best solution or repeated best solution.
					//CheckWallHug(solutions[i]);
					//if(repeatedbestsol){
					//	Debug.Log(solutions[i].solutionpositions[0] + "" + solutions[i].solutionpositions[1] + solutions[i].myturns);
					//}
//					Debug.Log(solutions[i].myturns);
				}
				bestsolutions.Add(bestsol);
				//if(repeatedbestsol){
				//int solsattwo = bestsol;
				//Debug.Log(solsatone + "At one, " + solsattwo + "at two.");
				break;
			case 3:
				for(int a = 0; a<CreateMethod.piecetiles.Count; a++){
						for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
							string[,] newtiles = (string[,]) CreateMethod.generatedmap.Clone();
							int x = (int)possibletiles[i].x;
							int y = (int)possibletiles[i].y;
							currenttest = new Vector2(x,y);
							newtiles[x,y] = CreateMethod.piecetiles[a];
							Solve(newtiles, new Vector2(0,0));
						}
					}
				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
					//Debug.Log(solutions[i].myturns + "Turns with one piece" );
					CountOrStay(solutions[i].myturns, solutions[i]);
				}
				bestsolutions.Add(bestsol);
				//int solsatone = bestsol;
				if(bestsolutions[1]!= 0){
					bestsolutions.Add(0);
					break;
				}
				bestsol = 0;
				solutions = new List<Solution>();
				Debug.Log("Solving for two pieces and " +possibletiles.Count + "ice");
				for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
					string[,] newtiles = (string[,]) ogtiles.Clone();
					int x = (int)possibletiles[i].x;
					int y = (int)possibletiles[i].y;
					currenttest.Set(x,y);
					newtiles[x,y] = CreateMethod.piecetiles[0];
					bool stop = false;
					for(int j = 0; j<possibletiles.Count; j++){
						if(j!=i){
							if(CreateMethod.piecetiles[0] == "Up" && CreateMethod.piecetiles[1] == "Down"){
								if(x==(int)possibletiles[j].x && (y > (int)possibletiles[j].y)){
									stop =true;
								}
							}
							if(CreateMethod.piecetiles[0] == "Down" && CreateMethod.piecetiles[1] == "Up"){
								if(x==(int)possibletiles[j].x && (y <(int)possibletiles[j].y)){
									stop =true;
								}
							}
							if(!stop){
								string[,] newertiles = (string[,]) newtiles.Clone();
								int x2 = (int)possibletiles[j].x;
								int y2 = (int)possibletiles[j].y;
								currenttest2.Set(x2,y2);

								newertiles[x2,y2] = CreateMethod.piecetiles[1];
								//Debug.Log("Solving for " +x+y+x2+y2);
								Vector2 test2 = new Vector2(x2,y2);
								Solve(newertiles, test2);
							}
							stop = false;
						}
					}
				}
				//Debug.Log(solutions.Count + "is the solution bank"); 
				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
					//Debug.Log(solutions[i].myturns + "Turns with one piece" );
					CountOrStay(solutions[i].myturns,solutions[i]);
//					Debug.Log(solutions[i].myturns);
				}
				bestsolutions.Add(bestsol);
//					int solsattwo = bestsol;
				if(bestsolutions[2]!= 0){
					bestsolutions.Add(0);
					break;
				}
				bestsol = 0;
				solutions = new List<Solution>();
				Debug.Log("HERE WE GO 3 (pendiente)");
				/*for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
					string[,] newtiles = (string[,]) ogtiles.Clone();
					int x = (int)possibletiles[i].x;
					int y = (int)possibletiles[i].y;
					currenttest.Set(x,y);
					newtiles[x,y] = CreateMethod.piecetiles[0];
					bool stop = false;
					for(int j = 0; j<possibletiles.Count; j++){
						if(j!=i){
							if(CreateMethod.piecetiles[0] == "Up" && CreateMethod.piecetiles[1] == "Down"){
								if(x==(int)possibletiles[j].x && (y > (int)possibletiles[j].y)){
									stop =true;
								}
							}
							if(CreateMethod.piecetiles[0] == "Down" && CreateMethod.piecetiles[1] == "Up"){
								if(x==(int)possibletiles[j].x && (y <(int)possibletiles[j].y)){
									stop =true;
								}
							}
							if(!stop){
								string[,] newertiles = (string[,]) newtiles.Clone();
								int x2 = (int)possibletiles[j].x;
								int y2 = (int)possibletiles[j].y;
								currenttest2.Set(x2,y2);

								newertiles[x2,y2] = CreateMethod.piecetiles[1];
								//Debug.Log("Solving for " +x+y+x2+y2);
								Vector2 test2 = new Vector2(x2,y2);
								Solve(newertiles, test2);
							}
							stop = false;
						}
					}
				}*/
				break;

		}
	}

	public void PopulateProbableTiles(){
		possibletiles.Clear();
		for(int j = 0; j<8; j++){
			for(int k= 0; k<8; k++){ //loop through all tiles ogtiles
				if(ogtiles[k,j]=="Ice"){
					possibletiles.Add(new Vector2(k,j));
				}
			}
		}
	}

	public void CheckInLineStoppedTiles(){

	}
	public void TryEverything(){
		//solutions.Clear(); //resets
		//GameObject[] tiles = GameObject.FindGameObjectsWithTag("Ground"); //finds tags on all
		foreach(GameObject tileP in tiles){
			tileP.GetComponent<TileProperties>().GatherData(); //populates aibrain.tiles
		}
		turns = 0;
		Debug.Log(solutions.Count());
		ogtiles = AIBrain.tiles; //og map
//		Solve(ogtiles);			//solves for no pieces
		//CyclePieceSolution();	//This is important

		if(solutions.Count>0){
			for(int i =0; i<solutions.Count; i++){
			Debug.Log(solutions[i].myturns + "Turns" + "with piece at" + solutions[i].x +  "+" + solutions[i].y );
			}

		}
		if(solutions.Count == 0){
			Debug.Log("No solutions");
		}
	}
	public void TryPieces(string[,] thistiles){
		bestsol = 0;
		bestsolutions.Clear();
		solutions.Clear();	
		int solsatorigin = 0;
		turns = 0;
		ogtiles = thistiles; //og map
		Solve(ogtiles, new Vector2(0,0));			//solves for no pieces
		if(solutions.Count>0){
			for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
//				Debug.Log(solutions[i].myturns + "Turns with no piece" );
				CountOrStay(solutions[i].myturns, solutions[i]);
			}
			Debug.Log(bestsol + "is the best empty sol");
			solsatorigin = solutions.Count -1;
			ANNBrain.sol =1;
			dsol = bestsol/10f;
//			Debug.Log(dsol + "No empty solution");
			ANNBrain.los = 0;
		}
		bestsolutions.Add(bestsol);
		//NewCycle();	//This is important with create method
		PopulateProbableTiles();
		//bestsol = 0;
		solutions.Clear();
		CycleAll();
		PrintBestSols();
		//Debug.Log
		/*if(solutions.Count>0){//For anything with pieces.
			for(int i =0; i<solutions.Count; i++){
			Debug.Log(solutions[i].myturns + "Turns" + "with piece at" + solutions[i].x +  "+" + solutions[i].y );
			CountOrStay(solutions[i].myturns);

			}
			Debug.Log("X means solutions with pieces");
		}
		if(solutions.Count == 0){
			Debug.Log("No solutions extra");
			bestsol = 0;
			ANNBrain.sol = 0;
			ANNBrain.los = 1;
			dsol = 0;

		}*/
	}
	public void PrintBestSols(){
		string solutionnums = "";
			for(int i = 0; i<bestsolutions.Count; i++){
				solutionnums = solutionnums + "" + bestsolutions[i].ToString();
		}
		Debug.Log(solutionnums + repeatedbestsol + besthaswallhug.Count);
	}
	public void CountOrStay(int num, Solution currentsolution){//updates if new solution is lower than the newest
		if(bestsol == 0){
			besthaswallhug.Clear();
			bestsol = num;
			repeatedbestsol = false;
			bestsolution = currentsolution;
			return;
		}
		if (num<bestsol){
			besthaswallhug.Clear();
			bestsol = num;
			repeatedbestsol = false;
			bestsolution = currentsolution;
			return;
		}
		if(num==bestsol){
			repeatedbestsol = true;
		}
		//Debug.Log("Repeated best at " + num)
	}
	public void CheckWallHug(Solution solutiontocheck){
		for (int i = 0; i<CreateMethod.piecetiles.Count; i++){//Check each piece placed for LRUD
			if(CreateMethod.piecetiles[i] == "Left"){
				Vector2 piecetocheck = solutiontocheck.solutionpositions[i];
				Debug.Log(piecetocheck);
				if((ogtiles[(int)(piecetocheck.x - 1), (int)piecetocheck.y] == "Wall") || (ogtiles[(int)(piecetocheck.x - 1), (int)piecetocheck.y] == "Start")){
					Debug.Log("Wallhugged Left");
					besthaswallhug.Add(true);
				}
				else{
				Debug.Log("Didnt hug Left");
				}
			}
			if(CreateMethod.piecetiles[i] == "Right"){
				Vector2 piecetocheck = solutiontocheck.solutionpositions[i];
				Debug.Log(piecetocheck);
				if((ogtiles[(int)(piecetocheck.x + 1), (int)piecetocheck.y] == "Wall") || (ogtiles[(int)(piecetocheck.x + 1), (int)piecetocheck.y] == "Start")){
					Debug.Log("Wallhugged Right");
					besthaswallhug.Add(true);
				}
				else{
					Debug.Log("Didnt hug Right");
				}

			}
			if(CreateMethod.piecetiles[i] == "Up"){
				Vector2 piecetocheck = solutiontocheck.solutionpositions[i];
				Debug.Log(piecetocheck);
				if((ogtiles[(int)(piecetocheck.x), (int)piecetocheck.y - 1] == "Wall") || (ogtiles[(int)(piecetocheck.x), (int)piecetocheck.y - 1] == "Start")){
					Debug.Log("Wallhugged Up");
					besthaswallhug.Add(true);
				}
				else{
					Debug.Log("Didnt hug Up");
				}
			}
			if(CreateMethod.piecetiles[i] == "Down"){
				Vector2 piecetocheck = solutiontocheck.solutionpositions[i];
				Debug.Log(piecetocheck);
				if((ogtiles[(int)(piecetocheck.x), (int)piecetocheck.y + 1] == "Wall") || (ogtiles[(int)(piecetocheck.x), (int)piecetocheck.y + 1] == "Start")){
					Debug.Log("Wallhugged Down");
					besthaswallhug.Add(true);
				}
				else{
					Debug.Log("Didnt hug Down");
				}
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