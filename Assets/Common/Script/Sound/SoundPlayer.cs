/**********************************************************
 * SoundPlayer.cs
 * Author harada
 * *******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**********************************************************
 * SoundPlayer
 * サウンド再生機構
 * IBgmPlayerとISePlayerを持つクラスをアタッチすること
 * *******************************************************/
public class SoundPlayer : UnitySingleton<SoundPlayer>
{
	IBgmPlayer bgmPlayer;
	ISePlayer sePlayer;

	private void Awake()
	{
		//playerを取得
		bgmPlayer = gameObject.GetComponent<IBgmPlayer>();
		sePlayer = gameObject.GetComponent<ISePlayer>();
	}

	public void PlayBgm(string bgmPath,bool isLoop = true)
	{
		bgmPlayer.Play(bgmPath, isLoop);
	}

	//Time = [s]
	public void PlayBgmCrossFade(string bgmPath,bool isLoop = true,float fadeTime = 1.0f)
	{
		bgmPlayer.PlayCrossFade(bgmPath, isLoop, fadeTime);
	}

	public void PlaySe(string sePath)
	{
		sePlayer.Play(sePath);
	}

	public void StopBgm()
	{
		bgmPlayer.Stop();
	}
	
	//Time = [s]
	public void StopBgmFade(float fadeTime)
	{
		bgmPlayer.StopFade(fadeTime);
	}

}
