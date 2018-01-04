using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStreamingAssetsPath : MonoBehaviour {

string dbPath = "";

string realPath = "" ;

	public string levelname;
	public string leveltext;

void GetPath(string levelname, string leveltext){
	if (Application.platform == RuntimePlatform.Android){
		string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, leveltext);
  
  // Android only use WWW to read file
  		WWW reader = new WWW(oriPath);
 		while ( ! reader.isDone) {}
  
		realPath = Application.persistentDataPath + "/" + levelname;
		System.IO.File.WriteAllText(realPath, reader.text);
  
  		dbPath = realPath;
		}
	else{
  // iOS
		dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, leveltext);
		}
	}
}