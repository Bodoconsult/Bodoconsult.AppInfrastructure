// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Interfaces;

namespace JobExecuter.Base;

/// <summary>
/// Delegate for email message handling
/// </summary>
public delegate void JobHandlerMailDelegate(string mailTo, string subject, IStructuredText body);

/// <summary>
/// Delegate for saving a text as HTML
/// </summary>
/// <param name="fileName">HTML filename</param>
/// <param name="text">Text to save</param>
/// <param name="title">Title for the HTML file</param>
/// <returns>filename in case of success</returns>
public delegate string SaveAsHtmlDelegate(string fileName, IStructuredText text, string title);

/// <summary>
/// Delegate for saving a text as PDF
/// </summary>
/// <param name="fileName">HTML filename</param>
/// <param name="text">Text to save</param>
/// <param name="title">Title for the HTML file</param>
/// <returns>filename in case of success</returns>
public delegate string SaveAsPdfDelegate(string fileName, IStructuredText text, string title);

/// <summary>
/// Delegate for saving (text) data to a JSON 
/// </summary>
/// <typeparam name="T">type to serialize</typeparam>
/// <param name="fileName">JSON filename</param>
/// <param name="data">data to serialize</param>
/// <returns>filename in case of success</returns>
public delegate string SaveAsJsonDelegate<in T>(string fileName, T data);