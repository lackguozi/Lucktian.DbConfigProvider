using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucktian.DbConfigProvider
{
    public static class LuckDictionaryHelper
    {
        public static IDictionary<string, string> CloneData(this IDictionary<string, string> dic)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            foreach(var item in dic)
            {
                result[item.Key] = item.Value;
            }
            return result;
        }
        public static bool IsChanged(IDictionary<string, string> a, IDictionary<string, string> b)
        {
            if(a.Count != b.Count)
            {
                return true;
            }
            foreach(var item in a)
            {
                var oldKey =item.Key;
                var oldValue =item.Value;
                var newValue = b[oldKey];
                {
                    if(oldValue != newValue)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
