// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.WinForms.Forms;

/// <summary>
/// Delegate used to validate an InputBox
/// </summary>
public delegate void InputBoxValidatingDelegate(object sender, InputBoxValidatingArgs e);

/// <summary>
/// Defines a simple inputbox
/// </summary>
public class InputBox : Form
{
    /// <summary>
    /// Required designer variable
    /// </summary>
    private readonly System.ComponentModel.Container _components = null;

    /// <summary>
    /// Delegate used to validate the object
    /// </summary>
    private InputBoxValidatingDelegate _validator;

    /// <summary>
    /// Button OK
    /// </summary>
    protected Button ButtonOk;

    /// <summary>
    /// Button Cancel
    /// </summary>
    protected Button ButtonCancel;

    /// <summary>
    /// Label prompt
    /// </summary>
    protected Label LabelPrompt;

    /// <summary>
    /// Textbox text
    /// </summary>
    protected TextBox TextBoxText;

    /// <summary>
    /// Error provider text
    /// </summary>
    protected ErrorProvider ErrorProviderText;

    /// <summary>
    /// Validation delegate instance
    /// </summary>
    protected InputBoxValidatingDelegate Validator
    {
        get => _validator;
        set => _validator = value;
    }

    /// <summary>
    /// Default ctor
    /// </summary>

    private InputBox()
    {
        //
        // Required for Windows Form Designer support
        //
        InitializeComponent();

        //
        // TODO: Add any constructor code after InitializeComponent call
        //
    }


