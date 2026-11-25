using System.Data;
using Microsoft.Data.SqlClient;

namespace Bodoconsult.App.WinForms.DataBindings;

/// <summary>
/// Handler for data bindings
/// </summary>
public class DataBindingHandler
{
    private readonly BindingSource _bs = new BindingSource();
    private SqlDataAdapter _da;
    private SqlCommandBuilder _cb;

    /// <summary>
    /// Is saved?
    /// </summary>
    public bool IsSaved { get; set; } = true;

    /// <summary>
    /// Caption text saying 'Edit item'
    /// </summary>
    public string SaveChangesCaption { get; set; } = "Edit item";

    /// <summary>
    /// Text saying 'Save changes?'
    /// </summary>
    public string SaveChangesText { get; set; } = "Save changes?";

    /// <summary>
    /// Add textbox binding on individual property
    /// </summary>
    /// <param name="c"></param>
    /// <param name="property"></param>
    /// <param name="memberName"></param>
    public void AddTextboxBinding(TextBox c, string property, string memberName)
    {
        var b = new Binding(property, _bs, memberName);
        b.Parse += DefaultParser;
        b.Format += DefaultFormat;
        c.DataBindings.Add(b);
    }

    /// <summary>
    /// Add standard textbox binding on control property Text
    /// </summary>
    /// <param name="c">Textbox control</param>
    /// <param name="memberName">Membername in datasource</param>
    public void AddTextboxBinding(TextBox c, string memberName)
    {
        AddTextboxBinding(c, "Text", memberName);
    }

    /// <summary>
    /// Add trackbar binding on individual property
    /// </summary>
    /// <param name="c">trackbar control</param>
    /// <param name="property">name of trackbar property to bind</param>
    /// <param name="memberName">data source property to bind</param>
    public void AddTrackBarBinding(TrackBar c, string property, string memberName)
    {
        var b = new Binding(property, _bs, memberName);
        b.Parse += DefaultParser;
        b.Format += DefaultFormat;
        c.DataBindings.Add(b);
    }

    /// <summary>
    /// Add standard trackbar binding on control property Value
    /// </summary>
    /// <param name="c">Textbox control</param>
    /// <param name="memberName">Membername in datasource</param>
    public void AddTrackBarBinding(TrackBar c, string memberName)
    {
        AddTrackBarBinding(c, "Value", memberName);
    }

    /// <summary>
    /// Add trackbar binding on individual property
    /// </summary>
    /// <param name="c">trackbar control</param>
    /// <param name="property">name of trackbar property to bind</param>
    /// <param name="memberName">data source property to bind</param>
    public void AddTrackBarBinding100(TrackBar c, string property, string memberName)
    {
        var b = new Binding(property, _bs, memberName);
        b.Parse += Double100ToDoubleParser;
        b.Format += DoubleToDouble100Format;
        c.DataBindings.Add(b);
    }

    /// <summary>
    /// Add standard trackbar binding on control property Value
    /// </summary>
    /// <param name="c">Textbox control</param>
    /// <param name="memberName">Membername in datasource</param>
    public void AddTrackBarBinding100(TrackBar c, string memberName)
    {
        AddTrackBarBinding100(c, "Value", memberName);
    }

    /// <summary>
    /// Add a binding to a <see cref="RichTextBox"/>>
    /// </summary>
    /// <param name="c">RichTextBox control</param>
    /// <param name="property">Property name</param>
    /// <param name="memberName">Membername in datasource</param>
    public void AddRichTextBoxBinding(RichTextBox c, string property, string memberName)
    {
        var b = new Binding(property, _bs, memberName);
        b.Parse += DefaultParser;
        b.Format += DefaultFormat;
        c.DataBindings.Add(b);
    }

