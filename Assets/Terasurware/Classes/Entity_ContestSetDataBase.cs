using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_ContestSetDataBase : ScriptableObject
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
		public int set_score;
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
		public int Sp1_Wind;
		public int Sp2_Sco;
		public int Sp3_Sco;
		public int Sp4_Sco;
		public int Sp5_Sco;
		public int Sp6_Wind;
		public int Sp7_Sco;
		public int Sp8_Sco;
		public int Sp9_Sco;
		public int Sp10_Sco;
		public string topping01;
		public string topping02;
		public string topping03;
		public string topping04;
		public string topping05;
		public string topping06;
		public string topping07;
		public string topping08;
		public string topping09;
		public int tp_score01;
		public int tp_score02;
		public int tp_score03;
		public int tp_score04;
		public int tp_score05;
		public int tp_score06;
		public int tp_score07;
		public int tp_score08;
		public int tp_score09;
		public int Non_tpscore;
		public string desc;
		public int commet_flag;
		public int search_endflag;
	}
}

