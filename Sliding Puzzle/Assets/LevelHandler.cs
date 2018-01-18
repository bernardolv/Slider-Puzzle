using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour {
	public static int efficientturns;
	static Dictionary<int, LevelStats> leveldic = new Dictionary<int, LevelStats>();
	static public int maxlevels;
	bool hasinitd;
	// Use this for initialization
	void Start () {
		
		/*if (PlayerPrefs.HasKey ("Loaded")) {
			Debug.Log ("Has playerpref");
		} else {*/
			//Load levels for the first time. Init values. only level 1 unlocked.
			Debug.Log ("Giving loaded");
			LevelStats lv1 = new LevelStats(1,2,false);
			LevelStats lv2 = new LevelStats(2,3,true); 

			leveldic.Add (1, lv1);
			leveldic.Add (2, lv2);
			PlayerPrefs.SetInt ("Loaded", 1);
		//}
		//PlayerPrefs.DeleteAll();



	}

	void Update () {
		if (efficientturns <= 0) {
			Lookfor (LevelManager.levelnum);
		}
	}
	public static void Lookfor(int levelnum){
		maxlevels = leveldic.Count ;
		Debug.Log ("maxcount" + maxlevels);
		if (levelnum <= maxlevels) {
			efficientturns = leveldic [levelnum].turns;
			Debug.Log ("The number of turns for level " + levelnum + " is " + efficientturns);
		} 
		else {
			Debug.Log ("No turn number stored" + levelnum + efficientturns);

		}
	}
}
