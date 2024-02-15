using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_ItemDataBase : ScriptableObject
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
		public string name;
		public string nameHyouji;
		public string desc;
		public int comp_hosei;
		public int hp;
		public int day;
		public int quality;
		public int exp;
		public int ex_probability;
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
		public int powdery;
		public int oily;
		public int watery;
		public int beauty;
		public string type;
		public string subtype;
		public string subtypeB;
		public string subtype_category;
		public int base_score;
		public int girl1_like;
		public int cost_price;
		public int sell_price;
		public string topping01;
		public string topping02;
		public string topping03;
		public string topping04;
		public string topping05;
		public string topping06;
		public string topping07;
		public string topping08;
		public string topping09;
		public string topping10;
		public string koyu_topping1;
		public string koyu_topping2;
		public string koyu_topping3;
		public string koyu_topping4;
		public string koyu_topping5;
		public int item_hyouji;
		public int Set_JudgeNum;
		public int Rare;
		public int Manpuku;
		public int SecretFlag;
	}
}

