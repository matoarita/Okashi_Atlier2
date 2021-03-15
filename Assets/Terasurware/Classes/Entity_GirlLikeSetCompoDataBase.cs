using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_GirlLikeSetCompoDataBase : ScriptableObject
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
		
		public int ID;
		public int set_compID;
		public int set1;
		public int set2;
		public int set3;
		public string spquest_name1;
		public string spquest_name2;
		public string spquest_name3;
		public string desc;
		public string comment;
		public int set_flag;
		public int set_score;
		public string hint_text;
		public bool clear;
	}
}

