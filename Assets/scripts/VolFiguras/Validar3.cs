using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Validar3 {
	public GameObject entrada;
	public bool btnCalcular(){
		Text input = GameObject.Find("txtInput").GetComponent<Text> ();
		Debug.Log ("Entrando a Calcular.  " + input.text);
	//	Regex Val = new Regex ("^[0-9]+$"); //SOlo enteros
		Regex Val = new Regex (@"^[+-]?\d+(\.\d+)?$"); //Cualquier numero flotante
		if (Val.IsMatch (input.text)) {
			Debug.Log ("Numero:  " + input.text);
		
			return true;
			} 
			else {
				Debug.Log("Favor de introducir Números Solamente, Error de Sintaxis");
				GameObject.Find ("txtAviso").GetComponent<Text> ().text= "Favor de solo escribir números enteros";		
			}
		return false;
		}

	public static string StringClean(string str){
		return Regex.Replace (str, @"[^\w\.@-]", ""); 
	}
}