# Data export services IDataExportService<T>/DataExportServiceBase<T>

Data export services based on IDataExportService<T>/DataExportServiceBase<T> are intended to store a datastream of messages like raw byte arrays, strings, custom data classes etc. to file.

Typical scenario is a network device sending communication data to a client. Client is intended to save the received data into files for later usage.

Use properties like TargetPath, Extenion, FileName, MaxFileSize to configure the service and its rolling mechanism. The implemented rolling mechanism adds a time stamp to the filename. If the maximum file size is reached, a new filename is created and the data are written in the new file.

## Requirements

The following requirements where intended to be flfilled with the implementation:

-   Flexible data input with base class DataExDataExportServiceBase<T>

-   Flexible output as binary, CSV, JSON, XML file (override method DataExDataExportServiceBase<T>.ToMemory(T data) as required in your superclass based on DataExDataExportServiceBase<T>)

-   Keep the order of the incoming data

-   Simple rolling mechanism dependent on export file maximum size (configurable)

-   Single threaded access to file resource

-   High performance

-   Low GC pressure


## ByteArrayDataExportServiceile size

``` csharp
[Test]
public void Add_ValidDefaultSetup1000000_FileWritten()
{
    // Arrange 
    const string text = "Blubb\r\n";

    var data = Encoding.UTF8.GetBytes(text);

    var service = new ByteArrayDataExportService
        {
            FileExtension = "bin"
        };
    service.Start();

    // Act
    for (var i = 0; i < 1000000; i++)
    {
        service.Add(data);
    }

    service.Stop();

    // Assert
    Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);
    Assert.That(File.Exists(service.CurrentFilePath));
    Assert.That(service.RowCounter, Is.EqualTo(1000000));

    FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
}
```

## StringDataExportService

``` csharp
[Test]
public void Add_ValidDefaultSetup1000000_FileWritten()
{
    // Arrange 
    const string text = "Blubb\r\n";

    var service = new StringDataExportService();
    service.Start();

    // Act
    for (var i = 0; i < 1000000; i++)
    {
        service.Add(text);
    }

    service.Stop();

    // Assert
    Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);
    Assert.That(File.Exists(service.CurrentFilePath));
    Assert.That(service.RowCounter, Is.EqualTo(1000000));

    FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
}
```

## Custom data export service implementation

The following code shows how implement a customer implementation of IDataExportService<T> based on DataExportServiceBase<T>.

The class below is defines the data to store in a file properties separated by semicolon (CSV file):

``` csharp
internal class TestData
{
    public string Text { get; set; } = "Some text";

    public DateTime Date { get; set; } = DateTime.Now;

    public bool IsValid { get; set; }

    public double Number { get; set; } = 12345.67;
}
```

Here the implementation of the service called TestDataExportService:

``` csharp
/// <summary>
/// Data export service for TestData instances
/// </summary>
public class TestDataExportService : DataExportServiceBase<TestData>
{
    private readonly CultureInfo _cultureInfo = new("en-us");

    /// <summary>
    /// Converts an object of type T into a ReadOnlyMemory&lt;byte&gt; instance
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">Thrown if type T is NOT string, ReadOnlyMemory&lt;byte&gt; or byte[]</exception>
    public override ReadOnlyMemory<byte> ToMemory(TestData data)
    {
        var sb = new StringBuilder();

        sb.Append($"{data.Text};");
        sb.Append($"{data.Date:O};");
        sb.Append($"{data.IsValid.ToString(_cultureInfo)};");
        sb.Append($"{data.Number.ToString("N", _cultureInfo)}{Environment.NewLine}");

        var b = Encoding.UTF8.GetBytes(sb.ToString());
        return b.AsMemory();
    }
}
```

Here the test showing how to use TestDataExportService class:

``` csharp
[Test]
public void Add_ValidDefaultSetup1000000_FileWritten()
{
    // Arrange 
    var data = new TestData();

    var service = new TestDataExportService();
    service.Start();

    // Act
    for (var i = 0; i < 1000000; i++)
    {
        service.Add(data);
    }

    service.Stop();

    // Assert
    Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);
    Assert.That(File.Exists(service.CurrentFilePath));
    Assert.That(service.RowCounter, Is.EqualTo(1000000));

    FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
}
```

