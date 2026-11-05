using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Helpers;

public static class CopyTaskHelper
{

    public static IList<string> RunAllTasks(IList<CopyTask> tasks)
    {

        var result = new List<string>();

        foreach (var task in tasks)
        {

            // Chart
            try
            {
                File.Copy(task.SourceFileName, task.TargetFileName, true);
            }
            catch (Exception ex)
            {
                result.Add($"Copy {task.SourceFileName} to {task.TargetFileName}: Error {ex.Message}");
            }

            // Include file
            if (string.IsNullOrEmpty(task.IncludeSourceFileName)) continue;

            try
            {
                File.Copy(task.IncludeSourceFileName, task.IncludeTargetFileName, true);
            }
            catch (Exception ex)
            {
                result.Add($"Copy {task.IncludeSourceFileName} to {task.IncludeTargetFileName}: Error {ex.Message}");
            }
        }
        return result;
    }

}