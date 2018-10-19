using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NewButton : MonoBehaviour {
    public void OnMathSelect(){
        Texture2D textura;
        textura = new Texture2D(150, 150, TextureFormat.RGBA32, false);

        if (Application.isMobilePlatform)
        {
            string ruta = Application.streamingAssetsPath + "/images/" + "math1.png";

            WWW www = new WWW(ruta);
            while (!www.isDone)
            {
            }
            www.LoadImageIntoTexture(textura);
        }
        else
        {
            textura.LoadImage(File.ReadAllBytes(Application.dataPath + "/images/" + "math1.png"));
        }
        GameObject.Find("imgCategoria").GetComponent<RawImage>().texture = textura;
    }

    public void OnReturnSelected()
    {
        Texture2D textura;
        textura = new Texture2D(150, 150, TextureFormat.RGBA32, false);

        if (Application.isMobilePlatform)
        {
            string ruta = Application.streamingAssetsPath + "/images/" + "cr2.png";

            WWW www = new WWW(ruta);
            while (!www.isDone)
            {
            }
            www.LoadImageIntoTexture(textura);
        }
        else
        {
            textura.LoadImage(File.ReadAllBytes(Application.dataPath + "/images/" + "cr2.png"));
        }
        GameObject.Find("imgCategoria").GetComponent<RawImage>().texture = textura;
    }

    public void OnReturn(){
        GameObject.Find("btnMathPanel").GetComponent<Button>().enabled = false;
        GameObject.Find("btnMathPanel").GetComponent<Button>().enabled = true;
        GameObject.Find("btnMathPanel").GetComponent<Animator>().SetTrigger("Normal");
    }
}