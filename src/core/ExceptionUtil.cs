using System;
using System.Diagnostics;
using System.Text.Json;

namespace NotifiableTools;

public static class ExceptionUtil
{

    public static void Print(Exception e)
    {
            Debug.Print($"Exception: {e.Message}");
            Debug.Print($"Type: {e.GetType().Name}");
            Debug.Print(e.StackTrace);
    }

    /// <summary>
    /// try-catch文の省略形。
    /// <code>
    /// try { return func(); } catch { return outlierValue; }
    /// </code>
    /// </summary>
    /// <param name="outlierValue"></param>
    /// <param name="func"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T TryCatch<T>(T outlierValue, Func<T> func)
    {
        try
        {
            return func();
        }
        catch(Exception e)
        {
            ExceptionUtil.Print(e);
            return outlierValue;
        }
    }

    /// <summary>
    /// try-catch文の省略形。
    /// <code>
    /// try { action(); return normalValue; } catch { return outlierValue; }
    /// </code>
    /// </summary>
    /// <param name="normalValue"></param>
    /// <param name="outlierValue"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T TryCatch<T>(T normalValue, T outlierValue, Action action)
    {
        try
        {
            action();
            return normalValue;
        }
        catch(Exception e)
        {
            ExceptionUtil.Print(e);
            return outlierValue;
        }
    }
}
