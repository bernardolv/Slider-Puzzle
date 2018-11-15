
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution{
	public int numsolution;
	public List<string> mygenes;
	public int myturns;
	public int x;
	public int y;
	public int x2;
	public int y2;
	public List<Vector2> solutionpositions = new List<Vector2>();
	public List<string> piecetags = new List<string>();
	public Solution(int num, int turns, List<string> newgenes, Vector2 theplace){
		numsolution = num;
		myturns = turns;
		mygenes = newgenes;
//		Debug.Log(theplace);
		x = Mathf.RoundToInt(theplace.x);
		y = Mathf.RoundToInt(theplace.y);
	
	}
	public Solution(){
		//myx=1;
		//myy=1;
	}
	public Solution(int num, int turns, List<string> newgenes, Vector2 theplace, Vector2 theplace2){
		numsolution = num;
		myturns = turns;
		mygenes = newgenes;
//		Debug.Log(theplace);
		x = Mathf.RoundToInt(theplace.x);
		y = Mathf.RoundToInt(theplace.y);
		x2 = Mathf.RoundToInt(theplace2.x);
		y2 = Mathf.RoundToInt(theplace2.y);
	}
	public Solution(int num, int turns, List<string> newgenes, List<Vector2> solutionpieces, List<string> piecenames){
		numsolution = num;
		myturns = turns;
		mygenes = newgenes;
		piecetags = new List<string>(piecenames);
		solutionpositions = new List<Vector2>(solutionpieces);
	}
}