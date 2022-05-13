
using UnityEngine;
//using UnityEngine.Windows;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
public class CreateAssetBundle : Editor {
	

	[MenuItem("Tool/Build AssetBundles")]
	static void BuildAllAssetBundle() {

		var folder = AssetBundleManager.folder;
		if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);  //��Ƨ����s�b�A�h�إ�

		if (BuildPipeline.BuildAssetBundles(folder, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows)) {
			Debug.Log("���\���]");
		}
		else {
			Debug.Log("���]����");
		}
	}


	[MenuItem("Tool/Test AssetBundle Load")]
	static void TestLoadBundle() {
		var ab = AssetBundleManager.LoadBundle("unitychan");
		
		//Debug.Log(string.Join(", ", ab.GetAllAssetNames()));

	//	Debug.Log(AssetBundleManager.GetAsset<Sprite>("unitychan", "cindy_blink_1"));
	}

	[MenuItem("Tool/Test AssetBundle UnLoad")]
	static void TestUnLoadBundle() {
		AssetBundleManager.UnLoadAll();
		
	}


}
#endif


public class AssetBundleManager {
	static public string folder = "AssetBundles";  //�w�q��Ƨ��W�r
	static Dictionary<string, AssetBundle> bundleList = new Dictionary<string, AssetBundle>();

	static public AssetBundle LoadBundle(string abName) {
		
		var ab = AssetBundle.LoadFromFile(Path.Combine($"{folder}/", abName));
		if (ab) {
			bundleList[abName] = ab;
			Debug.Log($"bundle�G{abName}���J����");
			return ab;
		} else {
			return null;
		}

	}


	// �@�w�n�[where���q�A�ڤ�������
	static public T GetAsset<T>(string abName, string filename) where T : UnityEngine.Object {

		var ab = bundleList[abName];
		if (ab) {
			return ab.LoadAsset<T>(filename);
		} else {
			return null;
		}

	}

	static public void UnLoadAll() {
		AssetBundle.UnloadAllAssetBundles(true);

		bundleList.Clear();

		Debug.Log("Asset Bundle All Unloaded");
	}

	// ���o�M�ץؿ�
	static string GetProjectPath() {
		var assetPath = Application.dataPath;
		return assetPath.Substring(0, assetPath.LastIndexOf("/") + 1);
	}

}