using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour {
	public static int efficientturns;
	public static Dictionary<int, LevelStats> leveldic = new Dictionary<int, LevelStats>();
	static public int maxlevels;
	bool hasinitd;
	private static LevelHandler instance = null;
	int reset;

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
/*		reset = PlayerPrefs.GetInt ("Reset");
		if (reset == 0) {
			PlayerPrefs.DeleteAll ();
			PlayerPrefs.SetInt ("Reset", 1);
		}*/
		/*if (PlayerPrefs.HasKey ("Loaded")) {
			Debug.Log ("Has playerpref");
		} else {*/
			//Load levels for the first time. Init values. only level 1 unlocked.

			Debug.Log ("Giving loaded");
			LevelStats lv1 = new LevelStats(1,2,false,0);
			LevelStats lv2 = new LevelStats(2,3,true,0); 
			LevelStats lv3 = new LevelStats(3,3,true,0); 
			LevelStats lv4 = new LevelStats(4,4,true,0); 
			LevelStats lv5 = new LevelStats(5,2,true,0); 
			LevelStats lv6 = new LevelStats(6,4,true,0); 
			LevelStats lv7 = new LevelStats(7,4,true,0); 
			LevelStats lv8 = new LevelStats(8,3,true,0); 
			LevelStats lv9 = new LevelStats(9,4,true,0); 
			LevelStats lv10 = new LevelStats(10,3,true,0); 
			LevelStats lv11 = new LevelStats(11,5,true,0); 
			LevelStats lv12 = new LevelStats(12,5,true,0); 
			LevelStats lv13 = new LevelStats(13,4,true,0); 
			LevelStats lv14 = new LevelStats(14,4,true,0); 
			LevelStats lv15 = new LevelStats(15,3,true,0); 
			LevelStats lv16 = new LevelStats(16,4,true,0); 
			LevelStats lv17 = new LevelStats(17,5,true,0); 
			LevelStats lv18 = new LevelStats(18,3,true,0); 
			LevelStats lv19 = new LevelStats(19,4,true,0); 
			LevelStats lv20 = new LevelStats(20,5,true,0); 
			LevelStats lv21 = new LevelStats(21,5,true,0); 
			LevelStats lv22 = new LevelStats(22,4,true,0); 
			LevelStats lv23 = new LevelStats(23,3,true,0); 
			LevelStats lv24 = new LevelStats(24,5,true,0); 
			LevelStats lv25 = new LevelStats(25,6,true,0); 
			LevelStats lv26 = new LevelStats(26,7,true,0); 
			LevelStats lv27 = new LevelStats(27,7,true,0); 
			LevelStats lv28 = new LevelStats(28,5,true,0); 
			LevelStats lv29 = new LevelStats(29,8,true,0); 
			LevelStats lv30 = new LevelStats(30,5,true,0); 
			LevelStats lv31 = new LevelStats(31,5,true,0); 
			LevelStats lv32 = new LevelStats(32,1,true,0); 
			LevelStats lv33 = new LevelStats(33,3,true,0); 
			LevelStats lv34 = new LevelStats(34,2,true,0); 
			LevelStats lv35 = new LevelStats(35,4,true,0); 
			LevelStats lv36 = new LevelStats(36,2,true,0); 
			LevelStats lv37 = new LevelStats(37,5,true,0); 
			LevelStats lv38 = new LevelStats(38,5,true,0); 
			LevelStats lv39 = new LevelStats(39,7,true,0); 
			LevelStats lv40 = new LevelStats(40,5,true,0); 
			LevelStats lv41 = new LevelStats(41,3,true,0); 
			LevelStats lv42 = new LevelStats(42,4,true,0); 
			LevelStats lv43 = new LevelStats(43,5,true,0); 
			LevelStats lv44 = new LevelStats(44,6,true,0); 
			LevelStats lv45 = new LevelStats(45,4,true,0); 
			LevelStats lv46 = new LevelStats(46,3,true,0); 
			LevelStats lv47 = new LevelStats(47,3,true,0); 
			LevelStats lv48 = new LevelStats(48,3,true,0); 
			LevelStats lv49 = new LevelStats(49,4,true,0); 
			LevelStats lv50 = new LevelStats(50,7,true,0); 
			LevelStats lv51 = new LevelStats(51,5,true,0); 
			LevelStats lv52 = new LevelStats(52,4,true,0); 
			LevelStats lv53 = new LevelStats(53,5,true,0); 
			LevelStats lv54 = new LevelStats(54,5,true,0); 
			LevelStats lv55 = new LevelStats(55,4,true,0);  //save 1
			LevelStats lv56 = new LevelStats(56,5,true,0); 
			LevelStats lv57 = new LevelStats(57,7,true,0); 
			LevelStats lv58 = new LevelStats(58,7,true,0); 
			LevelStats lv59 = new LevelStats(59,4,true,0); 
			LevelStats lv60 = new LevelStats(60,6,true,0); //swap with 62
			LevelStats lv61 = new LevelStats(61,8,true,0); 
			LevelStats lv62 = new LevelStats(62,9,true,0); 
			LevelStats lv63 = new LevelStats(63,8,true,0); 
			LevelStats lv64 = new LevelStats(64,7,true,0); 
			LevelStats lv65 = new LevelStats(65,7,true,0); 
			LevelStats lv66 = new LevelStats(66,7,true,0); 
			LevelStats lv67 = new LevelStats(67,1,true,0); 
			LevelStats lv68 = new LevelStats(68,7,true,0); 
			LevelStats lv69 = new LevelStats(69,4,true,0); 
			LevelStats lv70 = new LevelStats(70,4,true,0); 
			LevelStats lv71 = new LevelStats(71,6,true,0); 
			LevelStats lv72 = new LevelStats(72,6,true,0); 
			LevelStats lv73 = new LevelStats(73,5,true,0); 
			LevelStats lv74 = new LevelStats(74,6,true,0); 
			LevelStats lv75 = new LevelStats(75,3,true,0); 
			LevelStats lv76 = new LevelStats(76,7,true,0); 
			LevelStats lv77 = new LevelStats(77,7,true,0); 
			LevelStats lv78 = new LevelStats(78,2,true,0); 
			LevelStats lv79 = new LevelStats(79,7,true,0); 
			LevelStats lv80 = new LevelStats(80,5,true,0); 
			LevelStats lv81 = new LevelStats(81,6,true,0); 
			LevelStats lv82 = new LevelStats(82,5,true,0); 
			LevelStats lv83 = new LevelStats(83,5,true,0); 
			LevelStats lv84 = new LevelStats(84,6,true,0); 
			LevelStats lv85 = new LevelStats(85,4,true,0); 
			LevelStats lv86 = new LevelStats(86,4,true,0); //extra
			LevelStats lv87 = new LevelStats(87,4,true,0); 
			LevelStats lv88 = new LevelStats(88,7,true,0); 
			LevelStats lv89 = new LevelStats(89,3,true,0); 
			LevelStats lv90 = new LevelStats(90,5,true,0); 
			LevelStats lv91 = new LevelStats(91,1,true,0); 
			LevelStats lv92 = new LevelStats(92,5,true,0); 
			LevelStats lv93 = new LevelStats(93,5,true,0); 
			LevelStats lv94 = new LevelStats(94,5,true,0); 
			LevelStats lv95 = new LevelStats(95,5,true,0); 
			LevelStats lv96 = new LevelStats(96,5,true,0); 
			LevelStats lv97 = new LevelStats(97,5,true,0); 
			LevelStats lv98 = new LevelStats(98,5,true,0); 
			LevelStats lv99 = new LevelStats(99,5,true,0); 
			LevelStats lv100 = new LevelStats(100,5,true,0); 
	
				

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
			leveldic.Add (41, lv41);
			leveldic.Add (42, lv42);
			leveldic.Add (43, lv43);
			leveldic.Add (44, lv44);
			leveldic.Add (45, lv45);
			leveldic.Add (46, lv46);
			leveldic.Add (47, lv47);
			leveldic.Add (48, lv48);
			leveldic.Add (49, lv49);
			leveldic.Add (50, lv50);
			leveldic.Add (51, lv51);
			leveldic.Add (52, lv52);
			leveldic.Add (53, lv53);
			leveldic.Add (54, lv54);
			leveldic.Add (55, lv55);
			leveldic.Add (56, lv56);
			leveldic.Add (57, lv57);
			leveldic.Add (58, lv58);
			leveldic.Add (59, lv59);
			leveldic.Add (60, lv60);
			leveldic.Add (61, lv61);
			leveldic.Add (62, lv62);
			leveldic.Add (63, lv63);
			leveldic.Add (64, lv64);
			leveldic.Add (65, lv65);
			leveldic.Add (66, lv66);
			leveldic.Add (67, lv67);
			leveldic.Add (68, lv68);
			leveldic.Add (69, lv69);
			leveldic.Add (70, lv70);
			leveldic.Add (71, lv71);
			leveldic.Add (72, lv72);
			leveldic.Add (73, lv73);
			leveldic.Add (74, lv74);
			leveldic.Add (75, lv75);
			leveldic.Add (76, lv76);
			leveldic.Add (77, lv77);
			leveldic.Add (78, lv78);
			leveldic.Add (79, lv79);
			leveldic.Add (80, lv80);
			leveldic.Add (81, lv81);
			leveldic.Add (82, lv82);
			leveldic.Add (83, lv83);
			leveldic.Add (84, lv84);
			leveldic.Add (85, lv85);
			leveldic.Add (86, lv86);
			leveldic.Add (87, lv87);
			leveldic.Add (88, lv88);
			leveldic.Add (89, lv89);
			leveldic.Add (90, lv90);
			leveldic.Add (91, lv91);
			leveldic.Add (92, lv92);
			leveldic.Add (93, lv93);
			leveldic.Add (94, lv94);
			leveldic.Add (95, lv95);
			leveldic.Add (96, lv96);
			leveldic.Add (97, lv97);
			leveldic.Add (98, lv98);
			leveldic.Add (99, lv99);
			leveldic.Add (100, lv100);









			
			//PlayerPrefs.SetInt ("Loaded", 1);
		//}
//		PlayerPrefs.DeleteAll();



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
		int r = RatingBehaviour.rating;
		LevelStats newvalue = new LevelStats(x,y,z,r); 
		leveldic [x] = newvalue;
	}

	public static void LockLevel(int levelnum){
		int x = leveldic [levelnum].levelnum;
		int y = leveldic [levelnum].turns;
		bool z = true;
		int r = RatingBehaviour.rating;
		LevelStats newvalue = new LevelStats(x,y,z,r); 
		leveldic [x] = newvalue;
		string mystring = "Level"+levelnum+"Rating";
		PlayerPrefs.SetInt (mystring, 0);
		Debug.Log (levelnum);

	}

	public static void UnlockAllLevels(){
		for (int i = 1; i < leveldic.Count+1 ; i++){
			UnlockLevel (i);
		}
	}

	public static void LockAllLevels(){
		string mystring = "Level1Rating";
		PlayerPrefs.SetInt (mystring, 0);
		for (int i = 2; i < leveldic.Count ; i++){
			LockLevel (i);
		}
	}
}
