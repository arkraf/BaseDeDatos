using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class DataBase : MonoBehaviour
{

    public Text inventory;

    private string dbName = "URI=file:Inventory.db";
    // Start is called before the first frame update
    void Start()
    {
        CreateDatabase();

    }

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

    public void AddTool(string toolName, int weaponDamage, int toolPrice)
    {
        using(var connection = new SqliteConnection(dbName))
        {
             connection.Open();

            using(var command = connection.CreateCommand())
            {
                //creamos una tabla sino existe una con el mismo nombre
                command.CommandText = "INSERT INTO tools (name,damage,price) VALUES ('" + toolName + "', '"+ weaponDamage +"', '" + toolPrice + "');";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

    }

    public void DisplayTools()
    {

        inventory.text = "";

        using(var connection = new SqliteConnection(dbName))
        {
             connection.Open();

            using(var command = connection.CreateCommand())
            {
                //creamos una tabla sino existe una con el mismo nombre
                command.CommandText = "SELECT * FROM tools;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                
                    inventory.text +=reader["name"] + "\tDmg: " + reader["damage"] + "\tPrice:" + reader["price"] + "\n";
                
                    reader.Close();    
                }
            }
            connection.Close();
        }
    }
}
