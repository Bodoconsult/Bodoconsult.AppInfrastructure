// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.Office.Test.Models;

/// <summary>
/// Contains a result of a time path simulation for 1 period
/// </summary>
public class ResultTimePath
{

    public int Id { get; set; }

    /// <summary>
    /// Current number of the run
    /// </summary>
    public int Run { get; set; }

    /// <summary>
    /// Current period of the result
    /// </summary>
    public int Period { get; set; }


    /// <summary>
    /// Current value of the period's result
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// Cumulative value resulting from the simulation's earlier periods inclduing current value
    /// </summary>
    public double CumulativeValue { get; set; }

}