﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour {

	public void LoadScene(){
		Debug.Log ("BOOP");
		SceneManager.LoadScene(1);
	}
}
