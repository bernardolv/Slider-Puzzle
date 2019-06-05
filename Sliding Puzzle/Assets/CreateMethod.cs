using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Replay{
	public List<double> states;
	public double reward;
	public int action;

	public Replay(List<double> newlist,int newaction, double newreward){
		states = newlist;
		action = newaction;
		reward = newreward;
	}
}

public class CreateMethod : MonoBehaviour {
	public static Vector2 goalpos;
	public static string [,] generatedmap;	//Used to build
	public string[,] themap; 				//Used to store built map
	public string[,] testmap;
	public static string[,] bitmap;
	public static double[,] mapvalues;
	public static int icenextcounter;						
	public static List<Vector2> doorable = new List<Vector2>();
	public static List<Vector2> cleanice = new List<Vector2> ();
	public static List<Vector2> temporarilyoutice = new List<Vector2>();
	public static List<string> piecetiles = new List<string>();
	public int extrawalls;
	public int lava;
	public int pedro;
	public int onmaptiles;
	public int piecenum;
	public SolveMethod Solver;
	public static bool hassolution;
	public static bool onestepsolution;
	public int numberofwantedmaps;
	public List<double>inputs = new List<double>();
	public List <double> qs = new List<double>();
	public static string curstring;
	public int lavamax;
	public int fragilemax;
	public int woodmax;
	public int wallmax;
	public int stringcounter;
	public int icedimensions;
	public int totaldimensions;
	public static int mapdimension;
	public static int wallsleft;
	public static int wallsright;
	public static int wallsup;
	public static int wallsdown;
	//For NN inputs
	public static string nextpiece;
	public static int Goalx;
	public static int Goaly;
	public static int Startx;
	public static int Starty;
	public static bool cansolve; //using solvemethod on the generatedmap

	static private int endianDiff1;
	static private int endianDiff2;
	static private int idx;
	static private byte [] byteBlock;	
	public static bool isicetaken;
	public List<Vector2> possibleIce = new List<Vector2>();
	public List<Vector2> currentIce = new List<Vector2>();

	public static List<string> mapsdata = new List<string>();
	StreamWriter mdf;
	StreamWriter gmdf;
	public static string md;
	public static List<string> goodmapsdata = new List<string>();

	bool trainingDone = false;
	float trainingProgress = 0;
	double sse = 0;
	double lastSSE = 1;

	public static float epochs = 600;

	public static int inputnum = 64;
	int[] myclassifier;
	public static bool waslasttrue;
	bool haschanged = false;
	string currentWeights;
	public static bool loadFromFile = false;
	public static double lowestSSE; 
	enum ArrayType {Float, Int32, Bool, String, Vector2, Vector3, Quaternion, Color}
	public static string mapstring;
	public string monster1;
	public string monster2;
	public string monster3;
	public string monster4;
	public static int startingturns;
	public int desiredturns;
	public int action;

	float reward = 0.0f;
	List<Replay> replayMemory = new List<Replay>();
	int mCapacity = 10000;

	float discount = 0.99f;
	float exploreRate = 100.0f;
	float maxExploreRate = 100.0f;
	float minExploreRate = 0.01f;
	float exploreDecay = 0.0001f;


	//public static string monstertile1;
	//public static string monstertile2;
	// Use this for initialization
	void Start () {
		//Test();

		startingturns = desiredturns;
		int initint = 10;
		bitmap = new string[initint,initint];
		generatedmap = new string [initint,initint];	//Used to build
		themap = new string[initint, initint]; 				//Used to store built map
		testmap = new string[initint,initint];
		mapvalues = new double[initint,initint];
		string path = Application.dataPath + "/mapsData.txt";
		mdf = File.CreateText(path);
		string goodpath = Application.dataPath + "/goodmapsData.txt";
		gmdf = File.CreateText(goodpath);
		//Debug.Log(SolveMethod.solutions[0].x);
		//monstertile1 = "Wall";
		//monstertile2 = "None";
		ANNBrain.ann = new ANN(16, 16,2,16,.1f);
		SolveMethod.classifier = new float[] {0};
		/*if(loadFromFile){
			LoadWeightsFromFile();
			trainingDone=true;
			StartCoroutine(TestTrainingSet());
		}
		else{
			StartCoroutine(LoadTrainingSet());			
		}*/
		waslasttrue = false;
		//CreateTest();
		////ANNBrain.InitializeClassifiers(1);
		//StartCoroutine(LoadTrainingSet());
		//StartCoroutine(TestTrainingSet());
		//GenerateAndDraw(1);sssss
		//CreateSeed();
		//CreateAdvSeed();
		//StartCoroutine(GenerateUntilConditions());
		//AddMaps();
		//CreateTest();
	}
	public void Test(){
		int[,] test = new int[3,4];
		
		Debug.Log(test.GetLength(0));
		Debug.Log(test.GetLength(1));
	}

	public void CreateSeed(){
		isicetaken =false;
		//4
		CreateBase();
		PopulateDoorPool();
		AssignGoalAndStart();
		themap = generatedmap;
		PopulateIcePool();
		SolveMethod.startx = Startx;
		SolveMethod.starty = Starty;
		Solver.TryPieces(themap);

		//IntelligentAction();
		DrawMap();
	}
	public void CreateAdvSeed(){
		isicetaken = false;
		CreateBase();
		themap = generatedmap;
		PopulateIcePool();
		RandomizeSeed();
		themap = generatedmap;
		DrawMap();
	}
	public void RandomizeSeed(){
		PopulateCurrentIce();
		int walls = Random.Range(0,16);
		for(int i = 0; i < walls; i++){
			int ran = Random.Range(0, currentIce.Count);
			Vector2 newpos = currentIce[ran];
			generatedmap[(int)newpos.x,(int)newpos.y] = "Wall";
			currentIce.RemoveAt(ran);
		}
	}
	public void PopulateCurrentIce(){
		currentIce.Clear();
		for(int i = 0 ; i<totaldimensions; i++){
			for(int j=0 ; j<totaldimensions; j++){
				if(themap[j,i]=="Ice"){
					currentIce.Add(new Vector2(j,i));
					//Debug.Log("Add");
				}
			}
		}
	}

