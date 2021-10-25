using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_QuestSetDataBase : ScriptableObject
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
		public int QuestID;
		public int QuestType;
		public int QuestHyouji;
		public string file_name;
		public string quest_itemName;
		public string quest_itemsubtype;
		public int kosu_default;
		public int kosu_min;
		public int kosu_max;
		public int buy_price;
		public int rich;
		public int sweat;
		public int bitter;
		public int sour;
		public int crispy;
		public int fluffy;
		public int smooth;
		public int hardness;
		public int jiggly;
		public int chewy;
		public int juice;
		public int beauty;
		public string topping01;
		public string topping02;
		public string topping03;
		public string topping04;
		public string topping05;
		public int tp_score1;
		public int tp_score2;
		public int tp_score3;
		public int tp_score4;
		public int tp_score5;
		public string quest_Title;
		public string desc;
	}
}

