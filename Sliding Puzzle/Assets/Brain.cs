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
	public KeySimulator mykeysimulator;
	public List <Vector3> stoppedtiles = new List <Vector3>();
	public List<string> possibilities = new List <string> ();
	public bool pathtoleft;
	// Use this for initialization


	void Start () {
		//initialposition = transform.position;
		//stoppedtiles.Add (initialposition);
		if(stoppedtiles.Count == 0){
			Debug.Log ("No tiles");
			initialposition = transform.position;
			stoppedtiles.Add (initialposition);
		}
		if (PopulationManager.botnum == 0) {
			dna = new DNA ();
			dna.genes.Add ("Left");
			ActonGene ();
			PopulationManager.botnum++;
		}
		if (PopulationManager.botnum > 0) {
			dna = new DNA ();
		}
		//Debug.Log("DNA SIZE " + dna.genes.Count);
		//dna.addGene();	
		//ActonGene ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			Findpossibilities ();
			//dna.Clone(possibilities);
			//popoulationmanager.clone(possibilities);
			//dna.RollnewGene ();
			//ActonGene ();
		} 	

//		dna.genes.Clear();
	//	Debug.Log ("THE COUNT IS " + dna.genes.Count);
		//Debug.Log (dna.genes [dna.genes.Count]);
		//Debug.Log("random" + Random.Range(1,5));
	}
	public void clonegene(List<string> dadgene){
		dna.genes = dadgene;
	}
	public void givedna(DNA dna){
		dna = new DNA();
	}
	public void ActonGene(){
		int lastplace = dna.genes.Count - 1;
		string newmove = dna.genes[lastplace];
		if (newmove == "Up") {
			mykeysimulator.W = true;
		}
		if (newmove == "Right") {
			mykeysimulator.D = true;
		}
		if (newmove == "Down") {
			mykeysimulator.S = true;
		}
		if (newmove == "Left") {
			mykeysimulator.A = true;
		}

	}
	public void Findpossibilities(){
		PopulationManager.dadgenes = dna.genes;
		Vector3 center = transform.position;
		Debug.Log ("Will find Path starting at " + center);
		Vector3 left = center + Vector3.left;
		Vector3 right = center + Vector3.right;
		Vector3 up = center + Vector3.up;
		Vector3 down = center + Vector3.down;

		testwall (left, "Left");
		testwall (right, "Right");
		Debug.Log ("Gonna do up next");
		testwall (up, "Up");
		testwall (down, "Down");

	}
	void testwall(Vector3 position, string direction){
		Debug.Log ("Trying to clone");
		Collider2D[] colliders = null;
		colliders = Physics2D.OverlapCircleAll(position, .1f); ///Presuming the object you are testing also has a collider 0 otherwise{
		if (colliders.Length == 0) {

		} else {
			foreach (Collider2D component in colliders) {
				if (component.tag == "Ground") {
					GameObject Ground = component.gameObject;
					TileHandler tilescript = Ground.GetComponent<TileHandler> ();
					if (tilescript.myTaker != null) {
						GameObject Taker = tilescript.myTaker;
						if (Taker.tag == "Wall") {
							//Debug.Log ("Wall on left");
							//CreateClone();
						}
					}
					else{
						//Debug.Log ("Move Left");
						Vector3 origin = transform.position;
						GameObject dad = this.gameObject;
						List <string> dadgene = dna.genes;
						Debug.Log ("cloning " + direction);
						PopulationManager.Clone(origin, dad, dadgene, direction);
					}
				}
			}
		}
	}
}
