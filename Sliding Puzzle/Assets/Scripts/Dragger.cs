﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dragger : MonoBehaviour {
	private Vector3 screenPoint; 
	private Vector3 offset; 
	private float _lockedYPosition;
	public GameObject tentativetile;
	public float mouseTilePosX;
	public float mouseTilePosY;

	void Update(){
		Ray ();

	}

	 void OnMouseDown() {
    //screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position); // I removed this line to prevent centring 
   // _lockedYPosition = screenPoint.y;
    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    Cursor.visible = false;
     
     
           
 }
 
 void OnMouseDrag() 
 { 
    Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
   // curPosition.x = _lockedYPosition;
    transform.position = curPosition;
 }
 
 void OnMouseUp()
 {
    Cursor.visible = true;
 }
/*void Ray(){
		RaycastHit2D hitPoint;
		Ray2D ray = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
		if (Physics2D.Raycast(ray, out hitPoint, Mathf.Infinity)) {
			if (hitPoint.collider.tag == "Ground") {
				tentativetile = hitPoint.transform.gameObject;
				mouseTilePosX = tentativetile.transform.position.x;
				mouseTilePosY = tentativetile.transform.position.x;
				Debug.Log (tentativetile);
			}
		}
	}*/
}