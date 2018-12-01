//*************************************************
//SceneChangerManager.cs
//Author y-harada
//*************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//*************************************************
//SceneChangerManager
//*************************************************
public class SceneChangerManager : UnitySingleton<SceneChangerManager>
{
	ISceneChanger sceneChanger;
	ILoadingAnimation loadingAnimation;

	//シーン切り替え完了フラグ
	bool isDone = true;
	bool IsDone { get { return isDone; } }

	//インターフェース取得:UnityはインターフェースをSerializeFieldでセットできないので・・・
	void Awake ()
	{
		sceneChanger = GetComponent<ISceneChanger>();
		loadingAnimation = GetComponent<ILoadingAnimation>();	
	}	

	public void ChangeScene(string sceneName)
	{
		if (!isDone)
			return;

		StartCoroutine(ChangeSceneCoroutine(sceneName));
	}

	IEnumerator ChangeSceneCoroutine(string sceneName)
	{
		isDone = false;

		loadingAnimation.PlayInAnimation();

		while(!loadingAnimation.IsDone)
		{
			yield return null;
		}

		sceneChanger.ChangeScene(sceneName);

		while(!sceneChanger.IsDone)
		{
			yield return null;
		}

		loadingAnimation.PlayOutAnimation();

		while (!loadingAnimation.IsDone)
		{
			yield return null;
		}

		isDone = true;
	}

	public void ReturnScene(string forceSceneName)
	{
		if (!isDone)
			return;

		StartCoroutine(ReturnSceneCoroutine(forceSceneName));
	}

	IEnumerator ReturnSceneCoroutine(string forceSceneName)
	{
		isDone = false;

		loadingAnimation.PlayInAnimation();

		while (!loadingAnimation.IsDone)
		{
			yield return null;
		}

		sceneChanger.ReturnScene(forceSceneName);

		while (!sceneChanger.IsDone)
		{
			yield return null;
		}

		loadingAnimation.PlayOutAnimation();

		while (!loadingAnimation.IsDone)
		{
			yield return null;
		}

		isDone = true;

	}
}
