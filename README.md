# Sim List View

# SimListView

SimListView is a .NET 8 library/component designed to read in a YAML file and render a list view control with high performance. 
It is optimized for handling large data sets efficiently while providing a flexible and customizable user interface.

## Features
- **High Performance**: Designed to handle large datasets efficiently.
- **Test Mode**: Includes a test mode for simulating data without requiring an actual data source.
- **Customizable**: Supports custom item templates and styles for flexible UI design.
- **Event Handling**: Provides events for item value changes.

## Getting Started

### Prerequisites

- .NET 8 SDK or later

### Installation

Add the SimListView package to your project:
### Usage

1. Import the namespace in your code:
2. Add the SimListView control to your UI and bind it to your data source.

ListView can be used in WPF, WinForms, or any other .NET UI framework that supports custom controls.

```csharp
using SimListView;

// Example of binding a YAML data source
var listView = new SimListView();
listViewlistView.load("path/to/your/data.yaml");
```

```yaml
# Example YAML data	
measures:
  - name: "Engine One"
    variable: "GENERAL ENG RPM:1"
    real:
       min: 0
       max: 16384
       rotation: RESTART
       increment: 1638
    display:
       min: 0
       max: 100
       unit: "RPM"
```	

## Documentation

- [API Reference](docs/API.md)
- [Examples](docs/Examples.md)
- [Changelog](CHANGELOG.md)

## Contributing

Contributions are welcome! Please open issues or submit pull requests via GitHub.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.md) file for details.
