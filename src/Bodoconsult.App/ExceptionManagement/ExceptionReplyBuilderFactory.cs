// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.App.Interfaces;

namespace Bodoconsult.App.ExceptionManagement;

/// <summary>
/// Factory to create and return a singleton instance of <see cref="ExceptionReplyBuilder"/>
/// </summary>
public class ExceptionReplyBuilderFactory : IExceptionReplyBuilderFactory
{

    private IExceptionReplyBuilder? _builder;

    /// <summary>
    /// Create or get a <see cref="ExceptionReplyBuilder"/> instance. Implements a singleton internally.
    /// </summary>
    /// <param name="defaultErrorCode">Default error code</param>
    /// <returns>Instance of <see cref="IExceptionReplyBuilder"/></returns>
    public IExceptionReplyBuilder CreateInstance(int defaultErrorCode)
    {
        return _builder ??= new ExceptionReplyBuilder
        {
            DefaultErrorCode = 0
        };
    }
}