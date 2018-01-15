using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour {
	public List<LevelStats> levelstats = new List<LevelStats> ();
	// Use this for initialization
	void Start () {
	//	List<LevelStats> levelstats = new List<LevelStats> ();
		levelstats.Add (new LevelStats (0, 0, false));
		levelstats.Add (new LevelStats (1, 3, false));
		levelstats.Add (new LevelStats (2, 4, true));
		Debug.Log ("Ping");


	}
	
	// Update is called once per frame
	void Update () {
		foreach (LevelStats ls in levelstats) {
			Debug.Log (ls.levelnum);
		}
	}
}
