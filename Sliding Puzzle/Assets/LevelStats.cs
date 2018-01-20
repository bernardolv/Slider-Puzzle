using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats {
	public int levelnum;
	public int turns;
	public bool islocked;
	public int rating;

	public LevelStats (int newlevelnum, int newturns, bool newislocked, int newrating){
		levelnum = newlevelnum;
		turns = newturns;
		islocked = newislocked;
		rating = newrating;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
