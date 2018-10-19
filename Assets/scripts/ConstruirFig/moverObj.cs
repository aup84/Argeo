using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moverObj : MonoBehaviour {
	
	float lerpTime = 7f;
	float currentLerpTime;
	float moveDistance = -5f;
	//float times; 
	Vector3 startPos, endPos, auxPos, auxStart;

	protected void Start() {
		startPos = transform.position;
		endPos = transform.position + transform.up * moveDistance;
	}

	protected void Update() {
//times = Time.deltaTime;
		//reset when we press spacebar
		if (transform.position.Equals (endPos)) {
			currentLerpTime = 0f;
			//transform.rotation = Quaternion.Euler (0, 0, 0);
			auxPos = endPos;
			endPos = startPos;
			startPos = auxPos;
		}

		//increment timer once per frame
		currentLerpTime += Time.deltaTime;
		if (currentLerpTime > lerpTime) {
			currentLerpTime = lerpTime;
		}

		//lerp!
		float perc = currentLerpTime / lerpTime;
		transform.position = Vector3.Lerp(startPos, endPos, perc);
			transform.Rotate(new Vector3(15f,15f,15f) * Time.deltaTime);
	}
}

/*
	void Update(){
		transform.Translate( 0, 0, 1.25f * Time.deltaTime );    // speed; see below
		if (transform.localPosition.z >= 10)              // at position 10...			
			transform.rotation = Quaternion.Euler(0,180,0);//face backward
		if (transform.localPosition.z <= 5)               // at position 0....
			transform.rotation = Quaternion.Euler(0,0,0);   // face forward
		if (transform.localPosition.z < 10 && transform.localPosition.z > 5){
			transform.Rotate(new Vector3(15f,15f,15f) * Time.deltaTime);
		}  
		
	}
}
*/