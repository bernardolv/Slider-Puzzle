using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour {
	public static bool readytobrain;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
/*		Debug.Log ("ARE YOU READY O BRAIN" + readytobrain);
		if (readytobrain == true) {
			foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){
				Brain thisbrain = player.GetComponent<Brain> ();
				thisbrain.enabled = true;
				Debug.Log ("TURNINGITON");
			}
		
		}*/
	}
	public void turnOnBrain() {
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Brain thisbrain = player.GetComponent<Brain> ();
			thisbrain.enabled = true;
			Debug.Log ("TURNINGITON");
		}
	}
}
