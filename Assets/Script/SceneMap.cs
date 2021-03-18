using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMap : SceneBase
{
    public AudioClip bgm;
    public GameEvent[] events;
    public GameInterpreter interpreter;
    GameMessage msgObj;


    public override void Start()
    {

        string a = "string";
        string b = a;
        b = b.Remove(0, 3);
        Debug.Log(a);
        Debug.Log(b);
        int c = 5;


        // super
        base.Start();
        interpreter = GetComponent<GameInterpreter>();
        events = FindObjectsOfType<GameEvent>();
        msgObj = interpreter.msgObj;
        gameScreen.FadeIn(Color.black, 30);
        Global.Audio.PlayBGM(bgm);

        


        //StartCoroutine(Global.Audio.FadeInBGM(bgm, 3));  // 淡入測試
    }


    public override void FixedUpdate()
    {
        updateScreen();
    }

    // 定期更新，另外其他物件的update也放在這(其他物件不寫Update())
    public override void Update() {
        // super
        base.Update();
        updateInterpreter();
        
        updateMessage();
        updateEvents();
    }

    void updateScreen()
    {
        gameScreen.CustomFixUpdate();
    }

    void updateInterpreter()
    {
        interpreter.CustomUpdate();
    }

    void updateMessage()
    {
        msgObj.CustomUpdate();
    }

    void updateEvents()
    {
        foreach (var ev in events) {

            if (interpreter.isRunning) { break; }
            if (ev.start)
            {
                interpreter.setup(ev.GetNowPage(), ev.gameObject.name);
               // interpreter.StartTalk();
                ev.start = false;
            }
        }

    }

}
