using System.Diagnostics;
using System.Windows.Forms;

namespace SimListView
{
    public enum COLUMNS
    {
        Parameter,
        Index,
        Value,
        Unit
    }
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


            this.Items.Clear();
            this.Columns.Clear();
            this.Columns.Add(COLUMNS.Parameter.ToString(), 150, HorizontalAlignment.Center);
            this.Columns.Add(COLUMNS.Index.ToString(), 50, HorizontalAlignment.Center);
            this.Columns.Add(COLUMNS.Value.ToString(), 200, HorizontalAlignment.Center);
            this.Columns.Add(COLUMNS.Unit.ToString(), 100, HorizontalAlignment.Center);

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
