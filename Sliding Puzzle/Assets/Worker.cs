using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker{

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
	bool repeated;

	public Worker(int newturns, int newx, int newy,string[,] newtiles, string newdirection, List<string> newgenes, List<Vector2> newstopped){
		turns = newturns;
		x= newx;
		y = newy;
		mytiles = newtiles;
		direction = newdirection;
		mygenes = newgenes;
		stoppedtiles = newstopped;
		Debug.Log("Myposition" + x + "+" + y);
	}

	public void Move(){
		mygenes.Add(direction);
		//direction = newdirection;
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
		Donemoving = false;
		firstmove = true;
		int newx = x + shiftx;
		int newy = y + shifty;
		while(!Donemoving){
			string newtag = AIBrain.tiles[newx,newy];
			ActonTile(newtag, newx, newy);
			newx = x + shiftx;
			newy = y + shifty;
		}
		laststopped = new Vector2(x,y);
		for(int i=0; i<stoppedtiles.Count; i++){
			if(stoppedtiles[i].x == x && stoppedtiles[i].y == y){ //if repeated stoppedtile
			Debug.Log("Repeated");
			repeated = true;
			}
			else{

			}
		}
		if(repeated != true){
			Debug.Log("Added");
			SolveMethod.workersalive.Add(this);
			stoppedtiles.Add(laststopped);
		}

	}
	public void ActonTile(string newtag, int tilex, int tiley){
		if(newtag == "Ice" || newtag == "Goal" || newtag == "Start"){
			x = tilex;
			y = tiley;
		}
		if(newtag == "Wall"){
			if(firstmove){
				Debug.Log("Cant go that way" + x + "+" + y + "+" + direction);
				Donemoving = true;
			}
			Debug.Log("Moved to "+ x + ","+ y);
			Donemoving = true;
			Debug.Log("Hitit");
		}
		firstmove = false;
	}
}
