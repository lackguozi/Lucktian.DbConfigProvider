using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucktian.DbConfigProvider
{
    public  static class DbConfigurationProviderExtensions
    {
        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder,DbConfigOptions options)
        {
            return builder.Add(new DbConfigurationSourcecs(options));
        }
        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder, Func<IDbConnection> creatDbConnection, string tableName ="T_Configs",bool reloadOnChange=false,TimeSpan ? reloadInterval = null)
        {
            return builder.AddDbConfiguration(new DbConfigOptions { CreateDbConnection = creatDbConnection, TableName = tableName, ReloadOnChange = reloadOnChange, ReloadInterval = reloadInterval });
        }
    }
}
