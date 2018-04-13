
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution{
	public int numsolution;
	public List<string> mygenes;
	public int myturns;
	public int x;
	public int y;
	public Solution(int num, int turns, List<string> newgenes, Vector2 theplace){
		numsolution = num;
		myturns = turns;
		mygenes = newgenes;
		x = Mathf.RoundToInt(theplace.x);
		y = Mathf.RoundToInt(theplace.y);
	
	}
	public Solution(){
		//myx=1;
		//myy=1;
	}
}