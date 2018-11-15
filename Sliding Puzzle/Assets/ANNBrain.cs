using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANNBrain : MonoBehaviour {
	public static ANN ann;
	public static double sol;//solution in bool
	public static double los;
	public static string curstring = "";
	public static List<double> inputs = new List<double>();
	int[] curarray = {};
	float[] floatarray = {};
	//public static float dsol;
	public static float sx;
	public static float sy;
	public static float gx;
	public static float gy;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static List<double> Run(double[,] map,double yn, bool train)
	{
		List<double> inputs = new List<double>();
		List<double> outputs = new List<double>();
		for(int i=0; i<8;i++){
			for(int j=0;j<8;j++){
				inputs.Add(CalculateZ(map[j,i]));
			}
		}
		outputs.Add(yn);
		//outputs.Add(ny);
//		Debug.Log(inputs.Count);
		if(train)
			return (ann.Train(inputs,outputs));
		else
			return (ann.CalcOutput(inputs,outputs));
	}
	public static void CreateNetwork(){

	}
	public static void InitializeClassifiers(int num){
		SolveMethod.classifier = new float[] {0,0,0,0,0,0,0,0};
//		Debug.Log(SolveMethod.classifier.Length);
		if(num!=0 && num<=SolveMethod.classifier.Length){
		SolveMethod.classifier[num-1] = 1;
		}
		else if(SolveMethod.classifier.Length < num){
			SolveMethod.classifier[SolveMethod.classifier.Length -1] = 1;
			Debug.Log("TooHigh");
		}
	}
	public static void InitializeClassifiers2(int num){
		SolveMethod.classifier = new float[] {0};
		SolveMethod.classifier[0] = SolveMethod.dsol;
//		Debug.Log(SolveMethod.classifier.Length);
		//if (ANNBrain.sol ==1){
		//	SolveMethod.classifier[0] = 1;
		//}
	}
		//if(num =10)
	public static List<double> RunV2(double[,] map, int solutionnum,bool train)
	{
		List<double> inputs = new List<double>();
		List<double> outputs = new List<double>();
		for(int i=0; i<8;i++){
			for(int j=0;j<8;j++){
				inputs.Add((int)(map[j,i]));
			}
		}
		InitializeClassifiers(solutionnum);
//		Debug.Log(SolveMethod.classifier.Length);
		for(int i = 0; i < SolveMethod.classifier.Length; i++){
			outputs.Add(SolveMethod.classifier[i]);
			//Debug.Log
		}
		//for(int i = 0; i<SolveMethod.classifier.Length; i++){
		//	Debug.Log("option" + i + "is" + outputs[i]);
		//} 
		//Debug.Log(outputs.Count);
		//for(int i=0; i<pedros; i++){
		//	inputs.add((double) 1)
		//}
		//outputs.Add(yn);
		//outputs.Add(ny);
//		Debug.Log(inputs.Count);
		if(train)
			return (ann.Train(inputs,outputs));
		else
			return (ann.CalcOutput(inputs,outputs));
	}
	public static List<double> RunV3(double[,] map, int solutionnum,bool train)
	{
		inputs.Clear();
		List<double> outputs = new List<double>();
		string totalstring = "";
		for(int i=1; i<7;i++){
			for(int j=1;j<7;j++){
				//Debug.Log((int)map[j,i]);
				//Debug.Log(ConvertToBits((int)map[j,i])[0]);
				//CreateMethod.bitmap[j,i]= ConvertToBits((int)map[j,i]);
				curstring = "";
				ConvertToBitsV2((int)map[j,i], j, i);
				/*for(int x =0; x< currentarray.Length; x++){
					inputs.Add((double)currentarray[x]);
					curstring = curstring + currentarray[x].ToString() + ",";
				}	*/
//				Debug.Log(curstring);
//				Debug.Log(totalstring);
				totalstring = totalstring +curstring.ToString();
//				//Debug.Log(curstring);
			}
		}
//		Debug.Log(totalstring);
//		Debug.Log(inputs.Count);
		totalstring = totalstring.Substring(0,totalstring.Length-1);
		CreateMethod.md = totalstring;
		InitializeClassifiers2(solutionnum);//output digitizing
		for(int i = 0; i < SolveMethod.classifier.Length; i++){
			outputs.Add(SolveMethod.classifier[i]);

		}

		if(train)
			return (ann.Train(inputs,outputs));
		else
			return (ann.CalcOutput(inputs,outputs));
	}
	public static List<double> RunV4(double[,] map, int solutionnum,bool train)
	{
		inputs.Clear();
		List<double> outputs = new List<double>();
		string totalstring = "";
		for(int i=1; i<7;i++){
			for(int j=1;j<7;j++){
				//Debug.Log((int)map[j,i]);
				//Debug.Log(ConvertToBits((int)map[j,i])[0]);
				//CreateMethod.bitmap[j,i]= ConvertToBits((int)map[j,i]);
				curstring = "";
				ConvertToBitsV2((int)map[j,i], j, i);
				/*for(int x =0; x< currentarray.Length; x++){
					inputs.Add((double)currentarray[x]);
					curstring = curstring + currentarray[x].ToString() + ",";
				}	*/
//				Debug.Log(curstring);
//				Debug.Log(totalstring);
				totalstring = totalstring +curstring.ToString();
//				//Debug.Log(curstring);
			}
		}
//		Debug.Log(totalstring);
//		Debug.Log(inputs.Count);
		totalstring = totalstring.Substring(0,totalstring.Length-1);
		CreateMethod.md = totalstring;
		InitializeClassifiers2(solutionnum);//output digitizing
		for(int i = 0; i < SolveMethod.classifier.Length; i++){
			outputs.Add(SolveMethod.classifier[i]);

		}

		if(train)
			return (ann.Train(inputs,outputs));
		else
			return (ann.CalcOutput(inputs,outputs));
	}
	public static double CalculateZ(double num){
		return (num-2)/1.58;
	}

	public static int[] ConvertToBits(int theval){
		int[] curarray = {0,0,0,0,0};
		switch (theval){
			case 0:
			curarray[0] = 1;
			return curarray;
			case 1:
			curarray[1] = 1;
			return curarray;
			case 2:
			curarray[2] = 1;
			return curarray;
			case 3:
			curarray[3] = 1;
			return curarray;
			case 4:
			curarray[4] = 1;
			return curarray;
			default:
			return curarray;
		}
	}
	public static void ConvertToBitsV2(int theval, int x, int y){
		//int[] curarray = {};
		int length;
		if((x==1 && y==1)||(x==6 && y==6)||(x==1 && y==6)||(x==6 && y==1)){//corners
//			Debug.Log("Corner");
//			curarray = {};
			int[] curarray = {};
//			Debug.Log(curarray.Length);
		}
		else if(x==1 || x==6 || y==1 || y==6){//OuterWalls
			//Debug.Log("Edge");
			int[] curarray = {0,0};
			switch (theval){
				/*case 1:
				curarray [0] = 1;
				break;*/
				case 2:
				curarray[0] = 1;
				break;
				case 3:
				curarray[1] = 1;
				break;
				/*case 3:
				curarray[2] = 1;
				break;*/
			}
//			Debug.Log(curarray.Length);
			if(curarray.Length!=0){
				for(int i =0; i< curarray.Length; i++){
					inputs.Add((double)curarray[i]);
					curstring = curstring + curarray[i].ToString() + ",";
				}	
//				Debug.Log(curarray[0] +""+curarray[1]);
			}
		}
		else{//ConvertToBitsIce
//			Debug.Log("Mid");
			int[] curarray = {0,0};
//			Debug.Log(theval);
			switch (theval){
				/*case 0:
				curarray[0] = 1;
				break;*/
				case 1:
				curarray[0] = 1;
				break;
				case 4:
				curarray[1] = 1;
				break;
			}	
			if(curarray.Length!=0){
				for(int i =0; i< curarray.Length; i++){
					inputs.Add((double)curarray[i]);
					curstring = curstring + curarray[i].ToString() + ",";
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
/*		if(curarray.Length!=0){
			for(int i =0; i< curarray.Length; i++){
				inputs.Add((double)curarray[i]);
				curstring = curstring + curarray[i].ToString() + ",";
			}	
		}*/
	}
	public static void ConvertToBitsV3(int theval, int x, int y){
		//int[] curarray = {};
		int length;
		if((x==1 && y==1)||(x==6 && y==6)||(x==1 && y==6)||(x==6 && y==1)){//corners
//			Debug.Log("Corner");
//			curarray = {};
			int[] curarray = {};
//			Debug.Log(curarray.Length);
		}
		else if(x==1 || x==6 || y==1 || y==6){//OuterWalls
			//Debug.Log("Edge");
			int[] curarray = {0,0};
			switch (theval){
				case 2:
				curarray[0] = 1;
				break;
				case 3:
				curarray[1] = 1;
				break;
			}
			Debug.Log(curarray.Length);
			if(curarray.Length!=0){
				for(int i =0; i< curarray.Length; i++){
					inputs.Add((double)curarray[i]);
					curstring = curstring + curarray[i].ToString() + ",";
				}	
			}
		}
		else{//ConvertToBitsIce
			int[] curarray = {0,0};
			switch (theval){
				case 1:
				curarray[0] = 1;
				break;
				case 4:
				curarray[1] = 1;
				break;
			}	
			if(curarray.Length!=0){
				for(int i =0; i< curarray.Length; i++){
					inputs.Add((double)curarray[i]);
					curstring = curstring + curarray[i].ToString() + ",";
				}	
			}	

		}

	}
	public static int[] ConvertToBitsIce(int theval){
		int[] curarray = {0,0,0,0,0};
		switch (theval){
			case 0:
			curarray[0] = 1;
			return curarray;
			case 1:
			curarray[1] = 1;
			return curarray;
			case 2:
			curarray[2] = 1;
			return curarray;
			case 3:
			curarray[3] = 1;
			return curarray;
			case 4:
			curarray[4] = 1;
			return curarray;
			default:
			return curarray;
		}
	}
}
