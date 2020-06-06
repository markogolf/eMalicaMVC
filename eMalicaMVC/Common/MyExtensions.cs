using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace eMalicaMVC.Common
{
    public static class MyExtensions
    {
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.ToString() };
            return new SelectList(values, "Id", "Name", enumObj);
        }

        public static void TrimAllStrings<TSelf>(this TSelf obj)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

            foreach (PropertyInfo p in obj.GetType().GetProperties(flags))
            {
                Type currentNodeType = p.PropertyType;
                if (currentNodeType == typeof(String))
                {
                    string currentValue = (string)p.GetValue(obj, null);
                    if (currentValue != null)
                    {
                        p.SetValue(obj, currentValue.Trim(), null);
                    }
                }
                // see http://stackoverflow.com/questions/4444908/detecting-native-objects-with-reflection
                //else if (currentNodeType != typeof(object) && Type.GetTypeCode(currentNodeType) == TypeCode.Object)
                //{
                //    p.GetValue(obj, null).TrimAllStrings();
                //}
            }
        }
    }
}