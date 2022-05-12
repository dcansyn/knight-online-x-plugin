using KO.Core.Helpers.Message;
using KO.Domain.Accounts;
using Microsoft.VisualBasic;
using Microsoft.Win32;

namespace KO.UI.Helpers
{
    public class AccountHelper
    {
        public static Account GetAccountInformation()
        {
            var path = GameHelper.GetGameFilePath();

            if (string.IsNullOrEmpty(path))
                return null;

            Registry.CurrentUser.SetValue("CachePath", path);

            var gameName = Interaction.InputBox("Game Name", $"Please enter the game name.", "KO");
            var userName = Interaction.InputBox("User Name", $"Please enter the account user name.", "");
            var userPassword = Interaction.InputBox("User Password", $"Please enter the account password.", "");
            var accountDetail = Interaction.InputBox("Detail", $"Please enter account detail.", "EMPTY");

            if (string.IsNullOrEmpty(gameName) || string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userPassword))
            {
                MessageHelper.Send("Character information is missing.");
                return null;
            }

            return new Account(gameName, path, userName.ToUpper(), userPassword, accountDetail);
        }
    }
}
