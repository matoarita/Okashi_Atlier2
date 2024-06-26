using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_ItemDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Excel/Entity_ItemDataBase.xlsx";
	private static readonly string exportPath = "Assets/Resources/Excel/Entity_ItemDataBase.asset";
	private static readonly string[] sheetNames = { "01_ItemDB_Material","02_ItemDB_Okashi","03_ItemDB_Potion","04_ItemDB_Etc", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_ItemDataBase data = (Entity_ItemDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_ItemDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_ItemDataBase> ();
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

					Entity_ItemDataBase.Sheet s = new Entity_ItemDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_ItemDataBase.Param p = new Entity_ItemDataBase.Param ();
						
					cell = row.GetCell(0); p.ItemID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.file_name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.nameHyouji = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.desc = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.comp_hosei = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.hp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.day = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.quality = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.exp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.ex_probability = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.rich = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.sweat = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.bitter = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.sour = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.crispy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.fluffy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.smooth = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.hardness = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(19); p.jiggly = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.chewy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(21); p.powdery = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(22); p.oily = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(23); p.watery = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(24); p.beauty = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(25); p.tea_flavor = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(26); p.SP_wind = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(27); p.SP2_sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(28); p.SP3_sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(29); p.SP4_sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(30); p.SP5_sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(31); p.SP6_sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(32); p.SP7_sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(33); p.SP8_sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(34); p.SP9_sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(35); p.SP10_sco = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(36); p.type = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(37); p.subtype = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(38); p.subtypeB = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(39); p.subtype_category = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(40); p.base_score = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(41); p.girl1_like = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(42); p.cost_price = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(43); p.sell_price = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(44); p.topping01 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(45); p.topping02 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(46); p.topping03 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(47); p.topping04 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(48); p.topping05 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(49); p.topping06 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(50); p.topping07 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(51); p.topping08 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(52); p.topping09 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(53); p.topping10 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(54); p.koyu_topping1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(55); p.koyu_topping2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(56); p.koyu_topping3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(57); p.koyu_topping4 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(58); p.koyu_topping5 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(59); p.item_hyouji = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(60); p.Set_JudgeNum = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(61); p.Rare = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(62); p.Manpuku = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(63); p.Magic = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(64); p.Attribute1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(65); p.SecretFlag = (int)(cell == null ? 0 : cell.NumericCellValue);
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
