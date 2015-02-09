using System;

namespace DataTemplateSelectorDemo
{
    public class DataItem
    {
        public int ID { get; set; }

        private dynamic _value;

        public dynamic Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Description { get; set; }

        public string[] DayOfWeekArray { get { return Enum.GetNames(typeof(DayOfWeek)); } }
    }
}
