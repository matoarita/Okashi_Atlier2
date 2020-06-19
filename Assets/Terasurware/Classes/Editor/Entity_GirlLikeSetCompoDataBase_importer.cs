using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_GirlLikeSetCompoDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Excel_Data/Entity_GirlLikeSetCompoDataBase.xlsx";
	private static readonly string exportPath = "Assets/Excel_Data/Entity_GirlLikeSetCompoDataBase.asset";
	private static readonly string[] sheetNames = { "01_Stage1_Set", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_GirlLikeSetCompoDataBase data = (Entity_GirlLikeSetCompoDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_GirlLikeSetCompoDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_GirlLikeSetCompoDataBase> ();
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

					Entity_GirlLikeSetCompoDataBase.Sheet s = new Entity_GirlLikeSetCompoDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_GirlLikeSetCompoDataBase.Param p = new Entity_GirlLikeSetCompoDataBase.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.set_compID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.set1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.set2 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.set3 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.spquest_name1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.spquest_name2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.desc = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.comment = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.set_flag = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.set_score = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.hint_text = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(12); p.clear = (cell == null ? false : cell.BooleanCellValue);
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
