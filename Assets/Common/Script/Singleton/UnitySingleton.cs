/**********************************************************
 * UnitySingleton.cs
 * Author harada
 * *******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**********************************************************
 * UnitySingleTon
 * 
 * *******************************************************/
public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	static T instance;
	static public T Instance
	{
		get
		{
			//無い場合は探して取得する
			if(instance == null)
			{
				instance = FindObjectOfType<T>();//シーン上に存在しないとダメ
			}

			return instance;
		}
	}
}
