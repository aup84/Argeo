using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class Juego : MonoBehaviour
{
    Text txtCorrectos, txtBase, txtAltura, txtErrores, txtTarget, txtTiempo2, txtAyudas, txtFind, lblInfo, txtStatus, lblObjetivo, txtId;
    private int finished = 0;
    static int errores, correctos;
    int IdFig = 0, nAyudas = 0, level;
    string target, find;
    static GameObject pnlValidar;
    GameObject pnlPausa, pnlAyuda;
    ReglasJuego reglas;
    static List<FiguraGeo> resul;
    List<FiguraGeo> terminados;
    public float valResultado;
    bool resultado = false, detener;
    double tiempo, tiempo2, tempTiempo;
    Text txtTiempo;
    FuzzyLogic fl;

    void Start(){
        tiempo = 0d;
        txtBase = GameObject.Find("txtBase").GetComponent<Text>();
        txtId = GameObject.Find("txtId").GetComponent<Text>();
        txtTiempo = GameObject.Find("txtTiempo").GetComponent<Text>();
        txtTiempo2 = GameObject.Find("txtTiempo2").GetComponent<Text>();
        txtAyudas = GameObject.Find("txtAyudas").GetComponent<Text>();
        txtAltura = GameObject.Find("txtAltura").GetComponent<Text>();
        txtCorrectos = GameObject.Find("txtCorrectos").GetComponent<Text>();
        txtErrores = GameObject.Find("txtErrores").GetComponent<Text>();
        lblInfo = GameObject.Find("lblInfo").GetComponent<Text>();
        txtTarget = GameObject.Find("txtTarget").GetComponent<Text>();
        txtFind = GameObject.Find("txtFind").GetComponent<Text>();
        txtStatus = GameObject.Find("txtStatus").GetComponent<Text>();
        lblObjetivo = GameObject.Find("lblObjetivo").GetComponent<Text>();
        pnlPausa = GameObject.Find("pnlPausa");
        pnlPausa.GetComponent<RectTransform>().localScale = Vector3.zero;
        pnlAyuda = GameObject.Find("pnlAyuda");
        pnlAyuda.GetComponent<RectTransform>().localScale = Vector3.zero;
        pnlValidar = GameObject.Find("pnlValidar");
        detener = false;
        errores = 0;
        correctos = 0;
        tiempo2 = 0;
        level = 1;
        tempTiempo = 0;
      
        reglas = ReglasJuego.Instance;
        resul = reglas.Datos;

        pnlValidar.GetComponent<RectTransform>().localScale = Vector3.zero;
        SigObj(level);
        StartCoroutine("ControlTiempo");
    }

    IEnumerator ControlTiempo(){
        try{
            tiempo = double.Parse(txtTiempo.text, System.Globalization.NumberStyles.Any);
            tiempo2 = double.Parse(txtTiempo2.text, System.Globalization.NumberStyles.Any);
            tiempo += Time.deltaTime;
            tiempo2 += Time.deltaTime;
            txtTiempo.text = "" + tiempo.ToString("F");
            txtTiempo2.text = "" + tiempo2.ToString("F");
        }
        catch (Exception e) {
            Debug.Log(e.Message);
        }
        yield return new WaitForSeconds(0);
    }

    public void PausarTiempo(){
        Debug.Log("Parando Rutina Tiempo");
        StopCoroutine("ControlTiempo");
    }

    void Update() {           // Update is called once per frame
        if (detener == false) {
            StartCoroutine("ControlTiempo");
        }
    }

    public void SigObj(int nivel){
        pnlPausa.SetActive(true);
        pnlPausa.GetComponent<RectTransform>().localScale = Vector3.zero;

        FiguraGeo f2 = reglas.GetNext(nivel);
        
        IdFig = f2.IdFigura;
        txtId.text = IdFig.ToString();

        target = f2.Figura;
        txtTarget.text = target;
        Debug.Log("ID Figura:  " + IdFig + "    Target" + target);
    }


    public void GetLista(){
        terminados = reglas.getLista();
    }

    public static List<FiguraGeo> Listado(){
        return resul;
    }

    IEnumerator MostrarPanel(bool result, string aviso, string opcion){
        pnlPausa.SetActive(true);
        pnlPausa.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.5f, 1f);
        RawImage raw2 = GameObject.Find("RawPausa").GetComponent<RawImage>();
        Text txtAviso = GameObject.Find("txtAviso").GetComponent<Text>();
        txtAviso.text = aviso;
        string arch;
        if (result)
        {
            arch = "correcto.jpg";
            raw2.GetComponent<RectTransform>().localScale = new Vector3(2f, 2f, 2f);
        }
        else
        {
            if (opcion.Equals("Volumen"))
            {
                arch = "ErrorV.png";
            }
            else
            {
                arch = GetImgAyuda();
            }
            raw2.GetComponent<RectTransform>().localScale = new Vector3(3f, 2f, 2f);
        }
        Texture2D textura = new Texture2D(150, 150, TextureFormat.RGBA32, false);
        if (Application.isMobilePlatform)
        {
            string ruta = Application.streamingAssetsPath + "/images/" + arch;

            WWW www = new WWW(ruta);
            while (!www.isDone)
            {
            }
            www.LoadImageIntoTexture(textura);
        }
        else
        {
            textura.LoadImage(File.ReadAllBytes(Application.dataPath + "/images/" + arch));
        }
        raw2.texture = textura;
        ClearInputField();
        yield return new WaitForSeconds(4);
        pnlPausa.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    }

    public static float getVolumen(string concepto, int campo, int idFigura)
    {
        if (resul.Count > 0)
        {
            Debug.Log("Concepto: " + concepto + " TIpo: " + campo + " idFigura: " + idFigura);
            var temp = resul.Find (r3 => r3.Figura == concepto && r3.IdFigura == idFigura);
            
            switch (campo)
            {
                case 1:
                    return temp.Lado;
                case 2:
                    return temp.Altura;
                case 3:
                    return temp.Apotema;
                case 4:
                    return temp.Volumen;
                case 5:
                    return temp.Area;
                case 6:
                    return temp.Nivel;
            }
        }
        return -1;
    }
   
    public void OcultarPanel(){
        pnlPausa.GetComponent<RectTransform>().localScale = Vector3.zero;
        pnlAyuda.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    void OcultarGameObjects() {
        txtBase.text = "";
        GameObject.Find("txtApotema").GetComponent<Text>().text = "";
        GameObject.Find("lblApotema").GetComponent<Text>().text = "";
        lblInfo.text = "Area";
        txtAltura.text = "";
        ClearInputField();
    }

    public int GetListSize()
    {
        GetLista();
        Debug.Log(terminados.Count);
        return terminados.Count;
    }

    public float GetResultado(string texto)
    {
        valResultado = float.Parse(texto);
        return valResultado;
    }

    public void RespuestaCorrecta(Text cant)
    {
        string str = "";
        string opcion = "";
        GetLista();
        //if (terminados.Count > resul.Count() * 0.40)
        if (finished < 10) {
            if (txtStatus.text.Equals(""))
            {
                Debug.Log("Lo detecta como vacio");
                return;
            }
            float respuesta;
            float cantidad = float.Parse(cant.text);

            if (lblInfo.text.Contains("Area"))
            {
                respuesta = getVolumen(txtTarget.text, 5, IdFig);
                if (respuesta <= (cantidad + 0.1) && respuesta >= (cantidad - 0.1))
                {
                    resultado = true;
                    str = "Correcto, presiona continuar para Calcular el Volumen";
                    lblObjetivo.text = "Escribe en el cuadro de texto el valor del Volumen de " + txtTarget.text;
                    lblInfo.text = "Volumen";
                    txtStatus.text = "Area: " + cantidad;
                    GameObject.Find("txtAltura").GetComponent<RectTransform>().localScale = new Vector3(1.25f, 28f, 1f);
                    GameObject.Find("lblAltura").GetComponent<RectTransform>().localScale = new Vector3(1.25f, 28f, 1f);
                }
                else
                {
                    errores = Convert.ToInt32(GameObject.Find("txtErrores").GetComponent<Text>().text);
                    errores++;
                    txtErrores.text = errores.ToString();
                    str = "Incorrecto. El Área de la base de un " + txtTarget.text + " se calcula";
                    resultado = false;
                    opcion = "Area";                 
                }
            }
            else
            {
                respuesta = getVolumen(txtTarget.text, 4, IdFig);
                if (respuesta <= (cantidad + 0.1) && respuesta >= (cantidad - 0.1))
                {
                    resultado = true;
                    correctos++;
                    txtCorrectos.text = correctos.ToString();
                    finished++;
                    str = "Correcto, presiona continuar para el siguiente objetivo";
                    lblObjetivo.text = "Coloca sobre la cámara el marcador de la figura que se te pide";
                    txtFind.text = "";
                    OcultarGameObjects();
                    
                    reglas.QuitarElemento(txtTarget.text, IdFig);
                    Debug.Log("Se ha eliminado " + txtTarget.text + "  con el ID " + IdFig);
                    reglas.VerListaCompleta();

                    //Fuzzy Logic
                    tempTiempo = Math.Round(tiempo2 / 150, 3);
                    tempTiempo = (tempTiempo < 1) ? tempTiempo : 1d;
                    txtTiempo2.text = "0.0";
                    tiempo2 = 0;
                    double denominador = correctos + errores + nAyudas;
                    FuzzyLogic fuzzy = new FuzzyLogic(correctos / denominador, errores / denominador, tempTiempo, nAyudas / denominador);
                    double d = fuzzy.Inferencias();
                    int level = fuzzy.FuzzyToCrisp(d);

                    int ant = Convert.ToInt32(GameObject.Find("txtNivel").GetComponent<Text>().text);
                    if ((ant - level) == 0 || (ant - level) == 1 || (ant - level) == -1)
                        {
                        Debug.Log("Se queda en el nivel " + level);
                    }
                    else if ((ant - level) < 0) {                         
                  //      Debug.Log(" Se procede a cambiar del nivel " + level + " al  " + (ant+1));
                        level = ant + 1;
                    }
                    else {
                  //      Debug.Log(" Cambiando de nivel " + level + " al nivel " + (ant-1));
                        level = ant - 1;
                    }

                    GameObject.Find("txtNivel").GetComponent<Text>().text = level.ToString(); // + "    " + Math.Round(d,4);
                    Debug.Log("txtNivel se ha actualizado a : " + level);
                    //Termina Fuzzy Logic

                    
                    //Determina el siguiente objetivo   
                    SigObj(level);
                    pnlValidar.GetComponent<RectTransform>().localScale = Vector3.zero;
                    GameObject.Find("pnlDatos").GetComponent<RectTransform>().localScale = Vector3.zero;
                    lblInfo.text = "Area";
                    
                    Avance();
                }
                else
                {
                    opcion = "Volumen";
                    errores = Convert.ToInt32(GameObject.Find("txtErrores").GetComponent<Text>().text);                    
                    errores++;
                    txtErrores.text = errores.ToString();
                    str = "Incorrecto, Te recordamos que la fórmula del Volumen es:";
                    resultado = false;
                }
            }
            StartCoroutine(MostrarPanel(resultado, str, opcion));
            ClearInputField();
        }
        else if (terminados.Count <= resul.Count() *0.40)
        {
            Avance();
        }
    }

    public bool Avance()    {
        //if (terminados.Count <= resul.Count * 0.4)       { //el juego termina cuando quede el 20% de los ejercicios

        if (finished >= 10) { 
            detener = true;
            Vuforia.CameraDevice.Instance.Stop();
            GameObject.Find("ARCamera").GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            GameObject.Find("txtAviso").GetComponent<Text>().text = "FIN DEL JUEGO";
            txtTarget.text = "FIN DEL JUEGO";
            PausarTiempo();
            int pto = ((correctos * 2) - errores) * 100;
            GuardarDatos g = new GuardarDatos("1", correctos, errores, pto, tiempo, 1, "Facil");
            g.Insert();
            return false;
        }
        return true;
    }

    public string GetImgAyuda(){
        string arch = "";
        switch (target){
            case "Prisma Triangular":
                arch = "AreaT.png";
                break;
            case "Prisma Cuadrangular":
                arch = "AreaC.png";
                break;
            case "Cilindro":
                arch = "AreaC2.png";
                break;
            default:
                arch = "AreaP.png";
                break;
        }
        return arch;
    }

    public void ClearInputField()
    {
        txtStatus.text = "";
        GameObject.Find("btnPunto").GetComponent<BtnAcciones>().enabled = true;
        GameObject.Find("btnPunto").GetComponent<Button>().enabled = true;
    }

    public void ObtenerAyuda() {
        string mensaje = "";
        nAyudas++;
        txtAyudas.text = nAyudas.ToString();

        if (pnlValidar.GetComponent<RectTransform>().localScale.y == 0)     {
            Ayudas ay = new Ayudas();            
            mensaje = ay.ObtenerAyuda(txtTarget.text);
            StartCoroutine(MostrarPanel2(mensaje));
        }
        else {
            string s = "";
            if (lblInfo.GetComponent<Text>().text.Contains("Area")) {
                s = "El Área de la base de un " + txtTarget.text + " se calcula";
                StartCoroutine(MostrarPanel(false, s, "Area"));
            }
            else if (lblInfo.GetComponent<Text>().text.Contains("Vol"))
            {                
                s = "El Volumen de un " + txtTarget.text + " se calcula";
                StartCoroutine(MostrarPanel(false, s, "Volumen"));
            }           
        }       
    }

    IEnumerator MostrarPanel2(string aviso){
        pnlAyuda.SetActive(true);
        pnlAyuda.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.5f, 1f);
        
        Text txtAvisoAyuda = GameObject.Find("txtAvisoAyuda").GetComponent<Text>();
        txtAvisoAyuda.text = aviso;
       
        yield return new WaitForSeconds(5);
        pnlAyuda.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    }
}
