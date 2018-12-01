//***********************************************
//PopupManager.cs
//Author y-harada
//***********************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//***********************************************
//PopupManager
//***********************************************
public class PopupManager : UnitySingleton<PopupManager>
{
	[SerializeField]
	Transform root;
	[SerializeField]
	GameObject blockSheet;

	//IPopupを持つインスタンスを生成
	public T Create<T>(T prefab) where T : MonoBehaviour,IPopup
	{
		var ins = Instantiate<T>(prefab);
		ins.transform.SetParent(root);
		ins.transform.localPosition = new Vector3(0, 0, 0);
		ins.transform.localScale = new Vector3(0, 0, 0);
		return ins;
	}

	//ポップアップをオープンさせる
	//この関数からオープンするとタッチ封鎖オブジェクトも開かれる
	//今の所ポップアップが複数でないと思っているのでこの想定
	//複数出る場合は、現在の数を保持し０になったときにブロックを解く必要あり
	public void Open(IPopup popup)
	{
		blockSheet.SetActive(true);
		popup.AddClosedAction(() => { blockSheet.SetActive(false); });
		popup.Open();
	}
}
