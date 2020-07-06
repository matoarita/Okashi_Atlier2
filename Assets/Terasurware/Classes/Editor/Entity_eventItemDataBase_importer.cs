using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_eventItemDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Excel_Data/Entity_eventItemDataBase.xlsx";
	private static readonly string exportPath = "Assets/Excel_Data/Entity_eventItemDataBase.asset";
	private static readonly string[] sheetNames = { "01_event_recipi", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_eventItemDataBase data = (Entity_eventItemDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_eventItemDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_eventItemDataBase> ();
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

					Entity_eventItemDataBase.Sheet s = new Entity_eventItemDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_eventItemDataBase.Param p = new Entity_eventItemDataBase.Param ();
						
					cell = row.GetCell(0); p.ev_ItemID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.fileName = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.nameHyouji = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.cost_price = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.sell_price = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.kosu = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.read_flag = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.list_hyouji_on = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.memo = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(10); p.Re_flag_num = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.Ev_flag_num = (int)(cell == null ? 0 : cell.NumericCellValue);
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
