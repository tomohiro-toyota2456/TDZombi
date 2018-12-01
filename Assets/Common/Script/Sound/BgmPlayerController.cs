/**********************************************************
 * BgmPlayerController.cs
 * Author harada
 * *******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**********************************************************
 * BgmPlayerController
 * Bgm再生機構
 * *******************************************************/
public class BgmPlayerController : MonoBehaviour,IBgmPlayer
{
	[SerializeField]
	UnityEngine.Audio.AudioMixerGroup mixerGroup;
	//クロスフェード用に2つ用意
	AudioSource[] audioSources = new AudioSource[2];
	int fadeOutIdx = -1;

	//AudioClip管理
	Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>();

	//監視用コルーチン
	Coroutine crossFadeCoroutine;
	Coroutine fadeStoppingCoroutine;

	//******************************************************
	//Awake
	//初期化
	//******************************************************
	void Awake()
	{
		//アタッチしておく
		audioSources[0] = gameObject.AddComponent<AudioSource>();
		audioSources[1] = gameObject.AddComponent<AudioSource>();

		audioSources[0].outputAudioMixerGroup = mixerGroup;
		audioSources[1].outputAudioMixerGroup = mixerGroup;
	}

	//******************************************************
	//LoadAudioClip
	//AudioClipをロードする。一度ロードしたデータはキャッシュする
	//******************************************************
	AudioClip LoadAudioClip(string path)
	{
		if(audioClipDict.ContainsKey(path))
		{
			return audioClipDict[path];
		}

		var clip = Resources.Load<AudioClip>(path);
		audioClipDict.Add(path, clip);

		return clip;
	}

	//即時再生
	void IBgmPlayer.Play(string bgmPath, bool isLoop)
	{
		PlayCrossFade(bgmPath,isLoop,0);
	}

	//クロスフェード再生
	void IBgmPlayer.PlayCrossFade(string bgmPath,bool isLoop,float fadeTime)
	{
		PlayCrossFade(bgmPath, isLoop, fadeTime);
	}

	//即時停止
	void IBgmPlayer.Stop()
	{
		StopFade(0);
	}

	//フェード停止
	void IBgmPlayer.StopFade(float fadeTime)
	{
		StopFade(fadeTime);
	}

	//コード共通化のためのクロスフェード関数
	void PlayCrossFade(string bgmPath, bool isLoop, float fadeTime)
	{
		//フェードで停止中なら停止を止める
		if(fadeStoppingCoroutine != null)
		{
			StopCoroutine(fadeStoppingCoroutine);
			fadeStoppingCoroutine = null;
		}

		//クロスフェード中の場合
		if (crossFadeCoroutine != null)
		{
			StopCoroutine(crossFadeCoroutine);
			//フェードアウト中のAudioSourceをStopし空きにしておく
			audioSources[fadeOutIdx].Stop();
		}

		crossFadeCoroutine = StartCoroutine(PlayCrossFadeCoroutine(bgmPath, isLoop, fadeTime));
	}

	IEnumerator PlayCrossFadeCoroutine(string bgmPath,bool isLoop,float fadeTime)
	{
		int fadeInIdx = audioSources[0].isPlaying ? 1 : 0;
		fadeOutIdx = audioSources[0].isPlaying ? 0 : 1;

		//セットアップ
		audioSources[fadeInIdx].clip = LoadAudioClip(bgmPath);
		audioSources[fadeInIdx].loop = isLoop;
		audioSources[fadeInIdx].Play();

		//今のボリュームを保存
		float inVol = audioSources[fadeInIdx].volume;
		float outVol = audioSources[fadeOutIdx].volume;

		float timer = 0;

		//クロスフェード
		while(fadeTime != 0 && timer <= fadeTime)
		{
			float t = timer / fadeTime;
			audioSources[fadeInIdx].volume = inVol * (1 - t) + t;
			audioSources[fadeOutIdx].volume = outVol * (1 - t);
			timer += Time.deltaTime;
			yield return null;
		}

		//一応正規化とストップ
		audioSources[fadeInIdx].volume = 1.0f;
		audioSources[fadeOutIdx].volume = 0.0f;
		audioSources[fadeOutIdx].Stop();
		crossFadeCoroutine = null;
	}

	void StopFade(float fadeTime)
	{
		if (fadeStoppingCoroutine != null)
		{
			StopCoroutine(fadeStoppingCoroutine);
		}

		fadeStoppingCoroutine = StartCoroutine(StopFadeCoroutine(fadeTime));
	}

	IEnumerator StopFadeCoroutine(float fadeTime)
	{
		float vol0 = audioSources[0].volume;
		float vol1 = audioSources[1].volume;

		float timer = 0;
		while (fadeTime != 0 && timer <= fadeTime)
		{
			float t = timer / fadeTime;
			audioSources[0].volume = vol0 * (1 - t);
			audioSources[1].volume = vol1 * (1 - t);
			timer += Time.deltaTime;
			yield return null;
		}

		audioSources[0].Stop();
		audioSources[1].Stop();
		audioSources[0].volume = 0;
		audioSources[1].volume = 0;

		fadeStoppingCoroutine = null;
	}

}
