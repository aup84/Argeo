using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class ReglasJuego4 {
	string nomFigura;
	string nomImagen;
	List<Aux2C> datos;
	List<string> terminados;
	Text txtErrores;
	private static ReglasJuego4 instance;

	public ReglasJuego4(){
		try{
			datos = new List<Aux2C> ();
			terminados = new List<string>();
			datos.Add (new Aux2C (0, "Prisma_Triangular", "Prisma Triangular"));
			datos.Add (new Aux2C (1, "Prisma_Cuadrangular", "Prisma Cuadrangular"));
			datos.Add (new Aux2C (2, "Prisma_Pentagonal", "Prisma Pentagonal"));
			datos.Add (new Aux2C (3, "Prisma_Hexagonal", "Prisma Hexagonal"));
			datos.Add (new Aux2C (4, "Prisma_Decagonal", "Prisma Decagonal"));
			datos.Add (new Aux2C (5, "Cilindro", "Cilindro"));
		//	datos.Add (new Aux2C (6, "Tetraedro", "Tetraedro"));

			foreach (Aux2C t in datos) {
				//terminados.Add (t.getDesc ());
				terminados.Add(t.getFiguraConcepto());
			}
			Desordenar ();
		}
		catch{
		}
	}

	public static ReglasJuego4 Instance{
		get{ 
			if (instance == null) {
				instance = new ReglasJuego4 ();
			}
			return instance;
		}
	}

	public void Desordenar(){		//Ordenar
		if (terminados.Count > 0) {		
			Debug.Log ("Desordenando");	
			var rnd = new System.Random ();
			terminados = terminados.OrderBy (item => rnd.Next ()).ToList();
		}
	}

	public string getDescripcion(string concepto){
		if (datos.Count > 0) {			
			var temp = datos.Find (x => x.getDesc () == concepto);
			Debug.Log("Concepto:  " + concepto + "    " + temp.getFiguraConcepto ().ToString ());
			return temp.getFiguraConcepto ().ToString ();
		}
		return "Elemento no encontrado";
	}

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
		Debug.Log (terminados.Count);
		if (terminados.Count > 0) {	
			Debug.Log ("Elementos restantes: " + terminados.Count);
			return terminados.First();
		}
		return "Elemento no encontrado";
	}

	public void QuitarElemento(string concepto){

		if (terminados.Count > 0) {						
			terminados.RemoveAll (x => x.Contains (concepto));
			Debug.Log ("Elementos restantes:  " + terminados.Count);
		}
		else {
			Debug.Log ("No entro al MEtodo");
		}
	}

	public List<string> getLista(){
		return terminados;
	}
}