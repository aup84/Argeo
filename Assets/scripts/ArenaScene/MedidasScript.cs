using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedidasScript : MonoBehaviour {
	Dropdown drop;
    Text txtOpcion;
    void Start()
    {
		drop = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        drop.GetComponentInChildren<Text>().text = "Seleccione";
    }

    public void Actualizar(){
		drop = GameObject.Find("Dropdown").GetComponent<Dropdown>();
		float[] vect = new float[3];
		string [] texto = drop.captionText.text.Split ('x');
		for (int i=0; i < 3; i++){
			vect [i] = float.Parse (texto [i]);
		}
		Vector3 vec = new Vector3(vect[0], vect[1], vect[2]);
	
        txtOpcion = GameObject.Find("txtOpcion").GetComponent<Text>();
		txtOpcion.text = drop.captionText.text;
	
		GameObject.Find ("Dropdown").GetComponent<RectTransform> ().localScale = Vector3.zero;
		GameObject.Find ("lblDrop").GetComponent<Text> ().text = "Medida: " + drop.captionText.text;
		GameObject.Find ("pnlSize").GetComponent<RectTransform> ().localScale = Vector3.zero;

		Text txtInstrucciones = GameObject.Find("txtInstrucciones").GetComponent<Text>();
		txtInstrucciones.text = "Coloca el marcador Cubo frente a la Cámara \n y colisionalo con el Arenero";

		switch (drop.value) {
		case 0:
			vec = new Vector3 (1f, 0.1f, 1f);
		break;
		case 1:
			vec = new Vector3 (1f, 0.2f, 1f);
			break;
		case 2:
			vec = new Vector3 (1f, 0.5f, 1f);
			break;
		}
		GameObject.Find ("CuboArena").GetComponent<Transform> ().localScale = vec;
    }
		
    public int GetAltura(string opcion){
        string [] texto = drop.captionText.text.Split('x');
        int ix = int.Parse(texto[1]);
        return ix;
    }
}