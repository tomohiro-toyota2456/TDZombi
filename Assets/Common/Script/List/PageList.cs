//***********************************************
//ListPage.cs
//Author y-harada
//***********************************************
using System.Collections;
using System.Collections.Generic;

//***********************************************
//ListPage
//リスト形式でページになるものデータ処理部分
//***********************************************
public class ListPage
{
	int onePage = 20;
	int maxPage;
	int curPage;

	//データ・セット
	public delegate void SetDataDelegate(int contentIdx, int dataIdx, bool isActive);
	public SetDataDelegate setDataDel;
	public SetDataDelegate SetDataDel { set { setDataDel = value; } }

	//ページ切り替え通知
	public delegate void NotificationChangedPageDelegate(int curPage);
	public NotificationChangedPageDelegate notificationChangedPageDel;
	public NotificationChangedPageDelegate NotificationChangedPageDel { set { notificationChangedPageDel = value; } }

	int dataCount = 0;

	//初期化用関数
	//総データ数と１ページあたりのデータを入れる
	//ページ切り替えのアクションとデータを入れるときのアクションを入れる
	public int Setup(int dataCount, int onePage,NotificationChangedPageDelegate notificationChangedPageAction,SetDataDelegate setDataAction)
	{
		setDataDel = setDataAction;
		notificationChangedPageDel = notificationChangedPageAction;

		this.onePage = onePage;
		this.dataCount = dataCount;
		maxPage = dataCount / onePage;
		maxPage = dataCount % onePage == 0 ? maxPage : maxPage + 1;
		curPage = 1;

		ChangePage(1);

		return maxPage;
	}

	//ページ移動　引数はいくつページが移動するか
	public void MovePage(int movingNum)
	{
		int page = curPage + movingNum;
		if (page > maxPage)
		{
			page = maxPage;
		}
		else if (page < 1)
		{
			page = 1;
		}

		if (curPage == page)
			return;


		ChangePage(curPage + movingNum);
	}

	//ページセット　引数は何ページ目にいくか
	public void SetPage(int page)
	{
		if (page > maxPage)
		{
			page = maxPage;
		}
		else if (page < 1)
		{
			page = 1;
		}

		if (curPage == page || maxPage == 0)
			return;

		ChangePage(page);
	}

	//ページ切り替え
	void ChangePage(int page)
	{
		//ページ切り替え通知
		notificationChangedPageDel(page);
		curPage = page;

		int stIdx = (curPage - 1) * onePage;
		int endIdx = stIdx + onePage < dataCount ? stIdx + onePage - 1 : dataCount - 1;
		int count = endIdx - stIdx + 1;
		for (int i = 0; i < count; i++)
		{
			setDataDel(i, stIdx + i, true);
		}

		for (int i = count; i < onePage; i++)
		{
			setDataDel(i, 0, false);
		}

	}
}
