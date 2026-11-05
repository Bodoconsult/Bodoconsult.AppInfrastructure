namespace BodoWebReporting.Dal.Model;

public class ReportingFile
{

    public ReportingFile()
    {
        ReportingFileParts = new List<ReportingFileParts>();
        CopyTasks = new List<CopyTask>();
    }

    public string FileName { get; set; }


    public string Author { get; set; }

    public string Shortname { get; set; }


    public IList<ReportingFileParts> ReportingFileParts { get; set; }


    public IList<CopyTask> CopyTasks { get; set; }
    public string LogoPath { get; set; }
}