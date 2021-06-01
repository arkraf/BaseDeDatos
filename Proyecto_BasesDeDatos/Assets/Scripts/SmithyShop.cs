using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmithyShop : MonoBehaviour
{
    DataBase Smithy = new DataBase();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void BuyPickaxe()
    {
        Smithy.AddTool("Pickaxe", 15, 50);
    }

    public void BuyAxe()
    {
        Smithy.AddTool("Axe", 15, 50);
    }

    public void BuyHoe()
    {
        Smithy.AddTool("Hoe", 15, 50);
    }

    public void Salir()
    {

    }
}
