using System;
using System.Data;
using System.IO;
using System.Text;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuardarDatos {
    private string connection;
    private IDbConnection dbCon;
    private IDbCommand dbCmd;
    private IDataReader reader;

    private string ejercicio, dificultad;
    private int correctos, errores, puntos, nivel;
    private double tiempo;

   
    public GuardarDatos(string ejercicio, int correctos, int errores, int puntos, double tiempo, int nivel, string dificultad)
    {
        this.ejercicio = ejercicio;
        this.correctos = correctos;
        this.errores = errores;
        this.puntos = puntos;
        this.tiempo = tiempo;
        this.nivel = nivel;
        this.dificultad = dificultad;
    }

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
        dbCon = new SqliteConnection(connection);
        dbCon.Open();
    }

    public void CerrarConexion()
    {
        reader.Close();
        reader = null;
        dbCmd.Dispose();
        dbCmd = null;
        dbCon.Close();
        dbCon = null;
        Debug.Log("Cerrando Conexion");
    }

    public void Insert()
    {   
        AbrirConexion();
        dbCmd = dbCon.CreateCommand(); //   La consulta de la tabla en la base de datos
        Debug.Log("Antes de Agregar");

        dbCmd.CommandText = "select count(*) from ejercicios";
        reader = dbCmd.ExecuteReader();     //Ejecuta la consulta
        int num = 0;
        while (reader.Read()){
            num = reader.GetInt32(0) + 1;
        }
        reader.Close();
        reader = null;
        tiempo = Math.Round(tiempo, 2);
        dbCmd.CommandText = "INSERT INTO EJERCICIOS (folio, idEjercicio, idUsuario, aciertos, errores, tiempo, puntos, nivel, dificultad, fecha) " +
            "VALUES (" + num + ","+ ejercicio +", '" + PlayerPrefs.GetString("id") + "'," + correctos + "," + 
            errores +", " + tiempo +"," + puntos + "," + nivel + ",'" + dificultad +"', '" +
            DateTime.Now.ToShortDateString() + "')";            
        Debug.Log(dbCmd.CommandText);
        dbCmd.ExecuteNonQuery();
        Debug.Log("Se ha AGREGADO el registro");      
        dbCmd.Dispose();
        dbCmd = null;
        dbCon.Close();
        dbCon = null;
        Debug.Log("Cerrando Conexion \n");
    }
}
