using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker{

	public Solution mysolution = new Solution();
	public int turns;												//number of moves the worker has made
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
		for(int i=0; i<stoppedtiles.Count; i++){
			if(stoppedtiles[i].x == x && stoppedtiles[i].y == y){ //if repeated stoppedtile
			done = true;
			}
			else{

			}
		}
		if(done != true){
			stoppedtiles.Add(laststopped);

			SolveMethod.workersalive.Add(this);
			//stoppedtiles.Add(laststopped);
		}

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
				Debug.Log("MY lrud" + lrud);
				x = tilex;
				y = tiley;
				done = true;
				mysolutionnumber = SolveMethod.numberofsolutions;
				mysolution = new Solution(mysolutionnumber, turns, mygenes, SolveMethod.solutionpieceposition, SolveMethod.solutionpiecenames, stoppedtiles, lrud);
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
			if(previoustag != "UpSeed"){
				lrud++;
			}
			previoustag = "FragileUp";
			mytiles[x,y] = "Hole";
			//newtag = "Hole";
			isshifted = true;

		}		
		if(newtag == "FragileLeft"){
			x=tilex;
			y=tiley;
			if(previoustag != "LeftSeed"){
				lrud++;
			}
			previoustag = "FragileLeft";

			mytiles[x,y] = "Hole";
			//newtag = "Hole";
			isshifted = true;

		}		
		if(newtag == "FragileRight"){
			x=tilex;
			y=tiley;
			if(previoustag != "RightSeed"){
				lrud++;
			}
			previoustag = "FragileRight";
			mytiles[x,y] = "Hole";
			//newtag = "Hole";
			isshifted = true;

		}		
		if(newtag == "FragileDown"){
			x=tilex;
			y=tiley;
			if(previoustag != "DownSeed"){
				lrud++;
			}
			previoustag = "FragileDown";
			mytiles[x,y] = "Hole";
			//newtag = "Hole";
			isshifted = true;

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
			if(previoustag != "LeftSeed"){
				lrud++;
			}
			previoustag = "Left";
			isshifted = true;
//			Debug.Log(tilex + "" + tiley);
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
			if(previoustag != "RightSeed"){
				lrud++;
			}
			previoustag = "Right";
			isshifted = true;

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
			if(previoustag != "UpSeed"){
				lrud++;
			}
			previoustag = "Up";
			isshifted = true;

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
			if(previoustag != "DownSeed"){
				lrud++;
			}
			previoustag = "Down";
			isshifted = true;

		}
		if(newtag == "WallSeed"){
			x = tilex;
			y = tiley;
			previoustag = "WallSeed";
			mytiles[x,y] = "Wall";
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
}
