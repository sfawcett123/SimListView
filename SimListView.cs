using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

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

        private SimListViewItem? Find(string measure, int index)
        {
            if (index < 1)
            {
                logger?.LogWarning($"Index {index} is less than 1. Cannot find measure '{measure}'.");
                return  null;
            }
            foreach (SimListViewItem item in this.Items)
            {
                if (item.Contains(measure, index))
                {
                    logger?.LogInformation($"Found Item: {item.Name}, Index: {item.Index}, Text: {item.Value}");
                    return item;
                }
            }
            logger?.LogWarning($"Item with measure '{measure}' and index '{index}' not found.");
            return null;
        }
        
        public int getValue(string measure, int index)
        {
            SimListViewItem? item = Find(measure, index);
            if (item != null)
            {
                logger?.LogInformation($"Item: {item.Name}, Index: {item.Index}, Text: {item.Value}");
                return item.Value;
            }

            return 0;
        }

        public void load(string filePath)
        {
            logger?.LogDebug($"Loading file {filePath}");
            Yaml y = new Yaml(filePath);
            if (y.Data == null)
            {
                logger?.LogError("No _Data found in the YAML file.");
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

        public bool setValue(string measure, int index, bool value)
        {
            return setValue(measure, index, value.ToString());
        }

        public bool setValue(string measure, bool value)
        {
            return setValue(measure, 1, value.ToString());
        }

        public bool setValue(string measure, int index , int value)
        {
            return setValue(measure, index, value.ToString());
        }

        public bool setValue(string measure, int value)
        {
            return setValue(measure, 1, value.ToString() );
        }

        public bool setValue(string measure, string value)
        {
            return setValue(measure, 1, value);
        }
        public bool setValue(string measure, int index, string value)
        {
            SimListViewItem? item = Find(measure, index);
            if (item != null)
            {
                item.Value = int.Parse( value);
                logger?.LogInformation($"Item: {item.Name}, Index: {item.Index}, Text: {item.Value}");
                return true;
            }

            logger?.LogWarning($"Item with measure '{measure}' and index '{index}' not found.");
            return false;

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
