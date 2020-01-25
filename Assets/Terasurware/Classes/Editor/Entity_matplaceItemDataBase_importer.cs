using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_matplaceItemDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Excel_Data/Entity_matplaceItemDataBase.xlsx";
	private static readonly string exportPath = "Assets/Excel_Data/Entity_matplaceItemDataBase.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_matplaceItemDataBase data = (Entity_matplaceItemDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_matplaceItemDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_matplaceItemDataBase> ();
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

					Entity_matplaceItemDataBase.Sheet s = new Entity_matplaceItemDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_matplaceItemDataBase.Param p = new Entity_matplaceItemDataBase.Param ();
						
					cell = row.GetCell(0); p.ItemID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.place_Name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.place_Name_Hyouji = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.place_cost = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.place_flag = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.drop_item1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.drop_item2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.drop_item3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.drop_item4 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.drop_item5 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(10); p.drop_rare1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(11); p.drop_rare2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(12); p.drop_prob1 = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.drop_prob2 = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.drop_prob3 = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.drop_prob4 = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.drop_prob5 = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.drop_rare_prob1 = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.drop_rare_prob2 = (float)(cell == null ? 0 : cell.NumericCellValue);
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
