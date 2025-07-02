using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EscapeGame.Editor{
    public class ReferencesToolMenu : UnityEditor.Editor{

        #region lorna Extend
        private static void FindAllYamls(string searchPattern)
        {
            string rootPath = Application.dataPath;
            string[] allPrefabs = Directory.GetFiles(Application.dataPath, searchPattern, SearchOption.AllDirectories);
            foreach (string filePath in allPrefabs)
            {
                string yamlContent = File.ReadAllText(filePath);
                allyamls.Add(filePath, yamlContent);
            }
        }

        private static void InitDictionaryAllCName(string targetFolder)
        {

            // 获取所有.cs文件（含完整路径）
            DirectoryInfo dirInfo = new DirectoryInfo(targetFolder);
            FileInfo[] csFiles = dirInfo.GetFiles("*.cs", SearchOption.AllDirectories);

            foreach (FileInfo file in csFiles)
            {
                if (file.Extension == ".cs") // 双重验证扩展名
                {
                    string name1 = file.Name.Split('.')[0];
                    name1 = name1 + ",";
                    allCName.Add(name1);
                }
            }
        }

        private static HashSet<string> allCName = new HashSet<string>();
        private static Dictionary<string, string> allyamls = new Dictionary<string, string>();
        //key:c#  value:yama path
        private static Dictionary<string, List<string>> map = new Dictionary<string, List<string>>();

        [MenuItem("Assets/查找引用/test1【查找BaoExtend路径下的c#文件名 和ADF：Assembly-CSharp-firstpass 均存在的资源名，导出到dictData1】", false, 104)]
        private static void test1()
        {
            string _folder = "Assets/Game/Scripts/Runtime/BaoExtend";
            string _exportPath = "dictData1.txt";
            string contentCmd = "Assembly-CSharp-firstpass";
            allCName.Clear();
            InitDictionaryAllCName(_folder);
            Check(_exportPath, (filetext)=>
            {
                return filetext.Contains(contentCmd);
            });
        }
        [MenuItem("Assets/查找引用/test2【查找 Module 路径下的c#文件名 和某些ADF 均存在的资源名，导出到 dictData2 】", false, 104)]
        private static void test2()
        {
            string _folder = "Assets/Game/Scripts/Module";
            string _exportPath = "dictData2.txt";
            string contentCmd = "EscapeGame.";
            string contentCmd2 = "Assembly-CSharp-firstpass";
            string contentCmd3 = "Assembly-CSharp";
            allCName.Clear();
            InitDictionaryAllCName(_folder);

            Check(_exportPath, (filetext) =>
            {
                return filetext.Contains(contentCmd) ||
                filetext.Contains(contentCmd2) ||
                filetext.Contains(contentCmd3);
            });
        }
        [MenuItem("Assets/查找引用/test3【查找 除module、Plugins、Ditor 路径下的c#文件名 和某些ADF 均存在的资源名，导出到dictData3】", false, 104)]
        private static void test3()
        {

            string _exportPath = "dictData3.txt";
            string contentCmd = "EscapeGame.";
            string contentCmd2 = "Assembly-CSharp-firstpass";
            string contentCmd3 = "Assembly-CSharp";
            allCName.Clear();
            InitDictionaryAllCName("Assets/Game/Res");
            InitDictionaryAllCName("Assets/Game/Scripts/Runtime");
            InitDictionaryAllCName("Assets/Game/Scripts/TempBase");
            InitDictionaryAllCName("Assets/Resources");
            InitDictionaryAllCName("Assets/Settings");
            InitDictionaryAllCName("Assets/StreamingAssets");
            Check(_exportPath, (filetext) =>
            {
                return filetext.Contains(contentCmd) ||
                filetext.Contains(contentCmd2) ||
                filetext.Contains(contentCmd3);
            });
        }
        [MenuItem("Assets/查找引用/test4【查找 third 路径下的c#文件名 和某些ADF 均存在的资源名，导出到dictData4】", false, 104)]
        private static void test4()
        {
            string _folder = "Assets/Game/Scripts/Runtime";
            string _exportPath = "dictData4.txt";
            string contentCmd = "Assembly-CSharp-firstpass";


            UnityEngine.Debug.Log("开始" + Time.realtimeSinceStartup);
            allCName.Clear();
            InitDictionaryAllCName("Assets/Plugins/DoTween");
            InitDictionaryAllCName("Assets/Plugins/FImpossible Creations");
            InitDictionaryAllCName("Assets/Plugins/RootMotion");

            Check(_exportPath, (filetext) =>
            {
                return filetext.Contains(contentCmd);
            });
        }

        [MenuItem("Assets/查找引用/test5【ADF：EscapeGame.Config 或EscapeGame.GameConfig存在的资源名，导出到dictData5】", false, 104)]
        private static void test5()
        {
            string _folder = "Assets";
            string _exportPath = "dictData5.txt";
            string contentCmd = "EscapeGame.Config";
            string contentCmd2 = "EscapeGame.GameConfig";
            allCName.Clear();
            Check(_exportPath, (filetext) =>
            {
                return filetext.Contains(contentCmd) || filetext.Contains(contentCmd2);
            });
        }

        private static List<string> adfList = new List<string>();
        [MenuItem("Assets/查找引用/test6【所有被删的ADF：存在的资源名，导出到dictData6】", false, 104)]
        private static void test6()
        {
            string _folder = "Assets";
            string _exportPath = "dictData6.txt";
            adfList.Clear();
            adfList.Add("EscapeGame.Extensions");
            adfList.Add("EscapeGame.Runtime.Log");
            adfList.Add("EscapeGame.Module.Operate.Runtime");
            adfList.Add("EscapeGame.Wrap.Runtime");
            adfList.Add("EscapeGame.GameConfig");
            adfList.Add("EscapeGame.Const");
            adfList.Add("EscapeGame.Input");
            adfList.Add("EscapeGame.Core");
            adfList.Add("EscapeGame.Event");
            adfList.Add("EscapeGame.GamePipline");
            adfList.Add("EscapeGame.Config");
            adfList.Add("EscapeGame.Utils");
            adfList.Add("EscapeGame.Utils.ManagerUtils");
            adfList.Add("EscapeGame.Utils.ScriptableSingleton");
            allCName.Clear();
            Check(_exportPath, (filetext) =>
            {
                foreach(var j in adfList)
                {
                    if( filetext.Contains(j))
                    {
                        return true;
                    }
                }
                return false;
            });
        }
        private static void Check(string _exportPath, Func<string,bool> _check)
        {
            allyamls.Clear();
            FindAllYamls("*.prefab");
            FindAllYamls("*.unity");
            FindAllYamls("*.asset");
            UnityEngine.Debug.Log("yamal find success" + Time.realtimeSinceStartup);
            map.Clear();
            foreach (var i in allyamls)
            {
                //有条件且条件不满足，执行下一个
                if (_check != null && !_check.Invoke(i.Value))
                {
                    continue;
                }

                if(allCName.Count <= 0)
                {
                    if (!map.ContainsKey("Test"))
                    {
                        map.Add("Test", new List<string>());
                    }
                    map["Test"].Add(i.Key);
                    continue;
                }

                foreach (var j in allCName)
                {
                    if (i.Value.Contains(j))
                    {
                        if (!map.ContainsKey(j))
                        {
                            map.Add(j, new List<string>());
                        }
                        map[j].Add(i.Key);
                        break;
                    }
                }
            }
            ExportToTxt(_exportPath);
            UnityEngine.Debug.Log("结束" + Time.realtimeSinceStartup);
        }

        private static void ExportToTxt(string _exportPath)
        {
            string filePath = Path.Combine(Application.dataPath, _exportPath);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var kvp in map)
                {
                    sw.WriteLine($"{kvp.Key}"); // 格式示例：Health:100
                    foreach (var j in kvp.Value)
                    {
                        sw.WriteLine($"       {j}"); // 格式示例：Health:100
                    }
                }
            }
            UnityEngine.Debug.Log($"TXT 导出成功！路径：{filePath}");
        }
        #endregion
    }
}
