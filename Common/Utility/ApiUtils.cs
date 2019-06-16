using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility
{
    public class ApiUtils
    {
        /// <summary>
        /// Media Type Formatter Json
        /// </summary>
        public static MediaTypeFormatter MediaTypeFormatterJson = GetJsonFormatter();

        /// <summary>
        /// Get App Setting
        /// </summary>
        /// <param name="appSettingKey">AppSetting Key</param>
        /// <returns></returns>
        public static string GetAppSetting(string appSettingKey)
        {
            return ConfigurationManager.AppSettings[appSettingKey];
        }
        /// <summary>
        /// Get Json Formatter
        /// </summary>
        /// <returns></returns>
        private static MediaTypeFormatter GetJsonFormatter()
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return jsonFormatter;
        }

        /// <summary>
        /// List To DataTable
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(IList<T> data)
        {
            if (data == null)
            {
                return null;
            }

            DataTable table = new DataTable();

            //special handling for value types and string
            if (typeof(T).IsValueType || typeof(T).Equals(typeof(string)))
            {

                DataColumn dc = new DataColumn("Value");
                table.Columns.Add(dc);
                foreach (T item in data)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = item;
                    table.Rows.Add(dr);
                }
            }
            else
            {
                var properties = TypeDescriptor.GetProperties(typeof(T));
                foreach (PropertyDescriptor prop in properties)
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                    {
                        try
                        {
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        }
                        catch
                        {
                            row[prop.Name] = DBNull.Value;
                        }
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }
        /// <
    }
}
