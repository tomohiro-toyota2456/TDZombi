//***************************************
//IData.cs
//Author y-harada
//***************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//***************************************
//IData
//Database用にIdがあることを保証
//***************************************
public interface IData
{
	int Id { get; set; }
}
