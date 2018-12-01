//***********************************************
//ISceneChanger.cs
//Author y-Harada
//***********************************************
using System.Collections;
using System.Collections.Generic;

//***********************************************
//ISceneChanger
//
//***********************************************
public interface ISceneChanger
{
	void ChangeScene(string sceneName);
	void ReturnScene(string forceChangeSceneName);//シーン戻り用
	bool IsDone { get; }
}
