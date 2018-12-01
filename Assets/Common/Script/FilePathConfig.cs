using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲームでも使うパスを記入
//EditorのみはNG
static public class FilePathConfig
{
	static public string PartDataBasePath = "Assets/SceneData/Unit/UnitData";
	static public string HeadPartDataPath = PartDataBasePath + "/HeadPart";
	static public string CorePartDataPath = PartDataBasePath + "/CorePart";
	static public string WeponPartDataPath = PartDataBasePath + "/Wepon";
	static public string LegPartDataPath = PartDataBasePath + "/Leg";
}
