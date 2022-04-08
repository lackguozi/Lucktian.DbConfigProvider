using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucktian.DbConfigProvider
{
    public class DbConfigOptions
    {
        public Func<IDbConnection> CreateDbConnection { get; set; }
        public string TableName { get; set; } = "T_Configs";
        public bool  ReloadOnChange { get; set; }
        public TimeSpan? ReloadInterval { get; set; }
    }
}
