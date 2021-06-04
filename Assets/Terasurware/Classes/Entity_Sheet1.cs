using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_Sheet1 : ScriptableObject
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
		
		public int ItemID;
		public string file_name;
		public string place_Name;
		public string place_Name_Hyouji;
		public string comment;
		public int place_day;
		public int place_cost;
		public int place_hp;
		public int place_flag;
		public int place_type;
		public string drop_item1;
		public string drop_item2;
		public string drop_item3;
		public string drop_item4;
		public string drop_item5;
		public string drop_item6;
		public string drop_item7;
		public string drop_item8;
		public string drop_item9;
		public string drop_item10;
		public string drop_rare1;
		public string drop_rare2;
		public string drop_rare3;
		public float drop_prob1;
		public float drop_prob2;
		public float drop_prob3;
		public float drop_prob4;
		public float drop_prob5;
		public float drop_prob6;
		public float drop_prob7;
		public float drop_prob8;
		public float drop_prob9;
		public float drop_prob10;
		public float drop_rare_prob1;
		public float drop_rare_prob2;
		public float drop_rare_prob3;
		public string center_bg;
		public string back_bg;
	}
}

