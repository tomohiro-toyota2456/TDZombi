//***********************************************
//FadeAnimation.cs
//Author y-harada
//***********************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//***********************************************
//FadeAnimation
//***********************************************
public class FadeAnimation : MonoBehaviour,ILoadingAnimation
{
	[SerializeField]
	Image fadeImage;
	[SerializeField]
	float fadeTime;

	bool isDone = false;
	public bool IsDone { get { return isDone; } }
	bool isAnimation = false;

	public void PlayInAnimation()
	{
		if (isAnimation)
			return;

		StartCoroutine(Fade(true, fadeTime));
	}

	public void PlayOutAnimation()
	{
		if (isAnimation)
			return;

		StartCoroutine(Fade(false, fadeTime));
	}

	IEnumerator Fade(bool isFadeIn,float time)
	{
		isAnimation = true;
		isDone = false;
		fadeImage.gameObject.SetActive(true);
		Color stCol = fadeImage.color;
		Color edCol = stCol;

		stCol.a = isFadeIn ? 0.0f : 1.0f;
		edCol.a = isFadeIn ? 1.0f : 0.0f;

		float timer = 0;

		while(timer <= time)
		{
			timer += Time.deltaTime;
			float t = timer / time;
			fadeImage.color = stCol * (1 - t) + edCol * t;
			yield return null;
		}

		fadeImage.color = edCol;

		if(fadeImage.color.a <= 0)
		{
			fadeImage.gameObject.SetActive(false);
		}

		isDone = true;
		isAnimation = false;
	}
}
