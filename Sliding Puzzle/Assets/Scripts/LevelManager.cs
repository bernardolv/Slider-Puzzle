using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static int levelnum;
	public static FourbyFourLevels levelselector;
	//static int levelwidth;
	public static bool readytodraw;
	public static int worldnum;
	public static int myefficientturns;
	public static IceTileHandler myicehandler;


	private static LevelManager instance = null;



	void Awake(){
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
			return;
		}
		Destroy(this.gameObject);
	}

	public static void NextLevel(int mynum){
		Debug.Log("GONNAGETICE");
		LevelHandler.Lookfor (mynum);
		TurnCounter.turncount = 0;
		levelselector.DestroyAllExceptCamera ();
		levelselector.DrawIce ();
		//Debug.Log("GONNAGETICE");

		levelselector.DrawNextLevel (mynum);
//		Debug.Log("GONNAGETICE");
//		myicehandler.GiveIce();
	}
	void Update(){
	}
}
