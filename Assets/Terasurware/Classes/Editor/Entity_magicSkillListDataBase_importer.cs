using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_magicSkillListDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Excel_Data/Entity_magicSkillListDataBase.xlsx";
	private static readonly string exportPath = "Assets/Excel_Data/Entity_magicSkillListDataBase.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_magicSkillListDataBase data = (Entity_magicSkillListDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_magicSkillListDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_magicSkillListDataBase> ();
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

					Entity_magicSkillListDataBase.Sheet s = new Entity_magicSkillListDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_magicSkillListDataBase.Param p = new Entity_magicSkillListDataBase.Param ();
						
					cell = row.GetCell(0); p.skillID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.skill_koyuID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.file_name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.skill_Name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.skill_Name_Hyouji = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.comment = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.skill_day = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.skill_cost = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.skill_flag = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.skill_lv = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.skill_maxlv = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.skill_uselv = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.skill_lvSelect = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(13); p.skill_type = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.skill_category = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.success_rate = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.cost_time = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.comment_full = (cell == null ? "" : cell.StringCellValue);
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
