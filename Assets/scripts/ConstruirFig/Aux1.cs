using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aux1 {

    public int Nivel { get; set; }
    public string Descripcion { get; set; }
    public int Indice { get; set; }
	public string FiguraConcepto { get; set; }
	public string Corte { get; set; }

    public Aux1(int Indice, string Descripcion, string FiguraConcepto, string Corte, int Nivel)
	{
		this.Indice = Indice;
		this.Descripcion = Descripcion;
		this.FiguraConcepto = FiguraConcepto;
		this.Corte = Corte;
        this.Nivel = Nivel;
	}	
}
