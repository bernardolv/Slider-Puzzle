using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {
	public Vector3 initialposition;
	public Vector3 myposition;
	public Vector3 newposition;
	public BotMovement mybotcontroller;
	public DNA dna;
	public KeySimulator mykeysimulator;
	// Use this for initialization


	void Start () {
		initialposition = transform.position;
		dna = new DNA();
		ActonGene ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			dna.RollnewGene ();
			ActonGene ();
		}
//		dna.genes.Clear();
	//	Debug.Log ("THE COUNT IS " + dna.genes.Count);
		//Debug.Log (dna.genes [dna.genes.Count]);
		//Debug.Log("random" + Random.Range(1,5));
	}
	void ActonGene(){
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
}
