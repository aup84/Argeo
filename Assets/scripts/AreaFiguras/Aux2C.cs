using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aux2C {
	private string desc;
	private int indice;
	private string figuraConcepto;

	public Aux2C(int indice, string desc, string figuraConcepto){
		this.indice = indice;
		this.desc = desc;
		this.figuraConcepto = figuraConcepto;
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