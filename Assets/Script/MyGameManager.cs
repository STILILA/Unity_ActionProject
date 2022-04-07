using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour {

	static MyGameManager _instance;

	GameMessage Message;



	public static MyGameManager instance {
		get {
			if (_instance == null) {
				var obj = new GameObject("GameManager");
				_instance = obj.AddComponent<MyGameManager>();
				DontDestroyOnLoad(obj);
			}
			return _instance;
		}
	}


	void CreateObj() {
		Message = new GameMessage();
	}


}
