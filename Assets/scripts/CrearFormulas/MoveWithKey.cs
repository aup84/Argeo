using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveWithKey : MonoBehaviour {
	public float speed;

	void Start(){
		speed = 30f;
	}
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			transform.Translate (new Vector3(-1,0,0) * speed * Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			transform.Translate (new Vector3 (1, 0, 0) * speed * Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			transform.Translate (new Vector3 (0, 0, 1) * speed * Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			transform.Translate (new Vector3 (0, 0, -1) * speed * Time.deltaTime);
		}
	}
}