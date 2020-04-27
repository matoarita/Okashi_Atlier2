using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_compoItemDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Excel_Data/Entity_compoItemDataBase.xlsx";
	private static readonly string exportPath = "Assets/Excel_Data/Entity_compoItemDataBase.asset";
	private static readonly string[] sheetNames = { "Compound_ItemDB", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_compoItemDataBase data = (Entity_compoItemDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_compoItemDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_compoItemDataBase> ();
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

					Entity_compoItemDataBase.Sheet s = new Entity_compoItemDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_compoItemDataBase.Param p = new Entity_compoItemDataBase.Param ();
						
					cell = row.GetCell(0); p.ItemID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.cmpitemName = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.cmpitemID_1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.cmpitemID_2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.cmpitemID_3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.cmp_subtype_1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.cmp_subtype_2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.cmp_subtype_3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.result_itemID = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.result_kosu = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.cmpitem_kosu1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.cmpitem_kosu2 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.cmpitem_kosu3 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.cmp_flag = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.cost_time = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.success_rate = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.renkin_Bexp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.Comment = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(18); p.Comment2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(20); p.KeisanMethod = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(21); p.comp_count = (int)(cell == null ? 0 : cell.NumericCellValue);
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
