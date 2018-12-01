//***********************************************
//SceneChanger.cs
//Author y-harada
//***********************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//***********************************************
//SceneChanger
//戻り機能付きシーン切り替え機構
//***********************************************
public class SceneChanger: MonoBehaviour, ISceneChanger
{
	bool isDone;
	public bool IsDone { get { return isDone; } }

	List<string> scenes = new List<string>();
	string TopScene
	{
		get
		{
			if(scenes.Count != 0)
			{
				return scenes[scenes.Count - 1];
			}

			return "";
		}
	}

	Coroutine coroutine;

	public void ChangeScene(string sceneName)
	{
		if(coroutine == null && !sceneName.Equals(TopScene))
		{
			coroutine = StartCoroutine(ChangeSceneCoroutine(sceneName));
		}
	}

	IEnumerator ChangeSceneCoroutine(string sceneName)
	{
		isDone = false;

		//前シーンがあれば削除する

		if (scenes.Count != 0)
		{
			var op1 = SceneManager.UnloadSceneAsync(TopScene);
			while (op1.isDone)
			{
				yield return null;
			}
		}

		//TODO:基本開発中にルートシーン以外から開始した際に開始シーンが見れていないのに対処するよう　微妙なのでどうにかしたい
		if (scenes.Count == 0 && SceneManager.sceneCount > 1 )
		{
			var scene = SceneManager.GetSceneAt(0);
			var op = SceneManager.UnloadSceneAsync(scene.name);
			
			while (op.isDone)
			{
				yield return null;
			}

			scenes.Add(scene.name);
		}
	 
		//ロード
		var op2 = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		while (op2.isDone)
		{
			yield return null;
		}


		//過去シーンリストに同じシーンがあるかチェック
		int idx = -1;
		for(int i = 0; i < scenes.Count; i++)
		{
			if(sceneName.Equals(scenes[i]))
			{
				idx = i;
				break;
			}
		}

		//ある場合は該当以降を削除
		if(idx != -1)
		{
			scenes.RemoveRange(idx, scenes.Count - idx);
		}

		scenes.Add(sceneName);
		isDone = true;
		coroutine = null;
	}

	public void ReturnScene(string forceChangeSceneName)
	{
		if (coroutine != null || scenes.Count <= 1)
		{
			return;
		}

		if(!string.IsNullOrEmpty(forceChangeSceneName))
		{
			scenes.RemoveRange(0, scenes.Count - 1);//一番上以外削除
			ChangeScene(forceChangeSceneName);
			return;
		}

		string changeSceneName = scenes[scenes.Count - 2];
		ChangeScene(changeSceneName);
	}
}
