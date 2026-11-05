// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using BodoWebReporting.Dal;
using BodoWebReporting.Dal.Handler;

namespace BodoWebReporting;

public class Program
{
    [STAThread]
    public static void Main(
        //string[] args
    )
    {

        GlobalValues.LoadAppSettings();
            
        var appSettings = GlobalValues.CurrentAppSettings;


        var dataLayer = new DataHandler(appSettings.DatabaseConnection,
            appSettings.ErrorMailReceiver);


        var wrh = new WebReportingHandler(dataLayer)
        {
            TargetPath = appSettings.TargetPath,
            PreRunSql = appSettings.PreRunSql,
            StatusChange = Status
        };


        // WebTables
        wrh.CreateWebTables();

        //// Webcharts
        wrh.CreateWebCharts();

        // WebReports
        wrh.CreateWebReports();


        //lo = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = "dbo.[spMAIL_New]" };
        //lo.Parameters.Add("@To", SqlDbType.VarChar);
        //lo.Parameters[0].Value = ErrorMailer;
        //lo.Parameters.Add("@Subject", SqlDbType.VarChar);
        //lo.Parameters[1].Value = "BodoMoneyTransfer: Überweisungen fehlgeschlagen";
        //lo.Parameters.Add("@Msg", SqlDbType.VarChar);
        //lo.Parameters[2].Value = mail;
        //_mt._db.Exec(lo);


        //Console.ReadLine();

        if (!string.IsNullOrEmpty(appSettings.PostRunSql))
        {
            Status("Run post app SQL...");
            try
            {
                dataLayer.Exec(appSettings.PostRunSql);
            }
            catch 
            {
                // Do nothing
            }
        }

        Environment.Exit(0);
    }

    /// <summary>
    /// Status anzeigen
    /// </summary>
    /// <param name="msg">Anzuzeigende Nachricht</param>
    static void Status(string msg)
    {
        Console.WriteLine(msg);

    }
}