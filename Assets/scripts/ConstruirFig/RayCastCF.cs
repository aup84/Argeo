﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class RayCastCF : MonoBehaviour {
	string target;
	int encontradas, consecutivas, puntos, errores, final;
	Text txtTarget, lblStatus, txtFind, txtPuntos, txtConsecutivas, txtTiempo, txtAviso;
	CuerposInfo inf;
	RawImage raw2, imgObjetivo;
	GameObject pnlPausa;
	double tiempo;
	bool quitadas = false;

	void Start () {				// Use this for initialization
		tiempo = 0d;
		raw2 = GameObject.Find ("RawPausa").GetComponent<RawImage> ();
		imgObjetivo = GameObject.Find ("imgObjetivo").GetComponent<RawImage> ();
		inf = new CuerposInfo ();
		txtTiempo = GameObject.Find ("txtTiempo").GetComponent<Text> ();
		txtTarget = GameObject.Find ("txtTarget").GetComponent<Text> ();
		txtFind = GameObject.Find ("txtFind").GetComponent<Text> ();
		target = inf.getNextItem ();
		txtTiempo.text = "0";
		//txtTarget.text = inf.getDescripcion (target);
		txtTarget.text = inf.getNextItemCorte();
		CambiarImgObj ();
		//CambiarImg (target);
		Debug.Log ("El objetivo es: " + target);
		pnlPausa = GameObject.Find("pnlPausa");
		pnlPausa.GetComponent<RectTransform> ().localScale = Vector3.zero;
		StartCoroutine ("ControlTiempo");
		MostrarFigurasObj ();
	}

	public void MostrarFigurasObj(){
		string tar = target.Substring (0, 3);
        Debug.Log("Tar: " + tar);
		GameObject [] gmeO = GameObject.FindGameObjectsWithTag ("Mov1");
		foreach (GameObject gme in gmeO){
			if (gme.name.Contains (tar)) {
				gme.GetComponent<moverObj> ().enabled = true;
				gme.GetComponent<MeshRenderer> ().enabled = true;
				gme.GetComponent<Transform> ().localScale = Vector3.one;
			} 
		}
	}

	public void DestruirCilindros(){
		GameObject [] gmeO = GameObject.FindGameObjectsWithTag ("Mov1");
		foreach (GameObject gme in gmeO){
			if (gme.name.Contains ("Cil")) {
				Destroy (gme);
			} 
		}
	}

	IEnumerator ControlTiempo(){
		try {
			tiempo = double.Parse (txtTiempo.text, System.Globalization.NumberStyles.Any);
			tiempo += Time.deltaTime;
			txtTiempo.text = "" + tiempo.ToString ("F");
		} 
		catch (Exception e) {
			Debug.Log (e.Message);
		}
		yield return new WaitForSeconds (0);
	}
	
	public void PausarTiempo(){
		StopCoroutine ("ControlTiempo");
	}
	
	void Update () {			// Update is called once per frame
		if (final >= 6) {
			PausarTiempo ();
		}
		else{
			StartCoroutine ("ControlTiempo");
		}

	
		if (Input.GetMouseButtonDown (0)) {
			Ray rayOrigin = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast (rayOrigin, out hitInfo)) {
				Debug.DrawLine (transform.position, hitInfo.point, Color.red, 0.5f);
				string nom = hitInfo.collider.gameObject.name;
				string nom2 = nom = nom.Substring (0, nom.Length - 1);

				if (!nom.Equals ("GUI") && !nom.Equals ("Plane")) {
					txtFind.text = inf.getDescripcion(nom2);
					Debug.Log ("Detectado: " + hitInfo.collider.gameObject.name + ", Objetivo: "+ target);
					if (target.Equals (nom)) {
						consecutivas++;
						Debug.Log ("Encontrada la parte " + consecutivas + " de 2 " + nom + ". Final " + final);
						puntos += 100;
						hitInfo.collider.gameObject.GetComponent<Transform> ().localScale = Vector3.zero;

						inf.getDescripcion (nom2);
						if (consecutivas == 1) {
							imgObjetivo.GetComponent<RectTransform> ().localScale = new Vector3 (4f, 3f, 4f);
							CambiarImg (target);
						}	

						if (consecutivas == 2) {	
							encontradas++;
							GameObject.Find ("txtAciertos").GetComponent<Text> ().text = encontradas.ToString ();	
							GameObject.Find (nom + "A").GetComponent<Transform> ().localScale = Vector3.one;
							GameObject.Find (nom + "B").GetComponent<Transform> ().localScale = Vector3.one;
							MostrarPanel (true);
							txtAviso.text = "Lo lograste, ahora avanza al siguiente objetivo";
							txtAviso.text = txtAviso.text + " " + txtTarget.text;
							inf.QuitarElemento (nom);
							target = inf.getNextItem ();
							if (!target.Contains("Cil") && !quitadas){
								quitadas = true;
								DestruirCilindros();
							}
							txtTarget.text = inf.getNextItemCorte();
							Debug.Log ("Cambiaremos de Objetivo a " + target);
							CambiarImg ("nada");
							imgObjetivo.GetComponent<RectTransform> ().localScale = Vector3.zero;
							CambiarImgObj ();
							consecutivas = 0;
							final++;
							MostrarFigurasObj ();
							PausarJuego ();
						}
					} 
					else {						
						puntos -= 100;
                        errores = Convert.ToInt32(GameObject.Find("txtErrores").GetComponent<Text>().text);
                        errores++;
                        GameObject.Find("txtErrores").GetComponent<Text>().text = errores.ToString();

                        consecutivas = 0;     
						MostrarPanel (false);
						txtAviso = GameObject.Find ("txtAviso").GetComponent<Text> ();
						txtAviso.text = "Incorrecto, se te pide que encuentres los fragmentos para armar la figura ";
						txtAviso.text = txtAviso.text + " " + inf.getNextItemFigura ();// + " " + txtTarget.text;
						Debug.Log ("A:  " + nom  + "A" + ", " + nom + "B");
						PausarJuego ();
						VerAyuda(txtTarget.text, target);
					}

					if (final > 5) {
                        int aciertos = Convert.ToInt32(GameObject.Find("txtAciertos").GetComponent<Text>().text);
                        int err = Convert.ToInt32(GameObject.Find("txtErrores").GetComponent<Text>().text);
                        Debug.Log("Errores " + err);                        
                        GuardarDatos g = new GuardarDatos("3", aciertos, err, puntos, tiempo, 1, "Facil");
                        g.Insert();
						txtTarget.text = "FIN DEL JUEGO";
						Debug.Log ("Fin del juego");
						GameObject  [] gmeO = GameObject.FindGameObjectsWithTag ("Mov1");
						foreach (GameObject gme in gmeO){

							Destroy(gme,0);
						}
					}
					GameObject.Find ("txtPuntos").GetComponent<Text> ().text = puntos.ToString ();
					GameObject.Find ("txtConsecutivas").GetComponent<Text> ().text = consecutivas.ToString ();										
				}
			}
		}
	}

	string GetNomImg(string target){
		string valor = "nada.png";
		switch (target) {
		case "Cil_Paralelo_Base":
			valor = "Cil_Paralelo_Base.png";
			break;
		case "Cil_Perpendicular_Base":
			valor = "Cil_Perpendicular_Base.png";
			break;
		case "Cilindro_Oblicuo":
			valor = "Cilindro_Oblicuo.png";
			break;
		case "Cono_Oblicuo_Base":
			valor = "Cono_Oblicuo.png";
			break;
		case "Cono_Paralelo_Base":
			valor = "Cono_Paralelo_Base.png";
			break;
		case "Cono_Perpendicular_Base":
			valor = "Cono_Perpendicular_Base.png";
			break;
		case "Cono_Paralelo_Generatriz":
			valor = "Cono_Paralelo_Generatriz.png";
			break;
		default:
			valor = "nada.png";
			break;
		}
		return valor;
	}

	void CambiarImg(string imagen){
		string arch = GetNomImg (imagen);
		Debug.Log (arch);

		Texture2D textura = new Texture2D (150, 150, TextureFormat.RGBA32, false);
		if (Application.isMobilePlatform) {
			string ruta = Application.streamingAssetsPath + "/images/ConstruirFig/" + arch; 

			WWW www = new WWW (ruta);
			while (!www.isDone) {
			}
			www.LoadImageIntoTexture (textura);
		}
		else {
			textura.LoadImage(File.ReadAllBytes(Application.dataPath + "/images/ConstruirFig/" + arch));
		}
		imgObjetivo.texture = textura;

	}

	void CambiarImgObj(){
		Texture2D textura = new Texture2D (150, 150, TextureFormat.RGBA32, false);
		string tarOBj;

		if (target.Contains("Cil")){
			tarOBj = "Cilindro.png";
		}
		else{
			tarOBj = "Cono.png";		
		}
		if (Application.isMobilePlatform) {
			string ruta = Application.streamingAssetsPath + "/images/" + tarOBj; 
			WWW www = new WWW (ruta);
			while (!www.isDone) {
			}
			www.LoadImageIntoTexture (textura);
		}
		else {
			textura.LoadImage(File.ReadAllBytes(Application.dataPath + "/images/" + tarOBj));
		}
		GameObject.Find ("imgFigura").GetComponent<RawImage> ().texture = textura;
	}

	public void MostrarPanel(bool result){
		Debug.Log ("Pausando juego");
		pnlPausa.GetComponent<RectTransform>().localScale = new Vector3 (0.31f, 0.5f, 0.3f);

		txtAviso = GameObject.Find("txtAviso").GetComponent<Text>();
		string arch = "incorrecto.png";
		if (result) {
			if (target.Contains("Cil")){
				arch = "Cilindro.png";
			}
			else{
				arch = "Cono.png";		
			}
		}

		Texture2D textura = new Texture2D (150, 150, TextureFormat.RGBA32, false);
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
		Text txtPausar = GameObject.Find("txtPausar").GetComponent<Text>();
		txtPausar.text = "Si";
	}
	
	public void PausarJuego(){
		GameObject  [] gmeO = GameObject.FindGameObjectsWithTag ("Mov1");
		foreach (GameObject gme in gmeO){
			gme.GetComponent<moverObj> ().enabled = false;
			gme.GetComponent<Transform> ().localScale = Vector3.zero;
		}
	}
	public void ReanudarJuego(){
		pnlPausa.SetActive (true);
		pnlPausa.transform.localScale = Vector3.zero;
		GameObject  [] gmeO = GameObject.FindGameObjectsWithTag ("Mov1");
		foreach (GameObject gme in gmeO){
			gme.GetComponent<moverObj> ().enabled = true;
			//gme.GetComponent<MeshRenderer> ().enabled = true;
			gme.GetComponent<Transform> ().localScale = Vector3.one;
		}
	}

	public string VerAyuda(string corte, string fig){
		string respuesta = "";
		string imagen = GetNomImg (target);
		Texture2D textura = new Texture2D (150, 150, TextureFormat.RGBA32, false);
		if (Application.isMobilePlatform) {
			string ruta = Application.streamingAssetsPath + "/images/ConstruirFig/help/" + imagen; 

			WWW www = new WWW (ruta);
			while (!www.isDone) {
			}
			www.LoadImageIntoTexture (textura);
		}
		else {
			textura.LoadImage(File.ReadAllBytes(Application.dataPath + "/images/ConstruirFig/help/" + imagen));
		}
		raw2.texture = textura;

		return respuesta;
	}
}