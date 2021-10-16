using System;
using System.Collections.Generic;
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
                PropertyInfo[] properties = typeof(T).GetProperties();
                List<string> schema = reader.GetColumnSchema().Select((x) => x.ColumnName).ToList();
                while (reader.Read())
                {
                    return MapResult<T>(reader, properties, schema);
                }
            }

            return null;
        }

        public List<T> CreateList<T>(DbDataReader reader) where T : class
        {
            if (reader.HasRows)
            {
                List<T> list = new();
                PropertyInfo[] properties = typeof(T).GetProperties();
                List<string> schema = reader.GetColumnSchema().Select((x) => x.ColumnName).ToList();
                while (reader.Read())
                {
                    list.Add(MapResult<T>(reader, properties, schema));
                }

                return list;
            }

            return null;
        }

        private static T MapResult<T>(DbDataReader reader, PropertyInfo[] properties, List<string> schema)
        {
            T record = (T)Activator.CreateInstance(typeof(T));
            foreach (PropertyInfo property in properties)
            {
                if (schema.Contains(property.Name))
                {
                    object value = reader.GetValue(reader.GetOrdinal(property.Name));
                    if (value == DBNull.Value)
                    {
                        value = null;
                    }
                    property.SetValue(record, value);
                }
            }

            return record;
        }
    }
}
