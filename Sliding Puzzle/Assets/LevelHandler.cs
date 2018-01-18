using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour {
	public static int efficientturns;
	public static Dictionary<int, LevelStats> leveldic = new Dictionary<int, LevelStats>();
	static public int maxlevels;
	bool hasinitd;
	private static LevelHandler instance = null;


	// Use this for initialization
	void Awake(){
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
			return;
		}
		Destroy(this.gameObject);
	}
	// Use this for initialization
	void Start () {
		
		/*if (PlayerPrefs.HasKey ("Loaded")) {
			Debug.Log ("Has playerpref");
		} else {*/
			//Load levels for the first time. Init values. only level 1 unlocked.
			Debug.Log ("Giving loaded");
			LevelStats lv1 = new LevelStats(1,2,false);
			LevelStats lv2 = new LevelStats(2,3,true); 
			LevelStats lv3 = new LevelStats(3,3,true); 
			LevelStats lv4 = new LevelStats(4,4,true); 
			LevelStats lv5 = new LevelStats(5,2,true); 
			LevelStats lv6 = new LevelStats(6,4,true); 
			LevelStats lv7 = new LevelStats(7,4,true); 
			LevelStats lv8 = new LevelStats(8,3,true); 
			LevelStats lv9 = new LevelStats(9,4,true); 
			LevelStats lv10 = new LevelStats(10,3,true); 
			LevelStats lv11 = new LevelStats(11,5,true); 
			LevelStats lv12 = new LevelStats(12,5,true); 
			LevelStats lv13 = new LevelStats(13,4,true); 
			LevelStats lv14 = new LevelStats(14,4,true); 
			LevelStats lv15 = new LevelStats(15,3,true); 
			LevelStats lv16 = new LevelStats(16,4,true); 
			LevelStats lv17 = new LevelStats(17,5,true); 
			LevelStats lv18 = new LevelStats(18,3,true); 
			LevelStats lv19 = new LevelStats(19,4,true); 
			LevelStats lv20 = new LevelStats(20,5,true); 
			LevelStats lv21 = new LevelStats(21,5,true); 
			LevelStats lv22 = new LevelStats(22,4,true); 
			LevelStats lv23 = new LevelStats(23,3,true); 
			LevelStats lv24 = new LevelStats(24,5,true); 
			LevelStats lv25 = new LevelStats(25,6,true); 
			LevelStats lv26 = new LevelStats(26,7,true); 
			LevelStats lv27 = new LevelStats(27,7,true); 
			LevelStats lv28 = new LevelStats(28,5,true); 
			LevelStats lv29 = new LevelStats(29,8,true); 
			LevelStats lv30 = new LevelStats(30,5,true); 
			LevelStats lv31 = new LevelStats(31,5,true); 
			LevelStats lv32 = new LevelStats(32,1,true); 
			LevelStats lv33 = new LevelStats(33,3,true); 
			LevelStats lv34 = new LevelStats(34,2,true); 
			LevelStats lv35 = new LevelStats(35,4,true); 
			LevelStats lv36 = new LevelStats(36,3,true); 
			LevelStats lv37 = new LevelStats(37,5,true); 
			LevelStats lv38 = new LevelStats(38,5,true); 
			LevelStats lv39 = new LevelStats(39,7,true); 
			LevelStats lv40 = new LevelStats(40,5,true); 

				

			leveldic.Add (1, lv1);
			leveldic.Add (2, lv2);
			leveldic.Add (3, lv3);
			leveldic.Add (4, lv4);
			leveldic.Add (5, lv5);
			leveldic.Add (6, lv6);
			leveldic.Add (7, lv7);
			leveldic.Add (8, lv8);
			leveldic.Add (9, lv9);
			leveldic.Add (10, lv10);
			leveldic.Add (11, lv11);
			leveldic.Add (12, lv12);
			leveldic.Add (13, lv13);
			leveldic.Add (14, lv14);
			leveldic.Add (15, lv15);
			leveldic.Add (16, lv16);
			leveldic.Add (17, lv17);
			leveldic.Add (18, lv18);
			leveldic.Add (19, lv19);
			leveldic.Add (20, lv20);
			leveldic.Add (21, lv21);
			leveldic.Add (22, lv22);
			leveldic.Add (23, lv23);
			leveldic.Add (24, lv24);
			leveldic.Add (25, lv25);
			leveldic.Add (26, lv26);
			leveldic.Add (27, lv27);
			leveldic.Add (28, lv28);
			leveldic.Add (29, lv29);
			leveldic.Add (30, lv30);
			leveldic.Add (31, lv31);
			leveldic.Add (32, lv32);
			leveldic.Add (33, lv33);
			leveldic.Add (34, lv34);
			leveldic.Add (35, lv35);
			leveldic.Add (36, lv36);
			leveldic.Add (37, lv37);
			leveldic.Add (38, lv38);
			leveldic.Add (39, lv39);
			leveldic.Add (40, lv40);

			
			PlayerPrefs.SetInt ("Loaded", 1);
		//}
		//PlayerPrefs.DeleteAll();



	}

	/*void Update () {
		if (efficientturns <= 0) {
			Lookfor (LevelManager.levelnum);
		}
	}*/
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
	public static void UnlockLevel(int levelnum){
		int x = leveldic [levelnum].levelnum;
		int y = leveldic [levelnum].turns;
		bool z = false;
		LevelStats newvalue = new LevelStats(x,y,z); 
		leveldic [x] = newvalue;
	}
}