	public void PopulateIcePool(){
		possibleIce.Clear();
		for(int i = 0 ; i<totaldimensions; i++){
			for(int j=0 ; j<totaldimensions; j++){
				if(themap[j,i]=="Ice"){
					possibleIce.Add(new Vector2(j,i));
					//Debug.Log("Add");
				}
			}
		}
	}

	public void IntelligentAction(){
		bool go = false;
		isicetaken = false;
		inputs = new List<double>();
		qs = new List<double>();
		reward = 0;

		//Debug.Log(BinaryMap(themap)); //prints and converts to NN format
		BinaryMap(themap);
		qs = ANNBrain.ann.CalcOutput2(inputs);
		double maxQ = qs.Max();
		int maxQIndex = qs.ToList().IndexOf(maxQ);

		exploreRate = Mathf.Clamp(exploreRate - exploreDecay, minExploreRate, maxExploreRate);

		if(Random.Range(0,100) < exploreRate){
			maxQIndex = Random.Range(0,16);
		}
		//if(maxQIndex == 0){

		//}
		//else if(maxQIndex>0){
			placeNextTile((int)maxQIndex); //will also return isicetaken true if not ice
		//}
		themap = generatedmap;

		if(isicetaken){//Punish if it decides to pick a tile that already has something
			reward = -1f;
		}
		else// if(checkIce(possibleIce)<15){
		{
			//Debug.Log("New");
			reward = 1f;
		}
		Debug.Log(checkIce(possibleIce));
		/*if(checkIce(possibleIce) == 0){
			reward = 1.0f;
		}*/


//		Debug.Log(maxQIndex);
		Replay lastMemory = new Replay(inputs,maxQIndex,reward);

		if (replayMemory.Count>mCapacity){
			replayMemory.RemoveAt(0);
		}
		replayMemory.Add(lastMemory);

		if(isicetaken || checkIce(possibleIce) == 0){
			for(int i = replayMemory.Count -1; i>=0; i--){
				List<double> toutputsOld = new List<double>();
				List<double> toutputsNew = new List<double> ();
				toutputsOld = SoftMax(ANNBrain.ann.CalcOutput2(replayMemory[i].states));

				//double maxQold = toutputsOld.Max();
				//int action = toutputsOld.ToList().IndexOf(maxQold);
				action = replayMemory[i].action;
				double feedback;
				if(i==replayMemory.Count-1){
					feedback = replayMemory[i].reward;
				}
				else{
					toutputsNew = SoftMax(ANNBrain.ann.CalcOutput2(replayMemory[i+1].states));
					maxQ = toutputsNew.Max();

//					Debug.Log(maxQ);
					feedback = (replayMemory[i].reward + discount * maxQ);
				}
				toutputsOld[action] = feedback;
				ANNBrain.ann.Train(replayMemory[i].states, toutputsOld);
			}
			replayMemory.Clear();
			CreateSeed();
			//CreateAdvSeed();
		}
			

		/*if(improved){

		}
		else{

		}*/


		//Solver.TryPieces(themap);
		//DrawMap();
		//if(){

		//}
		DrawMap();

	}
	public void placeNextTile(int num){
//		Debug.Log(possibleIce.Count);
//		Debug.Log(num);
//		Debug.Log(possibleIce[num]);
		Vector2 tilepos = new Vector2(((int)possibleIce[num].x), (int)possibleIce[num].y);
		if(generatedmap[(int)tilepos.x,(int)tilepos.y] == "Ice"){
			generatedmap[(int)tilepos.x,(int)tilepos.y] = nextpiece;
		}
		else{
			isicetaken = true;
		}
	}

