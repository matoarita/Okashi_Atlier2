using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_ContestCommentDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Excel_Data/Entity_ContestCommentDataBase.xlsx";
	private static readonly string exportPath = "Assets/Excel_Data/Entity_ContestCommentDataBase.asset";
	private static readonly string[] sheetNames = { "01_ContestComment","02_ContestComment", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_ContestCommentDataBase data = (Entity_ContestCommentDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_ContestCommentDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_ContestCommentDataBase> ();
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

					Entity_ContestCommentDataBase.Sheet s = new Entity_ContestCommentDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_ContestCommentDataBase.Param p = new Entity_ContestCommentDataBase.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.commentID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.item_Name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.setID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.comment1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.comment2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.comment3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.comment4 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.Memo = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.search_endflag = (int)(cell == null ? 0 : cell.NumericCellValue);
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
