using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneBase : MonoBehaviour
{
    // 宣告實體變數
    static bool gameInit = false;
    public GameScreen gameScreen;

    // Start is called before the first frame update


    public virtual void Awake()
    {
        if (!gameInit)
        {
            var nowScene = SceneManager.GetActiveScene().name;
            if (nowScene != "StartUse") {
                Debug.Log("遊戲尚未初始化，先移至初始化場景建立必須物件");
                SceneManager.LoadScene("StartUse");
                gameInit = true;
                SceneManager.LoadScene(nowScene);
            } 
            else
            {
                gameInit = true;
            }
        }
    }

    public virtual void Start()
    {
        //Audio = FindObjectOfType<AudioManager>();
        //if (Audio == null)
        //{
        //    Audio = Instantiate(targetAudioManager).GetComponent<AudioManager>();
        //}
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }




}
