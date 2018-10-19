using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Vuforia;
using System;

public class Colisionar2 : MonoBehaviour{
    [SerializeField] float currentAmount;
    [SerializeField] float speed;
	Dropdown drop;
	public Transform LoadingBar;
	public Transform txtPercent;
    GameObject fig4;
    GameObject cuboArena;
    bool terminada ;
	float altura, excedente;
	int ncolisiones;
	Material mtArena, blue;
	string tar;
	bool lista = false;
    double tiempo;
    Text txtTiempo;

    void Start(){
        tiempo = 0d;
		drop = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        txtTiempo = GameObject.Find("txtTiempo").GetComponent<Text>();
        cuboArena = GameObject.Find("CuboArena");
        fig4 = GameObject.Find("incremento");
        terminada = false;
		mtArena = Resources.Load ("mtArena", typeof(Material)) as Material;
		blue = Resources.Load ("blue", typeof(Material)) as Material;
		mtArena.name = "mtArena";
		blue.name = "blue";
		tar = "";
		ncolisiones = 0;
		excedente = 0;
		GameObject.Find ("pnlAyuda").GetComponent<RectTransform> ().localScale = Vector3.zero;
        StartCoroutine("ControlTiempo");
    }

    IEnumerator ControlTiempo()
    {
        try
        {
            tiempo = double.Parse(txtTiempo.text, System.Globalization.NumberStyles.Any);
            tiempo += Time.deltaTime;
            txtTiempo.text = "" + tiempo.ToString("F");
            Debug.Log("Iniciando Rutina");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        yield return new WaitForSeconds(0);
    }

    public void PausarTiempo()
    {
        Debug.Log("Parando Rutina Tiempo");
        StopCoroutine("ControlTiempo");
    }


    private void OnTriggerEnter(Collider other){
		if (!terminada && currentAmount < 100f && GameObject.Find("Dropdown").GetComponent<RectTransform>().localScale == Vector3.zero)  {	
			Debug.Log (gameObject.name + " colisiona con " + other.name + "  Tag:" + other.GetComponent<Collider> ().tag + "  Target  " + tar);
			if (other.tag.Equals ("Trigger") && other.name.Equals ("Arenero") && gameObject.name.Equals ("CuboArena")) {
				if (tar.Equals ("")) {
					tar = "Arenero";
				}

				if (tar.Equals ("Arenero")) {						
					Debug.Log ("Se ha cargado el cucharon de Arena");
					//GameObject.FindGameObjectWithTag ("Mov1").GetComponent<MeshRenderer> ().material = mtArena;
				//	GameObject.FindGameObjectWithTag ("Mov1").GetComponent<Transform> ().localScale = new Vector3 (1.2f, 1.2f, 1.1f);
					cuboArena.GetComponent<MeshRenderer> ().material = mtArena;	
					cuboArena.tag = "Player";
					tar = "Contenedor";
					lista = true;
					GameObject.Find("txtInstrucciones").GetComponent<Text>().text = "Colisiona el Marcador Cubo con el Contenedor";
				}
			} 
			else if (other.tag.Equals ("Player") && other.name.Equals ("Contenedor") && lista
			        && gameObject.name.Equals ("CuboArena")) {				
				Debug.Log ("Se ha detectado el Tag " + other.tag + " en " + other.name + ".\t Target: " + tar);

				if (cuboArena.GetComponent<MeshRenderer> ().material.name.Contains ("Arena") && tar.Equals ("Contenedor")) {
				//if (GameObject.FindGameObjectWithTag ("Mov1").GetComponent<MeshRenderer> ().material.name.Contains ("Arena") && tar.Equals ("Contenedor")) {
					Debug.Log ("Descargando el Cucharon en el Recipiente.");
					getAltura ();
					BarraProgreso ();
					//	float ff = (currentAmount / 100f);

					fig4.transform.localScale = new Vector3 (4, currentAmount / 10f, 4);
					//Aqui se cambia la posicion respecto de tablet y windows y mac
					fig4.transform.localPosition = new Vector3 (6.83f, (-6.39f + (currentAmount / 20f)), 24.9f);
					//Aqui termina
					cuboArena.tag = "Trigger";
					tar = "Arenero";
					ncolisiones++;
					cuboArena.GetComponent<MeshRenderer> ().material = blue;
					//GameObject.FindGameObjectWithTag ("Mov1").GetComponent<MeshRenderer> ().material = null;
					//GameObject.FindGameObjectWithTag ("Mov1").GetComponent<Transform> ().localScale = Vector3.zero;
					Debug.Log ("Avance  " + currentAmount);
					lista = false;
					if (currentAmount >= 100f) {
						GameObject.Find ("pnlAyuda").GetComponent<RectTransform> ().localScale = new Vector3 (0.25f, 0.3f, 0.1f);
						if (excedente != 0) {
							GameObject.Find ("txtAlerta").GetComponent<Text> ().text = "El Contenedor se ha llenado. \n Tu recipiente auxiliar tiene " + excedente + "% de excedente, excediste las dimeinsiones 3x10x3 del contenedor";
						}
						GameObject.Find ("txtMensaje").GetComponent<Text> ().text = "Bravo. Alcanzaste la meta en " + ncolisiones + " pasos. \n" +
							"Comenta con tus amigos que aprendiste.";
						GameObject.Find ("Arenero").GetComponent<Transform> ().localScale = Vector3.zero;
						GameObject.Find("txtInstrucciones").GetComponent<Text>().text = "El Contenedor se ha llenado. Lo Lograste.";
						GameObject.Find ("ARCamera").GetComponent<NoARCamera> ().enabled = true;
						//	GameObject.Find ("ARCamera").GetComponent<Camera> ().clearFlags = CameraClearFlags.Skybox;
						Vuforia.CameraDevice.Instance.Stop ();
						GameObject.Find ("ARCamera").GetComponent<Camera> ().clearFlags = CameraClearFlags.Skybox;
                        PausarTiempo();
                        GuardarDatos guardar = new GuardarDatos("2", ncolisiones , 0, (ncolisiones * 300), 0, 1, "Facil");
                        guardar.Insert();
                        

                    } 
					else {
						GameObject.Find("txtInstrucciones").GetComponent<Text>().text = "Colisiona el Marcador Cubo con el Arenero";
					}
				}			
			}
			GameObject.Find ("txtOpcion").GetComponent<Text> ().text = tar;
		}
		else if (currentAmount >= 100f) {
			GameObject.Find ("pnlAyuda").GetComponent<RectTransform> ().localScale = new Vector3 (0.25f, 0.3f, 0.1f);
			if (excedente != 0) {
				GameObject.Find ("txtAlerta").GetComponent<Text> ().text = "El Contenedor se ha llenado. Tu recipiente auxiliar tiene " + excedente + "% de excedente, excediste las dimeinsiones 3x10x3 del contenedor";
			}
			GameObject.Find ("txtMensaje").GetComponent<Text> ().text = "Bravo. Alcanzaste la meta en " + ncolisiones + " pasos. \n" +
					"Comenta con tus amigos que aprendiste.";
			GameObject.Find ("Arenero").GetComponent<Transform> ().localScale = Vector3.zero;
			//GameObject.Find ("ARCamera").GetComponent<Camera> ().clearFlags = CameraClearFlags.Skybox;
			Vuforia.CameraDevice.Instance.Stop ();
			GameObject.Find ("ARCamera").GetComponent<Camera> ().clearFlags = CameraClearFlags.Skybox;
		}
	}

	public void BarraProgreso()
    {
		float volTotal;
		currentAmount += getAltura();
		if (currentAmount >= 100f) {			
			excedente = currentAmount - 100f;
			currentAmount = 100.0f;
			terminada = true;
			txtPercent.GetComponent<Text> ().text = ((int)currentAmount).ToString () + "%";
			LoadingBar.GetComponent<UnityEngine.UI.Image> ().fillAmount = 100 / 100;
			volTotal = 90* LoadingBar.GetComponent<UnityEngine.UI.Image> ().fillAmount;
			GameObject.Find ("lblDrop").GetComponent<Text> ().text = "Medida : "+  drop.captionText.text + "\n Avance Total: " + volTotal + " cm3";
		} 
		else {
			txtPercent.GetComponent<Text> ().text = ((int)currentAmount).ToString () + "%";
			LoadingBar.GetComponent<UnityEngine.UI.Image>().fillAmount = currentAmount / 100;
			volTotal = 90 *  LoadingBar.GetComponent<UnityEngine.UI.Image> ().fillAmount;
			GameObject.Find ("lblDrop").GetComponent<Text> ().text = "Medida : "+  drop.captionText.text + "\n Avance Total: " + volTotal + " cm3";
		}
    }

    public float getAltura()
    {
		switch (drop.value) {
		case 0:
			altura = 10;
			break;
		case 1:
			altura = 20;
			break;
		case 2:
			altura = 50;
			break;
		}
		return altura;
    }

	public void ReStart(){
/*		currentAmount = 0f;
		LoadingBar.GetComponent<UnityEngine.UI.Image> ().fillAmount = 0;
		drop.GetComponentInChildren<Text>().text = "Seleccione";
		txtPercent.GetComponent<Text> ().text = "0%";
		terminada = false;
		altura = 0;
		fig4.transform.localScale = new Vector3 (4f, 0.01f, 4f);
		fig4.transform.localPosition = new Vector3 (6.82f, -6.39f, 24.9f);
		*/
		SceneManager.LoadScene ("ArenaScene");
	}
}