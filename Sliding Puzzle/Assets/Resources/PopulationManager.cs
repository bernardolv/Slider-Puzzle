using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour {
	public static bool readytobrain;
	public GameObject bot;
	public static int botnum = 0;
	public static List<string> dadgenes = new List<string>();
	public static List<Vector3> UniqueStoppedTiles = new List<Vector3>();
	//public DNA dna;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void turnOnBrain() {
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Brain thisbrain = player.GetComponent<Brain> ();
			thisbrain.enabled = true;
			Debug.Log ("TURNINGITON");
		}
	}
	public void NextAct() {//clones everyone to available sides
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Brain thisbrain = player.GetComponent<Brain> ();
			thisbrain.Findpossibilities ();
			Destroy (player);
		}
	}
	public void NewMove(){
		
	}
	public static void Clone(Vector3 origin, GameObject mydad, List<string> originalgenes, string newgene){//function that clones with right settings
		float x = mydad.transform.position.x;
		float y = mydad.transform.position.y;
		GameObject newbot = Instantiate	(mydad, new Vector3 (x, y, 0), mydad.transform.rotation);
		Debug.Log ("NEWBOT");
		Brain newbrain = newbot.GetComponent<Brain> ();
		newbrain.genes.Add(newgene);
		BotMovement botm = newbot.GetComponent<BotMovement> ();
		botm.myturns = newbot.GetComponentInChildren<BotTurns> ();
	}
	public void MovenewClones(){
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Brain thisbrain = player.GetComponent<Brain> ();
			thisbrain.ActonGene ();
		}
	}
	public void DestroyStuckBots(){

	}
}
