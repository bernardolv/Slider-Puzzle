using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCounter : MonoBehaviour {
	public int bestturns;
	public static int turncount;
	private static TurnCounter instance = null;


	// Use this for initialization
	void Awake(){
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
			return;
		}
		Destroy(this.gameObject);
	}
	// Use this for initialization
	void Start () {
		turncount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
