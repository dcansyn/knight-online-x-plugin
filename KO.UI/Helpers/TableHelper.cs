using System.IO;

namespace KO.UI.Helpers
{
    public class TableHelper
    {
        public static string GetDataPath()
        {
            var path = GameHelper.GetGameFilePath("*");

            if (string.IsNullOrWhiteSpace(path))
                return null;

            var fileInfo = new FileInfo(path);
            var dataPath = Path.Combine(fileInfo.DirectoryName, "Data");

            if (!Directory.Exists(dataPath))
                return null;

            return dataPath;
        }
    }
}
