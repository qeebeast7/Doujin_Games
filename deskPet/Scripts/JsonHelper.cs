using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonHelper : MonoBehaviour {
    public static void Write<T>(T data,string fileName)
    {
        string js = JsonUtility.ToJson(data);
        string filePath = Application.streamingAssetsPath + "\\" + fileName+".txt";
        StreamWriter sw = new StreamWriter(filePath);
        sw.WriteLine(js);
        sw.Close();
    }
    public static T Read<T>(string fileName)
    {
        string filePath= Application.streamingAssetsPath + "\\" + fileName + ".txt";
        if (File.Exists(filePath))
        {
            StreamReader sr = File.OpenText(filePath);
            string js = sr.ReadToEnd();
            sr.Close();
            T obj = JsonUtility.FromJson<T>(js);
            return obj;
        }
        else
        {
            return default(T);
        }
    }
}
