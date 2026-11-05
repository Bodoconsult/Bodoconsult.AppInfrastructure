// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace BodoWebReporting.Dal;

//internal class WebReportMigraDoc: WebReport
//{

//    private List<Chart> _charts;

//    public override void AddChart(ChartData data)
//    {
//        var w = new PdfChart
//        {
//            TypeOfChart = data.TypeOfChart,
//            Left = data.Left,
//            LegendLength = data.LegendLength
//        };
//        w.XSeries(data.Data, data.XRow);
//        w.AddSeries(data.Data, data.DataRow1, data.Label1);
//        if (data.DataRow2 != 0) w.AddSeries(data.Data, data.DataRow2, data.Label2);
//        if (data.DataRow3 != 0) w.AddSeries(data.Data, data.DataRow3, data.Label3);

//        w.Title = data.Title;
//        w.YAxisLabel = data.YLabel;
//        _charts.Add(w.Draw());
//    }



//    public override void EndCharting()
//    {
//        Pdf.TableStart();
//        Pdf.TableAddColumn(ParagraphAlignment.Left, 13);
//        Pdf.TableAddColumn(ParagraphAlignment.Left, 0.4);
//        Pdf.TableAddColumn(ParagraphAlignment.Right, 13);
//        var i = 0;
//        var zeile = -1;
//        foreach (var chart in _charts)
//        {
//            if (i == 0)
//            {
//                Pdf.TableAddRow();
//                zeile++;
//            }

//            Pdf.TableSetContent(i, zeile, chart);

//            i = i == 2 ? 0 : 2;
//        }

//        _charts = null;
//    }

//    public override void StartCharting()
//    {
//        _charts = new List<Chart>();
//    }
//}