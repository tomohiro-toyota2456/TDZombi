/**********************************************************
 * IBgmPlayer.cs
 * Author harada
 * *******************************************************/

 /*********************************************************
	* IBgmPlayer Interface
	* BGM再生と停止制御
	* ******************************************************/
public interface IBgmPlayer
{
	void Play(string bgmPath,bool isLoop = true);
	void PlayCrossFade(string bgmPath,bool isLoop,float fadeTime);
	void Stop();
	void StopFade(float fadeTime);
}
