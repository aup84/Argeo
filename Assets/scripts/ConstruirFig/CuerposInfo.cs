using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class CuerposInfo{
	string nomFigura;
	string nomImagen;
	List<Aux1> datos;
	List<string> terminados;
	List <string> gameTargets;

	public CuerposInfo(){
		datos = new List<Aux1> ();
		terminados = new List<string> ();
		gameTargets = new List<string> ();
		datos.Add (new Aux1 (0, "Cil_Paralelo_Base", "Cilindro Paralelo a la Base", "Paralelo a la Base"));
		datos.Add (new Aux1 (1, "Cil_Perpendicular_Base", "Cilindro Perpendicular a la Base", "Perpendicular a la Base"));
		datos.Add (new Aux1 (2, "Cilindro_Oblicuo", "Cilindro Oblicuo a la Base", "Oblicuo a la Base"));

		datos.Add (new Aux1 (3, "Cono_Paralelo_Base", "Cono Paralelo a la Base", "Paralelo a la Base"));
		datos.Add (new Aux1 (4, "Cono_Perpendicular_Base", "Cono Perpendicular a la Base",  "Perpendicular a la Base"));
		datos.Add (new Aux1 (5, "Cono_Oblicuo_Base", "Cono Oblicuo a la Base", "Oblicuo a la Base"));
		datos.Add (new Aux1 (6, "Cono_Paralelo_Generatriz", "Cono Paralelo a la Generatriz", "Paralelo a la Generatriz"));

		foreach (Aux1 t in datos) {
			gameTargets.Add (t.getDesc() + "A");
			gameTargets.Add (t.getDesc() + "B");
			terminados.Add (t.getDesc ());
		}
		Debug.Log ("Datos Cargados Satisfactoriamente. Elementos en pantalla " + gameTargets.Count + ". Targets" + terminados.Count);
	//	Orden ();
	}
		/*

	public void Orden(){		//Ordenar
		if (terminados.Count > 0) {			
			var rnd = new System.Random ();
			terminados = terminados.OrderBy (item => rnd.Next ()).ToList();
		}
	}
*/
	//Obtener Busqueda por una descripcion
	public string getDescripcion(string concepto){
		if (datos.Count > 0) {			
			var temp = datos.Find (x => x.getDesc () == concepto);
			//Text txtTarget = GameObject.Find ("txtTarget").GetComponent<Text> ();
			//txtTarget.text = temp.getFiguraConcepto ().ToString ();
			return temp.getFiguraConcepto ().ToString ();
		}
		return "Elemento no encontrado";
	}
	/*
	public string getDescripcionByIndex(){
		if (datos.Count > 0) {			
			System.Random rnd = new System.Random ();
			int rn = rnd.Next (0, datos.Count);
			var temp = datos.Find (x => x.getIndice () == rn);
			return temp.getDesc().ToString ();
		}
		return "Elemento no encontrado";
	}
*/

	public bool RevisaTerminados(string concepto){
		if (terminados.Count > 0) {
			var temp = terminados.Find (x => x == concepto);
			if (temp != null) {
				Debug.Log ("No existen en la lista, puedes seguir");
				return true;
			}
		}	Debug.Log ("Dato existente, vuelve a intentar");
		return false;
	}


	public string getNextItem(){
		if (terminados.Count > 0) {	
			Debug.Log ("Elementos restantes: " + terminados.Count);
			return terminados.First();
		}
		return "Elemento no encontrado";
	}

	public string getNextItemCorte(){
		if (terminados.Count > 0) {	
			var temp = datos.Find (x => x.getDesc () == terminados.First());
			Debug.Log ("Corte " + temp.getCorte ());
			return temp.getCorte();
		}
		return "Elemento no encontrado";
	}

	public string getNextItemFigura(){
		if (terminados.Count > 0) {	
			var temp = datos.Find (x => x.getDesc () == terminados.First());
			string[] dat = temp.getFiguraConcepto ().Split ('_');
			Debug.Log (dat [0]);
			return dat [0];
		}
		return "Elemento no encontrado";
	}

	public void QuitarElemento(string concepto){
		if (gameTargets.Count > 0 && terminados.Count > 0) {						
			gameTargets.RemoveAll(x => x.Contains(concepto));
			terminados.RemoveAll(x => x.Contains(concepto));
			Debug.Log ("Elementos restantes:  " + gameTargets.Count + "\t Terminados:  " + terminados.Count );
		}
	}
}