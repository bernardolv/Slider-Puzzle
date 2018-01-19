using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class FourbyFourLevels : MonoBehaviour {
	private string filePath;
	private string result;

	IEnumerator checkAndroid(string file)
	{
		filePath = System.IO.Path.Combine(Application.streamingAssetsPath, file);
		Debug.Log (filePath);
		result = " ";
		if (file.Contains ("://")) {
			UnityWebRequest www = UnityWebRequest.Get (file);
			yield return www.SendWebRequest ();
			result = www.downloadHandler.text;
			Debug.Log (result);

		} else
			result = System.IO.File.ReadAllText (filePath);
		Debug.Log (result);
		Debug.Log (filePath);
	}

	string[][] readFile(string file){
		string text = System.IO.File.ReadAllText (file);
		string[] lines = Regex.Split (text, "\n");
		int rows = lines.Length;
		string[][] levelBase = new string[rows][];
		for (int i = 0; i < lines.Length; i++) {
			string[] stringsOfLine = Regex.Split (lines [i], " ");
			levelBase [i] = stringsOfLine;
		}
		return levelBase;
	}
		public Transform player;
		public Transform floor_ice;
		public Transform floor_wall;
		public Transform floor_rock;
		public Transform floor_start;
		public Transform floor_goal;
		public Transform floor_hole;
		public Transform floor_wood;
		public Transform floor_left;
		public Transform floor_right;
		public Transform floor_up;
		public Transform floor_down;
		public Transform floor_fragile;
		public Transform floor_quicksand;
		public Transform floor_boss;

		public Transform s_floor_left;
		public Transform s_floor_right;
		public Transform s_floor_up;
		public Transform s_floor_down;
		public Transform s_floor_rock;


		
		public int levelnum;
		//public bool readytodraw;

		public const string sfloor_ice = "0";
		public const string sfloor_wall = "1";
		public const string sfloor_rock = "2";
		public const string sstart = "S";
		public const string sgoal = "G";
		public const string sfloor_hole = "H";
		public const string sfloor_wood = "#";
		public const string sfloor_left = "L";
		public const string sfloor_right = "R";
		public const string sfloor_up = "U";
		public const string sfloor_down = "D";
		public const string sfloor_fragile = "F";
		public const string sfloor_quicksand = "Q";
		public const string sfloor_boss = "B";

	public const string ssfloor_left = "l";
	public const string ssfloor_right = "r";
	public const string ssfloor_up = "u";
	public const string ssfloor_down = "d";
	public const string ssfloor_rock = "w";




		void Start() {
		LevelManager.levelselector = this;
		if (LevelManager.levelnum == null || LevelManager.levelnum == 0) {
			LevelManager.levelnum = 1;
		}
		levelnum = LevelManager.levelnum;
		LevelHandler.Lookfor (levelnum);
		DrawIce ();
		DrawNextLevel (levelnum);
		}


		/*if (LevelManager.readytodraw) {
			
			Debug.Log ("OW");
			DestroyAllExceptCamera ();
			DrawIce ();
			DrawNextLevel (levelnum);
			LevelManager.readytodraw = false;
		}*/
	//}
	public void DrawIce(){
		string filePath = System.IO.Path.Combine (Application.streamingAssetsPath, "8by8ice.txt");
		//checkAndroid ("8by8ice.txt");
		//Debug.Log (filePath);
		//string leveltext = ("Assets/Resources/8by8ice.txt");
		string[][] jagged = readFile (filePath);

		// create planes based on matrix
		for (int y = 0; y < jagged.Length; y++) {
			for (int x = 0; x < jagged [0].Length; x++) {
				switch (jagged [y] [x]) {
				case sfloor_ice:
					Instantiate (floor_ice, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				}
			}
		} 
	}

	public void DrawNextLevel(int levelnumber){

		string leveltext = ("Level" + levelnumber.ToString() + ".txt");
		string levelname = ("Level" + levelnumber.ToString ());
		checkAndroid (leveltext);
		string filePath = System.IO.Path.Combine (Application.streamingAssetsPath, leveltext);
		string[][] jagged = readFile (filePath);

		// create planes based on matrix
		for (int y = 0; y < jagged.Length; y++) {
			for (int x = 0; x < jagged [0].Length; x++) {
				switch (jagged [y] [x]) {
				case sstart:
					Instantiate (floor_start, new Vector3 (x, -y, 0), Quaternion.identity);
					Instantiate (player, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sgoal:
					Instantiate (floor_goal, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
			//	case sfloor_ice:
				//	Instantiate (floor_ice, new Vector3 (x, -y, 0), Quaternion.identity);
					//break;
				case sfloor_wall:
					Instantiate (floor_wall, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sfloor_hole:
					Instantiate (floor_hole, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sfloor_rock:
					Instantiate (floor_rock, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sfloor_wood:
					Instantiate (floor_wood, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sfloor_left:
					Instantiate	(floor_left, new Vector3 (x, -y, 0), floor_left.transform.rotation);
					break;
				case sfloor_right:
					Instantiate	(floor_right, new Vector3 (x, -y, 0), floor_right.transform.rotation);
					break;
				case sfloor_up:
					Instantiate	(floor_up, new Vector3 (x, -y, 0), floor_up.transform.rotation);
					break;
				case sfloor_down:
					Instantiate	(floor_down, new Vector3 (x, -y, 0), floor_down.transform.rotation);
					break;
				case sfloor_fragile:
					Instantiate (floor_fragile, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sfloor_quicksand:
					Instantiate (floor_quicksand, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sfloor_boss:
					Instantiate (floor_boss, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case ssfloor_left:
					Instantiate	(s_floor_left, new Vector3 (x, -y, 0), s_floor_left.transform.rotation);
					break;
				case ssfloor_right:
					Instantiate	(s_floor_right, new Vector3 (x, -y, 0), s_floor_right.transform.rotation);
					break;
				case ssfloor_up:
					Instantiate	(s_floor_up, new Vector3 (x, -y, 0), s_floor_up.transform.rotation);
					break;
				case ssfloor_down:
					Instantiate	(s_floor_down, new Vector3 (x, -y, 0), s_floor_down.transform.rotation);
					break;
				case ssfloor_rock:
					Instantiate (s_floor_rock, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				}

			}
		} 
	}
	public void DestroyAllExceptCamera(){
		TurnBehaviour.turn = 0;
		GameObject[] gameobjects = GameObject.FindObjectsOfType <GameObject>();

		foreach (GameObject component in gameobjects)
		{
			if (component.tag != "MainCamera" && component.tag != "Canvas") {
				Destroy (component);
				Debug.Log ("destroyed" + component);
			}

		}
	}
}