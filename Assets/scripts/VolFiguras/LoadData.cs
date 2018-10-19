using System;
using System.Data;
using System.IO;
using System.Text;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
public class LoadData {
    private string connection;
    private IDbConnection dbCon1;
    private IDbCommand dbCmd1;
    private IDataReader reader1;
    List<FiguraGeo> lista;
  
    public void AbrirConexion()
    {
        string p = "DataBase.db";
        string filePath;
        if (Application.isMobilePlatform)
        {
            filePath = Application.persistentDataPath + "/" + p;
        }
        else
        {
            filePath = Application.streamingAssetsPath + "/" + p;
        }

        connection = "URI=file:" + filePath;
        Debug.Log("Conectando a la base de datos  " + connection);
        dbCon1 = new SqliteConnection(connection);
        dbCon1.Open();
    }

    public void CerrarConexion()
    {
        dbCmd1.Dispose();
        dbCmd1 = null;
        dbCon1.Close();
        dbCon1 = null;
        
        Debug.Log("Cerrando Conexion");
    }

    public List <FiguraGeo> CrearLista(){
        lista = new List<FiguraGeo>();
        AbrirConexion();
        dbCmd1 = dbCon1.CreateCommand(); //   La consulta de la tabla en la base de datos

        dbCmd1.CommandText = "select * from VOLFIGURAS order by Nivel";
        reader1 = dbCmd1.ExecuteReader();     //Ejecuta la consulta
        while (reader1.Read())
        {
            FiguraGeo r3 = new FiguraGeo
            {
                IdFigura = reader1.GetInt32(0),
                Figura = reader1.GetString(1),
                Marcador = reader1.GetString(2),
                Lado = reader1.GetFloat(3),
                Apotema = reader1.GetFloat(4),
                Altura = reader1.GetFloat(5),
                Area = reader1.GetFloat(6),
                Volumen = reader1.GetFloat(7),
                Nivel = reader1.GetInt32(8)
            };
             lista.Add(r3);
            
            //lista.Add(r3);
        }
        reader1.Close();
        reader1 = null;
        CerrarConexion();
        string stri = "";
        lista.ForEach(item => stri += item.Figura + ", ");
        Debug.Log(stri);
        
        return lista;        
    }
    public List<FiguraGeo> RecuperarLista() {
        return lista;
    }
}