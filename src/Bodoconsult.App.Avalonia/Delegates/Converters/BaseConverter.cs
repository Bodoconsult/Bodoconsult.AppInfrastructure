// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Avalonia.Markup.Xaml;

namespace Bodoconsult.App.Avalonia.Delegates.Converters;

/// <summary>
/// Base class for converters to make it useable as markup extension
/// Use derived converters like shown the sample below. 
/// </summary>
/// <example>Text={Binding Time, Converter={x:MyConverter}} </example>
public abstract class BaseConverter : MarkupExtension
{
    /// <summary>When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.</summary>
    /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
    /// <returns>The object value to set on the property where the extension is applied.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}