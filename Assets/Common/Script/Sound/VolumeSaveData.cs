using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSaveData : UnitySingleton<VolumeSaveData>
{
	[SerializeField]
	AudioMixer mixer;

	string path = "kjekw39082-3[3,]}DDkww0ki";

	[System.Serializable]
	public struct VolumeData
	{
		public float master;
		public float bgm;
		public float se;
	}

	private void Start()
	{
		LoadVolume();
	}

	public void SaveVolume(float master,float bgm,float se)
	{
		VolumeData data;
		data.master = master;
		data.bgm = bgm;
		data.se = se;

		string json = JsonUtility.ToJson(data);

		PlayerPrefs.SetString(path,json);
	}

	public void LoadVolume()
	{
		string json = PlayerPrefs.GetString(path);

		VolumeData volumeData;
		volumeData.master = -6;
		volumeData.bgm = 0;
		volumeData.se = 0;

		if(!string.IsNullOrEmpty(json))
		{
			volumeData = JsonUtility.FromJson<VolumeData>(json);
		}

		mixer.SetFloat("MasterVolume", volumeData.master);
		mixer.SetFloat("BgmVolume", volumeData.bgm);
		mixer.SetFloat("SeVolume", volumeData.se);
	}
}
