//***********************************************
//ILoadingAnimation.cs
//Author y-harada
//***********************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//***********************************************
//ILoadingAnimation
//***********************************************
public interface ILoadingAnimation
{
	void PlayInAnimation();
	void PlayOutAnimation();
	bool IsDone { get; }
}
