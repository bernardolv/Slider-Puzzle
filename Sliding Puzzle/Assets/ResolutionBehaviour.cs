using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionBehaviour : MonoBehaviour {
	public static float leftX;
	public static float rightX;
	public static float upY;
	public static float downY;
	public static float xlength;
	public static float ylength;
	public static Vector3 upperleft;
	public static float originx;
	public static float originy;
	public static float unitx;
	public static float unity;
	// Use this for initialization
	void Start () {
		leftX = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).x;
		rightX = Camera.main.ViewportToWorldPoint(new Vector3(1,0,0)).x;
		upY = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)).y;
		downY = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).y;

		Debug.Log (leftX +"is left");
		Debug.Log (rightX);
		Debug.Log (upY +"is up");
		Debug.Log (downY);

		xlength = Mathf.Abs (leftX - rightX);
		ylength = Mathf.Abs (upY - downY);

		originx = leftX;
		originy = upY;

	}
	
	// Update is called once per frame
	void Update () {

		leftX = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).x;
		rightX = Camera.main.ViewportToWorldPoint(new Vector3(1,0,0)).x;
		upY = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)).y;
		downY = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).y;

		Debug.Log (leftX);
		Debug.Log (rightX);
		Debug.Log (upY);
		Debug.Log (downY);

		xlength = Mathf.Abs (leftX - rightX);
		ylength = Mathf.Abs (upY - downY);

		originx = leftX;
		originy = upY;


	}
}
