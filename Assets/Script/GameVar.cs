using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;


// 管理公共開關的類
public class GameVar : MonoBehaviour
{
    public Dictionary<string, int> _data;



    // 讀取
    public int getValue(string key) 
    {
        return _data[key];
    }
    // 寫入
    public void setValue(string key, int value)
    {
        _data[key] = value;
    }
}