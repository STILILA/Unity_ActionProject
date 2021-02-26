using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // 要使用這個庫的腳本必須在Assets/Inspector/Editor/


[CustomEditor(typeof(SceneTitle))] // 要指定這腳本要修改哪種元件
public class CustomEditorUI: Editor
{
   // public override void OnInspectorGUI()
   // {
     //   base.OnInspectorGUI();
       // if (GUILayout.Button("點我")){Debug.Log("痛");}
  //  }
}
