using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


    public class SaveSystem
    {
      readonly  static string SavePath = "/SaveData.txt";
        public static void Save()
        {
            BinaryFormatter Binary = new();
            string path = Application.persistentDataPath + SavePath;
            FileStream stream = new(path, FileMode.OpenOrCreate);

            SaveDatabase Database = new();

            Binary.Serialize(stream, Database);
            stream.Seek(0, SeekOrigin.Begin);
            stream.Close();
        }
        public static SaveDatabase LoadFile()
        {
            string path = Application.persistentDataPath + SavePath;
            if (File.Exists(path))
            {
                BinaryFormatter binary = new();
                FileStream stream = new(path, FileMode.Open);
                SaveDatabase database = new();
                stream.Position = 0;
                try
                {
                    database = binary.Deserialize(stream) as SaveDatabase;
                    database.LoadBackTocollector();
                    stream.Close();
                    return database;
                }
                catch (System.Exception)
                {
                    stream.Close();
                    Debug.Log("End Early");
                    return database;
                }
            }
            else
            {
                return null;
            }
        }
    }
