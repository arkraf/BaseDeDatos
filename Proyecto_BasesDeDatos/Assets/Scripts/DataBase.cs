using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class DataBase : MonoBehaviour
{
    //Declaramos las variables necesarias
    public Text inventory;

    private string dbName = "URI=file:Inventory.db";
    // Start is called before the first frame update
    void Start()
    {
        //Creamos la base de datos
        CreateDatabase();
    }

    // En el update actualizaremos constantemente la base de datos para poder ver la inforción actual en cualquier momento
    void Update()
    {
        DisplayTools();
    }

    // Update is called once per frame

    public void CreateDatabase()
    {
        //Creamos la conexión con la base de datos
        using(var connection = new SqliteConnection(dbName))
        
        {
            connection.Open();

            using(var command = connection.CreateCommand())
            {
                //creamos una tabla sino existe una con el mismo nombre
                command.CommandText = "CREATE TABLE IF NOT EXISTS tools (name VARCHAR(12), damage INT, price INT);";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    //En esta función añadiremos los datos a la base de datos
    public void AddTool(string toolName, int weaponDamage, int toolPrice)
    {
        using(var connection = new SqliteConnection(dbName))
        {
             connection.Open();

            using(var command = connection.CreateCommand())
            {
                //añadimos a la tabla los datos proporcionados
                command.CommandText = "INSERT INTO tools (name,damage,price) VALUES ('" + toolName + "', '"+ weaponDamage +"', '" + toolPrice + "');";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

    }

    //Esta función se encarga de mostrar los datos almacenados en la base de datos
    public void DisplayTools()
    {

        inventory.text = "";

        using(var connection = new SqliteConnection(dbName))
        {
             connection.Open();

            using(var command = connection.CreateCommand())
            {
                //Seleccionamos la información que queremos mostrar
                command.CommandText = "SELECT * FROM tools;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())

                    //Mostramos por pantalla la información
                    inventory.text +=reader["name"] + "\tDmg: " + reader["damage"] + "\tPrice:" + reader["price"] + "\n";
                
                    reader.Close();    
                }
            }
            connection.Close();
        }
    }
}
