using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace SimListView
{
    partial class SimListView
    {
        /// <summary>  
        /// Required designer variable.  
        /// </summary>  
        private System.ComponentModel.IContainer components = null;
        /// <summary>  
        /// Clean up any resources being used.  
        /// </summary>  
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>  
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Component Designer generated code  

        /// <summary>  
        /// Required method for Designer support - do not modify  
        /// the contents of this method with the code editor.  
        /// </summary>  
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.Alignment = ListViewAlignment.Default;
            this.View = View.Details;
            this.FullRowSelect = true;
            this.GridLines = true;
            this.Location = new Point(10, 9);
            this.Margin = new Padding(3, 2, 3, 2);
            this.Name = "SimulatorData";
            this.Size = new Size(500, 400);
            this.TabIndex = 0;
            this.UseCompatibleStateImageBehavior = false;
        }
        // <summary>
        /// Creates columns based on the properties of the specified type.
        // </summary>
        private void CreateColumns()
        {
            PropertyInfo[] properties = typeof(Yaml.DataDefinition.Details).GetProperties().ToArray();

            foreach (var property in properties)
            {
                if (property.PropertyType.IsClass && !property.PropertyType.FullName.StartsWith("System."))
                {
                    PropertyInfo[] p2 = property.PropertyType.GetProperties().ToArray();
                    foreach (var subProperty in p2)
                    {
                        if (!this.Columns.Cast<ColumnHeader>().Any(item => item.Text == $"{property.Name}.{subProperty.Name}"))
                        {
                            ColumnHeader item = new ColumnHeader
                            {
                                Text = $"{property.Name}.{subProperty.Name}",
                                Tag = subProperty
                            };
                            this.Columns.Add(item);
                        }
                    }
                }
                else
                {
                    if (!this.Columns.Cast<ColumnHeader>().Any(item => item.Text == property.Name))
                    {
                        ColumnHeader item = new ColumnHeader
                        {
                            Text = property.Name,
                            Tag = property
                        };
                        this.Columns.Add(item);
                    }
                }
            }

            if (!this.Columns.Cast<ColumnHeader>().Any(item => item.Text == "Value"))
            {
                ColumnHeader vitem = new ColumnHeader
                {
                    Text = "Value",
                    Tag = "Value"
                };
                this.Columns.Add(vitem);
            }
        }

        private void CreateRows(Yaml.DataDefinition data)
        {
            PropertyInfo[] properties = typeof(Yaml.DataDefinition.Details).GetProperties().ToArray();

            foreach ( Yaml.DataDefinition.Details measure in data.measures)
            {
               SimListViewItem listViewItem = new SimListViewItem( measure.Id , this);

               foreach (var property in properties)
                {
                    if (property.Name == "Id")
                    {
                        listViewItem.Text = (string)YamlExtensions.GetPropertyValue(measure, property.Name);
                        continue;
                    }
                    if (property.PropertyType.IsClass && !property.PropertyType.FullName.StartsWith("System."))
                    {
                        PropertyInfo[] p2 = property.PropertyType.GetProperties().ToArray();
                        foreach (var subProperty in p2)
                        {
                           var s = $"{property.Name}.{subProperty.Name}";
                            // Get the value of the sub-property
                            listViewItem.Set(s, YamlExtensions.GetPropertyValue ( measure , property.Name , subProperty.Name ) ); 
                        }
                    }
                    else
                    {
                        listViewItem.Set( property.Name, YamlExtensions.GetPropertyValue(measure , property.Name));
                    }
                }

                listViewItem.Set("Value", "");
                Items.Add(listViewItem);
            }
        }
        #endregion
        public List<string> to_string()
        {
            if (this.InvokeRequired)
            {
                return (List<string>)this.Invoke(new Func<List<string>>(to_string));
            }

            List<string> list = new List<string>();
            foreach (ListViewItem item in this.Items)
            {
                list.Add(item.Text);
            }

            return list;
        }
    }
}
