using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Masiv.Roulette.Data
{
    public class QueryHelper : IQueryHelper
    {
        public T Create<T>(DbDataReader reader) where T : class
        {
            if (reader.HasRows)
            {
                T record = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] properties = typeof(T).GetProperties();
                List<string> schema = reader.GetColumnSchema().Select((x) => x.ColumnName).ToList();
                while (reader.Read())
                {
                    foreach (PropertyInfo property in properties)
                    {
                        if (schema.Contains(property.Name))
                        {
                            property.SetValue(record, reader.GetValue(reader.GetOrdinal(property.Name)));
                        }
                    }
                }

                return record;
            }

            return null;
        }
    }
}
