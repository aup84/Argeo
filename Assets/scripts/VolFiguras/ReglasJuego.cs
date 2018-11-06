using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class ReglasJuego
{
    List<FiguraGeo> datos;
    List<FiguraGeo> datosOriginales;
    List<FiguraGeo> terminados;
    private static ReglasJuego instance;

    private ReglasJuego() { }

    public List<FiguraGeo> Datos
    {
        get
        {
            return datos;
        }
    }

    public static ReglasJuego Instance
    {
        get
        {
            if (instance == null)
            {
                LoadData load = new LoadData();
                instance = new ReglasJuego();
                instance.datos = load.CrearLista();
                instance.terminados = instance.datos;
                instance.datosOriginales = new List<FiguraGeo> ();
            }
            return instance;
        }
    }
    
    public void VerListaCompleta()
    {       //Ordenar
        string stri = "Elementos: ";

        // Ordenando la listas por nivel        
        instance.datos.Sort((p, q) => p.Nivel.CompareTo(q.Nivel));
        instance.datos.ForEach(item => stri += item.Figura + ", ");
        Debug.Log(stri);
    }

    public string GetDescripcionMarker(string concepto, int idFig)
    {
        if (instance.Datos.Count > 0)
        {
            var temp = instance.terminados.Find(x => x.Marcador == concepto && x.IdFigura == idFig);
            Debug.Log("Concepto:  " + concepto + "    " + temp.Figura);
            return temp.Figura;
        }
        return "Elemento no encontrado";
    }

    public string GetDescripcion(string concepto)
    {
        if (instance.Datos.Count > 0)
        {
            var temp = instance.datos.Find(x => x.Marcador == concepto);
            return temp.Figura;
        }
        return "Elemento no encontrado";
    }

    public string GetNextItem(int idFig)
    {
        if (instance.terminados.Count > 0)
        {
            var temp = instance.terminados.Where(x => x.IdFigura == idFig);
            Debug.Log("Elementos restantes: " + instance.terminados.Count + ".  Nombre" + temp.First().Figura + "  . ID " + temp.First().IdFigura);
            if (temp.Count() > 0)
                return temp.First().Figura;
        }
        return "Elemento no encontrado";
    }

    public FiguraGeo GetNext(int nivel){        
        System.Random r = new System.Random();
        var temp = instance.terminados.Where(x => x.Nivel == nivel);       
        Debug.Log("Terminados Nivel:  " + nivel + "  le restan" + temp.Count());

        if (temp.Count() <= 0){
            Debug.Log("Colocando los datos de la tabla temporal datosOriginales");
            var g = instance.datosOriginales.Where(x => x.Nivel == nivel);
            Debug.Log("datosOrginales  " + datosOriginales.Count());
            foreach (FiguraGeo f in g)
            {
                instance.terminados.Add(f);
                Debug.Log("Agregando " + f.Figura);
            }
            instance.datosOriginales.RemoveAll(x => x.Nivel == nivel);
            temp = instance.terminados.Where(x => x.Nivel == nivel);
        }

        int indice = r.Next(0, temp.Count()-1);
        Debug.Log("Terminados Indice temporal " + indice);
        FiguraGeo f1 = temp.ElementAt(indice);
        return f1;
    }

    public FiguraGeo GetNextItemId2(int idFig)
    {
        FiguraGeo t = instance.Datos.Where(x => x.IdFigura == idFig).First();
        return t;
    }

    public void QuitarElemento(string concepto, int IdFig)
    {
        if (instance.terminados.Count > 0)
        {
            if (instance.terminados.Exists(x => x.IdFigura == IdFig))
            {
                int a = instance.terminados.FindIndex(x => x.IdFigura == IdFig);
                instance.datosOriginales.Add(instance.terminados.ElementAt(a));
                Debug.Log("retirados " + instance.datosOriginales.Count());
                instance.terminados.RemoveAt(a);
                Debug.Log("Retirando el objeto  " + concepto + " Elementos restantes:  <<" + instance.terminados.Count + " >>");
            }
            else
            {
                Debug.Log("No entro al eliminar el registro");
            }
        }
    }

    public List<FiguraGeo> getLista()
    {
        return instance.terminados;
    }
}
/*
public int GetNextItemId(int nivel) {
    System.Random r = new System.Random();
    int indice;
    if (terminados.Count > 0) {
        var temp = terminados.Where(x => x.Nivel == nivel);
        if (temp.Count() > 0)
        {
            indice = r.Next(0, temp.Count());
            return temp.ElementAt(indice).IdFigura;
        }           
        else
        {
            temp = datos.Where(x => x.Nivel == nivel);
            indice = r.Next(0, temp.Count());
            return temp.ElementAt(indice).IdFigura;
        }           
    }        
    Debug.Log("Se han terminado los ejercicios de este nivel");
    return -1;
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

public void Desordenar(){		//Ordenar
    if (terminados.Count > 0) {		
        Debug.Log ("Desordenando");	
        var rnd = new System.Random ();
        terminados = terminados.OrderBy (item => rnd.Next ()).ToList();
    }
}
*/

/*
 public FiguraGeo GetNext(int nivel)
{
    FiguraGeo f1 = null;
    System.Random r = new System.Random();
    int indice;
    var temp = instance.terminados.Where(x => x.Nivel == nivel);       
    Debug.Log("Terminados Nivel:  " + nivel + "  le restan" + temp.Count());

    if (temp.Count() > 0)
    {
        indice = r.Next(0, temp.Count());
        Debug.Log("Terminados Indice temporal " + indice);            
        f1 = temp.ElementAt(indice);
    }
    else
    {
        Debug.Log("Colocando los datos de la tabla temporal datosOriginales");
        var g = instance.datosOriginales.Where(x => x.Nivel == nivel);
        Debug.Log("datosOrginales  " + datosOriginales.Count());
        foreach(FiguraGeo f in g){
            instance.terminados.Add(f);
            Debug.Log("Agregando " + f.Figura); 
        }
        instance.datosOriginales.RemoveAll(x => x.Nivel == nivel);
        temp = instance.terminados.Where(x => x.Nivel == nivel);
        indice = r.Next(0, temp.Count()-1);
        Debug.Log("Terminados Indice temporal " + indice);
        f1 = temp.ElementAt(indice);
    }        
    return f1;
}

 */
