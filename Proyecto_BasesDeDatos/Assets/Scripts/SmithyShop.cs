using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmithyShop : MonoBehaviour
{
    //hacemos referencia al script de la base de datos para usar la información contenida en ella
    DataBase Smithy = new DataBase();
    
    //Creamos las funciones para añadir los diversos objetos a la base de datos
    public void BuyPickaxe()
    {
        Smithy.AddTool("Pickaxe", 15, 50);
    }

    public void BuyAxe()
    {
        Smithy.AddTool("Axe", 12, 60);
    }

    public void BuyHoe()
    {
        Smithy.AddTool("Hoe", 10, 40);
    }
}
