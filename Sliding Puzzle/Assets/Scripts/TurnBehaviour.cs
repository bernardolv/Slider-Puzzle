using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBehaviour : MonoBehaviour {
	public static int turn;
	private static TurnBehaviour instance = null;


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
		turn = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
