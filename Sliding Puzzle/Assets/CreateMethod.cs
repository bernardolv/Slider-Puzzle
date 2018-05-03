using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMethod : MonoBehaviour {
	public static string [,] generatedmap = new string [8,8];
	public static int icenextcounter;
	public static List<Vector2> doorable = new List<Vector2>();
	// Use this for initialization
	void Start () {
		Createfourbyfour();
	}
	
	// Update is called once per frame
	void Update () {
		Createfourbyfour();
	}
	public void Createfourbyfour(){
		ResetAll();
		CreateIce();
		Add2Outerwalls();
		PopulateDoorPool();
		AssignGoalAndStart();
		//PrintMap();
		DrawMap();

	}
	public static void CreateIce(){
		for(int y = 0; y<8; y++){
			for(int x = 0; x<8; x++){
				generatedmap[x,y] = "Ice";
			}
		}
	}
	public static void Add2Outerwalls(){
		for(int i = 0; i<8; i++){
			int var;
			if(i<2 || i>5){
				var = 0;
			}
			else{
				var = 1;
			}
			switch(var){
				case 0:
					for(int j = 0; j<8; j++){
						generatedmap[j,i] = "Wall";
					}
					break;
				case 1:
					for(int j = 0; j<8; j++){
						if(j<2 || j>5){
							generatedmap[j,i] = "Wall";
						}
						else{
						}
					}
					break;
				default:
					break;
			}
		}
	}
	public static void PopulateDoorPool(){
		for(int i = 0; i<8; i++){
			for(int j = 0; j<8; j++){
				if(generatedmap[j,i] == "Wall"){
					CheckSides(j,i);
				}
			}
		}
	}
	public static void CheckSides(int x, int y){ //This checks if a tile is suitable for a Door
		icenextcounter = 0;
		GetWallTag(x-1,y);
		GetWallTag(x+1,y);
		GetWallTag(x,y+1);
		GetWallTag(x,y-1);
		if(icenextcounter>0){
			int myx = x;
			int myy = y;
			doorable.Add(new Vector2(myx,myy));
		}
	}
	public static void AssignGoalAndStart(){ //grabs the doopool and assigns start and goal
		int max = doorable.Count;
		int num = Random.Range(0,max-1);
		Vector2 Goal = doorable[num];
		generatedmap[Mathf.RoundToInt(Goal.x),Mathf.RoundToInt(Goal.y)] = "Goal";
		doorable.Remove(Goal);
		max = doorable.Count;
		num = Random.Range(1,max-1);
		Vector2 Start = doorable[num];
		generatedmap[Mathf.RoundToInt(Start.x), Mathf.RoundToInt(Start.y)] = "Start";
	}
	public static void GetWallTag(int x, int y){//currently only checks for ice (for doorables)
		if (x > 7 || x < 0){

		}
		else if(y>7 || y < 0){

		}
		else if (generatedmap[x,y] == "Ice"){
			icenextcounter++;
		}
		else{

		}
	}
	public static void PrintMap(){
		for(int i = 0; i<8; i++){
			for(int j = 0; j<8; j++){
				Debug.Log(generatedmap[j,i]);
			}
		}
	}
	public static void DrawMap(){
		for(int i = 0; i<8; i++){
			for(int j = 0; j<8; j++){
				PaintTag(j,i);
			}
		}
	}
	public static void PaintTag(int x, int y){
		string tag = generatedmap[x,y];
		int myx = x;
		int myy = -y;
		Vector3 tiletotest = new Vector3(myx, myy, 0);
		Collider2D[] colliders = Physics2D.OverlapCircleAll(tiletotest, .1f);
		if(colliders.Length == 0){

		}
		else{
			foreach(Collider2D component in colliders){
				if(tag == "Wall"){
					component.GetComponent<SpriteRenderer>().color = Color.black;
				}
				else if(tag == "Start"){
					component.GetComponent<SpriteRenderer>().color = Color.red;
				}
				else if(tag == "Goal"){
					component.GetComponent<SpriteRenderer>().color = Color.green;
				}
				else{

				}
			}
		}
	}
	public static void ResetColor(){
		GameObject[] icetiles;
		icetiles = GameObject.FindGameObjectsWithTag("Ground");
		foreach(GameObject icetile in icetiles ){
			icetile.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}
	public static void ResetAll(){
		ResetColor();
		doorable.Clear();
	}
}
