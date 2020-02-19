using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_GirlLikeSetDataBase : ScriptableObject
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
		
		public int setID;
		public int compNum;
		public string girllike_itemname;
		public string girllike_itemsubtype;
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
		public string topping01;
		public string topping02;
		public string topping03;
		public string topping04;
		public string topping05;
		public string desc;
	}
}

