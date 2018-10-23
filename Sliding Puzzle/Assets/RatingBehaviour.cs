using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingBehaviour : MonoBehaviour {
	static float totturns;
	static float curturns;
	static float turnratio;
	public static int rating;
	private static RatingBehaviour instance = null;


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

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//CalculateRating ();
	}

	static public void CalculateRating(){
		totturns = LevelHandler.efficientturns;
		curturns = TurnCounter.turncount;
		turnratio = curturns / totturns;
		if (turnratio <= 1) {
			rating = 3;
		} else if (turnratio <= 2) {
			rating = 2;
		} else {
			rating = 1;
		}
		Debug.Log ("max " + totturns + "    cur " + curturns + "     rating " + rating + " " + turnratio);
	}
}
