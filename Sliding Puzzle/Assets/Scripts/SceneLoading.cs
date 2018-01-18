using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour {
	public int num;

	public void LoadScene(int num){
		Debug.Log ("BOOP");
		LevelManager.levelnum = num;
		LevelManager.readytodraw = true;
		SceneManager.LoadScene(4);
	}
	public void LoadMenu(){
		SceneManager.LoadScene(0);
	}
	public void NextlevelButton(){
		LevelManager.levelnum++;
		LevelHandler.Lookfor (LevelManager.levelnum);
		TurnCounter.turncount = 0;
		LevelManager.NextLevel (LevelManager.levelnum);
	}
	public void PreviousLevelButton(){
		LevelManager.levelnum--;
		LevelHandler.Lookfor (LevelManager.levelnum);
		TurnCounter.turncount = 0;
		LevelManager.NextLevel (LevelManager.levelnum);
	}
	public void GoToWorld(int worldnumber){


		//LevelManager.worldnum = worldnumber;
		SceneManager.LoadScene(worldnumber);
		//change camera position depending on 
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
}
