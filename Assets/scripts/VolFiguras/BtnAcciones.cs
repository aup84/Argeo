using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnAcciones : MonoBehaviour {
	Text result;

	void Start(){
		result = GameObject.Find ("txtStatus").GetComponent<Text> ();
	}

	public void OnClick(string valor){
		result.text += valor;
	}

	public void OnPuntoClick(){
		if (GameObject.Find ("btnPunto").GetComponent<BtnAcciones> ().enabled == true) {
			GameObject.Find ("btnPunto").GetComponent<BtnAcciones> ().enabled = false;
			result.text += ".";
		}				
	}

	public void OnClickBorrar1(){
		string calculo = result.text;
		if (calculo.Length > 0) {
			calculo = calculo.Substring (0, calculo.Length - 1);
			result.text = calculo;
		//	Debug.Log("Resultado:  " +  calculo);
		}				
	}
	public void Resetear(){
		result.text = "";
		GameObject.Find ("btnPunto").GetComponent<BtnAcciones> ().enabled = true;
		GameObject.Find ("btnPunto").GetComponent<Button> ().enabled = true;
	}
}