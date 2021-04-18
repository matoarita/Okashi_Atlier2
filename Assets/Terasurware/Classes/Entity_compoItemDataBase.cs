using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_compoItemDataBase : ScriptableObject
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
		public string cmpitemName;
		public string cmpitemID_1;
		public string cmpitemID_2;
		public string cmpitemID_3;
		public string cmp_subtype_1;
		public string cmp_subtype_2;
		public string cmp_subtype_3;
		public string result_itemID;
		public int result_kosu;
		public int cmpitem_kosu1;
		public int cmpitem_kosu2;
		public int cmpitem_kosu3;
		public float best_kosu1;
		public float best_kosu2;
		public float best_kosu3;
		public int cmp_flag;
		public int cost_time;
		public int success_rate;
		public int renkin_Bexp;
		public string Comment;
		public string Comment2;
		public string KeisanMethod;
		public int comp_count;
		public string release_recipi;
		public int recipi_count;
	}
}