	List<double>SoftMax(List<double> values){
		double max = values.Max();

		float scale = 0.0f;
		for(int i =0; i< values.Count; ++i){
			scale += Mathf.Exp((float)(values[i] - max));
		}

		List<double> result = new List<double>();
		for(int i=0; i<values.Count; ++i){
			result.Add(Mathf.Exp((float)(values[i]-max))/ scale);
		}
		return result;
	}
	public int checkIce(List<Vector2> Icer){
		int counter = 0;
		for (int i=0; i<Icer.Count; i++){
			if (themap[(int)Icer[i].x, (int)Icer[i].y] == "Ice"){
				counter++;
			}
		}
		return counter;
	}
	public void TestWallHug(){
		/*Debug.Log(themap[(int)SolveMethod.bestsolution.solutionpositions[0].x-1, (int)SolveMethod.bestsolution.solutionpositions[0].y]);
		Debug.Log(themap[(int)SolveMethod.bestsolution.solutionpositions[0].x+1, (int)SolveMethod.bestsolution.solutionpositions[0].y]);
		Debug.Log(themap[(int)SolveMethod.bestsolution.solutionpositions[0].x, (int)SolveMethod.bestsolution.solutionpositions[0].y-1]);
		Debug.Log(themap[(int)SolveMethod.bestsolution.solutionpositions[0].x, (int)SolveMethod.bestsolution.solutionpositions[0].y+1]);
	*/
	}
	public IEnumerator GenerateUntilConditions(){
//		monstertile1 = monster1;
//		monstertile2 = monster2;
		startingturns = desiredturns;
		for(int i=0; i<numberofwantedmaps;i++){
			bool conditionsmet = false;

			while(!conditionsmet){
				SolveMethod.bestturns =0;
				Createfourbyfour();
//				Debug.Log(SolveMethod.bestsolutions.Count);
				if(SolveMethod.bestsolutions.Count == 3){
					if(SolveMethod.bestsolutions[0] == 0 && SolveMethod.bestsolutions[1] == 0 && SolveMethod.bestsolutions[2]>startingturns-1){// && SolveMethod.bestsolution.lrud >0){
//						Debug.Log("Conditions were met ");
						if(SolveMethod.besthaswallhug.Count == 0 && !SolveMethod.gottago){ //CHECKS FOR WALLHUG IN SOLUTION
							if(AppropriateLrud() && !CheckWallHug() && !CheckBoring()){
//								Debug.Log("Conditions were met without hug ");
								conditionsmet = true;
								Debug.Log("Map numero " + (i+1) + " Turns " + SolveMethod.bestsolutions[2]);
								DrawMap();
								Resources.UnloadUnusedAssets();
								//Print2DArray(SolveMethod.newertiles);
								yield return null;								
							}


						}
					}
				}
				if(SolveMethod.bestsolutions.Count == 2){
					if(SolveMethod.bestsolutions[0] == 0 && SolveMethod.bestsolutions[1] > startingturns-1){ //&& SolveMethod.bestsolution.lrud >0){
						//Debug.Log("Conditions were met");
						if(SolveMethod.besthaswallhug.Count == 0 && !SolveMethod.gottago){ //CHECKS FOR WALLHUG IN SOLUTION\
							if(AppropriateLrud() && !CheckBoring() && !CheckWallHug() ){
								//Debug.Log("Conditions were met without hug ");
								conditionsmet = true;
								Debug.Log("Map numero " + (i+1) + " Turns " + SolveMethod.bestsolutions[1]);
								//Debug.Log("Map numero " + (i+1) + "lrud:" + SolveMethod.bestsolution.lrud);
								//Print2DArray(SolveMethod.newtiles);

								//for(int j =0; j<8;j++){
									//Debug.Log(SolveMethod.newertiles[j);
								//}
								//Debug.Log(Vector2.Distance(SolveMethod.solutions[0].solutionpositions[0], CreateMethod.goalpos));
								DrawMap();
								Resources.UnloadUnusedAssets();
								yield return null;								
							}

						}
					}
				}
				if(SolveMethod.bestsolutions.Count == 4){
					if(SolveMethod.bestsolutions[0] == 0 && SolveMethod.bestsolutions[1] == 0 && SolveMethod.bestsolutions[2] == 0 && SolveMethod.bestsolutions[3] > startingturns-1){
						//Debug.Log("Conditions were met for 3 pieces");
						if(SolveMethod.besthaswallhug.Count == 0 && !SolveMethod.gottago){ //CHECKS FOR WALLHUG IN SOLUTION
							//Debug.Log("Conditions were met without hug ");
							if(!CheckWallHug() && AppropriateLrud()){
								conditionsmet = true;
								Debug.Log("Map numero " + (i+1) + " Turns " + SolveMethod.bestsolutions[3] + "approproiate" + AppropriateLrud());
								DrawMap();
								Resources.UnloadUnusedAssets();
								yield return null;								
							}


						}
					}
				}	
				if(SolveMethod.bestsolutions.Count == 5){
					if(SolveMethod.bestsolutions[0] == 0 && SolveMethod.bestsolutions[1] == 0 && SolveMethod.bestsolutions[2] == 0 && SolveMethod.bestsolutions[3] == 0 && SolveMethod.bestsolutions[4] > 0){
						Debug.Log("Conditions were met");
						//if(SolveMethod.besthaswallhug.Count == 0 && !SolveMethod.gottago){ //CHECKS FOR WALLHUG IN SOLUTION
							//Debug.Log("Conditions were met without hug ");
							conditionsmet = true;
							Debug.Log("Map numero " + (i+1)+ " Turns " + SolveMethod.bestsolutions[4]);
							yield return null;

						//}
					}
				}				
			}
			ConvertMapToText();
		}
	}
	public bool CheckStoppedSeed(){
		bool gottago = false;


		return gottago;
	}

	public bool CheckBoring(){
		for (int i=0; i < SolveMethod.bestsolution.solutionpositions.Count; i++){
			if(SolveMethod.bestsolution.piecetags[i] == "Wall" || SolveMethod.bestsolution.piecetags[i] == "WallSeed"){
				if(Vector2.Distance(SolveMethod.bestsolution.solutionpositions[i], CreateMethod.goalpos) > 1.5){

				}
				else{
					return true;
				}
			}
			if(SolveMethod.bestsolution.piecetags[i] == "Left" || SolveMethod.bestsolution.piecetags[i] == "Right" || 
				SolveMethod.bestsolution.piecetags[i] == "Up" || SolveMethod.bestsolution.piecetags[i] == "Down" ||
				SolveMethod.bestsolution.piecetags[i] == "LeftSeed" || SolveMethod.bestsolution.piecetags[i] == "RightSeed" || 
				SolveMethod.bestsolution.piecetags[i] == "UpSeed" || SolveMethod.bestsolution.piecetags[i] == "DownSeed"){
				
				//Debug.Log(Vector2.Distance(SolveMethod.bestsolution.solutionpositions[i], CreateMethod.goalpos));
				if(Vector2.Distance(SolveMethod.bestsolution.solutionpositions[i], CreateMethod.goalpos) != 2){

				}
				else{
					return true;
				}
			}

		}
		return false;
	}

