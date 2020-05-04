using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_slotNameDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Excel_Data/Entity_slotNameDataBase.xlsx";
	private static readonly string exportPath = "Assets/Excel_Data/Entity_slotNameDataBase.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_slotNameDataBase data = (Entity_slotNameDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_slotNameDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_slotNameDataBase> ();
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

					Entity_slotNameDataBase.Sheet s = new Entity_slotNameDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_slotNameDataBase.Param p = new Entity_slotNameDataBase.Param ();
						
					cell = row.GetCell(0); p.ItemID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.slot_Name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.slot_Hyouki_1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.slot_Hyouki_2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.slot_totalscore = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.slot_girlscore = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.slot_money = (int)(cell == null ? 0 : cell.NumericCellValue);
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
