using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_ContestStartListDataBase : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public int ContestID;
		public int Contest_placeNum;
		public string file_name;
		public string Contest_Name;
		public string Contest_Name_Hyouji;
		public string theme_comment;
		public int Contest_Pmonth;
		public int Contest_Pday;
		public int Contest_cost;
		public int Contest_flag;
		public int Contest_PatissierRank;
		public int Contest_Lv;
		public int Contest_BringType;
		public int Contest_BringMax;
		public int Contest_RankingType;
		public int Contest_Accepted;
		public int GetPatissierPoint;
		public string comment_out;
		public int read_endflag;
	}
}

