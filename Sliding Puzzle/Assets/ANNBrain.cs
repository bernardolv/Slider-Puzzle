using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANNBrain : MonoBehaviour {
	public static ANN ann;
	public static double sol;//solution in bool
	public static double los;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static List<double> Run(int[,] map,double yn, bool train)
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
		SolveMethod.classifier = new int[] {0,0,0,0,0,0,0,0,0,0};
		SolveMethod.classifier[num] = 1;
	}
	public static List<double> RunV2(int[,] map, int solutionnum,bool train)
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
	public static double CalculateZ(int num){
		return (num-2)/1.58;
	}
}
