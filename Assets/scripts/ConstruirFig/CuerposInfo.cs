using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class CuerposInfo{
	string nomFigura;
	string nomImagen;
	List<DatosFigura> datos;
	List<DatosFigura> terminados;
	List <string> gameTargets;

	public CuerposInfo(){
		datos = new List<DatosFigura> ();
		terminados = new List<DatosFigura> ();
		gameTargets = new List<string> ();
        datos = DatosF();
		foreach (DatosFigura t in datos) {
			gameTargets.Add (t.Descripcion + "A");
			gameTargets.Add (t.Descripcion + "B");
			terminados.Add (t);
		}
		Debug.Log ("Datos Cargados Satisfactoriamente. Elementos en pantalla " + gameTargets.Count + ". Targets" + terminados.Count);
	}

    public List<DatosFigura> DatosF() {
        List<DatosFigura> cuerpos = new List<DatosFigura>();
        cuerpos.Add(new DatosFigura(0, "Cil_Paralelo_Base", "Cilindro Paralelo a la Base", "Paralelo a la Base", 1));
        cuerpos.Add(new DatosFigura(1, "Cil_Perpendicular_Base", "Cilindro Perpendicular a la Base", "Perpendicular a la Base", 1));
        cuerpos.Add(new DatosFigura(2, "Cilindro_Oblicuo", "Cilindro Oblicuo a la Base", "Oblicuo a la Base", 2));
        cuerpos.Add(new DatosFigura(3, "Cono_Paralelo_Base", "Cono Paralelo a la Base", "Paralelo a la Base", 2));
        cuerpos.Add(new DatosFigura(4, "Cono_Perpendicular_Base", "Cono Perpendicular a la Base", "Perpendicular a la Base", 2));
        cuerpos.Add(new DatosFigura(5, "Cono_Oblicuo_Base", "Cono Oblicuo a la Base", "Oblicuo a la Base", 3));
        cuerpos.Add(new DatosFigura(6, "Cono_Paralelo_Generatriz", "Cono Paralelo a la Generatriz", "Paralelo a la Generatriz", 3));
        return cuerpos;
    } 
		/*

	public void Orden(){		//Ordenar
		if (terminados.Count > 0) {			
			var rnd = new System.Random ();
			terminados = terminados.OrderBy (item => rnd.Next ()).ToList();
		}
	}
*/
	//Obtener Busqueda por una descripcion
	public string GetDescripcion(string concepto){
		if (datos.Count > 0) {			
			var temp = datos.Find (x => x.Descripcion == concepto);
			return temp.FiguraConcepto;
		}
		return "Elemento no encontrado";
	}

	public bool RevisaTerminados(string concepto){
		if (terminados.Count > 0) {
			var temp = terminados.Find (x => x.Descripcion == concepto);
			if (temp != null) {
				Debug.Log ("No existen en la lista, puedes seguir");
				return true;
			}
		}	Debug.Log ("Dato existente, vuelve a intentar");
		return false;
	}

    public DatosFigura GetNextItem(int Nivel){
        DatosFigura df = null;
        if (terminados.Count > 0) { 
            var temp = terminados.Where(x => x.Nivel == Nivel);
            Debug.Log("Elementos restantes: " + terminados.Count);
            if (temp.Count() > 0){
                df = temp.ElementAt(0);
                Debug.Log("Proximo Blanco:  " + df.Descripcion);
            }
            else
            {
                Debug.Log("No detecta mas elementos de este nivel");
                var temp2 = DatosF().Where(x => x.Nivel == Nivel);
                int indice = 0;
                if (temp2.Count() != 1) {
                    System.Random r = new System.Random();
                    indice = r.Next(0, temp2.Count());
                    indice= (r.NextDouble() >= 0.5) ? 1: 0; 
                }                
                df = temp2.ElementAt(indice);
            }
        }
        return df;
    }

	public void QuitarElemento(string concepto){
		if (gameTargets.Count > 0 && terminados.Count > 0) {						
			gameTargets.RemoveAll(x => x.Contains(concepto));
			terminados.RemoveAll(x => x.Descripcion.Contains(concepto));
			Debug.Log ("Elementos restantes:  " + gameTargets.Count + "\t Terminados:  " + terminados.Count );
		}
	}
}