    /// <summary>
    /// Add a binding to a <see cref="ComboBox"/>>
    /// </summary>
    /// <param name="c">ComboBox control</param>
    /// <param name="property">Property name</param>
    /// <param name="memberName">Membername in datasource</param>
    public void AddComboBoxBinding(ComboBox c, string property, string memberName)
    {
        var b = new Binding(property, _bs, memberName);
        b.Parse += DefaultParser;
        b.Format += DefaultFormat;
        c.DataBindings.Add(b);
    }

    /// <summary>
    /// Add a plain binding to a combobox
    /// </summary>
    /// <param name="c">Combobox control</param>
    /// <param name="binding">Binding</param>
    public void AddPlainBinding(ComboBox c, Binding binding)
    {
        c.DataBindings.Add(binding);
    }

    /// <summary>
    /// Add a binding to a <see cref="CheckBox"/>>
    /// </summary>
    /// <param name="c">CheckBox control</param>
    /// <param name="property">Property name</param>
    /// <param name="memberName">Membername in datasource</param>
    public void AddCheckboxBinding(CheckBox c, string property, string memberName)
    {
        var b = new Binding(property, _bs, memberName);
        b.Format += BitToCheckBox;
        b.Parse += CheckBoxToBit;
        c.DataBindings.Add(b);
    }

    /// <summary>
    /// Add a binding to CheckState property for a <see cref="CheckBox"/>>
    /// </summary>
    /// <param name="c">CheckBox control</param>
    /// <param name="memberName">Membername in datasource</param>
    public void AddCheckboxBinding(CheckBox c, string memberName)
    {
        AddCheckboxBinding(c, "CheckState", memberName);
    }

    /// <summary>
    /// currency binding for a textbox on individual property
    /// </summary>
    /// <param name="c">Textbox control </param>
    /// <param name="property">Textbox property to bind</param>
    /// <param name="memberName">Datasource property to bind</param>
    public void AddCurrencyBinding(TextBox c, string property, string memberName)
    {
        var b = new Binding(property, _bs, memberName);
        b.Format += DoubleToCurrencyString;
        b.Parse += CurrencyStringToDouble;
        c.DataBindings.Add(b);
        c.TextAlign = HorizontalAlignment.Right;
    }

    /// <summary>
    /// standard currency binding for a textbox property Text
    /// </summary>
    /// <param name="c">Textbox property to bind</param>
    /// <param name="memberName">Datasource property to bind</param>
    public void AddCurrencyBinding(TextBox c, string memberName)
    {
        AddCurrencyBinding(c, "Text", memberName);
    }

    /// <summary>
    /// percent binding for a textbox on individual property
    /// </summary>
    /// <param name="c">Textbox control </param>
    /// <param name="property">Textbox property to bind</param>
    /// <param name="memberName">Datasource property to bind</param>
    public void AddPercentBinding(TextBox c, string property, string memberName)
    {
        var b = new Binding(property, _bs, memberName);
        b.Format += DoubleToPercentString;
        b.Parse += PercentStringToDouble;
        c.DataBindings.Add(b);
        c.TextAlign = HorizontalAlignment.Right;
    }

    /// <summary>
    /// standard percent binding for a textbox property Text
    /// </summary>
    /// <param name="c">Textbox property to bind</param>
    /// <param name="memberName">Datasource property to bind</param>
    public void AddPercentBinding(TextBox c, string memberName)
    {
        AddPercentBinding(c, "Text", memberName);
    }

    /// <summary>
    /// date binding for a textbox on individual property
    /// </summary>
    /// <param name="c">Textbox control </param>
    /// <param name="property">Textbox property to bind</param>
    /// <param name="memberName">Datasource property to bind</param>
    public void AddDateBinding(TextBox c, string property, string memberName)
    {
        var b = new Binding(property, _bs, memberName);
        b.Format += DateToDateString;
        b.Parse += DateStringToDate;
        c.DataBindings.Add(b);
    }

