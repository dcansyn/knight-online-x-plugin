using KO.Core.Constants;
using System;
using System.IO;
using System.Text;

namespace KO.Core.Helpers.Storage
{
    public class StorageHelper
    {
        public static bool ExistsData()
        {
            return Exists(Path.Combine(Environment.CurrentDirectory, "Data", "local.db"));
        }

        public static string GetDataConnectionString()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Data", "local.db");

            var fileInfo = new FileInfo(path);
            if (!Directory.Exists(fileInfo.DirectoryName))
                Directory.CreateDirectory(fileInfo.DirectoryName);

            return $"Data Source={path};Foreign Keys=true;";
        }

        public static void Write(string path, string data)
        {
            var fileInfo = new FileInfo(path);

            if (!Directory.Exists(fileInfo.DirectoryName))
                Directory.CreateDirectory(fileInfo.DirectoryName);

            using (var st = new StreamWriter(path))
                st.Write(data);
        }

        public static string Read(string path)
        {
            if (!File.Exists(path))
                return null;

            return File.ReadAllText(path);
        }

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static string ReadIni(string section, string key, string path)
        {
            var RetVal = new StringBuilder(255);
            WinApi.GetPrivateProfileString(section, key, "", RetVal, 255, path);
            return RetVal.ToString();
        }

        public static void WriteIni(string section, string key, string value, string path)
        {
            WinApi.WritePrivateProfileString(section, key, value, path);
        }
    }
}
