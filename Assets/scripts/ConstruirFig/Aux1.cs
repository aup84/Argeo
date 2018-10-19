using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aux1 {
	private string desc;
	private int indice;
	private string figuraConcepto;
	private string corte;

	public Aux1(int indice, string desc, string figuraConcepto, string corte)
	{
		this.indice = indice;
		this.desc = desc;
		this.figuraConcepto = figuraConcepto;
		this.corte = corte;
	}
	public void SetCorte(string corte){
		this.corte = corte;
	}

	public string getCorte(){
		return corte;
	}

	public void SetDesc(string desc){
		this.desc = desc;
	}
		
	public string getDesc(){
		return desc;
	}
	public void SetIndice(int indice){
		this.indice = indice;
	}

	public int getIndice(){
		return indice;
	}
	public void SetFiguraConcepto(string figuraConcepto){
		this.figuraConcepto = figuraConcepto;
	}

	public string getFiguraConcepto(){
		return figuraConcepto;
	}
}
