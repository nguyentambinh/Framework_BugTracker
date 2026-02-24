using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BugTracker.Common.Helpers
{
    public static class ExcelExportHelper
    {
        public static byte[] ExportToExcelXml<T>(IEnumerable<T> data, string sheetName = "Sheet1")
        {
            var sb = new StringBuilder();

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
            sb.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sb.AppendLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            sb.AppendLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            sb.AppendLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");

            sb.AppendLine("  <Styles>");
            sb.AppendLine("    <Style ss:ID=\"HeaderStyle\">");
            sb.AppendLine("      <Font ss:Bold=\"1\" ss:Color=\"#FFFFFF\" />");
            sb.AppendLine("      <Interior ss:Color=\"#f59e0b\" ss:Pattern=\"Solid\" />");
            sb.AppendLine("      <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\" />");
            sb.AppendLine("    </Style>");
            sb.AppendLine("    <Style ss:ID=\"NormalStyle\">");
            sb.AppendLine("      <Borders>");
            sb.AppendLine("        <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#D3D3D3\"/>");
            sb.AppendLine("      </Borders>");
            sb.AppendLine("    </Style>");
            sb.AppendLine("  </Styles>");

            sb.AppendLine($"  <Worksheet ss:Name=\"{sheetName}\">");
            sb.AppendLine("    <Table>");

            if (data != null && data.Any())
            {
                PropertyInfo[] properties = typeof(T).GetProperties();

                sb.AppendLine("      <Row ss:Height=\"20\">");
                foreach (var prop in properties)
                {
                    sb.AppendLine($"        <Cell ss:StyleID=\"HeaderStyle\"><Data ss:Type=\"String\">{EscapeXml(prop.Name)}</Data></Cell>");
                }
                sb.AppendLine("      </Row>");

                foreach (var item in data)
                {
                    sb.AppendLine("      <Row>");
                    foreach (var prop in properties)
                    {
                        var value = prop.GetValue(item, null);
                        string valStr = value != null ? value.ToString() : "";

                        sb.AppendLine($"        <Cell ss:StyleID=\"NormalStyle\"><Data ss:Type=\"String\">{EscapeXml(valStr)}</Data></Cell>");
                    }
                    sb.AppendLine("      </Row>");
                }
            }

            sb.AppendLine("    </Table>");
            sb.AppendLine("  </Worksheet>");
            sb.AppendLine("</Workbook>");

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private static string EscapeXml(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }
    }
}
