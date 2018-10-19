using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class Colisiones: MonoBehaviour{
    [SerializeField] float currentAmount;
    [SerializeField] float speed;
	public Transform LoadingBar;
	public Transform txtPercent;
    GameObject fig4;
    GameObject figura;
    bool terminada ;
	float altura;
	Material blue, mtRadio, mtAltura, mtCara, mtApotema, mtLongitud, mtAncho, mtPerimetro, mtPi;
	string tar;
	bool lista = false;
	Text txtObjetivo;

    void Start(){
		txtObjetivo = GameObject.Find("txtObjetivo").GetComponent<Text>();
		figura = GameObject.Find("Figura");
        fig4 = GameObject.Find("incremento");
        terminada = false;
		blue = Resources.Load ("blue", typeof(Material)) as Material;
		blue.name = "blue";
		mtRadio = Resources.Load ("mtRadio", typeof(Material)) as Material;
		mtAltura = Resources.Load ("mtAltura", typeof(Material)) as Material;
		mtCara = Resources.Load ("mtCara", typeof(Material)) as Material;
		mtApotema = Resources.Load ("mtApotema", typeof(Material)) as Material;
		mtLongitud = Resources.Load ("mtLongitud", typeof(Material)) as Material;
		mtAncho = Resources.Load ("mtAncho", typeof(Material)) as Material;
		mtPerimetro = Resources.Load ("mtPerimetro", typeof(Material)) as Material;
		mtPi = Resources.Load ("mtPi", typeof(Material)) as Material;
		blue = Resources.Load ("blue", typeof(Material)) as Material;
		mtRadio.name = "mtRadio";
		mtAltura.name = "mtAltura";
		mtCara.name = "mtCara";
		mtApotema.name = "mtApotema";
		mtLongitud.name = "mtLongitud";
		mtAncho.name = "mtAncho";
		mtPerimetro.name = "mtPerimetro";
		mtPi.name = "mtPi";
		tar = "";
    }

	Material SetMaterial(string cuerpo){
		Material mat = blue;
		switch (cuerpo) {
		case "Altura":
			mat = mtAltura;
			break;
		case "Apotema":
			mat = mtApotema;
			break;
		case "Radio":
			mat = mtRadio;
			break;
		case "Perimetro":
			mat = mtPerimetro;
			break;
		case "Longitud":
			mat = mtLongitud;
			break;
		case "Ancho":
			mat = mtAncho;
			break;
		case "Pi":
			mat = mtPi;
			break;
		}
		return mat;
	}

	void SetDimensiones(){
		if (txtObjetivo.text.Equals ("Prisma Cuadrangular")) {
			GameObject.Find ("Obj04").GetComponent<RectTransform> ().localScale = Vector3.zero;
			GameObject.Find ("x2").GetComponent<RectTransform> ().localScale = Vector3.zero;
		}
		else {
			if (txtObjetivo.text.Equals("Prisma Cuadrangular")){
				GameObject.Find("Obj04").GetComponent<RectTransform>().localScale = Vector3.one;
				GameObject.Find("x2").GetComponent<RectTransform>().localScale = Vector3.one;
			}
		}
	}

	private void OnTriggerEnter(Collider other){
		SetDimensiones ();
		if (!terminada && currentAmount < 100f ){	
			Debug.Log(gameObject.name + " colisiona con " + other.name  + "  Tag:" + other.GetComponent<Collider>().tag + "  Target  " + tar);
			Debug.Log (gameObject.name);
			//figura.GetComponent<MeshRenderer> ().material = SetMaterial (other.name);

			if (other.tag.Equals ("Trigger") && gameObject.name.Equals ("Figura")) {				
				Debug.Log ("Se ha cargado el Material");
				figura.GetComponent<MeshRenderer> ().material = SetMaterial (other.name);	
				figura.tag = "Player";	
				tar = other.name;
				lista = true;
			} 
		//	else if (other.tag.Equals ("Player") && other.name.Contains ("Prisma") && lista && gameObject.name.Equals ("Figura") ||
		//	        other.tag.Equals ("Player") && other.name.Contains ("Cilindro") && lista && gameObject.name.Equals ("Figura")) {				
			else if (other.tag.Equals ("Player") && other.name.Contains ("Prisma") && lista && !gameObject.name.Contains ("_") ||
				other.tag.Equals ("Player") && other.name.Contains ("Cilindro") && lista && !gameObject.name.Contains ("_")) {				
				Debug.Log ("Se ha detectado el Tag " + other.tag + " en " + other.name + ".\t Target: " + tar);

				GameObject [] g = GameObject.FindGameObjectsWithTag ("Ej1");
				Array.Sort(g, delegate(GameObject wp1, GameObject wp2) { return wp1.name.CompareTo(wp2.name); });
				string str = "";

				for (int i = 0; i < g.Length; i++) {
					if( g[i].GetComponent<RectTransform>().childCount == 0){
						str = g [i].name;
						Debug.Log ("El primer Slot vacio es: " + str );
						GameObject newPadre = GameObject.Find(str);

						if (figura.GetComponent<MeshRenderer> ().material.name.Contains("mt")){
							string [] s = figura.GetComponent<MeshRenderer> ().material.name.Split(' ');
							string nomFig = s[0] + 'S';
							Debug.Log ("Material  " + s[0].Substring (2) + "\t" + figura.GetComponent<MeshRenderer>().material.name  + "  " + s[0].Substring (2).Length );
							Debug.Log (nomFig);
							GameObject temp = GameObject.Find (tar + "S");
							if (nomFig.Equals ("Lado")) {
								temp = (GameObject) Instantiate (temp);
								temp.name = "LadoS";
							}
							Debug.Log ("Parent: " + newPadre.GetComponent<Transform> ().position + "  de " + temp.name);
							temp.GetComponent<Transform> ().parent = newPadre.GetComponent<Transform>();
							temp.GetComponent<Transform> ().localPosition = Vector3.zero;
							temp.GetComponent<Transform> ().localRotation = Quaternion.Euler (0f,0f,0f);
							temp.GetComponent<Transform> ().localScale = new Vector3 (0.5f, 1f, 1f);
							tar = "";
							getAltura ();
							BarraProgreso ();
						}
						break;
					}
				}
				Debug.Log ("Reseteando el Material");

				figura.tag = "Trigger";

				figura.GetComponent<MeshRenderer> ().material = blue;
				Debug.Log ("Avance  " + currentAmount);
				lista = false;
			}
		
			GameObject.Find ("txtOpcion").GetComponent<Text> ().text = tar;
		}
	}


	public void BarraProgreso()
    {
		currentAmount += getAltura();
		if (currentAmount >= 100f) {
			currentAmount = 100.0f;
			terminada = true;
		}
		txtPercent.GetComponent<Text> ().text = ((int)currentAmount).ToString () + "%";
		LoadingBar.GetComponent<Image> ().fillAmount = (float) Math.Round (currentAmount / 100, 0);
    }

	public float getAltura()
	{
		switch (txtObjetivo.text) {
		case "Cilindro":
			altura = 25f;
			break;
		case "Prisma Triangular":
			altura = 33.334f;
			break;
		case "Prisma Cuadrangular":
			altura = 33.34f;
			break;
		case "Prisma Pentagonal":
			altura = 20f;
			break;
		}
		return altura;
	}

	public void ReStart(){
		currentAmount = 0f;
		LoadingBar.GetComponent<Image> ().fillAmount = 0;
		txtPercent.GetComponent<Text> ().text = "0%";
		terminada = false;
		altura = 0f;
		fig4.transform.localScale = new Vector3 (8f, 0.01f, 20f);
		SceneManager.LoadScene ("CrearFormulas");
	}
}