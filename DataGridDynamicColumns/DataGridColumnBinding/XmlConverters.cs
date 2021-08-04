using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;

namespace DataGridColumnBinding
{
    public class XmlConverters
    {
        #region Public Methods

        public static DataGridModel ConvertXmlToDataTable(XDocument XDoc)
        {
            DataGridModel model = new DataGridModel();
            var g = XDoc.Descendants("grid");
            if ((g == null) || (g.Count() == 0))
            {
                return null;
            }
            XElement f = g.FirstOrDefault();
            IEnumerable<XElement> eles = f.Elements();
            XElement ele = eles.FirstOrDefault();
            IEnumerable<XElement> grid_columns = ele.Elements();
            foreach (XElement col in grid_columns)
            {
                string name = col.GetString("name");
                bool editable = col.GetBool("editable");
                string data_type = col.GetString("data_type");
                int precision = 0;
                if (data_type.Contains('('))
                {
                    int i = data_type.IndexOf('(');
                    string sub = data_type.Substring(0, i);
                    i = data_type.IndexOf(',');
                    if (i > 0)
                    {
                        var p = data_type.Substring(i + 1, 1);
                        precision = Convert.ToInt32(p);
                    }
                    data_type = sub;
                }
                DataColumn dataCol = new DataColumn();
                dataCol.ColumnName = name;
                dataCol.Caption = name;
                dataCol.ReadOnly = !editable;
                dataCol.DataType = data_type switch
                {
                    "bigint" => typeof(Int64),
                    "int" => typeof(Int32),
                    "decimal" => typeof(decimal),
                    "float" => typeof(float),
                    "double" => typeof(double),
                    _ => typeof(string)
                };

                model.DataTable.Columns.Add(dataCol);

                string titleCase = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(name.Replace('_', ' '));
                if (editable)
                {
                    DataGridTemplateColumn dgt = new DataGridTemplateColumn();
                    dgt.Header = titleCase;
                    dgt.IsReadOnly = !editable;
                    DataTemplate dt = new DataTemplate();
                    dt.VisualTree = new FrameworkElementFactory(typeof(TextBlock));
                    dt.VisualTree.SetValue(TextBlock.BackgroundProperty, Brushes.LightYellow);
                    dt.VisualTree.SetValue(TextBlock.ForegroundProperty, Brushes.Black);
                    dgt.CellTemplate = dt;

                    if (dataCol.DataType == typeof(Int64) || dataCol.DataType == typeof(Int32) || dataCol.DataType == typeof(decimal) || dataCol.DataType == typeof(float) || dataCol.DataType == typeof(double))
                    {
                        if (dataCol.DataType == typeof(Int64) || dataCol.DataType == typeof(int))
                        {
                            dt.VisualTree.SetBinding(TextBlock.TextProperty, new Binding(titleCase) { StringFormat = "N0" });
                        }
                        else
                        {
                            if (precision > 0)
                            {
                                dt.VisualTree.SetBinding(TextBlock.TextProperty, new Binding(titleCase) { StringFormat = $"N{precision}" });
                            }
                            {
                                dt.VisualTree.SetBinding(TextBlock.TextProperty, new Binding(titleCase) { StringFormat = "N" });
                            }
                        }

                        DataTemplate edt = new DataTemplate();
                        edt.VisualTree = new FrameworkElementFactory(typeof(NumericUpDown));
                        edt.VisualTree.SetBinding(NumericUpDown.ValueProperty, new Binding(titleCase) { UpdateSourceTrigger = UpdateSourceTrigger.LostFocus });
                        edt.VisualTree.SetValue(NumericUpDown.HideUpDownButtonsProperty, true);
                        edt.VisualTree.SetValue(NumericUpDown.BackgroundProperty, Brushes.LightYellow);
                        edt.VisualTree.SetValue(NumericUpDown.ForegroundProperty, Brushes.Black);
                        dgt.CellEditingTemplate = edt;
                        model.ColumnCollection.Add(dgt);
                    }
                    else if (dataCol.DataType == typeof(string))
                    {
                        DataTemplate edt = new DataTemplate();
                        edt.VisualTree = new FrameworkElementFactory(typeof(TextBox));
                        edt.VisualTree.SetBinding(TextBox.TextProperty, new Binding(titleCase) { UpdateSourceTrigger = UpdateSourceTrigger.LostFocus });
                        edt.VisualTree.SetValue(TextBox.BackgroundProperty, Brushes.LightYellow);
                        edt.VisualTree.SetValue(TextBox.ForegroundProperty, Brushes.Black);
                        dgt.CellEditingTemplate = edt;
                        model.ColumnCollection.Add(dgt);
                    }
                }
                else
                {
                    DataGridTextColumn dgc = new DataGridTextColumn();
                    dgc.Header = titleCase;
                    dgc.IsReadOnly = !editable;
                    dgc.Binding = new Binding(titleCase);
                    if (dataCol.DataType == typeof(Int64) || dataCol.DataType == typeof(Int32))
                    {
                        dgc.Binding.StringFormat = "N0";
                    }
                    else if (dataCol.DataType == typeof(decimal) || dataCol.DataType == typeof(float) || dataCol.DataType == typeof(double))
                    {
                        if (precision > 0)
                        {
                            dgc.Binding.StringFormat = $"N{precision}";
                        }
                        else
                        {
                            dgc.Binding.StringFormat = "N";
                        }
                    }
                    model.ColumnCollection.Add(dgc);
                }
            }

            IEnumerable<XElement> rows = f.Descendants("grid_rows");
            if ((rows != null) && rows.Count() > 0)
            {
                var grid_data = rows.FirstOrDefault().Elements();
                foreach (XElement row in grid_data)
                {
                    DataRow dataRow = model.DataTable.NewRow();
                    foreach (XAttribute att in row.Attributes())
                    {
                        try
                        {
                            string name = att.Name.LocalName;
                            string value = att.Value;
                            if (string.IsNullOrEmpty(value))
                            {
                                continue;
                            }
                            if (model.DataTable.Columns.Contains(name))
                            {
                                dataRow[name] = model.DataTable.Columns[name].DataType.Name switch
                                {
                                    "Int64" => value.Contains('.') ? Convert.ToInt64(value.Substring(0, value.IndexOf('.'))) : Convert.ToInt64(value),
                                    "Int32" => value.Contains('.') ? Convert.ToInt32(value.Substring(0, value.IndexOf('.'))) : Convert.ToInt32(value),
                                    "decimal" => Convert.ToDecimal(value),
                                    "float" => Convert.ToSingle(value),
                                    "double" => Convert.ToDouble(value),
                                    _ => value
                                };
                            }
                            else
                            {
                                MessageBox.Show($"Can't find {name} column in sp returned XML.", "Error");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Data Conversion Exception");
                        }
                    }
                    model.DataTable.Rows.Add(dataRow);
                }
            }

            foreach (DataColumn dataCol in model.DataTable.Columns)
            {
                dataCol.ColumnName = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(dataCol.ColumnName.Replace('_', ' '));
            }

            return model;
        }

        #endregion Public Methods
    }
}

