using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using Timer = System.Timers.Timer;

namespace SimListView
{
    internal class SimListViewItem : ListViewItem
    {
        public enum Rotation
        {
            RESTART,
            REVERSE
        }
        #region Private Objects
        private bool _testMode;
        private Timer tick = new Timer()
        {
            Interval = 1000,
            Enabled = true
        };
        #endregion
        #region Public Events
        public event EventHandler<ItemData>? ItemChanged;
        #endregion
        #region public Properties
        public string Key
        {
            get
            {
                string? valueText = SubItems["variable"]?.Text;
                string? indexInt = SubItems["index"]?.Text;
                return $"{valueText}[{indexInt}]" ?? string.Empty; // Return empty string if Key subitem is not found    
            }
        }
        public bool TestMode
        {
            get => _testMode;
            set
            {
                if (value)
                {
                    tick.Start();
                    _testMode = true;
                }
                else
                {
                    tick.Stop();
                    _testMode = false;
                }
            }
        }
        private int Increment
        {
            set
            {
                try
                {
                    if (SubItems.ContainsKey("Real.Increment") && SubItems["Real.Increment"] != null)
                    {
                        SubItems["Real.Increment"]!.Text = value.ToString(); // Use null-forgiving operator to suppress CS8602
                    }
                    else
                    {
                        Debug.WriteLine("SubItem 'Real.Increment' is null or does not exist.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error setting Increment: {ex.Message}");
                }
            }

            get
            {
                try
                {
                    if (SubItems.ContainsKey("Real.Increment") && SubItems["Real.Increment"] != null)
                    {
                        return int.Parse(SubItems["Real.Increment"]!.Text); // Use null-forgiving operator to suppress CS8602
                    }
                    else
                    {
                        Debug.WriteLine("SubItem 'Real.Increment' is null or does not exist.");
                        return 0; // Default value if parsing fails
                    }
                }
                catch (Exception)
                {
                    return 0; // Default value if parsing fails
                }
            }
        }
        private int Value
        {
            set
            {
                if (SubItems.ContainsKey("Value") && SubItems["Value"] != null)
                {
                    OnItemChanged(new ItemData { key = this.Key , value = value.ToString() });
                    SubItems["Value"]!.Text = value.ToString(); // Use null-forgiving operator to suppress CS8602
                }
                else
                {
                    Debug.WriteLine("SubItem 'Value' is null or does not exist.");
                }
            }
            get
            {

                if ( !SubItems.ContainsKey("Value") || SubItems["Value"] == null )
                {
                    return 0; // Default value if parsing fails
                }

                string? valueText = SubItems["Value"]?.Text;
                

                if (string.IsNullOrEmpty(valueText))
                {
                    return 0; // Default value if parsing fails
                }
                
                if ( ! int.TryParse( valueText, out int i) ) 
                {
                    
                    return 0; // Default value if parsing fails
                }
           
                return i; 
            }
        }
        private int Max
        {
            get
            {
                try
                {
                    if (SubItems.ContainsKey("Real.Max") && SubItems["Real.Max"] != null)
                    {
                        return int.Parse(SubItems["Real.Max"]!.Text); // Use null-forgiving operator to suppress CS8602
                    }
                    else
                    {
                        Debug.WriteLine("SubItem 'Real.Max' is null or does not exist.");
                        return 0; // Default value if parsing fails
                    }
                }
                catch (FormatException)
                {
                    return 0; // Default value if parsing fails
                }
            }
        }
        private int Min
        {
            get
            {
                try
                {
                    if (SubItems.ContainsKey("Real.Min") && SubItems["Real.Min"] != null)
                    {
                        return int.Parse(SubItems["Real.Min"]!.Text); // Use null-forgiving operator to suppress CS8602
                    }
                    else
                    {
                        Debug.WriteLine("SubItem 'Real.Min' is null or does not exist.");
                        return 0; // Default value if parsing fails
                    }
                }
                catch (FormatException)
                {
                    return 0; // Default value if parsing fails
                }
            }
        }
        private ILogger? logger = null;
        private Rotation RotationType
        {
            get
            {
                try
                {
                    if (SubItems.ContainsKey("Real.Rotation") && SubItems["Real.Rotation"] != null)
                    {
                        return (Rotation)Enum.Parse(typeof(Rotation), SubItems["Real.Rotation"]!.Text, true); // Use null-forgiving operator to suppress CS8602
                    }
                    else
                    {
                        Debug.WriteLine("SubItem 'Real.Rotation' is null or does not exist.");
                        return Rotation.RESTART; // Default value if parsing fails
                    }
                }
                catch (ArgumentException)
                {
                    return Rotation.RESTART; // Default value if parsing fails
                }
            }
        }
        #endregion
        #region Constructors
        public SimListViewItem(string text, SimListView container , ILogger logger) : base(text)
        {
            logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");

            TestMode = false; // Default to not in test mode
            tick.Elapsed += onTimerTick; // Correctly attach the event handler for the Timer's Elapsed event
            for (int i = 1; i < container.Columns.Count; i++)
            {
                ListViewSubItem subItem = new(this, "");
                logger.LogDebug($"Creating SubItem for column {i}: {container.Columns[i].Text}");
                subItem.Name = container.Columns[i].Text; // Assuming you want to use the column text as the subitem name
                this.SubItems.Add(subItem);
            }
        }
        #endregion
        #region Public Methods
        public void Set(string key, string value)
        {
            if (SubItems.ContainsKey(key)) // Check if the key exists in SubItems
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                ListViewSubItem si = SubItems[key];
                if (si.Text != value) si.Text = value ?? "";
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.  
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            else
            {
                throw new ArgumentException($"Key '{key}' does not exist in SubItems.", nameof(key));
            }
        }
        #endregion 
        #region Private Methods
        private int incrementValue( int value , int min , int max , int increment , Rotation rotation = Rotation.RESTART)
        {
            value += Increment;

            if (value >= max)
                if (rotation == Rotation.RESTART)
                {
                    value = min;
                }
                else if (rotation == Rotation.REVERSE)
                {
                    value = max;
                    Increment = -Increment; // Reverse the increment direction
                }

            if (value <= min)
                if (rotation == Rotation.RESTART)
                {
                    value = min;
                }
                else if (rotation == Rotation.REVERSE)
                {
                    value = min;
                    Increment = -Increment; // Reverse the increment direction
                }

            return value;
        }
        private void onTimerTick(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (!TestMode)
            {
                return;
            }

            if (this.ListView != null && this.ListView.InvokeRequired)
            {
                try
                {   // If InvokeRequired, use Invoke to update the ListView from the UI thread
                    this.ListView.Invoke(new Action(() =>
                    {
                           Value = incrementValue(Value, Min, Max, Increment, RotationType); 

                    }));
                }
                catch (Exception ex)
                {
                    logger?.LogError($"Error in Invoke  onTimerTick: {ex.Message}");
                }

            }
            else
            {
                try
                {
                    Value = incrementValue(Value, Min, Max, Increment, RotationType);
                }
                catch (Exception ex)
                {
                    logger?.LogError($"Error in onTimerTick: {ex.Message}");
                }
            }
        }
        protected virtual void OnItemChanged(ItemData e)
        {
            try
            {
                ItemChanged?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error invoking Item Changed event.");
            }
        }
        #endregion
    }
}
