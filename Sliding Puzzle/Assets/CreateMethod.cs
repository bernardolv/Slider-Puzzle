using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
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
	public static double[,] mapvalues = new double[8,8];

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

	public static List<string> mapsdata = new List<string>();
	StreamWriter mdf;
	public static string md;

	bool trainingDone = false;
	float trainingProgress = 0;
	double sse = 0;
	double lastSSE = 1;

	public static float epochs = 1000;

	public static int inputnum = 64;
	int[] myclassifier;
	public static bool waslasttrue;
	bool haschanged = false;
	string currentWeights;
	public static bool loadFromFile = false;
	public static double lowestSSE; 
	public static string[,] bitmap = new string[8,8];
	enum ArrayType {Float, Int32, Bool, String, Vector2, Vector3, Quaternion, Color}
	public static string mapstring;
	// Use this for initialization
	void Start () {
		string path = Application.dataPath + "/mapsData.txt";
		mdf = File.CreateText(path);
		//Debug.Log(SolveMethod.solutions[0].x);

		ANNBrain.ann = new ANN(64, 1,1,44,.01);
		SolveMethod.classifier = new int[] {0};
		if(loadFromFile){
			LoadWeightsFromFile();
			trainingDone=true;
			StartCoroutine(TestTrainingSet());
		}
		else{
			StartCoroutine(LoadTrainingSet());			
		}
		waslasttrue = false;
		////ANNBrain.InitializeClassifiers(1);
		//StartCoroutine(LoadTrainingSet());
		//StartCoroutine(TestTrainingSet());
		//GenerateAndDraw(10000);
		//AddMaps();

	}
	void GenerateAndDraw(int num){
		for(int i=0; i<num;i++){
			Createfourbyfour();
		//	DrawMap();
			/*if(8<SolveMethod.bestsol){
				DrawMap();
				break;
			}*/
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

	/*void AddMaps(){
		foreach(string md in mapsdata){
			if(mapsdata.Contains(md)){
			mapsdata.Add(md);)
			mdf.WriteLine(md);
		}
		mdf.Close();
	}*/

	void OnApplicationQuit(){
		foreach(string md in mapsdata){
			mdf.WriteLine(md);
		}
		mdf.Close();
	}

	void LoadWeightsFromFile(){
		string path = Application.dataPath + "/weights.txt";
		StreamReader wf= File.OpenText(path);
		if(File.Exists(path)){
			string line = wf.ReadLine();
			ANNBrain.ann.LoadWeights(line);
		}
	}

	IEnumerator LoadTrainingSet(){
		lowestSSE = 1;
		lastSSE =.999;
		//haschanged = true;
		string path = Application.dataPath + "/manyData.txt";
		string line;
		Debug.Log(File.Exists(path));
		if(File.Exists(path)){
			int lineCount = File.ReadAllLines(path).Length;
			StreamReader mdf = File.OpenText(path);
			List<double>calcOutputs = new List<double>();
			List<double>inputs = new List<double>();
			List<double>outputs = new List<double>();

			for(int i = 0; i<epochs; i++){

//				Debug.Log(i);
				//set file pointer to beginning of file
				sse = 0;
				mdf.BaseStream.Position = 0;
				//if(haschanged){
					currentWeights = ANNBrain.ann.PrintWeights();

				//}
				while((line = mdf.ReadLine()) != null){
					string[]data = line.Split(',');
					//if nothing to be learned ignore this line
					double thisError = 0;
					inputs.Clear();
					outputs.Clear();
					for(int j = 0; j<inputnum; j++){
						inputs.Add(System.Convert.ToDouble(data[j]));
					}
					//myclassifier = SolveMethod.classifier;
					//Debug.Log(myclassifier.Length);
					for(int j = 0; j<1; j++){
						outputs.Add(System.Convert.ToDouble(data[j+inputnum]));
					}
//					Debug.Log(outputs.Count);
					calcOutputs = ANNBrain.ann.Train(inputs,outputs);
					thisError = CalculateError(outputs, calcOutputs);
//					Debug.Log(thisError);
					sse += thisError;
				}		
				trainingProgress = (float)i/(float)epochs;
				sse /= lineCount;
				//Debug.Log ()
				//if(!haschanged){
					if(sse<lastSSE){
						if(ANNBrain.ann.alpha<.001){
							ANNBrain.ann.alpha = Mathf.Clamp((float)ANNBrain.ann.alpha +.0001f, .00001f,.9f);
							lastSSE = sse;
						}
						else{
							//ANNBrain.ann.alpha = Mathf.Clamp((float)ANNBrain.ann.alpha +.0001f, .00001f,1.5f);
							lastSSE = sse;

						}
					}
					else{
						if(ANNBrain.ann.alpha<.001){

						ANNBrain.ann.LoadWeights(currentWeights);
						ANNBrain.ann.alpha = Mathf.Clamp((float)ANNBrain.ann.alpha - .0001f, .00001f,.9f);							
						}
						else{

						ANNBrain.ann.LoadWeights(currentWeights);
						ANNBrain.ann.alpha = Mathf.Clamp((float)ANNBrain.ann.alpha - (float)(.9*ANNBrain.ann.alpha), .00001f,.9f);
						}
						//haschanged =true;	
						//if()					
					}

					//NNBrain.ann.alpha = Mathf.Clamp((float)ANNBrain.ann.alpha + .0003f, .0001f,.9f);
				//}
				//lastSSE = sse;
				if(haschanged){
					if(lastSSE<sse){
						ANNBrain.ann.LoadWeights(currentWeights);
						ANNBrain.ann.alpha = Mathf.Clamp((float)ANNBrain.ann.alpha - .00001f, .00001f,.9f);
						if(lastSSE<lowestSSE){
							lowestSSE=lastSSE;
						}
						//lastSSE=sse; 
					}
					else{
						if(ANNBrain.ann.alpha<.001){
							ANNBrain.ann.alpha = Mathf.Clamp((float)ANNBrain.ann.alpha +.0001f, .00001f,.9f);
							lastSSE=sse;
						}
						else{

						//ANNBrain.ann.alpha = Mathf.Clamp((float)ANNBrain.ann.alpha - .00001f, .00001f,.9f);
						ANNBrain.ann.alpha = Mathf.Clamp((float)ANNBrain.ann.alpha -.0001f,.00001f,.9f);
						lastSSE=sse; 
						}
					}
				}
				//if(!haschanged){

				//	ANNBrain.ann.alpha = Mathf.Clamp((float)ANNBrain.ann.alpha - .0001f, .0001f,.9f);
				//}
				if(lastSSE<.05 && !haschanged){
					//ANNBrain.ann.alpha = .007f;
					//haschanged = true;
				}
				if(ANNBrain.ann.alpha<.00002f){
							//yield return null;
					break;
						}
				yield return null;
			}
		}
		Debug.Log("Training Done");
		//trainingDone = true;
		ANNBrain.ann.SaveWeights();
		trainingDone = true;
		StartCoroutine(TestTrainingSet());
	}
	IEnumerator TestTrainingSet(){
		string path = Application.dataPath + "/testData.txt";
		string line;
		if(File.Exists(path)){
			int lineCount = File.ReadAllLines(path).Length;
			StreamReader mdf = File.OpenText(path);
			List<double>calcOutputs = new List<double>();
			List<double>inputs = new List<double>();
			List<double>outputs = new List<double>();

			for(int i = 0; i<1; i++){
				//set file pointer to beginning of file
				sse = 0;
				mdf.BaseStream.Position = 0;
				//string currentWeights = ANNBrain.ann.PrintWeights();
				while((line = mdf.ReadLine()) != null){
					string[]data = line.Split(',');
					//if nothing to be learned ignore this line
					double thisError = 0;
					inputs.Clear();
					outputs.Clear();
					for(int j = 0; j<inputnum; j++){
						inputs.Add(System.Convert.ToDouble(data[j]));
					}
					//myclassifier = SolveMethod.classifier;
					//Debug.Log(myclassifier.Length);
					for(int j = 0; j<1; j++){
						outputs.Add(System.Convert.ToDouble(data[j+inputnum]));
					}
//					Debug.Log(outputs.Count);
					calcOutputs = ANNBrain.ann.CalcOutput(inputs,outputs);
					//thisError = CalculateError(outputs, calcOutputs);
//					Debug.Log(thisError);
					//sse += thisError;
					Debug.Log(Mathf.RoundToInt((float)outputs[0]) +"" + Mathf.RoundToInt((float)calcOutputs[0]));
				}		
				//trainingProgress = (float)i/(float)epochs;
				//sse /= lineCount;
				//Debug.Log ()
				//lastSSE = sse;
				/*if(lastSSE<sse){
					ANNBrain.ann.LoadWeights(currentWeights);
					ANNBrain.ann.alpha = Mathf.Clamp((float)ANNBrain.ann.alpha - .001f, .01f,.9f);
				}
				else{
					ANNBrain.ann.alpha = Mathf.Clamp((float)ANNBrain.ann.alpha +.00f,.01f,.9f);
					lastSSE=sse; 
				}*/
				yield return null;
			}
		}
		Debug.Log("Test Done");
		//trainingDone = true;
	}

	void Debugger(){

	}

	void OnGUI(){
		//guiStyle.fontSize = 40;
		GUIStyle style = new GUIStyle();
		style.fontSize = 40;
		GUI.Label(new Rect(25,25,250,30), "SSE: " +lastSSE, style);
		GUI.Label(new Rect(25,75,250,30), "Alpha: "+ ANNBrain.ann.alpha,style);
		GUI.Label(new Rect(25,125,250,30), "Trained: " +trainingProgress,style);
		GUI.Label(new Rect(25,175,250,30), "Lowest SSE: " +lowestSSE,style);
	}
	double CalculateError(List <double> Results, List<double> CalculatedResults){
		double curerror = 0;
		for(int i=0; i<SolveMethod.classifier.Length;i++){
		curerror = curerror + Mathf.Pow((float)(Results[i] - CalculatedResults[i]),2);
		}
		curerror = curerror/2.0;
		/*double altcurerror = (Mathf.Pow((float)(Results[0]-CalculatedResults[0]),2)
			+ Mathf.Pow((float)(Results[1]-CalculatedResults[1]),2) + Mathf.Pow((float)(Results[2]-CalculatedResults[2]),2) 
			+ Mathf.Pow((float)(Results[3]-CalculatedResults[3]),2) + Mathf.Pow((float)(Results[4]-CalculatedResults[4]),2) 
			+ Mathf.Pow((float)(Results[5]-CalculatedResults[5]),2) + 
			Mathf.Pow((float)(Results[6]-CalculatedResults[6]),2)
			 + Mathf.Pow((float)(Results[7]-CalculatedResults[7]),2))/2.0f;*/
		//Debug.Log(curerror);
		//Debug.Log(altcurerror); 
			 if(double.IsNaN(curerror)){
			 	curerror = 0;
			 }
		return curerror;
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
		DrawMap();
		FeedMap(themap);
		//PrintNumMap(mapvalues);
		//ANNBrain.Run(mapvalues, ANNBrain.sol, false);
		ANNBrain.RunV3(mapvalues,SolveMethod.bestsol, true );
		//CreateDataLine();
		//if(ANNBrain.sol == 1){
		//if((!waslasttrue && ANNBrain.sol ==1)||(waslasttrue && ANNBrain.sol ==0)){

		CreateDataLineV2();
		//waslasttrue = !waslasttrue;
		//}
		//}
//		Debug.Log("Fact is " + ANNBrain.bestsol + "AI says " +   )

		//Debug.Log("BestSolutions is " + SolveMethod.bestsol);
//		Debug.Log("Best turns " + ANNBrain.sol);
		//ANNBrain

	}
	public static void CreateDataLine(){
		string md = "";
		for(int y = 0; y<8; y++){
			for(int x = 0; x<8; x++){
				md = md + mapvalues[x,y].ToString() + ",";
			}
		}

		for(int y = 0; y<SolveMethod.classifier.Length;y++){
			md = md + SolveMethod.classifier[y].ToString() + ",";
		}
		md = md.Substring(0, md.Length - 1);
		Debug.Log(md);
		mapsdata.Add(md);

	}
	public static void CreateDataLineV2(){
		md = md + ",";
		for(int y = 0; y<SolveMethod.classifier.Length;y++){
			md = md + SolveMethod.classifier[y].ToString() + ",";
		}
		md = md.Substring(0, md.Length - 1);
		Debug.Log(md);
		if(!mapsdata.Contains(md)){
			mapsdata.Add(md);
			waslasttrue = !waslasttrue;
		}
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
//			Debug.Log("doorable" + myx + " " + myy); 
		}
	}
	public static void AssignGoalAndStart(){ //grabs the doopool and assigns start and goal
		int max = doorable.Count;
		int num = Random.Range(0,max);
		Debug.Log(num);
		Debug.Log(max);
		//Debug.Log(doorable[15]);
		Vector2 Goal = doorable[num];
		Goalx = Mathf.RoundToInt(Goal.x);
		Goaly = Mathf.RoundToInt(Goal.y);
		generatedmap[Mathf.RoundToInt(Goal.x),Mathf.RoundToInt(Goal.y)] = "Goal";
		doorable.Remove(Goal);
		max = doorable.Count;
		num = Random.Range(0,max);
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
			int num = Random.Range (0, max);
			Vector2 newwall = cleanice[num];
			generatedmap [Mathf.RoundToInt(newwall.x), Mathf.RoundToInt(newwall.y)] = "Wall";
			cleanice.Remove (newwall);
		}
		for (int i = 0; i < lava; i++) {
			int max = cleanice.Count;
			int num = Random.Range (0, max);
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
	public static void FeedMapV2(string[,] stringer){
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
	public static void BinaryCategorize(){

	}
	public static void PrintNumMap(double[,] maper){
		for(int i = 0; i<8; i++){
			for(int j = 0; j<8; j++){
				Debug.Log(maper[j,i]);
			}
		}
	}
	//public void
}
