using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeChanger : MonoBehaviour
{
	[SerializeField]
	AudioMixer audioMixer;

	public void SetMasterVolume(float value)
	{
		audioMixer.SetFloat("MasterVolume", value);
	}

	public void SetBgmVolume(float value)
	{
		audioMixer.SetFloat("BgmVolume", value);
	}

	public void SetSeVolume(float value)
	{
		audioMixer.SetFloat("SeVolume", value);
	}

	public float GetMasterVolume()
	{
		float val = 0;
		audioMixer.GetFloat("MasterVolume",out val);
		return val;
	}

	public float GetBgmVolume()
	{
		float val = 0;
		audioMixer.GetFloat("BgmVolume", out val);
		return val;
	}

	public float GetSeVolume()
	{
		float val = 0;
		audioMixer.GetFloat("SeVolume", out val);
		return val;
	}
}
