using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class CreateMethod : MonoBehaviour {
	public static string [,] generatedmap = new string [8,8];	//Used to build
	public string[,] themap = new string[8, 8]; 				//Used to store built map
	public static int icenextcounter;						
	public static List<Vector2> doorable = new List<Vector2>();
	public static List<Vector2> cleanice = new List<Vector2> ();
	public static List<string> piecetiles = new List<string>();
	public int extrawalls;
	public int lava;
	public int pedro;
	public int onmaptiles;
	public int piecenum;
	public SolveMethod Solver;
	public static bool hassolution;
	public static bool onestepsolution;
	public static int[,] mapvalues = new int[8,8];

	//For NN inputs
	public static int Goalx;
	public static int Goaly;
	public static int Startx;
	public static int Starty;
	public static bool cansolve; //using solvemethod on the generatedmap

	static private int endianDiff1;
	static private int endianDiff2;
	static private int idx;
	static private byte [] byteBlock;	

	enum ArrayType {Float, Int32, Bool, String, Vector2, Vector3, Quaternion, Color}

	// Use this for initialization
	void Start () {
		//Debug.Log(SolveMethod.solutions[0].x);
		ANNBrain.ann = new ANN(64, 10,2, 32, 1);
		for(int i=0; i<100000;i++){
			Createfourbyfour();
			if(6<SolveMethod.bestsol){
				DrawMap();
				break;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		//Createfourbyfour();
		if(Input.GetKeyDown(KeyCode.K)){
			FeedMap(themap);
			PrintNumMap(mapvalues);
		}
	}
	public void Createmoreandmore(){
		for(int i=0; i<1000;i++){
			Createfourbyfour();
		}

	}
	public void Createfourbyfour(){
		SolveMethod.solutions.Clear ();
		ResetAll();
		CreateIce();
		Add2Outerwalls();
		PopulateDoorPool();
		AssignGoalAndStart();
		AddOnMapTiles ();
		SolveMethod.startx = Startx;
		SolveMethod.starty = Starty;
		AddPieceTiles ();
		themap = generatedmap;
		Solver.TryPieces(themap);//Solve normal, adds annbrain.sol.

		//Solve with possible piecetiles
		//PrintMap();
		//DrawMap();
		FeedMap(themap);
		//PrintNumMap(mapvalues);
		//ANNBrain.Run(mapvalues, ANNBrain.sol, false);
		ANNBrain.RunV2(mapvalues,SolveMethod.bestsol, true );
//		Debug.Log("Fact is " + ANNBrain.bestsol + "AI says " +   )

		//Debug.Log("BestSolutions is " + SolveMethod.bestsol);
//		Debug.Log("Best turns " + ANNBrain.sol);
		//ANNBrain

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
		Goalx = Mathf.RoundToInt(Goal.x);
		Goaly = Mathf.RoundToInt(Goal.y);
		generatedmap[Mathf.RoundToInt(Goal.x),Mathf.RoundToInt(Goal.y)] = "Goal";
		doorable.Remove(Goal);
		max = doorable.Count;
		num = Random.Range(1,max-1);
		Vector2 Start = doorable[num];
		Startx = Mathf.RoundToInt(Start.x);		//NN
		Starty = Mathf.RoundToInt(Start.y);		//NN
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
					component.GetComponent<SpriteRenderer>().color = Color.yellow;
				}
				else if(tag == "Goal"){
					component.GetComponent<SpriteRenderer>().color = Color.green;
				}
				else if(tag == "Hole"){
					component.GetComponent<SpriteRenderer>().color = Color.red;
				}
				else{
					component.GetComponent<SpriteRenderer>().color = Color.white;
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
		//ResetColor(); //notnow
		doorable.Clear();
	}
	public void AddOnMapTiles(){
		PopulateCleanIce ();
		for (int i = 0; i < extrawalls; i++) {
			int max = cleanice.Count;
			int num = Random.Range (0, max - 1);
			Vector2 newwall = cleanice[num];
			generatedmap [Mathf.RoundToInt(newwall.x), Mathf.RoundToInt(newwall.y)] = "Wall";
			cleanice.Remove (newwall);
		}
		for (int i = 0; i < lava; i++) {
			int max = cleanice.Count;
			int num = Random.Range (0, max - 1);
			Vector2 newlava = cleanice[num];
			generatedmap [Mathf.RoundToInt(newlava.x), Mathf.RoundToInt(newlava.y)] = "Hole";
			cleanice.Remove (newlava);
		}
	}
	public static void PopulateCleanIce(){
		//Playerprefsx
		cleanice.Clear ();
		for(int i = 0; i<8; i++){
			for(int j = 0; j<8; j++){
				if(generatedmap[j,i] == "Ice"){
					cleanice.Add(new Vector2(j,i));
				}
			}
		}	
	}
	public void AddPieceTiles(){
		for (int i = 0; i < pedro; i++) {
			piecetiles.Add ("Wall");
		}
	}
	public static bool SetStringArray (string key, string[] stringArray){
		var bytes = new byte[stringArray.Length + 1];
		bytes[0] = System.Convert.ToByte (ArrayType.String);	// Identifier
		Initialize();
	 
		// Store the length of each string that's in stringArray, so we can extract the correct strings in GetStringArray
		for (var i = 0; i < stringArray.Length; i++)
		{
			if (stringArray[i] == null)
			{
				Debug.LogError ("Can't save null entries in the string array when setting " + key);
				return false;
			}
			if (stringArray[i].Length > 255)
			{
				Debug.LogError ("Strings cannot be longer than 255 characters when setting " + key);
				return false;
			}
			bytes[idx++] = (byte)stringArray[i].Length;
		}
	 
		try
		{
			PlayerPrefs.SetString (key, System.Convert.ToBase64String (bytes) + "|" + string.Join("", stringArray));
		}
		catch
		{
			return false;
		}
		return true;
	}
	private static void Initialize (){
		if (System.BitConverter.IsLittleEndian)
		{
			endianDiff1 = 0;
			endianDiff2 = 0;
		}
		else
		{
			endianDiff1 = 3;
			endianDiff2 = 1;
		}
		if (byteBlock == null)
		{
			byteBlock = new byte[4];
		}
		idx = 1;
	}
	public static string[] GetStringArray (string key)
	{
		if (PlayerPrefs.HasKey(key)) {
			var completeString = PlayerPrefs.GetString(key);
			var separatorIndex = completeString.IndexOf("|"[0]);
			if (separatorIndex < 4) {
				Debug.LogError ("Corrupt preference file for " + key);
				return new string[0];
			}
			var bytes = System.Convert.FromBase64String (completeString.Substring(0, separatorIndex));
			if ((ArrayType)bytes[0] != ArrayType.String) {
				Debug.LogError (key + " is not a string array");
				return new string[0];
			}
			Initialize();
	 
			var numberOfEntries = bytes.Length-1;
			var stringArray = new string[numberOfEntries];
			var stringIndex = separatorIndex + 1;
			for (var i = 0; i < numberOfEntries; i++)
			{
				int stringLength = bytes[idx++];
				if (stringIndex + stringLength > completeString.Length)
				{
					Debug.LogError ("Corrupt preference file for " + key);
					return new string[0];
				}
				stringArray[i] = completeString.Substring(stringIndex, stringLength);
				stringIndex += stringLength;
			}
	 
			return stringArray;
		}
		return new string[0];
	}
	public static void FeedMap(string[,] stringer){
		for(int i = 0; i<8; i++){
			for(int j = 0; j<8; j++){
				switch (stringer[j,i]){
				case "Ice":
					mapvalues[j,i] = 0;
					break;
				case "Wall":
					mapvalues[j,i] = 1;
					break;
				case "Start":
					mapvalues[j,i] = 2;
					break;
				case "Goal":
					mapvalues[j,i] = 3;
					break;
				case "Hole":
					mapvalues[j,i] = 4;
					break;
				}
			}
		}
	}
	public static void PrintNumMap(int[,] maper){
		for(int i = 0; i<8; i++){
			for(int j = 0; j<8; j++){
				Debug.Log(maper[j,i]);
			}
		}
	}
	//public void
}
