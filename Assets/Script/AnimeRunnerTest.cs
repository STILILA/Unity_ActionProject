using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeRunnerTest : MonoBehaviour
{

	// 宣告animes變數
	Dictionary<string, List<FrameData>> _animes = new Dictionary<string, List<FrameData>>();
	// 設定frame結構
	public class FrameData {
		public int pic { get; set; }
		public int wait { get; set; }
		public object next { get; set; }
		public List<int> bodyRect { get; set; }
		public string physical { get; set; }
	}
	public void test() {
		// 設定frame
		_animes["stand"][0] = new FrameData { pic = 1, wait = 7, next = 1 };
		_animes["stand"][1] = new FrameData { pic = 2, wait = 7, next = 2 };
		_animes["stand"][2] = new FrameData { pic = 2, wait = 7, next = new List<object> { "stand", "1" } };
		_animes["stand"][3] = new FrameData { pic = 2, wait = 7 };

		// 讀取frame資訊並套用
		foreach (var data in _animes["stand"]) {
			var a = data.pic;
			//
		}
	}


	//// 宣告animes變數 ver.2
	//Dictionary<string, List<object>> _animes = new Dictionary<string, List<object>>();
	//// 設定frame結構
	//public void test()
	//{
	//    var f_data = _animes["stand"];
	//    // 設定frame
	//    f_data[0] = new { pic = 1, wait = 7, next = 1 };
	//    f_data[1] = new { pic = 2, wait = 7, next = 2 };
	//    f_data[2] = new { pic = 3, wait = 7, next = new List<object> { "stand", "1" } };
	//    f_data[3] = new { pic = 4, wait = 7 };
	//    f_data[4] = new { pic = 5, wait = 7, next = "walk" };


	//    // 讀取frame資訊並套用
	//    foreach (var data in _animes["stand"][0].GetType().GetProperties())
	//    {

	//        //
	//    }
	//}


}


public class TestSendMethodName
{
    public int Method1(string input)
    {
        //... do something
        return 0;
    }

    public int Method2(string input)
    {
        //... do something different
        return 1;
    }

    public bool RunTheMethod(Func<string, int> myMethodName)
    {
        //... do stuff
        int i = myMethodName("My String");
        //... do more stuff
        return true;
    }

    public bool Test()
    {
        return RunTheMethod(Method1);
    }
}


