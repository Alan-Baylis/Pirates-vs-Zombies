using UnityEngine;
using System.Collections;

public static class DebugHandler {

    // DEBUG LOGGING:

    // Warning
    public static void printWarning(string sender, string message)
    {
        Debug.LogWarning(format(sender, message));
    }

    // Error
    public static void printError(string sender, string message)
    {
        Debug.LogError(format(sender, message));
    }

    // Normal Print
    public static void print(string sender, string message)
    {
        Debug.Log(format(sender, message));
    }



    // string formatter
    private static string format(string sender, string message)
    {
        return (sender + ":\t\t\t" + message);
    }

}
