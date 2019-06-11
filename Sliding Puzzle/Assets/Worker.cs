using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker{

	public Solution mysolution = new Solution();
	public int turns;		//number of moves the worker has made
	public int piecesused;								
	public string[,] mytiles = new string[8,8];						//This is the map
	public List<Vector2> stoppedtiles;	
	Vector2 laststopped;							//Where the worker has stopped.
	public int x;
	public int y;
	public List <string> mygenes;
	public string direction;
	bool firstmove;
	bool Donemoving;
	public bool kill;
	bool done;
	public bool dead;
	int mysolutionnumber;
	public string[,] boardpieces = new string[8,8];
	string previoustag;
	//public Vector2 test2 = new Vector2(0,0);
	public bool isshifted = false;
	public int lrud;
	public List<Vector2> lrudpos;
	public List<Map> stoppedmaps;
	public int piecesturned;
	public List<int> turnedperturn;
	//public <Vector2> pieces

	public Worker(int newturns, int newx, int newy,string[,] newtiles, 
		string newdirection, List<string> newgenes, List<Vector2> newstopped){
		turns = newturns;
		x= newx;
		y = newy;
		mytiles = (string[,]) newtiles.Clone();
		direction = newdirection;
		mygenes = new List<string>(newgenes);
		stoppedtiles = new List<Vector2>(newstopped);
//		Debug.Log("Myposition" + x + "+" + y);
	}
	public Worker(int newturns, int newx, int newy,string[,] newtiles, 
		string newdirection, List<string> newgenes, List<Vector2> newstopped, int newlrud){
		turns = newturns;
		x= newx;
		y = newy;
		mytiles = (string[,]) newtiles.Clone();
		direction = newdirection;
		mygenes = new List<string>(newgenes);
		stoppedtiles = new List<Vector2>(newstopped);
		lrud = newlrud;
//		Debug.Log("Myposition" + x + "+" + y);
	}
	public Worker(int newturns, int newx, int newy,string[,] newtiles, 
		string newdirection, List<string> newgenes, List<Vector2> newstopped, int newlrud, List<Vector2> newlrudpos){
		turns = newturns;
		x= newx;
		y = newy;
		mytiles = (string[,]) newtiles.Clone();
		direction = newdirection;
		mygenes = new List<string>(newgenes);
		stoppedtiles = new List<Vector2>(newstopped);
		lrud = newlrud;
		lrudpos = newlrudpos;
//		Debug.Log("Myposition" + x + "+" + y);
	}
	public Worker(int newturns, int newx, int newy,string[,] newtiles, 
		string newdirection, List<string> newgenes, List<Vector2> newstopped, int newlrud, List<Vector2> newlrudpos, List<Map> newstoppedmaps){
		turns = newturns;
		x= newx;
		y = newy;
		mytiles = (string[,]) newtiles.Clone();
		direction = newdirection;
		mygenes = new List<string>(newgenes);
		stoppedtiles = new List<Vector2>(newstopped);
		lrud = newlrud;
		lrudpos = newlrudpos;
		stoppedmaps = newstoppedmaps;
//		Debug.Log("Myposition" + x + "+" + y);
	}
	/*public Worker(int newturns, int newx, int newy,string[,] newtiles, 
		string newdirection, List<string> newgenes, List<Vector2> newstopped, int newlrud, List<Vector2> newlrudpos, int newpiecesturned){
		turns = newturns;
		x= newx;
		y = newy;
		mytiles = (string[,]) newtiles.Clone();
		direction = newdirection;
		mygenes = new List<string>(newgenes);
		stoppedtiles = new List<Vector2>(newstopped);
		lrud = newlrud;
		lrudpos = newlrudpos;
		//stoppedmaps = newstoppedmaps;
		piecesturned = newpiecesturned;
//		Debug.Log("Myposition" + x + "+" + y);
	}*/
	public Worker(int newturns, int newx, int newy,string[,] newtiles, 
		string newdirection, List<string> newgenes, List<Vector2> newstopped, int newlrud, List<Vector2> newlrudpos, List<int> newturnedperturn){
		turns = newturns;
		x= newx;
		y = newy;
		mytiles = (string[,]) newtiles.Clone();
		direction = newdirection;
		mygenes = new List<string>(newgenes);
		stoppedtiles = new List<Vector2>(newstopped);
		lrud = newlrud;
		lrudpos = newlrudpos;
		//stoppedmaps = newstoppedmaps;
		turnedperturn = newturnedperturn;
		piecesturned = turnedperturn[turnedperturn.Count-1];
//		Debug.Log("Myposition" + x + "+" + y);
	}
	public Worker(int newturns, int newx, int newy,string[,] newtiles, 
		string newdirection, List<string> newgenes, List<Vector2> newstopped, int newlrud, List<Vector2> newlrudpos, int newpiecesused){
		piecesused = newpiecesused;
		turns = newturns;
		x= newx;
		y = newy;
		mytiles = (string[,]) newtiles.Clone();
		direction = newdirection;
		mygenes = new List<string>(newgenes);
		stoppedtiles = new List<Vector2>(newstopped);
		lrud = newlrud;
		lrudpos = newlrudpos;
		//stoppedmaps = newstoppedmaps;
		//turnedperturn = newturnedperturn;
//		piecesturned = turnedperturn[turnedperturn.Count-1];
//		Debug.Log("Myposition" + x + "+" + y);
	}
	public void Move(){
		mygenes.Add(direction);
		int shiftx = 0;
		int shifty = 0;
		if(direction == "Left"){
			shiftx = -1;
		}
		if(direction == "Right"){
			shiftx = 1;
		}
		if(direction == "Up"){
			shifty = -1;
		}
		if(direction == "Down"){
			shifty = 1;
		}
		if (mytiles[x,y]== "Start"){
			mytiles[x,y] = "Wall";
//			Debug.Log("Now a wall");
//			Debug.Log(SolveMethod.ogtiles[x,y]);
		}

		Donemoving = false;
		firstmove = true;
		int newx = x + shiftx;
		int newy = y + shifty;
		//Debug.Log(newx + "+" + newy);
		while(!Donemoving){
//			Debug.Log(newx + "" + newy);
			//if(0<=newx<8 && 0<newy<8){
				string newtag = mytiles[newx,newy];
				ActonTile(newtag, newx, newy);
				if(isshifted){//do this when its ran into a lrud
					if(previoustag == "Left" || previoustag == "FragileLeft"){
						shiftx = -1;
						shifty = 0;
					}
					if(previoustag == "Right" || previoustag == "FragileRight"){
						shiftx = 1;
						shifty = 0;
					}
					if(previoustag == "Down" || previoustag == "FragileDown"){
						shiftx = 0;
						shifty =1;
					}
					if(previoustag == "Up" || previoustag == "FragileUp"){
						shiftx = 0;
						shifty = -1;
					}
					isshifted = false;
				}
				newx = x + shiftx;
				newy = y + shifty;
				//}
			}
		laststopped = new Vector2(x,y);
		/*if(stoppedtiles.Count>3){
			stoppedtiles.RemoveAt(0);
			Debug.Log(stoppedtiles.Count);
		}*/
		//turnedperturn.Add(piecesturned);
		//if(done!= true){
			//done = CheckRepetitionEasy();
		//}
		if(done == true){
			return;
		}
		for(int i=0; i<stoppedtiles.Count; i++){
			if(stoppedtiles[i].x == x && stoppedtiles[i].y == y){ //if repeated stoppedtile
				done = true;
				break;
			}
			else{

			}
		}
		if(done != true){
			stoppedtiles.Add(laststopped);
			//stoppedmaps.Add(new Map(mytiles));
			//turnedperturn.Add(piecesturned);
			SolveMethod.workersalive.Add(this);
			//stoppedtiles.Add(laststopped);
		}

	}
	public bool CheckRepetition(){
		for(int i=0; i<stoppedtiles.Count; i++){
			if(stoppedtiles[i].x == x && stoppedtiles[i].y == y){ //if repeated stoppedtile
				if(piecesturned == turnedperturn[i]){
					//Debug.Log(piecesturned + " p " + turnedperturn[i]);
					return true;
				}
			}
		}		
		return false;
	}
	public bool CheckRepetitionEasy(){
		for(int i=0; i<stoppedtiles.Count; i++){
			if(stoppedtiles[i].x == x && stoppedtiles[i].y == y){ //if repeated stoppedtile
				//if map == starting map

				return true;
			}	
		}	
		return false;
	}
	public bool mapHasChanged(int position){
		//Debug.Log(mytiles[1,1] + "1" + SolveMethod.solvingtiles[1,1]);
		for(int i = 0; i < mytiles.GetLength(0); i++){
			for(int j =0; j < mytiles.GetLength(1); j++){
				if(mytiles[i,j] != stoppedmaps[position].tiles[i,j]){
					if(stoppedmaps[position].tiles[i,j] != "Start" && stoppedmaps[position].tiles[i,j] != "Fragile"){
						//Debug.Log("Foundit");
						return true;
					}
					//Debug.Log("Not equal at" + i + j + "first is" + mytiles[i,j] + "then its " + SolveMethod.solvingtiles[i,j]);
				}
			}
		}
		return false;
	}

	public void ActonTile(string newtag, int tilex, int tiley){
		if(x==2 && y==2){
//			Debug.Log(mytiles[x,y]);
//			Debug.Log(mytiles[tilex,tiley]);
		}
//		Debug.Log("firstmove" + firstmove);
		if(firstmove){
			turns++;
		//	Debug.Log(turns+ " turns");
		}
		if(newtag == "Ice" || newtag == "Wood"){
			x = tilex;
			y = tiley;
			previoustag = "Ice";
		}
		//Debug.Log(turns + "Turns");
		if(newtag == "Goal"){
			if(turns > 0){
//				Debug.Log("MY lrud" + lrud + "turns" + turns);
//				Debug.Log("best turns is" + SolveMethod.bestturns);
				x = tilex;
				y = tiley;
				done = true;
				mysolutionnumber = SolveMethod.numberofsolutions;
				mysolution = new Solution(mysolutionnumber, turns, mygenes, SolveMethod.solutionpieceposition, SolveMethod.solutionpiecenames, stoppedtiles, lrud, piecesused);
				if(turns<=SolveMethod.bestturns || SolveMethod.bestturns == 0){
					SolveMethod.solutions.Add(mysolution);
					SolveMethod.bestturns = turns;

				}
//				if(SolveMethod.solutionpieceposition.Count>1)
//				Debug.Log(SolveMethod.solutionpieceposition[1]);
//				Debug.Log(SolveMethod.solutions.Count + "" + turns + SolveMethod.currenttest + SolveMethod.currenttest2 + test2);
				previoustag = "Goal";
				//SolveMethod.numberofsolutions++;
//				Debug.Log("Got goal at "+ x + ","+ y);
				Donemoving = true;
				CreateMethod.goalpos = new Vector2(tilex,tiley);
//				Debug.Log(CreateMethod.goalpos);
				if(turns<CreateMethod.startingturns){
					SolveMethod.crapsolution = true;
				}

			}
			else{
				Debug.Log("NNON" + turns);
				Donemoving=true;
				done=true;
			}


			//Save solution
		}
		if(newtag == "Hole"){
			x=tilex;
			y=tiley;
			done = true; //not really repeated, just dead but this value is so it wont create a new worker.
			Donemoving = true;
			previoustag = "Hole";
		}
		if(newtag == "Fragile"){
			x=tilex;
			y=tiley;
			previoustag = "Fragile";
			mytiles[x,y] = "Hole";
			//newtag = "Hole";
		}	
		if(newtag == "FragileUp"){
			x=tilex;
			y=tiley;
			if(previoustag != "UpSeed" && !repeatlrud(new Vector2(x,y))){
				lrud++;
				lrudpos.Add(new Vector2(x,y));
			}
			previoustag = "FragileUp";
			mytiles[x,y] = "Hole";
			//newtag = "Hole";
			isshifted = true;
			piecesused++;

		}		
		if(newtag == "FragileLeft"){
			x=tilex;
			y=tiley;
			if(previoustag != "LeftSeed" && !repeatlrud(new Vector2(x,y))){
				lrud++;
				lrudpos.Add(new Vector2(x,y));
			}
			previoustag = "FragileLeft";

			mytiles[x,y] = "Hole";
			//newtag = "Hole";
			isshifted = true;
			piecesused++;
		}		
		if(newtag == "FragileRight"){
			x=tilex;
			y=tiley;
			if(previoustag != "RightSeed" && !repeatlrud(new Vector2(x,y))){
				lrud++;
				lrudpos.Add(new Vector2(x,y));
			}
			previoustag = "FragileRight";
			mytiles[x,y] = "Hole";
			//newtag = "Hole";
			isshifted = true;
			piecesused++;
		}		
		if(newtag == "FragileDown"){
			x=tilex;
			y=tiley;
			if(previoustag != "DownSeed" && !repeatlrud(new Vector2(x,y))){
				lrud++;
				lrudpos.Add(new Vector2(x,y));
			}
			previoustag = "FragileDown";
			mytiles[x,y] = "Hole";
			//newtag = "Hole";
			isshifted = true;
			piecesused++;
		}			
		if(newtag == "Wall"){
			if(firstmove){
				Debug.Log("Cant go that way" + x + "+" + y + "+" + direction);
				Donemoving = true;
			}
			if(previoustag == "Fragile" || previoustag == "FragileLeft" || previoustag == "FragileRight" || 
				previoustag == "FragileUp" || previoustag == "FragileDown"){
//				Debug.Log("Died from fragile");
				done = true;
				Donemoving = true;
			}
//			Debug.Log("Moved to "+ x + ","+ y);
			Donemoving = true;
			previoustag = "Wall";
			//Debug.Log("Hitit");
			//Check for fragile
			//Check if it's a wall in Solvemethod.ogtiles too.
			if(SolveMethod.ogtiles[tilex,tiley] == "Ice"){
				//Debug.Log(x + " " + y + SolveMethod.ogtiles[x,y]);
				piecesused++;
			}
		}
		if(newtag == "Left"){
			/*if(previoustag == "Right"){
				x=tilex;
				y=tiley;
				done = true;
				Donemoving = true;
				previoustag = "Left";
				Debug.Log("Not there");
				return;
			}*/
			x=tilex;
			y=tiley;
			if(previoustag != "LeftSeed" && !repeatlrud(new Vector2(x,y))){
				lrud++;
				lrudpos.Add(new Vector2(x,y));
			}
			previoustag = "Left";
			isshifted = true;
//			Debug.Log(tilex + "" + tiley);
			piecesused++;
		}
		if(newtag == "Right"){
			/*if(previoustag == "Left"){
				x=tilex;
				y=tiley;
				done = true;
				Donemoving = true;
				previoustag = "Right";
				return;
			}*/
			x=tilex;
			y=tiley;
			if(previoustag != "RightSeed" && !repeatlrud(new Vector2(x,y))){
				lrud++;
				lrudpos.Add(new Vector2(x,y));
			}
			previoustag = "Right";
			isshifted = true;
			piecesused++;
		}
		if(newtag == "Up"){
			/*if(previoustag == "Down"){
				x=tilex;
				y=tiley;
				done = true;
				Donemoving = true;
				previoustag = "Up";
				return;
			}*/
			x=tilex;
			y=tiley;
			if(previoustag != "UpSeed" && !repeatlrud(new Vector2(x,y))){
				lrud++;
				lrudpos.Add(new Vector2(x,y));
			}
			previoustag = "Up";
			isshifted = true;
			piecesused++;
		}
		if(newtag == "Down"){
			/*if(previoustag == "Up"){
				x=tilex;
				y=tiley;
				done = true;
				Donemoving = true;
				previoustag = "Down";
				return;
			}*/
			x=tilex;
			y=tiley;
			if(previoustag != "DownSeed" && !repeatlrud(new Vector2(x,y))){
				lrud++;
				lrudpos.Add(new Vector2(x,y));
			}
			previoustag = "Down";
			isshifted = true;
			piecesused++;
		}
		if(newtag == "WallSeed"){
			x = tilex;
			y = tiley;
			previoustag = "WallSeed";
			mytiles[x,y] = "Wall";
			piecesturned++;
		}
		if(newtag == "LeftSeed"){
			x = tilex;
			y = tiley;
			previoustag = "LeftSeed";
			mytiles[x,y] = "Wall";
			if(mytiles[x-1,y] == "Ice"){
				mytiles[x-1,y] = "Left";
			}
			if(mytiles[x-1,y] == "Fragile"){
				mytiles[x-1,y] = "FragileLeft";
			}
			if(mytiles[x-1,y] == "Wood"){
				mytiles[x-1,y] = "Left";
			}
			piecesturned++;
		}
		if(newtag == "RightSeed"){
			x = tilex;
			y = tiley;
			previoustag = "RightSeed";
			mytiles[x,y] = "Wall";
			if(mytiles[x+1,y] == "Ice"){
				mytiles[x+1,y] = "Right";
			}
			if(mytiles[x+1,y] == "Fragile"){
				mytiles[x+1,y] = "FragileRight";
			}
			if(mytiles[x+1,y] == "Wood"){
				mytiles[x+1,y] = "Right";
			}
			piecesturned++;
		}
		if(newtag == "UpSeed"){
			x = tilex;
			y = tiley;
			previoustag = "UpSeed";
			mytiles[x,y] = "Wall";
			if(mytiles[x,y-1] == "Ice"){
				mytiles[x,y-1] = "Up";
			}
			if(mytiles[x,y-1] == "Fragile"){
				mytiles[x,y-1] = "FragileUp";
			}
			if(mytiles[x,y-1] == "Wood"){
				mytiles[x,y-1] = "Up";
			}
			piecesturned++;
		}
		if(newtag == "DownSeed"){
			x = tilex;
			y = tiley;
			previoustag = "DownSeed";
			mytiles[x,y] = "Wall";
			if(mytiles[x,y+1] == "Ice"){
				mytiles[x,y+1] = "Down";
			}
			if(mytiles[x,y+1] == "Fragile"){
				mytiles[x,y+1] = "FragileDown";
			}
			if(mytiles[x,y+1] == "Wood"){
				mytiles[x,y+1] = "Down";
			}
			piecesturned++;
		}
		firstmove = false;
	}
	public void AssignPieces(){
		for(int i=0; i<8; i++){
			for(int j=0; j<8; j++){
				if(mytiles[i,j]!=SolveMethod.ogtiles[i,j]){
					Debug.Log("Not the same");
				}
				Debug.Log("Checking tile" + i + "+" + j);
			}
		}
	}
	public bool repeatlrud(Vector2 Pos){
		for(int i=0; i < lrudpos.Count; i++){
			if(lrudpos[i] == Pos){
				return true;
			}
		}
		return false;
	}
}
