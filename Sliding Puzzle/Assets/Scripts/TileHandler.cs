using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour {

	public bool isTaken;
	public GameObject myTaker;
	public Vector3 myTakerpos;
	// Use this for initialization
	void Start () {
		isTaken = false;
	}
	
	// Update is called once per frame
	void Update () {
	//		myTakerpos = myTaker.transform.position;
		//when mytaker is not inmytile
		if (myTaker != null && (myTaker.transform.position.x != transform.position.x || myTaker.transform.position.y != transform.position.y)){
			Debug.Log("Leaving" + transform.position);
			//Debug.Log (myTaker.transform.position ==transform.position);
			//Debug.Log (transform.position);
			isTaken = false;
			myTaker = null;

		}
	}
}
