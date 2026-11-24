// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Serialize and deserialize derivative data to a storage target
/// </summary>
public interface ISerializer<T>
{
    /// <summary>
    /// deserialize data from the storage data
    /// </summary>
    /// <returns></returns>
    T Deserialize();

    /// <summary>
    /// Serialize data to the storage target
    /// </summary>
    /// <param name="data"></param>
    void Serialize(T data);

}