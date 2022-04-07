using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePictures : MonoBehaviour
{
    [SerializeField] GamePicture prefab;
    public List<GamePicture> list = new List<GamePicture>();


    public void Add() {
        var obj = Instantiate(prefab);
        obj.transform.SetParent(this.transform);
        obj.transform.localPosition = new Vector2(0, 0);
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.Setup();
        list.Add(obj);
    }

    public GamePicture GetPicture(int index) {
        return list[index];
	}

    public void CustomUpdate() {
        foreach (var pic in list) {
            pic.CustomUpdate();
		}
	}
}
