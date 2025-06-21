using System.Diagnostics;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace SimListView
{
        internal class Yaml
        {
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
            public Yaml(SimListView parent,  string filePath)
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
                foreach (var item in data.measures)
                {
                    Debug.WriteLine($"ID: {item.id}, Name: {item.name}, Variable: {item.variable}, Index: {item.index}");
                    ListViewItem newItem = new ListViewItem(item.name);
                    newItem.SubItems.Add(item.id);
                    newItem.SubItems.Add(item.display.unit);
                    parent.Items.Add(newItem);

                }
            }
            public DataDefinition Deserializer(string Data)
            {
                var deserializer = new DeserializerBuilder().Build();

                return deserializer.Deserialize<DataDefinition>(Data.ToString());
            }
        }
}


