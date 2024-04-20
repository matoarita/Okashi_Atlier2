using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Entity_shopItemDataBase_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Excel/Entity_shopItemDataBase.xlsx";
	private static readonly string exportPath = "Assets/Resources/Excel/Entity_shopItemDataBase.asset";
	private static readonly string[] sheetNames = { "ShopItemDB_1","FarmItemDB_1","EmeraldShopItemDB_1","Or_ShopItemDB_1","Or_ShopItemDB_2","Or_ShopItemDB_3","Or_ShopItemDB_4", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_shopItemDataBase data = (Entity_shopItemDataBase)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_shopItemDataBase));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_shopItemDataBase> ();
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

					Entity_shopItemDataBase.Sheet s = new Entity_shopItemDataBase.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_shopItemDataBase.Param p = new Entity_shopItemDataBase.Param ();
						
					cell = row.GetCell(0); p.ShopID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.zaiko = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.itemType = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.dongriType = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.shop_sell_price = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.shop_buy_price = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.item_hyouji = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.item_hyouji_on = (cell == null ? false : cell.BooleanCellValue);
					cell = row.GetCell(9); p.read_endflag = (int)(cell == null ? 0 : cell.NumericCellValue);
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
