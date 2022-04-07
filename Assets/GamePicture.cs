using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePicture : MonoBehaviour
{
    [SerializeField] Material oriMat;
    [SerializeField] Image image;


    public void Setup() {
        // 為什麼要這麼做呢？因為直接用 xxx.material 的話會直接取得素材庫的材質球本尊ㄛ，這樣改其中一個大家都會一起被改啦☆
        // 馬  德  智  障
        image.material = Instantiate(oriMat);
        image.material.SetFloat("_FlashAmount", 0);
        // 圖片要用等比放大
        image.preserveAspect = true;
    }


    public void SetColor(int r, int g, int b, int a) {
        image.material.SetColor("_Color", new Color(r / 255, g / 255, b / 255, a / 255));
    }

    public void CustomUpdate() {

	}
}
