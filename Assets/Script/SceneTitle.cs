using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTitle : SceneBase
{

    int index = 0;
    bool cursorLock = false;
    public GameObject[] buttons;
    public GameObject cursor;
    public float[] selectOX;
    public AudioClip bgm;
    public AudioClip se_OK;
    public AudioClip se_Buzzer;
    public AudioClip se_Cursor;
    // Start is called before the first frame update
    public override void Start()
    {
        // super
        base.Start();
        gameScreen.FadeIn(30);
        GlobalManager.Audio.PlayBGM(bgm);
        //StartCoroutine(Global.Audio.FadeInBGM(bgm, 3));  // 淡入測試
    }

    // Update is called once per frame
    public override void Update()
    {
        // super
        base.Update();

        gameScreen.CustomUpdate();

        // 還在漸變就中斷
        if (gameScreen.isFading() || cursorLock) { return; }

        // 游標上
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index--;
            if (index < 0) { index = 2; }
            MoveCursor(index);
        }

        // 游標下
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index++;
            if (index > 2) { index = 0; }
            MoveCursor(index);
        }

        // 確定
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GlobalManager.Audio.PlaySE(se_OK);
            TriggerOK(index);
        }

    }

    void MoveCursor(int index)
    {
        GlobalManager.Audio.PlaySE(se_Cursor);
        Vector3 pos = cursor.transform.position;
        pos.x = selectOX[index];
        pos.y = buttons[index].transform.position.y;
        cursor.transform.position = pos;
    }

    void TriggerOK(int index)
    {
        cursorLock = true;
        switch (index)
        {
            case 0:
                StartCoroutine(NewGame());
                break;
            case 1:
                StartCoroutine(LoadGame());
                break;
            case 2:
                StartCoroutine(Option());
                break;
        }
    }

    // 新遊戲
    IEnumerator NewGame() {
        // 預載測試
       // var async = SceneManager.LoadSceneAsync("Stage01");
      //  async.allowSceneActivation = false;
        buttons[index].GetComponent<Button>().ChangePicture(1);
        yield return new WaitForSeconds(0.1f);
        buttons[index].GetComponent<Button>().ChangePicture(0);
        gameScreen.FadeOut(120);
        StartCoroutine(GlobalManager.Audio.FadeOutBGM(2));
        // 至少等2秒
        yield return new WaitForSeconds(2f);
        // 等待BGM完全停止
        while (GlobalManager.Audio.BGM.isPlaying)  // 不能用if ，不然只會中斷一次
        {
            yield return null;
        }
        //  while (async.progress < 0.9f)
        //     yield return null;
        //  }
        // async.allowSceneActivation = true;
        // 轉換場景
        SceneManager.LoadScene("Stage01");
        // 釋放沒用到的物件
        Resources.UnloadUnusedAssets();
    }
    // 讀檔
    IEnumerator LoadGame()
    {
        buttons[index].GetComponent<Button>().ChangePicture(1);
        yield return new WaitForSeconds(0.1f);
        buttons[index].GetComponent<Button>().ChangePicture(0);
        cursorLock = false;
    }
    // 系統設定
    IEnumerator Option()
    {
        buttons[index].GetComponent<Button>().ChangePicture(1);
        yield return new WaitForSeconds(0.1f);
        buttons[index].GetComponent<Button>().ChangePicture(0);
        cursorLock = false;
    }
}
