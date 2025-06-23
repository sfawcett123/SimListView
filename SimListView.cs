using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimListView
{
    public partial class SimListView : ListView
    {
        private bool _TestMode = false;

        [DefaultValue(false)]
        public bool TestMode
        {
            get { return _TestMode; }
            set
            {
                _TestMode = value; // Ensure the backing field is updated
                foreach (SimListViewItem item in this.Items)
                {
                    if (item is SimListViewItem simItem)
                    {
                        item.TestMode = value;
                    }
                }
            }
        }

        public void load(string filePath)
        {
           Debug.WriteLine( $"Loading file {filePath}" );
           Yaml y = new Yaml( filePath);
           if (y.Data == null)
                {
                 Debug.WriteLine("No _Data found in the YAML file.");
                 return;
            }
            if (y.Data.measures != null)
            {
                CreateColumns();
                CreateRows(y.Data);
                foreach (ColumnHeader column in this.Columns)
                {
                    AutoResizeColumn(column.Index, ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }

        }
    }
}
