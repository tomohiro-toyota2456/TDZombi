//***********************************************
//IPopup.cs
//Auhor y-harada
//***********************************************
using System.Collections;
using System.Collections.Generic;

//***********************************************
//IPopup
//***********************************************
public interface IPopup
{
	void Open();
	void Close();
	void AddClosedAction(System.Action action);
}
