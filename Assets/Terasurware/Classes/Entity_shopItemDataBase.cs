using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_shopItemDataBase : ScriptableObject
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
		public string name;
		public int zaiko;
		public int itemType;
		public int shop_sell_price;
		public int shop_buy_price;
		public int item_hyouji;
	}
}

