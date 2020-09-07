using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_slotNameDataBase : ScriptableObject
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
		public string slot_Name;
		public string slot_Hyouki_1;
		public string slot_Hyouki_2;
		public int slot_totalscore;
		public int slot_getgirllove;
		public int slot_money;
	}
}

