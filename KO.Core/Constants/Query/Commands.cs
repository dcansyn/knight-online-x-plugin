namespace KO.Core.Constants.Query
{
    public class Commands
    {
        public const string Insert = "INSERT INTO {0} ({1}) VALUES ({2})";
        public const string Update = "UPDATE {0} SET {1} WHERE Id=@Id";
        public const string Where = "SELECT * FROM {0} WHERE ({1})";
        public const string Count = "SELECT COUNT(0) FROM {0} WHERE ({1})";
    }
}
