using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using System;
using System.IO;

public class AreaFiguras : MonoBehaviour, ITrackableEventHandler{
	protected TrackableBehaviour mTrackableBehaviour;
	Text txtTarget, txtFind, txtBase, txtAltura, lblObjetivo, lblApotema, lblBase, txtApotema, txtAviso;
	ReglasJuego4 reglas;
	RawImage raw2;
	Texture2D textura;

	protected virtual void Start(){
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		txtTarget = GameObject.Find ("txtTarget").GetComponent<Text> ();
		txtFind = GameObject.Find ("txtFind").GetComponent<Text> ();
		txtAviso = GameObject.Find ("txtAviso").GetComponent<Text> ();
		lblObjetivo = GameObject.Find ("lblObjetivo").GetComponent<Text> ();
		reglas = new ReglasJuego4 ();
		raw2 = GameObject.Find ("RawPausa").GetComponent<RawImage> ();
		textura = new Texture2D (150, 150, TextureFormat.RGBA32, false);
	}


	public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus){
		if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED ||
		    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
			GameObject.Find ("pnlPausa").SetActive (true);
			GameObject.Find ("pnlPausa").GetComponent<RectTransform> ().localScale = Vector3.zero;
			if (reglas.getLista ().Count > 0) {
				Debug.Log ("Trackable " + mTrackableBehaviour.TrackableName + " found");
				OnTrackingFound ();

				string obj1 = reglas.getDescripcion (mTrackableBehaviour.TrackableName);
				txtFind.text = obj1;

				if (!obj1.Equals (txtTarget.text)) {
					GameObject.Find ("pnlPausa").SetActive (true);
					GameObject.Find ("pnlPausa").GetComponent<RectTransform> ().localScale = new Vector3 (0.3f, 0.5f, 1f);
					MostrarPanel ();
				}
			}
		}
		else if (previousStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.NO_POSE) {
			Debug.Log ("Trackable " + mTrackableBehaviour.TrackableName + " lost");
			OnTrackingLost ();

			string obj1 = reglas.getDescripcion (mTrackableBehaviour.TrackableName);
			txtFind.text = obj1;
			GameObject.Find ("txtStatus").GetComponent<Text> ().text = "";

			if (obj1.Equals (txtTarget.text)) {
				GameObject.Find ("pnlValidar").GetComponent<RectTransform> ().localScale = new Vector3 (0.3f, 0.4f, 1f);
				GameObject.Find ("pnlDatos").GetComponent<RectTransform> ().localScale = new Vector3 (1f, 0.05f, 1f);
				lblObjetivo.text = "Escribe en el cuadro de texto el resultado del cálculo del volumen y presiona Aceptar";
			}

		else{
			OnTrackingLost();
		}
	}
	}
		
	protected virtual void OnTrackingFound(){
		var rendererComponents = GetComponentsInChildren<Renderer>(true);
		var colliderComponents = GetComponentsInChildren<Collider>(true);
		var canvasComponents = GetComponentsInChildren<Canvas>(true);

		// Enable rendering:
		foreach (var component in rendererComponents)
			component.enabled = true;

		// Enable colliders:
		foreach (var component in colliderComponents)
			component.enabled = true;

		// Enable canvas':
		foreach (var component in canvasComponents)
			component.enabled = true;
	}


	protected virtual void OnTrackingLost(){
		var rendererComponents = GetComponentsInChildren<Renderer>(true);
		var colliderComponents = GetComponentsInChildren<Collider>(true);
		var canvasComponents = GetComponentsInChildren<Canvas>(true);

		// Disable rendering:
		foreach (var component in rendererComponents)
			component.enabled = false;

		// Disable colliders:
		foreach (var component in colliderComponents)
			component.enabled = false;

		// Disable canvas':
		foreach (var component in canvasComponents)
			component.enabled = false;
	}

	public void MostrarPanel(){
		GameObject.Find("pnlPausa").GetComponent<RectTransform> ().localScale.Set (0.3f, 0.5f, 1f);
		txtAviso.text = "La figura mostrada es incorrecta, favor de colocar otro marcador \t";

		if (Application.isMobilePlatform) {
			string ruta = Application.streamingAssetsPath + "/images/incorrecto.png";

			WWW www = new WWW (ruta);
			while (!www.isDone) {
			}
			www.LoadImageIntoTexture (textura);
		}
		else {
			textura.LoadImage(File.ReadAllBytes(Application.dataPath + "/images/incorrecto.png"));
		}
		raw2.texture = textura;
		Text txtErrores = GameObject.Find ("txtErrores").GetComponent<Text> ();
		int errores = int.Parse (txtErrores.text);
		errores++;
		txtErrores.text = errores.ToString ();
	}
}