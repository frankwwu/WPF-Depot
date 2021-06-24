using System;
using System.Xml.Linq;

namespace DataGridColumnBinding
{
    public static class XElementExtensions
    {
        public static string GetString(this XElement source, String name)
        {
            if (source != null)
            {
                XAttribute attr = source.Attribute(name);
                return (attr != null) ? attr.Value : string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool GetBool(this XElement source, String name)
        {
            if (source != null)
            {
                XAttribute attr = source.Attribute(name);
                return (attr != null) && attr.Value.Equals("1");
            }
            else
            {
                return false;
            }
        }

        public static Byte GetByte(this XElement source, String name)
        {
            if (source != null)
            {
                XAttribute attr = source.Attribute(name);
                return (attr != null) ? Convert.ToByte(attr.Value) : (Byte)0;
            }
            else
            {
                return (Byte)0;
            }
        }

        public static int GetInt(this XElement source, String name)
        {
            if (source != null)
            {
                XAttribute attr = source.Attribute(name);
                return (attr != null) ? Convert.ToInt32(attr.Value) : 0;
            }
            else
            {
                return 0;
            }
        }

        public static int? GetNullableInt(this XElement source, String name)
        {
            if (source != null)
            {
                XAttribute attr = source.Attribute(name);
                return (attr != null) ? Convert.ToInt32(attr.Value) : (int?)null;
            }
            else
            {
                return null;
            }
        }

        public static long GetLong(this XElement source, String name)
        {
            if (source != null)
            {
                XAttribute attr = source.Attribute(name);
                return (attr != null) ? Convert.ToInt64(attr.Value) : 0;
            }
            else
            {
                return 0;
            }
        }

        public static long? GetNullableLong(this XElement source, String name)
        {
            if (source != null)
            {
                XAttribute attr = source.Attribute(name);
                return (attr != null) ? Convert.ToInt64(attr.Value) : (long?)null;
            }
            else
            {
                return null;
            }
        }

        public static float GetFloat(this XElement source, String name)
        {
            if (source != null)
            {
                XAttribute attr = source.Attribute(name);
                return (attr != null) ? Convert.ToSingle(attr.Value) : 0;
            }
            else
            {
                return 0;
            }
        }

        public static decimal GetDecimal(this XElement source, String name)
        {
            if (source != null)
            {
                XAttribute attr = source.Attribute(name);
                return (attr != null) ? Convert.ToDecimal(attr.Value) : 0;
            }
            else
            {
                return 0;
            }
        }

        public static double GetDouble(this XElement source, String name)
        {
            if (source != null)
            {
                XAttribute attr = source.Attribute(name);
                return (attr != null) ? Convert.ToDouble(attr.Value) : 0;
            }
            else
            {
                return 0;
            }
        }
    }
}
