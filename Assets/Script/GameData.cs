using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    GameObject[] objs;
}

public class Temp
{
    public bool isEventRunning = false;
    public bool canControl = true;
    public bool canAI = false;

}


public class GameBattler : MonoBehaviour
{
    int _hp;
    int _mp;
    int maxHP;
    int maxMP;
    int _atk;
    public int HP
    {
        get { return _hp; }
        set {_hp = Mathf.Clamp(value, 0, maxHP);}
    }
    public int MP
    {
        get { return _mp; }
        set { _mp = Mathf.Clamp(value, 0, maxMP); }
    }
    public int atk
    {
        get { return _atk; }
        set { _atk = Mathf.Clamp(value, 0, 999); }
    }
}

public class Actor : GameBattler
{

}
public class Enemy : GameBattler
{

}


public class Troop
{

}