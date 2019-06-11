using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Map{
	public string[,] tiles;

	public Map(string[,] incomingtiles){
		tiles = (string[,]) incomingtiles.Clone();
	}
}

public class SolveMethod : MonoBehaviour {
	public static string[,] ogtiles = new string[10,10]; // Initial Map to test (Without Piece tiles)

	public static string[,] newtiles = new string[10,10];

	public static string[,] newertiles = new string[10,10];

	public static string[,] newerertiles = new string[10,10];

	public static string[,] solvingtiles = new string[10,10];

	public static List<Solution> solutions = new List<Solution>(); //

	public static List<int> bestsolutions  = new List<int>();//shows string of numerical value forbest turn solution with 0 tiles, 1 tiles ... n tiles.

	public static int numberofsolutions;

	public static int lrud; //checks for using a lrud as lrud and not wall.

	public static List<Vector2> lrudpos;

	//public static List<Map> stoppedmaps = new List<Map>();

	//public static List<Map> currentstoppedmaps = new List<Map>();

	//public static List<int> turnedperturn = new List<int>();

	//public static List<int> initialturnedperturn = new List<int>();

	//public static List<int> currentturnedperturn = new List<int>();

	public static int startx;

	public static int starty;

	public static List <Worker> workers = new List<Worker>();

	public static List <string> initialgenes = new List<string>();

	public static List <Worker> lastgen = new List<Worker>();

	public static List<string> curgenes = new List<string>();

	public static List<Vector2> totalstoppedtiles = new List<Vector2>();

	public static List<Vector2> currentstoppedtiles;

	public static Vector2 startingposition;

	public static List<Worker> workersalive = new List<Worker>();

	public static int turns;

	public static GameObject[] tiles;

	public static List <Vector2> Icetiles;

	public static string[,] currenttiles = new string[10,10];

	public static Vector2 currentGoal;

	public static List<Vector2> inLineTiles;

	public static Vector2 currenttest = new Vector2(0,0);

	public static Vector2 currenttest2 = new Vector2(0,0);

	public static Vector2 currenttest3 = new Vector2(0,0);

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

	public static bool crapsolution;
	
	public static bool gottago;

	public static int bestturns;

	public static int piecesused;

	void Start(){
			//TryEverything();
		//Debug.Log("Solve");
	} 

	/*void Update(){
		/*if(Input.GetKeyDown(KeyCode.O)){
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
	}*/

