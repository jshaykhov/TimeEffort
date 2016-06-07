using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace TimeEffort.Utilities
{
    public class CSVUtility
    {

        public static MemoryStream GetCSV(DataTable data)
        {
            string[] fieldsToExpose = new string[data.Columns.Count];
            for (int i = 0; i < data.Columns.Count; i ++ )
            {
                fieldsToExpose[i] = data.Columns[i].ColumnName;
            }
 
            return GetCSV(fieldsToExpose, data);
        }
 
        public static MemoryStream GetCSV(string[] fieldsToExpose, DataTable data)
        {
            MemoryStream stream = new MemoryStream();
            using (var writer = new StreamWriter(stream))
            {
                for (int i = 0; i < fieldsToExpose.Length; i++)
                {
                    if (i != 0) { writer.Write(","); }
                    writer.Write("\"");
                    writer.Write(fieldsToExpose[i].Replace("\"", "\"\""));
                    writer.Write("\"");
                }
                writer.Write("\n");
 
                foreach (DataRow row in data.Rows)
                {
                    for (int i = 0; i < fieldsToExpose.Length; i++)
                    {
                        if (i != 0) { writer.Write(","); }
                        writer.Write("\"");
                        writer.Write(row[fieldsToExpose[i]].ToString()
                            .Replace("\"", "\"\""));
                        writer.Write("\"");
                    }
 
                    writer.Write("\n");
                }
            }
 
            return stream;
        }
    }
}
