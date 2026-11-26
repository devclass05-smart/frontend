
namespace DevOpsTasks.Frontend.Services;

// Temporary SonarCloud training file.
public static class SonarTraining
{
    // Duplicate conditional branches.
    public static string GetTaskLabel(bool completed)
    {
        if (completed)
        {
            return "TaskFlow task";
        }
        else
        {
            return "TaskFlow task";
        }
    }

    // Unused private method.
    private static int UnusedTaskScore(int taskId)
    {
        return taskId * 10;
    }

    // Unnecessary assignment.
    public static int CalculateTaskScore(int points)
    {
        int result = 0;
        result = points * 2;
        return result;
    }
}