    /// <summary>Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form" />.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _components?.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        ButtonOk = new Button();
        ButtonCancel = new Button();
        TextBoxText = new TextBox();
        LabelPrompt = new Label();
        ErrorProviderText = new ErrorProvider();
        SuspendLayout();
        // 
        // buttonOK
        // 
        ButtonOk.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
        ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        ButtonOk.Location = new System.Drawing.Point(288, 72);
        ButtonOk.Name = "ButtonOk";
        ButtonOk.TabIndex = 2;
        ButtonOk.Text = "OK";
        ButtonOk.Click += new System.EventHandler(ButtonOkClick);
        // 
        // buttonCancel
        // 
        ButtonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
        ButtonCancel.CausesValidation = false;
        ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        ButtonCancel.Location = new System.Drawing.Point(376, 72);
        ButtonCancel.Name = "ButtonCancel";
        ButtonCancel.TabIndex = 3;
        ButtonCancel.Text = "Cancel";
        ButtonCancel.Click += new System.EventHandler(ButtonCancelClick);
        // 
        // textBoxText
        // 
        TextBoxText.Location = new System.Drawing.Point(16, 32);
        TextBoxText.Name = "TextBoxText";
        TextBoxText.Size = new System.Drawing.Size(376 + ButtonCancel.Width - 16, 20);
        TextBoxText.TabIndex = 1;
        TextBoxText.Text = "";
        TextBoxText.Validating += new System.ComponentModel.CancelEventHandler(TextBoxTextValidating);
        TextBoxText.TextChanged += new System.EventHandler(TextBoxTextTextChanged);
        // 
        // labelPrompt
        // 
        LabelPrompt.AutoSize = true;
        LabelPrompt.Location = new System.Drawing.Point(15, 15);
        LabelPrompt.Name = "LabelPrompt";
        LabelPrompt.Size = new System.Drawing.Size(39, 13);
        LabelPrompt.TabIndex = 0;
        LabelPrompt.Text = "prompt";
        // 
        // errorProviderText
        // 
        ErrorProviderText.DataMember = null;
        // 
        // InputBox
        // 
        AcceptButton = ButtonOk;
        AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        CancelButton = ButtonCancel;
        ClientSize = new System.Drawing.Size(464, 104);
        Controls.AddRange(new Control[] {
            LabelPrompt,
            TextBoxText,
            ButtonCancel,
            ButtonOk});
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Name = "InputBox";
        Text = "Title";
        ResumeLayout(false);

    }
    #endregion

    private void ButtonCancelClick(object sender, EventArgs e)
    {
        Validator = null;
        Close();
    }

    private void ButtonOkClick(object sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Displays a prompt in a dialog box, waits for the user to input text or click a button.
    /// </summary>
    /// <param name="prompt">String expression displayed as the message in the dialog box</param>
    /// <param name="title">String expression displayed in the title bar of the dialog box</param>
    /// <param name="defaultResponse">String expression displayed in the text box as the default response</param>
    /// <param name="validator">Delegate used to validate the text</param>
    /// <param name="xpos">Numeric expression that specifies the distance of the left edge of the dialog box from the left edge of the screen.</param>
    /// <param name="ypos">Numeric expression that specifies the distance of the upper edge of the dialog box from the top of the screen</param>
    /// <param name="multiLine"> </param>
    /// <returns>An InputBoxResult object with the Text and the OK property set to true when OK was clicked.</returns>
    public static InputBoxResult Show(string prompt, string title, string defaultResponse, InputBoxValidatingDelegate validator, int xpos, int ypos, bool multiLine)
    {
        using (var form = new InputBox())
        {
            form.LabelPrompt.Text = prompt;
            form.Text = title;
            form.TextBoxText.Text = defaultResponse;
            form.TextBoxText.Multiline = true;
            if (xpos >= 0 && ypos >= 0)
            {
                form.StartPosition = FormStartPosition.Manual;
                form.Left = xpos;
                form.Top = ypos;
                    
            }

            if (multiLine)
            {
                form.Height = 300;
                form.TextBoxText.Height = 200;
                form.ButtonCancel.Top = 240;
                form.ButtonOk.Top = 240;
            }

            form.Validator = validator;

            var result = form.ShowDialog();

            var retval = new InputBoxResult();
            if (result != DialogResult.OK)
            {
                return retval;
            }

            retval.Text = form.TextBoxText.Text;
            retval.Ok = true;
            return retval;
        }
    }

    /// <summary>
    /// Displays a prompt in a dialog box, waits for the user to input text or click a button.
    /// </summary>
    /// <param name="prompt">String expression displayed as the message in the dialog box</param>
    /// <param name="title">String expression displayed in the title bar of the dialog box</param>
    /// <param name="defaultText"> </param>
    /// <param name="validator">Delegate used to validate the text</param>
    /// <returns>An InputBoxResult object with the Text and the OK property set to true when OK was clicked.</returns>
    public static InputBoxResult Show(string prompt, string title, string defaultText, InputBoxValidatingDelegate validator)
    {
        return Show(prompt, title, defaultText, validator, -1, -1, false);
    }

    /// <summary>
    /// Displays a prompt in a dialog box, waits for the user to input text or click a button.
    /// </summary>
    /// <param name="prompt">String expression displayed as the message in the dialog box</param>
    /// <param name="title">String expression displayed in the title bar of the dialog box</param>
    /// <param name="defaultText"> </param>
    /// <param name="validator">Delegate used to validate the text</param>
    /// <param name="multiLine">Text input in multiple rows possible</param>
    /// <returns>An InputBoxResult object with the Text and the OK property set to true when OK was clicked.</returns>
    public static InputBoxResult Show(string prompt, string title, string defaultText, InputBoxValidatingDelegate validator, bool multiLine)
    {
        return Show(prompt, title, defaultText, validator, -1, -1, multiLine);
    }


    /// <summary>
    /// Reset the ErrorProvider
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TextBoxTextTextChanged(object sender, EventArgs e)
    {
        ErrorProviderText.SetError(TextBoxText, "");
    }

    /// <summary>
    /// Validate the Text using the Validator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TextBoxTextValidating(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (Validator == null)
        {
            return;
        }

        var args = new InputBoxValidatingArgs {Text = TextBoxText.Text};
        Validator(this, args);
        if (!args.Cancel)
        {
            return;
        }
        e.Cancel = true;
        ErrorProviderText.SetError(TextBoxText, args.Message);
    }
}

