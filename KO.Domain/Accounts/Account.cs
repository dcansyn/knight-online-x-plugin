using KO.Core.Extensions;

namespace KO.Domain.Accounts
{
    public class Account
    {
        public string GameName { get; protected set; }
        public string GamePath { get; protected set; }
        public string UserName { get; protected set; }
        public string UserPassword { get; protected set; }
        public string UserPasswordHash { get; protected set; }
        public string Arguments { get; protected set; }
        public string Detail { get; protected set; }

        public Account(string gameName, string gamePath, string userName, string userPassword, string detail)
        {
            GameName = gameName;
            GamePath = gamePath;
            UserName = userName;
            UserPassword = userPassword;
            Detail = detail;

            UserPasswordHash = UserPassword.Hash();
        }

        public Account(string gameName, string gamePath, string userName, string userPassword)
        {
            GameName = gameName;
            GamePath = gamePath;
            UserName = userName;
            UserPassword = userPassword;

            UserPasswordHash = UserPassword.Hash();

            Arguments = $"MGAMEJP {UserName} {UserPasswordHash}";
        }
    }
}
