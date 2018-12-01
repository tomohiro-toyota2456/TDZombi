//***********************************************
//MyTween.cs
//Author y-harada
//***********************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//***********************************************
//使うかどうか不明だけど自前のTween
//***********************************************
public class MyTween : MonoBehaviour
{
	public void LerpScl(Transform transform,float duration,Vector3 begin,Vector3 end, Action completeAction = null, CompletionFunctions.CompFunc func = null)
	{
		StartCoroutine(LerpSclCoroutine(transform, duration, begin, end, completeAction, func));
	}

	IEnumerator LerpSclCoroutine(Transform trans,float duration,Vector3 begin,Vector3 end, Action completeAction, CompletionFunctions.CompFunc func)
	{
		if(func == null)
		{
			func = CompletionFunctions.Liner;
		}

		float timer = 0;
		while(timer <= duration)
		{
			float t = timer / duration;
			float ans = func(t);

			trans.localScale = begin * (1 - ans) + end * ans;
			timer += Time.deltaTime;
			yield return null;
		}

		trans.localScale = end;
		if(completeAction != null)
		{
			completeAction();
		}
	}
}
