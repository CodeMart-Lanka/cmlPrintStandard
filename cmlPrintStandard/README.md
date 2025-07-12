# cmlPrintStandard

A flexible and powerful .NET Standard 2.0 library for creating and managing print layouts with table-based structures. This library simplifies the process of creating complex print layouts with tables, text, and images.

## Features

- **Table-Based Layout System**: Create sophisticated print layouts using a table structure system
- **Flexible Cell Types**:
  - Text cells with customizable fonts, alignments, and rotations
  - Image cells for incorporating graphics
  - Container cells for hierarchical layouts
- **Watermark Support**: Add watermarks to your print documents
- **Multi-Page Support**: Handle content that spans multiple pages automatically
- **Text Rotation**: Rotate text horizontally or vertically
- **Cell Styling**: Control borders, background colors, and alignment
- **Export to Bitmap**: Generate bitmap images of print layouts
- **Preview Support**: Integrate with PrintPreviewControl or PrintPreviewDialog
- **Paper Type Settings**: Support for continuous paper and single sheet printing

## Requirements

- .NET Standard 2.0 compatible framework
- System.Drawing.Common dependencies

## Installation

Add a reference to the cmlPrintStandard library in your project:

```
dotnet add reference path/to/cmlPrintStandard.csproj
```

Or include the compiled DLL in your project references.

## Quick Start

### Basic Usage

```csharp
using cmlPrint.Print;
using cmlPrint.TableStructures;
using System.Drawing.Printing;

// Create a print document
PrintDocument doc = new PrintDocument();

// Create a container for your print content
PrintTableContainerCell container = new PrintTableContainerCell();

// Create a table with 3 columns
PrintTable table = new PrintTable(3);

// Add a row with text
PrintTableRow row = new PrintTableRow();
PrintTableTextCell cell1 = new PrintTableTextCell { Text = "Hello" };
PrintTableTextCell cell2 = new PrintTableTextCell { Text = "World" };
PrintTableTextCell cell3 = new PrintTableTextCell { Text = "Example" };
row.Add(cell1);
row.Add(cell2);
row.Add(cell3);

// Add the row to the table
table.Rows.Add(row);

// Set the container content
container.SetContent(table);

// Create the print document with the container
new cmlPrintDocument(doc, container);

// Print or preview the document
doc.Print();
// or
PrintPreviewDialog previewDialog = new PrintPreviewDialog();
previewDialog.Document = doc;
previewDialog.ShowDialog();
```

### Creating a Text Cell with Rotation

```csharp
PrintTableTextCell rotatedCell = new PrintTableTextCell
{
    Text = "Rotated Text",
    Font = new Font("Arial", 12),
    Rotation = Rotations.Vertical,
    ContentHorizontalAlign = HorizontalAlign.Center,
    ContentVerticalAlign = VerticalAlign.Center
};
```

### Adding a Watermark

```csharp
// Create a watermark cell
PrintTableTextCell watermark = new PrintTableTextCell
{
    Text = "CONFIDENTIAL",
    Font = new Font("Arial", 36),
    ForeColor = Color.FromArgb(50, Color.Red),
    ContentHorizontalAlign = HorizontalAlign.Center,
    ContentVerticalAlign = VerticalAlign.Center
};

// Create print document with watermark
new cmlPrintDocument(doc, container, watermark);
```

## Table Structure

The library uses a hierarchical structure of print elements:

- `PrintTableCell` - Base class for all print elements
  - `PrintTableTextCell` - Cell containing text content
  - `PrintTableImageCell` - Cell containing image content
  - `PrintTableContainerCell` - Cell that can contain another print element
  - `PrintTable` - Table structure containing rows
    - `PrintTableRow` - Row containing cells

## Customizing Appearance

### Cell Alignment

```csharp
textCell.ContentHorizontalAlign = HorizontalAlign.Center; // Left, Center, Right
textCell.ContentVerticalAlign = VerticalAlign.Bottom; // Top, Center, Bottom
```

### Cell Styling

```csharp
cell.BorderStyle = BorderStyle.All; // None, Left, Right, Top, Bottom, All
cell.BorderWidth = 1.0f;
cell.BorderColor = Color.Black;
cell.BackColor = Color.LightGray;
```

## Advanced Usage

### Exporting to Bitmap

```csharp
// Create a PrintPageEventArgs to define the export area
PrintPageEventArgs e = /* initialize with appropriate values */;

// Export to bitmap
Bitmap bitmap = printDocument.ExportToBitmap(e);
```

### Working with Multi-Page Documents

The library automatically handles content that spans multiple pages. Use the `AllowRowSplitting` property on tables to control how rows are split between pages.

```csharp
PrintTable table = new PrintTable(3);
table.AllowRowSplitting = true; // Allow rows to split across pages
```

## License

This library is provided for use under [your license terms].

## Contributing

Contributions are welcome. Please feel free to submit a Pull Request.