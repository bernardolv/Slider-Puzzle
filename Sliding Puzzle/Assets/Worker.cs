﻿using System.Collections;
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
	public Vector2 test2 = new Vector2(0,0);
	public bool isshifted = false;
	//public <Vector2> pieces

	public Worker(int newturns, int newx, int newy,string[,] newtiles, string newdirection, List<string> newgenes, List<Vector2> newstopped){
		turns = newturns;
		x= newx;
		y = newy;
		mytiles = (string[,]) newtiles.Clone();
		direction = newdirection;
		mygenes = new List<string>(newgenes);
		stoppedtiles = new List<Vector2>(newstopped);
//		Debug.Log("Myposition" + x + "+" + y);
	}
	public Worker(int newturns, int newx, int newy,string[,] newtiles, string newdirection, List<string> newgenes, List<Vector2> newstopped, Vector2 testv){
		test2 = testv;
		turns = newturns;
		x= newx;
		y = newy;
		mytiles = (string[,]) newtiles.Clone();
		direction = newdirection;
		mygenes = new List<string>(newgenes);
		stoppedtiles = new List<Vector2>(newstopped);
//		Debug.Log("Myposition" + x + "+" + y);
	}

	public void Move(Vector2 testv){
		test2 = testv;
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
				if(isshifted){
					if(previoustag == "Left"){
						shiftx = -1;
						shifty = 0;
					}
					if(previoustag == "Right"){
						shiftx = 1;
						shifty = 0;
					}
					if(previoustag == "Down"){
						shiftx = 0;
						shifty =1;
					}
					if(previoustag == "Up"){
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
			SolveMethod.workersalive.Add(this);
			stoppedtiles.Add(laststopped);
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
				x = tilex;
				y = tiley;
				done = true;
				mysolutionnumber = SolveMethod.numberofsolutions;
				mysolution = new Solution(mysolutionnumber, turns, mygenes, SolveMethod.solutionpieceposition, SolveMethod.solutionpiecenames);
				SolveMethod.solutions.Add(mysolution);
				if(SolveMethod.solutionpieceposition.Count>1)
//				Debug.Log(SolveMethod.solutionpieceposition[1]);
//				Debug.Log(SolveMethod.solutions.Count + "" + turns + SolveMethod.currenttest + SolveMethod.currenttest2 + test2);
				previoustag = "Goal";
				//SolveMethod.numberofsolutions++;
//				Debug.Log("Got goal at "+ x + ","+ y);
				Donemoving = true;
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
		if(newtag == "Wall"){
			if(firstmove){
				Debug.Log("Cant go that way" + x + "+" + y + "+" + direction);
				Donemoving = true;
			}
			if(previoustag == "Fragile"){
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
			if(previoustag == "Right"){
				x=tilex;
				y=tiley;
				done = true;
				Donemoving = true;
				previoustag = "Left";
				return;
			}
			x=tilex;
			y=tiley;
			previoustag = "Left";
			isshifted = true;
		}
		if(newtag == "Right"){
			if(previoustag == "Left"){
				x=tilex;
				y=tiley;
				done = true;
				Donemoving = true;
				previoustag = "Right";
				return;
			}
			x=tilex;
			y=tiley;
			previoustag = "Right";
			isshifted = true;
		}
		if(newtag == "Up"){
			if(previoustag == "Down"){
				x=tilex;
				y=tiley;
				done = true;
				Donemoving = true;
				previoustag = "Up";
				return;
			}
			x=tilex;
			y=tiley;
			previoustag = "Up";
			isshifted = true;
		}
		if(newtag == "Down"){
			if(previoustag == "Up"){
				x=tilex;
				y=tiley;
				done = true;
				Donemoving = true;
				previoustag = "Down";
				return;
			}
			x=tilex;
			y=tiley;
			previoustag = "Down";
			isshifted = true;
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
