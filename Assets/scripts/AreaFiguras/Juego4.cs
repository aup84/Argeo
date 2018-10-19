using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class Juego4 : MonoBehaviour {
	Text txtCorrectos, txtTiempo, txtBase, txtErrores, txtStatus, txtTarget, txtFind, lblInfo, lblBase, txtApotema, lblApotema, lblObjetivo;
	//InputField inputF;
    static int errores, correctos;
	string target, find;
	GameObject figura;
	static GameObject pnlValidar;
	GameObject pnlPausa;
	ReglasJuego4 reglas;
	static List<Resultados4> resul;
	List<string> terminados;
	public float valResultado;
	bool resultado = false, detener;
    double tiempo;

	public Juego4(){
		resul = new List<Resultados4> ();
		resul.Add (new Resultados4 ("Prisma Triangular", 4f,	10f, 6f, 120.0f, 12f));
		resul.Add (new Resultados4 ("Prisma Cuadrangular", 3f, 10f, 0f, 90f, 9f));
		resul.Add (new Resultados4 ("Prisma Pentagonal", 2.4f, 10.0f, 1.65f, 99.0f, 9.9f));
		resul.Add (new Resultados4 ("Prisma Hexagonal", 2.0f, 10.0f,	1.73f, 103.8f, 10.38f));
		resul.Add (new Resultados4 ("Prisma Decagonal", 1.2f, 10.0f, 1.85f, 111f, 11.1f));
		resul.Add (new Resultados4 ("Cilindro", 2.0f, 10.0f, 0f, 125.66f, 12.56f));
		errores = 0;
		correctos = 0;
		reglas = new ReglasJuego4 ();
	}

	void Start () {
		txtBase = GameObject.Find("txtBase").GetComponent<Text>();
        txtTiempo = GameObject.Find("txtTiempo").GetComponent<Text>();
        txtStatus = GameObject.Find("txtStatus").GetComponent<Text>();
		txtTarget = GameObject.Find("txtTarget").GetComponent<Text>();
		txtCorrectos = GameObject.Find("txtCorrectos").GetComponent<Text>();
		txtErrores = GameObject.Find("txtErrores").GetComponent<Text>();
		lblInfo = GameObject.Find("lblInfo").GetComponent<Text>();
		txtTarget = GameObject.Find("txtTarget").GetComponent<Text>();
		lblBase = GameObject.Find("lblBase").GetComponent<Text>();
		txtFind = GameObject.Find("txtFind").GetComponent<Text>();
		txtApotema = GameObject.Find ("txtApotema").GetComponent<Text> ();
		lblApotema = GameObject.Find ("lblApotema").GetComponent<Text> ();
		lblObjetivo = GameObject.Find ("lblObjetivo").GetComponent<Text> ();
		//inputF = GameObject.Find ("InputField").GetComponent<InputField>();
		pnlPausa = GameObject.Find("pnlPausa");
		pnlPausa.GetComponent<RectTransform> ().localScale = Vector3.zero;
		pnlValidar = GameObject.Find("pnlValidar");
		pnlValidar.GetComponent<RectTransform> ().localScale = Vector3.zero;
        SigObj();
        detener = false;
        tiempo = 0d;
		CambiarDatos ();

		if (Application.isMobilePlatform) {
			GameObject.Find ("btnValidar").GetComponent<Transform> ().localScale = Vector3.zero;
		}
	}
    IEnumerator ControlTiempo()
    {
        try
        {
            tiempo = double.Parse(txtTiempo.text, System.Globalization.NumberStyles.Any);
            tiempo += Time.deltaTime;
            txtTiempo.text = "" + tiempo.ToString("F");
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

    void Update()
    {           // Update is called once per frame
        if (detener == false)
        {
            {
                StartCoroutine("ControlTiempo");
            }
        }
    }

    public void SigObj(){
		pnlPausa.SetActive(true);
		pnlPausa.GetComponent<RectTransform> ().localScale = Vector3.zero;
		target = reglas.getNextItem();
		txtTarget.text = target;
	}

	public void GetLista(){
		terminados = reglas.getLista ();
	}
		
	IEnumerator MostrarPanel(bool result, string aviso, string opcion){
		pnlPausa.SetActive (true);
		pnlPausa.GetComponent<RectTransform> ().localScale = new Vector3 (0.3f, 0.5f, 1f);
		RawImage raw2 = GameObject.Find ("RawPausa").GetComponent<RawImage> ();
		Text txtAviso = GameObject.Find("txtAviso").GetComponent<Text>();
		txtAviso.text = aviso;
		string arch;
		if (result) {
			arch = "correcto.jpg";
			raw2.GetComponent<RectTransform> ().localScale = new Vector3 (2f, 2f, 2f);
		}
		else {
			if (opcion.Equals ("Volumen")) {
				arch = "ErrorV.png";
			} 
			else {
				arch = GetAyuda ();
			}
			raw2.GetComponent<RectTransform> ().localScale = new Vector3 (3f, 2f, 2f);
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
		yield return new WaitForSeconds (4);
		pnlPausa.GetComponent<RectTransform> ().localScale = new Vector3 (0,0,0);
	}

	public static float getVolumen(string concepto, int campo){
		if (resul.Count > 0) {			
			var temp = resul.Find (x => x.GetFigura () == concepto);
			switch (campo) {
			case 1:
				return temp.GetLado ();
			case 2:
				return temp.GetAltura ();
			case 3:
				return temp.GetApotema ();
			case 4:
				return temp.GetVolumen ();
			case 5:
				return temp.GetArea();
			}
		}
		return -1;
	}

	public void OcultarPanel(){
		pnlPausa.GetComponent<RectTransform> ().localScale = Vector3.zero;
	}

	void OcultarGameObjects(){
		txtBase.text = String.Empty;
		GameObject.Find ("txtApotema").GetComponent<Text> ().text = String.Empty;
		GameObject.Find ("lblApotema").GetComponent<Text> ().text = String.Empty;
		lblInfo.text = "Area de la Base";
		//txtAltura.text = String.Empty;
		ClearInputField ();
	}

	public int GetListSize(){
		GetLista ();
		Debug.Log (terminados.Count);
		return terminados.Count;
	}

	public float getResultado(string texto){
		valResultado = float.Parse (texto);
		return valResultado;
	}

	public void CambiarDatos(){
		txtBase.text = getVolumen (txtTarget.text, 1).ToString ();
	//	txtAltura.text = getVolumen (txtTarget.text, 2).ToString ();

		if (getVolumen (txtTarget.text, 3) != 0) {
			txtApotema.text = getVolumen (txtTarget.text, 3).ToString ();
			lblApotema.text = "Apotema";
			if (target.Contains ("Triang")) {
				lblApotema.text = "Altitud Triángulo";
			}
		} 
		else {
			txtApotema.text = String.Empty;
			lblApotema.text = String.Empty;
		}

		if (!target.Equals ("Cilindro")) {
			lblBase.text = "Lado";
		} 
		else {
			lblBase.text = "Radio";
		}
	}

	public void RespuestaCorrecta(Text cant){
		string str = String.Empty;
		GetLista ();
		if (terminados.Count > 2) {

			Debug.Log ("Con Calculadora Manual");
			if (txtStatus.text.Equals ("")) {
				Debug.Log ("Lo detecta como vacio");
				return;
			}
			float respuesta;
			Debug.Log ("Continuando....");
			float cantidad = float.Parse (cant.text);

			if (lblInfo.text.Contains ("Area")) {
				respuesta = getVolumen (txtTarget.text, 5);				
				if (respuesta <= (cantidad + 0.1) && respuesta >= (cantidad - 0.1)) {
					resultado = true;
					str = "Correcto, presiona continuar con el siguiente Prisma";
					correctos++;
					txtCorrectos.text = correctos.ToString ();
					reglas.QuitarElemento (txtTarget.text);
					txtFind.text = String.Empty;
					OcultarGameObjects ();
					SigObj ();
					CambiarDatos ();
					pnlValidar.GetComponent<RectTransform> ().localScale = Vector3.zero;
					GameObject.Find ("pnlDatos").GetComponent<RectTransform> ().localScale = Vector3.zero;
					Avance();
					lblObjetivo.text = "Coloca sobre la cámara el marcador de la figura que se te pide.";
				} 
				else {
					errores = int.Parse (txtErrores.text);
					errores++;
					txtErrores.text = errores.ToString ();
					str = "El Área es incorrecta, intente de nuevo";
					resultado = false;
				//	MostrarPanel (resultado, str);
				}
			} 
			StartCoroutine(MostrarPanel (resultado, str, "Area"));
			//MostrarPanel (resultado, str);
			//			Debug.Log (str);
		} 
		else if (terminados.Count <= 2) {
			Avance ();
		}
		GC.Collect ();
	}

	public bool Avance(){
		if (terminados.Count <= 2) {
			Vuforia.CameraDevice.Instance.Stop ();
			GameObject.Find ("ARCamera").GetComponent<Camera> ().clearFlags = CameraClearFlags.Skybox;
            detener = true;
            PausarTiempo();
            int pto = (correctos - errores) * 100;
            if (pto < 0)
            {
                pto = 0;
            }               
            GuardarDatos g = new GuardarDatos("4", correctos, errores, pto, tiempo, 1, "Facil");
            g.Insert();

			GameObject.Find ("txtAviso").GetComponent<Text> ().text = "FIN DEL JUEGO";
			pnlPausa.GetComponent<RectTransform> ().localScale = Vector3.zero;
			txtTarget.text = "FIN DEL JUEGO";	
			return false;
		}
		return true;
	}

	public void UnLoadMe(){
		Resources.UnloadUnusedAssets ();
		SceneManager.LoadScene ("menu",LoadSceneMode.Single);

	}
	public string GetAyuda(){
		string arch = "";
		switch (target) {
		case "Prisma Triangular": 
			arch = "AreaT.png"; 
			break;
		case "Prisma Cuadrangular": 
			arch = "AreaC.png"; 
			break; 
		case "Cilindro": 
			arch = "AreaC2.png"; 
			break;
		default:
			arch = "AreaP.png"; 
			break;
		}
		return arch;
	}

	public void ClearInputField(){
		txtStatus.text = "";
		GameObject.Find ("btnPunto").GetComponent<BtnAcciones> ().enabled = true;
		GameObject.Find ("btnPunto").GetComponent<Button> ().enabled = true;
	}
}