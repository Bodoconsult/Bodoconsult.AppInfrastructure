Bodoconsult.Charting
============================

# Overview

>   [Chart data input](#chart-data-input)

>   [Styling the charts](#styling-the-charts)

>   [Creating charts](#creating-charts)

>>   [Line charts](#line-chart)

>>   [Bar chart](#bar-chart)

>>   [Column chart](#column-chart)

>>   [Stacked bar chart](#stacked-bar-chart)

>>   [Stacked column chart](#stacked-column-chart)

>>   [Stacked column chart 100 percent](#stacked-column-chart-100)

>>   [Pie chart](#pie-chart)

>>   [Point chart](#point-chart-risk-return-chart-in-finance)

>   [Using Bodoconsult.Charting in a DI container environment](#using-bodoconsultcharting-in-a-di-container-environment)

>   [Extension methods for charting](#extension-methods-for-charting)

## What does Bodoconsult.Charting library

Bodoconsult.Charting is a library for creating charts from database data. 

Here a sample chart created with Bodoconsult.Charting:

![Sample for stacked column chart 100%](../../images/StackedColumn100Chart.png)

The workflow for using the library is generally as follows:

1. Getting data as DataTable from a database
2. Create a ChartData object to make general settings for the chart and load the data from the DataTable as list of IChartItemData items 
3. Create a ChartHandler and deliver the ChartData object to it
4. Export the data to a file

To work properly the DataTable objects must have a certain logical structure depending on the type of chart you want to create.

## How to use the library

The source code contains NUnit test classes the following source code is extracted from. The samples below show the most helpful use cases for the library.

# Chart data input

All chart types require a IList<IChartItemData> as input.

The following classes are implementing IChartItemData currently: 

-   ChartItemData

-   PieChartItemData

-   PointChartItemData 

See the description for each chart type below for the required type of data input.

# Styling the charts

Chart styling is based on the class ChartStyle from the library Bodoconsult.App.Abstractions you can download via Nuget. See the following code fragments for how chart styling is done in the test project.


``` csharp
public static class TestHelper
{
    ...
    
    /// <summary>
    /// Load a default style for the charts
    /// </summary>
    /// <param name="chartData"></param>
    /// <param name="highResolution"></param>
    public static void LoadDefaultChartStyle(ChartData chartData, bool highResolution = false)
    {

        var chartStyle = GlobalValues.DefaultTypography().ChartStyle;

        if (highResolution)
        {
            chartStyle.Width = 4500;
            chartStyle.Height = 2781;
            chartStyle.FontSize = 12;
        }

        chartStyle.CopyrightFontSizeDelta = 0.6F;
        chartStyle.BackgroundColor = Color.Transparent;
        //_chartStyle.AxisLineColor = Color.DarkBlue;

        //
        chartData.ChartStyle  = chartStyle;
        chartData.Copyright = "(c) Testfirma";
    }
    
...
}
```

``` csharp
public static class GlobalValues
{
    /// <summary>
    /// Get a elegant default typography
    /// </summary>
    /// <returns></returns>
    public static ITypography DefaultTypography()
    {
        var typography = new ElegantTypographyPageHeader("Aptos", "Aptos", "Aptos")
        {
            ChartStyle =
            {
                Width = 750,
                Height = 464,
                FontSize = 10,
                BackGradientStyle = GradientStyle.TopBottom,
            }
        };

        return typography;
    }
}
```

# Creating charts

## Line chart

### Sample image

![Sample for a line chart](../../images/LineChart.png)

### Chart data

ChartItemData class

``` csharp
/// <summary>
/// Data items for line charts, stacked bar charts and others.
/// </summary>
public class ChartItemData : IChartItemData
{
    /// <summary>
    /// Value for the x axis
    /// </summary>
    public double XValue { get; set; }

    /// <summary>
    /// Value for the data series 1
    /// </summary>
    public double YValue1 { get; set; }
    /// <summary>
    /// Value for the data series 2
    /// </summary>
    public double YValue2 { get; set; }
    /// <summary>
    /// Value for the data series 3
    /// </summary>
    public double YValue3 { get; set; }
    /// <summary>
    /// Value for the data series 4
    /// </summary>
    public double YValue4 { get; set; }
    /// <summary>
    /// Value for the data series 5
    /// </summary>
    public double YValue5 { get; set; }
    /// <summary>
    /// Value for the data series 6
    /// </summary>
    public double YValue6 { get; set; }
    /// <summary>
    /// Value for the data series 7
    /// </summary>
    public double YValue7 { get; set; }
    /// <summary>
    /// Value for the data series 8
    /// </summary>
    public double YValue8 { get; set; }
    /// <summary>
    /// Value for the data series 9
    /// </summary>
    public double YValue9 { get; set; }
    /// <summary>
    /// Value for the data series 10
    /// </summary>
    public double YValue10 { get; set; }

    /// <summary>
    /// Are x axis values dates?
    /// </summary>
    public bool IsDate { get; set; }

    /// <summary>
    /// Label to show for the item. May be null. If null, the XValue is used as label.
    /// </summary>
    public string Label  { get; set; }
}
```

### Code

``` csharp
const string fileName = @"d:\temp\ScottPlott_LineChart.png";

if (File.Exists(fileName)) File.Delete(fileName);

var data = new ChartData
{
    Title = "Test portfolio",
    Copyright = "Testfirma",
    XLabelText = "Anlageklassen",
    YLabelText = "Anteilwert",
    FileName = fileName,
    ChartType = ChartType.LineChart,
    //PaperColor = Color.Red
};

TestHelper.LoadDefaultChartStyle(data, HighResolution);

TestDataHelper.ChartSample(UseDatabase, data);

data.ChartStyle.XAxisNumberformat = "dd.MM.yyyy";

data.PropertiesToUseForChart.Add("XValue");
data.PropertiesToUseForChart.Add("YValue1");
data.PropertiesToUseForChart.Add("YValue2");
data.PropertiesToUseForChart.Add("YValue3");

data.LabelsForSeries.Add("Aktien");
data.LabelsForSeries.Add("Renten");
data.LabelsForSeries.Add("Liquidität");

var bitmapServiceFactory = new BitmapServiceFactory();

var x = new ChartHandler(data, bitmapServiceFactory);

x.Export();
```

# Bar chart

## Sample image

![Sample for bar chart](../../images/BarChart.png)

## Chart data

ChartItemData class

``` csharp
/// <summary>
/// Data items for line charts, stacked bar charts and others.
/// </summary>
public class ChartItemData : IChartItemData
{
    /// <summary>
    /// Value for the x axis
    /// </summary>
    public double XValue { get; set; }

    /// <summary>
    /// Value for the data series 1
    /// </summary>
    public double YValue1 { get; set; }
    /// <summary>
    /// Value for the data series 2
    /// </summary>
    public double YValue2 { get; set; }
    /// <summary>
    /// Value for the data series 3
    /// </summary>
    public double YValue3 { get; set; }
    /// <summary>
    /// Value for the data series 4
    /// </summary>
    public double YValue4 { get; set; }
    /// <summary>
    /// Value for the data series 5
    /// </summary>
    public double YValue5 { get; set; }
    /// <summary>
    /// Value for the data series 6
    /// </summary>
    public double YValue6 { get; set; }
    /// <summary>
    /// Value for the data series 7
    /// </summary>
    public double YValue7 { get; set; }
    /// <summary>
    /// Value for the data series 8
    /// </summary>
    public double YValue8 { get; set; }
    /// <summary>
    /// Value for the data series 9
    /// </summary>
    public double YValue9 { get; set; }
    /// <summary>
    /// Value for the data series 10
    /// </summary>
    public double YValue10 { get; set; }

    /// <summary>
    /// Are x axis values dates?
    /// </summary>
    public bool IsDate { get; set; }

    /// <summary>
    /// Label to show for the item. May be null. If null, the XValue is used as label.
    /// </summary>
    public string Label  { get; set; }
}
```

## Code

``` csharp
const string fileName = @"d:\temp\ScottPlott_BarChart.png";

if (File.Exists(fileName)) File.Delete(fileName);

var data = new ChartData
{
    Title = "Test portfolio",
    Copyright = "Testfirma",
    XLabelText = "Anlageklassen",
    YLabelText = "Anteil in %",

    FileName = fileName,
    ChartType = ChartType.BarChart,

};

TestHelper.LoadDefaultChartStyle(data, HighResolution);

TestDataHelper.BarChartSample(UseDatabase, data);

data.ChartStyle.XAxisNumberformat = "0.00";

var bitmapServiceFactory = new BitmapServiceFactory();

var x = new ChartHandler(data, bitmapServiceFactory);

x.Export();
```

# Column chart

## Sample image

![Sample for column chart](../../images/ColumnChart.png)

## Chart data

ChartItemData class

``` csharp
/// <summary>
/// Data items for line charts, stacked bar charts and others.
/// </summary>
public class ChartItemData : IChartItemData
{
    /// <summary>
    /// Value for the x axis
    /// </summary>
    public double XValue { get; set; }

    /// <summary>
    /// Value for the data series 1
    /// </summary>
    public double YValue1 { get; set; }
    /// <summary>
    /// Value for the data series 2
    /// </summary>
    public double YValue2 { get; set; }
    /// <summary>
    /// Value for the data series 3
    /// </summary>
    public double YValue3 { get; set; }
    /// <summary>
    /// Value for the data series 4
    /// </summary>
    public double YValue4 { get; set; }
    /// <summary>
    /// Value for the data series 5
    /// </summary>
    public double YValue5 { get; set; }
    /// <summary>
    /// Value for the data series 6
    /// </summary>
    public double YValue6 { get; set; }
    /// <summary>
    /// Value for the data series 7
    /// </summary>
    public double YValue7 { get; set; }
    /// <summary>
    /// Value for the data series 8
    /// </summary>
    public double YValue8 { get; set; }
    /// <summary>
    /// Value for the data series 9
    /// </summary>
    public double YValue9 { get; set; }
    /// <summary>
    /// Value for the data series 10
    /// </summary>
    public double YValue10 { get; set; }

    /// <summary>
    /// Are x axis values dates?
    /// </summary>
    public bool IsDate { get; set; }

    /// <summary>
    /// Label to show for the item. May be null. If null, the XValue is used as label.
    /// </summary>
    public string Label  { get; set; }
}
```

## Code

``` csharp
const string fileName = @"d:\temp\ScottPlott_ColumnChart.png";

if (File.Exists(fileName)) File.Delete(fileName);

var data = new ChartData
{

    Title = "Test portfolio",
    Copyright = "Testfirma",
    XLabelText = "Anlageklassen",
    YLabelText = "Anteil in %",


    FileName = fileName,
    ChartType = ChartType.ColumnChart,

};

TestHelper.LoadDefaultChartStyle(data, HighResolution);
TestDataHelper.BarChartSample(UseDatabase, data);

data.ChartStyle.YAxisNumberformat = "0.00";

var bitmapServiceFactory = new BitmapServiceFactory();

var x = new ChartHandler(data, bitmapServiceFactory);

x.Export();

TestHelper.StartFile(fileName);
```

# Stacked bar chart

## Sample image

![Sample for stacked bar chart](../../images/StackedBarChart.png)

## Chart data

ChartItemData class

``` csharp
/// <summary>
/// Data items for line charts, stacked bar charts and others.
/// </summary>
public class ChartItemData : IChartItemData
{
    /// <summary>
    /// Value for the x axis
    /// </summary>
    public double XValue { get; set; }

    /// <summary>
    /// Value for the data series 1
    /// </summary>
    public double YValue1 { get; set; }
    /// <summary>
    /// Value for the data series 2
    /// </summary>
    public double YValue2 { get; set; }
    /// <summary>
    /// Value for the data series 3
    /// </summary>
    public double YValue3 { get; set; }
    /// <summary>
    /// Value for the data series 4
    /// </summary>
    public double YValue4 { get; set; }
    /// <summary>
    /// Value for the data series 5
    /// </summary>
    public double YValue5 { get; set; }
    /// <summary>
    /// Value for the data series 6
    /// </summary>
    public double YValue6 { get; set; }
    /// <summary>
    /// Value for the data series 7
    /// </summary>
    public double YValue7 { get; set; }
    /// <summary>
    /// Value for the data series 8
    /// </summary>
    public double YValue8 { get; set; }
    /// <summary>
    /// Value for the data series 9
    /// </summary>
    public double YValue9 { get; set; }
    /// <summary>
    /// Value for the data series 10
    /// </summary>
    public double YValue10 { get; set; }

    /// <summary>
    /// Are x axis values dates?
    /// </summary>
    public bool IsDate { get; set; }

    /// <summary>
    /// Label to show for the item. May be null. If null, the XValue is used as label.
    /// </summary>
    public string Label  { get; set; }
}
```

## Code

``` csharp
const string fileName = @"d:\temp\ScottPlott_Db_StackedBarChart.png";

if (File.Exists(fileName)) File.Delete(fileName);

var data = new ChartData
{
    Title = "Test portfolio",
    Copyright = "Testfirma",
    XLabelText = "Anlageklassen",
    YLabelText = "Anteilswert",
    FileName = fileName,
    ChartType = ChartType.StackedBarChart,

};

TestHelper.LoadDefaultChartStyle(data);

var dt = TestHelper.GetDataTable("StackedBarChart.xml");
ChartUtility.DataTableToChartItemData(dt, "", data);

var bitmapServiceFactory = new BitmapServiceFactory();

var x = new ChartHandler(data, bitmapServiceFactory);

x.Export();
```

# Stacked column chart

## Sample image

![Sample for stacked column chart](../../images/StackedColumnChart.png)

## Chart data

ChartItemData class

``` csharp
/// <summary>
/// Data items for line charts, stacked bar charts and others.
/// </summary>
public class ChartItemData : IChartItemData
{
    /// <summary>
    /// Value for the x axis
    /// </summary>
    public double XValue { get; set; }

    /// <summary>
    /// Value for the data series 1
    /// </summary>
    public double YValue1 { get; set; }
    /// <summary>
    /// Value for the data series 2
    /// </summary>
    public double YValue2 { get; set; }
    /// <summary>
    /// Value for the data series 3
    /// </summary>
    public double YValue3 { get; set; }
    /// <summary>
    /// Value for the data series 4
    /// </summary>
    public double YValue4 { get; set; }
    /// <summary>
    /// Value for the data series 5
    /// </summary>
    public double YValue5 { get; set; }
    /// <summary>
    /// Value for the data series 6
    /// </summary>
    public double YValue6 { get; set; }
    /// <summary>
    /// Value for the data series 7
    /// </summary>
    public double YValue7 { get; set; }
    /// <summary>
    /// Value for the data series 8
    /// </summary>
    public double YValue8 { get; set; }
    /// <summary>
    /// Value for the data series 9
    /// </summary>
    public double YValue9 { get; set; }
    /// <summary>
    /// Value for the data series 10
    /// </summary>
    public double YValue10 { get; set; }

    /// <summary>
    /// Are x axis values dates?
    /// </summary>
    public bool IsDate { get; set; }

    /// <summary>
    /// Label to show for the item. May be null. If null, the XValue is used as label.
    /// </summary>
    public string Label  { get; set; }
}
```

## Code

``` csharp
const string fileName = @"d:\temp\ScottPlott_Db_StackedColumnChart.png";

if (File.Exists(fileName)) File.Delete(fileName);

var data = new ChartData
{
    Title = "Test portfolio",
    Copyright = "Testfirma",
    XLabelText = "Anlageklassen",
    YLabelText = "Anteilswert",
    FileName = fileName,
    ChartType = ChartType.StackedColumnChart,

};

TestHelper.LoadDefaultChartStyle(data);

var dt = TestHelper.GetDataTable("StackedColumnChart.xml");
ChartUtility.DataTableToChartItemData(dt, "", data);

var bitmapServiceFactory = new BitmapServiceFactory();

var x = new ChartHandler(data, bitmapServiceFactory);

x.Export();
```

# Stacked column chart 100%

## Sample image

![Sample for stacked column chart 100%](../../images/StackedColumn100Chart.png)

## Chart data

ChartItemData class

``` csharp
/// <summary>
/// Data items for line charts, stacked bar charts and others.
/// </summary>
public class ChartItemData : IChartItemData
{
    /// <summary>
    /// Value for the x axis
    /// </summary>
    public double XValue { get; set; }

    /// <summary>
    /// Value for the data series 1
    /// </summary>
    public double YValue1 { get; set; }
    /// <summary>
    /// Value for the data series 2
    /// </summary>
    public double YValue2 { get; set; }
    /// <summary>
    /// Value for the data series 3
    /// </summary>
    public double YValue3 { get; set; }
    /// <summary>
    /// Value for the data series 4
    /// </summary>
    public double YValue4 { get; set; }
    /// <summary>
    /// Value for the data series 5
    /// </summary>
    public double YValue5 { get; set; }
    /// <summary>
    /// Value for the data series 6
    /// </summary>
    public double YValue6 { get; set; }
    /// <summary>
    /// Value for the data series 7
    /// </summary>
    public double YValue7 { get; set; }
    /// <summary>
    /// Value for the data series 8
    /// </summary>
    public double YValue8 { get; set; }
    /// <summary>
    /// Value for the data series 9
    /// </summary>
    public double YValue9 { get; set; }
    /// <summary>
    /// Value for the data series 10
    /// </summary>
    public double YValue10 { get; set; }

    /// <summary>
    /// Are x axis values dates?
    /// </summary>
    public bool IsDate { get; set; }

    /// <summary>
    /// Label to show for the item. May be null. If null, the XValue is used as label.
    /// </summary>
    public string Label  { get; set; }
}
```

## Code

``` csharp
const string fileName = @"d:\temp\ScottPlott_Db_StackedColumn100Chart.png";

if (File.Exists(fileName)) File.Delete(fileName);

var data = new ChartData
{
    Title = "Test portfolio",
    Copyright = "Testfirma",
    XLabelText = "Anlageklassen",
    YLabelText = "Anteilswert",
    FileName = fileName,
    ChartType = ChartType.StackedColumn100Chart,
};

TestHelper.LoadDefaultChartStyle(data);

var dt = TestHelper.GetDataTable("StackedColumnChart.xml");
ChartUtility.DataTableToChartItemData(dt, "", data);

var bitmapServiceFactory = new BitmapServiceFactory();

var x = new ChartHandler(data, bitmapServiceFactory);

x.Export();
```

# Pie chart

## Sample image

![Sample for pie chart](../../images/PieChart.png)

## Chart data

PieChartItemData class

``` csharp
/// <summary>
/// Item data for a pie chart
/// </summary>
public class PieChartItemData: IChartItemData
{
    /// <summary>
    /// X axis value
    /// </summary>
    public string XValue { get; set; }

    /// <summary>
    /// Value for the series
    /// </summary>
    public double YValue { get; set; }
}
```

## Code

``` csharp
const string fileName = @"d:\temp\ScottPlott_PieChart.png";

if (File.Exists(fileName)) File.Delete(fileName);

var data = new ChartData
{
    Title = "Test portfolio",
    Copyright = "Testfirma",
    YLabelText = "Anteil in %",
    FileName = fileName,
    ChartType = ChartType.PieChart,
};

TestHelper.LoadDefaultChartStyle(data, HighResolution);

TestDataHelper.PieChartSample(UseDatabase, data);

var bitmapServiceFactory = new BitmapServiceFactory();

var x = new ChartHandler(data, bitmapServiceFactory);

x.Export();
```


# Point chart (risk-return-chart in finance)

## Sample image

![Sample for point chart](../../images/PointChart.png)

## Chart data

PointChartItemData class

``` csharp
/// <summary>
/// Point item to show in a point chart
/// </summary>
public class PointChartItemData: IChartItemData
{
    /// <summary>
    /// X axis value
    /// </summary>
    public double XValue { get; set; }

    /// <summary>
    /// Y axis value
    /// </summary>
    public double YValue { get; set; }

    /// <summary>
    /// Label for the data point
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Color index for the data point
    /// </summary>
    public TypoColor Color { get; set; }
}
```

## Code

``` csharp
const string fileName = @"d:\temp\ScottPlott_Db_StackedColumn100Chart.png";

if (File.Exists(fileName)) File.Delete(fileName);

var data = new ChartData
{
    Title = "Test portfolio",
    Copyright = "Testfirma",
    XLabelText = "Anlageklassen",
    YLabelText = "Anteilswert",
    FileName = fileName,
    ChartType = ChartType.StackedColumn100Chart,
};

TestHelper.LoadDefaultChartStyle(data);

var dt = TestHelper.GetDataTable("StackedColumnChart.xml");
ChartUtility.DataTableToChartItemData(dt, "", data);

var bitmapServiceFactory = new BitmapServiceFactory();

var x = new ChartHandler(data, bitmapServiceFactory);

x.Export();
```

# Stock chart

ToDo

# Using Bodoconsult.Charting in a DI container environment

``` csharp
// Arrange 
var di = new DiContainer();

// load other providers here if necessary
...

// Act  
var provider = new ChartingDiContainerServiceProvider();
provider.AddServices(di);

// Load more other providers here if necessary
...

// Now build the container
di.BuildServiceProvider();

// Assert
var result = di.Get<IChartHandlerFactory>();
Assert.That(result, Is.Not.Null);

// Use DI
var data = new ChartData(); // configuring chartdata as required has to be added here

var ch = result.CreateInstance(data); // Create a chart handler instance now

ch.Export(); // Export the chart to PNG file
```

# Extension methods for charting

For System.Data.DataTables instances there are helpful methods for converting them to chart input data. See the following methods:

-   ToChartItemData()

-   ToPieChartItemData()

-   ToPointChartItemData()

Alternatively you can use the class ChartUtility class.

Column names of the DataTable instances are used as labels for the legend (if applicable).

## ToChartItemData()

ToDo: add description for DataTable

## ToPieChartItemData()

ToDo: add description for DataTable

## ToPointChartItemData()

ToDo: add description for DataTable

# About us

Bodoconsult <http://www.bodoconsult.de> is a Munich based software company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

