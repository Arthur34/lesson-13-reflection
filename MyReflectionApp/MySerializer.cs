using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyReflectionApp
{
    public class MySerializer
    {
        /// <summary>
        /// Разделитель полей
        /// </summary>
        private static readonly char _fieldSep = ';';

        /// <summary>
        /// Разделитель строк
        /// </summary>
        private static readonly string _lineSep = Environment.NewLine;

        /// <summary>
        /// Сериализация в csv
        /// </summary>
        /// <param name="o">Объект для сериализации</param>
        /// <returns>CSV</returns>
        public static string ObjToCsv(object o)
        {
            var objType = o.GetType();
            var fieldList = objType.GetFields();
            var serializeList = new Dictionary<string, string>();

            for (int i = 0; i < fieldList.Length; i++)
                serializeList.Add(fieldList[i].Name, value: fieldList[i].GetValue(o).ToString());

            var csvString = new StringBuilder();
            csvString.AppendJoin(_fieldSep, serializeList.Keys).Append(Environment.NewLine);
            csvString.AppendJoin(_fieldSep, serializeList.Values);

            return csvString.ToString();
        }

        /// <summary>
        /// Десериализация объекта из csv
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csv"></param>
        /// <returns></returns>
        public static object CsvToObj<T>(string csv) where T : class, new()
        {
            var objType = typeof(T);
            object obj = new T();

            var lines = csv.Split(_lineSep);
            var fields = lines[0].Split(_fieldSep);
            var values = lines[1].Split(_fieldSep);

            for (int i = 0; i < fields.Length; i++)
            {
                var field = objType.GetField(fields[i]);
                if (field == null)
                    continue;
                field.SetValue(obj, Convert.ChangeType(values[i], field.FieldType));
            }

            return obj;
        }
    }
}
