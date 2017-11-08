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

	public bool readytodraw;
	public int levelnum;

		public const string sfloor_ice = "0";
		public const string sfloor_wall = "1";
		public const string sfloor_rock = "2";
		public const string sstart = "S";
		public const string sgoal = "G";
		public const string sfloor_hole = "H";

		/*void Start() {
		string[][] jagged = readFile ("Assets/Resources/Level1.txt");

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
				case sfloor_ice:
					Instantiate (floor_ice, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sfloor_wall:
					Instantiate (floor_wall, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sfloor_hole:
					Instantiate (floor_hole, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sfloor_rock:
					Instantiate (floor_rock, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				}
			}
		}     
	}*/
	void Update(){
		if (readytodraw) {
			//BackToTheIce ();
			DrawNextLevel (levelnum);
			bool readytodraw = false;	
		}
	}
	void BackToTheIce(){
		//Destroy everything except Ice_tiles
	}
	void DrawNextLevel(int levelnumber){

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
				case sfloor_ice:
					Instantiate (floor_ice, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sfloor_wall:
					Instantiate (floor_wall, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sfloor_hole:
					Instantiate (floor_hole, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				case sfloor_rock:
					Instantiate (floor_rock, new Vector3 (x, -y, 0), Quaternion.identity);
					break;
				}
			}
		} 
		readytodraw = false;	

	}
}