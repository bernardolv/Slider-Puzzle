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
		SceneManager.LoadScene(1);
	}
	public void LoadMenu(){
		SceneManager.LoadScene(0);
	}
	public void NextlevelButton(){
		LevelManager.levelnum++;
		LevelManager.NextLevel (LevelManager.levelnum);
	}
	public void PreviousLevelButton(){
		LevelManager.levelnum--;
		LevelManager.NextLevel (LevelManager.levelnum);
	}
}
