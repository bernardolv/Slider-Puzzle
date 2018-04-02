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
	public static bool areallready;
	public static List<int> botready;
	public static bool turnstarted;
	public static int populationsize;
	//public DNA dna;
	void Start () {
		populationsize = 0;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(areallready&&turnstarted){
			AddifUnique();
			turnstarted =false;
		}*/

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
			thisbrain.DestroyProperly();
		}
	}
	public void AddifUnique(){
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Brain thisbrain = player.GetComponent<Brain> ();
			thisbrain.AddifUnique ();
		}
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
		Debug.Log("Moving");
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Brain thisbrain = player.GetComponent<Brain> ();
			Debug.Log("Bout to act");
			thisbrain.ActonGene ();
		}
	}
	public void AWholeturn(){
		NextAct();
		StartCoroutine(Turn(2));

	}
	public void GatherdatafromAll(){
		foreach(GameObject ice in GameObject.FindGameObjectsWithTag("Ground")){
			TileProperties myproperties = ice.GetComponent<TileProperties>();
			myproperties.GatherData();
		}
	}
	public void KillifStuck(){
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Brain thisbrain = player.GetComponent<Brain> ();
			thisbrain.AddorBreak();
		}
	}

	public IEnumerator Turn (int sec){

		yield return new WaitForSeconds(sec);
		MovenewClones();
		areallready = false;
		turnstarted = true;
		yield return new WaitForSeconds(sec);
		GatherdatafromAll();
		AddifUnique();
		KillifStuck();

	}
	public void DestroyStuckBots(){

	}
}
