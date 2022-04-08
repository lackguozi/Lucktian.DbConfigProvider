using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lucktian.DbConfigProvider
{
    public  static class JsonElementExtensions
    {
        public static string GetValueForConfig(this JsonElement e)
        {
            if(e.ValueKind == JsonValueKind.String)
            {
                return e.ToString();
            }
            else if(e.ValueKind == JsonValueKind.Null || e.ValueKind == JsonValueKind.Undefined)
            {
                return null;
            }
            else
            {
                return e.GetRawText();
            }
        }
    }
}
