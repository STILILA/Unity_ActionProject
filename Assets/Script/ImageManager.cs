using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    static Dictionary<string, Sprite> cache = new Dictionary<string, Sprite>();

    public static Sprite LoadImage(string path, string name)
    {
        string filename = path + name;
        Sprite file;
        // 嘗試存取對應key的Sprite物件，代入file變數
        if (cache.TryGetValue(filename, out file))
        {
            //return cache[filename];
        }
        // 不然的話讀檔
        else
        {
            file = Resources.Load<Sprite>(filename);
            cache.Add(filename, file);
        }
        // 回傳
        return file;
    }
    public static Sprite LoadPicture(string name)
    {
        return LoadImage($"Graphic/Picture/", name);
    }

}
