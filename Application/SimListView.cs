﻿using Microsoft.Extensions.Logging;
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

        private SimListViewItem? Find(string measure)
        {

            foreach (SimListViewItem item in this.Items)
            {
                if (item.Contains(measure))
                {
                    logger?.LogInformation($"Found Item: {item.Name}, Text: {item.Value}");
                    return item;
                }
            }
            logger?.LogWarning($"Item with measure '{measure}' not found.");
            return null;
        }
        
        public string getValue(string measure)
        {
            SimListViewItem? item = Find(measure);
            if (item != null)
            {
                logger?.LogInformation($"Item: {item.Name}, Text: {item.Value}");
                return item.Value;
            }

            return "";
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
                    if( column.Width > 0)  // Only resize columns that are visible
                        AutoResizeColumn(column.Index, ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }

        }


        public bool setValue(string measure, bool value)
        {
            return setValue(measure, value.ToString());
        }

        public bool setValue(string measure, int value)
        {
            return setValue(measure, value.ToString());
        }

        public bool setValue(string measure, string value)
        {
            SimListViewItem? item = Find(measure);
            if (item != null)
            {
                item.Value = value;
                logger?.LogInformation($"Item: {item.Name}, Text: {item.Value}");
                return true;
            }

            logger?.LogWarning($"Item with measure '{measure}' not found.");
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
