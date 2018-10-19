public class Resultados4 {
	private string figura;
	private float lado;
	private float altura;
	private float volumen;
	private float apotema;
	private float area;

	public Resultados4(string figura, float lado, float altura, float apotema, float volumen, float area){
		this.figura = figura;
		this.lado = lado;
		this.altura = altura;
		this.apotema = apotema;
		this.volumen = volumen;
		this.area = area;
	}

	public float GetArea(){
		return area;
	}

	public void SetArea(float area){
		this.area = area;
	}

	public float GetApotema(){
		return apotema;
	}

	public void SetApotema(float apotema){
		this.apotema = apotema;
	}

	public string GetFigura(){
		return figura;
	}

	public void SetFigura(string figura){
		this.figura = figura;
	}

	public float GetVolumen(){
		return volumen;
	}

	public void SetVolumen(float volumen){
		this.volumen = volumen;
	}

	public float GetLado(){
		return lado;
	}

	public void SetLado(float lado){
		this.lado = lado;
	}
		
	public float GetAltura(){
		return altura;
	}

	public void SetAltura(float altura){
		this.altura = altura;
	}
}