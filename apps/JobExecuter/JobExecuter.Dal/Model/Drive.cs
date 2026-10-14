namespace JobExecuter.Dal.Model;

/// <summary>
/// Drive information for <see cref="DiskSizeJobTask"/>
/// </summary>
public class Drive
{
    /// <summary>
    /// Drive name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Minimum  percentage of free disk space required
    /// </summary>
    public double MinimumPercentageOfFreeSpace { get; set; }

    /// <summary>
    /// IP address (if disk size of a remote computer is to be calculated)
    /// </summary>
    public string IpAddress { get; set; }


}