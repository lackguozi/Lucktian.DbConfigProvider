using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucktian.DbConfigProvider
{
    public class DbConfigurationSourcecs : IConfigurationSource
    {
        private DbConfigOptions options;
        public DbConfigurationSourcecs(DbConfigOptions options)
        {
            this.options = options;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DbConfigurationProvider(options);
        }
    }
}
