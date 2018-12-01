//*************************************************************************
//GameDataImporter
//Author y-harada
//*************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

//*************************************************************************
//GameDataImpoter
//スクリプタブルオブジェクトをあるディレクトリから一括読み込み
//*************************************************************************
public static class GameDataImporter
{
	public static List<T> Import<T>(string path) where T : ScriptableObject
	{
		if(!Directory.Exists(path))
		{
			Debug.LogError("ディレクトリーが存在しません" + path);
			return null;
		}

		var guids = AssetDatabase.FindAssets("", new string[1] { path });

		List<T> list = new List<T>();
		foreach(var guid in  guids)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(guid);
			var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

			if(asset == null)
			{
				continue;
			}

			var ins = GameObject.Instantiate<T>(asset);
			list.Add(ins);
		}

		return list;
	}
}