	public void Solve(string[,] tiles){	//single solution
		//B();	
		workersalive.Clear();	
		initialgenes.Clear();
		lastgen.Clear();
		workers.Clear();	
		curgenes.Clear();
		totalstoppedtiles.Clear();
		startingposition = new Vector2(startx,starty);
		//usedlruds = 0;
		currenttiles = tiles;
		//pieceused = 0;
		totalstoppedtiles.Add(startingposition);

		//stoppedmaps.Add(new Map(currenttiles));
		//initialturnedperturn.Clear();
		//initialturnedperturn.Add(0);
		CheckAndCreate(0, startx, starty, currenttiles, initialgenes, totalstoppedtiles, 0, new List<Vector2>(), 0);	//Creates first worker (Unless he can move more than one place)
		if(workers.Count == 0){ //If Start tile is Walled up, No solution.
//			Debug.Log("Cant");
			return;
		}
		for(int i= 0; i< workers.Count ;i++){//Move all clones from CheckandCreate
			workers[i].Move();
		}
		lastgen = new List<Worker>(workersalive);	
		while(lastgen.Count>0){
			AWholeTurn();
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
	public void CheckAndCreate(int newturns, int x, int y,string[,] thistiles, List<string> newgenes, List<Vector2> newstoppedtiles, int newlrud, List<Vector2> newlrudpos, int newpiecesused){
		//workers.Clear();
		List <string> genes = newgenes;
		List <Vector2> mystoppedtiles = newstoppedtiles;
		turns = newturns;
		lrud = newlrud;
		lrudpos = newlrudpos;

		//List<Map> mystoppedmaps = newstoppedmaps;
		//int mypiecesturned = newpiecesturned;
		//List<int>myturnedperturn = newturnedperturn;
		if(x<CreateMethod.mapdimension-1){
			if(thistiles[x+1,y] != "Wall"){//Checking Right
				if(turns == 0 && thistiles[x+1,y] == "Goal"){
				}
				else{
		//			Debug.Log("Cloning Right");
					Worker worker = new Worker(turns, x, y, thistiles, "Right", genes, newstoppedtiles,lrud, lrudpos, newpiecesused);
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
					Worker worker = new Worker(turns, x, y, thistiles, "Left", genes, newstoppedtiles,  lrud, lrudpos, newpiecesused);
					workers.Add(worker);
				}
			}
		}
		if(y<CreateMethod.mapdimension-1){
			if(thistiles[x,y+1] != "Wall"){//Checking Down
				if(turns == 0 && thistiles[x,y+1] == "Goal"){
				}
				else{
	//				Debug.Log("Cloning Down");
					Worker worker = new Worker(turns, x, y, thistiles, "Down", genes, newstoppedtiles, lrud, lrudpos, newpiecesused);
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
					Worker worker = new Worker(turns, x, y, thistiles, "Up", genes, newstoppedtiles,  lrud, lrudpos, newpiecesused);
					workers.Add(worker);
				}
			}	
		}
	}
	public void Anotherturn(){
		workers.Clear();																	//Clears worker class to repopulate with clones
			for(int i=0; i<lastgen.Count ; i++){ 												//for every one in previous gen
				curgenes = lastgen[i].mygenes;
				turns = lastgen[i].turns;	
				lrud = lastgen[i].lrud;	
				lrudpos = lastgen[i].lrudpos;
				//currentstoppedtiles = lastgen[i].stoppedtiles;	
				//int currentpiecesturned = lastgen[i].piecesturned;
				//currentturnedperturn = lastgen[i].turnedperturn;
				//currentstoppedmaps = new List<Map>(lastgen[i].stoppedmaps);
																		//Copy the genes of the worker for the new bots
				//Debug.Log(lastgen[i].mytiles[2,2]);
				CheckAndCreate(turns, lastgen[i].x, lastgen[i].y, lastgen[i].mytiles, curgenes, lastgen[i].stoppedtiles, lrud, lrudpos, lastgen[i].piecesused);	
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
	public void placeIcarus(int degree, Vector2 coords, string type){
		if(degree == 0){
			if(type == "Right"){
				if(newtiles[(int)coords.x+1,(int)coords.y] == "Ice" || newtiles[(int)coords.x+1,(int)coords.y] == "Wood"){
					newtiles[(int)coords.x+1,(int)coords.y] = "Right";
				}
				if(newtiles[(int)coords.x+1,(int)coords.y] == "Fragile" ){
					newtiles[(int)coords.x+1,(int)coords.y] = "FragileRight";
				}
			}
			if(type == "Left"){
				if(newtiles[(int)coords.x-1,(int)coords.y] == "Ice" || newtiles[(int)coords.x-1,(int)coords.y] == "Wood"){
					newtiles[(int)coords.x-1,(int)coords.y] = "Left";
				}
				if(newtiles[(int)coords.x-1,(int)coords.y] == "Fragile" ){
					newtiles[(int)coords.x-1,(int)coords.y] = "FragileLeft";
				}
			}
			if(type == "Up"){
				if(newtiles[(int)coords.x,(int)coords.y-1] == "Ice" || newtiles[(int)coords.x,(int)coords.y-1] == "Wood"){
					newtiles[(int)coords.x,(int)coords.y-1] = "Up";
				}
				if(newtiles[(int)coords.x,(int)coords.y-1] == "Fragile" ){
					newtiles[(int)coords.x,(int)coords.y-1] = "FragileUp";
				}
			}
			if(type == "Down"){
				if(newtiles[(int)coords.x,(int)coords.y+1] == "Ice" || newtiles[(int)coords.x,(int)coords.y+1] == "Wood" ){
					newtiles[(int)coords.x,(int)coords.y+1] = "Down";
				}
				if(newtiles[(int)coords.x,(int)coords.y+1] == "Fragile" ){
					newtiles[(int)coords.x,(int)coords.y+1] = "FragileDown";
				}
			}			
		}
		if(degree == 1){
			if(type == "Right"){
				if(newertiles[(int)coords.x+1,(int)coords.y] == "Ice" || newertiles[(int)coords.x+1,(int)coords.y] == "Wood"){
					newertiles[(int)coords.x+1,(int)coords.y] = "Right";
				}
				if(newertiles[(int)coords.x+1,(int)coords.y] == "Fragile" ){
					newertiles[(int)coords.x+1,(int)coords.y] = "FragileRight";
				}
			}
			if(type == "Left"){
				if(newertiles[(int)coords.x-1,(int)coords.y] == "Ice" || newertiles[(int)coords.x-1,(int)coords.y] == "Wood"){
					newertiles[(int)coords.x-1,(int)coords.y] = "Left";
				}
				if(newertiles[(int)coords.x-1,(int)coords.y] == "Fragile" ){
					newertiles[(int)coords.x-1,(int)coords.y] = "FragileLeft";
				}
			}
			if(type == "Up"){
				if(newertiles[(int)coords.x,(int)coords.y-1] == "Ice" || newertiles[(int)coords.x,(int)coords.y-1] == "Wood"){
					newertiles[(int)coords.x,(int)coords.y-1] = "Up";
				}
				if(newertiles[(int)coords.x,(int)coords.y-1] == "Fragile" ){
					newertiles[(int)coords.x,(int)coords.y-1] = "FragileUp";
				}
			}
			if(type == "Down"){
				if(newertiles[(int)coords.x,(int)coords.y+1] == "Ice" || newertiles[(int)coords.x,(int)coords.y+1] == "Wood" ){
					newertiles[(int)coords.x,(int)coords.y+1] = "Down";
				}
				if(newertiles[(int)coords.x,(int)coords.y+1] == "Fragile" ){
					newertiles[(int)coords.x,(int)coords.y+1] = "FragileDown";
				}
			}			
		}
		if(degree == 2){
			if(type == "Right"){
				if(newerertiles[(int)coords.x+1,(int)coords.y] == "Ice" || newerertiles[(int)coords.x+1,(int)coords.y] == "Wood"){
					newerertiles[(int)coords.x+1,(int)coords.y] = "Right";
				}
				if(newerertiles[(int)coords.x+1,(int)coords.y] == "Fragile" ){
					newerertiles[(int)coords.x+1,(int)coords.y] = "FragileRight";
				}
			}
			if(type == "Left"){
				if(newerertiles[(int)coords.x-1,(int)coords.y] == "Ice" || newerertiles[(int)coords.x-1,(int)coords.y] == "Wood"){
					newerertiles[(int)coords.x-1,(int)coords.y] = "Left";
				}
				if(newerertiles[(int)coords.x-1,(int)coords.y] == "Fragile" ){
					newerertiles[(int)coords.x-1,(int)coords.y] = "FragileLeft";
				}
			}
			if(type == "Up"){
				if(newerertiles[(int)coords.x,(int)coords.y-1] == "Ice" || newerertiles[(int)coords.x,(int)coords.y-1] == "Wood"){
					newerertiles[(int)coords.x,(int)coords.y-1] = "Up";
				}
				if(newerertiles[(int)coords.x,(int)coords.y-1] == "Fragile" ){
					newerertiles[(int)coords.x,(int)coords.y-1] = "FragileUp";
				}
			}
			if(type == "Down"){
				if(newerertiles[(int)coords.x,(int)coords.y+1] == "Ice" || newerertiles[(int)coords.x,(int)coords.y+1] == "Wood" ){
					newerertiles[(int)coords.x,(int)coords.y+1] = "Down";
				}
				if(newerertiles[(int)coords.x,(int)coords.y+1] == "Fragile" ){
					newerertiles[(int)coords.x,(int)coords.y+1] = "FragileDown";
				}
			}			
		}
		
	}
	public void InitiateNewSolution(){
		solutionpieceposition = new List <Vector2>();//workers populate these.
		solutionpiecenames = new List<string>();
		newtiles = (string[,]) CreateMethod.generatedmap.Clone();

	}

	public void UpdateMapWithPiece(int x, int y, int piecenumber){
		currenttest = new Vector2(x,y);

		solutionpieceposition.Add(new Vector2(x,y)); 
		solutionpiecenames.Add(CreateMethod.piecetiles[piecenumber]);			
	}

	public void PlacePiece(int degree, int x, int y, int piecenumber){
		if(degree == 0){	

			if(CreateMethod.piecetiles[piecenumber] == "Left" || CreateMethod.piecetiles[piecenumber] == "Right" 
			|| CreateMethod.piecetiles[piecenumber] == "Up" || CreateMethod.piecetiles[piecenumber] == "Down"){//check if lrud
				
				newtiles[x,y] = "Wall";

				placeIcarus(0,currenttest, CreateMethod.piecetiles[piecenumber]);
				// remove the lefted one from possibletiles (in next ones)

			}
			else{
//				Debug.Log("degree 0 has " + CreateMethod.piecetiles[piecenumber]);
				newtiles[x,y] = CreateMethod.piecetiles[piecenumber];

			}				
		}
		if(degree == 1){	
			
			if(CreateMethod.piecetiles[piecenumber] == "Left" || CreateMethod.piecetiles[piecenumber] == "Right" 
			|| CreateMethod.piecetiles[piecenumber] == "Up" || CreateMethod.piecetiles[piecenumber] == "Down"){//check if lrud
				
				newertiles[x,y] = "Wall";

				placeIcarus(1,currenttest2, CreateMethod.piecetiles[piecenumber]);
				// remove the lefted one from possibletiles (in next ones)

			}
			else{
			
				newertiles[x,y] = CreateMethod.piecetiles[piecenumber];
				//Debug.Log("degree 1 has " + CreateMethod.piecetiles[piecenumber]);

			}	

		}
		if(degree == 2){	

			if(CreateMethod.piecetiles[piecenumber] == "Left" || CreateMethod.piecetiles[piecenumber] == "Right" 
			|| CreateMethod.piecetiles[piecenumber] == "Up" || CreateMethod.piecetiles[piecenumber] == "Down"){//check if lrud
				
				newerertiles[x,y] = "Wall";

				placeIcarus(2,currenttest3, CreateMethod.piecetiles[piecenumber]);
				// remove the lefted one from possibletiles (in next ones)

			}
			else{
			
				newerertiles[x,y] = CreateMethod.piecetiles[piecenumber];
//				Debug.Log("degree 2 has " + CreateMethod.piecetiles[piecenumber]);

			}				
		}

	}	
	public bool HasLoop(int x, int y, int piece1, int piece2, int iceplace){
		if((CreateMethod.piecetiles[piece1] == "Up" || CreateMethod.piecetiles[piece1] == "UpSeed")&& 
			(CreateMethod.piecetiles[piece2] == "Down" || CreateMethod.piecetiles[piece2] == "DownSeed")){
			if(x==(int)possibletiles[iceplace].x && (y > (int)possibletiles[iceplace].y)){
				return true;
			}
			return false;

		}
		if((CreateMethod.piecetiles[piece1] == "Down" || CreateMethod.piecetiles[piece1] == "DownSeed") && 
			(CreateMethod.piecetiles[piece2] == "Up" || CreateMethod.piecetiles[piece2] == "UpSeed")){
			if(x==(int)possibletiles[iceplace].x && (y <(int)possibletiles[iceplace].y)){
				return true;
			}
			return false;

		}
		if((CreateMethod.piecetiles[piece1] == "Left" || CreateMethod.piecetiles[piece1] == "LeftSeed") && 
			(CreateMethod.piecetiles[piece2] == "Right" || CreateMethod.piecetiles[piece2] == "RightSeed")){
			if(y==(int)possibletiles[iceplace].y && (x > (int)possibletiles[iceplace].x)){
				return true;
			}
			return false;

		}
		if((CreateMethod.piecetiles[piece1] == "Right" || CreateMethod.piecetiles[piece1] == "RightSeed")&& 
			(CreateMethod.piecetiles[piece2] == "Left" || CreateMethod.piecetiles[piece2] == "LeftSeed")){
			if(y==(int)possibletiles[iceplace].y && (x <(int)possibletiles[iceplace].x)){
				return true;
			}
			return false;
		}	
		return false;

	}
	public void CycleAll(){
		crapsolution = false;
		bestsol = 0;
		bestturns = 0;
		//solutions.Clear();
//		Debug.Log(CreateMethod.piecetiles.Count + "Pieces, probably pedros");
		switch(CreateMethod.piecetiles.Count){
			case 0:
				//Solve()
				break;

			case 1:
				//Debug.Log("Solving for one piece");
				for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles, tries single piece in every possible ice tile
						int x = (int)possibletiles[i].x;
						int y = (int)possibletiles[i].y;
						InitiateNewSolution();
						UpdateMapWithPiece(x,y,0);
						PlacePiece(0,x,y,0);
						//solvingtiles = (string[,]) newtiles.Clone();
						Solve(newtiles);
						if(crapsolution == true){
							return;
						}

					}

				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
//					Debug.Log(solutions[i].solutionpositions[0] +"" + solutions[i].myturns + solutions[i].solutionpositions.Count);
					int bestturns = bestsol;
					CountOrStay(solutions[i].myturns, solutions[i]);
					if((bestsol < bestturns) || (bestturns == 0 && bestsol != 0) || 
						(bestsol == solutions[i].myturns)){
					CheckWallHug(solutions[i]);
					CheckStoppedSeed(solutions[i]);
					}
				}	
				bestsolutions.Add(bestsol);
				break;

			case 2:
				//Try solving for one piece individually to find redundancies.
				for(int a = 0; a<CreateMethod.piecetiles.Count; a++){//loop through pieces
					for(int i= 0; i<possibletiles.Count ; i++){ //loop through possible ice tiles
						int x = (int)possibletiles[i].x;
						int y = (int)possibletiles[i].y;
						InitiateNewSolution();
						UpdateMapWithPiece(x,y,a);
						PlacePiece(0,x,y,a);
						//solvingtiles = (string[,]) newtiles.Clone();
						Solve(newtiles);
						if(crapsolution == true){
							return;
						}						
					}
				}
				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
					//Debug.Log(solutions[i].myturns + "Turns with one piece" );
					CountOrStay(solutions[i].myturns, solutions[i]); //Updates the best solution turn number into its spot.

				}
				bestsolutions.Add(bestsol); //adds the number of turns with 1 piece

				if(bestsolutions[1]!= 0){//if it is solved for one piece out of a group of 2 then scrapsolution.
					bestsolutions.Add(0);
					return;
				}
				// Debug.Log(bestsolutions[1] + "Shouldn't not be 0");
				bestsol = 0;
				solutions = new List<Solution>();
				//Debug.Log("Solving for two pieces and " +possibletiles.Count + "ice");
				for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
					int x = (int)possibletiles[i].x;
					int y = (int)possibletiles[i].y;
					InitiateNewSolution();
					UpdateMapWithPiece(x,y,0); //adds currenttest
					PlacePiece(0,x,y,0);
					//CreateMethod.Print2DArray(newtiles);

					//solutionpieceposition.Add(new Vector2(x,y));
					solutionpiecenames.Add(CreateMethod.piecetiles[1]);
					//List<Vector2> firstLayer = new List<Vector2>(newtile)
					for(int j = 0; j<possibletiles.Count; j++){
						if(j!=i){
							if(!HasLoop(x,y,0,1,j)){

								int x2 = (int)possibletiles[j].x;
								int y2 = (int)possibletiles[j].y;	
															
								newertiles = (string[,]) newtiles.Clone();//clone previous seed

								if(solutionpieceposition.Count<2){
									solutionpieceposition.Add(new Vector2(x2,y2));
								}
								currenttest2.Set(x2,y2);

								solutionpieceposition[1] = new Vector2(x2,y2);
								PlacePiece(1,x2,y2,1);
								//solvingtiles = (string[,]) newertiles.Clone();
								Solve(newertiles);

								if(crapsolution == true){
									return;
								}
							}
						}
					}
				}
				//Debug.Log(solutions.Count + "is the solution bank"); 
				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
					//Debug.Log(solutions[i].myturns + "Turns with one piece" );
					//Debug.Log(solutions[i].solutionpositions[0] + "" + solutions[i].solutionpositions[1] + solutions[i].myturns + solutions[i].solutionpositions.Count);
					int bestturns = bestsol;
					CountOrStay(solutions[i].myturns, solutions[i]);
					if((bestsol < bestturns) || (bestturns == 0 && bestsol != 0) || (bestsol == solutions[i].myturns)){
					CheckWallHug(solutions[i]);
					CheckStoppedSeed(solutions[i]);
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
			//Try solving for one piece individually to find redundancies.
				for(int a = 0; a<CreateMethod.piecetiles.Count; a++){
					for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
						int x = (int)possibletiles[i].x;
						int y = (int)possibletiles[i].y;
						InitiateNewSolution();
						UpdateMapWithPiece(x,y,a);
						PlacePiece(0,x,y,a);
						//solvingtiles = (string[,]) newtiles.Clone();
						Solve(newtiles);
						if(crapsolution == true){
							return;
						}
					}
				}
				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
					//Debug.Log(solutions[i].myturns + "Turns with one piece" );
					CountOrStay(solutions[i].myturns, solutions[i]);
				}
				bestsolutions.Add(bestsol);
				//int solsatone = bestsol;
				if(bestsolutions[1]!= 0){//if it is solved for one piece out of a group of 2 then scrapsolution.
					bestsolutions.Add(0);
					return;
				}
				bestsol = 0;
				solutions = new List<Solution>();
				// Debug.Log("Solving for two pieces and " +possibletiles.Count + "ice");
				for(int z = 0; z<CreateMethod.piecetiles.Count-1;z++){
					for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
						int x = (int)possibletiles[i].x;
						int y = (int)possibletiles[i].y;
						InitiateNewSolution();

						UpdateMapWithPiece(x,y,z);
						PlacePiece(0,x,y,z);
						solutionpiecenames.Add(CreateMethod.piecetiles[1]);
						//List<Vector2> firstLayer = new List<Vector2>(newtile)
						for(int a=1; a<CreateMethod.piecetiles.Count; a++){
							if(a!=z){
								for(int j = 0; j<possibletiles.Count; j++){
									if(j!=i){
										if(!HasLoop(x,y,z,a,j)){
//											Debug.Log("NO LOOP");
											newertiles = (string[,]) newtiles.Clone();
											int x2 = (int)possibletiles[j].x;
											int y2 = (int)possibletiles[j].y;
											if(solutionpieceposition.Count<2){
												solutionpieceposition.Add(new Vector2(x2,y2));
											}
											currenttest2.Set(x2,y2);
											solutionpieceposition[1] = new Vector2(x2,y2);
			//								Debug.Log(solutionpieceposition[1]);
											PlacePiece(1,x2,y2,a);

											//newertiles[x2,y2] = CreateMethod.piecetiles[a];
											//Debug.Log("Solving for " +x+y+x2+y2);
											Vector2 test2 = new Vector2(x2,y2);
//											Debug.Log(newertiles[1,1]);
											//solvingtiles = (string[,]) newertiles.Clone();
											Solve(newertiles);
										}
									}
								}
							}
						}
					}
				}
//				Debug.Log(solutions.Count + "is the solution bank"); 
				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.

