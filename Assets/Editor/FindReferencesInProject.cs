using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FindReferencesInProject
{
    /// <summary>
    /// 選択したファイルがどこで参照されているかを調べます。
    /// </summary>
    public class FindReferencesInProject : EditorWindow
    {
        private static Dictionary<AssetData, List<AssetData>> results = new Dictionary<AssetData, List<AssetData>>();
        private static Dictionary<AssetData, bool> foldOuts = new Dictionary<AssetData, bool>();
        private Vector2 ScrollPosition = Vector2.zero;
        [MenuItem("Assets/Find References In Project", true)]
        static bool IsEnabled()
        {
            return Selection.objects.Any();
        }
        [MenuItem("Assets/Find References In Project", false, 25)]
        static void Search()
        {
            results.Clear();
            foldOuts.Clear();
            var isBreak = false;
            // 検索対象のディレクトリ
            var targets = AssetDatabase.FindAssets("t:Scene t:Prefab t:material",
                new string[]
                {
                    "Assets",
                }
                ).Select(AssetData.CreateByGuid).ToList();
            // 全てのアセットの中から検索
            for (int ii = 0; ii < targets.Count; ii++)
            {
                if (isBreak)
                {
                    break;
                }
                var target = targets[ii];
                if (EditorUtility.DisplayCancelableProgressBar("参照ファイルチェック中", target.Name, (float)ii / (float)targets.Count))
                {
                    break;
                }
                var referents = AssetDatabase.GetDependencies(target.Path).Select(AssetData.CreateByPath).ToList();
                referents.Remove(target);
                for (int jj = 0; jj < referents.Count; jj++)
                {
                    if (isBreak)
                    {
                        break;
                    }
                    var referent = referents[jj];
                    foreach (var selected in Selection.objects.Select(AssetData.CreateByObject))
                    {
                        if (isBreak)
                        {
                            break;
                        }
                        if (referent.Equals(selected))
                        {
                            results.AddSafety(referent, new List<AssetData>());
                            results[referent].Add(target);
                            if (results.Count >= 30)
                            {
                                isBreak = true;
                            }
                        }
                    }
                }
            }
            EditorUtility.ClearProgressBar();
            GetWindow<FindReferencesInProject>();
        }
        void OnGUI()
        {
            this.ScrollPosition = GUILayout.BeginScrollView(this.ScrollPosition);
            foreach (var referent in results.Keys.OrderBy(key => key.Name).ToList())
            {
                foldOuts.AddSafety(referent, true);
                if (foldOuts[referent] = EditorGUILayout.Foldout(foldOuts[referent], referent.Name))
                {
                    foreach (var target in results[referent])
                    {
                        var iconSize = EditorGUIUtility.GetIconSize();
                        EditorGUIUtility.SetIconSize(Vector2.one * 16);
                        if (GUILayout.Button(target.Name))
                        {
                            var obj = target.ToObject();
                            Selection.objects = new[] { obj };
                        }
                        EditorGUIUtility.SetIconSize(iconSize);
                    }
                }
            }
            GUILayout.EndScrollView();
        }
    }
    public class AssetData
    {
        public string Name { get; }
        public string Path { get; }
        public string Guid { get; }
        public AssetData(string name, string path, string guid)
        {
            this.Name = name;
            this.Path = path;
            this.Guid = guid;
        }
        public static AssetData CreateByObject(Object obj)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            var guid = AssetDatabase.AssetPathToGUID(path);
            var name = obj.name;
            return new AssetData(name, path, guid);
        }
        public static AssetData CreateByPath(string path)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            var name = System.IO.Path.GetFileName(path);
            return new AssetData(name, path, guid);
        }
        public static AssetData CreateByGuid(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var name = System.IO.Path.GetFileName(path);
            return new AssetData(name, path, guid);
        }
        public Object ToObject()
        {
            return AssetDatabase.LoadAssetAtPath<Object>(this.Path);
        }
        public override bool Equals(object obj)
        {
            var other = obj as AssetData;
            Debug.Assert(other != null);
            return this.Guid == other.Guid;
        }
        public override int GetHashCode()
        {
            return this.Guid.GetHashCode();
        }
    }
    public static class DictionaryExtension
    {
        public static void AddSafety<K, V>(this Dictionary<K, V> self, K key, V value)
        {
            if (!self.ContainsKey(key))
            {
                self.Add(key, value);
            }
        }
    }
}