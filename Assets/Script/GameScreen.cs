using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScreen : MonoBehaviour
{
    // 宣告實體變數
    int nowFlashTime = 0;
    int nowFadeTime = 0;
    float fadeSpeed = 0;
    int fadeDir = 1;  // (-1:遮罩漸透明 / 1:遮罩漸明顯)
    string colorName = "black";
    Color targetColor = Color.black;

    public Image toneImage;
    public Image flashImage;


    public void CustomFixUpdate()
    {

        // 處理次數
        int times = 1;// ADVDatabase.updateTimes;
        for (var start = 0; start < times; start++)
        {
            UpdateFade();
            if (nowFadeTime <= 0) { break; } 
        }
        for (var start = 0; start < times; start++)
        {
            UpdateFlash();
            if (nowFlashTime <= 0) { break; }
        }
        //print(Time.deltaTime);
    }

    void UpdateFade()
    {
        if (nowFadeTime > 0)
        {
            nowFadeTime--;
            // 跳出詢問視窗，可以拿來逐格暫停用
           // UnityEditor.EditorUtility.DisplayDialog("test", "test", "ok");
            if (nowFadeTime == 0)
            {
                //if (GetColor(colorName) == Color.white) { 
                //    screenImage.color = Color.clear; 
                //} else
                //{
                toneImage.color = ((fadeDir == -1) ? Color.clear : targetColor);//GetColor(colorName));
                //}
            }
            else
            {
                Color color = toneImage.color;
                //color.a += fadeSpeed;
                color.a = (color.a * nowFadeTime + targetColor.a) / (nowFadeTime + 1);
                toneImage.color = color;
            }
        }
    }

    void UpdateFlash()
    {
        if (nowFlashTime > 0)
        {
            Color c = flashImage.color;
            c.a = c.a * (nowFlashTime - 1) / nowFlashTime;
            nowFlashTime--;
            flashImage.color = c;
        }
    }

    Color GetColor(string name)
    {
        Color color;
        switch (name) {
            case "White":
                color = Color.white;
               break;
            case "Black":
                color = Color.black;
                break;
            default:
                color = Color.black;
                break;
        }
        return color;
    }

    public void SetColor(Color color)
    {
        targetColor = color;
        toneImage.color = color;
    }

    public void FadeIn(Color startColor, Color targetColor, int time)
    {
        if (isFading()) { return; }
        Color c = startColor;
        //c.a = 1;
        toneImage.color = c;
        nowFadeTime = time;
        fadeSpeed = -1f / time;/// Time.deltaTime;
        fadeDir = -1;
        this.targetColor = targetColor;
    }

    // 畫面淡出
    public void FadeOut(Color startColor, Color targetColor, int time)
    {
        if (isFading()) { return; }
        Color c = startColor;


        //c.a = 0;
        toneImage.color = c;
        nowFadeTime = time;
        fadeSpeed = 1f / time;// / Time.deltaTime;
        fadeDir = 1;
        this.targetColor = targetColor;
    }

    // 畫面淡入
    public void FadeInByName(string colorName, int time)
    {
        FadeIn(GetColor(colorName), time);
    }
    // 畫面淡出
    public void FadeOutByName(string colorName, int time)
    {
        FadeOut(GetColor(colorName), time);
    }

    // 畫面淡入
    public void FadeIn(Color startColor, int time)
    {
        FadeIn(startColor, Color.clear, time);
    }

    // 畫面淡出
    public void FadeOut(Color targetColor, int time)
    {
        var c = targetColor;
        c.a = 0;

        FadeOut(c, targetColor, time);
    }

    public void RemoveBlack()
    {
        toneImage.color = Color.clear;
    }

    public void StartFlash(int time)
    {
        Color c = flashImage.color;
        c.a = 1;
        flashImage.color = c;
        nowFlashTime = time;
    }

    public bool isFading()
    {
        return (nowFadeTime > 0);
    }
    public bool isFlashing()
    {
        return (nowFlashTime > 0);
    }
    public bool isBusy()
    {
        return (isFading() || isFlashing());
    }

}
