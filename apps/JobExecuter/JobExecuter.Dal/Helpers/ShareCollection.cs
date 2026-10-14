// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections;
using System.IO;

namespace JobExecuter.Dal.Helpers;

#region Share Collection

/// <inheritdoc />
/// <summary>
/// A collection of shares
/// </summary>
public class ShareCollection : ReadOnlyCollectionBase
{



    #region Private Data

    /// <summary>The name of the server this collection represents</summary>
    private readonly string _server;

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor - local machine
    /// </summary>
    public ShareCollection()
    {
        _server = string.Empty;
        ShareHelper.EnumerateShares(_server, this);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="server">Server name or IP address</param>
    public ShareCollection(string server)
    {
        _server = server;
        ShareHelper.EnumerateShares(_server, this);
    }

    #endregion

    #region Add

    /// <summary>
    /// Add a share to the collection
    /// </summary>
    /// <param name="share"></param>
    public void Add(Share share)
    {
        InnerList.Add(share);
    }

    /// <summary>
    /// Add a share to the collection by details
    /// </summary>
    /// <param name="netName"></param>
    /// <param name="path"></param>
    /// <param name="shareType"></param>
    /// <param name="remark"></param>
    public void Add(string netName, string path, ShareType shareType, string remark)
    {
        InnerList.Add(new Share(_server, netName, path, shareType, remark));
    }

    #endregion

    #region Properties

    /// <summary>
    /// Returns the name of the server this collection represents
    /// </summary>
    public string Server => _server;

    /// <summary>
    /// Returns the <see cref="Share"/> at the specified index.
    /// </summary>
    public Share this[int index] => (Share)InnerList[index];

    /// <summary>
    /// Returns the <see cref="Share"/> which matches a given local path
    /// </summary>
    /// <param name="path">The path to match</param>
    public Share this[string path]
    {
        get
        {
            if (null == path || 0 == path.Length) return null;

            path = Path.GetFullPath(path);
            if (!ShareHelper.IsValidFilePath(path)) return null;

            Share match = null;

            for (int i = 0; i < InnerList.Count; i++)
            {
                Share s = (Share)InnerList[i];

                if (s.IsFileSystem && s.MatchesPath(path))
                {
                    //Store first match
                    if (null == match)
                        match = s;

                    // If this has a longer path,
                    // and this is a disk share or match is a special share, 
                    // then this is a better match
                    else if (match.Path.Length < s.Path.Length)
                    {
                        if (ShareType.Disk == s.ShareType || ShareType.Disk != match.ShareType)
                            match = s;
                    }
                }
            }

            return match;
        }
    }

    #endregion

    #region Implementation of ICollection

    /// <summary>
    /// Copy this collection to an array
    /// </summary>
    /// <param name="array"></param>
    /// <param name="index"></param>
    public void CopyTo(Share[] array, int index)
    {
        InnerList.CopyTo(array, index);
    }

    #endregion
}

#endregion