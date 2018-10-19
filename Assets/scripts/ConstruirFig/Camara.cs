using UnityEngine;
using UnityEngine.UI;
using System;
public class Camara : MonoBehaviour {
	WebCamTexture webCameraTexture;
	public GameObject webCameraPlane;

	void Start () {
		if (Application.isMobilePlatform){
			GameObject cameraParent = new GameObject ("camParent");
			cameraParent.transform.position = this.transform.position;
			this.transform.parent = cameraParent.transform;
			cameraParent.transform.Rotate (Vector3.right, 90);
		}
		Input.gyro.enabled = true;

		webCameraTexture = new WebCamTexture();
		webCameraPlane.GetComponent<MeshRenderer>().material.mainTexture = webCameraTexture;
		webCameraTexture.Play();	
	}

	// Update is called once per frame
	void Update () {
		Quaternion cameraRotation = new Quaternion (Input.gyro.attitude.x, Input.gyro.attitude.y,
			-Input.gyro.attitude.z, -Input.gyro.attitude.w);
		this.transform.localRotation = cameraRotation;
		if (GameObject.Find("lblStatus").GetComponent<Text>().text.Equals("Stop")){
			webCameraTexture.Stop ();
		}
	}
	public void Detener(){
		webCameraTexture.Stop ();
	}
}