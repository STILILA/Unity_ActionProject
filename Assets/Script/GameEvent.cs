using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{

    public SceneMap scene;
    public bool start;
    public TextAsset[] pages;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    //void Update()
    //{

    //}



    private void OnTriggerStay2D(Collider2D col)
    {
        // 注意，預設狀態調用此方法的物件有勾運動學、且兩邊物件都靜止的話會停止觸發，所以RigidBody的Sleep Mode要選Never Sleep
        if (Input.GetKeyDown(KeyCode.Return) && !scene.interpreter.isRunning)
        {
            EventStart();
        }
    }


    void EventStart()
    {
        start = true;
    }

    public string GetNowPage()
    {
        return pages[0].text;
    }

}
