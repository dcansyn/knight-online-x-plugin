using KO.Application;
using KO.Application.Addresses.Extensions;
using Microsoft.Win32;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KO.UI.Helpers
{
    public class GameHelper
    {
        public static async Task InjectGame()
        {
            foreach (var item in Client.Characters)
            {
                item.StartGame();
                item.UpdateReceiveHookHandle(await item.GetReceiveHandle());
                if (item.IsMain)
                    item.UpdateSendHookHandle(await item.GetSendHandle());

                Thread.Sleep(100);
            }
        }

        public static string GetGameFilePath(string fileName = "KnightOnLine")
        {
            var fileDialog = new OpenFileDialog()
            {
                Filter = $"{fileName} | {fileName}.exe",
                InitialDirectory = Registry.CurrentUser.GetValue("CachePath")?.ToString() ?? @"C:\"
            };
            fileDialog.ShowDialog();

            if (fileDialog.FileName != null)
                Registry.CurrentUser.SetValue("CachePath", fileDialog.FileName);

            return fileDialog.FileName;
        }

        public static string GetGameDirectoryPath(string fileName = "KnightOnLine")
        {
            var path = GetGameFilePath(fileName);

            if (string.IsNullOrEmpty(path))
                return null;

            return new FileInfo(path).DirectoryName;
        }
    }
}
