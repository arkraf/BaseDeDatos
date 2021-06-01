using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Personaje : MonoBehaviour
{
    //Declaramos las variables necesarias para la información del personaje
    public string _name = "";
    public int _level = 7;
    public int _hp = 53;
    public string _class = "Mage";

    public JObject Serialize()
    {
        string jsonString = JsonUtility.ToJson(this);
        JObject retVal = JObject.Parse(jsonString);
        return retVal;
    }

    public void Deserialize(string jsonString)
    {
        JsonUtility.FromJsonOverwrite(jsonString, this);
    }
}
