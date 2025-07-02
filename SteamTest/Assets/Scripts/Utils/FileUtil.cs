using System.IO;

    public class FileUtil
    {
        public static void WriteToJson(string _path, string _content)
        {
            var director = Path.GetDirectoryName(_path);
            if (!Directory.Exists(director))
            {
                Directory.CreateDirectory(director);
            }
            StreamWriter sw = new StreamWriter(_path, false); //创建一个写入流
            sw.Write(_content);//将dateStr写入
            sw.Close();//关闭流
        }

        public static string ReadByJson(string _path)
        {
            if (File.Exists(_path))  //判断这个路径里面是否为空
            {
                StreamReader sr = new StreamReader(_path, false);//创建读取流;
                string jsonStr = sr.ReadToEnd();//使用方法ReadToEnd（）遍历的到保存的内容
                sr.Close();
                return jsonStr;
            }
            UnityEngine.Debug.Log(_path +"路径上未找到文件");
            return string.Empty;
        }
    }

