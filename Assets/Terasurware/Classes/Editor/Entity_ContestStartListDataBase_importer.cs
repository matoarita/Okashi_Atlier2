using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_ContestStartListDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Excel/Entity_ContestStartListDataBase.xlsx";
	private static readonly string exportPath = "Assets/Resources/Excel/Entity_ContestStartListDataBase.asset";
	private static readonly string[] sheetNames = { "Or_Contest_001","Or_Contest_002","Or_Contest_003","Or_Contest_004", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_ContestStartListDataBase data = (Entity_ContestStartListDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_ContestStartListDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_ContestStartListDataBase> ();
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

					Entity_ContestStartListDataBase.Sheet s = new Entity_ContestStartListDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_ContestStartListDataBase.Param p = new Entity_ContestStartListDataBase.Param ();
						
					cell = row.GetCell(0); p.ContestID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.Contest_placeNum = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.file_name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.Contest_Name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.Contest_Name_Hyouji = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.theme_comment = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.Contest_Pmonth = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.Contest_Pday = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.Contest_Endmonth = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.Contest_Endday = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.Contest_cost = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.Contest_flag = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.Contest_PatissierRank = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.Contest_Lv = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.Contest_BringType = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.Contest_BringMax = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.Contest_RankingType = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.Contest_Accepted = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.GetPatissierPoint = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(19); p.ContestVictory = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.ContestFightsCount = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(21); p.ContestBGName = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(22); p.ContestBGChubouName = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(23); p.ContestBGMSelect = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(24); p.comment_out = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(25); p.read_endflag = (int)(cell == null ? 0 : cell.NumericCellValue);
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
