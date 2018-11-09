using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class GameRules {
    Text txtFind, txtPuntos;
	string[,] targets = new string[14, 3]; //0 trackable, 1 fig.png, 2 descripcion
    string[] targets2 = new string[14]; //0 trackable, 1 fig.png, 2 descripcion
	int puntos;
    int maxFiguras;
    int aciertos;
    public int errores=0;
    Texture2D textura2;
	int level = 1;
	public bool pausar = false;
	public GameObject pnlPausa;
	public GameRules()
    {
        initFiguras();
    }

	public void initFiguras(){
        maxFiguras = 14;
		for (int i = 0; i < maxFiguras; i++) {
            if (i < 10){
               targets[i, 0] = "FIG1-0" + i; 
            }
            else
            {
                targets[i, 0] = "FIG1-" + i;
            }
			targets [i, 1] = "figura" + i;
		}
        targets[0,2] = "Esfera";
		targets[1,2] = "Cilindro";
		targets[2,2] = "Cilindro Oblicuo";
		targets[3,2] = "Cono";
		targets[4,2] = "Cubo";
		targets[5,2] = "Dodecaedro";
		targets[6,2] = "Pirámide Pentagonal";
		targets[7, 2] = "Octaedro";
		targets[8,2] = "Pirámide Truncada";
		targets[9,2] = "Prisma Pentagonal";
		targets[10,2] = "Tetraedro";
		targets[11,2] = "Cono Truncado";
        targets[12, 2] = "Prisma Triangular";
        targets[13, 2] = "Prisma Rectangular";

        for (int i = 0; i < maxFiguras;i++){
            targets2[i] = targets[i, 2];
        }
		//Vector3 escala = new Vector3 (0.3f, 0.5f, 1f);
	}

    public void desordenar(){
			// Knuth shuffle algorithm :: courtesy of Wikipedia :)
			for (int t = 0; t < targets2.Length; t++)
			{
				string tmp = targets2[t];
				int r = Random.Range(t, targets2.Length);
				targets2[t] = targets2[r];
				targets2[r] = tmp;
			}
        for (int i = 0; i < targets2.Length; i++){
            Debug.Log(targets2[i]);
        }
    }

	public string getDescripciones(int renglon, int columna){
		return targets[renglon, columna];
	}

	public string getObjetivo(int level)
	{
		return targets2[level];
	}

    public string getFiguraPrefab(string tb)
    {
        string resultado = "figura" + tb.Substring(5, 2);
        return resultado;
    }
   
    public void SetImgObjetivo(){ //Permite repetir elementos
		RawImage imgObjetivo = GameObject.Find("imgObjetivo").GetComponent<RawImage>();
		System.Random rnd = new System.Random();
        int tarFind = rnd.Next (1, maxFiguras);
        string resultado = getDescripciones(tarFind, 2);
		txtFind = GameObject.Find("txtFind").GetComponent<Text>();
		txtFind.text = resultado;
		textura2 = new Texture2D(150, 150, TextureFormat.RGBA32, false);

		if (Application.isMobilePlatform) {
			string ruta = Application.streamingAssetsPath + "/images/plano/P" + targets[tarFind, 0] + ".png"; 

			WWW www = new WWW (ruta);
			while (!www.isDone) {
			}
			www.LoadImageIntoTexture (textura2);
		} else {
			textura2.LoadImage(File.ReadAllBytes(Application.dataPath + "/images/plano/P" + targets[tarFind, 0] + ".png"));
		}
		imgObjetivo.texture = textura2;
    }
   
	public void SetImgObjetivo2()//No permite elementos
	{       
        RawImage imgObjetivo = GameObject.Find("imgObjetivo").GetComponent<RawImage>();
        string resultado = getObjetivo(level);
        string tarFind = "" + getTargetIndex(resultado);
        Debug.Log("targets[" + level + "] = " + resultado + "\t level " + level + "\t tarFind: " + tarFind);

        txtFind = GameObject.Find("txtFind").GetComponent<Text>();
		txtFind.text = resultado;
		textura2 = new Texture2D(150, 150, TextureFormat.RGBA32, false);
        if (tarFind.Length == 1)
            tarFind = "0" + tarFind;

		if (Application.isMobilePlatform) {
			string ruta = Application.streamingAssetsPath + "/images/plano/PFIG1-" + tarFind + ".png"; 

			WWW www = new WWW (ruta);
			while (!www.isDone) {
			}
			www.LoadImageIntoTexture (textura2);
		} else {
			textura2.LoadImage(File.ReadAllBytes(Application.dataPath + "/images/plano/PFIG1-" + tarFind + ".png"));
		}
        imgObjetivo.texture = textura2;
	}

	private int getTargetIndex(string nomFigura)
	{
		int i = 0;
		for (i = 0; i < maxFiguras; i++)
		{
			if (targets[i, 2] == nomFigura)
				break;
		}
		return i;
	}

	public bool Validar(string tb){
		bool result = false;
		txtFind = GameObject.Find ("txtFind").GetComponent<Text> ();
		Text txtAciertos = GameObject.Find("txtAciertos").GetComponent<Text>();
        txtPuntos = GameObject.Find("txtPuntos").GetComponent<Text>();

        puntos = int.Parse(txtPuntos.text);
        Text txtErrores = GameObject.Find("txtErrores").GetComponent<Text>();

        errores = int.Parse(txtErrores.text); 
        aciertos = int.Parse(txtAciertos.text);
        Debug.Log("txtfind.text = " + txtFind.text + "\t tb=" + tb);

		if (txtFind.text.Equals(tb)) {
			result = true;
			level++;
			puntos += 100;			
			int arch;
			if (errores < 5 &&  aciertos <10) {
				aciertos++;
				if (aciertos % 2 == 1) {
                    arch = (aciertos + 1) / 2;
				}
				else {
					arch = aciertos / 2;
				}

				//string ruta = Application.dataPath + "/icons/" + arch + ".png";
				RawImage imgStar = GameObject.Find ("imgStar").GetComponent<RawImage> ();
				Texture2D textura = new Texture2D (150, 150, TextureFormat.RGBA32, false);
				//textura.LoadImage (File.ReadAllBytes (ruta));

				if (Application.isMobilePlatform) {
					string ruta = Application.dataPath + "/icons/" + arch + ".png"; 

					WWW www = new WWW (ruta);
					while (!www.isDone) {
					}
					www.LoadImageIntoTexture (textura);
				}
				else {
					textura.LoadImage(File.ReadAllBytes(Application.dataPath + "/icons/" + arch + ".png"));
				}
					
				imgStar.texture = textura;
                GameObject.Find("txtAyuda").GetComponent<Text>().text = "";
				//MostrarPanel (result);
				pausar = true;
            }
            else{				
                Debug.Log("Se han alcanzado los 10 aciertos o 5 errores");
                Text txtAyuda = GameObject.Find("txtAyuda").GetComponent<Text>();
                txtAyuda.text = "Fin del juego porque has alncanzado los 10 aciertos o 5 errores";
            }
		}
		else {
			result = false;
			errores++;
			puntos -= 100;
            getAyuda(txtFind.text);
		}

		txtPuntos = GameObject.Find ("txtPuntos").GetComponent<Text> ();
		txtPuntos.text = puntos.ToString ();
        txtAciertos.text = aciertos.ToString();
        txtErrores.text = errores.ToString();
		pausar = true;
		return result;
	}
		

	public void MostrarPanel(bool result){
		Debug.Log ("Pausando juego");
		GameObject.Find ("pnlPausa").gameObject.GetComponent<RectTransform> ().localScale.Set (0.3f, 0.5f, 1f);
		RawImage raw2 = GameObject.Find ("RawPausa").GetComponent<RawImage> ();
		Text txtAviso = GameObject.Find("txtAviso").GetComponent<Text>();
		txtAviso.text = "Presiona el botón continuar para seguir al siguiente objetivo";
		string arch = "incorrecto.png";
		if (result) {
			arch = "correcto.jpg";
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


	void getAyuda(string target)
	{
		Text txtAyuda = GameObject.Find("txtAyuda").GetComponent<Text>();
		switch (target)
		{
			case "Esfera":
				txtAyuda.text = "La esfera es el conjunto de los puntos del espacio tridimensional que tienen la misma distancia a un punto fijo denominado centro; tanto el segmento que une un punto con el centro, como la longitud del segmento, se denomina radio.";
				break;
			case "Cilindro":
				txtAyuda.text = "El cilindro es una superficie cilíndrica que se forma cuando una recta, llamada generatriz gira alrededor de otra recta paralela, eje.";
				break;
			case "Cilindro Oblicuo":
				txtAyuda.text = "El Cilindro cuyas bases son oblicuas a las generatrices de la superficie cilíndrica.";
                break;
            case "Cono":
				txtAyuda.text = "Un Cono es un sólido de revolución generado por el giro de un triángulo rectángulo alrededor de uno de sus catetos. Al círculo conformado por el otro cateto se denomina base y al punto donde confluyen las generatrices se llama vértice.";
				break;
			case "Cubo":
				txtAyuda.text = "El Cubo es una figura con seis lados iguales ";
				break;
			case "Dodecaedro":
				txtAyuda.text = "El Dodecaedro es un sólido que dispone de doce caras. La suma del número de vértices y el número de caras de un dodecaedro regular es igual a 2 más el número de aristas";
				break;
            case "Octaedro":
                txtAyuda.text = "El Octaedro es un poliedro de ocho caras. Sus caras han de ser polígonos de siete lados o menos. Si las ocho caras del octaedro son triángulos equiláteros, iguales entre sí, el octaedro es convexo y se denomina regular.";
                break;
            case "Pirámide Truncada":
                txtAyuda.text = "Una Pirámide Truncada es un poliedro comprendido entre la base de la pirámide y un plano que corta a todas las aristas laterales";
                break;
           	case "Prisma Pentagonal":
				txtAyuda.text = "El Prisma Pentagonal es un prisma recto que tiene como bases dos pentágonos.";
				break;
			case "Tetraedro":
				txtAyuda.text = "El Tetraedro es un poliedro de cuatro caras. Si las cuatro caras del tetraedro son triángulos equiláteros, iguales entre sí, el tetraedro se denomina regular.";
				break;
			case "Cápsula":
				txtAyuda.text = "Una Cápsula tiene extremos cilóndrico encajado en forma rectangular circular.";
				break;
			case "Cono Truncado":
				txtAyuda.text = "Un Cono Truncado es un cuerpo geométrico que resulta al cortar un cono por un plano paralelo a la base y separar la parte que contiene al vértice.";
				break;
			case "Prisma Triangular":
				txtAyuda.text = "El Prisma Triangular es un prisma recto que tiene como bases dos triángulos equiláteros.";
				break;
			case "Prisma Rectangular":
                txtAyuda.text = "El Prisma Rectangular u Ortoedro es un poliedro cuya superficie está formada por dos rectángulos iguales y paralelos llamados bases y por cuatro caras laterales que son rectángulos paralelos";
				break;
			case "":
                txtAyuda.text = "Felicidades, has avanzado al nivel " + level;
				break;
			case "Salir":
				txtAyuda.text = "Fin de la Partidas";
				break;
		}
	}
	public bool Ccontinuar(){
		Debug.Log ("Proceso de pausado es: " + pausar);
		return pausar;
	}

}