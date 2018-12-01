//***********************************************
//SimplePopup
//Author y-harada
//***********************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

//***********************************************
//SimplePopup.cs
//***********************************************
public class SimplePopup : MonoBehaviour,IPopup
{
	[SerializeField]
	TextMeshProUGUI title;
	[SerializeField]
	TextMeshProUGUI description;
	[SerializeField]
	Button[] buttons = new Button[2];//yes no close
	[SerializeField]
	TextMeshProUGUI[] buttonNames = new TextMeshProUGUI[2];
	[SerializeField]
	MyTween myTween;


	List<Action> closedActions = new List<Action>();
	Action yesAction;
	Action noAction;
	public void AddClosedAction(Action action)
	{
		if (action == null)
			return;

		closedActions.Add(action);
	}

	public void Close()
	{
		myTween.LerpScl(transform, 0.5f, transform.localScale, new Vector3(0, 0, 0),()=>
		{
			if(yesAction != null)
			{
				yesAction();
			}

			if(noAction != null)
			{
				noAction();
			}

			for(int i = 0; i < closedActions.Count;i++)
			{
				closedActions[i]();
			}
		});
	}

	public void Init(string title,string description,string buttonName,Action closedAction = null)
	{
		this.title.text = title;
		this.description.text = description;

		buttons[0].gameObject.SetActive(true);
		buttons[1].gameObject.SetActive(false);

		buttonNames[0].text = buttonName;

		AddClosedAction(closedAction);
	}

	public void Init(string title, string description, string yesButtonName,string noButtonName,Action yesAction=null,Action noAction=null)
	{
		this.title.text = title;
		this.description.text = description;
		buttonNames[0].text = yesButtonName;
		buttonNames[1].text = noButtonName;
		this.yesAction = yesAction;
		this.noAction = noAction;
	}

	public void Open()
	{
		myTween.LerpScl(transform, 0.5f, transform.localScale, new Vector3(1, 1, 1));
	}

	public void OnClickButtonA()
	{
		noAction = null;
		Close();
	}

	public void OnClickButtonB()
	{
		yesAction = null;
		Close();
	}
}
