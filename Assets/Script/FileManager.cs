using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FileManager
{

    public static FileInfo LoadFile(string name)
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Save");
        //FileInfo[] info = dir.GetFiles("*.*");
        //foreach (FileInfo f in info)
        //{
        //    Debug.Log(f.ToString());
        //}
        FileInfo[] targetinfo = dir.GetFiles(name);
        if (targetinfo.Length == 0)
        {
            return null;
        }
        else
        {
            return targetinfo[0];
        }

    }

    public static string[] LoadPath(string path)
    {
        string[] files = Directory.GetFiles(path);
        return files;
    }
        




}


