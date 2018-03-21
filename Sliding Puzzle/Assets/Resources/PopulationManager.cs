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
	public
	//public DNA dna;
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
	public void NextAct() {
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Brain thisbrain = player.GetComponent<Brain> ();
			thisbrain.Findpossibilities ();
			Destroy (player);
		}
	}
	public void NewMove(){
		
	}
	public static void Clone(Vector3 origin, GameObject mydad, List<string> originalgenes, string newgene){
		float x = mydad.transform.position.x;
		float y = mydad.transform.position.y;
		GameObject newbot = Instantiate	(mydad, new Vector3 (x, y, 0), mydad.transform.rotation);
		Debug.Log ("NEWBOT");
		Brain newbrain = newbot.GetComponent<Brain> ();
		//newbrain.enabled = true;
		newbrain.genes.Add(newgene);
		BotMovement botm = newbot.GetComponent<BotMovement> ();
		botm.myturns = newbot.GetComponentInChildren<BotTurns> ();

		//newbrain.ActonGene ();
		//int genelength = newbrain.genes.Count;
		//Debug.Log("Last gene is " + newbrain.dna.genes[genelength-1]);
		//newbrain.ActonGene ();

		//newbrain.ActonGene ();
		//Brain newbrain = newbot.GetComponent<Brain>();
		//newbrain.enabled = true;
		//string newgene = "Left";
		//List<string> newgenes = originalgenes;
//		newgenes.Add(newgene);
		//newbrain.clonegene (newgenes);
		//newbrain.ActonGene ();

	}
	public void MovenewClones(){
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Brain thisbrain = player.GetComponent<Brain> ();
			thisbrain.ActonGene ();
			//thisbrain.checkrepeatedtile();
		}
	}
}
