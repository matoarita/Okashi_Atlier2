using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_ContestCommentDataBase : ScriptableObject
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
		public int commentID;
		public string item_Name;
		public int setID;
		public string comment1;
		public string comment2;
		public string comment3;
		public string comment4;
		public string Memo;
	}
}

