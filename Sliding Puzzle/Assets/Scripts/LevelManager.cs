using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static int levelnum;
	public static FourbyFourLevels levelselector;
	//static int levelwidth;
	public static bool readytodraw;

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
		levelselector.DestroyAllExceptCamera ();
		levelselector.DrawIce ();
		levelselector.DrawNextLevel (mynum);
	}

}