    /// <summary>
    /// standard date binding for a textbox property Text
    /// </summary>
    /// <param name="c">Textbox property to bind</param>
    /// <param name="memberName">Datasource property to bind</param>
    public void AddDateBinding(TextBox c, string memberName)
    {
        AddDateBinding(c, "Text", memberName);
    }

    /// <summary>
    /// standard double binding for a textbox property Text
    /// </summary>
    /// <param name="c">Textbox property to bind</param>
    /// <param name="memberName">Datasource property to bind</param>
    public void AddDoubleBinding(TextBox c, string memberName)
    {
        AddDoubleBinding(c, "Text", memberName);
    }

    /// <summary>
    /// double binding for a textbox on individual property
    /// </summary>
    /// <param name="c">Textbox control </param>
    /// <param name="property">Textbox property to bind</param>
    /// <param name="memberName">Datasource property to bind</param>
    public void AddDoubleBinding(TextBox c, string property, string memberName)
    {
        var b = new Binding(property, _bs, memberName);
        b.Format += DoubleToDoubleString;
        b.Parse += StringToDouble;
        c.DataBindings.Add(b);
        c.TextAlign = HorizontalAlignment.Right;
    }

    /// <summary>
    /// Add a double binding with 5 decimals to a TextBox 
    /// </summary>
    /// <param name="c">TextBox control</param>
    /// <param name="property">Textbox property to bind</param>
    /// <param name="memberName">Datasource property to bind</param>
    public void AddDoubleBinding5(TextBox c, string property, string memberName)
    {
        var b = new Binding(property, _bs, memberName);
        b.Format += DoubleToDoubleString5;
        b.Parse += StringToDouble;
        c.DataBindings.Add(b);
        c.TextAlign = HorizontalAlignment.Right;
    }

    private static void DefaultFormat(object sender, ConvertEventArgs e)
    {
        e.Value = e.Value;
    }

    private void DefaultParser(object sender, ConvertEventArgs e)
    {
        e.Value = e.Value;
        IsSaved = false;
    }

    private static void DoubleToDouble100Format(object sender, ConvertEventArgs e)
    {
        e.Value = Convert.ToDouble(e.Value) * 100;
    }

    private void Double100ToDoubleParser(object sender, ConvertEventArgs e)
    {
        e.Value = Convert.ToDouble(e.Value) / 100;
        IsSaved = false;
    }

    private void CheckBoxToBit(object sender, ConvertEventArgs e)
    {
        if (e.Value is null)
        {
            e.Value = 0;
            return;
        }

        e.Value = (CheckState)e.Value == CheckState.Checked ? 1 : 0;
        IsSaved = false;
    }

    private static void BitToCheckBox(object sender, ConvertEventArgs e)
    {
        if (e.Value is null)
        {
            e.Value = CheckState.Unchecked;
            return;
        }

        e.Value = (bool)e.Value ? CheckState.Checked : CheckState.Unchecked;
    }

    /// <summary>
    /// Zahl in Währungsstring umrechnen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void DoubleToCurrencyString(object sender, ConvertEventArgs e)
    {
        try
        {
            e.Value = e.Value == null ? 0 : double.Parse(e.Value.ToString() ?? "0").ToString("c");
        }
        catch
        {
            e.Value = ((double)0).ToString("c");
        }
    }

    /// <summary>
    /// Währungsstring in Zahl umrechnen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CurrencyStringToDouble(object sender, ConvertEventArgs e)
    {
        try
        {
            e.Value = double.Parse(e.Value?.ToString() ?? "0", System.Globalization.NumberStyles.Currency);
        }
        catch
        {

            e.Value = (double)0;
        }
        IsSaved = false;
    }


    private static void DoubleToPercentString(object sender, ConvertEventArgs e)
    {
        try
        {
            if (e.Value == null)
            {
                e.Value = 0;
            }
            else
            {
                e.Value = $"{e.Value:p}";
            }
        }
        catch
        {
            e.Value = $"{0:p}";
        }
    }

