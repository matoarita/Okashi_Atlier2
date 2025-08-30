using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_QuestSetDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Excel/Entity_QuestSetDataBase.xlsx";
	private static readonly string exportPath = "Assets/Resources/Excel/Entity_QuestSetDataBase.asset";
	private static readonly string[] sheetNames = { "01_QuestSetData","Or_BarQuestData_1","Or_BarQuestData_2","Or_BarQuestData_3","Or_BarQuestData_4","Or_ClientQuestData", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_QuestSetDataBase data = (Entity_QuestSetDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_QuestSetDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_QuestSetDataBase> ();
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

					Entity_QuestSetDataBase.Sheet s = new Entity_QuestSetDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_QuestSetDataBase.Param p = new Entity_QuestSetDataBase.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.QuestID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.QuestType = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.QuestHyouji = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.QuestHyoujiHeart = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.HighType = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.GirlJudgeUse = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.GirlSetJudgeNum = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.GirlSetScore = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.file_name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(10); p.quest_itemName = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(11); p.quest_itemName2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(12); p.quest_itemName3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(13); p.quest_itemName4 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(14); p.quest_itemName5 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(15); p.quest_itemName6 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(16); p.quest_itemName7 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(17); p.quest_itemName8 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(18); p.quest_itemsubtype = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(19); p.kosu_default = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.kosu_min = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(21); p.kosu_max = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(22); p.buy_price = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(23); p.rich = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(24); p.sweat = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(25); p.bitter = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(26); p.sour = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(27); p.crispy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(28); p.fluffy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(29); p.smooth = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(30); p.hardness = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(31); p.jiggly = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(32); p.chewy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(33); p.juice = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(34); p.beauty = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(35); p.tea_flavor = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(36); p.topping01 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(37); p.topping02 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(38); p.topping03 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(39); p.topping04 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(40); p.topping05 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(41); p.tp_score1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(42); p.tp_score2 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(43); p.tp_score3 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(44); p.tp_score4 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(45); p.tp_score5 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(46); p.quest_afterday = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(47); p.limit_month = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(48); p.limit_day = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(49); p.area_Type = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(50); p.ClientName = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(51); p.ClientNumber = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(52); p.quest_Title = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(53); p.desc = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(54); p.read_endflag = (int)(cell == null ? 0 : cell.NumericCellValue);
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