					CountOrStay(solutions[i].myturns, solutions[i]);

				}
				bestsolutions.Add(bestsol);
				if(bestsolutions[2]!=0){
//					Debug.Log("SLDSLDSLS");
					bestsolutions.Add(0);
					return;
				}
				bestsol = 0;
				solutions = new List<Solution>();
//				Debug.Log(possibletiles.Count);
			//	Debug.Log(CreateMethod.piecetiles.Count);
				// Debug.Log(CreateMethod.piecetiles[0]+ CreateMethod.piecetiles[1] + CreateMethod.piecetiles[2] +possibletiles.Count + "ice");
				for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles

					int x = (int)possibletiles[i].x;
					int y = (int)possibletiles[i].y;
					InitiateNewSolution();
					UpdateMapWithPiece(x,y,0);
					PlacePiece(0,x,y,0);
					solutionpiecenames.Add(CreateMethod.piecetiles[1]);
					//List<Vector2> firstLayer = new List<Vector2>(newtile)
					for(int j = 0; j<possibletiles.Count; j++){
						if(j!=i){
							if(!HasLoop(x,y,0,1,j)){
								newertiles = (string[,]) newtiles.Clone();
								int x2 = (int)possibletiles[j].x;
								int y2 = (int)possibletiles[j].y;
								if(solutionpieceposition.Count<2){
									solutionpieceposition.Add(new Vector2(x2,y2));
								}
								currenttest2.Set(x2,y2);
								solutionpieceposition[1] = new Vector2(x2,y2);
//								Debug.Log(solutionpieceposition[1]);
								PlacePiece(1,x2,y2,1);
								//Debug.Log("Solving for " +x+y+x2+y2);
								Vector2 test2 = new Vector2(x2,y2);
								solutionpiecenames.Add(CreateMethod.piecetiles[2]);
//								Debug.Log("Readytotest third" + x+y+x2+y2);
								for(int k=0; k<possibletiles.Count;k++){
									if(j!=k && k!=i){
										if(!HasLoop(x,y,0,2,k) && !HasLoop(x2,y2,1,2,k)){
//											Debug.Log("No loop");
											newerertiles = (string[,]) newertiles.Clone();
											int x3 = (int)possibletiles[k].x;
											int y3 = (int)possibletiles[k].y;
//											Debug.Log(x3 + "" +y3);
											if(solutionpieceposition.Count<3){
												solutionpieceposition.Add(new Vector2(x3,y3));
											}
											currenttest3.Set(x3,y3);
											solutionpieceposition[2] = new Vector2(x3,y3);
			//								Debug.Log(solutionpieceposition[1]);
											PlacePiece(2,x3,y3,2);
											//Debug.Log("Solving for " +x+y+x2+y2);
											//solvingtiles = (string[,]) newerertiles.Clone();
											Solve(newerertiles);
										}							
									}
								}
							}
						}
					}
				}
