using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class SavePlayerJson : MonoBehaviour
{
    
    public Personaje[] Personajes;
    
  void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            JObject jSaveGame = new JObject();

            for (int i = 0; i < Personajes.Length; i++)
            {
                Personaje curPersonaje = Personajes[i];
                JObject serializedPersonaje = curPersonaje.Serialize();
                jSaveGame.Add(curPersonaje.name, serializedPersonaje);
            }

            string filePath = Application.persistentDataPath + "/characters.sav";

            byte[] encryptedMessage = Encrypt(jSaveGame.ToString());
            File.WriteAllBytes(filePath, encryptedMessage);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            string filePath = Application.persistentDataPath + "/characters.sav";
            Debug.Log("Loading from: " + filePath);

            byte[] decryptedMessage = File.ReadAllBytes(filePath);
            string jsonString = Decrypt(decryptedMessage);
            

            JObject jSaveGame = JObject.Parse(jsonString);

            for (int i = 0; i < Personajes.Length; i++)
            {
                Personaje curPersonaje = Personajes[i];
                string PersonajeJsonString = jSaveGame[curPersonaje.name].ToString();
                curPersonaje.Deserialize(PersonajeJsonString);
            }
            

        }
    }
    #region Encriptado
    byte[] _key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16};
    byte[] _inicializationVector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

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