using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_ContestSetDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Excel/Entity_ContestSetDataBase.xlsx";
	private static readonly string exportPath = "Assets/Resources/Excel/Entity_ContestSetDataBase.asset";
	private static readonly string[] sheetNames = { "01_ContestSetData1","02_Contest_D01","02_Contest_D100","02_Contest_D101", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_ContestSetDataBase data = (Entity_ContestSetDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_ContestSetDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_ContestSetDataBase> ();
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

					Entity_ContestSetDataBase.Sheet s = new Entity_ContestSetDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_ContestSetDataBase.Param p = new Entity_ContestSetDataBase.Param ();
						
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
					cell = row.GetCell(15); p.juice = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.beauty = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.Sp1_Wind = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.Sp2_Sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(19); p.Sp3_Sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.Sp4_Sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(21); p.Sp5_Sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(22); p.Sp6_Wind = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(23); p.Sp7_Sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(24); p.Sp8_Sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(25); p.Sp9_Sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(26); p.Sp10_Sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(27); p.topping01 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(28); p.topping02 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(29); p.topping03 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(30); p.topping04 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(31); p.topping05 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(32); p.topping06 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(33); p.topping07 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(34); p.topping08 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(35); p.topping09 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(36); p.tp_score01 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(37); p.tp_score02 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(38); p.tp_score03 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(39); p.tp_score04 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(40); p.tp_score05 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(41); p.tp_score06 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(42); p.tp_score07 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(43); p.tp_score08 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(44); p.tp_score09 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(45); p.Non_tpscore = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(46); p.desc = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(47); p.commet_flag = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(48); p.search_endflag = (int)(cell == null ? 0 : cell.NumericCellValue);
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
