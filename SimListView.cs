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
        public SimListView()
        {
            InitializeComponent();
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
            }

        }
    }
}
