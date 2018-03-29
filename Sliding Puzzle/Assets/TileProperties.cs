using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperties : MonoBehaviour {
	public TileHandler myhandler;
	public string mytag;
	public GameObject taker;
	public int myposx;
	public int myposy;
	bool datagathered;
	// Use this for initialization
	void Start () {
		datagathered = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
		GatherData();
		} 	
		if(datagathered && taker != myhandler.myTaker){
			GatherData();

		}
	}
	public void GatherData(){
		taker = myhandler.myTaker;
		Vector3 mypos = transform.position;
		myposx = (int)mypos.x;
		myposy = (int)mypos.y;
		int flipy = Mathf.Abs(myposy);
		if(taker != null){
		mytag = taker.tag;
		AIBrain.tiles[myposx,flipy] = mytag;
		}
		else{
			mytag = "Ice";
			AIBrain.tiles[myposx,flipy] = mytag;
			//Debug.Log("Gave Ice");
		}
		if(mytag == "Goal"){
			GameObject goalobject = this.gameObject;
			AIBrain.goaltile = goalobject;
			AIBrain.Goalx = myposx;
			AIBrain.Goaly = flipy;
		}
		datagathered = true;
	}

}
