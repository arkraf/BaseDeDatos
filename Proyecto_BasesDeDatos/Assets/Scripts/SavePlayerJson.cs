using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class SavePlayerJson : MonoBehaviour
{
    //Declaramos las variables que usaremos
    public Text team;
    public Personaje[] Personajes;
    
  void Update()
    {   
         //Esta parte se encargará de recoger la información que indiquemos y guardarla en la dirección deseada
        if (Input.GetKeyDown(KeyCode.S))
        {
            JObject jSaveGame = new JObject();
            //selecionamos la información que vamos a guardar en el fichero
            for (int i = 0; i < Personajes.Length; i++)
            {
                Personaje curPersonaje = Personajes[i];
                JObject serializedPersonaje = curPersonaje.Serialize();
                jSaveGame.Add(curPersonaje.name, serializedPersonaje);
            }

            //asignamos la dirección done guardaremos la información
            string filePath = Application.persistentDataPath + "/characters.sav";
            Debug.Log("Saving from: " + filePath);

            //Encriptamos la información guardada
            byte[] encryptedMessage = Encrypt(jSaveGame.ToString());
            File.WriteAllBytes(filePath, encryptedMessage);
        }

        //Esta parte se encargará de extraer y cargar la información guardada en el fichero json
        if (Input.GetKeyDown(KeyCode.L))
        {
            //introdución la dirección de la que queremos extraer la información
            string filePath = Application.persistentDataPath + "/characters.sav";
            Debug.Log("Loading from: " + filePath);

            //Desencriptamos la informacaión guardada para poder leerla 
            byte[] decryptedMessage = File.ReadAllBytes(filePath);
            string jsonString = Decrypt(decryptedMessage);
            

            JObject jSaveGame = JObject.Parse(jsonString);

            //Leemos la información guardada en el archivo json y la extraemos
            for (int i = 0; i < Personajes.Length; i++)
            {
                Personaje curPersonaje = Personajes[i];
                string PersonajeJsonString = jSaveGame[curPersonaje.name].ToString();
                curPersonaje.Deserialize(PersonajeJsonString);
            }
            
            //mostramos por pantalla la información extraida del fichero
            team.text += jSaveGame;
            
        }
    }

    //En esta región realizamos el encriptado de la información guardada
    #region Encriptado
    byte[] _key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16};
    byte[] _inicializationVector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

    //Esta función se encarga de encriptar la información
    byte[] Encrypt(string message)
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform encryptor = aes.CreateEncryptor(_key, _inicializationVector);

        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        StreamWriter streamWriter = new StreamWriter(cryptoStream);

        streamWriter.WriteLine(message);

        streamWriter.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return memoryStream.ToArray();
    }
    //Esta función se encargará de desencrpitar la información guardada 
    string Decrypt(byte[] message)
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform decrypter = aes.CreateDecryptor(_key, _inicializationVector);

        MemoryStream memoryStream = new MemoryStream(message);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Read);
        StreamReader streamReader = new StreamReader(cryptoStream);

        string decryptedMessage = streamReader.ReadToEnd();

        memoryStream.Close();
        cryptoStream.Close();
        streamReader.Close();

        return decryptedMessage;
    }
    #endregion

}