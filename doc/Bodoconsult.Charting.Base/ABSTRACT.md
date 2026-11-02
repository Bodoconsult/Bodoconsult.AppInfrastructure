Bodoconsult.Charting.Base is a base class library for creating charts from database data. The workflow for using the library is generally as follows:

1. Getting data as DataTable from a database
2. Create a ChartData object to make general settings for the chart and load the data from the DataTable as List<IChartItemData> 
3. Create a ChartHandler and deliver the ChartData object to it
4. Export the data to a file

To work properly the DataTable objects must have a certain logical structure depending on the type of chart you want to create.