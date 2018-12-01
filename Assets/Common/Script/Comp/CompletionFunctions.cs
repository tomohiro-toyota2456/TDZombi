//***********************************************
//CompletionFunctions.cs
//Author y-harada
//***********************************************
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//***********************************************
//CompletionFuncions
//***********************************************
public static class CompletionFunctions
{
	public delegate float CompFunc(float _t); 

	//線形
	static public float Liner(float _t)
	{
		return _t;
	}

	//早熟用式
	static public float Sin(float _t)
	{
		return Mathf.Sin((Mathf.PI*0.5f * _t));
	}

	//晩成用式
	static public float InvSin(float _t)
	{
		return (2 * _t - Mathf.Sin((Mathf.PI*0.5f * _t)));
	}

	static public float LvConvertCompVal(int lv,int maxLv)
	{
		//レベル１で０にするための-1
		return (float)(lv - 1) / (float)(maxLv - 1);
	}

	static public int Comp(int _maxVal,float _t,CompFunc _func)
	{
		if(_t >1.0)
		{
			_t = 1.0f;
		}

		if(_t < 0)
		{
			_t = 0;
		}

		float val =_func(_t) * (float)_maxVal;

		return (int)val;
	}

}
