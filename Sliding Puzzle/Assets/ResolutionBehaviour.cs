using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionBehaviour : MonoBehaviour {
	static float leftX;
	static float rightX;
	static float upY;
	static float downY;
	// Use this for initialization
	void Start () {
		leftX = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).x;
		rightX = Camera.main.ViewportToWorldPoint(new Vector3(1,0,0)).x;
		upY = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).y;
		downY = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)).y;


	}
	
	// Update is called once per frame
	void Update () {

		/*leftX = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).x;
		rightX = Camera.main.ViewportToWorldPoint(new Vector3(1,0,0)).x;
		upY = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).y;
		downY = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)).y;
*/
	}
}