//				Debug.Log(solutions.Count);
				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
					//Debug.Log(solutions[i].myturns + "Turns with one piece" );
//					Debug.Log(solutions[i].solutionpositions[0] + "" + solutions[i].solutionpositions[1] + solutions[i].myturns);
					int bestturns = bestsol;
					CountOrStay(solutions[i].myturns, solutions[i]);
					if((bestsol < bestturns) || (bestturns == 0 && bestsol != 0) || (bestsol == solutions[i].myturns)){
					CheckWallHug(solutions[i]);
					CheckStoppedSeed(solutions[i]);
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
			case 4:
			//Try solving for one piece individually to find redundancies.
				for(int a = 0; a<CreateMethod.piecetiles.Count; a++){
					for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
						solutionpiecenames = new List<string>();
						solutionpieceposition = new List <Vector2>();
						newtiles = (string[,]) CreateMethod.generatedmap.Clone();
						int x = (int)possibletiles[i].x;
						int y = (int)possibletiles[i].y;
						currenttest = new Vector2(x,y);	
						solutionpiecenames.Add(CreateMethod.piecetiles[a]);
						newtiles[x,y] = CreateMethod.piecetiles[a];
						solutionpieceposition.Add(new Vector2(x,y)); 
						Solve(newtiles);
					}
				}
				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
					//Debug.Log(solutions[i].myturns + "Turns with one piece" );
					CountOrStay(solutions[i].myturns, solutions[i]);
				}
				bestsolutions.Add(bestsol);
				//int solsatone = bestsol;
				if(bestsolutions[1]!= 0){//if it is solved for one piece out of a group of 2 then scrapsolution.
					bestsolutions.Add(0);
					break;
				}
				bestsol = 0;
				solutions = new List<Solution>();
				//Debug.Log("Solving for two pieces and " +possibletiles.Count + "ice");
				for(int z = 0; z<CreateMethod.piecetiles.Count-1;z++){
					for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
						solutionpiecenames = new List<string>();
						solutionpieceposition = new List <Vector2>();
						newtiles = (string[,]) ogtiles.Clone();
						int x = (int)possibletiles[i].x;
						int y = (int)possibletiles[i].y;
						currenttest.Set(x,y);
						solutionpieceposition.Add(new Vector2(x,y)); 
						solutionpiecenames.Add(CreateMethod.piecetiles[z]);
						newtiles[x,y] = CreateMethod.piecetiles[z];
						//solutionpieceposition.Add(new Vector2(x,y));
						solutionpiecenames.Add(CreateMethod.piecetiles[1]);
						//List<Vector2> firstLayer = new List<Vector2>(newtile)
						bool stop = false;
						for(int a=1; a<CreateMethod.piecetiles.Count; a++){
							if(a!=z){
								for(int j = 0; j<possibletiles.Count; j++){
									if(j!=i){
										if((CreateMethod.piecetiles[z] == "Up" || CreateMethod.piecetiles[z] == "UpSeed")&& (CreateMethod.piecetiles[a] == "Down" || CreateMethod.piecetiles[a] == "DownSeed")){
											if(x==(int)possibletiles[j].x && (y > (int)possibletiles[j].y)){
												stop =true;
											}
										}
										if((CreateMethod.piecetiles[z] == "Down" || CreateMethod.piecetiles[z] == "DownSeed") && (CreateMethod.piecetiles[a] == "Up" || CreateMethod.piecetiles[a] == "UpSeed")){
											if(x==(int)possibletiles[j].x && (y <(int)possibletiles[j].y)){
												stop =true;
											}
										}
										if((CreateMethod.piecetiles[z] == "Left" || CreateMethod.piecetiles[z] == "LeftSeed") && (CreateMethod.piecetiles[a] == "Right" || CreateMethod.piecetiles[a] == "RightSeed")){
											if(y==(int)possibletiles[j].y && (x > (int)possibletiles[j].x)){
												stop =true;
											}
										}
										if((CreateMethod.piecetiles[z] == "Right" || CreateMethod.piecetiles[z] == "RightSeed")&& (CreateMethod.piecetiles[a] == "Left" || CreateMethod.piecetiles[a] == "LeftSeed")){
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
											newertiles[x2,y2] = CreateMethod.piecetiles[a];
											//Debug.Log("Solving for " +x+y+x2+y2);
											Vector2 test2 = new Vector2(x2,y2);
											Solve(newertiles);
										}
										stop = false;
									}
								}
							}
						}
					}
				}
				Debug.Log(solutions.Count + "is the solution bank"); 

				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.

					CountOrStay(solutions[i].myturns, solutions[i]);

				}
				bestsolutions.Add(bestsol);
				if(bestsolutions[2]!=0){
					bestsolutions.Add(0);
					break;
				}
				Debug.Log("Solving for three pieces");
				bestsol = 0;
				solutions = new List<Solution>();
				Debug.Log(CreateMethod.piecetiles[0]+ CreateMethod.piecetiles[1] + CreateMethod.piecetiles[2] +possibletiles.Count + "ice");
				Debug.Log(CreateMethod.piecetiles.Count);
				for(int z = 0; z<CreateMethod.piecetiles.Count-2; z++){
					Debug.Log(CreateMethod.piecetiles[z]);
					for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
						solutionpiecenames = new List<string>();
						solutionpieceposition = new List <Vector2>();
						newtiles = (string[,]) ogtiles.Clone();
						int x = (int)possibletiles[i].x;
						int y = (int)possibletiles[i].y;
						currenttest.Set(x,y);
						//Vector2 elv = new Vector2(x,y);
						if(solutionpieceposition.Count<1){
							solutionpieceposition.Add(new Vector2(x,y));
							solutionpiecenames.Add(CreateMethod.piecetiles[z]);
						}
						solutionpieceposition[0].Set(x,y);
						solutionpiecenames[0]  = CreateMethod.piecetiles[z];
						newtiles[x,y] = CreateMethod.piecetiles[z];
						//solutionpieceposition.Add(new Vector2(x,y));
						//solutionpiecenames.Add(CreateMethod.piecetiles[1]);
						//List<Vector2> firstLayer = new List<Vector2>(newtile)
						bool stop = false;

						for(int a = 1; a<CreateMethod.piecetiles.Count-1; a++){
							if(z!=a){
								for(int j = 0; j<possibletiles.Count; j++){
									if(j!=i){
										if((CreateMethod.piecetiles[z] == "Up" || CreateMethod.piecetiles[z] == "UpSeed")&& (CreateMethod.piecetiles[a] == "Down" || CreateMethod.piecetiles[a] == "DownSeed")){
											if(x==(int)possibletiles[j].x && (y > (int)possibletiles[j].y)){
												stop =true;
											}
										}
										if((CreateMethod.piecetiles[z] == "Down" || CreateMethod.piecetiles[z] == "DownSeed") && (CreateMethod.piecetiles[a] == "Up" || CreateMethod.piecetiles[a] == "UpSeed")){
											if(x==(int)possibletiles[j].x && (y <(int)possibletiles[j].y)){
												stop =true;
											}
										}
										if((CreateMethod.piecetiles[z] == "Left" || CreateMethod.piecetiles[z] == "LeftSeed") && (CreateMethod.piecetiles[a] == "Right" || CreateMethod.piecetiles[a] == "RightSeed")){
											if(y==(int)possibletiles[j].y && (x > (int)possibletiles[j].x)){
												stop =true;
											}
										}
										if((CreateMethod.piecetiles[z] == "Right" || CreateMethod.piecetiles[z] == "RightSeed")&& (CreateMethod.piecetiles[a] == "Left" || CreateMethod.piecetiles[a] == "LeftSeed")){
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
												solutionpiecenames.Add(CreateMethod.piecetiles[a]);
											}
											currenttest2.Set(x2,y2);
											solutionpieceposition[1] = new Vector2(x2,y2);
											solutionpiecenames[1] = CreateMethod.piecetiles[a];
			//								Debug.Log(solutionpieceposition[1]);
											newertiles[x2,y2] = CreateMethod.piecetiles[a];
											//Debug.Log("Solving for " +x+y+x2+y2);
											Vector2 test2 = new Vector2(x2,y2);
											//solutionpieceposition.Add(new Vector2(x2,y2));
											//solutionpiecenames.Add(CreateMethod.piecetiles[2]);
			//								Debug.Log("Readytotest third" + x+y+x2+y2);
											bool stop2 = false;
											for(int b=2; b<CreateMethod.piecetiles.Count; b++){
												if(b!=a && b!=z){
													for(int k=0; k<possibletiles.Count;k++){
														if(j!=k && k!=i){
															if((CreateMethod.piecetiles[z] == "Up" || CreateMethod.piecetiles[z] == "UpSeed")&& (CreateMethod.piecetiles[b] == "Down" || CreateMethod.piecetiles[b] == "DownSeed")){
																if(x==(int)possibletiles[k].x && (y > (int)possibletiles[k].y)){
																	stop2 =true;
																}
															}
															if((CreateMethod.piecetiles[z] == "Down" || CreateMethod.piecetiles[z] == "DownSeed") && (CreateMethod.piecetiles[b] == "Up" || CreateMethod.piecetiles[b] == "UpSeed")){
																if(x==(int)possibletiles[k].x && (y <(int)possibletiles[k].y)){
																	stop2 =true;
																}
															}
															if((CreateMethod.piecetiles[z] == "Left" || CreateMethod.piecetiles[z] == "LeftSeed") && (CreateMethod.piecetiles[b] == "Right" || CreateMethod.piecetiles[b] == "RightSeed")){
																if(y==(int)possibletiles[k].y && (x > (int)possibletiles[k].x)){
																	stop2 =true;
																}
															}
															if((CreateMethod.piecetiles[z] == "Right" || CreateMethod.piecetiles[z] == "RightSeed")&& (CreateMethod.piecetiles[b] == "Left" || CreateMethod.piecetiles[b] == "LeftSeed")){
																if(y==(int)possibletiles[k].y && (x <(int)possibletiles[k].x)){
																	stop2 =true;
																}
															}	
															if((CreateMethod.piecetiles[a] == "Up" || CreateMethod.piecetiles[a] == "UpSeed")&& (CreateMethod.piecetiles[b] == "Down" || CreateMethod.piecetiles[b] == "DownSeed")){
																if(x2==(int)possibletiles[k].x && (y2 > (int)possibletiles[k].y)){
																	stop2 =true;
																}
															}
															if((CreateMethod.piecetiles[a] == "Down" || CreateMethod.piecetiles[a] == "DownSeed") && (CreateMethod.piecetiles[b] == "Up" || CreateMethod.piecetiles[b] == "UpSeed")){
																if(x2==(int)possibletiles[k].x && (y2 <(int)possibletiles[k].y)){
																	stop2 =true;
																}
															}
															if((CreateMethod.piecetiles[a] == "Left" || CreateMethod.piecetiles[a] == "LeftSeed") && (CreateMethod.piecetiles[b] == "Right" || CreateMethod.piecetiles[b] == "RightSeed")){
																if(y2==(int)possibletiles[k].y && (x2 > (int)possibletiles[k].x)){
																	stop2 =true;
																}
															}
															if((CreateMethod.piecetiles[a] == "Right" || CreateMethod.piecetiles[a] == "RightSeed")&& (CreateMethod.piecetiles[b] == "Left" || CreateMethod.piecetiles[b] == "LeftSeed")){
																if(y2==(int)possibletiles[k].y && (x2 <(int)possibletiles[k].x)){
																	stop2 =true;
																}
															}	
															if(!stop2){
																string[,] newerertiles = (string[,]) newertiles.Clone();
																int x3 = (int)possibletiles[k].x;
																int y3 = (int)possibletiles[k].y;
					//											Debug.Log(x3 + "" +y3);
																if(solutionpieceposition.Count<3){
																	solutionpieceposition.Add(new Vector2(x3,y3));
																	solutionpiecenames.Add(CreateMethod.piecetiles[b]);
																}
																//currenttest2.Set(x2,y2);
																solutionpieceposition[2] = new Vector2(x3,y3);
																solutionpiecenames[2] = CreateMethod.piecetiles[b];

								//								Debug.Log(solutionpieceposition[1]);
																newerertiles[x3,y3] = CreateMethod.piecetiles[b];
																//solutionpieceposition.Add(new Vector2(x3,y3));
																//solutionpiecenames.Add(CreateMethod.piecetiles[3]);
																//bool stop3 = false;

																//Debug.Log("Solving for " +x+y+x2+y2);
																Solve(newerertiles);

															}	
															stop2 = false;						
														}
													}
												}
												
											}
										}
										stop = false;
									}
								}
							}
							
						}
					}
				}
				Debug.Log(solutions.Count);
				Solution thebestsol;

				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
					CountOrStay(solutions[i].myturns, solutions[i]);

				}
				//Debug.Log(bestsolution.solutionpositions[0] + "" + bestsolution.solutionpositions[1] + bestsolution.solutionpositions[2]);
				//Debug.Log(bestsolution.stoppedtiles[0] + " " + bestsolution.stoppedtiles[1] + " " + bestsolution.stoppedtiles[2]);
				//Debug.Log(bestsolution.piecetags[0] + "" + bestsolution.piecetags[1] +bestsolution.piecetags[2]);
				bestsolutions.Add(bestsol);
				if(bestsolutions[3]!=0){
					bestsolutions.Add(0);
					break;
				}
				bestsol = 0;
				solutions = new List<Solution>();

				//if(repeatedbestsol){
				//int solsattwo = bestsol;
				//Debug.Log(solsatone + "At one, " + solsattwo + "at two.");
				for(int i= 0; i<possibletiles.Count ; i++){ //loop through all tiles ogtiles
					solutionpiecenames = new List<string>();
					solutionpieceposition = new List <Vector2>();
					newtiles = (string[,]) ogtiles.Clone();
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
							if((CreateMethod.piecetiles[0] == "Up" || CreateMethod.piecetiles[0] == "UpSeed")&& (CreateMethod.piecetiles[1] == "Down" || CreateMethod.piecetiles[1] == "DownSeed")){
								if(x==(int)possibletiles[j].x && (y > (int)possibletiles[j].y)){
									stop =true;
								}
							}
							if((CreateMethod.piecetiles[0] == "Down" || CreateMethod.piecetiles[0] == "DownSeed") && (CreateMethod.piecetiles[1] == "Up" || CreateMethod.piecetiles[1] == "UpSeed")){
								if(x==(int)possibletiles[j].x && (y <(int)possibletiles[j].y)){
									stop =true;
								}
							}
							if((CreateMethod.piecetiles[0] == "Left" || CreateMethod.piecetiles[0] == "LeftSeed") && (CreateMethod.piecetiles[1] == "Right" || CreateMethod.piecetiles[1] == "RightSeed")){
								if(y==(int)possibletiles[j].y && (x > (int)possibletiles[j].x)){
									stop =true;
								}
							}
							if((CreateMethod.piecetiles[0] == "Right" || CreateMethod.piecetiles[0] == "RightSeed")&& (CreateMethod.piecetiles[1] == "Left" || CreateMethod.piecetiles[1] == "LeftSeed")){
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
								solutionpieceposition.Add(new Vector2(x2,y2));
								solutionpiecenames.Add(CreateMethod.piecetiles[2]);
//								Debug.Log("Readytotest third" + x+y+x2+y2);
								bool stop2 = false;
								for(int k=0; k<possibletiles.Count;k++){
									if(j!=k && k!=i){
										if((CreateMethod.piecetiles[0] == "Up" || CreateMethod.piecetiles[0] == "UpSeed")&& (CreateMethod.piecetiles[2] == "Down" || CreateMethod.piecetiles[2] == "DownSeed")){
											if(x==(int)possibletiles[k].x && (y > (int)possibletiles[k].y)){
												stop2 =true;
											}
										}
										if((CreateMethod.piecetiles[0] == "Down" || CreateMethod.piecetiles[0] == "DownSeed") && (CreateMethod.piecetiles[2] == "Up" || CreateMethod.piecetiles[2] == "UpSeed")){
											if(x==(int)possibletiles[k].x && (y <(int)possibletiles[k].y)){
												stop2 =true;
											}
										}
										if((CreateMethod.piecetiles[0] == "Left" || CreateMethod.piecetiles[0] == "LeftSeed") && (CreateMethod.piecetiles[2] == "Right" || CreateMethod.piecetiles[2] == "RightSeed")){
											if(y==(int)possibletiles[k].y && (x > (int)possibletiles[k].x)){
												stop2 =true;
											}
										}
										if((CreateMethod.piecetiles[0] == "Right" || CreateMethod.piecetiles[0] == "RightSeed")&& (CreateMethod.piecetiles[2] == "Left" || CreateMethod.piecetiles[2] == "LeftSeed")){
											if(y==(int)possibletiles[k].y && (x <(int)possibletiles[k].x)){
												stop2 =true;
											}
										}	
										if((CreateMethod.piecetiles[1] == "Up" || CreateMethod.piecetiles[1] == "UpSeed")&& (CreateMethod.piecetiles[2] == "Down" || CreateMethod.piecetiles[2] == "DownSeed")){
											if(x2==(int)possibletiles[k].x && (y2 > (int)possibletiles[k].y)){
												stop2 =true;
											}
										}
										if((CreateMethod.piecetiles[1] == "Down" || CreateMethod.piecetiles[1] == "DownSeed") && (CreateMethod.piecetiles[2] == "Up" || CreateMethod.piecetiles[2] == "UpSeed")){
											if(x2==(int)possibletiles[k].x && (y2 <(int)possibletiles[k].y)){
												stop2 =true;
											}
										}
										if((CreateMethod.piecetiles[1] == "Left" || CreateMethod.piecetiles[1] == "LeftSeed") && (CreateMethod.piecetiles[2] == "Right" || CreateMethod.piecetiles[2] == "RightSeed")){
											if(y2==(int)possibletiles[k].y && (x2 > (int)possibletiles[k].x)){
												stop2 =true;
											}
										}
										if((CreateMethod.piecetiles[1] == "Right" || CreateMethod.piecetiles[1] == "RightSeed")&& (CreateMethod.piecetiles[2] == "Left" || CreateMethod.piecetiles[2] == "LeftSeed")){
											if(y2==(int)possibletiles[k].y && (x2 <(int)possibletiles[k].x)){
												stop2 =true;
											}
										}	
										if(!stop2){
											string[,] newerertiles = (string[,]) newertiles.Clone();
											int x3 = (int)possibletiles[k].x;
											int y3 = (int)possibletiles[k].y;
//											Debug.Log(x3 + "" +y3);
											if(solutionpieceposition.Count<3){
												solutionpieceposition.Add(new Vector2(x3,y3));
											}
											//currenttest2.Set(x2,y2);
											solutionpieceposition[2] = new Vector2(x3,y3);
			//								Debug.Log(solutionpieceposition[1]);
											solutionpiecenames.Add(CreateMethod.piecetiles[3]);
											newerertiles[x3,y3] = CreateMethod.piecetiles[2];
											solutionpieceposition.Add(new Vector2(x3,y3));
											bool stop3 = false;

											//Debug.Log("Solving for " +x+y+x2+y2);
											//Solve(newerertiles, test2);
											for(int l = 0; l<possibletiles.Count;l++){
												if(l!=k && l!=j && l!=i){
													if((CreateMethod.piecetiles[0] == "Up" || CreateMethod.piecetiles[0] == "UpSeed")&& (CreateMethod.piecetiles[3] == "Down" || CreateMethod.piecetiles[3] == "DownSeed")){
														if(x==(int)possibletiles[l].x && (y > (int)possibletiles[l].y)){
															stop3 =true;
														}
													}
													if((CreateMethod.piecetiles[0] == "Down" || CreateMethod.piecetiles[0] == "DownSeed") && (CreateMethod.piecetiles[3] == "Up" || CreateMethod.piecetiles[3] == "UpSeed")){
														if(x==(int)possibletiles[l].x && (y <(int)possibletiles[l].y)){
															stop3 =true;
														}
													}
													if((CreateMethod.piecetiles[0] == "Left" || CreateMethod.piecetiles[0] == "LeftSeed") && (CreateMethod.piecetiles[3] == "Right" || CreateMethod.piecetiles[3] == "RightSeed")){
														if(y==(int)possibletiles[l].y && (x > (int)possibletiles[l].x)){
															stop3 =true;
														}
													}
													if((CreateMethod.piecetiles[0] == "Right" || CreateMethod.piecetiles[0] == "RightSeed")&& (CreateMethod.piecetiles[3] == "Left" || CreateMethod.piecetiles[3] == "LeftSeed")){
														if(y==(int)possibletiles[l].y && (x <(int)possibletiles[l].x)){
															stop3 =true;
														}
													}	
													if((CreateMethod.piecetiles[1] == "Up" || CreateMethod.piecetiles[1] == "UpSeed")&& (CreateMethod.piecetiles[3] == "Down" || CreateMethod.piecetiles[3] == "DownSeed")){
														if(x2==(int)possibletiles[l].x && (y2 > (int)possibletiles[l].y)){
															stop3 =true;
														}
													}
													if((CreateMethod.piecetiles[1] == "Down" || CreateMethod.piecetiles[1] == "DownSeed") && (CreateMethod.piecetiles[3] == "Up" || CreateMethod.piecetiles[3] == "UpSeed")){
														if(x2==(int)possibletiles[l].x && (y2 <(int)possibletiles[l].y)){
															stop3 =true;
														}
													}
													if((CreateMethod.piecetiles[1] == "Left" || CreateMethod.piecetiles[1] == "LeftSeed") && (CreateMethod.piecetiles[3] == "Right" || CreateMethod.piecetiles[3] == "RightSeed")){
														if(y2==(int)possibletiles[l].y && (x2 > (int)possibletiles[l].x)){
															stop3 =true;
														}
													}
													if((CreateMethod.piecetiles[1] == "Right" || CreateMethod.piecetiles[1] == "RightSeed")&& (CreateMethod.piecetiles[3] == "Left" || CreateMethod.piecetiles[3] == "LeftSeed")){
														if(y2==(int)possibletiles[l].y && (x2 <(int)possibletiles[l].x)){
															stop3 =true;
														}
													}	
													if((CreateMethod.piecetiles[2] == "Up" || CreateMethod.piecetiles[2] == "UpSeed")&& (CreateMethod.piecetiles[3] == "Down" || CreateMethod.piecetiles[3] == "DownSeed")){
														if(x3==(int)possibletiles[l].x && (y3 > (int)possibletiles[l].y)){
															stop3 =true;
														}
													}
													if((CreateMethod.piecetiles[2] == "Down" || CreateMethod.piecetiles[2] == "DownSeed") && (CreateMethod.piecetiles[3] == "Up" || CreateMethod.piecetiles[3] == "UpSeed")){
														if(x3==(int)possibletiles[l].x && (y3 <(int)possibletiles[l].y)){
															stop3 =true;
														}
													}
													if((CreateMethod.piecetiles[2] == "Left" || CreateMethod.piecetiles[2] == "LeftSeed") && (CreateMethod.piecetiles[3] == "Right" || CreateMethod.piecetiles[3] == "RightSeed")){
														if(y3==(int)possibletiles[l].y && (x3 > (int)possibletiles[l].x)){
															stop3 =true;
														}
													}
													if((CreateMethod.piecetiles[2] == "Right" || CreateMethod.piecetiles[2] == "RightSeed")&& (CreateMethod.piecetiles[3] == "Left" || CreateMethod.piecetiles[3] == "LeftSeed")){
														if(y3==(int)possibletiles[l].y && (x3 <(int)possibletiles[l].x)){
															stop3 =true;
														}
													}	
													if(!stop3){
														string[,] newererertiles = (string[,]) newerertiles.Clone();
														int x4 = (int)possibletiles[l].x;
														int y4 = (int)possibletiles[l].y;
			//											Debug.Log(x3 + "" +y3);
														if(solutionpieceposition.Count<3){
															solutionpieceposition.Add(new Vector2(x4,y4));
														}
														//currenttest2.Set(x2,y2);
														solutionpieceposition[3] = new Vector2(x4,y4);
						//								Debug.Log(solutionpieceposition[1]);
														newererertiles[x4,y4] = CreateMethod.piecetiles[3];
														//Debug.Log("Solving for " +x+y+x2+y2);
														Solve(newererertiles);														
													}	
													stop3 = false;												
												}
											}
										}	
										stop2 = false;						
									}
								}
							}
							stop = false;
						}
					}
				}

				Debug.Log(solutions.Count);
				for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
					//Debug.Log(solutions[i].myturns + "Turns with one piece" );
					//Debug.Log(solutions[i].solutionpositions[0] + "" + solutions[i].solutionpositions[1] + solutions[i].myturns);
					int bestturns = bestsol;
					CountOrStay(solutions[i].myturns, solutions[i]);
					if((bestsol < bestturns) || (bestturns == 0 && bestsol != 0) || (bestsol == solutions[i].myturns)){
					CheckWallHug(solutions[i]);
					CheckStoppedSeed(solutions[i]);
					}				
					bestsolutions.Add(bestsol);
					Debug.Log(bestsolutions.Count);
				}
				break;
		}
	}

	public void PopulateProbableTiles(){
		possibletiles.Clear();
		for(int j = 0; j<10; j++){
			for(int k= 0; k<10; k++){ //loop through all tiles ogtiles
				if(ogtiles[k,j]=="Ice"){
					possibletiles.Add(new Vector2(k,j));
				}
			}
		}
	}

	public void CheckInLineStoppedTiles(){

	}
	public void CycleOne(){

	}
	public void CycleTwo(){

	}
	public void CycleThree(){
		
	}
	public void CheckStoppedSeed(Solution solutiontocheck){ //for filtering out solutions where you stand on top of the seed.
		gottago = false;
		for (int i = 0; i < CreateMethod.piecetiles.Count; i++){
			if (solutiontocheck.piecetags[i] == "WallSeed" || solutiontocheck.piecetags[i] == "LeftSeed" || 
				solutiontocheck.piecetags[i] == "RightSeed" || solutiontocheck.piecetags[i] == "UpSeed" || solutiontocheck.piecetags[i] == "DownSeed"){
				for (int j = 0; j<solutiontocheck.stoppedtiles.Count; j++){
					if(solutiontocheck.stoppedtiles[j] == solutiontocheck.solutionpositions[i]){
						gottago = true;
					}
				}
			}
		}
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
		ogtiles = thistiles; //og Map
		solvingtiles = (string[,]) ogtiles.Clone();
		Solve(ogtiles);			//solves for no pieces
		if(solutions.Count>0){
			for(int i =0; i<solutions.Count; i++){//This is what happens if theres a solution at origin.
//				Debug.Log(solutions[i].myturns + "Turns with no piece" );
				CountOrStay(solutions[i].myturns, solutions[i]);
			}
//			Debug.Log(bestsol + "is the best empty sol");
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
		//PrintBestSols();
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
		bool good = false;
		for(int i = 0; i <bestsolutions.Count; i++){
			if(bestsolutions[i] == 0){

			}
			if (bestsolutions[i] != 0 && i == bestsolutions.Count-1){
				good = true;
				Debug.Log("good is true");
			}
		}
		string solutionnums = "";
			for(int i = 0; i<bestsolutions.Count; i++){
				solutionnums = solutionnums + "" + bestsolutions[i].ToString();
		}
		if(good == true){
		Debug.Log(solutionnums + repeatedbestsol + besthaswallhug.Count);

		}
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
			//	Debug.Log(piecetocheck);
				if((int)piecetocheck.x > 1){
					if((ogtiles[(int)(piecetocheck.x - 2), (int)piecetocheck.y] == "Wall") || 
						(ogtiles[(int)(piecetocheck.x - 2), (int)piecetocheck.y] == "Start")){
//						Debug.Log("Wallhugged Left");
						besthaswallhug.Add(true);
					}
					else{
//					Debug.Log("Didnt hug Left");
					}
				}
			}
			if(CreateMethod.piecetiles[i] == "Right"){
				Vector2 piecetocheck = solutiontocheck.solutionpositions[i];
			//	Debug.Log(piecetocheck);
				if((int)piecetocheck.x < 6){
					if((ogtiles[(int)(piecetocheck.x + 2), (int)piecetocheck.y] == "Wall") || 
						(ogtiles[(int)(piecetocheck.x + 2), (int)piecetocheck.y] == "Start")){
//						Debug.Log("Wallhugged Right");
						besthaswallhug.Add(true);
					}
					else{
//						Debug.Log("Didnt hug Right");
					}
				}
			}
			if(CreateMethod.piecetiles[i] == "Up"){
				Vector2 piecetocheck = solutiontocheck.solutionpositions[i];
//				Debug.Log(piecetocheck);
				if((int)piecetocheck.y > 1){
					if((ogtiles[(int)(piecetocheck.x), (int)piecetocheck.y - 2] == "Wall") || 
						(ogtiles[(int)(piecetocheck.x), (int)piecetocheck.y - 2] == "Start")){
						// Debug.Log("Wallhugged Up");
						besthaswallhug.Add(true);
					}
					else{
						// Debug.Log("Didnt hug Up");
					}
				}

			}
			if(CreateMethod.piecetiles[i] == "Down"){
				Vector2 piecetocheck = solutiontocheck.solutionpositions[i];
				//Debug.Log(piecetocheck);
				if((int)piecetocheck.y < 6){
					if((ogtiles[(int)(piecetocheck.x), (int)piecetocheck.y + 2] == "Wall") || 
						(ogtiles[(int)(piecetocheck.x), (int)piecetocheck.y + 2] == "Start")){
						// Debug.Log("Wallhugged Down");
						besthaswallhug.Add(true);
					}
					else{
						// Debug.Log("Didnt hug Down");
					}
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