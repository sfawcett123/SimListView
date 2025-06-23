using System.Diagnostics;
using System.Reflection;
using YamlDotNet.Serialization;

namespace SimListView
{
    enum Rotataion
    {
        RESTART,
        RETURN
        
    }
    internal class Yaml
    {
        public DataDefinition? Data { get { return _Data; } }

        private readonly DataDefinition? _Data;

        public class DataDefinition
        {
            public class Settings
            {
                public int Max { get; set; } = 0;
                public int Min { get; set; } = 10000;
                public string Unit { get; set; } = "";
                public Rotataion Rotation { get; set; } = Rotataion.RESTART;
                public int Increment { get; set; } = 10;
            }

            public class Details
            {
                public string? Id { get; set; }
                public string? Name { get; set; }
                public string? Variable { get; set; }
                public int? Index { get; set; }
                public Settings? Real { get; set; }
                public Settings? Display { get; set; }
            }

            public List<Details>? measures { get; set; }
        }

        public Yaml(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified YAML file does not exist.", filePath);
            }
            // Load the YAML file
            string yaml = File.ReadAllText(filePath);
            Debug.WriteLine($"==> YAML file loaded successfully");
            _Data = Deserializer(yaml);
        }

        public DataDefinition Deserializer(string Data)
        {
            var deserializer = new DeserializerBuilder()
                .WithCaseInsensitivePropertyMatching()
                .Build();

            return deserializer.Deserialize<DataDefinition>(Data.ToString());
        }
    }

    // Fix for CS1106: Move the extension method to a non-generic static class
    internal static class YamlExtensions
    {
        public static string GetPropertyValue(this object setting, string propertyName, string detailsName = "")
        {
            if (setting is null)
            {
                throw new NullReferenceException("Setting cannot be null");
            }

            PropertyInfo parent , child;
            var detailsInstance = new Yaml.DataDefinition.Details();
            var settingInstance = new Yaml.DataDefinition.Settings();

            try
            {
                parent = detailsInstance.GetType().GetProperties().Single(pi => pi.Name == propertyName);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine($"Property '{propertyName}' not found in Details class: {ex.Message}");
                return "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving property '{propertyName}': {ex.Message}");
                return "";
            }

            var rval = "";

            if (!string.IsNullOrEmpty(detailsName))
            {
                child = settingInstance.GetType().GetProperties().Single(pi => pi.Name == detailsName);
                var p = parent.GetValue(setting);
                rval = child.GetValue(p)?.ToString() ?? "";
               
            }
            else
            {
                rval = parent.GetValue(setting)?.ToString() ?? "";
            }
            return rval;
        }
    }
}
