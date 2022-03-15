using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMap : SceneBase
{
    public AudioClip bgm;
    public GameObject player;
    public List<GameEvent> events;

    public GameInterpreter interpreter;
    GameMessage msgObj;
    ContactFilter2D contactFilter2D;
    List<GameObject> allObjects;


    public override void Start()
    {
        contactFilter2D = new ContactFilter2D();
// contactFilter2D.useTriggers = true;

        string a = "string";
        string b = a;
        b = b.Remove(0, 3);
        Debug.Log(a);
        Debug.Log(b);
        int c = 5;


        // super
        base.Start();
        interpreter = GetComponent<GameInterpreter>();
        //events = FindObjectsOfType<GameEvent>();
        msgObj = interpreter.msgObj;
        gameScreen.FadeIn(Color.black, 30);

        // 
        allObjects = new List<GameObject>();
        allObjects.Add(player);
        foreach (var ev in events) { allObjects.Add(ev.gameObject); }



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
        
        
        updateMessage();
        updateCollider();
        updateEvents();
        updateInterpreter();
    }

    void updateScreen()
    {
        gameScreen.CustomFixUpdate();
    }

    void updateInterpreter()
    {
        interpreter.CustomUpdate();
    }
    void updateCollider() {
        //foreach (GameObject ev in allObjects) {
        //          checkCollide(ev.gameObject);
        //}
        checkCollide(player);
	}

    void checkCollide(GameObject obj) {

		//var colliders = Physics2D.OverlapBoxAll(player.transform.position, player.transform.localScale / 2, 0);
		//foreach (var coler in colliders) {
		//	if (coler.gameObject == obj) { continue; }
		//	Debug.Log(coler.name);
		//}



		//BoxCollider2D[] selfCols = obj.GetComponents<BoxCollider2D>();
		//if (selfCols.Length > 0) {
  //          foreach (var col in selfCols) {
  //              foreach (GameObject target in allObjects) {
		//		BoxCollider2D[] targetCols = target.GetComponents<BoxCollider2D>();
		//			var colCount = col.OverlapCollider(contactFilter2D, targetCols);
		//			Debug.Log(target.name + ",   " + colCount);
		//			Debug.Log("=========================");
		//		}

		//	}
		//}

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
