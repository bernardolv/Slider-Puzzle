using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTileHandler : MonoBehaviour {
	public Sprite[] vanillasprites;
	public Sprite leftuppercorner;
	public Sprite leftbottomcorner;
	public Sprite rightuppercorner;
	public Sprite rightbottomcorner;
	public Sprite[] leftwalls; 
	public Sprite[] upperwalls;
	public Sprite[] bottomwalls;
	public Sprite[] rightwalls;
	public Sprite verticalpath;
	public Sprite horizontalpath;

	// Use this for initialization
	void Start () {
		//LevelManager.myicehandler = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void GiveIce(){
		Debug.Log("givingice");
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Ground");
		foreach (GameObject icetile in tiles){
			if(icetile.GetComponent<TileHandler>().isTaken == true){
				Debug.Log("Taken");
			}
		}

	}
	public static void Checksides(){

	}

}
