using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

//转为UTF8格式：
public class FolderOperations : EditorWindow
{
    /// <summary>
    /// exe放置的文件夹
    /// </summary>
    private string exeFolder = "E:/ExportExe";
    /// <summary>
    /// exe对应的 StreamingAssets 文件夹
    /// </summary>
    private string exeStreamingAssetFolder = "E:/ExportExe/hybridTest_Data/StreamingAssets";
    /// <summary>
    /// 热更文件名
    /// </summary>
    private string hotFileName = "HotUpdate.dll";
    /// <summary>
    /// 项目中要被克隆的 HotUpdate.dll 文件 所在的文件夹
    /// </summary>
    private string dllFoler = "HybridCLRData/HotUpdateDlls/StandaloneWindows64";
    /// <summary>
    /// 重命名后的名称
    /// </summary>
    private string renameFileName = "HotUpdate.dll.bytes";

    [MenuItem("Tools/Folder Operations")]
    public static void ShowWindow()
    {
        GetWindow<FolderOperations>("文件夹操作工具");
    }

    void OnGUI()
    {
        GUILayout.Label("文件夹操作设置", EditorStyles.boldLabel);
        exeFolder = EditorGUILayout.TextField("exe放置的文件夹", exeFolder);
        exeStreamingAssetFolder = EditorGUILayout.TextField("exe对应的StreamingAssets文件夹", exeStreamingAssetFolder);
        dllFoler = EditorGUILayout.TextField("要克隆的文件所在的文件夹", dllFoler);
        GUILayout.Label("热更文件名          " + hotFileName, EditorStyles.boldLabel);
        GUILayout.Label("重命名后的名称      " + renameFileName, EditorStyles.boldLabel);


        if (GUILayout.Button("一键出包前操作"))
        {
            EditorApplication.ExecuteMenuItem("HybridCLR/Generate/All");
        }
        if (GUILayout.Button("一键出包"))
        {
            ExportEXE();
        }

        GUILayout.Label("修改完代码后，执行此按键进行热更新，热更完毕后可打开EXE看效果" + renameFileName, EditorStyles.boldLabel);
        if (GUILayout.Button("一键热更前操作"))
        {
            EditorApplication.ExecuteMenuItem("HybridCLR/CompileDll/ActiveBuildTarget");
        }
        if (GUILayout.Button("一键热更"))
        {
            DoHotFix();
        }
    }

   /// <summary>
   /// 一键出包
   /// </summary>
    private void ExportEXE()
    {
        //1、运行菜单 HybridCLR/Generate/All 进行必要的生成操作
        //2、将{proj}/HybridCLRData/HotUpdateDlls/StandaloneWindows64(MacOS下为StandaloneMacXxx)目录下的HotUpdate.dll复制到Assets/StreamingAssets/HotUpdate.dll.bytes，注意，要加.bytes后缀
        //3、打开Build Settings对话框，点击Build And Run，打包并且运行热更新示例工程

        //EditorApplication.ExecuteMenuItem("HybridCLR/Generate/All");

        string deleteFilePath = string.Concat(Application.streamingAssetsPath, "/", renameFileName);
        DeleteFile(deleteFilePath);

        // 2. 将B文件内容克隆到A
        string dllFile = string.Concat(dllFoler, "/", hotFileName);
        CopyFile(dllFile, Application.streamingAssetsPath);

        //3、出包
        BuildAndRun();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 执行热更新
    /// </summary>
    private void DoHotFix()
    {

        //1、修改Assets/HotUpdate/Hello.cs的Run函数中Debug.Log("Hello, HybridCLR");代码，改成Debug.Log("Hello, World");。
        //2、运行菜单命令HybridCLR / CompileDll / ActiveBulidTarget重新编译热更新代码。
        //3、将{ proj}/ HybridCLRData / HotUpdateDlls / StandaloneWindows64(MacOS下为StandaloneMacXxx)目录下的HotUpdate.dll复制为刚才的打包输出目录的 XXX_Data/ StreamingAssets / HotUpdate.dll.bytes


        // 1. 删除文件夹下所有文件
        DeleteFilesInFolder(exeStreamingAssetFolder);

        // 2. 将B文件内容克隆到A
        string dllFile = string.Concat(dllFoler, "/", hotFileName);
        CopyFile(dllFile, exeStreamingAssetFolder);

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 删除对应文件
    /// </summary>
    /// <param name="filePath"></param>
    private void DeleteFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"源文件不存在: {filePath}");
            return;
        }
        File.Delete(filePath);
    }

    /// <summary>
    /// 删除文件夹里的所有文件
    /// </summary>
    /// <param name="folderPath"></param>
    private void DeleteFilesInFolder(string folderPath)
    {
        // 0. 安全检查
        if (!Directory.Exists(folderPath))
        {
            UnityEngine.Debug.LogError($"路径不存在: {folderPath}");
            return;
        }

        try
        {
            // 1. 删除所有文件
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                File.Delete(file);
                UnityEngine.Debug.Log($"已删除: {file}");
            }

            // 2. 删除所有子文件夹（可选）
            string[] subDirs = Directory.GetDirectories(folderPath);
            foreach (string dir in subDirs)
            {
                Directory.Delete(dir, true); // true表示递归删除子内容
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError($"删除失败: {e.Message}");
        }
    }

    /// <summary>
    /// 复制某文件到对应文件夹
    /// </summary>
    private void CopyFile(string sourceFilePath, string targetFolderPath)
    {
        //// 项目内文件路径（Assets下的相对路径）
        //string sourceFile = "Assets/B/example.txt";
        //// 转换为绝对路径
        //string sourcePath = Path.Combine(Application.dataPath, "B/example.txt");
        //// 外部文件夹绝对路径
        //string targetPath = @"D:\A\example.txt";

        try
        {
            // 检查源文件是否存在
            if (!File.Exists(sourceFilePath))
            {
                Debug.LogError($"源文件不存在: {sourceFilePath}");
                return;
            }

            string targetPath = string.Concat(targetFolderPath,"/", renameFileName);
            // 创建目标目录（若不存在）
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath));

            // 复制文件（覆盖模式）
            File.Copy(sourceFilePath, targetPath, true);
            Debug.Log($"已复制到: {targetPath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"复制失败: {e.Message}");
        }
    }

    private void BuildAndRun()
    {
        // 1. 设置构建路径和平台
        string buildPath = Path.Combine(exeFolder, "MyGame.exe");
        BuildTarget target = BuildTarget.StandaloneWindows64;

        // 2. 获取需构建的场景
        string[] scenes = GetEnabledScenePaths();

        // 3. 执行构建并运行
        BuildPipeline.BuildPlayer(
            scenes,
            buildPath,
            target,
            BuildOptions.AutoRunPlayer // 关键：自动运行
        );
    }

    // 获取所有启用的场景路径
    private string[] GetEnabledScenePaths()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }

}
