//***************************************
//GameDataExporter
//Author y-harada
//***************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
//***************************************
//GameDataExporter
//スクリブルオブジェクトを作成する
//***************************************
public static class GameDataExporter
{
	public static void Export<T>(List<T> list,string path,string baseFileName = "data",string idProperty = "Id") where T : ScriptableObject
	{
		if(string.IsNullOrEmpty(path))
		{
			Debug.LogError("Pathが設定されていません");
		}

		if(!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		foreach (var data in list)
		{
			System.Type type = data.GetType();
			var prop =type.GetProperty(idProperty);
			var ins = ScriptableObject.Instantiate<T>(data);
			AssetDatabase.CreateAsset(ins,path +"/"+baseFileName+prop.GetValue(data,null).ToString()+".asset");
		}

		AssetDatabase.Refresh();
	}
}
