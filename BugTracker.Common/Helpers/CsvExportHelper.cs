using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BugTracker.Common.Helpers
{
    public static class CsvExportHelper
    {
        public static byte[] ExportToCsv<T>(IEnumerable<T> data)
        {
            var sb = new StringBuilder();

            // Support for UTF-8
            sb.Append('\uFEFF');

            if (data == null || !data.Any())
                return Encoding.UTF8.GetBytes(sb.ToString());

            PropertyInfo[] properties = typeof(T).GetProperties();

            var headers = properties.Select(p => p.Name);
            sb.AppendLine(string.Join(",", headers));

            foreach (var item in data)
            {
                var rowValues = properties.Select(p =>
                {
                    var value = p.GetValue(item, null);
                    string valStr = value != null ? value.ToString() : "";

                    if (valStr.Contains(",") || valStr.Contains("\"") || valStr.Contains("\n"))
                    {
                        valStr = $"\"{valStr.Replace("\"", "\"\"")}\"";
                    }
                    return valStr;
                });

                sb.AppendLine(string.Join(",", rowValues));
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
