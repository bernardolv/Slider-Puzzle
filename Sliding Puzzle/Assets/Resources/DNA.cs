using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA {
	public List <string> genes = new List<string> ();
	//public string direction;
	//bool dead = false;
	//public float timeToDie = 0;
	//SpriteRenderer sRenderer;
	//Collider2D sCollider;	

	public DNA(){
		genes.Clear();
		//RollnewGene ();
	}
	void OnMouseDown()
	{
		
	/*	dead = true;
		timeToDie = PopulationManager.elapsed;
		//Debug.Log("Dead At: " + timeToDie);
		sRenderer.enabled = false;
		sCollider.enabled = false;*/

	}	

	// Use this for initialization
	void Start () {
		//sRenderer = GetComponent<SpriteRenderer>();
		//sCollider = GetComponent<Collider2D>();	
		//sRenderer.color = new Color(r,b,g);	
		//this.transform.localScale = new Vector3(s,s,s);
	}

	// Update is called once per frame
	void Update () {

	}
	public void RerollLastGene(){
		int selection = Random.Range (1, 4);
		int Dnalength = genes.Count;
		string mynewdirection = null;
		if (selection == 1) {
			mynewdirection = "Left";
		}
		if (selection == 2) {
			mynewdirection = "Up";
		}
		if (selection == 3) {
			mynewdirection = "Right";
		}
		if (selection == 4) {
			mynewdirection = "Down";
		}
		if (Dnalength == 0) {
			genes [1] = mynewdirection;
		}
		if (Dnalength != 0) {
			genes [Dnalength] = mynewdirection;
		}
	}
	public void RollnewGene(){
		int selection = Random.Range (1, 5);
		string mynewdirection = null;
		if (selection == 1) {
			mynewdirection = "Left";
		}
		if (selection == 2) {
			mynewdirection = "Up";
		}
		if (selection == 3) {
			mynewdirection = "Right";
		}
		if (selection == 4) {
			mynewdirection = "Down";
		}
		Debug.Log (selection + "IS MY RAOMD");
		genes.Add (mynewdirection);
	}
	public void RollfirstGen(){
		int selection = Random.Range (1, 5);
		string mynewdirection = null;
		if (selection == 1) {
			mynewdirection = "Left";
		}
		if (selection == 2) {
			mynewdirection = "Up";
		}
		if (selection == 3) {
			mynewdirection = "Right";
		}
		if (selection == 4) {
			mynewdirection = "Down";
		}
		genes[1] = (mynewdirection);
	}
	public void cloneleft(){

	}
}
