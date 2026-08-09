// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Collections.Generic;
using System.Data;
using System.IO;
using Bodoconsult.Text.Enums;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Model;

/// <summary>
/// Represents a complete structured text
/// </summary>
public class StructuredText : IStructuredText
{
    /// <summary>
    /// default ctor
    /// </summary>
    public StructuredText()
    {
        TextItems = new List<ITextItem>();
        KeepEmptyItems = false;
    }


    /// <summary>
    /// Keep items and formatted it as empty paragraphs. Default: false
    /// </summary>
    public bool KeepEmptyItems { get; set; }

    /// <summary>
    /// Contains the text items
    /// </summary>
    public IList<ITextItem> TextItems { get; private set; }

    /// <summary>
    /// Clear the structured text
    /// </summary>
    public void Clear()
    {
        TextItems.Clear();
    }

    /// <summary>
    /// Add a header level 1
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className"></param>
    public void AddHeader1(string message, string className = null)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.H1,
            Content = message,
            ClassName = className
        });
    }

    /// <summary>
    /// Add a header level 1
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    /// <param name="data">An object array to format</param>
    public void AddHeader1(string message, string className = null, params object[] data)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.H1,
            Content = string.Format(message, data),
            ClassName = className
        });
    }


    /// <summary>
    /// Add a header level 2
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    public void AddHeader2(string message, string className = null)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.H2,
            Content = message,
            ClassName = className
        });
    }

    /// <summary>
    /// Add a header level 2
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    /// <param name="data">An object array to format</param>
    public void AddHeader2(string message, string className = null, params object[] data)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.H2,
            Content = string.Format(message, data),
            ClassName = className
        });
    }

    /// <summary>
    /// Add a header level 3
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    public void AddHeader3(string message, string className = null)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.H3,
            Content = message,
            ClassName = className
        });
    }

    /// <summary>
    /// Add a header level 3
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    /// <param name="data">An object array to format</param>
    public void AddHeader3(string message, string className = null, params object[] data)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.H3,
            Content = string.Format(message, data),
            ClassName = className
        });
    }

    /// <summary>
    /// Add a header level 4
    /// </summary>
    /// <param name="message">text to append</param>
    /// /// <param name="className">Class name for formatting</param>
    public void AddHeader4(string message, string className = null)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.H4,
            Content = message,
            ClassName = className
        });
    }

    /// <summary>
    /// Add a header level 4
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    /// <param name="data">An object array to format</param>
    public void AddHeader4(string message, string className = null, params object[] data)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.H4,
            Content = string.Format(message, data),
            ClassName = className
        });
    }

    /// <summary>
    /// Add a paragraph
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    public void AddParagraph(string message, string className = null)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }

        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.P,
            Content = message,
            ClassName = className
        });

        //if (message.Contains("??pa??"))
        //{

        //    var rows = message.Replace("??pa??", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("??pe??", "\r").Split(new []{'\r'}, StringSplitOptions.RemoveEmptyEntries);
        //    foreach (var row in rows)
        //    {
        //        TextItems.Add(new TextItem { LogicalType = TextItemType.P, Content = row });
        //    }
        //}
        //else
        //{
        //TextItems.Add(new TextItem { LogicalType = TextItemType.P, Content = message });
        //}

    }

    /// <summary>
    /// Add a paragraph
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    /// <param name="data">An object array to format</param>
    public void AddParagraph(string message, string className = null, params object[] data)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }

        if (data.Length == 1 && data[0] is null)
        {
            return;
        }

        AddParagraph(string.Format(message, data), className);
    }

    /// <summary>
    /// Add a info paragraph
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    public void AddInfo(string message, string className = null)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.P,
            Content = message,
            ClassName = className
        });
    }

    /// <summary>
    /// Add a warning paragraph
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    public void AddWarning(string message, string className = null)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.P,
            Content = message,
            ClassName = className
        });
    }

    /// <summary>
    /// Add a error paragraph
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    public void AddError(string message, string className = null)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.P,
            Content = message,
            ClassName = className
        });
    }


    /// <summary>
    /// Add a info paragraph
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    /// <param name="data">An object array to format</param>
    public void AddInfo(string message, string className = null, params object[] data)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.P,
            Content = string.Format(message, data),
            ClassName = className
        });
    }

    /// <summary>
    /// Add a warning paragraph
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    /// <param name="data">An object array to format</param>
    public void AddWarning(string message, string className = null, params object[] data)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.P,
            Content = string.Format(message, data),
            ClassName = className
        });
    }

    /// <summary>
    /// Add a error paragraph
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    /// <param name="data">An object array to format</param>
    public void AddError(string message, string className = null, params object[] data)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.P,
            Content = string.Format(message, data),
            ClassName = className
        });
    }


    /// <summary>
    /// Add a text with a certain text type
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="textItemType">Type of text</param>
    /// <param name="className">Class name for formatting</param>
    public void AddText(string message, TextItemType textItemType, string className = null)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = textItemType,
            Content = message,
            ClassName = className
        });
    }

    /// <summary>
    /// Add a XML string as paragraph with indented formatting
    /// </summary>
    /// <param name="message">XML string to append</param>
    /// <param name="className">Class name for formatting</param>
    public void AddXml(string message, string className = null)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.Xml,
            Content = message,
            ClassName = className
        });
    }

    /// <summary>
    /// Add a code segment as paragraph with indented formatting
    /// </summary>
    /// <param name="message">XML string to append</param>
    /// <param name="className"></param>
    public void AddCode(string message, string className = null)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message))
        {
            return;
        }
        TextItems.Add(new TextItem
            {
                LogicalType = TextItemType.Code,
                Content = message,
                ClassName = className
            }
        );
    }

    /// <summary>
    /// Add a definition list line as paragraph
    /// </summary>
    /// <param name="leftColumn">Content of the left column</param>
    /// <param name="rightColumn">Content of the right column</param>
    public void AddDefinitionListLine(string leftColumn, string rightColumn)
    {
        if (string.IsNullOrEmpty(leftColumn)) return;

        if (!string.IsNullOrEmpty(rightColumn))
        {
            while (rightColumn.StartsWith("\r\n"))
            {
                rightColumn = rightColumn[2..];
            }

            while (rightColumn.EndsWith("\r\n"))
            {
                rightColumn = rightColumn[..^2];
            }
        }

        TextItems.Add(new DefinitionListTextItem
        {
            LogicalType = TextItemType.DefinitionListLine,
            Content = leftColumn,
            Content2 = rightColumn
        });
    }

    /// <summary>
    /// Add a text with a certain text type
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="textItemType">Type of text</param>
    /// <param name="className">Class name for formatting</param>
    /// <param name="data">An object array to format</param>
    public void AddText(string message, TextItemType textItemType, string className = null, params object[] data)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message)) return;
        TextItems.Add(new TextItem
        {
            LogicalType = textItemType,
            Content = string.Format(message, data),
            ClassName = className
        });
    }

    /// <summary>
    /// Add a list item to the text. Results in HTML in a ul-Tag with li-Tag items
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    public void AddListItem(string message, string className = null)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message)) return;
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.ListItem,
            Content = message,
            ClassName = className
        });
    }

    /// <summary>
    /// Add a list item to the text. Results in HTML in a ul-Tag with li-Tag items
    /// </summary>
    /// <param name="message">text to append</param>
    /// <param name="className">Class name for formatting</param>
    /// <param name="data">An object array to format</param>
    public void AddListItem(string message, string className = null, params object[] data)
    {
        if (!KeepEmptyItems && string.IsNullOrEmpty(message)) return;
        TextItems.Add(new TextItem
        {
            LogicalType = TextItemType.ListItem,
            Content = string.Format(message, data),
            ClassName = className
        });
    }

    /// <summary>
    /// Add a table to the text
    /// </summary>
    /// <param name="title">title for the table</param>
    /// <param name="data">data for the table content</param>
    public void AddTable(string title, DataTable data)
    {
        if (data is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(data.TableName))
        {
            data.TableName = "dataTable";
        }

        using (var ms = new MemoryStream())
        {

            data.WriteXml(ms, XmlWriteMode.WriteSchema);
            ms.Position = 0;

            using (var sr = new StreamReader(ms))
            {
                TextItems.Add(new TableTextItem
                {
                    LogicalType = TextItemType.Table,
                    Content = title,
                    DataTableXml = sr.ReadToEnd()
                });
            }
        }
    }
}