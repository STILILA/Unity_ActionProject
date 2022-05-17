using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 用於全域的物件母體
/// </summary>

public class Global : MonoBehaviour
{
    public AudioManager _audio;
    public static AudioManager Audio;

    static GameObject msgObj;

    public static bool canControl;
    public static bool canAI;
    public static bool isEventRunnig;
    public static int atkLayer = 10;
    public static int bodyLayer = 9;
    // Start is called before the first frame update




    void Awake()
    {
        Audio = _audio;
        DontDestroyOnLoad(this.gameObject);        // 轉場時不釋放
        AssetBundleManager.LoadBundle("unitychan");
        QualitySettings.vSyncCount = 0;   // 把垂直同步關掉
        Application.targetFrameRate = 60; // fps為60
    }

    void Start()
    {
        msgObj = GameObject.Find("Message_System");
        if (msgObj) { msgObj.SetActive(false); }
        
    }

    private void FixedUpdate() 
    {
        // 寫在Update的話，順序會比碰撞系判定晚
        if (isEventRunnig)
        {
            if (Input.GetKeyDown(KeyCode.Return)) {
                NextEvent();
            }
        }
    }

    public static void StartEvent()
    {
        msgObj.SetActive(true);
        canControl = canAI = false;
        isEventRunnig = true;
    }
    void NextEvent()
    {
        // ...
        EndEvent();
    }
    void EndEvent()
    {
        msgObj.SetActive(false);
        canControl = canAI = true;
        isEventRunnig = false;
    }
}
