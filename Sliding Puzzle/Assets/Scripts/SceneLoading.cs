using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoading : MonoBehaviour {
	public int num;
	public static GameObject gamewon;
	public static GameObject gamelost;
	public int testnum;

	public void LoadScene(int num){
		Swiping.mydirection = "Null";
		TurnCounter.turncount = 0;
		Debug.Log ("BOOP");
		LevelManager.levelnum = num;
		LevelManager.readytodraw = true;
		SceneManager.LoadScene(4);
	}
	public void LoadMenu(){
		SceneManager.LoadScene(0);
	}
	public void NextlevelButton(){
		Swiping.mydirection = "Null";
		LevelManager.levelnum++;
		LevelHandler.UnlockLevel (LevelManager.levelnum);
		LevelHandler.Lookfor (LevelManager.levelnum);
		TurnCounter.turncount = 0;
		LevelManager.NextLevel (LevelManager.levelnum);
	}
	public void PreviousLevelButton(){
		Swiping.mydirection = "Null";
		LevelManager.levelnum--;
		LevelHandler.Lookfor (LevelManager.levelnum);
		TurnCounter.turncount = 0;
		LevelManager.NextLevel (LevelManager.levelnum);
	}
	public void ResetLevelButton(){
		Swiping.mydirection = "Null";
		LevelHandler.Lookfor (LevelManager.levelnum);
		TurnCounter.turncount = 0;
		LevelManager.NextLevel (LevelManager.levelnum);
		IceTileHandler.GiveIce();
	}
	public void Testnum(int num){
		//initializevalues
		AIBrain.pieces.Clear();
		Swiping.mydirection = "Null";
		LevelManager.levelnum = num;		
		LevelHandler.Lookfor (num);
		TurnCounter.turncount = 0;
		LevelManager.NextLevel (num);
	}
	public void GoToWorld(int worldnumber){


		//LevelManager.worldnum = worldnumber;
		SceneManager.LoadScene(worldnumber);
		//change camera position depending on 
	}

	public void unlockAll(){
		LevelHandler.UnlockAllLevels();
	}

	public void LockAll(){
		LevelHandler.LockAllLevels();
	}

	public void GoToLevelSelect(){
		if (LevelManager.levelnum < 34) {
			SceneManager.LoadScene (1);
		} else if (LevelManager.levelnum < 67) {
			SceneManager.LoadScene (2);
		} else if (LevelManager.levelnum < 101) {
			SceneManager.LoadScene (3);
		}
	}

	public void GoToWorldSelect(){
			SceneManager.LoadScene (1);
	}
	void Update(){
//		Debug.Log (gamewon);
	}
}
