namespace SimListView.Demo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listView = new SimListView();
            SuspendLayout();
            // 
            // listView
            // 
            listView.Alignment = ListViewAlignment.Default;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Location = new Point(29, 20);
            listView.Margin = new Padding(3, 2, 3, 2);
            listView.Name = "listView";
            listView.Size = new Size(759, 370);
            listView.TabIndex = 0;
            listView.UseCompatibleStateImageBehavior = false;
            listView.View = View.Details;
            listView.ColumnWidthChanging += listView_ColumnWidthChanging;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(listView);
            Name = "Form1";
            Text = "Demo";
            ResumeLayout(false);
        }

        void listView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            Console.Write("Column Resizing");
            e.NewWidth = this.listView.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        #endregion

        void loadControlData()
        {
            // Load data into the SimListView
            listView.load("testdata/test_1.yaml");
        }

        private SimListView listView;
    }
}
