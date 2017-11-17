using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class FourbyFourLevels : MonoBehaviour {
	

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
		
		public int levelnum;
		//public bool readytodraw;

		public const string sfloor_ice = "0";
		public const string sfloor_wall = "1";
		public const string sfloor_rock = "2";
		public const string sstart = "S";
		public const string sgoal = "G";
		public const string sfloor_hole = "H";
		public const string sfloor_wood = "#";


		void Start() {
		LevelManager.levelselector = this;
		if (LevelManager.levelnum == null || LevelManager.levelnum == 0) {
			LevelManager.levelnum = 1;
		}
		levelnum = LevelManager.levelnum;
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
		string leveltext = ("Assets/Resources/8by8ice.txt");
		string[][] jagged = readFile (leveltext);

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

		string leveltext = ("Assets/Resources/Level" + levelnumber.ToString() + ".txt");
		string[][] jagged = readFile (leveltext);

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
				}
			}
		} 
	}
	public void DestroyAllExceptCamera(){
		GameObject[] gameobjects = GameObject.FindObjectsOfType <GameObject>();

		foreach (GameObject component in gameobjects)
		{
			if (component.tag != "MainCamera" && component.tag != "Canvas") {
				Destroy (component);
			}

		}
	}
}