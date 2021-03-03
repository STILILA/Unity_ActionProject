using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public SceneBase scene;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            scene.gameScreen.FadeIn(Color.black, 60);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            scene.gameScreen.FadeOut(Color.black, 60);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameConfig.Obj.BGM_volume -= 0.1f;
            
            Global.Audio.SetBGMVolume(1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            GameConfig.Obj.BGM_volume += 0.1f;
            Global.Audio.SetBGMVolume(1);
        }

    }
}
