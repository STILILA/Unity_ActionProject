using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInterpreter : MonoBehaviour
{
    string mapName;
    string eventName;
    string[] list = new string[] { };   // 事件內容
    int listIndex = 0;     // 事件進度
    public SceneBase scene;
    public GameMessage msgObj;
    [HideInInspector] public bool isRunning;  // 公開(會序列化)這變數，但不顯示在Inspector



    public void setup(string list, string eventName)
    {
        this.mapName = SceneManager.GetActiveScene().name;
        this.eventName = eventName;
        this.list = list.Split('\n');
        this.listIndex = 0;
        StartEvent();
    }

    // 解讀事件指令
    // listline:該行內容
    bool ReadList(string listline)
    {
        var pattern = @"\\cmd\[(\w+)([,\d+|\w+]*)\]";
        Match match = Regex.Match(listline, pattern) ;
        if (match.Success)
        {
            Debug.Log(match.Groups[1].Value);
          // Debug.Log(match.Groups[2].Value);
            switch (match.Groups[1].Value)
            {
                case "Item":
                    return true;
                case "Money":
                    return true;
                case "pic":
                    SetPicture(match.Groups[2].Value);
                    return true;
                case "StartTalk":
                    StartTalk();
                    return false;
                case "EndTalk":
                    EndTalk();
                    return true;
                case "Focuspic":
                    msgObj.FocusPicture(Int32.Parse(match.Groups[2].Value.Split(',')[1]));
                    return true;
                default:
                    return true;
            }

        } 
        else
        {

            return true;
        }

       // return SendMessage(match.Groups[1].Value, match.Groups[2].Value);

    }

    public void CustomUpdate()
    {
        if (msgObj.window.gameObject.activeInHierarchy) { return; }

        if (this.listIndex < this.list.Length)
        {
            // 往下找到沒空字串
            while (this.list[this.listIndex] == "") {this.listIndex++; }
            // 超出事件清單
            if (this.listIndex >= this.list.Length) { return; }
            // 解讀事件指令
            if (ReadList(this.list[this.listIndex]))
            {
                this.listIndex++;
            }
        } 
        else
        {
            EndEvent();
        }
    }



    void SetPicture(string str)
    {
        string[] param = str.Split(',');

        if (!Int32.TryParse(param[2], out int intP2))
        {
            string strP2 = param[2];
            msgObj.SetPicture(Int32.Parse(param[1]), param[2]);
            return;
        }
        msgObj.SetPicture(Int32.Parse(param[1]), intP2);

    }



    public void StartEvent()
    {
        isRunning = true;
    }
    public void EndEvent()
    {
        isRunning = false;
    }

    public void StartTalk()
    {
        msgObj.gameObject.SetActive(true);
        msgObj.window.gameObject.SetActive(true);
        string text = "";
        this.listIndex++;
        var pattern = @"^\\cmd.";
        Match match = Regex.Match(this.list[this.listIndex], pattern);
        // 組合文字
        while (!match.Success)
        {
            text += this.list[this.listIndex] + '\n';
            this.listIndex++;
            match = Regex.Match(this.list[this.listIndex], pattern);
        }
        msgObj.initMsg(text);
    }

    public void EndTalk()
    {
        for (var i=0; i < msgObj.imageObjs.Count; i++) {
            msgObj.RemovePicture(i);
        }
        msgObj.gameObject.SetActive(false);
    }






}
