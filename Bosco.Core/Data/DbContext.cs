using System.Configuration;

namespace Bosco.Core.Data;

public class DbContext
{
    public static string CnnStr(string connectionName) => ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
}
