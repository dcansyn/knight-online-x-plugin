using KO.Core.Constants;
using System;
using System.Windows.Forms;

namespace KO.Core.Helpers.Message
{
    public class MessageHelper
    {
        public static bool Send(string text, MessageBoxButtons button = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            return MessageBox.Show(text, Settings.Name, button, icon) == DialogResult.Yes;
        }

        public static bool Send(string text, string caption, MessageBoxButtons button = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            return MessageBox.Show(text, caption, button, icon) == DialogResult.Yes;
        }

        public static void Send(Exception ex)
        {
            var message = GetExceptionMessage(ex);
            var line = GetExceptionLine(ex);

            Send($"Unknown Error !{Environment.NewLine}Line :{line}{Environment.NewLine}Message:{message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static string GetExceptionMessage(Exception ex)
        {
            var message = ex.Message;
            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                message += $"| {innerEx.Message}";
                innerEx = innerEx.InnerException;
            }
            return message;
        }

        public static int GetExceptionLine(Exception ex)
        {
            var exLine = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' '), ex.StackTrace.Length - ex.StackTrace.LastIndexOf(' '));
            int.TryParse(exLine, out int retNumber);
            return retNumber;
        }
    }
}
