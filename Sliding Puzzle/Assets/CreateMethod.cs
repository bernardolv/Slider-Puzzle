using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMethod : MonoBehaviour {
	public static string [,] generatedmap = new string [8,8];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public static void Createfourbyfour(){
		CreateIce();

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

		}
	}
}
