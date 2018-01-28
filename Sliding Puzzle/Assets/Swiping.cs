using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swiping : MonoBehaviour {

	public static string mydirection;
	public static Vector2 firstPressPos;
	public static Vector2 secondPressPos;
	public static Vector2 currentSwipe;
	public static bool canswipe;
	public static GameObject The_Dragged;


	public enum SwipeDirection{
		Up,
		Down,
		Right,
		Left
	}

	public static event Action<SwipeDirection> Swipe;
	public static bool swiping = false;
	private bool eventSent = false;
	private Vector2 lastPosition;
	void Start(){
		canswipe = true;
	}
	void Update () {
		//one ();
		two ();
	}
	void one(){
		if (Input.touchCount == 0) {
			mydirection = "None";
			return;
		}

		if (Input.GetTouch (0).deltaPosition.sqrMagnitude != 0) {
			if (swiping == false) {
				swiping = true;
				lastPosition = Input.GetTouch (0).position;
				return;
			} else {
				if (!eventSent) {
					if (Swipe != null) {
						Vector2 direction = Input.GetTouch (0).position - lastPosition;

						if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
							if (direction.x > 0) {
								Swipe (SwipeDirection.Right);
								mydirection = "Right";
							} else {
								Swipe (SwipeDirection.Left);
								mydirection = "Left";
							}
						} else {
							if (direction.y > 0) {
								Swipe (SwipeDirection.Up);
								mydirection = "Up";
							} else {
								Swipe (SwipeDirection.Down);
								mydirection = "Down";
							}
						}
						eventSent = true;
					}
				}
			}
		} else {
			swiping = false;
			eventSent = false;
			mydirection = "none1";
		}
		Debug.Log (mydirection);
	}
	void two(){

		if(Input.touches.Length > 0 && canswipe)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began)
			{
				//save began touch 2d point
				firstPressPos = new Vector2(t.position.x,t.position.y);
			}
			if(t.phase == TouchPhase.Ended)
			{
				//save ended touch 2d point
				secondPressPos = new Vector2(t.position.x,t.position.y);

				//create vector from the two points
				currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

				//normalize the 2d vector
				currentSwipe.Normalize();

				//swipe upwards
				if(currentSwipe.y > 0.5 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
					Debug.Log("up swipe");
					mydirection = "Up";
				}
				//swipe down
				if(currentSwipe.y < -0.5 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
					Debug.Log("down swipe");
					mydirection = "Down";
				}
				//swipe left
				if(currentSwipe.x < -0.5 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					Debug.Log("left swipe");
					mydirection = "Left";
				}
				//swipe right
				if(currentSwipe.x > 0.5 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					Debug.Log("right swipe");
					mydirection = "Right";
				}
			}
		}
	}
}