using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed_Behaviour : MonoBehaviour {
	public bool isseed;
	public string mytag;
	public Sprite small;
	public Sprite big;

	// Use this for initialization
	void Start () {
		isseed = true;


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Unseed(){
		isseed = false;
		this.gameObject.tag = mytag;
	}
}
