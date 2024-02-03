using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_magicSkillListDataBase : ScriptableObject
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
		
		public int skillID;
		public int skill_koyuID;
		public string file_name;
		public string skill_Name;
		public string skill_Name_Hyouji;
		public string comment;
		public int skill_day;
		public int skill_cost;
		public int skill_flag;
		public int skill_lv;
		public int skill_maxlv;
		public int skill_uselv;
		public int skill_type;
		public int skill_category;
		public int success_rate;
		public string comment_full;
	}
}

