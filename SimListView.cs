using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Diagnostics;

namespace SimListView
{
    public partial class SimListView : ListView
    {
        private bool _TestMode = false;
        public SimListView() : base()
        {
            InitializeComponent();
            Init();
        }
        public SimListView(ILoggerFactory loggerFactory) : base()
        {
            this.factory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory), "Logger factory cannot be null.");
            InitializeComponent();
            Init();
        }
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
        public void clear()
        {
            this.Items.Clear();
            this.Columns.Clear();
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
        private void Init()
        {
            this.logger = factory.CreateLogger("SimView");

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

    }
}
