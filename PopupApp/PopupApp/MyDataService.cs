using System;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Media;

namespace PopupApp
{
    public class MyDataService
    {
        private List<MyData> myData = new List<MyData> { 
            new MyData() { Name ="Icebreaker", IsSelected = true }, 
            new MyData() { Name ="Organize your Speech", IsSelected = true },   
            new MyData() { Name ="Get to the Point", IsSelected = true }, 
            new MyData() { Name ="How to Say It", IsSelected = true },   
            new MyData() { Name ="Your Body Speaks", IsSelected = false },
            new MyData() { Name ="Vocal Variety", IsSelected = false },
            new MyData() { Name ="Get to the Point", IsSelected = false }, 
            new MyData() { Name ="Research Your Topic", IsSelected = true }, 
            new MyData() { Name ="Get Comfortable with Visual Aids", IsSelected = false }, 
            new MyData() { Name ="Persuade with Power", IsSelected = false }, 
            new MyData() { Name ="Inspire your Audience", IsSelected = false }, 
        };

        public List<MyData> Data
        {
            get
            {
                return myData;
            }
        }

        private List<Tuple<string, Brush>> brushList;
        public List<Tuple<string, Brush>> BrushList
        {
            get
            {
                if (brushList == null)
                {
                    brushList = new List<Tuple<string, Brush>>();
                    Type brushesType = typeof(Brushes);
                    var properties = brushesType.GetProperties(BindingFlags.Static | BindingFlags.Public);
                    foreach (var prop in properties)
                    {
                        string name = prop.Name;
                        Brush br = (Brush)prop.GetValue(null, null);
                        Tuple<string, Brush> tuple = new Tuple<string, Brush>(name, br);
                        brushList.Add(tuple);
                    }
                }
                return brushList;
            }
        }
    }
}