    public static void Print2DArray(string[,] matrix)
    {
    	string[] map = new string[matrix.GetLength(0)];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
        	string current = "";
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
            	current = current + matrix[j,i];
            }
            map[i] = current;
//            Debug.Log(map[i]);

        }
    }

	public bool AppropriateLrud(){
		int lrudpieces = 0;
		for (int i=0; i < SolveMethod.bestsolution.solutionpositions.Count; i++){
 			//Debug.Log("piece number " + i + " is " + SolveMethod.bestsolution.piecetags[i]);
			if(SolveMethod.bestsolution.piecetags[i] == "Left" || SolveMethod.bestsolution.piecetags[i] == "Right" || 
				SolveMethod.bestsolution.piecetags[i] == "Up" || SolveMethod.bestsolution.piecetags[i] == "Down" ||
				SolveMethod.bestsolution.piecetags[i] == "LeftSeed" || SolveMethod.bestsolution.piecetags[i] == "RightSeed" || 
				SolveMethod.bestsolution.piecetags[i] == "UpSeed" || SolveMethod.bestsolution.piecetags[i] == "DownSeed"){	
				lrudpieces++;
			}		
		}
//		Debug.Log(lrudpieces + " lrud" + SolveMethod.bestsolution.lrud);
		if(lrudpieces <= SolveMethod.bestsolution.lrud) {
			return true;			
		}
		else{
			return false;
		}

	}
	public bool CheckWallHug(){

		for (int i=0; i < SolveMethod.bestsolution.solutionpositions.Count; i++){
 			//Debug.Log("piece number " + i + " is " + SolveMethod.bestsolution.piecetags[i]);

			if((SolveMethod.bestsolution.piecetags[i] == "Left" ||
				SolveMethod.bestsolution.piecetags[i] == "LeftSeed" ) && SolveMethod.bestsolution.solutionpositions[i].x-2>-1){	
				if(themap[(int)SolveMethod.bestsolution.solutionpositions[i].x-2, (int)SolveMethod.bestsolution.solutionpositions[i].y] == "Wall"){
					
					return true;
				}
				return false;
			}
			if((SolveMethod.bestsolution.piecetags[i] == "Right" ||
				SolveMethod.bestsolution.piecetags[i] == "RightSeed") && SolveMethod.bestsolution.solutionpositions[i].x+2< totaldimensions){	
				if(themap[(int)SolveMethod.bestsolution.solutionpositions[i].x+2, (int)SolveMethod.bestsolution.solutionpositions[i].y] == "Wall"){
					return true;
				}	
				return false;			
			}		
			if((SolveMethod.bestsolution.piecetags[i] == "Up" ||
				SolveMethod.bestsolution.piecetags[i] == "UpSeed" )&& SolveMethod.bestsolution.solutionpositions[i].y-2>-1){	
				if(themap[(int)SolveMethod.bestsolution.solutionpositions[i].x, (int)SolveMethod.bestsolution.solutionpositions[i].y-2] == "Wall"){
					return true;
				}	
				return false;
			}		
			if((SolveMethod.bestsolution.piecetags[i] == "Down" ||
				SolveMethod.bestsolution.piecetags[i] == "DownSeed") && SolveMethod.bestsolution.solutionpositions[i].x+2<totaldimensions ){	
				if(themap[(int)SolveMethod.bestsolution.solutionpositions[i].x, (int)SolveMethod.bestsolution.solutionpositions[i].y+2] == "Wall"){
					return true;
				}	
				return false;
			}	
			return false;	
		}
		return false;
	}
	public void BinaryMap(string[,] wantedmap){ 
		curstring = "";
		stringcounter = 0;
		//Doing it for 4 icedimensions atm
		for (int i =1; i<7; i++){
			for(int j=1; j<7; j++){
				if((i==1&&j==1)||(i==1&&j==6)||(i==6&&j==1)||(i==6&&j==6)){

				}
				else if(i==1 || j==6 || j==1 || i==6){//OuterWalls
					//Debug.Log("Edge");
					/*int[] curarray = {0,0,0};
					stringcounter = stringcounter + curarray.Length;
					switch (wantedmap[j,i]){
						case "Wall":
						curarray [0] = 1;
						break;
						case "Start":
						curarray[1] = 1;
						break;
						case "Goal":
						curarray[2] = 1;
						break;
					}
		//			Debug.Log(curarray.Length);
					if(curarray.Length!=0){
						for(int a =0; a< curarray.Length; a++){
							inputs.Add((double)curarray[a]);
							curstring = curstring + curarray[a].ToString() + ",";
						}	
		//				Debug.Log(curarray[0] +""+curarray[1]);
					}*/
				}
				else{//ConvertToBitsIce
		//			Debug.Log("Mid");
					int[] curarray = {0};
					stringcounter = stringcounter + curarray.Length;

		//			Debug.Log(theval);
//					Debug.Log(wantedmap[j,i]);
					switch (wantedmap[j,i]){
						//case "Ice":
						//	curarray[0] = 1;
						//	break;
						case "Wall":
							curarray[0] = 1;
							break;
						/*case "Hole":
							curarray[2] = 1;
							break;
						case "Fragile":
							curarray[3] = 1;
							break;
						case "Wood":
							curarray[4] = 1;
							break;*/
					}	
					if(curarray.Length!=0){
						for(int a =0; a< curarray.Length; a++){
//							Debug.Log(a);
							inputs.Add((double)curarray[a]);
							curstring = curstring + curarray[a].ToString() + ",";
						}	
		//				Debug.Log(curarray[0] +""+curarray[1]);
					}	

		/*		if(curarray.Length!=0){
					for(int i =0; i< curarray.Length; i++){
						inputs.Add((double)curarray[i]);
						curstring = curstring + curarray[i].ToString() + ",";
					}	
				}*/
				}
			}
		}
//		Debug.Log(stringcounter + "Amount of inputs from map");
		nextpiece = "Wall";
		//int[] newarray = {0,0,0,0};
		//int randomizerer = Random.Range(0,4);
		/*switch(randomizerer){
			case 0:
				nextpiece = "Wall";
				break;
			case 1:
				nextpiece = "Hole";
				break;
			case 2: 
				nextpiece = "Fragile";
				break;
			case 3:
				nextpiece = "Wood";
				break;
		}*/
		//newarray[randomizerer] = 1;
		/*if(newarray.Length!=0){
			for(int a =0; a< newarray.Length; a++){
				inputs.Add((double)newarray[a]);
				curstring = curstring + newarray[a].ToString() + ",";
			}	
//				Debug.Log(curarray[0] +""+curarray[1]);
		}*/
		//piecetiles.Add(nextpiece);	
		//Debug.Log("next piece is " + nextpiece);
		curstring = curstring.Substring(0, curstring.Length-1);
//		Debug.Log(curstring);
		//return curstring;
	}
	public void Wrapper(){
		//monstertile1 = monster1;
		StartCoroutine(GenerateUntilConditions());
	}
	void GenerateAndDraw(int num){
		for(int i=0; i<num;i++){
			Createfourbyfour();
			//Debug.Log(BinaryMap(themap));
		//	DrawMap();
			/*if(8<SolveMethod.bestsol){
				DrawMap();
				break;
			}*/
		}
	}
	// Update is called once per frame
	/*void Update () {
		//Createfourbyfour();
		/*if(Input.GetKeyDown(KeyCode.K)){
			FeedMap(themap);
			PrintNumMap(mapvalues);
		}
	}*/

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
		foreach(string gmd in goodmapsdata){
			gmdf.WriteLine(gmd);
		}
		gmdf.Close();
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
		string path = Application.dataPath + "/quantData.txt";
		string line;
		Debug.Log(File.Exists(path));
		if(File.Exists(path)){
			int lineCount = File.ReadAllLines(path).Length;
			StreamReader mdf = File.OpenText(path);
			List<double>calcOutputs = new List<double>();
			inputs = new List<double>();
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
	public void AssignMaxSpawners(int thedimension){
		switch(thedimension){
			case 9:
				wallmax = Random.Range(8,10);
				lavamax = Random.Range(6,10);
				woodmax = Random.Range(10,14);
				fragilemax = Random.Range(10,14);
				break;
			case 8:
				wallmax = Random.Range(1,10);
				lavamax = Random.Range(0,8);
				woodmax = Random.Range(1,11);
				fragilemax = Random.Range(1,10);
				break;
			case 7:
				wallmax = Random.Range(1,9);
				lavamax = Random.Range(0,7);
				woodmax = Random.Range(1,9);
				fragilemax = Random.Range(0,9);
				break;
			case 6:
				wallmax = Random.Range(1,8);
				lavamax = Random.Range(0,6);
				woodmax = Random.Range(0,9);
				fragilemax = Random.Range(0,6);
				break;
			case 5:
				/*wallmax = Random.Range(2,3);
				lavamax = Random.Range(2,3);
				woodmax = Random.Range(0,6);*/
				wallmax = Random.Range(0,5);
				lavamax = Random.Range(0,4);
				woodmax = Random.Range(0,6);
				fragilemax = Random.Range(0,7);
				break;
			case 4:
				// wallmax = Random.Range(0,4);
				// lavamax = Random.Range(0,5);
				// woodmax = Random.Range(1,5);
				// fragilemax = Random.Range(0,1);	

				wallmax = Random.Range(1,4);
				lavamax = Random.Range(0,3);
				woodmax = Random.Range(0,2);
				fragilemax = Random.Range(0,5);			
				break;
		}

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
					Debug.Log((float)outputs[0] +"" + (float)calcOutputs[0]);
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
		//GUI.Label(new Rect(25,25,250,30), "SSE: " +lastSSE, style);
		/*GUI.Label(new Rect(25,75,250,30), "Alpha: "+ ANNBrain.ann.alpha,style);
		GUI.Label(new Rect(25,125,250,30), "Trained: " +trainingProgress,style);
		GUI.Label(new Rect(25,175,250,30), "Lowest SSE: " +lowestSSE,style);
		GUI.Label(new Rect(25,25,250,30), "exploreRate " +exploreRate,style);*/
		GUI.Label(new Rect(25,300,250,30), " Piece are " + monster1 + " and " + monster2);
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
	public void CreateTest(){
		ResetAll();

		piecetiles = new List<string>();
		icedimensions = 6;
		mapdimension = 8;
		//ResetAll();
		AssignWallCounter();
		CreateIce();
		Add2Outerwalls();
		testmap = generatedmap;
		testmap[3,1] = "Wall";
		testmap[6,2] = "Hole";
		testmap[6,3] = "Fragile";
		testmap[0,4] = "Goal";
		testmap[2,4] = "Wood";
		testmap[5,4] = "Hole";
		testmap[2,5] = "Fragile";
		testmap[3,5] = "Wood";
		testmap[4,5] = "Wood";
		testmap[1,6] = "Fragile";
		testmap[2,6] = "Fragile";
		testmap[6,6] = "Hole";
		testmap[5,7] = "Start";
		SolveMethod.startx = 5;
		SolveMethod.starty = 7;
		piecetiles.Add("Wall");
		piecetiles.Add("Wall");
		piecetiles.Add("Wall");
		DrawMap();
		generatedmap = (string[,]) testmap.Clone();;

		Solver.TryPieces(testmap);
		//PrintMap();
		DrawMap();
	}
	public void CreateTest2(){
		icedimensions = 4;
		ResetAll();
		AssignWallCounter();
		CreateIce();
		Add2Outerwalls();
		testmap = generatedmap;
		testmap[1,3] = "Start";
		testmap[5,5] = "Hole";
		testmap[5,2] = "Hole";
		testmap[2,4] = "Hole";
		testmap[2,3] = "Fragile";
		testmap[3,2] = "Fragile";
		testmap[4,3] = "Fragile";
		testmap[1,2] = "Goal";
		testmap[4,5] = "Wood";
		testmap[2,5] = "Wall";
		SolveMethod.startx = 1;
		SolveMethod.starty = 3;
		piecetiles.Add("Wall");
		piecetiles.Add("Down");
		piecetiles.Add("Up");
		DrawMap();
		generatedmap = testmap;
		Solver.TryPieces(testmap);
		DrawMap();
	}
	public void CreateBase(){
		icedimensions = 4;
		AssignWallCounter();
		CreateIce();
		Add2Outerwalls();
		testmap = generatedmap;		
	}
	public void Createfourbyfour(){
		mapdimension = totaldimensions;
		icedimensions = Random.Range(7,9);
		//desiredturns = icedimensions;
		startingturns = desiredturns;
		AssignWallCounter();
		AssignMaxSpawners(icedimensions);
		ResetAll();
		CreateIce();
		Add2Outerwalls();
		PopulateDoorPool();
		AssignGoalAndStart();
		//AddOnMapTiles ();
		AddSpecificTiles();
		SolveMethod.startx = Startx;
		SolveMethod.starty = Starty;
		AddPieceTiles ();
		themap = generatedmap;
		//DrawMap();
		Solver.TryPieces(themap);//Solve normal, adds annbrain.sol.

		//Solve with possible piecetiles
		//PrintMap();
		//DrawMap();
		FeedMap(themap);
		//PrintNumMap(mapvalues);
		//ANNBrain.Run(mapvalues, ANNBrain.sol, false);
		//ANNBrain.RunV3(mapvalues,SolveMethod.bestsol, true );
		//CreateDataLine();
		//if(ANNBrain.sol == 1){
		//if((!waslasttrue && ANNBrain.sol ==1)||(waslasttrue && ANNBrain.sol ==0)){

		CreateDataLineV2();

//		Debug.Log(BinaryMap(themap));

//		Debug.Log(BinaryMap(themap));		
		//waslasttrue = !waslasttrue;
		//}
		//}
//		Debug.Log("Fact is " + ANNBrain.bestsol + "AI says " +   )

		//Debug.Log("BestSolutions is " + SolveMethod.bestsol);
//		Debug.Log("Best turns " + ANNBrain.sol);
		//ANNBrain

	}
	public void AssignWallCounter(){
		int difference = (totaldimensions-icedimensions);
		if(difference%2 == 0){ //if pair difference, equal sides on each)
			wallsleft = (int)difference/2;
			wallsright = (int)difference/2;
			wallsup = (int)difference/2;
			wallsdown = (int)difference/2;
		}
		else{
			int randomizer = Random.Range(0,2);
			int randomizer2 = Random.Range(0,2);
			if(randomizer ==1){
				wallsleft = (int)difference/2;
				wallsright = (int)difference/2 + 1;
			}
			else{
				wallsleft = (int)difference/2+1;
				wallsright = (int)difference/2;
			}
			if(randomizer2 == 1){
				wallsup = (int)difference/2;
				wallsdown = (int)difference/2 + 1;
			}
			else{
				wallsup = (int)difference/2+1;
				wallsdown = (int)difference/2;				
			}
		}

	}
	public void CreateDataLine(){
		string md = "";
		for(int y = 0; y<totaldimensions; y++){
			for(int x = 0; x<totaldimensions; x++){
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
	public void CreateDataLineV2(){
		/*md = md + ",";
		for(int y = 0; y<SolveMethod.classifier.Length;y++){
			md = md + SolveMethod.classifier[y].ToString() + ",";
		}
		md = md.Substring(0, md.Length - 1);
		Debug.Log(md);
		if(!mapsdata.Contains(md)){
			mapsdata.Add(md);
			waslasttrue = !waslasttrue;
		}*/
	}

	public  void CreateIce(){
		for(int y = 0; y<totaldimensions; y++){
			for(int x = 0; x<totaldimensions; x++){
				generatedmap[x,y] = "Ice";
			}
		}
	}
	public void Add2Outerwalls(){
		for(int i = 0; i<totaldimensions; i++){
			int var;
			if(i<wallsup || i>totaldimensions-1-wallsdown){
				var = 0;
			}
			else{
				var = 1;
			}
			switch(var){
				case 0:
					for(int j = 0; j<totaldimensions; j++){
						generatedmap[j,i] = "Wall";
					}
					break;
				case 1:
					for(int j = 0; j<totaldimensions; j++){
						if(j<wallsleft || j>(totaldimensions-1-wallsright)){
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
	public void PopulateDoorPool(){
		for(int i = 0; i<totaldimensions; i++){
			for(int j = 0; j<totaldimensions; j++){
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
	public static void AssignGoalAndStart(){ //grabs the doopool and 1 start and goal
		int max = doorable.Count;
		int num = Random.Range(0,max);
//		Debug.Log(num);
//		Debug.Log(max);
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
	public void ConvertMapToText(){
		goodmapsdata.Add("Map" + totaldimensions.ToString()) ;
		for(int i = 0; i<totaldimensions; i++){
			mapstring = "";
			for(int j = 0; j<totaldimensions; j++){
				switch(themap[j,i]){
					case "Ice":
						mapstring = mapstring + "0 ";
						break;
					case "Wall":
						mapstring = mapstring + "1 ";
						break;
					case "Start":
						mapstring = mapstring + "S ";
						break;
					case "Goal":
						mapstring = mapstring + "G ";
						break;
					case "Wood":
						mapstring = mapstring + "# ";
						break;
					case "Fragile":
						mapstring = mapstring + "F ";
						break;
					case "Hole":

						mapstring = mapstring + "H ";
						break;
				}
			}
			goodmapsdata.Add(mapstring);
		}
		mapstring = "";
		for(int i = 0; i<piecetiles.Count; i++){
			switch(piecetiles[i]){
				case "Wall":
					mapstring = mapstring + "P";
					break;
				case "Left":
					mapstring = mapstring + "L";
					break;
				case "Right":
					mapstring = mapstring + "R";
					break;
				case "Up":
					mapstring = mapstring + "U";
					break;
				case "Down":
					mapstring = mapstring + "D";
					break;
				case "WallSeed":
					mapstring = mapstring + "p";
					break;
				case "LeftSeed":
					mapstring = mapstring + "l";
					break;
				case "UpSeed":
					mapstring = mapstring + "u";
					break;
				case "DownSeed":
					mapstring = mapstring + "d";
					break;
				case "RightSeed":
					mapstring = mapstring + "r";
					break;
			}
			mapstring = mapstring +""+ SolveMethod.bestsolution.solutionpositions[i].x + "" + SolveMethod.bestsolution.solutionpositions[i].y + " ";

		}
		mapstring = mapstring + "" + "T" + SolveMethod.bestsolutions[SolveMethod.bestsolutions.Count-1];
		goodmapsdata.Add(mapstring);
		goodmapsdata.Add("");
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
	public void PrintMap(){
		for(int i = 0; i<totaldimensions; i++){
			for(int j = 0; j<totaldimensions; j++){
				Debug.Log(generatedmap[j,i]);
			}
		}
	}
	public void DrawMap(){
		for(int i = 0; i<totaldimensions; i++){
			for(int j = 0; j<totaldimensions; j++){
				PaintTag(j,i);
			}
		}
	}
	public static void PaintTag(int x, int y){
		string tag = generatedmap[x,y];
//		Debug.Log(tag);
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
				else if(tag == "Wood"){
					component.GetComponent<SpriteRenderer>().color = Color.gray;
				}
				else if(tag == "Fragile"){
					component.GetComponent<SpriteRenderer>().color = Color.magenta;
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
		SolveMethod.solutions.Clear ();
		CreateMethod.piecetiles.Clear();
		//ResetColor(); //notnow
		doorable.Clear();
	}
	public void AddOnMapTiles(){
		PopulateCleanIce ();
		extrawalls = Random.Range(0,wallmax);
		for (int i = 0; i < extrawalls; i++) {
			ExcludeAdjacent();
			int max = cleanice.Count;
			int num = Random.Range (0, max);
			Vector2 newwall = cleanice[num];
			generatedmap [Mathf.RoundToInt(newwall.x), Mathf.RoundToInt(newwall.y)] = "Wall";
			cleanice.Remove (newwall);
			ReturnAdjacent();
		}
		lava = Random.Range(0,lavamax);
		for (int i = 0; i < lava; i++) {
			ExcludeAdjacent();
			int max = cleanice.Count;
			int num = Random.Range (0, max);
			Vector2 newlava = cleanice[num];
			generatedmap [Mathf.RoundToInt(newlava.x), Mathf.RoundToInt(newlava.y)] = "Hole";
			cleanice.Remove (newlava);
			ReturnAdjacent();
		}
		int wood  = Random.Range(0,woodmax);
		for (int i = 0; i < wood; i++) {
			int max = cleanice.Count;
			int num = Random.Range (0, max);
			Vector2 newwood = cleanice[num];
			generatedmap [Mathf.RoundToInt(newwood.x), Mathf.RoundToInt(newwood.y)] = "Wood";
			cleanice.Remove (newwood);
		}
		int fragilenum  = Random.Range(0,fragilemax);
		for (int i = 0; i < fragilenum; i++) {
			int max = cleanice.Count;
			int num = Random.Range (0, max);
			Vector2 newfragile = cleanice[num];
			generatedmap [Mathf.RoundToInt(newfragile.x), Mathf.RoundToInt(newfragile.y)] = "Fragile";
			cleanice.Remove (newfragile);
		}
//		Debug.Log(cleanice.Count + "Icetiles");
	}
	public void AddSpecificTiles(){
		PopulateCleanIce ();
		extrawalls = wallmax;
		for (int i = 0; i < extrawalls; i++) {
			ExcludeAdjacent();
			int max = cleanice.Count;
			int num = Random.Range (0, max);
			Vector2 newwall = cleanice[num];
			generatedmap [Mathf.RoundToInt(newwall.x), Mathf.RoundToInt(newwall.y)] = "Wall";
			cleanice.Remove (newwall);
			ReturnAdjacent();
		}
		lava = lavamax;
		for (int i = 0; i < lava; i++) {
			ExcludeAdjacent();
			int max = cleanice.Count;
			int num = Random.Range (0, max);
			Vector2 newlava = cleanice[num];
			generatedmap [Mathf.RoundToInt(newlava.x), Mathf.RoundToInt(newlava.y)] = "Hole";
			cleanice.Remove (newlava);
			ReturnAdjacent();
		}
		int wood  =woodmax;
		for (int i = 0; i < wood; i++) {
			int max = cleanice.Count;
			int num = Random.Range (0, max);
			Vector2 newwood = cleanice[num];
			generatedmap [Mathf.RoundToInt(newwood.x), Mathf.RoundToInt(newwood.y)] = "Wood";
			cleanice.Remove (newwood);
		}
		int fragilenum  = fragilemax;
		for (int i = 0; i < fragilenum; i++) {
			int max = cleanice.Count;
			int num = Random.Range (0, max);
			Vector2 newfragile = cleanice[num];
			generatedmap [Mathf.RoundToInt(newfragile.x), Mathf.RoundToInt(newfragile.y)] = "Fragile";
			cleanice.Remove (newfragile);
		}
//		Debug.Log(cleanice.Count + "Icetiles");
	}
	public void PopulateCleanIce(){
		//Playerprefsx
		cleanice.Clear ();
		for(int i = 0; i<totaldimensions; i++){
			for(int j = 0; j<totaldimensions; j++){
				if(generatedmap[j,i] == "Ice"){
					cleanice.Add(new Vector2(j,i));
				}
			}
		}	
	}
	public void ExcludeAdjacent(){
		temporarilyoutice.Clear();
		for(int i = 0; i< cleanice.Count; i++){
			Vector2 StartV = new Vector2(Startx,Starty);
			float diffS = Vector2.Distance(cleanice[i], StartV);
			float diffG = Vector2.Distance(cleanice[i], new Vector2(Goalx, Goaly));
			if(diffS == 1){
				//Debug.Log("Removing Adjacent at" +  cleanice[i]);
				temporarilyoutice.Add(cleanice[i]);
			}
			if(diffG == 1){
				//Debug.Log("Removing Adjacent at" + cleanice[i]);
				temporarilyoutice.Add(cleanice[i]);
			}

		}
		for (int i=0; i<temporarilyoutice.Count; i++){
			//if (cleanice.Contains(temporarilyoutice[i]){
				//Debug.Log("Removing Adjacent at" + temporarilyoutice[i]);
				cleanice.Remove(temporarilyoutice[i]);
			//}
		}
	}
	public void ReturnAdjacent(){
		for(int i = 0; i<temporarilyoutice.Count; i++){
			cleanice.Add(temporarilyoutice[i]);
		}
	}
	public void AddPieceTiles(){
		/*for (int i = 0; i < pedro; i++) {
			piecetiles.Add ("Wall");
		}*/
		//piecetiles = new List<string>();
		int randomint  = 0;//Random.Range(0, 10);
		int[] validchoices = {0};
		randomint = validchoices[Random.Range(0,validchoices.Length)];
		switch(randomint){
			case 0:
				monster1 = "Wall";
				break;
			case 1:
				monster1 = "Left";
				break;
			case 2:
				monster1 = "Right";
				break;
			case 3:
				monster1 = "Up";
				break;
			case 4:
				monster1 = "Down";
				break;
			case 5:
				monster1 = "None";
				break;
			case 6:
				monster1 = "LeftSeed";
				break;
			case 7:
				monster1 = "RightSeed";
				break;
			case 8:
				monster1 = "UpSeed";
				break;
			case 9:
				monster1 = "DownSeed";
				break;
			case 10:
				monster1 = "WallSeed";
				break;
		}
		//monster1 = "WallSeed";
		//randomint  = Random.Range(0, 5);
		int[] validchoices2 = {0,1,2,3,4,6,7,8,9,10};//,5,6,7,8,9,10};
		randomint = validchoices2[Random.Range(0,validchoices2.Length)];
//		Debug.Log(randomint);

		switch(randomint){
			case 0:
				monster2 = "Wall";
				break;
			case 1:
				monster2 = "Left";
				break;
			case 2:
				monster2 = "Right";
				break;
			case 3:
				monster2 = "Up";
				break;
			case 4:
				monster2 = "Down";
				break;
			case 5:
				monster2 = "None";
				break;
			case 6:
				monster2 = "LeftSeed";
				break;
			case 7:
				monster2 = "RightSeed";
				break;
			case 8:
				monster2 = "UpSeed";
				break;
			case 9:
				monster2 = "DownSeed";
				break;
			case 10:
				monster2 = "WallSeed";
				break;

		}
		piecetiles.Add(monster1);
		if(monster2 != "None"){
			piecetiles.Add(monster2);    //reactivate to add piece2.
			//piecetiles.Add("Wall");    //reactivate to add piece2.

		}
		int[] validchoices3 = {5};//0,1,2,3,4,5,5,5,5};//,5,6,7,8,9,10};

		randomint = validchoices3[Random.Range(0,validchoices3.Length)];
//		Debug.Log(randomint);
		switch(randomint){
			case 0:
				monster3 = "Wall";
				break;
			case 1:
				monster3 = "Left";
				break;
			case 2:
				monster3 = "Right";
				break;
			case 3:
				monster3 = "Up";
				break;
			case 4:
				monster3 = "Down";
				break;
			case 5:
				monster3 = "None";
				break;
			case 6:
				monster3 = "LeftSeed";
				break;
			case 7:
				monster3 = "RightSeed";
				break;
			case 8:
				monster3 = "UpSeed";
				break;
			case 9:
				monster3 = "DownSeed";
				break;
			case 10:
				monster3 = "WallSeed";
				break;
		}
//		Debug.Log(piecetiles.Count);
		if(monster3 != "None"){
		piecetiles.Add(monster3);    //reactivate to add piece3/
		}
		randomint = validchoices[Random.Range(0,validchoices.Length)];
		//Debug.Log(randomint);
		switch(randomint){
			case 0:
				monster4 = "Wall";
				break;
			case 1:
				monster4 = "Left";
				break;
			case 2:
				monster4 = "Right";
				break;
			case 3:
				monster4 = "Up";
				break;
			case 4:
				monster4 = "Down";
				break;
			case 5:
				monster4 = "None";
				break;
			case 6:
				monster4 = "LeftSeed";
				break;
			case 7:
				monster4 = "RightSeed";
				break;
			case 8:
				monster4 = "UpSeed";
				break;
			case 9:
				monster4 = "DownSeed";
				break;
		}
		if(monster4 != "None"){
		//piecetiles.Add(monster4);    //reactivate to add piece3/
		}

		//piecetiles.Add(monster4);
		//piecetiles.Add("Left");

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
	public void FeedMap(string[,] stringer){
		for(int i = 0; i<totaldimensions; i++){
			for(int j = 0; j<totaldimensions; j++){
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
	public void FeedMapV2(string[,] stringer){
		for(int i = 0; i<totaldimensions; i++){
			for(int j = 0; j<totaldimensions; j++){
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
	public void PrintNumMap(double[,] maper){
		for(int i = 0; i<totaldimensions; i++){
			for(int j = 0; j<totaldimensions; j++){
				Debug.Log(maper[j,i]);
			}
		}
	}
	//public void
}
