using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class GameConfig : ScriptableObject
{
    public float _BGM_volume = 1;
    public float _SE_volume = 1;
    public int _MSG_skip = 1;
    static GameConfig _instance;
    public UnityAction action;


    

    // 獨體方法(要使用GameConfig時就用GameConfig.instance.xxx，雖然很煩)
    public static GameConfig instance
    {
        get
        {
            // 實體未生成
            if (_instance == null)
            {

                
                // 取得設定存檔
                var file = FileManager.LoadFile("Config.txt");
                // 或者用File.Exists(Path);
                // 存檔不存在時，建立新物件並存檔
                if (file == null)
                {
                    Debug.Log("GameConfig不存在，重新建立");
                    _instance = new GameConfig();
                    _instance.Save();
                }
                // 讀取設定存檔
                else
                {
                    FileStream fs = new FileStream(Application.dataPath + "/Save/Config.txt", FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    //selfObject = JsonUtility.FromJson<GameConfig>(sr.ReadLine());
                    sr.Close();
                    fs.Close();

                    _instance = new GameConfig();
                    JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(Application.dataPath + "/Save/Config.txt"), _instance);



                }
            }
            // 回傳建好的設定檔實體
            return _instance;
        }

    }

    // JsonUtility.FromJson也會經過這方法？
    public GameConfig()
    {
        _BGM_volume = 1;
        _SE_volume = 1;
        _MSG_skip = 1;
    }

    // 儲存設定檔
    public void Save()
    {
        // Application.dataPath  =  遊戲目錄的Assets
        FileStream fs = new FileStream(Application.dataPath + "/Save/Config.txt", FileMode.OpenOrCreate);
        //FileStream fs = new FileStream("Save/Config.txt", FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine(JsonUtility.ToJson(this));
        sw.Close();
        fs.Close();
        Debug.Log("Config Saved!");

        //
        System.IO.File.WriteAllText(Application.dataPath + "/Save/Config.txt", JsonUtility.ToJson(this, true));

    }

    // 設置音量
    public float BGM_volume
    {
        get 
        {return _BGM_volume;}
        set
        {
            value = Mathf.Clamp(value, 0, 1);
            _BGM_volume = value;
            Save();
            Debug.Log(_BGM_volume); ///////
        }

    }
    public float SE_volume
    {
        get 
        { return _SE_volume; }
        set
        {
            value = Mathf.Clamp(value, 0, 1);
            _SE_volume = value;
            Save();
        }

    }
    public int MSG_skip
    {
        get 
        { return _MSG_skip; }
        set
        {
            _MSG_skip = value;
            Save();
        }
    }


    // --------------(廢棄) 用PlayerPrefs來存取---------------
    public void Save_Easyver()
    {
        PlayerPrefs.SetString("GameConfig", JsonUtility.ToJson(this));
    }
    public static GameConfig ObjEasy
    {
        get
        {
            // 實體未生成
            if (_instance == null)
            {
                // 取得設定存檔
                string str = PlayerPrefs.GetString("GameConfig");
                // 存檔不存在時，建立新物件並存檔
                if (str == "")
                {
                    Debug.Log("GameConfig不存在，重新建立");
                    _instance = new GameConfig();
                    _instance.Save_Easyver();
                }
                // 讀取設定存檔
                else
                {
                    _instance = new GameConfig(); // 如果是null就不能寫入只好先生成一個，馬德智障
                    JsonUtility.FromJsonOverwrite(str, _instance);
                }
            }
            // 回傳建好的設定檔實體
            return _instance;
        }

    }


// 測試
#if UNITY_EDITOR
    [UnityEditor.MenuItem("CustomTag/GameConfig")]
    public static void ShowGameSettings()
    {
        UnityEditor.Selection.activeObject = instance;
    }
#endif


}
