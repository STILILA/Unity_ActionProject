using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameMessage_old : MonoBehaviour
{

    public List<Image> imageObjs;
    public List<Sprite> spritesForUse;
    bool[] onUseIndex = new bool[5] ; // 顯示中的圖片欄位
    public Image window;
    public Text windowName;
    public Text windowMsg;
    public Material mat;

    int[] leftPos = new int[] {80, 328};
    int[] RightPos = new int[] {-75, 328};
    string allTexts = ""; // 對話事件時要顯示的文字
    Queue allTextArray;
    int textIndex = 0;

    int waitcount = 0;        // 等待n禎
    bool waitFlag = false;    // 等待決定鍵


    string textColor = "";  // 顏色模式，有值的時候所有處理文字都要加上改色控制碼
    bool textBold = false;  // 粗體模式
    bool textItalic = false;  // 粗體模式
    int textSize = 0;         // 指定文字大小
    int textSpeed = 0;        // 文字速度(正確來說是延遲值，越大越慢)
    public string textColorDefault = "";  // 預設顏色模式，有值的時候所有處理文字都要加上改色控制碼
    public bool textBoldDefault = false;  // 預設粗體模式
    public bool textItalicDefault = false;  // 預設粗體模式
    public int textSizeDefault = 0;         // 預設指定文字大小
    public int textSpeedDefault = 0;        // 預設文字速度(正確來說是延遲值，越大越慢)

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var image in imageObjs) {
            // 圖片要用等比放大
            image.preserveAspect = true;  

            // 為什麼要這麼做呢？因為直接用 xxx.material 的話會直接取得素材庫的材質球本尊ㄛ，這樣改其中一個大家都會一起被改啦☆
            // 馬  德  智  障
            image.material = Instantiate(mat);
            image.material.SetFloat("_FlashAmount", 0);
            var color = image.material.color;
            color.a = 0;
            image.material.color = color;
            // 啊對了，因為是額外生成的物件，所以記得順帶 Destroy(obj.material)
        }
        this.gameObject.SetActive(false);
    }
    
    bool isBusy()
    {
        return (textIndex < allTexts.Length); // &&  
    }


    // 全開模式(按著Shift)
    bool isFast()
    {
        return (Input.GetKeyDown(KeyCode.Return) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
    }
    // 快轉模式中(按著Ctrl)
    bool isSkip()
    {
        return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
    }


    // 初始化訊息內容
    public void initMsg(string texts)
    {
        // 清空
        windowMsg.text = "";
        textIndex = 0;
        textColor = textColorDefault; 
        textBold = textBoldDefault; 
        textItalic = textItalicDefault;  
        textSize = textSizeDefault;
        textSpeed = textSpeedDefault;      
        waitcount = 0;   
        waitFlag = false;  
        allTexts = ConvertEscapeChar(texts);
        //ProcessAllTextsToArray(allTexts);

        //foreach (var c in allTextArray)
        //{
        //    Debug.Log(c);
        //}
        
    }
    // 能先轉換的控制碼(跳脫字元)先轉換 (ex：顯示變數、金錢等)
    string ConvertEscapeChar(string input)
    {
        //string result = Regex.Replace(input, pattern, replacement);
        return input;
    }
    //// 切出字串(回傳：修改後的字串、out：切除的字串部分)
    //string SliceString(string str, out string shiftStr)
    //{
    //    shiftStr = str.Substring(0, 1);
    //    return str.Remove(0, 1);
    //}
 
    //Queue SplitText(string text)
    //{
    //    var textarray = Regex.Split(text, @"(\\[a-zA-Z]+\[[a-zA-Z0-9, ]*\])|(\\[a-zA-Z]+)|(\w)");
    //    return new Queue(textarray);
    //}


    // 以一個字或控制碼為單位分割，存到Quene (為了能用像Ruby的Array#shift方法，C#就是智障到要把動態語言的Array特性分成一大堆class)
    void ProcessAllTextsToArray(string texts)
    {
        allTextArray = new Queue();
        while (textIndex < texts.Length)
        {
            var c = ProcessChar(texts[textIndex], texts);
            allTextArray.Enqueue(c);
        }
        textIndex = 0;
    }



    // 處理所有文字
    void ProcessAllTexts(string texts)
    {
        while (true)
        {
            //ProcessWait();

            if (isFast() || isSkip()) { waitcount = 0; }

            // 等待
            if (waitcount > 0)
            {
                waitcount--;
                return; // 中斷等下一禎
            }
            // 等待決定鍵
            if (waitFlag && !isSkip())
            {
                if (Input.GetKeyDown(KeyCode.Return)){ waitFlag = false; }
                return; // 不管有沒有按都要中斷等下一禎，不然會把下句話全開
            }
            var text = ProcessChar(allTexts[textIndex], allTexts);
            windowMsg.text += text;  // 把字打上去
            if (textSpeed > 0) { waitcount = textSpeed; }   // 設定到下個字的延遲
            if (!isSkip() && !isFast()) { return; } // 打完一個字，中斷等下一禎
            if (!isBusy()) { break; }
        }
    }





    // 定期更新
    public void CustomUpdate()
    {
        if (textIndex < allTexts.Length)
        {
            ProcessAllTexts(allTexts);
            return;
        }


        // 顯示完畢，等待關掉對話框
        if (Input.GetKeyDown(KeyCode.Return) || isSkip())
        {
            window.gameObject.SetActive(false);
            // gameObject.SetActive(false);
        }
    }

    // 處理單一文字(c:一個文字, text:所有文字(碰到控制碼需要後續文字時用)
    string ProcessChar(char c, string texts)
    {
        string text = "";
        /*
                switch (c)
                {
                    case '\n':
                      textIndex++;
                      break;
                    case '\\':
                      textIndex++;
                      ProcessEscapeChar(c, texts);
                      break;
                    default:
                      text = ProcessNormalChar(c);
                      break;
                }
        */
        // 遇到換行符
        while (c == '\n') {
            text += c;
            textIndex++;
            if (textIndex == texts.Length) { return text; }
            if (textIndex < texts.Length) { c = texts[textIndex]; }
        }
        // 遇到控制符
        while (c == '\\') {
            ProcessEscapeChar(texts[textIndex], texts);
            if (textIndex == texts.Length) { return text; }
            if (textIndex < texts.Length) { c = texts[textIndex]; }
        }
        // 一般文字處理
        text += ProcessNormalChar(c);
        return text;
    }



    // 處理控制碼
    void ProcessEscapeChar(char c, string texts)
    {
        string nowTexts = texts.Substring(textIndex, texts.Length - textIndex); // 算出剩餘還沒處理的文字
        string pattern = @"\\([a-zA-Z]+|[\|\.\!]?)"; // 取得控制碼名稱
        string code = Regex.Match(nowTexts, pattern).Groups[1].Value; // 匹配
        textIndex++;
        switch (code)
        {
            case "|": // 等待60f
                textIndex++;
                waitcount = 60;
                break;
            case ".": // 等待15f
                textIndex++;
                waitcount = 15;
                break;
            case "!": // 等待決定鍵
                textIndex++;
                waitFlag = true;
                break;
            case "c": // 換色
                ChangeTextColor(texts);
                break;
            case "f": // 文字大小
                ChangeTextSize(texts);
                break;
            case "s": // 文字速度
                ChangeTextSpeed(texts);
                break;
            case "B": // 粗體切換
                ChangeTextBold();
                break;
            case "I": // 斜體切換
                ChangeTextItalic();
                break;
        }
    }
    // 切換顏色
    void ChangeTextColor(string texts)
    {
        textIndex++; // 推進到'c'文字後面
        string nowTexts = texts.Substring(textIndex, texts.Length - textIndex); // 算出剩餘還沒處理的文字
        var pattern = @"\[(#?\w*|\d*)\]";
        Match match = Regex.Match(nowTexts, pattern);
        textColor = match.Groups[1].Value;  // 設定字色
        textIndex += $"[{textColor}]".Length; // 推進的字數
    }
    // 改變文字大小
    void ChangeTextSize(string texts)
    {
        textIndex++; // 推進到'f'文字後面
        string nowTexts = texts.Substring(textIndex, texts.Length - textIndex); // 算出剩餘還沒處理的文字
        var pattern = @"\[(\d+)\]";
        Match match = Regex.Match(nowTexts, pattern);
        textSize = Int32.Parse(match.Groups[1].Value);  // 設定文字大小
        textIndex += $"[{textSize}]".Length;           // 推進的字數

    }
    // 改變文字速度
    void ChangeTextSpeed(string texts)
    {
        textIndex++; // 推進到's'文字後面
        string nowTexts = texts.Substring(textIndex, texts.Length - textIndex); // 算出剩餘還沒處理的文字
        var pattern = @"\[(\d+)\]";
        Match match = Regex.Match(nowTexts, pattern);
        textSpeed = Int32.Parse(match.Groups[1].Value);  // 設定文字大小
        textIndex += $"[{textSpeed}]".Length;           // 推進的字數
    }


    // 切換粗體
    void ChangeTextBold()
    {
        textIndex++; // 推進的字數
        textBold = !textBold;
        
    }
    void ChangeTextItalic()
    {
        textIndex++; // 推進的字數
        textItalic = !textItalic;
    }

    // 處理一般文字
    string ProcessNormalChar(char c)
    {
        string text = c.ToString();
        if (textColor.Length > 0) { text = $"<color={textColor}>{text}</color>"; }
        if (textSize > 0) { text = $"<size={textSize}>{text}</size>"; }
        if (textBold) { text = $"<b>{text}</b>"; }
        if (textItalic) { text = $"<i>{text}</i>"; }
        textIndex++;
        return text;
    }


    // 更新等待
    void ProcessWait()
    {
        // 全開
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            windowMsg.text = allTexts;
            textIndex = allTexts.Length;
        }

        // 等待決定鍵
        if (waitFlag)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                waitFlag = false;
            }
            else
            {
                return;
            }
        }
    }



    // 設定圖片(欄位, 圖片檔名)
    public void SetPicture(int index, string name)
    {
        var image = imageObjs[index];
        image.sprite = ImageManager.LoadPicture(name);
        var color = image.material.color;
        color.a = 1;
        image.material.color = color;
        onUseIndex[index] = true;
    }

    // 設定圖片(欄位, 這腳本設定的圖片組ID)
    public void SetPicture(int index, int spIndex)
    {
        var image = imageObjs[index];
        image.sprite = spritesForUse[spIndex];
        var color = image.material.color;
        color.a = 1;
        image.material.color = color;
        onUseIndex[index] = true;
    }

    // 清除圖片
    public void RemovePicture(int index)
    {
        var image = imageObjs[index];
        var color = image.material.color;
        color.a = 0;
        image.material.color = color;
        onUseIndex[index] = false;
    }

    // 高亮該欄位圖片
    public void FocusPicture(int index)
    {
        foreach (var image in imageObjs)
        {
            if (!onUseIndex[index]) { continue; }
            var color = image.material.color;
            color.r = color.g = color.b = 0.25f;
            image.material.color = color;
        }
        var image2 = imageObjs[index];
        var color2 = image2.material.color;
        color2.r = color2.g = color2.b = 1;
        color2.a = 1;
        image2.material.color = color2;
    }





    // 釋放
    public void Dispose()
    {
        foreach (var image in imageObjs)
        {
            Destroy(image.material);
        }
    }

}
