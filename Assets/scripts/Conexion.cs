using System.Data;
using System.IO;
using System.Text;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Conexion : MonoBehaviour
{
    InputField input1;    //ID
    InputField AddUser;
    InputField AddName;
    InputField addPass;
    InputField AddEscuela;
    InputField AddGrupo;
    InputField AddStatus;

    Text txtAcciones;
    private string connection;
    private IDbConnection dbCon;
    private IDbCommand dbCmd;
    private IDataReader reader;
    private string pass = "";
    //private int status = 0;

    private void Start()
    {
        txtAcciones = GameObject.Find("txtAcciones").GetComponent<Text>();

    }


    public void AbrirConexion() {
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

        if (!File.Exists(filePath))
        {
            CrearBaseDatos(filePath);
        }

        connection = "URI=file:" + filePath;
        Debug.Log("Conectando a la base de datos  " + connection);
        txtAcciones.text += "Conectando a la base de datos  " + connection + "\n";
        dbCon = new SqliteConnection(connection);
        dbCon.Open();
    }

    void CrearBaseDatos(string filePath){
        Debug.Log("Creando la base de datos");
        WWW loadDb = new WWW("jar:file//" + Application.dataPath + "!/assets/" + "Database.db");
        while (!loadDb.isDone) { }
        File.WriteAllBytes(filePath, loadDb.bytes);
        connection = "URI=file:" + filePath;
        dbCon = new SqliteConnection(connection);
        dbCon.Open();
        dbCmd = dbCon.CreateCommand();
        dbCmd.CommandText = "CREATE TABLE 'Usuarios' ('IdUsuario' TEXT NOT NULL UNIQUE, 'Nombre' TEXT NOT NULL, 'Pass' TEXT NOT NULL, 'status' INTEGER NOT NULL, 'Escuela' TEXT NOT NULL, 'Grupo' TEXT NOT NULL)";
        dbCmd.ExecuteNonQuery();
        dbCmd.CommandText = "CREATE TABLE 'SESION' ('IdUsuario' TEXT NOT NULL, 'Nombre' TEXT NOT NULL, 'LASTSESION' TEXT )";
        dbCmd.ExecuteNonQuery();
        dbCmd.CommandText = "INSERT INTO 'Usuarios' ('IdUsuario', 'Nombre', 'Pass', 'status', 'Escuela', 'Grupo')" +
            " VALUES ('0', 'Administrador', '123', '1', 'TecNM', '1A')";
        dbCmd.ExecuteNonQuery();
        dbCmd.CommandText = "INSERT INTO 'SESION' ('IdUsuario', 'Nombre', 'LASTSESION') VALUES ('" + 
            GameObject.Find("txtUsuario").GetComponent<Text>().text + "', '" +
            GameObject.Find("lblNombre").GetComponent<Text>().text + "' , '" +
            System.DateTime.Now.Date.ToShortDateString() + "')";
        dbCmd.ExecuteNonQuery();
        CerrarConexion();
        txtAcciones.text = "Se ha creado la base de datos \n";
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

    public void Consultar()
    {
        input1 = GameObject.Find("txtUsuario").GetComponent<InputField>();
        if (input1.text.Equals(""))
        {
            Debug.Log("NO SE PUEDEN CONSULTAR CLAVES VACIOS");
            return;
        }
        AbrirConexion();
        dbCmd = dbCon.CreateCommand();
        dbCmd.CommandText = "Select * from Usuarios where idUsuario = " + input1.text + " and status = 1";    // Es la consulta en codigo SQl
        reader = dbCmd.ExecuteReader();     //Ejecuta la consulta
        while (reader.Read())
        { //Recupera los datos del DataSet
            string id = reader.GetString(0);
            string nombre = reader.GetString(1);
            pass = reader.GetString(2);
            //status = reader.GetInt32(3);

            Debug.Log("ID:  " + id + "   Nombre: " + nombre + "  pass: " + pass);           
            GameObject.Find("lblNombre").GetComponent<Text>().text = nombre;
            GameObject.Find("txtPass").GetComponent<InputField>().ActivateInputField();
        }


        CerrarConexion();
        txtAcciones.text += "Cerrando Conexion \n";
    }

    public void Select()
    {
        Consultar();
    }

    public void Delete()
    {
        input1 = GameObject.Find("txtUsuario").GetComponent<InputField>();
        if (input1.GetComponent<Text>().text.Equals(""))
        {
            Debug.Log("NO SE PUEDEN ELIMINAR CAMPOS VACIOS");
            return;
        }
        AbrirConexion();

        dbCmd = dbCon.CreateCommand(); //   La consulta de la tabla en la base de datos
        Debug.Log("Antes de ELminiar");
        dbCmd.CommandText = "Update usuarios set status = 0 where idUsuario = " + input1.GetComponent<Text>().text;    // Es la consulta en codigo SQl.
        dbCmd.ExecuteNonQuery();
        Debug.Log("Se ha eliminado el registro");
        txtAcciones.text += txtAcciones.text + "Se ha eliminado el registro \n";
        dbCmd.Dispose();
        dbCmd = null;
        dbCon.Close();
        dbCon = null;
        txtAcciones.text += "Cerrando Conexion \n";
    }

    public bool RevisarInputs(){
        bool resultado = false;

        if (GameObject.Find("AddUsuario").GetComponent<InputField>().text.Equals("") || 
            GameObject.Find("InputNombre").GetComponent<InputField>().text.Equals("") ||
            GameObject.Find("InputPass").GetComponent<InputField>().text.Equals("") ||
            GameObject.Find("InputEscuela").GetComponent<InputField>().text.Equals("") ||
            GameObject.Find("InputGrupo").GetComponent<InputField>().text.Equals("")){
            resultado = true;
        }
        Debug.Log("Resultado" + resultado);
        return resultado;
    }

    public void Insert()
    {
        Debug.Log("Validando espaciados");
        if (RevisarInputs() == true){
            return;
        }
        AbrirConexion();
        dbCmd = dbCon.CreateCommand(); //   La consulta de la tabla en la base de datos
        Debug.Log("Antes de Agregar");
        dbCmd.CommandText = "INSERT INTO usuarios (idUsuario, nombre, pass, status, escuela, grupo) VALUES ('" +
            GameObject.Find("AddUsuario").GetComponent<InputField>().text + "', '" +
            GameObject.Find("InputNombre").GetComponent<InputField>().text + "', '" +
            GameObject.Find("InputPass").GetComponent<InputField>().text + "', 1, '" +
            GameObject.Find("InputEscuela").GetComponent<InputField>().text + "', '" +
            GameObject.Find("InputGrupo").GetComponent<InputField>().text + "')";
        Debug.Log(dbCmd.CommandText);
        dbCmd.ExecuteNonQuery();
        Debug.Log("Se ha AGREGADO el registro");
        txtAcciones.text += "Se ha agregado el registro \n";
        dbCmd.Dispose();
        dbCmd = null;
        dbCon.Close();
        dbCon = null;
        txtAcciones.text += "Cerrando Conexion \n";
    }

    public void Actualizar()
    {
        if (RevisarInputs() == false)
        {
            return;
        }
        AbrirConexion();

        dbCmd = dbCon.CreateCommand(); //   La consulta de la tabla en la base de datos
        Debug.Log("Antes de Agregar");
        dbCmd.CommandText = "UPDATE usuarios SET nombre = '" + input1.GetComponent<Text>().text + "', NOMBRE = '" + input1.GetComponent<Text>().text + "' WHERE IdUsuario =  " + input1.GetComponent<Text>().text;
        dbCmd.ExecuteNonQuery();
        Debug.Log("Se ha Actualizado el registro");
        txtAcciones.text += "Se ha Actualizado el registro \n";
        dbCmd.Dispose();
        dbCmd = null;
        dbCon.Close();
        dbCon = null;
        txtAcciones.text += "Cerrando Conexion \n";
    }

    public void Recargar()
    {
        SceneManager.LoadScene(0);
    }

    public void LogIn(){

        InputField txtUsuario = GameObject.Find("txtUsuario").GetComponent<InputField>();
        InputField txtPass = GameObject.Find("txtPass").GetComponent<InputField>();

        if (txtUsuario.text.Equals("") || txtPass.text.Equals("")) {
            Debug.Log("Favor de escribir usuario y contraseña");
            GameObject.Find("txtAcciones").GetComponent<Text>().text = "Falta usuario y contraseña";
            return;
        }

        string filePath;
        if (Application.isMobilePlatform)
        {
            filePath = Application.persistentDataPath + "/DataBase.db";

        }
        else
        {
            filePath = Application.streamingAssetsPath + "/DataBase.db";
        }

        if (!File.Exists(filePath))
        {
            CrearBaseDatos(filePath);
        }

        connection = "URI=file:" + filePath;
        Debug.Log("Conectando a la base de datos  " + connection);
        txtAcciones.text += "Conectando a la base de datos  " + connection + "\n";
        dbCon = new SqliteConnection(connection);

        dbCmd = dbCon.CreateCommand(); //   La consulta de la tabla en la base de datos
        Debug.Log("Select * from Usuarios where idUsuario = '" + txtUsuario.text + "' and status = 1");    // Es la consulta en codigo SQl)
        dbCon.Open();
        dbCmd.CommandText = "Select * from Usuarios where idUsuario = '" + txtUsuario.text + "' and status = 1";
        reader = dbCmd.ExecuteReader();     //Ejecuta la consulta

        while (reader.Read())
        { //Recupera los datos del DataSet
          //  string id = reader.GetString(0);
            string nombre = reader.GetString(1);
            pass = reader.GetString(2);
        //    status = reader.GetInt32(3);
            txtAcciones.text = "Conexion establecida. \n Usuario: " + nombre;
            GameObject.Find("lblNombre").GetComponent<Text>().text = nombre;
            GameObject.Find("txtUsuario").GetComponent<InputField>().text = nombre;
            GameObject.Find("txtPass").GetComponent<InputField>().text = pass.ToString();
        }
        reader.Close();
        reader = null;

        dbCmd.CommandText = "INSERT INTO 'Sesion' ('IdUsuario', 'Nombre', 'LastSesion') VALUES ('" +
                  GameObject.Find("txtUsuario").GetComponent<InputField>().text + "', '" +
                  GameObject.Find("lblNombre").GetComponent<Text>().text + "' , '" +
                  System.DateTime.Now.Date.ToShortDateString().ToString() + "')";
        dbCmd.ExecuteNonQuery();
        Debug.Log("Login registrado en la tabla sesion");
        PlayerPrefs.SetString ("id",txtUsuario.text);
        PlayerPrefs.SetString("nombre", GameObject.Find("txtUsuario").GetComponent<InputField>().text);


        dbCmd.Dispose();
        dbCmd = null;
        dbCon.Close();
        dbCon = null;

        Debug.Log ("Cerrando Conexion");
        GameObject.Find("MainPanel").GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
        GameObject.Find("LoginPanel").GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}