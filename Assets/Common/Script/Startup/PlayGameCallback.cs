//***********************************************
//PlayGameCallback.cs
//Author y-harada
//***********************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//***********************************************
//PlayGameCallback
//ゲーム再生時に行うコード
//今回は必要シーンのロード
//***********************************************
public class PlayGameCallback
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void OnBeforeSceneLoadCreateObject()
	{
		SceneManager.LoadScene("Manager", LoadSceneMode.Additive);
	}
}
