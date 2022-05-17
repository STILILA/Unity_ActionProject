using System;
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
    List<GameMotion> allBattlers = new List<GameMotion>();


    public override void Start()
    {

        contactFilter2D = new ContactFilter2D();
        //contactFilter2D.SetLayerMask(9);
        //contactFilter2D.useTriggers = true;

        //string a = "string";
        //string b = a;
        //b = b.Remove(0, 3);
        //Debug.Log(a);
        //Debug.Log(b);
        //int c = 5;


        // super
        base.Start();
        interpreter = GetComponent<GameInterpreter>();
        //events = FindObjectsOfType<GameEvent>();
        msgObj = interpreter.msgObj;
        gameScreen.FadeIn(Color.black, 30);

        // 
        allBattlers.Add(player.GetComponent<GameMotion>());
        foreach (var ev in events) { allBattlers.Add(ev.GetComponent<GameMotion>()); }
        allBattlers.RemoveAll(x => x == null); // 移除null
        //Debug.Log(allBattlers.Count);


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




    void updateCollider()
    {
        foreach (var battler in allBattlers) {
           checkCollide(battler);
        }
      //  checkCollide(player);
	}

    void OnDrawGizmos() {
        var allBattlers = new List<GameObject>();
        allBattlers.Add(player);
        foreach (var ev in events) { allBattlers.Add(ev.gameObject); }

        foreach (var obj in allBattlers) {
            var selfCols = obj.GetCompsInChildrenNoRoot<BoxCollider2D>();
          //  var dir = obj.GetComponent<MoveCharacterAction>().dir;

            if (selfCols.Count > 0) {
                foreach (var col in selfCols) {
                    // 算法1 (需假設碰撞那個GameObject位置為0、縮放為1)
                    // var pos = new Vector2(obj.transform.localPosition.x, obj.transform.localPosition.y) + new Vector2(col.offset.x * obj.transform.localScale.x, col.offset.y * obj.transform.localScale.y);
                    //var size = new Vector2(Math.Abs(obj.transform.localScale.x), obj.transform.localScale.y) * col.size;

                    //算法2 (系統會自己算出上面的答案，只是bounds.extents會是實際size除以2，要自己乘回去)
                    var pos = col.bounds.center;
                    var size = col.bounds.extents * 2;
                    switch (col.gameObject.layer) {
                        case 9:
                            Gizmos.color = new Color(0, 0, 1);
                            break;
                        case 10:
                            Gizmos.color = new Color(1, 0, 0);
                            break;
                        default:
                            Gizmos.color = new Color(1, 1, 1);
                            break;

                        
                    } 
                    
                    Gizmos.DrawWireCube(pos, size);

                }

            }
        }
    }

    void checkCollide(GameMotion atker) {


        var atkerRects = atker.atkRects;


        if (atkerRects.Count > 0) {
			foreach (var atkRect in atkerRects) {
                // 算法1 (需假設碰撞那個GameObject位置為0、縮放為1)
                // var pos = new Vector2(atker.transform.localPosition.x, atker.transform.localPosition.y) + new Vector2(atkRect.offset.x * atker.transform.localScale.x, atkRect.offset.y * atker.transform.localScale.y);
                //  var size = new Vector2(Math.Abs(atker.transform.localScale.x), atker.transform.localScale.y) * atkRect.size;
                //算法2 (系統會自己算出上面的答案，只是bounds.extents會是實際size除以2，要自己乘回去)
                var pos = atkRect.bounds.center;
                var size = atkRect.bounds.extents * 2;
                var colliders = Physics2D.OverlapBoxAll(pos, size, 0).ToList();
            //    Debug.Log($"{pos}, {size}");

                colliders.RemoveAll((x) => (x.gameObject.layer != 9) || !x.GetComponent<MotionGetter>() || (x.GetComponent<MotionGetter>().motion == atker) || (x.transform.parent.GetInstanceID() == atker.transform.GetInstanceID()));

                foreach (var targetRect in colliders) {
                    var tMotion = targetRect.GetComponent<MotionGetter>().motion;
                    //
                    if (!tMotion) { continue; }
                    //
                    if (atker.hitTargets.Contains(tMotion)) {
                        continue;
					}
                    Debug.Log($"{atker.name} 攻擊到 {tMotion.name}");
                    //
                    tMotion.DoDamage(atker);
                    atker.hitTargets.Add(tMotion);
                    continue;
                }

                //  Debug.Log($"{col.name},{col.offset},{col.size}");
                //   Debug.Log("=================");

                //		foreach (GameObject target in allObjects) {
                //			BoxCollider2D[] targetCols = target.GetComponents<BoxCollider2D>();
                //                  var colCount = col.OverlapCollider(contactFilter2D, targetCols);
                //			//var colCount = col.OverlapCollider(contactFilter2D, targetCols);
                //			Debug.Log(target.name + ",   " + colCount);
                //			Debug.Log("=========================");
               // }

		    }
		}

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
