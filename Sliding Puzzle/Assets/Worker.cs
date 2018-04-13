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
	//public <Vector2> pieces

	public Worker(int newturns, int newx, int newy,string[,] newtiles, string newdirection, List<string> newgenes, List<Vector2> newstopped){
		turns = newturns;
		x= newx;
		y = newy;
		mytiles = (string[,]) newtiles.Clone();
		direction = newdirection;
		mygenes = new List<string>(newgenes);
		stoppedtiles = new List<Vector2>(newstopped);
		Debug.Log("Myposition" + x + "+" + y);
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
			Debug.Log("Now a wall");
			Debug.Log(SolveMethod.ogtiles[x,y]);
		}

		Donemoving = false;
		firstmove = true;
		int newx = x + shiftx;
		int newy = y + shifty;
		while(!Donemoving){
			string newtag = mytiles[newx,newy];
			ActonTile(newtag, newx, newy);
			newx = x + shiftx;
			newy = y + shifty;
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
			SolveMethod.workersalive.Add(this);
			stoppedtiles.Add(laststopped);
		}

	}
	public void ActonTile(string newtag, int tilex, int tiley){
				if(x==2 && y==2){
			Debug.Log(mytiles[x,y]);
			Debug.Log(mytiles[tilex,tiley]);
		}
		if(firstmove){
			turns++;
			//Debug.Log(turns+ " turns");
		}
		if(newtag == "Ice"){
			x = tilex;
			y = tiley;
		}

		if(newtag == "Goal"){
			if(turns > 0){
				x = tilex;
				y = tiley;
				done = true;
				mysolutionnumber = SolveMethod.numberofsolutions;
				mysolution = new Solution(mysolutionnumber, turns, mygenes, SolveMethod.currenttest );
				SolveMethod.solutions.Add(mysolution);
				//SolveMethod.numberofsolutions++;
				Debug.Log("Got goal at "+ x + ","+ y);
			}
			else{
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
		}
		if(newtag == "Wall" || newtag == "Start"){
			if(firstmove){
				Debug.Log("Cant go that way" + x + "+" + y + "+" + direction);
				Donemoving = true;
			}
			Debug.Log("Moved to "+ x + ","+ y);
			Donemoving = true;
			//Debug.Log("Hitit");
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
