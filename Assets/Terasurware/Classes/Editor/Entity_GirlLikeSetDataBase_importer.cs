using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_GirlLikeSetDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Excel_Data/Entity_GirlLikeSetDataBase.xlsx";
	private static readonly string exportPath = "Assets/Excel_Data/Entity_GirlLikeSetDataBase.asset";
	private static readonly string[] sheetNames = { "01_GirlLikeSetData", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_GirlLikeSetDataBase data = (Entity_GirlLikeSetDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_GirlLikeSetDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_GirlLikeSetDataBase> ();
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

					Entity_GirlLikeSetDataBase.Sheet s = new Entity_GirlLikeSetDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_GirlLikeSetDataBase.Param p = new Entity_GirlLikeSetDataBase.Param ();
						
					cell = row.GetCell(0); p.setID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.compNum = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.girllike_itemname = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.girllike_itemsubtype = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.set_score = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.rich = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.sweat = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.bitter = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.sour = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.crispy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.fluffy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.smooth = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.hardness = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.jiggly = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.chewy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.topping01 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(16); p.topping02 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(17); p.topping03 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(18); p.topping04 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(19); p.topping05 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(20); p.desc = (cell == null ? "" : cell.StringCellValue);
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
