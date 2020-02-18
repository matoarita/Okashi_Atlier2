using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_eventItemDataBase : ScriptableObject
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
		
		public int ev_ItemID;
		public string name;
		public string nameHyouji;
		public int cost_price;
		public int sell_price;
		public int kosu;
		public int read_flag;
		public int list_hyouji_on;
		public string memo;
	}
}

