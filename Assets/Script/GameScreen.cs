using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{

    // 宣告實體變數
    int nowFadeTime = 0;
    float fadeSpeed = 0;
    int fadeDir = 1;  // (-1:變亮 / 1變黑)
    public Image toneScreen;
    public Image flashScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void CustomUpdate()
    {
        if (nowFadeTime > 0)
        {
            nowFadeTime--;
            if (nowFadeTime == 0)
            {
                toneScreen.color = ((fadeDir == -1) ? Color.clear : Color.black);
            }
            else
            {
                Color color = toneScreen.color;
                color.a += fadeSpeed * Time.deltaTime;
                toneScreen.color = color;
            }
        }
    }

    //淡入(移除黑畫面)
    public void FadeIn(int time)
    {
        if (isFading()) { return; }
        nowFadeTime = time;
        fadeSpeed = -1f / time / Time.deltaTime;
        fadeDir = -1;
    }


    // 淡出(變成黑畫面)
    public void FadeOut(int time)
    {
        if (isFading()) { return; }
        nowFadeTime = time;
        fadeSpeed = 1f / time / Time.deltaTime;
        fadeDir = 1;
    }

    public bool isFading()
    {
        return (nowFadeTime > 0);
    }

}
