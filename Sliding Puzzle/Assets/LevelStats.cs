using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats {
	public int levelnum;
	public int turns;
	public bool islocked;

	public LevelStats (int newlevelnum, int newturns, bool newislocked){
		levelnum = newlevelnum;
		turns = newturns;
		islocked = newislocked;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
