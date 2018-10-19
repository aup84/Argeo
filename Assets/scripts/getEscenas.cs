using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class getEscenas : MonoBehaviour {
	private ArrayList escenas = new ArrayList();
	private string setEscena = "menu";

	// Use this for initialization
	void Start () {
		escenas.Add ("menu");
        escenas.Add ("ArenaScene");
		escenas.Add ("ConstruirFig");
		escenas.Add ("VolFiguras");
		escenas.Add ("AreaFiguras");
        escenas.Add("newMenu");
        Debug.Log("Escenas cargadas:  "+ SceneManager.sceneCount);
	}
	
	// Update is called once per frame
	public void mainMenu(){
		int indice = 0;
		OnSetScene (indice);
	}

    public void NewmainMenu()
    {
        int indice = 5;
        OnSetScene(indice);
    }

    public void OnClicArenaColliders(){
		int indice = 1;
		OnSetScene(indice);
	}

	public void OnClicConstruirFig(){
		int indice = 2;
		OnSetScene(indice);
	}
		
	 
	public void OnclicVolFiguras(){
		int indice = 3;
		OnSetScene(indice);
	}
	public void OnclicAreaFiguras(){
		int indice = 4;
		OnSetScene(indice);
	}

	public void OnClickQuit() {
		Application.Quit ();
	}


	public void OnSetScene(int indice){
		setEscena = escenas [indice].ToString();
		Debug.Log ("Se ha cargado la escena " + setEscena);
		SceneManager.LoadScene (setEscena);
	}
}