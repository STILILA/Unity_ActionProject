using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    SpriteRenderer spriteRender;
    public Sprite[] pictures;
    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }
    public void ChangePicture(int index)
    {
        spriteRender.sprite = pictures[index];
    }


}
