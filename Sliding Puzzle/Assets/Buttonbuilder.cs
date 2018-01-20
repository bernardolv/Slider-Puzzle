using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttonbuilder : MonoBehaviour {

	public GameObject Button;
	float posx;
	float posy;
	float unitx;
	float unity;
	int levels;
	int counter;
	public Transform CanvasParent;
	private static Buttonbuilder instance = null;


	// Use this for initialization
	/*void Awake(){
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
			return;
		}
		Destroy(this.gameObject);
	}*/
	// Use this for initialization

	void Start () {
		CanvasParent = GameObject.Find ("Canvas").transform;
		GameObject Button = Instantiate(Resources.Load("LevelButton", typeof(GameObject))) as GameObject;
		levels = 33;
		counter = 0;
		while (counter<=levels){
			for(int y = 1; y<8; y++){

				for (int x = 1; x < 6; x++) {
					counter++;
					unitx = ResolutionBehaviour.xlength / 6;
					unity = ResolutionBehaviour.ylength / 8;
					float posx = ResolutionBehaviour.originx + unitx * x;
					float posy = ResolutionBehaviour.originy - unity * y;
					Debug.Log (posx);
					var newbutton = Instantiate (Button, new Vector3 (posx, posy, 0), Quaternion.identity);
					newbutton.transform.SetParent(CanvasParent);
					Vector3 newscale = new Vector3 (1, 1, 1);
					newbutton.transform.localScale = newscale;
					LevelButton buttonscript = newbutton.GetComponent<LevelButton> ();
					buttonscript.mylevelnumber = counter;
					if (counter == levels) {
						return;
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		Debug.Log (counter);
	}
	/*void OnEnable () {
		CanvasParent = GameObject.Find ("Canvas").transform;
		GameObject Button = Instantiate(Resources.Load("LevelButton", typeof(GameObject))) as GameObject;
		levels = 33;
		counter = 0;
		while (counter<=levels){
			for(int y = 1; y<8; y++){

				for (int x = 1; x < 6; x++) {
					counter++;
					unitx = ResolutionBehaviour.xlength / 6;
					unity = ResolutionBehaviour.ylength / 8;
					float posx = ResolutionBehaviour.originx + unitx * x;
					float posy = ResolutionBehaviour.originy - unity * y;
					Debug.Log (posx);
					var newbutton = Instantiate (Button, new Vector3 (posx, posy, 0), Quaternion.identity);
					newbutton.transform.SetParent(CanvasParent);
					Vector3 newscale = new Vector3 (1, 1, 1);
					newbutton.transform.localScale = newscale;
					LevelButton buttonscript = newbutton.GetComponent<LevelButton> ();
					buttonscript.mylevelnumber = counter;
					if (counter == levels) {
						return;
					}
				}
			}
		}
	}*/
}
