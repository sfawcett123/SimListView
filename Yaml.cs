using Microsoft.Win32;
using System.CodeDom;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace SimListView
{
    internal class Yaml
    {
        public DataDefinition? Data { get { return data; } }

        private DataDefinition? data;

        public class DataDefinition
        {
            public class Settings
            {
                public int max { get; set; }
                public int min { get; set; }
                public string unit { get; set; } = "";
            }

            public class Details
            {
                public string? id { get; set; }
                public string? name { get; set; }
                public string? variable { get; set; }
                public int? index { get; set; }
                public Settings? real { get; set; }
                public Settings? display { get; set; }
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
            data = Deserializer(yaml);
        }

        public DataDefinition Deserializer(string Data)
        {
            var deserializer = new DeserializerBuilder().Build();

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
                rval = child.GetValue(p).ToString();
               
            }
            else
            {
                rval = parent.GetValue(setting).ToString();
            }
            return rval;
        }
    }
}
