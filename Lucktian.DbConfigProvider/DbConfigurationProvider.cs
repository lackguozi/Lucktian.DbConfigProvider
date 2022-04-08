using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lucktian.DbConfigProvider
{
    public class DbConfigurationProvider : ConfigurationProvider,IDisposable
    {
        private DbConfigOptions option;
        private bool isDisposed = false;
        private ReaderWriterLockSlim lockobj = new ReaderWriterLockSlim();
        public DbConfigurationProvider(DbConfigOptions options)
        {
            this.option = options;
            var interval = TimeSpan.FromSeconds(10);
            if(option.ReloadInterval != null)
            {
                interval = option.ReloadInterval.Value;
            }
            if (option.ReloadOnChange)
            {
                ThreadPool.QueueUserWorkItem(a =>
                {
                    while (!isDisposed)
                    {
                        Load();
                        Thread.Sleep(interval);
                    }
                });
            }
        }

        public void Dispose()
        {
            this.isDisposed = true;
        }

        public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            lockobj.EnterReadLock();
            try
            {
                return base.GetChildKeys(earlierKeys, parentPath);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                lockobj.ExitReadLock();
            }
           
        }

       

        public override void Load()
        {
            base.Load();//加载数据
            IDictionary<string, string> dicdata = null;
            try
            {
                lockobj.EnterReadLock();
                dicdata = Data.CloneData();
                //数据复制一份
                //dicdata=Data.Me
                string tableName = option.TableName;
                Data.Clear();
                using(var conn = option.CreateDbConnection())
                {
                    conn.Open();
                    //数据库加载数据
                    DoLoad(tableName, conn);
                }
            }
            catch (Exception)
            {
                //如果异常，数据还是原来的
                this.Data = dicdata;
                throw;
            }
            finally
            {
                lockobj.ExitReadLock();
            }
        }

        public override void Set(string key, string value)
        {
            throw new NotImplementedException();
        }

        public override bool TryGet(string key, out string value)
        {
            lockobj.EnterReadLock();
            try
            {
                return base.TryGet(key, out value);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                lockobj.ExitReadLock();
            }
        }
        private void DoLoad(string tableName,IDbConnection conn)
        {
            using(var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"select  Name,value from {tableName} where id in (select max(id) from {tableName} group by name)";
                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string value = reader.GetString(1);
                        if (value == null)
                        {
                            this.Data[name] = value;
                            continue;
                        }
                        value = value.Trim();
                        if (value.StartsWith("[") && value.EndsWith("]") || value.StartsWith("{") && value.EndsWith("}"))
                        {
                            TryLoadAsJson(name, value);
                        }
                        else
                        {
                            this.Data[name] = value;
                        }
                    }
                }
            }
        }
        private void TryLoadAsJson(string name, string value)
        {
            var jsonOptions = new JsonDocumentOptions { AllowTrailingCommas = true ,CommentHandling=JsonCommentHandling.Skip};
            try
            {
                var jsonRoot = JsonDocument.Parse(value, jsonOptions).RootElement;
                LoadJsonElement(name, jsonRoot);
            }
            catch (JsonException ex)
            {
                this.Data[name] = value;
                Console.WriteLine($"json 格式化错误 name {name},异常{ex.Message}");
            }
        }
        private void LoadJsonElement(string name,JsonElement jsonRoot)
        {
            if (jsonRoot.ValueKind == JsonValueKind.Array)
            {
                int index = 0;
                foreach(var item in jsonRoot.EnumerateArray())
                {
                    string path = name + ConfigurationPath.KeyDelimiter + index;
                    LoadJsonElement(path, item);
                    index++;
;                }
            }else if(jsonRoot.ValueKind == JsonValueKind.Object)
            {
                foreach (var item in jsonRoot.EnumerateObject())
                {
                    string path = name + ConfigurationPath.KeyDelimiter +item.Name;
                    LoadJsonElement(path, item.Value);
                    
                    
                }
            }
            else
            {
                this.Data[name] = jsonRoot.GetValueForConfig();
            }
        }
    }
}
