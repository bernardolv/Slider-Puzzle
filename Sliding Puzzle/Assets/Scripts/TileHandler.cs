using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour {

	public bool isTaken;
	public GameObject myTaker;
	public Vector3 myTakerpos;
	public TileProperties myproperties;
	// Use this for initialization
	void Start () {
		isTaken = false;
		myTaker = null;
	}
	
	// Update is called once per frame
	/*void Update () {
		if (myTaker != null && (myTaker.transform.position.x != transform.position.x || myTaker.transform.position.y != transform.position.y)){
			Debug.Log("Leaving" + transform.position);

			isTaken = false;

			myTaker = null;
		}

	}*/	
}
