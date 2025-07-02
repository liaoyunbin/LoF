
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
namespace EscapeGame.ArchiveModule
{
    /// <summary>
    /// 存档Json文件管理类
    /// </summary>
    public static class ArchiveJsonFileUtil
    {
        /// <summary>
        /// json序列化和反序列化设置
        /// </summary>
        private static JsonSerializerSettings setting;
        private static Dictionary<ArchiveLocation, ArchivePath> m_PathFileDict = new Dictionary<ArchiveLocation, ArchivePath>();
        static ArchiveJsonFileUtil()
        {
            m_PathFileDict.Clear();
            //忽略实体中实体，不再序列化里面包含的实体
            //设置Json.NET能够序列化接口或继承类
            setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.All };
        }
        #region private function
        private static ArchivePath GetPathFile(ArchiveLocation _index)
        {
            bool flag = m_PathFileDict.TryGetValue(_index, out ArchivePath path);
            if (!flag)
            {
                path = new ArchivePath();
                path.InitPath((int)_index);
                m_PathFileDict.Add(_index, path);
            }
            return path;
        }
        private static void SaveToJson<T>(string datapath, T data)
        {
            string content = JsonUtils.ObjectToJson(data, setting);
            FileUtil.WriteToJson(datapath, content);
        }

        private static void LoadByJson<T>(string datapath, ref T data)
        {
            string content = FileUtil.ReadByJson(datapath);
            data = JsonUtils.JsonToObject<T>(content, setting);
        }

        /// <summary>
        /// 当前路径是否有Json文件
        /// </summary>
        /// <param name="_pathType"></param>
        /// <returns></returns>
        public static bool ExistJson(ArchiveLocation _index,SlotArchivePathEnum _pathType)
        {
            ArchivePath path = GetPathFile(_index);
            switch (_pathType)
            {
                case SlotArchivePathEnum.PermanentPath:
                    return File.Exists(path.PermanentArchivePath);
                case SlotArchivePathEnum.TemporaryPath:
                    return File.Exists(path.TemporaryArchivePath);
                case SlotArchivePathEnum.GlobalPath:
                    return File.Exists(path.GlobalArchivePath);
                case SlotArchivePathEnum.NowPath:
                    return File.Exists(path.NowArchivePath);
                default:
                    Logs.LogError("暂未支持{0}类型从json存在判定", _pathType);
                    return false;
            }
        }
        /// <summary>
        /// 删除对应json文件
        /// </summary>
        /// <param name="_pathType"></param>
        /// <returns></returns>
        public static void DeleteJson(ArchiveLocation _index, SlotArchivePathEnum _pathType)
        {
            ArchivePath path = GetPathFile(_index);
            if (!ExistJson(_index, _pathType))
            {
                return;
            }
            switch (_pathType)
            {
                case SlotArchivePathEnum.PermanentPath:
                    File.Delete(path.PermanentArchivePath);
                    break;
                case SlotArchivePathEnum.TemporaryPath:
                    File.Delete(path.TemporaryArchivePath);
                    break;
                case SlotArchivePathEnum.GlobalPath:
                    File.Delete(path.GlobalArchivePath);
                    break;
                case SlotArchivePathEnum.NowPath:
                    File.Delete(path.NowArchivePath);
                    break;
                default:
                    Logs.LogError("暂未支持{0}类型 的json删除", _pathType);
                    break;
            }
        }

        /// <summary>
        /// 删除当前存档文件夹
        /// </summary>
        public static void DeleteArchiveFolder(ArchiveLocation _index)
        {
            ArchivePath path = GetPathFile(_index);
            if (Directory.Exists(path.ArchiveDirectoryPath))
            {
                Directory.Delete(path.ArchiveDirectoryPath, true);
            }
        }


        /// <summary>
        /// 是否有当前存档的文件夹
        /// </summary>
        public static bool HasArchiveFolder(ArchiveLocation _index)
        {
            ArchivePath path = GetPathFile(_index);
            return Directory.Exists(path.ArchiveDirectoryPath);
        }
        #endregion
        /// <summary>
        /// 保存数据到json
        /// </summary>
        /// <param name="_pathType"></param>
        public static void SaveToJson(this SlotArchiveData _data, ArchiveLocation _index,SlotArchivePathEnum _pathType)
        {
            ArchivePath path = GetPathFile(_index);
            switch (_pathType)
            {
                case SlotArchivePathEnum.PermanentPath:
                    SaveToJson(path.PermanentArchivePath, _data.saveData);
                    break;
                case SlotArchivePathEnum.TemporaryPath:
                    SaveToJson(path.TemporaryArchivePath, _data.saveData);
                    break;
                case SlotArchivePathEnum.GlobalPath:
                    SaveToJson(path.GlobalArchivePath, _data.globalData);
                    break;
                case SlotArchivePathEnum.NowPath:
                    SaveToJson(path.NowArchivePath, _data.saveData);
                    break;
                default:
                    Logs.LogError("暂未支持{0}类型保存到json数据", _pathType);
                    break;
            }
        }

        /// <summary>
        /// 从json中加载数据
        /// </summary>
        /// <param name="_pathType"></param>
        public static void LoadByJson(this SlotArchiveData _data, ArchiveLocation _index, SlotArchivePathEnum _pathType)
        {
            ArchivePath path = GetPathFile(_index);
            switch (_pathType)
            {
                case SlotArchivePathEnum.PermanentPath:
                    LoadByJson(path.PermanentArchivePath, ref _data.saveData);
                    break;
                case SlotArchivePathEnum.TemporaryPath:
                    LoadByJson(path.TemporaryArchivePath, ref _data.saveData);
                    break;
                case SlotArchivePathEnum.GlobalPath:
                    LoadByJson(path.GlobalArchivePath, ref _data.globalData);
                    break;
                case SlotArchivePathEnum.NowPath:
                    LoadByJson(path.NowArchivePath, ref _data.saveData);
                    break;
                default:
                    Logs.LogError("暂未支持{0}类型从json加载", _pathType);
                    break;
            }
        }

        /// <summary>
        /// 删除所有插槽的存档
        /// </summary>
        public static void DeleteAllArchiveFolder()
        {
            var list = System.Enum.GetValues(typeof(ArchiveLocation));
            foreach (ArchiveLocation aType in list)
            {
                if(aType != ArchiveLocation.Max)
                {
                    DeleteArchiveFolder(aType);
                }
            }
        }
    }
}
