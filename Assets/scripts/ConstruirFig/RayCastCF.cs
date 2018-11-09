using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class RayCastCF : MonoBehaviour {
	string target;
	int encontradas, consecutivas, puntos, errores, final;
	Text txtTarget, lblStatus, txtFind, txtPuntos, txtConsecutivas, txtTiempo, txtTiempo2, txtAviso, txtAyudas;
	CuerposInfo inf;
	RawImage raw2, imgObjetivo;
	GameObject pnlPausa;
    double tiempo, tiempo2, tempTiempo;
    int nAyudas = 0;
    bool quitadas = false;
    FuzzyLogic fl;
    int level = 1;

    void Start () {				// Use this for initialization
		tiempo = 0d;
		raw2 = GameObject.Find ("RawPausa").GetComponent<RawImage> ();
		imgObjetivo = GameObject.Find ("imgObjetivo").GetComponent<RawImage> ();
		inf = new CuerposInfo ();
		txtTiempo = GameObject.Find ("txtTiempo").GetComponent<Text> ();
        txtAyudas = GameObject.Find("txtAyudas").GetComponent<Text>();
        txtTiempo2 = GameObject.Find("txtTiempo2").GetComponent<Text>();
        txtTarget = GameObject.Find ("txtTarget").GetComponent<Text> ();
		txtFind = GameObject.Find("txtFind").GetComponent<Text> ();		
		txtTiempo.text = "0";
        DatosFigura df = inf.GetNextItem(1);
        target = df.Descripcion;

        txtTarget.text = df.Corte;
		CambiarImgObj ();
		//CambiarImg (target);
		Debug.Log ("El objetivo es: " + target);
		pnlPausa = GameObject.Find("pnlPausa");
		pnlPausa.GetComponent<RectTransform> ().localScale = Vector3.zero;
		StartCoroutine ("ControlTiempo");    
	}

	public void MostrarFigurasObj(){
        Debug.Log("Empezando a mostrar los " + target);
        GameObject [] gmeO = GameObject.FindGameObjectsWithTag ("Mov1");
        if (level == 3 || target.Contains("Cono")){
            MostrarTodas();
        }
        else{
            foreach (GameObject gme in gmeO){
                gme.GetComponent<Transform>().localScale = Vector3.zero;
                gme.GetComponent<moverObj>().enabled = true;
                if (gme.name.Contains("Cil")){
                    gme.GetComponent<Transform>().localScale = Vector3.one;
                }
            }
        }
    }

    public void MostrarTodas() {
        GameObject[] gmeO = GameObject.FindGameObjectsWithTag("Mov1");
        foreach (GameObject gme in gmeO){           
            gme.GetComponent<Transform>().localScale = Vector3.one;
            gme.GetComponent<moverObj>().enabled = true;
        }
        Debug.Log("Se han activado todas las figuras");
    }

	IEnumerator ControlTiempo(){
		try {
			tiempo = double.Parse(txtTiempo.text, System.Globalization.NumberStyles.Any);
            tiempo2 = double.Parse(txtTiempo2.text, System.Globalization.NumberStyles.Any);
            tiempo += Time.deltaTime;
            tiempo2 += Time.deltaTime;
            txtTiempo.text = "" + tiempo.ToString("F");
            txtTiempo2.text = "" + tiempo2.ToString("F");
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
		if (final >= 10) {
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
					txtFind.text = inf.GetDescripcion(nom2);
					Debug.Log ("Detectado: " + hitInfo.collider.gameObject.name + ", Objetivo: "+ target);
					if (target.Equals (nom)) {
						consecutivas++;
						Debug.Log ("Encontrada la parte " + consecutivas + " de 2 " + nom + ". Final " + final);
						puntos += 100;
						hitInfo.collider.gameObject.GetComponent<Transform> ().localScale = Vector3.zero;

						inf.GetDescripcion (nom2);
						if (consecutivas == 1) {
							imgObjetivo.GetComponent<RectTransform> ().localScale = new Vector3 (4f, 3f, 4f);
							CambiarImg (target);
						}	

						if (consecutivas == 2) {	
							encontradas++;
							GameObject.Find ("txtAciertos").GetComponent<Text> ().text = encontradas.ToString ();							
							MostrarPanel (true);
							txtAviso.text = "Lo lograste, ahora avanza al siguiente objetivo";
							txtAviso.text = txtAviso.text + " " + txtTarget.text;
							inf.QuitarElemento (nom);

                            //Fuzzy Logic
                            tempTiempo = Math.Round(tiempo2 / 150, 3);
                            tempTiempo = (tempTiempo < 1) ? tempTiempo : 1d;
                            txtTiempo2.text = "0.01";
                            tiempo2 = 0;
                            double denominador = encontradas + errores + nAyudas;
                            FuzzyConstruirFig fuzzy = new FuzzyConstruirFig(encontradas / denominador, errores / denominador, tempTiempo, nAyudas / denominador);
                            double d = fuzzy.Inferencias();
                            level = fuzzy.FuzzyToCrisp(d);

                            int ant = Convert.ToInt32(GameObject.Find("txtNivel").GetComponent<Text>().text);
                            if ((ant - level) == 0 || (ant - level) == 1 || (ant - level) == -1){
                                Debug.Log("Se queda en el nivel " + level);
                            }
                            else if ((ant - level) < 0){
                                level = ant + 1;
                            }
                            else{
                                level = ant - 1;
                            }

                            GameObject.Find("txtNivel").GetComponent<Text>().text = level.ToString(); // + "    " + Math.Round(d,4);
                            Debug.Log("txtNivel se ha actualizado a : " + level);
                            //Termina Fuzzy Logic

                            DatosFigura df = inf.GetNextItem(level);
                            target = df.Descripcion;                            
                                                       
                            txtTarget.text = df.Corte;
							Debug.Log ("Cambiaremos de Objetivo a " + target);
							CambiarImg ("nada");
							imgObjetivo.GetComponent<RectTransform> ().localScale = Vector3.zero;
							CambiarImgObj ();
							consecutivas = 0;
							final++;
                            Debug.Log("Mostrando las figuras del nivel " + level);
                            if (level == 3){
                                MostrarTodas();
                            }
                            else { 
                                MostrarFigurasObj();                                
                            }
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
						txtAviso.text = txtAviso.text +  " " + txtTarget.text;
						Debug.Log ("A:  " + nom  + "A" + ", " + nom + "B");
						PausarJuego ();
						VerAyuda(txtTarget.text, target);
					}

					if (final > 10) {
                        int aciertos = Convert.ToInt32(GameObject.Find("txtAciertos").GetComponent<Text>().text);
                        int err = Convert.ToInt32(GameObject.Find("txtErrores").GetComponent<Text>().text);
                        Debug.Log("Errores " + err);                        
                        GuardarDatos g = new GuardarDatos("3", aciertos, err, puntos, tiempo, 1, "Facil");
                        g.Insert();
						txtTarget.text = "FIN DEL JUEGO";
						Debug.Log ("Fin del juego");
                        StopAllCoroutines();
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

    public void ReanudarJuego() {
        pnlPausa.SetActive(true);
        pnlPausa.transform.localScale = Vector3.zero;
        MostrarFigurasObj();
    }     

    public string VerAyuda(string corte, string fig){
		string respuesta = "";
		string imagen = GetNomImg (target);
        Debug.Log("corte  " + corte + "   fig   " + fig);
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

    public void ObtenerAyuda() {     
        nAyudas++;
        txtAyudas.text = nAyudas.ToString();
        PausarJuego();
        pnlPausa.GetComponent<RectTransform>().localScale = new Vector3(0.31f, 0.5f, 0.3f);
        txtAviso = GameObject.Find("txtAviso").GetComponent<Text>();
        txtAviso.text = "Necesitas encontrar las dos mitades que forman la figura de la imagen";
        Debug.Log(target);
        VerAyuda(txtTarget.text, target);
    }
}