//*********************************************************
//ReorderableListEditorWindow.cs
//Author y-harada
//*********************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using System;
using UnityEditor;
using System.Linq;

//********************************************************************
//ReorderableListEditorWindow
//スクリプタブルオブジェクトを対象に簡易的なリストウィンドウを表示できるように
//した
//********************************************************************
public class ReorderableListEditorWindow<T> where T : ScriptableObject
{
	ReorderableListNoSerializeObject<T> reorderableListNoSerializeObject = new ReorderableListNoSerializeObject<T>();

	//パス指定
	public string FilePath { get; set; }
	//吐き出すファイル名のベース
	public string ExportFileName { get; set; }

	public bool IsSettingFilePath { get { return !string.IsNullOrEmpty(FilePath); } }
	public bool IsSettingFileName { get { return !string.IsNullOrEmpty(ExportFileName); } }

	Vector2 scroll;

	public void OnGUI()
	{
		if (GUILayout.Button("Import"))
		{
			GameDataImporter.Import<T>(FilePath);
		}

		if (GUILayout.Button("Export"))
		{
			if (string.IsNullOrEmpty(ExportFileName))
			{
				ExportFileName = "Data";
			}
			GameDataExporter.Export<T>(reorderableListNoSerializeObject.list, FilePath, ExportFileName);
		}

		using (var sc = new EditorGUILayout.ScrollViewScope(scroll))
		{
			reorderableListNoSerializeObject.Init();
			reorderableListNoSerializeObject.DoLayoutList();
			scroll = sc.scrollPosition;
		}
		
		
	}

}

//配列を別ウィンドウで編集して制御しよう的な
public class ArraySubWindow : EditorWindow
{
	public static ArraySubWindow Open()
	{
		return EditorWindow.GetWindow<ArraySubWindow>();
	}

	public Action guiAction;

	private void OnGUI()
	{
		if (guiAction != null)
			guiAction();
	}
}

//EditorWindow辞退をジェネリックにするとうまくいかんので分離
public class ArraySubWindowGUI<T,F> where F : class
{
	ReorderableListNoSerializeObjectNoClass<T> reorderableListNoSerializeObject = new ReorderableListNoSerializeObjectNoClass<T>();

	int index = -1;
	System.Action<F,List<T>, int> applyAction;
	F root;

	public void Set(F root,List<T> list,int index,System.Action<F,List<T>,int> applyAction)
	{
		this.root = root;
		this.applyAction = applyAction;
		this.index = index;
		reorderableListNoSerializeObject.Init(list);
	}

	public void OnGUI()
	{
		if(GUILayout.Button("Apply"))
		{
			applyAction(root,reorderableListNoSerializeObject.list, index);
		}

		reorderableListNoSerializeObject.Init();

		reorderableListNoSerializeObject.DoLayoutList();
	}
}

//ReorderableList
public class ReorderableListNoSerializeObject<T> where T : class
{
	public List<T> list;
	ReorderableList reorderableList;

	static readonly int ColumnLimit = 5;
	int columnNum = 0;
	int elementHeight = 50;

	public void Init()
	{
		if (list == null)
			list = new List<T>();

		if (reorderableList == null)
		{
			reorderableList = new ReorderableList(list, typeof(T));
			reorderableList.drawElementCallback = Func;
			reorderableList.elementHeight = CalcHeight();
		}
	}

	float CalcHeight()
	{
		var props = typeof(T).GetProperties();
		var propList = props.ToList();

		propList = ExcludeElement(propList);

		bool isDivide = propList.Count > ColumnLimit;

		columnNum = isDivide ? propList.Count / 2 : propList.Count;

		return isDivide ? 60 : 35;
	}

	List<System.Reflection.PropertyInfo> ExcludeElement(List<System.Reflection.PropertyInfo> list)
	{
		string[] exclusionStr = new string[] {"name","hideFlags"};
		foreach(var str in exclusionStr)
		{
			string s = str;
			int fIdx = list.FindIndex((_) =>
			{
				string pt = _.PropertyType.ToString();

				string name = _.ToString();
				int i = name.IndexOf(" ");
				name = name.Substring(i + 1);

				if (name.Equals(s))
				{
					return true;
				}

				return false;
			});

			if(fIdx != -1)
			{
				list.RemoveAt(fIdx);
			}
		}

		return list;
	}

	public void Init(List<T> list)
	{
		this.list = list;
		Init();
	}

	public void DoLayoutList()
	{
		reorderableList.DoLayoutList();
	}

	public void DoList(Rect rect)
	{
		reorderableList.DoList(rect);
	}