    private void PercentStringToDouble(object sender, ConvertEventArgs e)
    {
        try
        {
            e.Value = double.Parse(e.Value?.ToString()?.Replace("%", "") ?? "0", System.Globalization.NumberStyles.Float) / 100;
        }
        catch
        {

            e.Value = (double)0;
        }
        IsSaved = false;
    }

    /// <summary>
    /// String in Double-Zahl umwandeln
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StringToDouble(object sender, ConvertEventArgs e)
    {
        try
        {
            e.Value = Convert.ToDouble(e.Value?.ToString() ?? "0");
        }
        catch
        {

            e.Value = (double)0;
        }
        IsSaved = false;
    }
    /// <summary>
    /// Double in Double-String umwandeln
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void DoubleToDoubleString(object sender, ConvertEventArgs e)
    {
        try
        {
            e.Value = e.Value == null ? (object)0 : double.Parse(e.Value?.ToString() ?? "0").ToString("#,##0.00");
        }
        catch
        {
            e.Value = ((double)0).ToString("#,##0.00");

        }
    }


    /// <summary>
    /// Double in Double-String umwandeln
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void DoubleToDoubleString5(object sender, ConvertEventArgs e)
    {
        try
        {
            e.Value = e.Value == null ? (object)0 : double.Parse(e.Value?.ToString() ?? "0").ToString("#,##0.00000");
        }
        catch
        {
            e.Value = ((double)0).ToString("#,##0.00000");

        }
    }

    /// <summary>
    /// Datum in Datum-Zeichenkette umwandeln
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void DateToDateString(object sender, ConvertEventArgs e)
    {
        try
        {
            if (e.Value != null)
            {
                e.Value = double.Parse(e.Value?.ToString() ?? "0").ToString("d");
            }
        }
        catch
        {
            e.Value = null;

        }
    }

    /// <summary>
    /// Datum-Zeichenkette in Datum umwandeln
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DateStringToDate(object sender, ConvertEventArgs e)
    {
        try
        {
            e.Value = DateTime.Parse(e.Value?.ToString() ?? "0");
        }
        // ReSharper disable EmptyGeneralCatchClause
        catch
        // ReSharper restore EmptyGeneralCatchClause
        {

        }
        IsSaved = false;
    }

    /// <summary>
    /// Data adapter
    /// </summary>
    public SqlDataAdapter DataAdapter
    {
        set
        {
            _da = value;
            _cb = new SqlCommandBuilder(_da);
            _cb.GetUpdateCommand();
            _da.Fill(DataTable);
            _bs.DataSource = DataTable;
        }
    }

    /// <summary>
    /// Data source
    /// </summary>
    public object DataSource
    {
        get => _bs.DataSource;
        set => _bs.DataSource = value;
    }

    /// <summary>
    /// If there are changes ask the user whether to save them to database
    /// </summary>
    public void Save()
    {
        // Check if rows were changed
        if (IsSaved)
        {
            return;
        }

        if (MessageBox.Show(SaveChangesText, SaveChangesCaption, MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            _bs.EndEdit();
            _da.Update(DataTable);
        }
        else
        {
            _bs.CancelEdit();
        }
    }

    /// <summary>
    /// If there are changes ask the user whether to save them
    /// </summary>
    public void Save1()
    {
        // Check if rows were changed
        if (IsSaved)
        {
            return;
        }

        if (MessageBox.Show(SaveChangesText, SaveChangesCaption, MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            _bs.EndEdit();
        }
        else
        {
            _bs.CancelEdit();
        }
    }

    /// <summary>
    /// Save without asking for confirmation
    /// </summary>
    public void Save2()
    {
        // Check if rows were changed
        if (IsSaved)
        {
            return;
        }

        _bs.EndEdit();
    }

    /// <summary>
    /// Current data table
    /// </summary>
    public DataTable DataTable { get; } = new();
}