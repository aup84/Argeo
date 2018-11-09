using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class JuegoFigInc : MonoBehaviour {
	int puntos = 0;
    double tiempo = 0.0d;
	public Text txtTiempo;
	Text txtPuntos, txtErrores, txtAciertos, txtPausar;
    int errores;
    int aciertos;
	GameObject boton;
	GameObject BtnReset;
	public GameObject pnlPausa;
    GameRules regla;
	public bool pause;

	void Start () {
		regla = new GameRules();
        regla.desordenar();
		regla.SetImgObjetivo ();

		txtTiempo = GameObject.Find("txtTiempo").GetComponent<Text>();
		txtPuntos = GameObject.Find("txtPuntos").GetComponent<Text>();
		txtErrores = GameObject.Find("txtErrores").GetComponent<Text>();
		txtAciertos = GameObject.Find("txtAciertos").GetComponent<Text>();
		txtPausar = GameObject.Find("txtPausar").GetComponent<Text>();
		boton = GameObject.Find ("BtnMenu");
		//boton.SetActive (false);
		BtnReset = GameObject.Find ("BtnReset");
		//BtnReset.SetActive (false);
		pnlPausa = GameObject.Find("pnlPausa");
		pnlPausa.GetComponent<RectTransform> ().localScale = Vector3.zero;
		Time.timeScale = 1;
		txtPausar.text = "No";
	}

	// Update is called once per frame
	void Update () {
		errores = Convert.ToInt32 (txtErrores.text);
		aciertos = int.Parse(txtAciertos.text);

        if (errores <5 && aciertos <10){
			if (txtPausar.text.Equals("No")) {
				StartCoroutine ("ControlTiempo");
			}
		}
		else {
			Time.timeScale = 0;
			PausarTiempo ();
			Text txtAyuda = GameObject.Find("txtAyuda").GetComponent<Text>();
			if (errores >= 5) {
				txtAyuda.text = "Fin del juego. Has acumulado 5 errores";
			} 
			else {
				txtAyuda.text = "Felicidades, has alcanzado los 10 objetivos";		
			}
			BtnReset.SetActive (true);
			boton.gameObject.SetActive (true);
		}		
	}
	IEnumerator ControlTiempo(){
		try {
			tiempo = double.Parse (txtTiempo.text, System.Globalization.NumberStyles.Any);
			tiempo += Time.deltaTime;
			txtTiempo.text = "" + tiempo.ToString ("F");
			puntos = int.Parse (txtPuntos.text);
			txtPuntos.text = puntos.ToString ();
		} 
		catch (Exception e) {
			Debug.Log (e.Message);
		}
		yield return new WaitForSeconds (0);
	}

	public void PausarTiempo(){
		StopCoroutine ("ControlTiempo");
	}

	public void SigObj(){
		pnlPausa.SetActive(true);
		pnlPausa.GetComponent<RectTransform> ().localScale = Vector3.zero;
		txtPausar.text = "No";
		regla.SetImgObjetivo ();
	}
}