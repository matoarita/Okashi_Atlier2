using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_ItemDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Excel_Data/Entity_ItemDataBase.xlsx";
	private static readonly string exportPath = "Assets/Excel_Data/Entity_ItemDataBase.asset";
	private static readonly string[] sheetNames = { "01_ItemDB_Material","02_ItemDB_Okashi","03_ItemDB_Potion","04_ItemDB_Etc", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_ItemDataBase data = (Entity_ItemDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_ItemDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_ItemDataBase> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					Entity_ItemDataBase.Sheet s = new Entity_ItemDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_ItemDataBase.Param p = new Entity_ItemDataBase.Param ();
						
					cell = row.GetCell(0); p.ItemID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.file_name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.nameHyouji = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.desc = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.comp_hosei = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.hp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.day = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.quality = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.exp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.ex_probability = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.rich = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.sweat = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.bitter = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.sour = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.crispy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.fluffy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.smooth = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.hardness = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(19); p.jiggly = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.chewy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(21); p.powdery = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(22); p.oily = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(23); p.watery = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(24); p.type = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(25); p.subtype = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(26); p.girl1_like = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(27); p.cost_price = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(28); p.sell_price = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(29); p.topping01 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(30); p.topping02 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(31); p.topping03 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(32); p.topping04 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(33); p.topping05 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(34); p.topping06 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(35); p.topping07 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(36); p.topping08 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(37); p.topping09 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(38); p.topping10 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(39); p.koyu_topping1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(40); p.koyu_topping2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(41); p.koyu_topping3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(42); p.koyu_topping4 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(43); p.koyu_topping5 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(44); p.item_hyouji = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(45); p.Set_JudgeNum = (int)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
