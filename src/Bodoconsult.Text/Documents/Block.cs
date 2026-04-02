// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Base class for block document elements
/// </summary>
public abstract class Block: TextElement
{
    /// <summary>
    /// Default ctor
    /// </summary>
    protected Block()
    {
        Blocks = new LdmlList<Block>(this);
        Inlines = new LdmlList<Inline>(this);
    }

    /// <summary>
    /// Allowed child block types
    /// </summary>
    protected List<Type> AllowedBlocks = [];

    /// <summary>
    /// All child blocks of the element
    /// </summary>
    protected LdmlList<Block> Blocks;

    /// <summary>
    /// All child blocks of the element
    /// </summary>
    [DoNotSerialize]
    public ReadOnlyLdmlList<Block> ChildBlocks => Blocks.ToList();

    /// <summary>
    /// Allowed child inline types
    /// </summary>
    protected List<Type> AllowedInlines = [];

    /// <summary>
    /// All child inlines of the element
    /// </summary>
    protected LdmlList<Inline> Inlines;

    /// <summary>
    /// All child blocks of the element
    /// </summary>
    [DoNotSerialize]
    public ReadOnlyLdmlList<Inline> ChildInlines => Inlines.ToList();

    /// <summary>
    /// Name of the style to apply to the block
    /// </summary>
    [DoNotSerialize]
    public string StyleName => $"{TagToUse}Style";

    /// <summary>
    /// The name of the tag for the block element
    /// </summary>
    public string TagName { get; set; }

    /// <summary>
    /// Add a block element
    /// </summary>
    /// <param name="block">Block element to add</param>
    public virtual void AddBlock(Block block)
    {
        var type = block.GetType();

        if (!AllowedBlocks.Contains(type))
        {
            throw new ArgumentException($"Type {type.Name} not allowed to add for the current element of type {GetType().Name}");
        }

        if (block.IsSingleton && Blocks.Exists(x=> x.GetType() == type))
        {
            throw new ArgumentException($"Type {type.Name} not allowed to add for the current element of type {GetType().Name} if there is already an existing one (singleton)");
        }

        Blocks.Add(block);
        block.Parent = this;
    }

    /// <summary>
    /// Add an inline element
    /// </summary>
    /// <param name="inline">Inline element to add</param>
    public virtual void AddInline(Inline inline)
    {
        var type = inline.GetType();

        if (!AllowedInlines.Contains(type))
        {
            throw new ArgumentException($"Type {type.Name} not allowed to add for the current element of type {GetType().Name}");
        }

        Inlines.Add(inline);
        inline.Parent = this;
    }

    /// <summary>
    /// Add the current element to a document defined in LDML (Logical document markup language)
    /// </summary>
    /// <param name="document">StringBuilder instance to create the LDML in</param>
    /// <param name="indent">Current indent</param>
    public override void ToLdmlString(StringBuilder document, string indent)
    {
        AddTagWithAttributes(indent, TagToUse, document);

        // Add the blocks now
        foreach (var block in ChildBlocks)
        {
            block.ToLdmlString(document, $"{indent}{Indentation}");
        }

        // Add the inlines now
        foreach (var inline in Inlines)
        {
            inline.ToLdmlString(document, $"{indent}{Indentation}");
        }


        document.AppendLine($"{indent}</{TagToUse}>");
    }

}