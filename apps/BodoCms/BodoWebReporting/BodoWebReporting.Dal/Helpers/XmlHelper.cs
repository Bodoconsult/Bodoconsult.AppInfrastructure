using System.Data;
using System.Text;

namespace BodoWebReporting.Dal.Helpers;

/// <summary>
/// Helper methods for XML handling
/// </summary>
public static class XmlHelper
{
    /// <summary>
    /// Save a <see cref="DataTable"/> as XML file
    /// </summary>
    /// <param name="fileName">target file name</param>
    /// <param name="data"><see cref="DataTable"/> to serialize</param>

    public static void SaveDataTableAsXml(string fileName, DataTable data)
    {

        StringWriter strXML = new StringWriter();


        if (string.IsNullOrEmpty(data.TableName)) data.TableName = "Data";

        data.WriteXml(strXML, XmlWriteMode.WriteSchema, false);
        var xml = strXML.ToString();

        File.WriteAllText(fileName, xml, Encoding.UTF8);


    }

    /// <summary>
    /// Read derialized <see cref="DataTable"/> from XML
    /// </summary>
    /// <param name="fileName">source file name</param>
    /// <returns>Data from XML file as <see cref="DataTable"/></returns>
    public static DataTable GetDataTableFromXml(string fileName)
    {

        var dt = new DataTable();
        dt.ReadXml(fileName);

        return dt;
    }

}