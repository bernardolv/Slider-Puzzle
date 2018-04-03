using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Brain : MonoBehaviour {
	public Vector3 initialposition;
	public Vector3 myposition;
	public Vector3 newposition;
	public BotMovement mybotcontroller;
	public DNA dna;
	public List <string> genes = new List <string>();
	public KeySimulator mykeysimulator;
	public List <Vector3> stoppedtiles = new List <Vector3>();
	public List<string> possibilities = new List <string> ();
	public string latestgene;
	public bool contains;
	public Vector3 myplace;
	public bool CanTakeCommand;
	//public Solvemethod mysolvemethod;
	//public bool pathtoleft;
	// Use this for initialization


	void Start () {
		PopulationManager.populationsize++;
		Debug.Log("+1");
		contains = false;
		//initialposition = transform.position;
		//stoppedtiles.Add (initialposition);
		if(stoppedtiles.Count == 0){
			Debug.Log ("No tiles");
			initialposition = transform.position;
			stoppedtiles.Add (initialposition);
		}
		if (PopulationManager.botnum == 0) {
			dna = new DNA ();
			genes = dna.genes;
			PopulationManager.botnum++;
			Debug.Log (genes.Count);
		}
		if (PopulationManager.botnum > 0) {
			//dna = new DNA ();
		}
		CanTakeCommand = true;

	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position == myplace){//conditions that Tells AI Brain it's ready to get working.
		CanTakeCommand = true;
		}
		myplace =transform.position;
		if (Input.GetKeyDown (KeyCode.I)) {
			//while(botsalive){
			AddifUnique();
			//}
		} 	
	}
	public void AddorBreak(){
		bool isrepeat = false;
		//Check stoppedtiles for repeats
		Vector3 mycurpos = transform.position;
		for(int i = 0; i < stoppedtiles.Count; i++){
			if (stoppedtiles[i]==mycurpos){
				isrepeat = true;
				DestroyProperly();
				Debug.Log("Destroyme");
			}
			else{

			}
		}
		if(isrepeat==false){
			stoppedtiles.Add(mycurpos);
			Debug.Log("Added to local");
		}
		//
	}
	public void AddifUnique(){
		Vector3 mynewV = transform.position;
		if(PopulationManager.UniqueStoppedTiles.Count == 0){
			Debug.Log("Empty");
			PopulationManager.UniqueStoppedTiles.Add(mynewV);
		}
		else{
			for(int i = 0; i <PopulationManager.UniqueStoppedTiles.Count; i++ ){
				Debug.Log(PopulationManager.UniqueStoppedTiles[i]);
				if(PopulationManager.UniqueStoppedTiles[i]==mynewV){
					contains = true;
					Debug.Log("isthere, wontadd");
				}
			}
			if(contains == false){
				PopulationManager.UniqueStoppedTiles.Add(mynewV);
				Debug.Log("added since it didn't contain");
			}
			//PopulationManager.UniqueStoppedTiles.ADD(mynewV);
		}
		contains = false;
		//for(int i = 0; i < )
	}
	public void AddtoLocalStopped(Vector3 newpos){
		for(int i = 0; i < stoppedtiles.Count; i++){
			if(newpos == stoppedtiles[i]){
				Debug.Log("IT exists");
			}
			//Find value.
		}
	}
	public void clonegene(List<string> dadgene){
		genes = dadgene;
	}
	public void givedna(DNA dna){
		dna = new DNA();
	}
	public void ActonGene(){
		//Debug.Log ("ACting on Gene");
		int genelength = genes.Count;
		int lastplace = genes.Count - 1;
		string newmove = genes[lastplace];
		Debug.Log ("have to move " + newmove);
		if (newmove == "Up") {
			mykeysimulator.W = true;
			Debug.Log ("Pressed UP");
		}
		if (newmove == "Right") {
			mykeysimulator.D = true;
			Debug.Log ("pressed RIGHT");
		}
		if (newmove == "Down") {
			mykeysimulator.S = true;
		}
		if (newmove == "Left") {
			mykeysimulator.A = true;
		}

	}
	public void DestroyProperly(){
		PopulationManager.populationsize--;
		Debug.Log("-1");
		Destroy(this.gameObject);
	}
	public void Findpossibilities(){
		PopulationManager.dadgenes = genes;
		Vector3 center = transform.position;
		//Debug.Log ("Will find Path starting at " + center);
		Vector3 left = center + Vector3.left;
		Vector3 right = center + Vector3.right;
		Vector3 up = center + Vector3.up;
		Vector3 down = center + Vector3.down;

		testwall (left, "Left");
		testwall (up, "Up");
		testwall (right, "Right");
		testwall (down, "Down");

	}
	/*public void CountTile(){
		Vector3 mypos = transform.position;
		if(stoppedtiles.Find(mypos)){
			Debug.Log("Already there");
		}
	}*/
	void testwall(Vector3 position, string direction){  //This tests one side, clones if free to move there.
		Debug.Log ("Trying to clone");
		Collider2D[] colliders = null;
		colliders = Physics2D.OverlapCircleAll(position, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
		if (colliders.Length == 0) {

		} else {
			foreach (Collider2D component in colliders) {
				if (component.tag == "Ground") {
					GameObject Ground = component.gameObject;
					TileHandler tilescript = Ground.GetComponent<TileHandler> ();
					if (tilescript.myTaker != null ) {
						GameObject Taker = tilescript.myTaker;
						if (Taker.tag == "Wall") {
							//Debug.Log ("Wall on left");
							//CreateClone();
						} else {
							Vector3 origin = transform.position;
							GameObject dad = this.gameObject;
							List <string> dadgene = genes;
							Debug.Log ("cloning " + direction);
							PopulationManager.Clone(origin, dad, dadgene, direction);
						}
					}
					else{
						//Debug.Log ("Move Left");
						Vector3 origin = transform.position;
						GameObject dad = this.gameObject;
						List <string> dadgene = genes;
						Debug.Log ("cloning " + direction);
						PopulationManager.Clone(origin, dad, dadgene, direction);
					}
				}
			}
		}
	}
}
