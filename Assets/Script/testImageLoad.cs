using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testImageLoad : MonoBehaviour
{
    public Image imageObj;
    public Image imageObj2;
    public Material mat;


    // Start is called before the first frame update
    void Start()
    {
        imageObj.preserveAspect = true;  // 圖片要用等比放大
        imageObj2.preserveAspect = true;

        // 為什麼要這麼做呢？因為直接用 xxx.material 的話會直接取得素材庫的材質球本尊ㄛ，這樣大家都會一起被改啦☆
        // 馬  德  智  障
        imageObj.material = Instantiate(mat);
        imageObj2.material = Instantiate(mat);

        // 啊對了，因為是額外生成的物件，所以記得順帶 Destroy(obj.material)
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {

            if (imageObj.sprite == null) {
                var sprite = ImageManager.LoadPicture("portrait_kohaku_01");
                imageObj.sprite = sprite;
                var color = imageObj.material.color;
                color.a = 1;
                imageObj.material.color = color;
            } 
            else
            {
                imageObj.sprite = null;
                var color = imageObj.material.color;
                color.a = 0;
                imageObj.material.color = color;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            var color = imageObj.material.color;
            if (color.r == 1)
            {
                color.r = color.g = color.b = 0.25f;
                imageObj.material.SetFloat("_FlashAmount", 0);
            }
            else
            {
                color.r = color.g = color.b = 1;
            }
            imageObj.material.color = color;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            var ro = imageObj.rectTransform.localRotation;
            ro.y = (ro.y == 0 ? -180 : 0) ;
            imageObj.rectTransform.localRotation = ro;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {

            var flashAlpha = imageObj.material.GetFloat("_FlashAmount");
            imageObj.material.SetFloat("_FlashAmount", flashAlpha == 1 ? 0 : 1);

        }



        if (Input.GetKeyDown(KeyCode.J))
        {

            if (imageObj2.sprite == null)
            {
                var sprite = ImageManager.LoadPicture("portrait_toko_01");
                imageObj2.sprite = sprite;
                var color = imageObj2.color;
                color.a = 255;
                imageObj2.color = color;
            }
            else
            {
                imageObj2.sprite = null;
                var color = imageObj2.color;
                color.a = 0;
                imageObj2.color = color;
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            var color = imageObj2.color;
            if (color.r == 1)
            {
                color.r = color.g = color.b = 0.25f;
            }
            else
            {
                color.r = color.g = color.b = 1;
            }
            imageObj2.color = color;

        }


        if (Input.GetKeyDown(KeyCode.L))
        {
            var ro = imageObj2.rectTransform.localRotation;
            ro.y = (ro.y == 0 ? -180 : 0);
            imageObj2.rectTransform.localRotation = ro;
        }


    }
}
