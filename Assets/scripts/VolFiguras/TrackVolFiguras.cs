using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using System;
using System.IO;

public class TrackVolFiguras : MonoBehaviour, ITrackableEventHandler{
	protected TrackableBehaviour mTrackableBehaviour;
	Text txtTarget, txtFind, txtBase, txtAltura, lblObjetivo, lblApotema, lblBase, txtApotema, txtAviso;
	ReglasJuego reglas;
	RawImage raw2;
	Texture2D textura;

	protected virtual void Start(){
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		txtTarget = GameObject.Find ("txtTarget").GetComponent<Text> ();
		txtFind = GameObject.Find ("txtFind").GetComponent<Text> ();
		txtBase = GameObject.Find ("txtBase").GetComponent<Text> ();
		txtAviso = GameObject.Find ("txtAviso").GetComponent<Text> ();
		txtAltura = GameObject.Find ("txtAltura").GetComponent<Text> ();
		txtApotema = GameObject.Find ("txtApotema").GetComponent<Text> ();
		lblApotema = GameObject.Find ("lblApotema").GetComponent<Text> ();
		lblObjetivo = GameObject.Find ("lblObjetivo").GetComponent<Text> ();
		lblBase = GameObject.Find ("lblBase").GetComponent<Text> ();
		txtFind = GameObject.Find ("txtFind").GetComponent<Text> ();
        reglas = ReglasJuego.Instance;
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

				string obj1 = reglas.GetDescripcion (mTrackableBehaviour.TrackableName);
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

			string obj1 = reglas.GetDescripcion (mTrackableBehaviour.TrackableName);
			txtFind.text = obj1;
			GameObject.Find ("txtStatus").GetComponent<Text> ().text = "";

			if (obj1.Equals (txtTarget.text)) {
				GameObject.Find ("pnlValidar").GetComponent<RectTransform> ().localScale = new Vector3 (0.3f, 0.4f, 1f);
				GameObject.Find ("pnlDatos").GetComponent<RectTransform> ().localScale = new Vector3 (1f, 0.05f, 1f);
				txtBase.text = Juego.getVolumen (txtTarget.text, 1).ToString ();
				txtAltura.text = Juego.getVolumen (txtTarget.text, 2).ToString ();
				GameObject.Find ("lblAltura").GetComponent<RectTransform> ().localScale = Vector3.zero;
				GameObject.Find ("txtAltura").GetComponent<RectTransform> ().localScale = Vector3.zero;
				lblObjetivo.text = "Escribe en el cuadro de texto el resultado del cálculo del Área y presiona Aceptar";

                txtApotema.text = Juego.getVolumen(txtTarget.text, 3).ToString();
                if (mTrackableBehaviour.TrackableName.Contains("Cil")){
                    lblBase.text = "Radio";
                    lblApotema.text = "Pi";
                }
                else if (mTrackableBehaviour.TrackableName.Contains("Triang"))
                {
                    lblApotema.text = "Altura";
                    lblBase.text = "Base";
                }
                else if (mTrackableBehaviour.TrackableName.Contains("gonal"))
                {
                    lblBase.text = "Lado";
                    lblApotema.text = "Apotema";
                }
                else if (mTrackableBehaviour.TrackableName.Contains("Cuadra"))
                {
                    lblBase.text = "Lado";
                    lblApotema.text = "Lado";
                }                
			} 
		}

		else{
			OnTrackingLost();
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

		string arch = "incorrecto.png";
		txtAviso.text = "La figura mostrada es incorrecta, favor de colocar otro marcador \t";

		if (Application.isMobilePlatform) {
			string ruta = Application.streamingAssetsPath + "/images/" + arch; 

			WWW www = new WWW (ruta);
			while (!www.isDone) {
			}
			www.LoadImageIntoTexture (textura);
		}
		else {
			textura.LoadImage(File.ReadAllBytes(Application.dataPath + "/images/" + arch));
		}
		raw2.texture = textura;
	}
}