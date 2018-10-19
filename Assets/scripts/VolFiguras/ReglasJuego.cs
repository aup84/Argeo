using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class ReglasJuego {
	string nomFigura;
	string nomImagen;
	List<FiguraGeo> datos;
	List<FiguraGeo> terminados;
    static List<FiguraGeo> pendientes;
    Text txtErrores;
	private static ReglasJuego instance;


    private ReglasJuego() {
        
    }

    public List<FiguraGeo> Datos
    {
        get { return datos; }
    }

     public static ReglasJuego Instance{
		get{ 
			if (instance == null) {
                LoadData load = new LoadData();
                instance = new ReglasJuego ();
                instance.datos = load.CrearLista();
                instance.terminados = instance.datos;
                pendientes = new List<FiguraGeo>();
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

    public void VerListaCompleta()
    {       //Ordenar
        string stri="Elementos: ";
        datos.ForEach( item => stri+= item.Figura + ", ");
      //  Debug.Log(stri);
    }

    public string GetDescripcionMarker(string concepto, int idFig){
		if (datos.Count > 0) {			
			var temp = terminados.Find (x => x.Marcador == concepto && x.IdFigura == idFig);
			Debug.Log("Concepto:  " + concepto + "    " + temp.Figura);
            return temp.Figura;
		}
		return "Elemento no encontrado";
	}

    public string GetDescripcion(string concepto )
    {
        
        if (datos.Count > 0)
        {
            //Debug.Log("Concepto: " + concepto + "   " + datos.Count);
            var temp = datos.Find(x => x.Marcador == concepto);
            //Debug.Log("Concepto:  " + concepto + "    " + temp.Figura);
            return temp.Figura;
        }
        return "Elemento no encontrado";
    }


    public bool RevisaTerminados(string concepto){
		if (terminados.Count > 0) {
			var temp = terminados.Find (x => x.Figura == concepto);
			if (temp != null) {
				Debug.Log ("No existen en la lista, puedes seguir");
				return true;
			}
		}	Debug.Log ("Dato existente, vuelve a intentar");
		return false;
	}
		
	public string GetNextItem(){		
		if (terminados.Count > 0) {	
			//Debug.Log ("Elementos restantes: " + terminados.Count);
			return terminados.First().Figura;
		}
		return "Elemento no encontrado";
	}
    public int GetNextItemId()
    {
      //  Debug.Log(terminados.Count);
        if (terminados.Count > 0)
        {
       //     Debug.Log("Elementos Actual: " + terminados.First().IdFigura);
            return terminados.First().IdFigura;
        }
        return -1;
    }

    public string GetNextItem(int nivel)
    {
        if (terminados.Count > 0)
        {
            //Debug.Log ("Elementos restantes: " + terminados.Count);
            var temp = datos.Find(x => x.Nivel == nivel);
           // datos.Where(x => x.Nivel = nivel);

            return temp.Figura;
        }
        return "Elemento no encontrado";
    }


    public void QuitarElemento(string concepto, int IdFig){
		if (terminados.Count > 0) {						
			terminados.RemoveAll (x => x.Figura.Contains (concepto) && x.IdFigura == IdFig);
			Debug.Log ("Retirando el objeto  " + concepto + " <<Elementos restantes:  " + terminados.Count);
		}
		else {
			Debug.Log ("No entro al Metodo");
		}
	}

	public List<FiguraGeo> getLista(){
		return terminados;
	}

    public void ListToRepeat(FiguraGeo fig) {
        pendientes.Add(fig);
    }
}