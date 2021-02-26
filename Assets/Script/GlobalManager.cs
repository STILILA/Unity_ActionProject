using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 用於全域的物件母體
/// </summary>

public class GlobalManager : MonoBehaviour
{
    public AudioManager targerAudio;
    public static AudioManager Audio;
    static GameObject msgObj;

    public static bool canControl;
    public static bool canAI;
    public static bool isEventRunnig;

    // Start is called before the first frame update




    void Awake()
    {
        Audio = FindObjectOfType<AudioManager>();
        if (Audio == null){Audio = Instantiate(targerAudio).GetComponent<AudioManager>();}
        DontDestroyOnLoad(this.gameObject);        // 轉場時不釋放
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
