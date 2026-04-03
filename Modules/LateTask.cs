using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnlockDleks;

// From: https://github.com/Gurge44/EndlessHostRoles/blob/main/Modules/LateTask.cs
internal static class LateTask
{
    /// <summary>
    ///     Creates a task that will be automatically completed after the specified amount of time
    /// </summary>
    /// <param name="action">The delayed task</param>
    /// <param name="time">The time to wait until the task is run</param>
    /// <param name="name">The name of the task</param>
    /// <param name="log">Whether to send log of the creation and completion of the Late Task</param>
    public static void New(Action action, float time, string name = "No Name Task", bool log = true, [CallerFilePath] string path = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
    {
        if (log && name is not "" and not "No Name Task") Main.Logger.LogInfo($"LateTask: \"{name}\" is created (completes in {time:N2})");
        Main.Instance.StartCoroutine(CoLateTask());
        return;

        IEnumerator CoLateTask()
        {
            yield return new WaitForSecondsRealtime(time);

            try
            {
                action();
            
                if (name is not "" and not "No Name Task" && log)
                    Main.Logger.LogInfo($"LateTask: \"{name}\" is finished");
            }
            catch (Exception ex) { Main.Logger.LogError($"LateTask: {ex.GetType()}: {ex.Message}\n  in \"{name}\"\n  (created at {path.Split('\\')[^1].Split('/')[^1]}, by member {member}, at line {line})\n  {ex.StackTrace}".Replace("\r\n", "\n")); }
        }
    }
}