	void Func(Rect rect, int index, bool isActive, bool isFocused)
	{
		Type type = list[index].GetType();
		var props = type.GetProperties();

		string typeName = type.ToString();
		string baseTypeName = type.BaseType.ToString();

		Rect r = rect;
		r.width = 200;
		r.height = 25;

		float firstPosx = r.x;

		//リスト化してname除外(クラス作ると自動でいるため)
		var propList = ExcludeElement(props.ToList());

		int counter = 0;

		propList.Sort((x, y) =>
		{
			int i = x.ToString().IndexOf(" ");
			var namex = x.ToString().Substring(i + 1);

			int j = y.ToString().IndexOf(" ");
			var namey = y.ToString().Substring(j + 1);

			//IDだけでも上にくるようにソートする
			if (namex.Equals("Id") && !namey.Equals("Id"))
			{
				return -1;
			}
			else if (!namex.Equals("Id") && namey.Equals("Id"))
			{
				return 1;
			}
			else if(namex.Contains("Id") && !namey.Contains("Id"))
			{
				return -1;
			}
			else if (namex.Contains("Id") && !namey.Contains("Id"))
			{
				return 1;
			}

			return 0;
		});

		//やけくそ気味なタイプ判定
		foreach (var prop in propList)
		{
			string pt = prop.PropertyType.ToString();

			string name = prop.ToString();
			int i = name.IndexOf(" ");
			name = name.Substring(i + 1);

			//配列かどうか
			if (pt.Contains("[]"))
			{
				Debug.Log(prop.ToString());
				if (GUI.Button(r, name))
				{
					if (pt.Contains("Int"))
					{
						var window = ArraySubWindow.Open();
						var gui = new ArraySubWindowGUI<int, T>();
						
						int[] ary = (int[])prop.GetValue(list[index], null);

						if (ary == null)
						{
							ary = new int[1];
						}

						gui.Set(list[index],ary.ToList(), index, (root, list, idx) =>
						{
							if (this.list[idx] != root)
								return;

							prop.SetValue(this.list[idx], list.ToArray(), null);
						});

						window.guiAction = gui.OnGUI;
	
					}
					else if (pt.Contains("Single"))
					{
						var window = ArraySubWindow.Open();
						var gui = new ArraySubWindowGUI<float, T>();

						float[] ary = (float[])prop.GetValue(list[index], null);

						if (ary == null)
						{
							ary = new float[1];
						}

						gui.Set(list[index], ary.ToList(), index, (root, list, idx) =>
						{
							if (this.list[idx] != root)
								return;

							prop.SetValue(this.list[idx], list.ToArray(), null);
						});

						window.guiAction = gui.OnGUI;
					}
					else if (pt.Contains("String"))
					{
						var window = ArraySubWindow.Open();
						var gui = new ArraySubWindowGUI<string, T>();

						string[] ary = (string[])prop.GetValue(list[index], null);

						if (ary == null)
						{
							ary = new string[1];
						}

						gui.Set(list[index], ary.ToList(), index, (root, list, idx) =>
						{
							if (this.list[idx] != root)
								return;

							prop.SetValue(this.list[idx], list.ToArray(), null);
						});

						window.guiAction = gui.OnGUI;
					}
					else if(prop.PropertyType.IsEnum)
					{
						
						var window = ArraySubWindow.Open();
						var gui = new ArraySubWindowGUI<Enum, T>();

						Enum[] ary = (Enum[])prop.GetValue(list[index], null);

						if (ary == null)
						{
							ary = new Enum[1];
						}

						gui.Set(list[index], ary.ToList(), index, (root, list, idx) =>
						{
							if (this.list[idx] != root)
								return;

							prop.SetValue(this.list[idx], list.ToArray(), null);
						});

						window.guiAction = gui.OnGUI;
					}
					


				}
				r.x += 250;

				counter++;

				if(counter >= columnNum)
				{
					r.x = firstPosx;
					r.y += r.height + 5;
					counter = 0;
				}

				continue;
			}

			//配列じゃないやつ
			if (pt.Contains("Int"))
			{
				Debug.Log(prop.ToString());
				int val = EditorGUI.IntField(r, name, (int)prop.GetValue(list[index], null));
				prop.SetValue(list[index], val, null);
				r.x += 250;
			}
			else if (pt.Contains("Single"))
			{
				Debug.Log(prop.ToString());
				float val = EditorGUI.FloatField(r, name, (float)prop.GetValue(list[index], null));
				prop.SetValue(list[index], val, null);
				r.x += 250;
			}
			else if (pt.Contains("String"))
			{
				Debug.Log(prop.ToString());
				string val = EditorGUI.TextField(r, name, (string)prop.GetValue(list[index], null));
				prop.SetValue(list[index], val, null);
				r.x += 250;
			}
			else if (prop.PropertyType.IsEnum)
			{
				Debug.Log(prop.ToString());
				Enum val = EditorGUI.EnumPopup(r, name, (Enum)prop.GetValue(list[index], null));
				prop.SetValue(list[index], val, null);
				r.x += 250;
			}

			counter++;

			if (counter >= columnNum)
			{
				r.x = firstPosx;
				r.y += r.height+5;
				counter = 0;
			}

		}

	}
}

//値型用　int float のみ enumも必要なら追加
public class ReorderableListNoSerializeObjectNoClass<T>
{
	public List<T> list;
	ReorderableList reorderableList;

	public void Init()
	{
		if (list == null)
			list = new List<T>();

		if (reorderableList == null)
		{
			reorderableList = new ReorderableList(list, typeof(T));
			reorderableList.drawElementCallback = Func;
		}
	}

	public void Init(List<T> list)
	{
		this.list = list;
		Init();
	}

	public void DoLayoutList()
	{
		reorderableList.DoLayoutList();
	}

	public void DoList(Rect rect)
	{
		reorderableList.DoList(rect);
	}

	void Func(Rect rect, int index, bool isActive, bool isFocused)
	{
		Type type = list[index].GetType();

		string typeName = type.ToString();
		string baseTypeName = type.BaseType.ToString();

		Rect r = rect;
		r.width = 200;

		if(typeName.Contains("Int"))
		{
				object obj = list[index];
				var val = EditorGUI.IntField(r,"Int", (int)obj);
				object val2 = val;
				list[index] = (T)val2;
				r.x += 250;		
		}
		else if(typeName.Contains("Single"))
		{
			object obj = list[index];
			var val = EditorGUI.FloatField(r, "Float", (float)obj);
			object val2 = val;
			list[index] = (T)val2;
			r.x += 250;
		}

	}
}
