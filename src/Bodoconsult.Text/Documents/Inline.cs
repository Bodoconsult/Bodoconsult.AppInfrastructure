// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Base class for inline elements
/// </summary>
public abstract class Inline : TextElement
{
    /// <summary>
    /// All allowed inlines to be loaded as child inlines
    /// </summary>
    protected List<Type> AllowedInlines = [];

    /// <summary>
    /// All child inlines
    /// </summary>
    protected readonly List<Inline> Inlines = [];

    /// <summary>
    /// All child inlines bound to the element
    /// </summary>
    [DoNotSerialize]
    public List<Inline> ChildInlines => Inlines.ToList();

    /// <summary>
    /// Name of the style to apply to the block
    /// </summary>
    [DoNotSerialize]
    public string StyleName => $"{TagToUse}Style";

    /// <summary>
    /// Add a block element
    /// </summary>
    /// <param name="inline">Inline element to add</param>
    public virtual void AddInline(Inline inline)
    {
        var type = inline.GetType();

        if (!AllowedInlines.Contains(type))
        {
            throw new ArgumentException($"Type {type.Name} not allowed to add for the current element");
        }

        Inlines.Add(inline);
        inline.Parent = this;
    }

}